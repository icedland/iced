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
	public sealed class DecoderTest_3_0F3860_0F3867 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpexpandV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpexpandV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpexpandV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D8B 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, true };

				yield return new object[] { "62 F27D28 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27DAB 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, true };

				yield return new object[] { "62 F27D48 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27DCB 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, true };

				yield return new object[] { "62 F2FD08 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD8B 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, true };

				yield return new object[] { "62 F2FD28 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FDAB 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, true };

				yield return new object[] { "62 F2FD48 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FDCB 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpexpandV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpexpandV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpexpandV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpexpandV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpexpandV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpexpandV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D8B 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, true };

				yield return new object[] { "62 F27D28 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27DAB 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, true };

				yield return new object[] { "62 F27D48 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27DCB 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, true };

				yield return new object[] { "62 F2FD08 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD8B 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, true };

				yield return new object[] { "62 F2FD28 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FDAB 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, true };

				yield return new object[] { "62 F2FD48 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FDCB 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpexpandV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpexpandV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpexpandV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpexpandV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpexpandV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpexpandV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D8B 62 50 01", 7, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, true };

				yield return new object[] { "62 F27D28 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27DAB 62 50 01", 7, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, true };

				yield return new object[] { "62 F27D48 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27DCB 62 50 01", 7, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, true };

				yield return new object[] { "62 F2FD08 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD8B 62 50 01", 7, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, true };

				yield return new object[] { "62 F2FD28 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FDAB 62 50 01", 7, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, true };

				yield return new object[] { "62 F2FD48 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FDCB 62 50 01", 7, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpexpandV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpexpandV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpexpandV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 62 D3", 6, Code.EVEX_Vpexpandb_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 62 D3", 6, Code.EVEX_Vpexpandb_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 62 D3", 6, Code.EVEX_Vpexpandb_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 62 D3", 6, Code.EVEX_Vpexpandw_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 62 D3", 6, Code.EVEX_Vpexpandw_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 62 D3", 6, Code.EVEX_Vpexpandw_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcompressV_RegMem_Reg_EVEX_1_Data))]
		void Test16_VpcompressV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpcompressV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D0B 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, false };

				yield return new object[] { "62 F27D28 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27D2B 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, false };

				yield return new object[] { "62 F27D48 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27D4B 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, false };

				yield return new object[] { "62 F2FD08 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD0B 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, false };

				yield return new object[] { "62 F2FD28 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FD2B 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, false };

				yield return new object[] { "62 F2FD48 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FD4B 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcompressV_RegMem_Reg_EVEX_2_Data))]
		void Test16_VpcompressV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpcompressV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcompressV_RegMem_Reg_EVEX_1_Data))]
		void Test32_VpcompressV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpcompressV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D0B 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, false };

				yield return new object[] { "62 F27D28 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27D2B 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, false };

				yield return new object[] { "62 F27D48 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27D4B 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, false };

				yield return new object[] { "62 F2FD08 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD0B 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, false };

				yield return new object[] { "62 F2FD28 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FD2B 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, false };

				yield return new object[] { "62 F2FD48 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FD4B 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcompressV_RegMem_Reg_EVEX_2_Data))]
		void Test32_VpcompressV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpcompressV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcompressV_RegMem_Reg_EVEX_1_Data))]
		void Test64_VpcompressV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpcompressV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt8, 1, false };
				yield return new object[] { "62 F27D0B 63 50 01", 7, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt8, 1, false };

				yield return new object[] { "62 F27D28 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt8, 1, false };
				yield return new object[] { "62 F27D2B 63 50 01", 7, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt8, 1, false };

				yield return new object[] { "62 F27D48 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt8, 1, false };
				yield return new object[] { "62 F27D4B 63 50 01", 7, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt8, 1, false };

				yield return new object[] { "62 F2FD08 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_UInt16, 2, false };
				yield return new object[] { "62 F2FD0B 63 50 01", 7, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_UInt16, 2, false };

				yield return new object[] { "62 F2FD28 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_UInt16, 2, false };
				yield return new object[] { "62 F2FD2B 63 50 01", 7, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_UInt16, 2, false };

				yield return new object[] { "62 F2FD48 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_UInt16, 2, false };
				yield return new object[] { "62 F2FD4B 63 50 01", 7, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt16, 2, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcompressV_RegMem_Reg_EVEX_2_Data))]
		void Test64_VpcompressV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpcompressV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 63 D3", 6, Code.EVEX_Vpcompressb_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 63 D3", 6, Code.EVEX_Vpcompressb_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 63 D3", 6, Code.EVEX_Vpcompressb_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 63 D3", 6, Code.EVEX_Vpcompressw_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 63 D3", 6, Code.EVEX_Vpcompressw_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 63 D3", 6, Code.EVEX_Vpcompressw_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendmdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpblendmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpblendmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendmdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpblendmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpblendmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendmdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpblendmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpblendmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendmdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpblendmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpblendmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendmdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpblendmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpblendmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 64 50 01", 7, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 64 50 01", 7, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 64 50 01", 7, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 64 50 01", 7, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 64 50 01", 7, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 64 50 01", 7, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendmdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpblendmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpblendmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 64 D3", 6, Code.EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 64 D3", 6, Code.EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 64 D3", 6, Code.EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 64 D3", 6, Code.EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 64 D3", 6, Code.EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 64 D3", 6, Code.EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendmpsV_VX_k1_HX_WX_1_Data))]
		void Test16_VblendmpsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VblendmpsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendmpsV_VX_k1_HX_WX_2_Data))]
		void Test16_VblendmpsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VblendmpsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendmpsV_VX_k1_HX_WX_1_Data))]
		void Test32_VblendmpsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VblendmpsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendmpsV_VX_k1_HX_WX_2_Data))]
		void Test32_VblendmpsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VblendmpsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendmpsV_VX_k1_HX_WX_1_Data))]
		void Test64_VblendmpsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VblendmpsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 65 50 01", 7, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 65 50 01", 7, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 65 50 01", 7, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 65 50 01", 7, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 65 50 01", 7, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 65 50 01", 7, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendmpsV_VX_k1_HX_WX_2_Data))]
		void Test64_VblendmpsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VblendmpsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 65 D3", 6, Code.EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 65 D3", 6, Code.EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 65 D3", 6, Code.EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 65 D3", 6, Code.EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 65 D3", 6, Code.EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 65 D3", 6, Code.EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendmbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpblendmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpblendmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblendmbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpblendmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpblendmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendmbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpblendmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpblendmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblendmbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpblendmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpblendmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendmbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpblendmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpblendmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 66 50 01", 7, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 66 50 01", 7, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 66 50 01", 7, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 66 50 01", 7, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 66 50 01", 7, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 66 50 01", 7, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblendmbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpblendmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpblendmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 66 D3", 6, Code.EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 66 D3", 6, Code.EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 66 D3", 6, Code.EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 66 D3", 6, Code.EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 66 D3", 6, Code.EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 66 D3", 6, Code.EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}
	}
}
