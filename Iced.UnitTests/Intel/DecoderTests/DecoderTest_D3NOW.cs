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
	public sealed class DecoderTest_D3NOW : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_D3NOW_1_Data))]
		void Test16_D3NOW_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0xA55AU, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(2, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_D3NOW_1_Data {
			get {
				yield return new object[] { "0F0F 88 5AA5 0C", 6, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 5AA5 0D", 6, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };
				yield return new object[] { "0F0F 88 5AA5 1C", 6, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 1D", 6, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 86", 6, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 87", 6, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 8A", 6, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 8E", 6, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 90", 6, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 94", 6, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 96", 6, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 97", 6, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 9A", 6, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 9E", 6, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 A0", 6, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 A4", 6, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 A6", 6, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 A7", 6, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 AA", 6, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 AE", 6, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 B0", 6, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 B4", 6, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 B6", 6, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 5AA5 B7", 6, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 5AA5 BB", 6, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };
				yield return new object[] { "0F0F 88 5AA5 BF", 6, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_D3NOW_2_Data))]
		void Test16_D3NOW_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_D3NOW_2_Data {
			get {
				yield return new object[] { "0F0F CD 0C", 4, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 0D", 4, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1C", 4, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1D", 4, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 86", 4, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 87", 4, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8A", 4, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8E", 4, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 90", 4, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 94", 4, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 96", 4, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 97", 4, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9A", 4, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9E", 4, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A0", 4, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A4", 4, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A6", 4, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A7", 4, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AA", 4, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AE", 4, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B0", 4, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B4", 4, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B6", 4, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B7", 4, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BB", 4, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BF", 4, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_D3NOW_1_Data))]
		void Test32_D3NOW_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0xA55A1234, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(4, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_D3NOW_1_Data {
			get {
				yield return new object[] { "0F0F 88 34125AA5 0C", 8, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 34125AA5 0D", 8, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };
				yield return new object[] { "0F0F 88 34125AA5 1C", 8, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 1D", 8, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 86", 8, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 87", 8, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 8A", 8, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 8E", 8, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 90", 8, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 94", 8, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 96", 8, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 97", 8, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 9A", 8, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 9E", 8, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A0", 8, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A4", 8, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A6", 8, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A7", 8, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 AA", 8, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 AE", 8, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B0", 8, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B4", 8, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B6", 8, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B7", 8, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 34125AA5 BB", 8, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };
				yield return new object[] { "0F0F 88 34125AA5 BF", 8, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_D3NOW_2_Data))]
		void Test32_D3NOW_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_D3NOW_2_Data {
			get {
				yield return new object[] { "0F0F CD 0C", 4, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 0D", 4, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1C", 4, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1D", 4, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 86", 4, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 87", 4, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8A", 4, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8E", 4, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 90", 4, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 94", 4, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 96", 4, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 97", 4, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9A", 4, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9E", 4, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A0", 4, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A4", 4, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A6", 4, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A7", 4, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AA", 4, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AE", 4, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B0", 4, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B4", 4, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B6", 4, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B7", 4, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BB", 4, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BF", 4, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_D3NOW_1_Data))]
		void Test64_D3NOW_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0xA55A1234, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(8, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_D3NOW_1_Data {
			get {
				yield return new object[] { "0F0F 88 34125AA5 0C", 8, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 34125AA5 0D", 8, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };
				yield return new object[] { "0F0F 88 34125AA5 1C", 8, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 1D", 8, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 86", 8, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 87", 8, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 8A", 8, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 8E", 8, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 90", 8, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 94", 8, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 96", 8, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 97", 8, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 9A", 8, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 9E", 8, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A0", 8, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A4", 8, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A6", 8, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 A7", 8, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 AA", 8, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 AE", 8, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B0", 8, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B4", 8, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B6", 8, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "0F0F 88 34125AA5 B7", 8, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
				yield return new object[] { "0F0F 88 34125AA5 BB", 8, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };
				yield return new object[] { "0F0F 88 34125AA5 BF", 8, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_D3NOW_2_Data))]
		void Test64_D3NOW_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_D3NOW_2_Data {
			get {
				yield return new object[] { "0F0F CD 0C", 4, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 0D", 4, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1C", 4, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 1D", 4, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 86", 4, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 87", 4, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8A", 4, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 8E", 4, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 90", 4, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 94", 4, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 96", 4, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 97", 4, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9A", 4, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD 9E", 4, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A0", 4, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A4", 4, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A6", 4, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD A7", 4, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AA", 4, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD AE", 4, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B0", 4, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B4", 4, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B6", 4, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD B7", 4, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BB", 4, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "0F0F CD BF", 4, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "4F 0F0F CD 0C", 5, Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 0D", 5, Code.D3NOW_Pi2fd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 1C", 5, Code.D3NOW_Pf2iw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 1D", 5, Code.D3NOW_Pf2id_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 86", 5, Code.D3NOW_Pfrcpv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 87", 5, Code.D3NOW_Pfrsqrtv_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 8A", 5, Code.D3NOW_Pfnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 8E", 5, Code.D3NOW_Pfpnacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 90", 5, Code.D3NOW_Pfcmpge_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 94", 5, Code.D3NOW_Pfmin_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 96", 5, Code.D3NOW_Pfrcp_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 97", 5, Code.D3NOW_Pfrsqrt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 9A", 5, Code.D3NOW_Pfsub_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD 9E", 5, Code.D3NOW_Pfadd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD A0", 5, Code.D3NOW_Pfcmpgt_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD A4", 5, Code.D3NOW_Pfmax_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD A6", 5, Code.D3NOW_Pfrcpit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD A7", 5, Code.D3NOW_Pfrsqit1_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD AA", 5, Code.D3NOW_Pfsubr_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD AE", 5, Code.D3NOW_Pfacc_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD B0", 5, Code.D3NOW_Pfcmpeq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD B4", 5, Code.D3NOW_Pfmul_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD B6", 5, Code.D3NOW_Pfrcpit2_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD B7", 5, Code.D3NOW_Pmulhrw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD BB", 5, Code.D3NOW_Pswapd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F0F CD BF", 5, Code.D3NOW_Pavgusb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}
	}
}
