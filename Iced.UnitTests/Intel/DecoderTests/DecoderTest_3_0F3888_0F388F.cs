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
	public sealed class DecoderTest_3_0F3888_0F388F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VexpandpsV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VexpandpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VexpandpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D8B 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, true };

				yield return new object[] { "62 F27D28 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27DAB 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, true };

				yield return new object[] { "62 F27D48 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27DCB 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD8B 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FDAB 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FDCB 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VexpandpsV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VexpandpsV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VexpandpsV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VexpandpsV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VexpandpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VexpandpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D8B 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, true };

				yield return new object[] { "62 F27D28 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27DAB 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, true };

				yield return new object[] { "62 F27D48 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27DCB 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD8B 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FDAB 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FDCB 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VexpandpsV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VexpandpsV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VexpandpsV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VexpandpsV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VexpandpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VexpandpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D8B 88 50 01", 7, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, true };

				yield return new object[] { "62 F27D28 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27DAB 88 50 01", 7, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, true };

				yield return new object[] { "62 F27D48 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27DCB 88 50 01", 7, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD8B 88 50 01", 7, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FDAB 88 50 01", 7, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FDCB 88 50 01", 7, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VexpandpsV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VexpandpsV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VexpandpsV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 88 D3", 6, Code.EVEX_Vexpandps_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 88 D3", 6, Code.EVEX_Vexpandps_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 88 D3", 6, Code.EVEX_Vexpandps_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 88 D3", 6, Code.EVEX_Vexpandpd_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 88 D3", 6, Code.EVEX_Vexpandpd_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 88 D3", 6, Code.EVEX_Vexpandpd_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpexpanddV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpexpanddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpexpanddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D8B 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, true };

				yield return new object[] { "62 F27D28 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27DAB 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, true };

				yield return new object[] { "62 F27D48 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27DCB 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, true };

				yield return new object[] { "62 F2FD08 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD8B 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, true };

				yield return new object[] { "62 F2FD28 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FDAB 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, true };

				yield return new object[] { "62 F2FD48 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FDCB 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpexpanddV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpexpanddV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpexpanddV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpexpanddV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpexpanddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpexpanddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D8B 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, true };

				yield return new object[] { "62 F27D28 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27DAB 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, true };

				yield return new object[] { "62 F27D48 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27DCB 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, true };

				yield return new object[] { "62 F2FD08 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD8B 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, true };

				yield return new object[] { "62 F2FD28 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FDAB 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, true };

				yield return new object[] { "62 F2FD48 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FDCB 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpexpanddV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpexpanddV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpexpanddV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpexpanddV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpexpanddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpexpanddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D8B 89 50 01", 7, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, true };

				yield return new object[] { "62 F27D28 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27DAB 89 50 01", 7, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, true };

				yield return new object[] { "62 F27D48 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27DCB 89 50 01", 7, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, true };

				yield return new object[] { "62 F2FD08 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD8B 89 50 01", 7, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, true };

				yield return new object[] { "62 F2FD28 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FDAB 89 50 01", 7, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, true };

				yield return new object[] { "62 F2FD48 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FDCB 89 50 01", 7, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpexpanddV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpexpanddV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpexpanddV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 89 D3", 6, Code.EVEX_Vpexpandd_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 89 D3", 6, Code.EVEX_Vpexpandd_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 89 D3", 6, Code.EVEX_Vpexpandd_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 89 D3", 6, Code.EVEX_Vpexpandq_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 89 D3", 6, Code.EVEX_Vpexpandq_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 89 D3", 6, Code.EVEX_Vpexpandq_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcompresspsV_RegMem_Reg_EVEX_1_Data))]
		void Test16_VcompresspsV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VcompresspsV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D0B 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, false };

				yield return new object[] { "62 F27D28 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27D2B 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, false };

				yield return new object[] { "62 F27D48 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27D4B 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, false };

				yield return new object[] { "62 F2FD08 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD0B 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, false };

				yield return new object[] { "62 F2FD28 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FD2B 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, false };

				yield return new object[] { "62 F2FD48 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FD4B 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcompresspsV_RegMem_Reg_EVEX_2_Data))]
		void Test16_VcompresspsV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VcompresspsV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcompresspsV_RegMem_Reg_EVEX_1_Data))]
		void Test32_VcompresspsV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VcompresspsV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D0B 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, false };

				yield return new object[] { "62 F27D28 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27D2B 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, false };

				yield return new object[] { "62 F27D48 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27D4B 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, false };

				yield return new object[] { "62 F2FD08 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD0B 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, false };

				yield return new object[] { "62 F2FD28 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FD2B 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, false };

				yield return new object[] { "62 F2FD48 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FD4B 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcompresspsV_RegMem_Reg_EVEX_2_Data))]
		void Test32_VcompresspsV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VcompresspsV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcompresspsV_RegMem_Reg_EVEX_1_Data))]
		void Test64_VcompresspsV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VcompresspsV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 4, false };
				yield return new object[] { "62 F27D0B 8A 50 01", 7, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 4, false };

				yield return new object[] { "62 F27D28 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 4, false };
				yield return new object[] { "62 F27D2B 8A 50 01", 7, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 4, false };

				yield return new object[] { "62 F27D48 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 4, false };
				yield return new object[] { "62 F27D4B 8A 50 01", 7, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 4, false };

				yield return new object[] { "62 F2FD08 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 8, false };
				yield return new object[] { "62 F2FD0B 8A 50 01", 7, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 8, false };

				yield return new object[] { "62 F2FD28 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 8, false };
				yield return new object[] { "62 F2FD2B 8A 50 01", 7, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 8, false };

				yield return new object[] { "62 F2FD48 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 8, false };
				yield return new object[] { "62 F2FD4B 8A 50 01", 7, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcompresspsV_RegMem_Reg_EVEX_2_Data))]
		void Test64_VcompresspsV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VcompresspsV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 8A D3", 6, Code.EVEX_Vcompressps_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 8A D3", 6, Code.EVEX_Vcompressps_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 8A D3", 6, Code.EVEX_Vcompressps_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 8A D3", 6, Code.EVEX_Vcompresspd_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 8A D3", 6, Code.EVEX_Vcompresspd_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 8A D3", 6, Code.EVEX_Vcompresspd_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcompressdV_RegMem_Reg_EVEX_1_Data))]
		void Test16_VpcompressdV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpcompressdV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D0B 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, false };

				yield return new object[] { "62 F27D28 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27D2B 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, false };

				yield return new object[] { "62 F27D48 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27D4B 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, false };

				yield return new object[] { "62 F2FD08 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD0B 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, false };

				yield return new object[] { "62 F2FD28 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FD2B 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, false };

				yield return new object[] { "62 F2FD48 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FD4B 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcompressdV_RegMem_Reg_EVEX_2_Data))]
		void Test16_VpcompressdV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpcompressdV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcompressdV_RegMem_Reg_EVEX_1_Data))]
		void Test32_VpcompressdV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpcompressdV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D0B 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, false };

				yield return new object[] { "62 F27D28 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27D2B 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, false };

				yield return new object[] { "62 F27D48 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27D4B 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, false };

				yield return new object[] { "62 F2FD08 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD0B 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, false };

				yield return new object[] { "62 F2FD28 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FD2B 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, false };

				yield return new object[] { "62 F2FD48 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FD4B 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcompressdV_RegMem_Reg_EVEX_2_Data))]
		void Test32_VpcompressdV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpcompressdV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcompressdV_RegMem_Reg_EVEX_1_Data))]
		void Test64_VpcompressdV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpcompressdV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 4, false };
				yield return new object[] { "62 F27D0B 8B 50 01", 7, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 4, false };

				yield return new object[] { "62 F27D28 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 4, false };
				yield return new object[] { "62 F27D2B 8B 50 01", 7, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 4, false };

				yield return new object[] { "62 F27D48 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 4, false };
				yield return new object[] { "62 F27D4B 8B 50 01", 7, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 4, false };

				yield return new object[] { "62 F2FD08 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 8, false };
				yield return new object[] { "62 F2FD0B 8B 50 01", 7, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 8, false };

				yield return new object[] { "62 F2FD28 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 8, false };
				yield return new object[] { "62 F2FD2B 8B 50 01", 7, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 8, false };

				yield return new object[] { "62 F2FD48 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 8, false };
				yield return new object[] { "62 F2FD4B 8B 50 01", 7, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcompressdV_RegMem_Reg_EVEX_2_Data))]
		void Test64_VpcompressdV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpcompressdV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 8B D3", 6, Code.EVEX_Vpcompressd_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 8B D3", 6, Code.EVEX_Vpcompressd_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 8B D3", 6, Code.EVEX_Vpcompressd_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 8B D3", 6, Code.EVEX_Vpcompressq_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 8B D3", 6, Code.EVEX_Vpcompressq_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 8B D3", 6, Code.EVEX_Vpcompressq_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaskmovdV_VX_HX_WX_1_Data))]
		void Test16_VpmaskmovdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaskmovdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 8C 10", 5, Code.VEX_Vpmaskmovd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8C 10", 5, Code.VEX_Vpmaskmovd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8C 10", 5, Code.VEX_Vpmaskmovq_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8C 10", 5, Code.VEX_Vpmaskmovq_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaskmovdV_VX_HX_WX_1_Data))]
		void Test32_VpmaskmovdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaskmovdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 8C 10", 5, Code.VEX_Vpmaskmovd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8C 10", 5, Code.VEX_Vpmaskmovd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8C 10", 5, Code.VEX_Vpmaskmovq_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8C 10", 5, Code.VEX_Vpmaskmovq_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaskmovdV_VX_HX_WX_1_Data))]
		void Test64_VpmaskmovdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaskmovdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 8C 10", 5, Code.VEX_Vpmaskmovd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C46249 8C 10", 5, Code.VEX_Vpmaskmovd_xmm_xmm_m128, Register.XMM10, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C46209 8C 10", 5, Code.VEX_Vpmaskmovd_xmm_xmm_m128, Register.XMM10, Register.XMM14, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8C 10", 5, Code.VEX_Vpmaskmovd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4624D 8C 10", 5, Code.VEX_Vpmaskmovd_ymm_ymm_m256, Register.YMM10, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4620D 8C 10", 5, Code.VEX_Vpmaskmovd_ymm_ymm_m256, Register.YMM10, Register.YMM14, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8C 10", 5, Code.VEX_Vpmaskmovq_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C462C9 8C 10", 5, Code.VEX_Vpmaskmovq_xmm_xmm_m128, Register.XMM10, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C46289 8C 10", 5, Code.VEX_Vpmaskmovq_xmm_xmm_m128, Register.XMM10, Register.XMM14, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8C 10", 5, Code.VEX_Vpmaskmovq_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C462CD 8C 10", 5, Code.VEX_Vpmaskmovq_ymm_ymm_m256, Register.YMM10, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4628D 8C 10", 5, Code.VEX_Vpmaskmovq_ymm_ymm_m256, Register.YMM10, Register.YMM14, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpermbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpermbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpermbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpermbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpermbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpermbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpermbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpermbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpermbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpermbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 8D 50 01", 7, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 8D 50 01", 7, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 8D 50 01", 7, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 8D 50 01", 7, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 8D 50 01", 7, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 8D 50 01", 7, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpermbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpermbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 8D D3", 6, Code.EVEX_Vpermb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 8D D3", 6, Code.EVEX_Vpermb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 8D D3", 6, Code.EVEX_Vpermb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 8D D3", 6, Code.EVEX_Vpermw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 8D D3", 6, Code.EVEX_Vpermw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 8D D3", 6, Code.EVEX_Vpermw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaskmovdV_WX_HX_VX_1_Data))]
		void Test16_VpmaskmovdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpmaskmovdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 8E 10", 5, Code.VEX_Vpmaskmovd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8E 10", 5, Code.VEX_Vpmaskmovd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8E 10", 5, Code.VEX_Vpmaskmovq_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8E 10", 5, Code.VEX_Vpmaskmovq_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaskmovdV_WX_HX_VX_1_Data))]
		void Test32_VpmaskmovdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpmaskmovdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 8E 10", 5, Code.VEX_Vpmaskmovd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8E 10", 5, Code.VEX_Vpmaskmovd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8E 10", 5, Code.VEX_Vpmaskmovq_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8E 10", 5, Code.VEX_Vpmaskmovq_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaskmovdV_WX_HX_VX_1_Data))]
		void Test64_VpmaskmovdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpmaskmovdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 8E 10", 5, Code.VEX_Vpmaskmovd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C46249 8E 10", 5, Code.VEX_Vpmaskmovd_m128_xmm_xmm, Register.XMM6, Register.XMM10, MemorySize.Packed128_Int32 };
				yield return new object[] { "C46209 8E 10", 5, Code.VEX_Vpmaskmovd_m128_xmm_xmm, Register.XMM14, Register.XMM10, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 8E 10", 5, Code.VEX_Vpmaskmovd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4624D 8E 10", 5, Code.VEX_Vpmaskmovd_m256_ymm_ymm, Register.YMM6, Register.YMM10, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4620D 8E 10", 5, Code.VEX_Vpmaskmovd_m256_ymm_ymm, Register.YMM14, Register.YMM10, MemorySize.Packed256_Int32 };

				yield return new object[] { "C4E2C9 8E 10", 5, Code.VEX_Vpmaskmovq_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Int64 };
				yield return new object[] { "C462C9 8E 10", 5, Code.VEX_Vpmaskmovq_m128_xmm_xmm, Register.XMM6, Register.XMM10, MemorySize.Packed128_Int64 };
				yield return new object[] { "C46289 8E 10", 5, Code.VEX_Vpmaskmovq_m128_xmm_xmm, Register.XMM14, Register.XMM10, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E2CD 8E 10", 5, Code.VEX_Vpmaskmovq_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Int64 };
				yield return new object[] { "C462CD 8E 10", 5, Code.VEX_Vpmaskmovq_m256_ymm_ymm, Register.YMM6, Register.YMM10, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4628D 8E 10", 5, Code.VEX_Vpmaskmovq_m256_ymm_ymm, Register.YMM14, Register.YMM10, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbitqmbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpshufbitqmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpshufbitqmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbitqmbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpshufbitqmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpshufbitqmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbitqmbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpshufbitqmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpshufbitqmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbitqmbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpshufbitqmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpshufbitqmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbitqmbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpshufbitqmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpshufbitqmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 8F 50 01", 7, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbitqmbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpshufbitqmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpshufbitqmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D0B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D03 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D2B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D23 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D4B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D43 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 8F D3", 6, Code.EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}
	}
}
