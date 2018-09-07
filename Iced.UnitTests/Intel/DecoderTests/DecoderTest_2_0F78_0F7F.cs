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
	public sealed class DecoderTest_2_0F78_0F7F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vmread_Ed_Gd_1_Data))]
		void Test16_Vmread_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_Vmread_Ed_Gd_1_Data {
			get {
				yield return new object[] { "0F78 CE", 3, Code.Vmread_Ed_Gd, Register.ESI, Register.ECX };
				yield return new object[] { "66 0F78 CE", 4, Code.Vmread_Ed_Gd, Register.ESI, Register.ECX };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vmread_Ed_Gd_2_Data))]
		void Test16_Vmread_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test16_Vmread_Ed_Gd_2_Data {
			get {
				yield return new object[] { "0F78 18", 3, Code.Vmread_Ed_Gd, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vmread_Ed_Gd_1_Data))]
		void Test32_Vmread_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_Vmread_Ed_Gd_1_Data {
			get {
				yield return new object[] { "0F78 CE", 3, Code.Vmread_Ed_Gd, Register.ESI, Register.ECX };
				yield return new object[] { "66 0F78 CE", 4, Code.Vmread_Ed_Gd, Register.ESI, Register.ECX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vmread_Ed_Gd_2_Data))]
		void Test32_Vmread_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test32_Vmread_Ed_Gd_2_Data {
			get {
				yield return new object[] { "0F78 18", 3, Code.Vmread_Ed_Gd, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vmread_Ed_Gd_1_Data))]
		void Test64_Vmread_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_Vmread_Ed_Gd_1_Data {
			get {
				yield return new object[] { "0F78 CE", 3, Code.Vmread_Eq_Gq, Register.RSI, Register.RCX };
				yield return new object[] { "44 0F78 CE", 4, Code.Vmread_Eq_Gq, Register.RSI, Register.R9 };
				yield return new object[] { "41 0F78 CE", 4, Code.Vmread_Eq_Gq, Register.R14, Register.RCX };
				yield return new object[] { "45 0F78 CE", 4, Code.Vmread_Eq_Gq, Register.R14, Register.R9 };
				yield return new object[] { "66 0F78 CE", 4, Code.Vmread_Eq_Gq, Register.RSI, Register.RCX };
				yield return new object[] { "4A 0F78 CE", 4, Code.Vmread_Eq_Gq, Register.RSI, Register.RCX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vmread_Ed_Gd_2_Data))]
		void Test64_Vmread_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test64_Vmread_Ed_Gd_2_Data {
			get {
				yield return new object[] { "0F78 18", 3, Code.Vmread_Eq_Gq, Register.RBX, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vmwrite_Gd_Ed_1_Data))]
		void Test16_Vmwrite_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_Vmwrite_Gd_Ed_1_Data {
			get {
				yield return new object[] { "0F79 CE", 3, Code.Vmwrite_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F79 CE", 4, Code.Vmwrite_Gd_Ed, Register.ECX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vmwrite_Gd_Ed_2_Data))]
		void Test16_Vmwrite_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vmwrite_Gd_Ed_2_Data {
			get {
				yield return new object[] { "0F79 18", 3, Code.Vmwrite_Gd_Ed, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vmwrite_Gd_Ed_1_Data))]
		void Test32_Vmwrite_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_Vmwrite_Gd_Ed_1_Data {
			get {
				yield return new object[] { "0F79 CE", 3, Code.Vmwrite_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F79 CE", 4, Code.Vmwrite_Gd_Ed, Register.ECX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vmwrite_Gd_Ed_2_Data))]
		void Test32_Vmwrite_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vmwrite_Gd_Ed_2_Data {
			get {
				yield return new object[] { "0F79 18", 3, Code.Vmwrite_Gd_Ed, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vmwrite_Gd_Ed_1_Data))]
		void Test64_Vmwrite_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_Vmwrite_Gd_Ed_1_Data {
			get {
				yield return new object[] { "0F79 CE", 3, Code.Vmwrite_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "44 0F79 CE", 4, Code.Vmwrite_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "41 0F79 CE", 4, Code.Vmwrite_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "45 0F79 CE", 4, Code.Vmwrite_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "66 0F79 CE", 4, Code.Vmwrite_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "4A 0F79 CE", 4, Code.Vmwrite_Gq_Eq, Register.RCX, Register.RSI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vmwrite_Gd_Ed_2_Data))]
		void Test64_Vmwrite_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vmwrite_Gd_Ed_2_Data {
			get {
				yield return new object[] { "0F79 18", 3, Code.Vmwrite_Gq_Eq, Register.RBX, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt1V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test16_Cvt1V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Cvt1V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt1V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test16_Cvt1V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Cvt1V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17C1B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C3B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FC0B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FC1B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC3B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC5B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC7B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17D0B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17D1B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D3B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt1V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test32_Cvt1V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Cvt1V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt1V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test32_Cvt1V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Cvt1V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17C1B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C3B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FC0B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FC1B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC3B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC5B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC7B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17D0B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17D1B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D3B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt1V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test64_Cvt1V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Cvt1V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 78 50 01", 7, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 78 50 01", 7, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 78 50 01", 7, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 78 50 01", 7, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 78 50 01", 7, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt1V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test64_Cvt1V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Cvt1V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C0B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17C8B 78 D3", 6, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C2B 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17CAB 78 D3", 6, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 317C1B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C17C3B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 78 D3", 6, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FC0B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC0B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FC8B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC2B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FCAB 78 D3", 6, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 31FC1B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C1FC3B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC5B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FC7B 78 D3", 6, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17D0B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D0B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17D8B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D2B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17DAB 78 D3", 6, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 317D1B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C17D3B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 78 D3", 6, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 31FD1B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C1FD3B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 78 D3", 6, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt2V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test16_Cvt2V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Cvt2V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt2V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test16_Cvt2V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Cvt2V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17C3B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17CDB 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FC3B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FCDB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt2V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test32_Cvt2V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Cvt2V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt2V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test32_Cvt2V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Cvt2V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17C3B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17CDB 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FC3B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FCDB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt2V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test64_Cvt2V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Cvt2V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9B 79 50 01", 7, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C28 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17C48 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDB 79 50 01", 7, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F1FC08 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FC9B 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FC28 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FCBB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FC48 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FCDB 79 50 01", 7, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17D08 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 79 50 01", 7, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 79 50 01", 7, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt2V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test64_Cvt2V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Cvt2V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C0B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17C8B 79 D3", 6, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C2B 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17CAB 79 D3", 6, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317C3B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17CDB 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 79 D3", 6, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC0B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FC8B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC2B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FCAB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FC3B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FCDB 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 79 D3", 6, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D0B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17D8B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D2B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17DAB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317D3B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17DDB 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 79 D3", 6, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FD3B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FDDB 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 79 D3", 6, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt3V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test16_Cvt3V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Cvt3V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F17E9B 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_UInt32, 4, true };

				yield return new object[] { "62 F17E28 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17EBB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17E48 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17EDB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F1FE08 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FE9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FE28 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FEBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FE48 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FEDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17F08 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17F9B 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17F28 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17FBB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F17F48 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F17FDB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt32, 4, true };

				yield return new object[] { "62 F1FF08 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FF9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FF28 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FFBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FF48 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FFDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17D08 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt3V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test16_Cvt3V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Cvt3V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D0B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17D1B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D3B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FE3B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FEDB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17F0B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F2B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F4B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F9B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17F3B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17FDB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17F7B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FF3B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FFDB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt3V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test32_Cvt3V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Cvt3V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F17E9B 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_UInt32, 4, true };

				yield return new object[] { "62 F17E28 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17EBB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17E48 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17EDB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F1FE08 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FE9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FE28 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FEBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FE48 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FEDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17F08 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17F9B 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17F28 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17FBB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F17F48 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F17FDB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt32, 4, true };

				yield return new object[] { "62 F1FF08 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FF9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FF28 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FFBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FF48 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FFDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17D08 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt3V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test32_Cvt3V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Cvt3V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D0B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17D1B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D3B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FE3B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FEDB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17F0B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F2B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F4B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F9B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17F3B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17FDB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17F7B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FF3B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FFDB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt3V_Reg_k1_RegMem_EVEX_1_Data))]
		void Test64_Cvt3V_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Cvt3V_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7A 50 01", 7, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7A 50 01", 7, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F17E9B 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_UInt32, 4, true };

				yield return new object[] { "62 F17E28 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17EBB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17E48 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17EDB 7A 50 01", 7, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F1FE08 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FE9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FE28 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FEBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FE48 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FEDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17F08 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F17F9B 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt32, 4, true };

				yield return new object[] { "62 F17F28 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F17FBB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_UInt32, 4, true };

				yield return new object[] { "62 F17F48 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F17FDB 7A 50 01", 7, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_UInt32, 4, true };

				yield return new object[] { "62 F1FF08 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1FF9B 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_UInt64, 8, true };

				yield return new object[] { "62 F1FF28 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1FFBB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.K3, MemorySize.Broadcast256_UInt64, 8, true };

				yield return new object[] { "62 F1FF48 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1FFDB 7A 50 01", 7, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.K3, MemorySize.Broadcast512_UInt64, 8, true };

				yield return new object[] { "62 F17D08 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17D9B 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17D28 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17DBB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D48 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DDB 7B 50 01", 7, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 7B 50 01", 7, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt3V_Reg_k1_RegMem_EVEX_2_Data))]
		void Test64_Cvt3V_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Cvt3V_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D0B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D0B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17D8B 7A D3", 6, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D2B 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17DAB 7A D3", 6, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 317D1B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C17D3B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D5B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17D7B 7A D3", 6, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB 7A D3", 6, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 31FD1B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C1FD3B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B 7A D3", 6, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E0B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17E8B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E2B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17EAB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E4B 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17ECB 7A D3", 6, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FE0B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FE8B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FE2B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FEAB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FE3B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FEDB 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B 7A D3", 6, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17F0B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317F0B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17F8B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F2B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317F2B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17FAB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F4B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F9B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317F3B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17FDB 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17F7B 7A D3", 6, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FF0B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FF8B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FF2B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FFAB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FF3B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FFDB 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B 7A D3", 6, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D0B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17D8B 7B D3", 6, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D2B 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17DAB 7B D3", 6, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317D3B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17DDB 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 7B D3", 6, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FD0B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FD3B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FDDB 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 7B D3", 6, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt4V_Reg_RegMem_er_1_Data))]
		void Test16_Cvt4V_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Cvt4V_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt4V_Reg_RegMem_er_2_Data))]
		void Test16_Cvt4V_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Cvt4V_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17F08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17E08 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17E38 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17F38 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt4V_Reg_RegMem_er_1_Data))]
		void Test32_Cvt4V_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Cvt4V_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt4V_Reg_RegMem_er_2_Data))]
		void Test32_Cvt4V_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Cvt4V_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17F08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17E08 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17E38 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17F38 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt4V_Reg_RegMem_er_1_Data))]
		void Test64_Cvt4V_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Cvt4V_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FE08 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE28 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE48 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE68 78 50 01", 7, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F1FF08 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF28 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF48 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF68 78 50 01", 7, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FE08 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE28 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE48 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE68 79 50 01", 7, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F1FF08 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF28 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF48 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF68 79 50 01", 7, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt4V_Reg_RegMem_er_2_Data))]
		void Test64_Cvt4V_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Cvt4V_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17E18 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.EDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 317E08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Register.R10D, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F1FE08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FE18 78 D3", 6, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.RDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 31FE08 78 D3", 6, Code.EVEX_Vcvttss2usi_Gq_WX_sae, Register.R10, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F17F08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17F18 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.EDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 317F08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Register.R10D, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F1FF08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FF18 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.RDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 31FF08 78 D3", 6, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, Register.R10, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F17E08 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17E18 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 317E38 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.R10D, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 79 D3", 6, Code.EVEX_Vcvtss2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1FE08 79 D3", 6, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FE18 79 D3", 6, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 31FE38 79 D3", 6, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.R10, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F1FE58 79 D3", 6, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1FE78 79 D3", 6, Code.EVEX_Vcvtss2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17F18 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 317F38 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.R10D, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1FF08 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FF18 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 31FF38 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.R10, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F1FF58 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1FF78 79 D3", 6, Code.EVEX_Vcvtsd2usi_Gq_WX_er, Register.RDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test16_Cvt5V_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E28 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E48 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E68 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };

				yield return new object[] { "62 F14F08 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F28 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F48 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F68 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test16_Cvt5V_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14E18 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F14E38 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F14E58 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F18 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F38 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F58 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test32_Cvt5V_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E28 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E48 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E68 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };

				yield return new object[] { "62 F14F08 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F28 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F48 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F68 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test32_Cvt5V_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14E18 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F14E38 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F14E58 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F18 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F38 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F58 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test64_Cvt5V_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Cvt5V_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E28 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E48 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14E68 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };

				yield return new object[] { "62 F1CE08 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CE28 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CE48 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CE68 7B 50 01", 7, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };

				yield return new object[] { "62 F14F08 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F28 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F48 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };
				yield return new object[] { "62 F14F68 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false };

				yield return new object[] { "62 F1CF08 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CF28 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CF48 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
				yield return new object[] { "62 F1CF68 7B 50 01", 7, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test64_Cvt5V_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Cvt5V_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E10E18 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 714E30 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM10, Register.XMM22, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D14E58 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1CE08 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E18E18 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 71CE30 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM10, Register.XMM22, Register.RBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D1CE58 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1CE78 7B D3", 6, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E10F18 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 714F30 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM10, Register.XMM22, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 D14F58 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };

				yield return new object[] { "62 F1CF08 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E18F18 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 71CF30 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM10, Register.XMM22, Register.RBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D1CF58 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1CF78 7B D3", 6, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_HaddHsubV_VX_WX_1_Data))]
		void Test16_HaddHsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_HaddHsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F7C 08", 4, Code.Haddpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7C 08", 4, Code.Haddps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F7D 08", 4, Code.Hsubpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7D 08", 4, Code.Hsubps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_HaddHsubV_VX_WX_2_Data))]
		void Test16_HaddHsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_HaddHsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F7C CD", 4, Code.Haddpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F7C CD", 4, Code.Haddps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F7D CD", 4, Code.Hsubpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F7D CD", 4, Code.Hsubps_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_HaddHsubV_VX_WX_1_Data))]
		void Test32_HaddHsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_HaddHsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F7C 08", 4, Code.Haddpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7C 08", 4, Code.Haddps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F7D 08", 4, Code.Hsubpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7D 08", 4, Code.Hsubps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_HaddHsubV_VX_WX_2_Data))]
		void Test32_HaddHsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_HaddHsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F7C CD", 4, Code.Haddpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F7C CD", 4, Code.Haddps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F7D CD", 4, Code.Hsubpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F7D CD", 4, Code.Hsubps_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_HaddHsubV_VX_WX_1_Data))]
		void Test64_HaddHsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_HaddHsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F7C 08", 4, Code.Haddpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7C 08", 4, Code.Haddps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F7D 08", 4, Code.Hsubpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0F7D 08", 4, Code.Hsubps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_HaddHsubV_VX_WX_2_Data))]
		void Test64_HaddHsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_HaddHsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F7C CD", 4, Code.Haddpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F7C CD", 5, Code.Haddpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F7C CD", 5, Code.Haddpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F7C CD", 5, Code.Haddpd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F7C CD", 4, Code.Haddps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F7C CD", 5, Code.Haddps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F7C CD", 5, Code.Haddps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F7C CD", 5, Code.Haddps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F7D CD", 4, Code.Hsubpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F7D CD", 5, Code.Hsubpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F7D CD", 5, Code.Hsubpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F7D CD", 5, Code.Hsubpd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F7D CD", 4, Code.Hsubps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F7D CD", 5, Code.Hsubps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F7D CD", 5, Code.Hsubps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F7D CD", 5, Code.Hsubps_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VhaddV_VX_HX_WX_1_Data))]
		void Test16_VhaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VhaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 7C 10", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7C 10", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7C 10", 5, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7C 10", 5, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7C 10", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7C 10", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7C 10", 5, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7C 10", 5, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 7D 10", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7D 10", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7D 10", 5, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7D 10", 5, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7D 10", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7D 10", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7D 10", 5, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7D 10", 5, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VhaddV_VX_HX_WX_2_Data))]
		void Test16_VhaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test16_VhaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 7C D3", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7C D3", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB 7C D3", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7C D3", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 7D D3", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7D D3", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB 7D D3", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7D D3", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VhaddV_VX_HX_WX_1_Data))]
		void Test32_VhaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VhaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 7C 10", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7C 10", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7C 10", 5, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7C 10", 5, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7C 10", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7C 10", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7C 10", 5, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7C 10", 5, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 7D 10", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7D 10", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7D 10", 5, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7D 10", 5, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7D 10", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7D 10", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7D 10", 5, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7D 10", 5, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VhaddV_VX_HX_WX_2_Data))]
		void Test32_VhaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test32_VhaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 7C D3", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7C D3", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB 7C D3", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7C D3", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 7D D3", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7D D3", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB 7D D3", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7D D3", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VhaddV_VX_HX_WX_1_Data))]
		void Test64_VhaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VhaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 7C 10", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7C 10", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7C 10", 5, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7C 10", 5, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7C 10", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7C 10", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7C 10", 5, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7C 10", 5, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 7D 10", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 7D 10", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 7D 10", 5, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 7D 10", 5, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB 7D 10", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF 7D 10", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB 7D 10", 5, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF 7D 10", 5, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VhaddV_VX_HX_WX_2_Data))]
		void Test64_VhaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test64_VhaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 7C D3", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7C D3", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 7C D3", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 7C D3", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 7C D3", 4, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 7C D3", 4, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 7C D3", 5, Code.VEX_Vhaddpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 7C D3", 5, Code.VEX_Vhaddpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CB 7C D3", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7C D3", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C54B 7C D3", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54F 7C D3", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C58B 7C D3", 4, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58F 7C D3", 4, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C14B 7C D3", 5, Code.VEX_Vhaddps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14F 7C D3", 5, Code.VEX_Vhaddps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 7D D3", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 7D D3", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 7D D3", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 7D D3", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 7D D3", 4, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 7D D3", 4, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 7D D3", 5, Code.VEX_Vhsubpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 7D D3", 5, Code.VEX_Vhsubpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CB 7D D3", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 7D D3", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C54B 7D D3", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54F 7D D3", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C58B 7D D3", 4, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58F 7D D3", 4, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C14B 7D D3", 5, Code.VEX_Vhsubps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14F 7D D3", 5, Code.VEX_Vhsubps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_1_Data))]
		void Test16_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7E 08", 3, Code.Movd_Ed_P, Register.MM1, MemorySize.UInt32 };

				yield return new object[] { "66 0F7E 08", 4, Code.Movd_Ed_VX, Register.XMM1, MemorySize.UInt32 };

				yield return new object[] { "C5F9 7E 08", 4, Code.VEX_Vmovd_Ed_VX, Register.XMM1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_2_Data))]
		void Test16_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7E CD", 3, Code.Movd_Ed_P, Register.EBP, Register.MM1 };

				yield return new object[] { "66 0F7E CD", 4, Code.Movd_Ed_VX, Register.EBP, Register.XMM1 };

				yield return new object[] { "C5F9 7E CD", 4, Code.VEX_Vmovd_Ed_VX, Register.EBP, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_1_Data))]
		void Test32_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7E 08", 3, Code.Movd_Ed_P, Register.MM1, MemorySize.UInt32 };

				yield return new object[] { "66 0F7E 08", 4, Code.Movd_Ed_VX, Register.XMM1, MemorySize.UInt32 };

				yield return new object[] { "C5F9 7E 08", 4, Code.VEX_Vmovd_Ed_VX, Register.XMM1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_2_Data))]
		void Test32_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7E CD", 3, Code.Movd_Ed_P, Register.EBP, Register.MM1 };

				yield return new object[] { "66 0F7E CD", 4, Code.Movd_Ed_VX, Register.EBP, Register.XMM1 };

				yield return new object[] { "C5F9 7E CD", 4, Code.VEX_Vmovd_Ed_VX, Register.EBP, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_1_Data))]
		void Test64_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7E 08", 3, Code.Movd_Ed_P, Register.MM1, MemorySize.UInt32 };
				yield return new object[] { "48 0F7E 08", 4, Code.Movq_Eq_P, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0F7E 08", 4, Code.Movd_Ed_VX, Register.XMM1, MemorySize.UInt32 };
				yield return new object[] { "66 48 0F7E 08", 5, Code.Movq_Eq_VX, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5F9 7E 08", 4, Code.VEX_Vmovd_Ed_VX, Register.XMM1, MemorySize.UInt32 };
				yield return new object[] { "C4E1F9 7E 08", 5, Code.VEX_Vmovq_Eq_VX, Register.XMM1, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_2_Data))]
		void Test64_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7E CD", 3, Code.Movd_Ed_P, Register.EBP, Register.MM1 };
				yield return new object[] { "41 0F7E CD", 4, Code.Movd_Ed_P, Register.R13D, Register.MM1 };

				yield return new object[] { "48 0F7E CD", 4, Code.Movq_Eq_P, Register.RBP, Register.MM1 };
				yield return new object[] { "49 0F7E CD", 4, Code.Movq_Eq_P, Register.R13, Register.MM1 };

				yield return new object[] { "66 0F7E CD", 4, Code.Movd_Ed_VX, Register.EBP, Register.XMM1 };
				yield return new object[] { "66 44 0F7E CD", 5, Code.Movd_Ed_VX, Register.EBP, Register.XMM9 };
				yield return new object[] { "66 41 0F7E CD", 5, Code.Movd_Ed_VX, Register.R13D, Register.XMM1 };
				yield return new object[] { "66 45 0F7E CD", 5, Code.Movd_Ed_VX, Register.R13D, Register.XMM9 };

				yield return new object[] { "66 48 0F7E CD", 5, Code.Movq_Eq_VX, Register.RBP, Register.XMM1 };
				yield return new object[] { "66 4C 0F7E CD", 5, Code.Movq_Eq_VX, Register.RBP, Register.XMM9 };
				yield return new object[] { "66 49 0F7E CD", 5, Code.Movq_Eq_VX, Register.R13, Register.XMM1 };
				yield return new object[] { "66 4D 0F7E CD", 5, Code.Movq_Eq_VX, Register.R13, Register.XMM9 };

				yield return new object[] { "C5F9 7E CD", 4, Code.VEX_Vmovd_Ed_VX, Register.EBP, Register.XMM1 };
				yield return new object[] { "C579 7E CD", 4, Code.VEX_Vmovd_Ed_VX, Register.EBP, Register.XMM9 };
				yield return new object[] { "C4C179 7E CD", 5, Code.VEX_Vmovd_Ed_VX, Register.R13D, Register.XMM1 };
				yield return new object[] { "C44179 7E CD", 5, Code.VEX_Vmovd_Ed_VX, Register.R13D, Register.XMM9 };

				yield return new object[] { "C4E1F9 7E CD", 5, Code.VEX_Vmovq_Eq_VX, Register.RBP, Register.XMM1 };
				yield return new object[] { "C461F9 7E CD", 5, Code.VEX_Vmovq_Eq_VX, Register.RBP, Register.XMM9 };
				yield return new object[] { "C4C1F9 7E CD", 5, Code.VEX_Vmovq_Eq_VX, Register.R13, Register.XMM1 };
				yield return new object[] { "C441F9 7E CD", 5, Code.VEX_Vmovq_Eq_VX, Register.R13, Register.XMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovV_RegMem_Reg_1_Data))]
		void Test16_VmovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VmovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "62 F17D08 7E 50 01", 7, Code.EVEX_Vmovd_Ed_VX, Register.XMM2, MemorySize.UInt32, 4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovV_RegMem_Reg_2_Data))]
		void Test16_VmovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VmovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "62 F17D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.EBX, Register.XMM2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovV_RegMem_Reg_1_Data))]
		void Test32_VmovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VmovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "62 F17D08 7E 50 01", 7, Code.EVEX_Vmovd_Ed_VX, Register.XMM2, MemorySize.UInt32, 4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovV_RegMem_Reg_2_Data))]
		void Test32_VmovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VmovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "62 F17D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.EBX, Register.XMM2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovV_RegMem_Reg_1_Data))]
		void Test64_VmovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VmovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "62 F17D08 7E 50 01", 7, Code.EVEX_Vmovd_Ed_VX, Register.XMM2, MemorySize.UInt32, 4 };

				yield return new object[] { "62 F1FD08 7E 50 01", 7, Code.EVEX_Vmovq_Eq_VX, Register.XMM2, MemorySize.UInt64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovV_RegMem_Reg_2_Data))]
		void Test64_VmovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VmovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "62 F17D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.EBX, Register.XMM2 };
				yield return new object[] { "62 E17D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.EBX, Register.XMM18 };
				yield return new object[] { "62 517D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.R11D, Register.XMM10 };
				yield return new object[] { "62 D17D08 7E D3", 6, Code.EVEX_Vmovd_Ed_VX, Register.R11D, Register.XMM2 };

				yield return new object[] { "62 F1FD08 7E D3", 6, Code.EVEX_Vmovq_Eq_VX, Register.RBX, Register.XMM2 };
				yield return new object[] { "62 E1FD08 7E D3", 6, Code.EVEX_Vmovq_Eq_VX, Register.RBX, Register.XMM18 };
				yield return new object[] { "62 51FD08 7E D3", 6, Code.EVEX_Vmovq_Eq_VX, Register.R11, Register.XMM10 };
				yield return new object[] { "62 D1FD08 7E D3", 6, Code.EVEX_Vmovq_Eq_VX, Register.R11, Register.XMM2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_VX_WX_1_Data))]
		void Test16_MovqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovqV_VX_WX_1_Data {
			get {
				yield return new object[] { "F3 0F7E 08", 4, Code.Movq_VX_WX, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5FA 7E 10", 4, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1FA 7E 10", 5, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_VX_WX_2_Data))]
		void Test16_MovqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_MovqV_VX_WX_2_Data {
			get {
				yield return new object[] { "F3 0F7E CD", 4, Code.Movq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 7E CD", 4, Code.VEX_Vmovq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_VX_WX_1_Data))]
		void Test32_MovqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovqV_VX_WX_1_Data {
			get {
				yield return new object[] { "F3 0F7E 08", 4, Code.Movq_VX_WX, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5FA 7E 10", 4, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1FA 7E 10", 5, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_VX_WX_2_Data))]
		void Test32_MovqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_MovqV_VX_WX_2_Data {
			get {
				yield return new object[] { "F3 0F7E CD", 4, Code.Movq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 7E CD", 4, Code.VEX_Vmovq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_VX_WX_1_Data))]
		void Test64_MovqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovqV_VX_WX_1_Data {
			get {
				yield return new object[] { "F3 0F7E 08", 4, Code.Movq_VX_WX, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5FA 7E 10", 4, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1FA 7E 10", 5, Code.VEX_Vmovq_VX_WX, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_VX_WX_2_Data))]
		void Test64_MovqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_MovqV_VX_WX_2_Data {
			get {
				yield return new object[] { "F3 0F7E CD", 4, Code.Movq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F7E CD", 5, Code.Movq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F7E CD", 5, Code.Movq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F7E CD", 5, Code.Movq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FA 7E CD", 4, Code.VEX_Vmovq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A 7E CD", 4, Code.VEX_Vmovq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A 7E CD", 5, Code.VEX_Vmovq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A 7E CD", 5, Code.VEX_Vmovq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_EVEX_VX_WX_1_Data))]
		void Test16_MovqV_EVEX_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_MovqV_EVEX_VX_WX_1_Data {
			get {
				yield return new object[] { "62 F1FE08 7E 50 01", 7, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_EVEX_VX_WX_2_Data))]
		void Test16_MovqV_EVEX_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_MovqV_EVEX_VX_WX_2_Data {
			get {
				yield return new object[] { "62 F1FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_EVEX_VX_WX_1_Data))]
		void Test32_MovqV_EVEX_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_MovqV_EVEX_VX_WX_1_Data {
			get {
				yield return new object[] { "62 F1FE08 7E 50 01", 7, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_EVEX_VX_WX_2_Data))]
		void Test32_MovqV_EVEX_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_MovqV_EVEX_VX_WX_2_Data {
			get {
				yield return new object[] { "62 F1FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_EVEX_VX_WX_1_Data))]
		void Test64_MovqV_EVEX_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_MovqV_EVEX_VX_WX_1_Data {
			get {
				yield return new object[] { "62 F1FE08 7E 50 01", 7, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_EVEX_VX_WX_2_Data))]
		void Test64_MovqV_EVEX_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_MovqV_EVEX_VX_WX_2_Data {
			get {
				yield return new object[] { "62 F1FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E1FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM18, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 11FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM10, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B1FE08 7E D3", 6, Code.EVEX_Vmovq_VX_WX, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_1_Data))]
		void Test16_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7F 08", 3, Code.Movq_Q_P, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_2_Data))]
		void Test16_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7F CD", 3, Code.Movq_Q_P, Register.MM5, Register.MM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_1_Data))]
		void Test32_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7F 08", 3, Code.Movq_Q_P, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_2_Data))]
		void Test32_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7F CD", 3, Code.Movq_Q_P, Register.MM5, Register.MM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_1_Data))]
		void Test64_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F7F 08", 3, Code.Movq_Q_P, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_2_Data))]
		void Test64_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F7F CD", 3, Code.Movq_Q_P, Register.MM5, Register.MM1 };
				yield return new object[] { "4F 0F7F CD", 4, Code.Movq_Q_P, Register.MM5, Register.MM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_RegMem_Reg_1_Data))]
		void Test16_MovdqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test16_MovdqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0F7F 08", 4, Code.Movdqa_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 7F 10", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 7F 10", 5, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 7F 10", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 7F 10", 5, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F7F 08", 4, Code.Movdqu_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 7F 10", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 7F 10", 5, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 7F 10", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 7F 10", 5, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_RegMem_Reg_2_Data))]
		void Test16_MovdqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_MovdqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F7F CD", 4, Code.Movdqa_WX_VX, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F9 7F CD", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 7F CD", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM5, Register.YMM1 };

				yield return new object[] { "F3 0F7F CD", 4, Code.Movdqu_WX_VX, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5FA 7F CD", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FE 7F CD", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_RegMem_Reg_1_Data))]
		void Test32_MovdqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test32_MovdqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0F7F 08", 4, Code.Movdqa_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 7F 10", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 7F 10", 5, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 7F 10", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 7F 10", 5, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F7F 08", 4, Code.Movdqu_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 7F 10", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 7F 10", 5, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 7F 10", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 7F 10", 5, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_RegMem_Reg_2_Data))]
		void Test32_MovdqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_MovdqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F7F CD", 4, Code.Movdqa_WX_VX, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F9 7F CD", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 7F CD", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM5, Register.YMM1 };

				yield return new object[] { "F3 0F7F CD", 4, Code.Movdqu_WX_VX, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5FA 7F CD", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FE 7F CD", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_RegMem_Reg_1_Data))]
		void Test64_MovdqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		}
		public static IEnumerable<object[]> Test64_MovdqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0F7F 08", 4, Code.Movdqa_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 7F 10", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 7F 10", 5, Code.VEX_Vmovdqa_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 7F 10", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 7F 10", 5, Code.VEX_Vmovdqa_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F7F 08", 4, Code.Movdqu_WX_VX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 7F 10", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 7F 10", 5, Code.VEX_Vmovdqu_WX_VX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 7F 10", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 7F 10", 5, Code.VEX_Vmovdqu_WY_VY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_RegMem_Reg_2_Data))]
		void Test64_MovdqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_MovdqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F7F CD", 4, Code.Movdqa_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "66 44 0F7F CD", 5, Code.Movdqa_WX_VX, Register.XMM5, Register.XMM9 };
				yield return new object[] { "66 41 0F7F CD", 5, Code.Movdqa_WX_VX, Register.XMM13, Register.XMM1 };
				yield return new object[] { "66 45 0F7F CD", 5, Code.Movdqa_WX_VX, Register.XMM13, Register.XMM9 };

				yield return new object[] { "C5F9 7F CD", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C579 7F CD", 4, Code.VEX_Vmovdqa_WX_VX, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C179 7F CD", 5, Code.VEX_Vmovdqa_WX_VX, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44179 7F CD", 5, Code.VEX_Vmovdqa_WX_VX, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FD 7F CD", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57D 7F CD", 4, Code.VEX_Vmovdqa_WY_VY, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17D 7F CD", 5, Code.VEX_Vmovdqa_WY_VY, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417D 7F CD", 5, Code.VEX_Vmovdqa_WY_VY, Register.YMM13, Register.YMM9 };

				yield return new object[] { "F3 0F7F CD", 4, Code.Movdqu_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "F3 44 0F7F CD", 5, Code.Movdqu_WX_VX, Register.XMM5, Register.XMM9 };
				yield return new object[] { "F3 41 0F7F CD", 5, Code.Movdqu_WX_VX, Register.XMM13, Register.XMM1 };
				yield return new object[] { "F3 45 0F7F CD", 5, Code.Movdqu_WX_VX, Register.XMM13, Register.XMM9 };

				yield return new object[] { "C5FA 7F CD", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C57A 7F CD", 4, Code.VEX_Vmovdqu_WX_VX, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C17A 7F CD", 5, Code.VEX_Vmovdqu_WX_VX, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C4417A 7F CD", 5, Code.VEX_Vmovdqu_WX_VX, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FE 7F CD", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57E 7F CD", 4, Code.VEX_Vmovdqu_WY_VY, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17E 7F CD", 5, Code.VEX_Vmovdqu_WY_VY, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417E 7F CD", 5, Code.VEX_Vmovdqu_WY_VY, Register.YMM13, Register.YMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovdqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test16_MovdqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D0B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17D2B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17D4B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FD08 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD0B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F1FD28 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FD2B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F1FD48 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FD4B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F17E08 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E0B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17E28 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17E2B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17E48 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17E4B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FE08 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE0B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1FE28 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FE2B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1FE48 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FE4B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };

				yield return new object[] { "62 F17F08 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F0B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F17F28 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17F2B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F17F48 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17F4B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F1FF08 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF0B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F1FF28 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FF2B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F1FF48 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FF4B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_RegMem_Reg_EVEX_2_Data))]
		void Test16_MovdqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_MovdqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DCB 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FECB 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FCB 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovdqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test32_MovdqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D0B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17D2B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17D4B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FD08 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD0B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F1FD28 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FD2B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F1FD48 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FD4B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F17E08 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E0B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17E28 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17E2B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17E48 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17E4B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FE08 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE0B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1FE28 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FE2B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1FE48 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FE4B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };

				yield return new object[] { "62 F17F08 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F0B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F17F28 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17F2B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F17F48 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17F4B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F1FF08 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF0B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F1FF28 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FF2B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F1FF48 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FF4B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_RegMem_Reg_EVEX_2_Data))]
		void Test32_MovdqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_MovdqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DCB 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FECB 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FCB 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovdqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test64_MovdqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D0B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17D2B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17D4B 7F 50 01", 7, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FD08 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD0B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F1FD28 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FD2B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F1FD48 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FD4B 7F 50 01", 7, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F17E08 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E0B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17E28 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17E2B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17E48 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17E4B 7F 50 01", 7, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F1FE08 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE0B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1FE28 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FE2B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1FE48 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FE4B 7F 50 01", 7, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };

				yield return new object[] { "62 F17F08 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F0B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F17F28 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17F2B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F17F48 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17F4B 7F 50 01", 7, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F1FF08 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF0B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F1FF28 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FF2B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F1FF48 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FF4B 7F 50 01", 7, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_RegMem_Reg_EVEX_2_Data))]
		void Test64_MovdqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_MovdqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317D8B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17D8B 7F D3", 6, Code.EVEX_Vmovdqa32_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317DAB 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17DAB 7F D3", 6, Code.EVEX_Vmovdqa32_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317DCB 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17DCB 7F D3", 6, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 7F D3", 6, Code.EVEX_Vmovdqa64_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 7F D3", 6, Code.EVEX_Vmovdqa64_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 7F D3", 6, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317E8B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17E8B 7F D3", 6, Code.EVEX_Vmovdqu32_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317EAB 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17EAB 7F D3", 6, Code.EVEX_Vmovdqu32_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317ECB 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17ECB 7F D3", 6, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FE8B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FE8B 7F D3", 6, Code.EVEX_Vmovdqu64_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FEAB 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FEAB 7F D3", 6, Code.EVEX_Vmovdqu64_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FECB 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FECB 7F D3", 6, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317F8B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17F8B 7F D3", 6, Code.EVEX_Vmovdqu8_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317FAB 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17FAB 7F D3", 6, Code.EVEX_Vmovdqu8_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317FCB 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17FCB 7F D3", 6, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FF8B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FF8B 7F D3", 6, Code.EVEX_Vmovdqu16_WX_k1z_VX, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFAB 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFAB 7F D3", 6, Code.EVEX_Vmovdqu16_WY_k1z_VY, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFCB 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFCB 7F D3", 6, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}
	}
}
