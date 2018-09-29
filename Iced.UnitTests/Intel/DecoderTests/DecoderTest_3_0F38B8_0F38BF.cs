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
	public sealed class DecoderTest_3_0F38B8_0F38BF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vfmadd231psV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B8 10", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B8 10", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B8 10", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B8 10", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231psV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231psV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B8 10", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B8 10", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B8 10", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B8 10", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231psV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231psV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B8 10", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B8 10", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B8 10", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B8 10", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231psV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 B8 D3", 5, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D B8 D3", 5, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 B8 D3", 5, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD B8 D3", 5, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B8 50 01", 7, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B8 50 01", 7, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B8 50 01", 7, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B8 50 01", 7, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B8 50 01", 7, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B8 50 01", 7, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B B8 D3", 6, Code.EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B B8 D3", 6, Code.EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B B8 D3", 6, Code.EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B B8 D3", 6, Code.EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B B8 D3", 6, Code.EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B B8 D3", 6, Code.EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D B9 10", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD B9 10", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 B9 D3", 5, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 B9 D3", 5, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB B9 50 01", 7, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB B9 50 01", 7, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B B9 D3", 6, Code.EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B B9 D3", 6, Code.EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231psV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BA 10", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BA 10", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BA 10", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BA 10", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231psV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231psV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BA 10", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BA 10", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BA 10", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BA 10", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231psV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231psV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BA 10", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BA 10", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BA 10", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BA 10", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231psV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BA D3", 5, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D BA D3", 5, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BA D3", 5, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD BA D3", 5, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BA 50 01", 7, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BA 50 01", 7, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BA 50 01", 7, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BA 50 01", 7, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BA 50 01", 7, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BA 50 01", 7, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BA D3", 6, Code.EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B BA D3", 6, Code.EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BA D3", 6, Code.EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BA D3", 6, Code.EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B BA D3", 6, Code.EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BA D3", 6, Code.EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BB 10", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BB 10", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BB D3", 5, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BB D3", 5, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BB 50 01", 7, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BB 50 01", 7, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BB D3", 6, Code.EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BB D3", 6, Code.EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BC 10", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BC 10", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BC 10", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BC 10", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BC 10", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BC 10", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BC 10", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BC 10", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BC 10", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BC 10", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BC 10", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BC 10", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BC D3", 5, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D BC D3", 5, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BC D3", 5, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD BC D3", 5, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfnmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfnmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BC 50 01", 7, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BC 50 01", 7, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfnmadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BC D3", 6, Code.EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B BC D3", 6, Code.EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BC D3", 6, Code.EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BC D3", 6, Code.EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B BC D3", 6, Code.EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BC D3", 6, Code.EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BD 10", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BD 10", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BD D3", 5, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BD D3", 5, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfnmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfnmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BD 50 01", 7, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BD 50 01", 7, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd231ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfnmadd231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BD D3", 6, Code.EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BD D3", 6, Code.EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BE 10", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BE 10", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BE 10", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BE 10", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BE 10", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BE 10", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BE 10", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BE 10", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BE 10", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D BE 10", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 BE 10", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD BE 10", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BE D3", 5, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D BE D3", 5, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BE D3", 5, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD BE D3", 5, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfnmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfnmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 BE 50 01", 7, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 BE 50 01", 7, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfnmsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BE D3", 6, Code.EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B BE D3", 6, Code.EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BE D3", 6, Code.EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BE D3", 6, Code.EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B BE D3", 6, Code.EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BE D3", 6, Code.EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub231ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D BF 10", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD BF 10", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub231ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 BF D3", 5, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 BF D3", 5, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfnmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfnmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub231ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub231ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB BF 50 01", 7, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB BF 50 01", 7, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub231ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub231ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfnmsub231ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B BF D3", 6, Code.EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B BF D3", 6, Code.EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}
	}
}
