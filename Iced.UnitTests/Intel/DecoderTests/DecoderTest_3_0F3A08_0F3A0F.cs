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
	public sealed class DecoderTest_3_0F3A08_0F3A0F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VroundpsV_VX_WX_Ib_1_Data))]
		void Test16_VroundpsV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VroundpsV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A08 08 A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E379 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3F9 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E3FD 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VroundpsV_VX_WX_Ib_2_Data))]
		void Test16_VroundpsV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VroundpsV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A08 CD A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 08 D3 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 08 D3 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VroundpsV_VX_WX_Ib_1_Data))]
		void Test32_VroundpsV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VroundpsV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A08 08 A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E379 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3F9 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E3FD 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VroundpsV_VX_WX_Ib_2_Data))]
		void Test32_VroundpsV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VroundpsV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A08 CD A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 08 D3 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 08 D3 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VroundpsV_VX_WX_Ib_1_Data))]
		void Test64_VroundpsV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VroundpsV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A08 08 A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E379 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3F9 08 10 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E3FD 08 10 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VroundpsV_VX_WX_Ib_2_Data))]
		void Test64_VroundpsV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VroundpsV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A08 CD A5", 6, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A08 CD A5", 7, Code.Roundps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A08 CD A5", 7, Code.Roundps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A08 CD A5", 7, Code.Roundps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 08 D3 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 08 D3 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 08 D3 A5", 6, Code.VEX_Vroundps_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E37D 08 D3 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C4637D 08 D3 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C37D 08 D3 A5", 6, Code.VEX_Vroundps_ymm_ymmm256_imm8, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 08 50 01 A5", 8, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VrndscalepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D0B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37D8B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D2B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37DAB 08 D3 A5", 7, Code.EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 337D1B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C37D3B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 08 D3 A5", 7, Code.EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VroundpdV_VX_WX_Ib_1_Data))]
		void Test16_VroundpdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VroundpdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A09 08 A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E379 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3F9 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E3FD 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VroundpdV_VX_WX_Ib_2_Data))]
		void Test16_VroundpdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VroundpdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A09 CD A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 09 D3 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 09 D3 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VroundpdV_VX_WX_Ib_1_Data))]
		void Test32_VroundpdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VroundpdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A09 08 A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E379 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3F9 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E3FD 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VroundpdV_VX_WX_Ib_2_Data))]
		void Test32_VroundpdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VroundpdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A09 CD A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 09 D3 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 09 D3 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VroundpdV_VX_WX_Ib_1_Data))]
		void Test64_VroundpdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VroundpdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A09 08 A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E379 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3F9 09 10 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E3FD 09 10 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VroundpdV_VX_WX_Ib_2_Data))]
		void Test64_VroundpdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VroundpdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A09 CD A5", 6, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A09 CD A5", 7, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A09 CD A5", 7, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A09 CD A5", 7, Code.Roundpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 09 D3 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 09 D3 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 09 D3 A5", 6, Code.VEX_Vroundpd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E37D 09 D3 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C4637D 09 D3 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C37D 09 D3 A5", 6, Code.VEX_Vroundpd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F3FD08 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F3FD0B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F3FD08 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F3FD0B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F3FD08 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 09 50 01 A5", 8, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VrndscalepdV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F3FD0B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD0B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FD8B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD2B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FDAB 09 D3 A5", 7, Code.EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 33FD1B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C3FD3B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 09 D3 A5", 7, Code.EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Roundss_xmm_xmmm32_imm8_1_Data))]
		void Test16_Roundss_xmm_xmmm32_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Roundss_xmm_xmmm32_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0A 08 A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Roundss_xmm_xmmm32_imm8_2_Data))]
		void Test16_Roundss_xmm_xmmm32_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Roundss_xmm_xmmm32_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0A CD A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Roundss_xmm_xmmm32_imm8_1_Data))]
		void Test32_Roundss_xmm_xmmm32_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Roundss_xmm_xmmm32_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0A 08 A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Roundss_xmm_xmmm32_imm8_2_Data))]
		void Test32_Roundss_xmm_xmmm32_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Roundss_xmm_xmmm32_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0A CD A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Roundss_xmm_xmmm32_imm8_1_Data))]
		void Test64_Roundss_xmm_xmmm32_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Roundss_xmm_xmmm32_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0A 08 A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Roundss_xmm_xmmm32_imm8_2_Data))]
		void Test64_Roundss_xmm_xmmm32_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Roundss_xmm_xmmm32_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0A CD A5", 6, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0A CD A5", 7, Code.Roundss_xmm_xmmm32_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A0A CD A5", 7, Code.Roundss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0A CD A5", 7, Code.Roundss_xmm_xmmm32_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vroundss_VX_HX_WX_Ib_1_Data))]
		void Test16_Vroundss_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vroundss_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E34D 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vroundss_VX_HX_WX_Ib_2_Data))]
		void Test16_Vroundss_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vroundss_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vroundss_VX_HX_WX_Ib_1_Data))]
		void Test32_Vroundss_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vroundss_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E34D 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vroundss_VX_HX_WX_Ib_2_Data))]
		void Test32_Vroundss_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vroundss_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vroundss_VX_HX_WX_Ib_1_Data))]
		void Test64_Vroundss_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vroundss_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E34D 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 0A 10 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vroundss_VX_HX_WX_Ib_2_Data))]
		void Test64_Vroundss_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vroundss_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E309 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 0A D3 A5", 6, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vrndscaless_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vrndscaless_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vrndscaless_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vrndscaless_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vrndscaless_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vrndscaless_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 0A 50 01 A5", 8, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vrndscaless_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vrndscaless_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E30D1B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 134D03 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D08 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 0A D3 A5", 7, Code.EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Roundsd_xmm_xmmm64_imm8_1_Data))]
		void Test16_Roundsd_xmm_xmmm64_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Roundsd_xmm_xmmm64_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0B 08 A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Roundsd_xmm_xmmm64_imm8_2_Data))]
		void Test16_Roundsd_xmm_xmmm64_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Roundsd_xmm_xmmm64_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0B CD A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Roundsd_xmm_xmmm64_imm8_1_Data))]
		void Test32_Roundsd_xmm_xmmm64_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Roundsd_xmm_xmmm64_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0B 08 A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Roundsd_xmm_xmmm64_imm8_2_Data))]
		void Test32_Roundsd_xmm_xmmm64_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Roundsd_xmm_xmmm64_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0B CD A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Roundsd_xmm_xmmm64_imm8_1_Data))]
		void Test64_Roundsd_xmm_xmmm64_imm8_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Roundsd_xmm_xmmm64_imm8_1_Data {
			get {
				yield return new object[] { "66 0F3A0B 08 A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Roundsd_xmm_xmmm64_imm8_2_Data))]
		void Test64_Roundsd_xmm_xmmm64_imm8_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Roundsd_xmm_xmmm64_imm8_2_Data {
			get {
				yield return new object[] { "66 0F3A0B CD A5", 6, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0B CD A5", 7, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A0B CD A5", 7, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0B CD A5", 7, Code.Roundsd_xmm_xmmm64_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vroundsd_VX_HX_WX_Ib_1_Data))]
		void Test16_Vroundsd_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vroundsd_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E34D 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E3C9 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vroundsd_VX_HX_WX_Ib_2_Data))]
		void Test16_Vroundsd_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vroundsd_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vroundsd_VX_HX_WX_Ib_1_Data))]
		void Test32_Vroundsd_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vroundsd_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E34D 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E3C9 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vroundsd_VX_HX_WX_Ib_2_Data))]
		void Test32_Vroundsd_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vroundsd_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vroundsd_VX_HX_WX_Ib_1_Data))]
		void Test64_Vroundsd_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vroundsd_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E34D 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E3C9 0B 10 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vroundsd_VX_HX_WX_Ib_2_Data))]
		void Test64_Vroundsd_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vroundsd_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E309 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 0B D3 A5", 6, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 0B 50 01 A5", 8, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vrndscalesd_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D1B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 13CD03 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD08 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 0B D3 A5", 7, Code.EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BlendV_VX_WX_Ib_1_Data))]
		void Test16_BlendV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_BlendV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0C 08 A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A0D 08 A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BlendV_VX_WX_Ib_2_Data))]
		void Test16_BlendV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_BlendV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0C CD A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F3A0D CD A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BlendV_VX_WX_Ib_1_Data))]
		void Test32_BlendV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_BlendV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0C 08 A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A0D 08 A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BlendV_VX_WX_Ib_2_Data))]
		void Test32_BlendV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_BlendV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0C CD A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F3A0D CD A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BlendV_VX_WX_Ib_1_Data))]
		void Test64_BlendV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_BlendV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0C 08 A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A0D 08 A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BlendV_VX_WX_Ib_2_Data))]
		void Test64_BlendV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_BlendV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0C CD A5", 6, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0C CD 5A", 7, Code.Blendps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A0C CD A5", 7, Code.Blendps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0C CD 5A", 7, Code.Blendps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };

				yield return new object[] { "66 0F3A0D CD A5", 6, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0D CD 5A", 7, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A0D CD A5", 7, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0D CD 5A", 7, Code.Blendpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendV_VX_HX_WX_Ib_1_Data))]
		void Test16_VblendV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VblendV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E34D 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
				yield return new object[] { "C4E3C9 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3CD 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendV_VX_HX_WX_Ib_2_Data))]
		void Test16_VblendV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VblendV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };

				yield return new object[] { "C4E349 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendV_VX_HX_WX_Ib_1_Data))]
		void Test32_VblendV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VblendV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E34D 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
				yield return new object[] { "C4E3C9 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3CD 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendV_VX_HX_WX_Ib_2_Data))]
		void Test32_VblendV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VblendV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };

				yield return new object[] { "C4E349 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendV_VX_HX_WX_Ib_1_Data))]
		void Test64_VblendV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VblendV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 0C 10 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 0C 10 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E34D 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
				yield return new object[] { "C4E3C9 0D 10 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3CD 0D 10 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendV_VX_HX_WX_Ib_2_Data))]
		void Test64_VblendV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VblendV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C46349 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C4E309 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0x5A };
				yield return new object[] { "C4C349 0C D3 A5", 6, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 0C D3 5A", 6, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0x5A };

				yield return new object[] { "C4E349 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C46349 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C4E309 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0x5A };
				yield return new object[] { "C4C349 0D D3 A5", 6, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 0D D3 5A", 6, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PblendwV_VX_WX_Ib_1_Data))]
		void Test16_PblendwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PblendwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0E 08 A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PblendwV_VX_WX_Ib_2_Data))]
		void Test16_PblendwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PblendwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0E CD A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PblendwV_VX_WX_Ib_1_Data))]
		void Test32_PblendwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PblendwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0E 08 A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PblendwV_VX_WX_Ib_2_Data))]
		void Test32_PblendwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PblendwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0E CD A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PblendwV_VX_WX_Ib_1_Data))]
		void Test64_PblendwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PblendwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A0E 08 A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PblendwV_VX_WX_Ib_2_Data))]
		void Test64_PblendwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PblendwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A0E CD A5", 6, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0E CD 5A", 7, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A0E CD A5", 7, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0E CD 5A", 7, Code.Pblendw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendwV_VX_HX_WX_Ib_1_Data))]
		void Test16_VpblendwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpblendwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E34D 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
				yield return new object[] { "C4E3C9 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E3CD 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendwV_VX_HX_WX_Ib_2_Data))]
		void Test16_VpblendwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpblendwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendwV_VX_HX_WX_Ib_1_Data))]
		void Test32_VpblendwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpblendwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E34D 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
				yield return new object[] { "C4E3C9 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E3CD 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendwV_VX_HX_WX_Ib_2_Data))]
		void Test32_VpblendwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpblendwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendwV_VX_HX_WX_Ib_1_Data))]
		void Test64_VpblendwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpblendwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E34D 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
				yield return new object[] { "C4E3C9 0E 10 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E3CD 0E 10 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendwV_VX_HX_WX_Ib_2_Data))]
		void Test64_VpblendwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpblendwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C46349 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C4E309 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0x5A };
				yield return new object[] { "C4C349 0E D3 A5", 6, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 0E D3 5A", 6, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PalignrV_VX_WX_Ib_1_Data))]
		void Test16_PalignrV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PalignrV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3A0F 08 A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int8, 0xA5 };

				yield return new object[] { "66 0F3A0F 08 A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PalignrV_VX_WX_Ib_2_Data))]
		void Test16_PalignrV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PalignrV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3A0F CD A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F3A0F CD A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PalignrV_VX_WX_Ib_1_Data))]
		void Test32_PalignrV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PalignrV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3A0F 08 A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int8, 0xA5 };

				yield return new object[] { "66 0F3A0F 08 A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PalignrV_VX_WX_Ib_2_Data))]
		void Test32_PalignrV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PalignrV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3A0F CD A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F3A0F CD A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PalignrV_VX_WX_Ib_1_Data))]
		void Test64_PalignrV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PalignrV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3A0F 08 A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int8, 0xA5 };

				yield return new object[] { "66 0F3A0F 08 A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PalignrV_VX_WX_Ib_2_Data))]
		void Test64_PalignrV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PalignrV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3A0F CD A5", 5, Code.Palignr_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F3A0F CD A5", 6, Code.Palignr_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F3A0F CD A5", 6, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A0F CD A5", 7, Code.Palignr_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A0F CD A5", 7, Code.Palignr_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A0F CD A5", 7, Code.Palignr_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpalignrV_VX_HX_WX_Ib_1_Data))]
		void Test16_VpalignrV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpalignrV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E34D 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
				yield return new object[] { "C4E3C9 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E3CD 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpalignrV_VX_HX_WX_Ib_2_Data))]
		void Test16_VpalignrV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpalignrV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpalignrV_VX_HX_WX_Ib_1_Data))]
		void Test32_VpalignrV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpalignrV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E34D 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
				yield return new object[] { "C4E3C9 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E3CD 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpalignrV_VX_HX_WX_Ib_2_Data))]
		void Test32_VpalignrV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpalignrV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpalignrV_VX_HX_WX_Ib_1_Data))]
		void Test64_VpalignrV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpalignrV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E34D 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
				yield return new object[] { "C4E3C9 0F 10 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "C4E3CD 0F 10 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpalignrV_VX_HX_WX_Ib_2_Data))]
		void Test64_VpalignrV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpalignrV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C46349 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E309 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C349 0F D3 A5", 6, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 0F D3 A5", 6, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpalignrV_VX_k1_HX_WX_Ib_1_Data))]
		void Test16_VpalignrV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpalignrV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpalignrV_VX_k1_HX_WX_Ib_2_Data))]
		void Test16_VpalignrV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpalignrV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D8B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpalignrV_VX_k1_HX_WX_Ib_1_Data))]
		void Test32_VpalignrV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpalignrV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpalignrV_VX_k1_HX_WX_Ib_2_Data))]
		void Test32_VpalignrV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpalignrV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D8B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpalignrV_VX_k1_HX_WX_Ib_1_Data))]
		void Test64_VpalignrV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpalignrV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F 50 01 A5", 8, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpalignrV_VX_k1_HX_WX_Ib_2_Data))]
		void Test64_VpalignrV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpalignrV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D8B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D03 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 0F D3 A5", 7, Code.EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DAB 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D23 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 0F D3 A5", 7, Code.EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 0F D3 A5", 7, Code.EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}
	}
}
