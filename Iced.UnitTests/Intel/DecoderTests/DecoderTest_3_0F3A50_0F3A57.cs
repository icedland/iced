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
	public sealed class DecoderTest_3_0F3A50_0F3A57 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VrangeV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VrangeV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrangeV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrangeV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VrangeV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrangeV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrangeV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VrangeV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrangeV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrangeV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VrangeV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrangeV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrangeV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VrangeV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrangeV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 50 50 01 A5", 8, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 50 50 01 A5", 8, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 50 50 01 A5", 8, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 50 50 01 A5", 8, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 50 50 01 A5", 8, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 50 50 01 A5", 8, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 51 50 01 A5", 8, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 51 50 01 A5", 8, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrangeV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VrangeV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrangeV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D0B 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D03 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 50 D3 A5", 7, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D2B 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D23 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 50 D3 A5", 7, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D4B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D43 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 50 D3 A5", 7, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D0B 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD03 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 50 D3 A5", 7, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D2B 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD23 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 50 D3 A5", 7, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D4B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD43 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 50 D3 A5", 7, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D0B 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D03 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 51 D3 A5", 7, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D0B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD03 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 51 D3 A5", 7, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VfixupimmV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VfixupimmV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VfixupimmV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VfixupimmV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VfixupimmV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VfixupimmV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 54 50 01 A5", 8, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 54 50 01 A5", 8, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };

				yield return new object[] { "62 F34D0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34DAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34DEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3CD0B 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDCB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CDEB 55 50 01 A5", 8, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VfixupimmV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VfixupimmV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D03 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F34D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D23 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D1B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D43 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D3B 54 D3 A5", 7, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D0B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD03 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDDB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D2B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD23 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3CDCB 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D4B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD43 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 54 D3 A5", 7, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F34D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 134D03 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DDB 55 D3 A5", 7, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D0B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 13CD03 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD7B 55 D3 A5", 7, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 50 01 A5", 8, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 56 50 01 A5", 8, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 50 01 A5", 8, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 56 50 01 A5", 8, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VreducepsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D0B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37D8B 56 D3 A5", 7, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D2B 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37DAB 56 D3 A5", 7, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 337D1B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C37D3B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 56 D3 A5", 7, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD0B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FD8B 56 D3 A5", 7, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD2B 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FDAB 56 D3 A5", 7, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 33FD1B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C3FD3B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 56 D3 A5", 7, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vreducess_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vreducess_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vreducess_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vreducess_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vreducess_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vreducess_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vreducess_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vreducess_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vreducess_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vreducess_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vreducess_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vreducess_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vreducess_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vreducess_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vreducess_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 57 50 01 A5", 8, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 57 50 01 A5", 8, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vreducess_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vreducess_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vreducess_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E30D1B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 134D03 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D08 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 57 D3 A5", 7, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D1B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 13CD03 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD08 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 57 D3 A5", 7, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}
	}
}
