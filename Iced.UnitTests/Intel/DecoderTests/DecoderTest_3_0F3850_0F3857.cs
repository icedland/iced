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
	public sealed class DecoderTest_3_0F3850_0F3857 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VX_k1_HX_WX_1_Data))]
		void Test16_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };

				yield return new object[] { "62 F24D0B 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VX_k1_HX_WX_2_Data))]
		void Test16_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VX_k1_HX_WX_1_Data))]
		void Test32_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };

				yield return new object[] { "62 F24D0B 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VX_k1_HX_WX_2_Data))]
		void Test32_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VX_k1_HX_WX_1_Data))]
		void Test64_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 50 50 01", 7, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 50 50 01", 7, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 50 50 01", 7, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D9D 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 51 50 01", 7, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DBD 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 51 50 01", 7, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DDD 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 51 50 01", 7, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F24D0B 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 52 50 01", 7, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 52 50 01", 7, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 52 50 01", 7, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };

				yield return new object[] { "62 F24D0B 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24D9D 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 53 50 01", 7, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F24D2B 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F24DBD 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 53 50 01", 7, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F24D4B 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F24DDD 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 53 50 01", 7, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VX_k1_HX_WX_2_Data))]
		void Test64_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 50 D3", 6, Code.EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 50 D3", 6, Code.EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 50 D3", 6, Code.EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 51 D3", 6, Code.EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 51 D3", 6, Code.EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 51 D3", 6, Code.EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 52 D3", 6, Code.EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 52 D3", 6, Code.EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 52 D3", 6, Code.EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F24D8B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 53 D3", 6, Code.EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 53 D3", 6, Code.EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 53 D3", 6, Code.EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test16_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test32_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test64_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 E20FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 724F40 52 50 01", 7, Code.EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 E20FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 724F40 53 50 01", 7, Code.EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpopcntV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpopcntV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpopcntV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27D8B 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };

				yield return new object[] { "62 F27D28 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DAB 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };

				yield return new object[] { "62 F27D48 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F27DCB 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 64, true };

				yield return new object[] { "62 F2FD08 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2FD8B 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };

				yield return new object[] { "62 F2FD28 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2FDAB 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };

				yield return new object[] { "62 F2FD48 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2FDCB 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpopcntV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpopcntV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpopcntV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpopcntV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpopcntV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpopcntV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27D8B 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };

				yield return new object[] { "62 F27D28 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DAB 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };

				yield return new object[] { "62 F27D48 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F27DCB 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 64, true };

				yield return new object[] { "62 F2FD08 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2FD8B 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };

				yield return new object[] { "62 F2FD28 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2FDAB 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };

				yield return new object[] { "62 F2FD48 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2FDCB 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpopcntV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpopcntV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpopcntV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpopcntV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpopcntV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpopcntV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27D8B 54 50 01", 7, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };

				yield return new object[] { "62 F27D28 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DAB 54 50 01", 7, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };

				yield return new object[] { "62 F27D48 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F27DCB 54 50 01", 7, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 64, true };

				yield return new object[] { "62 F2FD08 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2FD8B 54 50 01", 7, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };

				yield return new object[] { "62 F2FD28 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2FDAB 54 50 01", 7, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };

				yield return new object[] { "62 F2FD48 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2FDCB 54 50 01", 7, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpopcntV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpopcntV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpopcntV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 54 D3", 6, Code.EVEX_Vpopcntb_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 54 D3", 6, Code.EVEX_Vpopcntb_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 54 D3", 6, Code.EVEX_Vpopcntb_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 54 D3", 6, Code.EVEX_Vpopcntw_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 54 D3", 6, Code.EVEX_Vpopcntw_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 54 D3", 6, Code.EVEX_Vpopcntw_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpopcntV_VX_k1_WX_1_Data))]
		void Test16_VpopcntV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpopcntV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpopcntV_VX_k1_WX_2_Data))]
		void Test16_VpopcntV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpopcntV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpopcntV_VX_k1_WX_1_Data))]
		void Test32_VpopcntV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpopcntV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpopcntV_VX_k1_WX_2_Data))]
		void Test32_VpopcntV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpopcntV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpopcntV_VX_k1_WX_1_Data))]
		void Test64_VpopcntV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpopcntV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 55 50 01", 7, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 55 50 01", 7, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 55 50 01", 7, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 55 50 01", 7, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 55 50 01", 7, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 55 50 01", 7, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpopcntV_VX_k1_WX_2_Data))]
		void Test64_VpopcntV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpopcntV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B 55 D3", 6, Code.EVEX_Vpopcntd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B 55 D3", 6, Code.EVEX_Vpopcntd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B 55 D3", 6, Code.EVEX_Vpopcntd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B 55 D3", 6, Code.EVEX_Vpopcntq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B 55 D3", 6, Code.EVEX_Vpopcntq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B 55 D3", 6, Code.EVEX_Vpopcntq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
