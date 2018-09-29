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
	public sealed class DecoderTest_3_0F38D8_0F38DF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_AesimcV_VX_WX_1_Data))]
		void Test16_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesimcV_VX_WX_2_Data))]
		void Test16_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesimcV_VX_WX_1_Data))]
		void Test32_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesimcV_VX_WX_2_Data))]
		void Test32_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesimcV_VX_WX_1_Data))]
		void Test64_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesimcV_VX_WX_2_Data))]
		void Test64_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DB CD", 6, Code.Aesimc_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DB CD", 6, Code.Aesimc_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DB CD", 6, Code.Aesimc_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C46279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C44279 DB D3", 5, Code.VEX_Vaesimc_xmm_xmmm128, Register.XMM10, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesEncDecV_VX_WX_1_Data))]
		void Test16_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesEncDecV_VX_WX_2_Data))]
		void Test16_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesEncDecV_VX_WX_1_Data))]
		void Test32_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesEncDecV_VX_WX_2_Data))]
		void Test32_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesEncDecV_VX_WX_1_Data))]
		void Test64_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesEncDecV_VX_WX_2_Data))]
		void Test64_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DC CD", 6, Code.Aesenc_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DC CD", 6, Code.Aesenc_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DC CD", 6, Code.Aesenc_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DD CD", 6, Code.Aesenclast_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DD CD", 6, Code.Aesenclast_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DD CD", 6, Code.Aesenclast_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DE CD", 6, Code.Aesdec_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DE CD", 6, Code.Aesdec_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DE CD", 6, Code.Aesdec_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DF CD", 6, Code.Aesdeclast_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DF CD", 6, Code.Aesdeclast_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DF CD", 6, Code.Aesdeclast_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VX_HX_WX_1_Data))]
		void Test16_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VX_HX_WX_2_Data))]
		void Test16_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VX_HX_WX_1_Data))]
		void Test32_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VX_HX_WX_2_Data))]
		void Test32_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VX_HX_WX_1_Data))]
		void Test64_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VX_HX_WX_2_Data))]
		void Test64_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DC D3", 5, Code.VEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DD D3", 5, Code.VEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DE D3", 5, Code.VEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DF D3", 5, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VY_HY_WY_1_Data))]
		void Test16_VaesdecV_VY_HY_WY_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_VY_HY_WY_1_Data {
			get {
				yield return new object[] { "C4E24D DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VY_HY_WY_2_Data))]
		void Test16_VaesdecV_VY_HY_WY_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_VY_HY_WY_2_Data {
			get {
				yield return new object[] { "C4E24D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VY_HY_WY_1_Data))]
		void Test32_VaesdecV_VY_HY_WY_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_VY_HY_WY_1_Data {
			get {
				yield return new object[] { "C4E24D DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VY_HY_WY_2_Data))]
		void Test32_VaesdecV_VY_HY_WY_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_VY_HY_WY_2_Data {
			get {
				yield return new object[] { "C4E24D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E24D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VY_HY_WY_1_Data))]
		void Test64_VaesdecV_VY_HY_WY_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_VY_HY_WY_1_Data {
			get {
				yield return new object[] { "C4E24D DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DC 10", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DD 10", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DE 10", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };

				yield return new object[] { "C4E24D DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
				yield return new object[] { "C4E2CD DF 10", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VY_HY_WY_2_Data))]
		void Test64_VaesdecV_VY_HY_WY_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_VY_HY_WY_2_Data {
			get {
				yield return new object[] { "C4E24D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4C28D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM11 };
				yield return new object[] { "C4424D DC D3", 5, Code.VEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E24D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4C28D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM11 };
				yield return new object[] { "C4424D DD D3", 5, Code.VEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E24D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4C28D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM11 };
				yield return new object[] { "C4424D DE D3", 5, Code.VEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E24D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4C28D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM11 };
				yield return new object[] { "C4424D DF D3", 5, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesencV_EVEX_VX_HX_WX_1_Data))]
		void Test16_VaesencV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VaesencV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DC 50 01", 7, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DC 50 01", 7, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DC 50 01", 7, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesencV_EVEX_VX_HX_WX_2_Data))]
		void Test16_VaesencV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VaesencV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesencV_EVEX_VX_HX_WX_1_Data))]
		void Test32_VaesencV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VaesencV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DC 50 01", 7, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DC 50 01", 7, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DC 50 01", 7, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesencV_EVEX_VX_HX_WX_2_Data))]
		void Test32_VaesencV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VaesencV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesencV_EVEX_VX_HX_WX_1_Data))]
		void Test64_VaesencV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VaesencV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DC 50 01", 7, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DC 50 01", 7, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DC 50 01", 7, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesencV_EVEX_VX_HX_WX_2_Data))]
		void Test64_VaesencV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VaesencV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D00 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DC D3", 6, Code.EVEX_Vaesenc_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D20 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DC D3", 6, Code.EVEX_Vaesenc_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D40 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DC D3", 6, Code.EVEX_Vaesenc_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesenclastV_EVEX_VX_HX_WX_1_Data))]
		void Test16_VaesenclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VaesenclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DD 50 01", 7, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DD 50 01", 7, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DD 50 01", 7, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesenclastV_EVEX_VX_HX_WX_2_Data))]
		void Test16_VaesenclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VaesenclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesenclastV_EVEX_VX_HX_WX_1_Data))]
		void Test32_VaesenclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VaesenclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DD 50 01", 7, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DD 50 01", 7, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DD 50 01", 7, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesenclastV_EVEX_VX_HX_WX_2_Data))]
		void Test32_VaesenclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VaesenclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesenclastV_EVEX_VX_HX_WX_1_Data))]
		void Test64_VaesenclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VaesenclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DD 50 01", 7, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DD 50 01", 7, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DD 50 01", 7, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesenclastV_EVEX_VX_HX_WX_2_Data))]
		void Test64_VaesenclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VaesenclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D00 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DD D3", 6, Code.EVEX_Vaesenclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D20 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DD D3", 6, Code.EVEX_Vaesenclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D40 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DD D3", 6, Code.EVEX_Vaesenclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_EVEX_VX_HX_WX_1_Data))]
		void Test16_VaesdecV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DE 50 01", 7, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DE 50 01", 7, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DE 50 01", 7, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_EVEX_VX_HX_WX_2_Data))]
		void Test16_VaesdecV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_EVEX_VX_HX_WX_1_Data))]
		void Test32_VaesdecV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DE 50 01", 7, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DE 50 01", 7, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DE 50 01", 7, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_EVEX_VX_HX_WX_2_Data))]
		void Test32_VaesdecV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_EVEX_VX_HX_WX_1_Data))]
		void Test64_VaesdecV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DE 50 01", 7, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DE 50 01", 7, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DE 50 01", 7, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_EVEX_VX_HX_WX_2_Data))]
		void Test64_VaesdecV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D00 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DE D3", 6, Code.EVEX_Vaesdec_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D20 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DE D3", 6, Code.EVEX_Vaesdec_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D40 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DE D3", 6, Code.EVEX_Vaesdec_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdeclastV_EVEX_VX_HX_WX_1_Data))]
		void Test16_VaesdeclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VaesdeclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DF 50 01", 7, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DF 50 01", 7, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DF 50 01", 7, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdeclastV_EVEX_VX_HX_WX_2_Data))]
		void Test16_VaesdeclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VaesdeclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdeclastV_EVEX_VX_HX_WX_1_Data))]
		void Test32_VaesdeclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VaesdeclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DF 50 01", 7, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DF 50 01", 7, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DF 50 01", 7, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdeclastV_EVEX_VX_HX_WX_2_Data))]
		void Test32_VaesdeclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VaesdeclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdeclastV_EVEX_VX_HX_WX_1_Data))]
		void Test64_VaesdeclastV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VaesdeclastV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D08 DF 50 01", 7, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F24D28 DF 50 01", 7, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false };

				yield return new object[] { "62 F24D48 DF 50 01", 7, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdeclastV_EVEX_VX_HX_WX_2_Data))]
		void Test64_VaesdeclastV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VaesdeclastV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D00 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD08 DF D3", 6, Code.EVEX_Vaesdeclast_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D20 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD28 DF D3", 6, Code.EVEX_Vaesdeclast_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 124D40 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD48 DF D3", 6, Code.EVEX_Vaesdeclast_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}
	}
}
