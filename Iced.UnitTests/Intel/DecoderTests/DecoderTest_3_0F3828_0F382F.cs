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
	public sealed class DecoderTest_3_0F3828_0F382F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PmuldqV_VX_WX_1_Data))]
		void Test16_PmuldqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmuldqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3828 08", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmuldqV_VX_WX_2_Data))]
		void Test16_PmuldqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmuldqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3828 CD", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmuldqV_VX_WX_1_Data))]
		void Test32_PmuldqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmuldqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3828 08", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmuldqV_VX_WX_2_Data))]
		void Test32_PmuldqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmuldqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3828 CD", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmuldqV_VX_WX_1_Data))]
		void Test64_PmuldqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmuldqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3828 08", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmuldqV_VX_WX_2_Data))]
		void Test64_PmuldqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmuldqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3828 CD", 5, Code.Pmuldq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3828 CD", 6, Code.Pmuldq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3828 CD", 6, Code.Pmuldq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3828 CD", 6, Code.Pmuldq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuldqV_VX_HX_WX_1_Data))]
		void Test16_VpmuldqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmuldqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuldqV_VX_HX_WX_2_Data))]
		void Test16_VpmuldqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmuldqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuldqV_VX_HX_WX_1_Data))]
		void Test32_VpmuldqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmuldqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuldqV_VX_HX_WX_2_Data))]
		void Test32_VpmuldqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmuldqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuldqV_VX_HX_WX_1_Data))]
		void Test64_VpmuldqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmuldqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 28 10", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 28 10", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuldqV_VX_HX_WX_2_Data))]
		void Test64_VpmuldqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmuldqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 28 D3", 5, Code.VEX_Vpmuldq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 28 D3", 5, Code.VEX_Vpmuldq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuldqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmuldqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmuldqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F2CD9D 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xInt32, 8, true };
				yield return new object[] { "62 F2CD08 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F2CD2B 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F2CDBD 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xInt32, 8, true };
				yield return new object[] { "62 F2CD28 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F2CD4B 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F2CDDD 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xInt32, 8, true };
				yield return new object[] { "62 F2CD48 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuldqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmuldqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmuldqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuldqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmuldqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmuldqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F2CD9D 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xInt32, 8, true };
				yield return new object[] { "62 F2CD08 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F2CD2B 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F2CDBD 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xInt32, 8, true };
				yield return new object[] { "62 F2CD28 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F2CD4B 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F2CDDD 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xInt32, 8, true };
				yield return new object[] { "62 F2CD48 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuldqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmuldqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmuldqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuldqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmuldqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmuldqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F2CD9D 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xInt32, 8, true };
				yield return new object[] { "62 F2CD08 28 50 01", 7, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F2CD2B 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F2CDBD 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xInt32, 8, true };
				yield return new object[] { "62 F2CD28 28 50 01", 7, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F2CD4B 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F2CDDD 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xInt32, 8, true };
				yield return new object[] { "62 F2CD48 28 50 01", 7, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuldqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmuldqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmuldqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 28 D3", 6, Code.EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 28 D3", 6, Code.EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 28 D3", 6, Code.EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmovm2V_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vpmovm2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 28 D3", 6, Code.EVEX_Vpmovm2b_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 28 D3", 6, Code.EVEX_Vpmovm2b_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 28 D3", 6, Code.EVEX_Vpmovm2b_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 28 D3", 6, Code.EVEX_Vpmovm2w_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 28 D3", 6, Code.EVEX_Vpmovm2w_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 28 D3", 6, Code.EVEX_Vpmovm2w_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmovm2V_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vpmovm2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 28 D3", 6, Code.EVEX_Vpmovm2b_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 28 D3", 6, Code.EVEX_Vpmovm2b_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 28 D3", 6, Code.EVEX_Vpmovm2b_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 28 D3", 6, Code.EVEX_Vpmovm2w_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 28 D3", 6, Code.EVEX_Vpmovm2w_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 28 D3", 6, Code.EVEX_Vpmovm2w_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmovm2V_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vpmovm2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 28 D3", 6, Code.EVEX_Vpmovm2b_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E08 28 D3", 6, Code.EVEX_Vpmovm2b_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E08 28 D3", 6, Code.EVEX_Vpmovm2b_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 28 D3", 6, Code.EVEX_Vpmovm2b_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E28 28 D3", 6, Code.EVEX_Vpmovm2b_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E28 28 D3", 6, Code.EVEX_Vpmovm2b_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 28 D3", 6, Code.EVEX_Vpmovm2b_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E48 28 D3", 6, Code.EVEX_Vpmovm2b_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E48 28 D3", 6, Code.EVEX_Vpmovm2b_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 28 D3", 6, Code.EVEX_Vpmovm2w_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE08 28 D3", 6, Code.EVEX_Vpmovm2w_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE08 28 D3", 6, Code.EVEX_Vpmovm2w_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 28 D3", 6, Code.EVEX_Vpmovm2w_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE28 28 D3", 6, Code.EVEX_Vpmovm2w_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE28 28 D3", 6, Code.EVEX_Vpmovm2w_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 28 D3", 6, Code.EVEX_Vpmovm2w_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE48 28 D3", 6, Code.EVEX_Vpmovm2w_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE48 28 D3", 6, Code.EVEX_Vpmovm2w_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmovb2V_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vpmovb2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpmovb2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 29 D3", 6, Code.EVEX_Vpmovb2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 29 D3", 6, Code.EVEX_Vpmovb2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 29 D3", 6, Code.EVEX_Vpmovb2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 29 D3", 6, Code.EVEX_Vpmovw2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 29 D3", 6, Code.EVEX_Vpmovw2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 29 D3", 6, Code.EVEX_Vpmovw2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmovb2V_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vpmovb2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpmovb2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 29 D3", 6, Code.EVEX_Vpmovb2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 29 D3", 6, Code.EVEX_Vpmovb2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 29 D3", 6, Code.EVEX_Vpmovb2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 29 D3", 6, Code.EVEX_Vpmovw2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 29 D3", 6, Code.EVEX_Vpmovw2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 29 D3", 6, Code.EVEX_Vpmovw2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmovb2V_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vpmovb2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpmovb2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 29 D3", 6, Code.EVEX_Vpmovb2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E08 29 D3", 6, Code.EVEX_Vpmovb2m_k_xmm, Register.K2, Register.XMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E08 29 D3", 6, Code.EVEX_Vpmovb2m_k_xmm, Register.K2, Register.XMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 29 D3", 6, Code.EVEX_Vpmovb2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E28 29 D3", 6, Code.EVEX_Vpmovb2m_k_ymm, Register.K2, Register.YMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E28 29 D3", 6, Code.EVEX_Vpmovb2m_k_ymm, Register.K2, Register.YMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 29 D3", 6, Code.EVEX_Vpmovb2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E48 29 D3", 6, Code.EVEX_Vpmovb2m_k_zmm, Register.K2, Register.ZMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E48 29 D3", 6, Code.EVEX_Vpmovb2m_k_zmm, Register.K2, Register.ZMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 29 D3", 6, Code.EVEX_Vpmovw2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE08 29 D3", 6, Code.EVEX_Vpmovw2m_k_xmm, Register.K2, Register.XMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE08 29 D3", 6, Code.EVEX_Vpmovw2m_k_xmm, Register.K2, Register.XMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 29 D3", 6, Code.EVEX_Vpmovw2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE28 29 D3", 6, Code.EVEX_Vpmovw2m_k_ymm, Register.K2, Register.YMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE28 29 D3", 6, Code.EVEX_Vpmovw2m_k_ymm, Register.K2, Register.YMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 29 D3", 6, Code.EVEX_Vpmovw2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE48 29 D3", 6, Code.EVEX_Vpmovw2m_k_zmm, Register.K2, Register.ZMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE48 29 D3", 6, Code.EVEX_Vpmovw2m_k_zmm, Register.K2, Register.ZMM19, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqqV_VX_WX_1_Data))]
		void Test16_PcmpeqqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PcmpeqqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3829 08", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqqV_VX_WX_2_Data))]
		void Test16_PcmpeqqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PcmpeqqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3829 CD", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqqV_VX_WX_1_Data))]
		void Test32_PcmpeqqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PcmpeqqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3829 08", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqqV_VX_WX_2_Data))]
		void Test32_PcmpeqqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PcmpeqqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3829 CD", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqqV_VX_WX_1_Data))]
		void Test64_PcmpeqqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PcmpeqqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3829 08", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqqV_VX_WX_2_Data))]
		void Test64_PcmpeqqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PcmpeqqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3829 CD", 5, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3829 CD", 6, Code.Pcmpeqq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3829 CD", 6, Code.Pcmpeqq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3829 CD", 6, Code.Pcmpeqq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqqV_VX_HX_WX_1_Data))]
		void Test16_VpcmpeqqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpcmpeqqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqqV_VX_HX_WX_2_Data))]
		void Test16_VpcmpeqqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpcmpeqqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqqV_VX_HX_WX_1_Data))]
		void Test32_VpcmpeqqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpcmpeqqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqqV_VX_HX_WX_2_Data))]
		void Test32_VpcmpeqqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpcmpeqqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqqV_VX_HX_WX_1_Data))]
		void Test64_VpcmpeqqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpcmpeqqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 29 10", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 29 10", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqqV_VX_HX_WX_2_Data))]
		void Test64_VpcmpeqqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpcmpeqqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 29 D3", 5, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 29 D3", 5, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqqV_K_k1_HX_WX_1_Data))]
		void Test16_VpcmpeqqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpcmpeqqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqqV_K_k1_HX_WX_2_Data))]
		void Test16_VpcmpeqqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpcmpeqqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F2CD08 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F2CD28 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F2CD48 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqqV_K_k1_HX_WX_1_Data))]
		void Test32_VpcmpeqqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpcmpeqqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqqV_K_k1_HX_WX_2_Data))]
		void Test32_VpcmpeqqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpcmpeqqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F2CD08 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F2CD28 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F2CD48 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqqV_K_k1_HX_WX_1_Data))]
		void Test64_VpcmpeqqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpcmpeqqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 29 50 01", 7, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqqV_K_k1_HX_WX_2_Data))]
		void Test64_VpcmpeqqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpcmpeqqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F28D0B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 92CD03 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD08 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F28D2B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 92CD23 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD28 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F28D4B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 92CD43 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD48 29 D3", 6, Code.EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovntdqaV_Reg_RegMem_1_Data))]
		void Test16_VmovntdqaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmovntdqaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F382A 08", 5, Code.Movntdqa_xmm_m128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovntdqaV_Reg_RegMem_1_Data))]
		void Test32_VmovntdqaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmovntdqaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F382A 08", 5, Code.Movntdqa_xmm_m128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovntdqaV_Reg_RegMem_1_Data))]
		void Test64_VmovntdqaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmovntdqaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F382A 08", 5, Code.Movntdqa_xmm_m128, Register.XMM1, MemorySize.UInt128 };
				yield return new object[] { "66 44 0F382A 08", 6, Code.Movntdqa_xmm_m128, Register.XMM9, MemorySize.UInt128 };

				yield return new object[] { "C4E279 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C46279 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM10, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 2A 10", 5, Code.VEX_Vmovntdqa_xmm_m128, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4627D 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM10, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 2A 10", 5, Code.VEX_Vmovntdqa_ymm_m256, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovntdqaV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VmovntdqaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VmovntdqaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 2A 50 01", 7, Code.EVEX_Vmovntdqa_xmm_m128, Register.XMM2, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F27D28 2A 50 01", 7, Code.EVEX_Vmovntdqa_ymm_m256, Register.YMM2, Register.None, MemorySize.UInt256, 32, false };

				yield return new object[] { "62 F27D48 2A 50 01", 7, Code.EVEX_Vmovntdqa_zmm_m512, Register.ZMM2, Register.None, MemorySize.UInt512, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovntdqaV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VmovntdqaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VmovntdqaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 2A 50 01", 7, Code.EVEX_Vmovntdqa_xmm_m128, Register.XMM2, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F27D28 2A 50 01", 7, Code.EVEX_Vmovntdqa_ymm_m256, Register.YMM2, Register.None, MemorySize.UInt256, 32, false };

				yield return new object[] { "62 F27D48 2A 50 01", 7, Code.EVEX_Vmovntdqa_zmm_m512, Register.ZMM2, Register.None, MemorySize.UInt512, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovntdqaV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VmovntdqaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VmovntdqaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 2A 50 01", 7, Code.EVEX_Vmovntdqa_xmm_m128, Register.XMM2, Register.None, MemorySize.UInt128, 16, false };
				yield return new object[] { "62 727D08 2A 50 01", 7, Code.EVEX_Vmovntdqa_xmm_m128, Register.XMM10, Register.None, MemorySize.UInt128, 16, false };
				yield return new object[] { "62 627D08 2A 50 01", 7, Code.EVEX_Vmovntdqa_xmm_m128, Register.XMM26, Register.None, MemorySize.UInt128, 16, false };

				yield return new object[] { "62 F27D28 2A 50 01", 7, Code.EVEX_Vmovntdqa_ymm_m256, Register.YMM2, Register.None, MemorySize.UInt256, 32, false };
				yield return new object[] { "62 727D28 2A 50 01", 7, Code.EVEX_Vmovntdqa_ymm_m256, Register.YMM10, Register.None, MemorySize.UInt256, 32, false };
				yield return new object[] { "62 627D28 2A 50 01", 7, Code.EVEX_Vmovntdqa_ymm_m256, Register.YMM26, Register.None, MemorySize.UInt256, 32, false };

				yield return new object[] { "62 F27D48 2A 50 01", 7, Code.EVEX_Vmovntdqa_zmm_m512, Register.ZMM2, Register.None, MemorySize.UInt512, 64, false };
				yield return new object[] { "62 727D48 2A 50 01", 7, Code.EVEX_Vmovntdqa_zmm_m512, Register.ZMM10, Register.None, MemorySize.UInt512, 64, false };
				yield return new object[] { "62 627D48 2A 50 01", 7, Code.EVEX_Vmovntdqa_zmm_m512, Register.ZMM26, Register.None, MemorySize.UInt512, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F2FE08 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F2FE08 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpbroadcastmb2qV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F2FE08 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE08 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE08 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE28 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE28 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE48 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE48 2A D3", 6, Code.EVEX_Vpbroadcastmb2q_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PackusdwV_VX_WX_1_Data))]
		void Test16_PackusdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PackusdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F382B 08", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PackusdwV_VX_WX_2_Data))]
		void Test16_PackusdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PackusdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F382B CD", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PackusdwV_VX_WX_1_Data))]
		void Test32_PackusdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PackusdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F382B 08", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PackusdwV_VX_WX_2_Data))]
		void Test32_PackusdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PackusdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F382B CD", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PackusdwV_VX_WX_1_Data))]
		void Test64_PackusdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PackusdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F382B 08", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PackusdwV_VX_WX_2_Data))]
		void Test64_PackusdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PackusdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F382B CD", 5, Code.Packusdw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F382B CD", 6, Code.Packusdw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F382B CD", 6, Code.Packusdw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F382B CD", 6, Code.Packusdw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackusdwV_VX_HX_WX_1_Data))]
		void Test16_VpackusdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpackusdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackusdwV_VX_HX_WX_2_Data))]
		void Test16_VpackusdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpackusdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackusdwV_VX_HX_WX_1_Data))]
		void Test32_VpackusdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpackusdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackusdwV_VX_HX_WX_2_Data))]
		void Test32_VpackusdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpackusdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackusdwV_VX_HX_WX_1_Data))]
		void Test64_VpackusdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpackusdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2C9 2B 10", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E24D 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2CD 2B 10", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackusdwV_VX_HX_WX_2_Data))]
		void Test64_VpackusdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpackusdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 2B D3", 5, Code.VEX_Vpackusdw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 2B D3", 5, Code.VEX_Vpackusdw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackusdwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpackusdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpackusdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackusdwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpackusdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpackusdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackusdwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpackusdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpackusdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackusdwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpackusdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpackusdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackusdwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpackusdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpackusdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 2B 50 01", 7, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 2B 50 01", 7, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 2B 50 01", 7, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackusdwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpackusdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpackusdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 2B D3", 6, Code.EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 2B D3", 6, Code.EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 2B D3", 6, Code.EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaskmovpsV_VX_HX_WX_1_Data))]
		void Test16_VmaskmovpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmaskmovpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2C 10", 5, Code.VEX_Vmaskmovps_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2C 10", 5, Code.VEX_Vmaskmovps_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaskmovpsV_VX_HX_WX_1_Data))]
		void Test32_VmaskmovpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmaskmovpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2C 10", 5, Code.VEX_Vmaskmovps_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2C 10", 5, Code.VEX_Vmaskmovps_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaskmovpsV_VX_HX_WX_1_Data))]
		void Test64_VmaskmovpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmaskmovpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2C 10", 5, Code.VEX_Vmaskmovps_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C46249 2C 10", 5, Code.VEX_Vmaskmovps_xmm_xmm_m128, Register.XMM10, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C46209 2C 10", 5, Code.VEX_Vmaskmovps_xmm_xmm_m128, Register.XMM10, Register.XMM14, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2C 10", 5, Code.VEX_Vmaskmovps_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4624D 2C 10", 5, Code.VEX_Vmaskmovps_ymm_ymm_m256, Register.YMM10, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4620D 2C 10", 5, Code.VEX_Vmaskmovps_ymm_ymm_m256, Register.YMM10, Register.YMM14, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VscalefpV_VX_k1_HX_WX_1_Data))]
		void Test16_VscalefpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VscalefpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VscalefpV_VX_k1_HX_WX_2_Data))]
		void Test16_VscalefpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VscalefpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VscalefpV_VX_k1_HX_WX_1_Data))]
		void Test32_VscalefpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VscalefpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VscalefpV_VX_k1_HX_WX_2_Data))]
		void Test32_VscalefpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VscalefpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VscalefpV_VX_k1_HX_WX_1_Data))]
		void Test64_VscalefpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VscalefpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 2C 50 01", 7, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 2C 50 01", 7, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 2C 50 01", 7, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 2C 50 01", 7, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 2C 50 01", 7, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 2C 50 01", 7, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VscalefpV_VX_k1_HX_WX_2_Data))]
		void Test64_VscalefpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VscalefpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 2C D3", 6, Code.EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 2C D3", 6, Code.EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 2C D3", 6, Code.EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 2C D3", 6, Code.EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 2C D3", 6, Code.EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 2C D3", 6, Code.EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaskmovpdV_VX_HX_WX_1_Data))]
		void Test16_VmaskmovpdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmaskmovpdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2D 10", 5, Code.VEX_Vmaskmovpd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2D 10", 5, Code.VEX_Vmaskmovpd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaskmovpdV_VX_HX_WX_1_Data))]
		void Test32_VmaskmovpdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmaskmovpdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2D 10", 5, Code.VEX_Vmaskmovpd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2D 10", 5, Code.VEX_Vmaskmovpd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaskmovpdV_VX_HX_WX_1_Data))]
		void Test64_VmaskmovpdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmaskmovpdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 2D 10", 5, Code.VEX_Vmaskmovpd_xmm_xmm_m128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C46249 2D 10", 5, Code.VEX_Vmaskmovpd_xmm_xmm_m128, Register.XMM10, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C46209 2D 10", 5, Code.VEX_Vmaskmovpd_xmm_xmm_m128, Register.XMM10, Register.XMM14, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2D 10", 5, Code.VEX_Vmaskmovpd_ymm_ymm_m256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4624D 2D 10", 5, Code.VEX_Vmaskmovpd_ymm_ymm_m256, Register.YMM10, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4620D 2D 10", 5, Code.VEX_Vmaskmovpd_ymm_ymm_m256, Register.YMM10, Register.YMM14, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VscalefsV_VX_k1_HX_WX_1_Data))]
		void Test16_VscalefsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VscalefsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VscalefsV_VX_k1_HX_WX_2_Data))]
		void Test16_VscalefsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VscalefsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CD8B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VscalefsV_VX_k1_HX_WX_1_Data))]
		void Test32_VscalefsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VscalefsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VscalefsV_VX_k1_HX_WX_2_Data))]
		void Test32_VscalefsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VscalefsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CD8B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VscalefsV_VX_k1_HX_WX_1_Data))]
		void Test64_VscalefsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VscalefsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 2D 50 01", 7, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 2D 50 01", 7, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VscalefsV_VX_k1_HX_WX_2_Data))]
		void Test64_VscalefsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VscalefsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E20D0B 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 2D D3", 6, Code.EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CD8B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 2D D3", 6, Code.EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaskmovpsV_WX_HX_VX_1_Data))]
		void Test16_VmaskmovpsV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmaskmovpsV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2E 10", 5, Code.VEX_Vmaskmovps_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2E 10", 5, Code.VEX_Vmaskmovps_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaskmovpsV_WX_HX_VX_1_Data))]
		void Test32_VmaskmovpsV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmaskmovpsV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2E 10", 5, Code.VEX_Vmaskmovps_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2E 10", 5, Code.VEX_Vmaskmovps_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaskmovpsV_WX_HX_VX_1_Data))]
		void Test64_VmaskmovpsV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmaskmovpsV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2E 10", 5, Code.VEX_Vmaskmovps_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C46249 2E 10", 5, Code.VEX_Vmaskmovps_m128_xmm_xmm, Register.XMM6, Register.XMM10, MemorySize.Packed128_Float32 };
				yield return new object[] { "C46209 2E 10", 5, Code.VEX_Vmaskmovps_m128_xmm_xmm, Register.XMM14, Register.XMM10, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 2E 10", 5, Code.VEX_Vmaskmovps_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4624D 2E 10", 5, Code.VEX_Vmaskmovps_m256_ymm_ymm, Register.YMM6, Register.YMM10, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4620D 2E 10", 5, Code.VEX_Vmaskmovps_m256_ymm_ymm, Register.YMM14, Register.YMM10, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaskmovpdV_WX_HX_VX_1_Data))]
		void Test16_VmaskmovpdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmaskmovpdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2F 10", 5, Code.VEX_Vmaskmovpd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2F 10", 5, Code.VEX_Vmaskmovpd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaskmovpdV_WX_HX_VX_1_Data))]
		void Test32_VmaskmovpdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmaskmovpdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2F 10", 5, Code.VEX_Vmaskmovpd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2F 10", 5, Code.VEX_Vmaskmovpd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaskmovpdV_WX_HX_VX_1_Data))]
		void Test64_VmaskmovpdV_WX_HX_VX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmaskmovpdV_WX_HX_VX_1_Data {
			get {
				yield return new object[] { "C4E249 2F 10", 5, Code.VEX_Vmaskmovpd_m128_xmm_xmm, Register.XMM6, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C46249 2F 10", 5, Code.VEX_Vmaskmovpd_m128_xmm_xmm, Register.XMM6, Register.XMM10, MemorySize.Packed128_Float64 };
				yield return new object[] { "C46209 2F 10", 5, Code.VEX_Vmaskmovpd_m128_xmm_xmm, Register.XMM14, Register.XMM10, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E24D 2F 10", 5, Code.VEX_Vmaskmovpd_m256_ymm_ymm, Register.YMM6, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4624D 2F 10", 5, Code.VEX_Vmaskmovpd_m256_ymm_ymm, Register.YMM6, Register.YMM10, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4620D 2F 10", 5, Code.VEX_Vmaskmovpd_m256_ymm_ymm, Register.YMM14, Register.YMM10, MemorySize.Packed256_Float64 };
			}
		}
	}
}
