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
	public sealed class DecoderTest_3_0F38C8_0F38CF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VphminposuwV_Reg_RegMem_1_Data))]
		void Test16_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F38C8 08", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38C9 08", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CA 08", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CB 08", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CC 08", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CD 08", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphminposuwV_Reg_RegMem_2_Data))]
		void Test16_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F38C8 CD", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38C9 CD", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CA CD", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CB CD", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CC CD", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CD CD", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphminposuwV_Reg_RegMem_1_Data))]
		void Test32_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F38C8 08", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38C9 08", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CA 08", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CB 08", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CC 08", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CD 08", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphminposuwV_Reg_RegMem_2_Data))]
		void Test32_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F38C8 CD", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38C9 CD", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CA CD", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CB CD", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CC CD", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F38CD CD", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphminposuwV_Reg_RegMem_1_Data))]
		void Test64_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F38C8 08", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38C9 08", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CA 08", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CB 08", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CC 08", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };

				yield return new object[] { "0F38CD 08", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphminposuwV_Reg_RegMem_2_Data))]
		void Test64_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F38C8 CD", 4, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38C8 CD", 5, Code.Sha1nexte_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38C8 CD", 5, Code.Sha1nexte_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38C8 CD", 5, Code.Sha1nexte_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F38C9 CD", 4, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38C9 CD", 5, Code.Sha1msg1_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38C9 CD", 5, Code.Sha1msg1_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38C9 CD", 5, Code.Sha1msg1_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F38CA CD", 4, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38CA CD", 5, Code.Sha1msg2_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38CA CD", 5, Code.Sha1msg2_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38CA CD", 5, Code.Sha1msg2_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F38CB CD", 4, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38CB CD", 5, Code.Sha256rnds2_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38CB CD", 5, Code.Sha256rnds2_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38CB CD", 5, Code.Sha256rnds2_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F38CC CD", 4, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38CC CD", 5, Code.Sha256msg1_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38CC CD", 5, Code.Sha256msg1_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38CC CD", 5, Code.Sha256msg1_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F38CD CD", 4, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F38CD CD", 5, Code.Sha256msg2_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F38CD CD", 5, Code.Sha256msg2_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F38CD CD", 5, Code.Sha256msg2_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vexp2pVReg_k1_RegMem_EVEX_1_Data))]
		void Test16_Vexp2pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vexp2pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vexp2pVReg_k1_RegMem_EVEX_2_Data))]
		void Test16_Vexp2pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vexp2pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vexp2pVReg_k1_RegMem_EVEX_1_Data))]
		void Test32_Vexp2pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vexp2pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vexp2pVReg_k1_RegMem_EVEX_2_Data))]
		void Test32_Vexp2pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vexp2pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vexp2pVReg_k1_RegMem_EVEX_1_Data))]
		void Test64_Vexp2pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vexp2pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB C8 50 01", 7, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB C8 50 01", 7, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vexp2pVReg_k1_RegMem_EVEX_2_Data))]
		void Test64_Vexp2pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vexp2pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 327D1B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C27D3B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B C8 D3", 6, Code.EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 32FD1B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C2FD3B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B C8 D3", 6, Code.EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test16_Vrcp28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vrcp28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test16_Vrcp28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vrcp28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test32_Vrcp28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vrcp28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test32_Vrcp28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vrcp28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test64_Vrcp28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vrcp28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CA 50 01", 7, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CA 50 01", 7, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test64_Vrcp28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vrcp28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 327D1B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C27D3B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CA D3", 6, Code.EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 32FD1B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C2FD3B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CA D3", 6, Code.EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp28ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vrcp28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vrcp28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp28ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vrcp28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vrcp28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp28ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vrcp28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vrcp28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp28ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vrcp28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vrcp28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp28ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vrcp28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vrcp28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CB 50 01", 7, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CB 50 01", 7, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp28ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vrcp28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vrcp28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D0B CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 124D03 CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CB D3", 6, Code.EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E28D0B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 12CD03 CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD7B CB D3", 6, Code.EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data))]
		void Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB CC 50 01", 7, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD48 CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB CC 50 01", 7, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data))]
		void Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vrsqrt28pVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D4B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 327D1B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C27D3B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B CC D3", 6, Code.EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD4B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 32FD1B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C2FD3B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B CC D3", 6, Code.EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt28ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vrsqrt28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vrsqrt28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt28ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vrsqrt28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vrsqrt28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt28ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vrsqrt28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vrsqrt28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt28ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vrsqrt28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vrsqrt28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt28ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vrsqrt28ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vrsqrt28ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B CD 50 01", 7, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B CD 50 01", 7, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt28ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vrsqrt28ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vrsqrt28ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D0B CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 124D03 CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB CD D3", 6, Code.EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E28D0B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 12CD03 CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD7B CD D3", 6, Code.EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8mulbV_VX_WX_1_Data))]
		void Test16_Gf2p8mulbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Gf2p8mulbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38CF 08", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8mulbV_VX_WX_2_Data))]
		void Test16_Gf2p8mulbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Gf2p8mulbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38CF CD", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8mulbV_VX_WX_1_Data))]
		void Test32_Gf2p8mulbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Gf2p8mulbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38CF 08", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8mulbV_VX_WX_2_Data))]
		void Test32_Gf2p8mulbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Gf2p8mulbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38CF CD", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8mulbV_VX_WX_1_Data))]
		void Test64_Gf2p8mulbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Gf2p8mulbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38CF 08", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8mulbV_VX_WX_2_Data))]
		void Test64_Gf2p8mulbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Gf2p8mulbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38CF CD", 5, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38CF CD", 6, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38CF CD", 6, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38CF CD", 6, Code.Gf2p8mulb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8mulbV_VX_HX_WX_1_Data))]
		void Test16_Vgf2p8mulbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vgf2p8mulbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 CF 10", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D CF 10", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8mulbV_VX_HX_WX_2_Data))]
		void Test16_Vgf2p8mulbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vgf2p8mulbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8mulbV_VX_HX_WX_1_Data))]
		void Test32_Vgf2p8mulbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vgf2p8mulbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 CF 10", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D CF 10", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8mulbV_VX_HX_WX_2_Data))]
		void Test32_Vgf2p8mulbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vgf2p8mulbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8mulbV_VX_HX_WX_1_Data))]
		void Test64_Vgf2p8mulbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vgf2p8mulbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 CF 10", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D CF 10", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8mulbV_VX_HX_WX_2_Data))]
		void Test64_Vgf2p8mulbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vgf2p8mulbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 CF D3", 5, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D CF D3", 5, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8mulbV_VX_k1_HX_WX_1_Data))]
		void Test16_Vgf2p8mulbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vgf2p8mulbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8mulbV_VX_k1_HX_WX_2_Data))]
		void Test16_Vgf2p8mulbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vgf2p8mulbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8mulbV_VX_k1_HX_WX_1_Data))]
		void Test32_Vgf2p8mulbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vgf2p8mulbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8mulbV_VX_k1_HX_WX_2_Data))]
		void Test32_Vgf2p8mulbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vgf2p8mulbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8mulbV_VX_k1_HX_WX_1_Data))]
		void Test64_Vgf2p8mulbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vgf2p8mulbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 CF 50 01", 7, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8mulbV_VX_k1_HX_WX_2_Data))]
		void Test64_Vgf2p8mulbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vgf2p8mulbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B CF D3", 6, Code.EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B CF D3", 6, Code.EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B CF D3", 6, Code.EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}
	}
}
