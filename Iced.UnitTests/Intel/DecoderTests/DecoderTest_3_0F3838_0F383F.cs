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
	public sealed class DecoderTest_3_0F3838_0F383F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PminsbV_VX_WX_1_Data))]
		void Test16_PminsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PminsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3838 08", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminsbV_VX_WX_2_Data))]
		void Test16_PminsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PminsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3838 CD", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminsbV_VX_WX_1_Data))]
		void Test32_PminsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PminsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3838 08", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminsbV_VX_WX_2_Data))]
		void Test32_PminsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PminsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3838 CD", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminsbV_VX_WX_1_Data))]
		void Test64_PminsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PminsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3838 08", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminsbV_VX_WX_2_Data))]
		void Test64_PminsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PminsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3838 CD", 5, Code.Pminsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3838 CD", 6, Code.Pminsb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3838 CD", 6, Code.Pminsb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3838 CD", 6, Code.Pminsb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsbV_VX_HX_WX_1_Data))]
		void Test16_VpminsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpminsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsbV_VX_HX_WX_2_Data))]
		void Test16_VpminsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpminsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsbV_VX_HX_WX_1_Data))]
		void Test32_VpminsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpminsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsbV_VX_HX_WX_2_Data))]
		void Test32_VpminsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpminsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsbV_VX_HX_WX_1_Data))]
		void Test64_VpminsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpminsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 38 10", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 38 10", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsbV_VX_HX_WX_2_Data))]
		void Test64_VpminsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpminsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 38 D3", 5, Code.VEX_Vpminsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 38 D3", 5, Code.VEX_Vpminsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpminsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpminsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpminsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpminsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpminsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpminsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpminsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 38 50 01", 7, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 38 50 01", 7, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 38 50 01", 7, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpminsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpminsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 38 D3", 6, Code.EVEX_Vpminsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 38 D3", 6, Code.EVEX_Vpminsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 38 D3", 6, Code.EVEX_Vpminsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
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
		public static IEnumerable<object[]> Test16_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 38 D3", 6, Code.EVEX_Vpmovm2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 38 D3", 6, Code.EVEX_Vpmovm2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 38 D3", 6, Code.EVEX_Vpmovm2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 38 D3", 6, Code.EVEX_Vpmovm2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 38 D3", 6, Code.EVEX_Vpmovm2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 38 D3", 6, Code.EVEX_Vpmovm2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
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
		public static IEnumerable<object[]> Test32_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 38 D3", 6, Code.EVEX_Vpmovm2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 38 D3", 6, Code.EVEX_Vpmovm2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 38 D3", 6, Code.EVEX_Vpmovm2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 38 D3", 6, Code.EVEX_Vpmovm2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 38 D3", 6, Code.EVEX_Vpmovm2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 38 D3", 6, Code.EVEX_Vpmovm2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
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
		public static IEnumerable<object[]> Test64_Vpmovm2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 38 D3", 6, Code.EVEX_Vpmovm2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E08 38 D3", 6, Code.EVEX_Vpmovm2d_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E08 38 D3", 6, Code.EVEX_Vpmovm2d_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 38 D3", 6, Code.EVEX_Vpmovm2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E28 38 D3", 6, Code.EVEX_Vpmovm2d_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E28 38 D3", 6, Code.EVEX_Vpmovm2d_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 38 D3", 6, Code.EVEX_Vpmovm2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E48 38 D3", 6, Code.EVEX_Vpmovm2d_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E48 38 D3", 6, Code.EVEX_Vpmovm2d_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 38 D3", 6, Code.EVEX_Vpmovm2q_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE08 38 D3", 6, Code.EVEX_Vpmovm2q_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE08 38 D3", 6, Code.EVEX_Vpmovm2q_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 38 D3", 6, Code.EVEX_Vpmovm2q_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE28 38 D3", 6, Code.EVEX_Vpmovm2q_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE28 38 D3", 6, Code.EVEX_Vpmovm2q_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 38 D3", 6, Code.EVEX_Vpmovm2q_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 72FE48 38 D3", 6, Code.EVEX_Vpmovm2q_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E2FE48 38 D3", 6, Code.EVEX_Vpmovm2q_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminsdV_VX_WX_1_Data))]
		void Test16_PminsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PminsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3839 08", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminsdV_VX_WX_2_Data))]
		void Test16_PminsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PminsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3839 CD", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminsdV_VX_WX_1_Data))]
		void Test32_PminsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PminsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3839 08", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminsdV_VX_WX_2_Data))]
		void Test32_PminsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PminsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3839 CD", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminsdV_VX_WX_1_Data))]
		void Test64_PminsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PminsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3839 08", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminsdV_VX_WX_2_Data))]
		void Test64_PminsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PminsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3839 CD", 5, Code.Pminsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3839 CD", 6, Code.Pminsd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3839 CD", 6, Code.Pminsd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3839 CD", 6, Code.Pminsd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsdV_VX_HX_WX_1_Data))]
		void Test16_VpminsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpminsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsdV_VX_HX_WX_2_Data))]
		void Test16_VpminsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpminsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsdV_VX_HX_WX_1_Data))]
		void Test32_VpminsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpminsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsdV_VX_HX_WX_2_Data))]
		void Test32_VpminsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpminsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsdV_VX_HX_WX_1_Data))]
		void Test64_VpminsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpminsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 39 10", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 39 10", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsdV_VX_HX_WX_2_Data))]
		void Test64_VpminsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpminsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 39 D3", 5, Code.VEX_Vpminsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 39 D3", 5, Code.VEX_Vpminsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsV_VX_k1_HX_WX_1_Data))]
		void Test16_VpminsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminsV_VX_k1_HX_WX_2_Data))]
		void Test16_VpminsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsV_VX_k1_HX_WX_1_Data))]
		void Test32_VpminsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminsV_VX_k1_HX_WX_2_Data))]
		void Test32_VpminsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsV_VX_k1_HX_WX_1_Data))]
		void Test64_VpminsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 39 50 01", 7, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 39 50 01", 7, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 39 50 01", 7, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 39 50 01", 7, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 39 50 01", 7, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 39 50 01", 7, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminsV_VX_k1_HX_WX_2_Data))]
		void Test64_VpminsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 39 D3", 6, Code.EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 39 D3", 6, Code.EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 39 D3", 6, Code.EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 39 D3", 6, Code.EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 39 D3", 6, Code.EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 39 D3", 6, Code.EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmovd2V_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vpmovd2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpmovd2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 39 D3", 6, Code.EVEX_Vpmovd2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 39 D3", 6, Code.EVEX_Vpmovd2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 39 D3", 6, Code.EVEX_Vpmovd2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 39 D3", 6, Code.EVEX_Vpmovq2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 39 D3", 6, Code.EVEX_Vpmovq2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 39 D3", 6, Code.EVEX_Vpmovq2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmovd2V_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vpmovd2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpmovd2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 39 D3", 6, Code.EVEX_Vpmovd2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 39 D3", 6, Code.EVEX_Vpmovd2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 39 D3", 6, Code.EVEX_Vpmovd2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 39 D3", 6, Code.EVEX_Vpmovq2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 39 D3", 6, Code.EVEX_Vpmovq2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 39 D3", 6, Code.EVEX_Vpmovq2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmovd2V_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vpmovd2V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpmovd2V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 39 D3", 6, Code.EVEX_Vpmovd2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E08 39 D3", 6, Code.EVEX_Vpmovd2m_k_xmm, Register.K2, Register.XMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E08 39 D3", 6, Code.EVEX_Vpmovd2m_k_xmm, Register.K2, Register.XMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 39 D3", 6, Code.EVEX_Vpmovd2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E28 39 D3", 6, Code.EVEX_Vpmovd2m_k_ymm, Register.K2, Register.YMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E28 39 D3", 6, Code.EVEX_Vpmovd2m_k_ymm, Register.K2, Register.YMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 39 D3", 6, Code.EVEX_Vpmovd2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D27E48 39 D3", 6, Code.EVEX_Vpmovd2m_k_zmm, Register.K2, Register.ZMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E48 39 D3", 6, Code.EVEX_Vpmovd2m_k_zmm, Register.K2, Register.ZMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE08 39 D3", 6, Code.EVEX_Vpmovq2m_k_xmm, Register.K2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE08 39 D3", 6, Code.EVEX_Vpmovq2m_k_xmm, Register.K2, Register.XMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE08 39 D3", 6, Code.EVEX_Vpmovq2m_k_xmm, Register.K2, Register.XMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE28 39 D3", 6, Code.EVEX_Vpmovq2m_k_ymm, Register.K2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE28 39 D3", 6, Code.EVEX_Vpmovq2m_k_ymm, Register.K2, Register.YMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE28 39 D3", 6, Code.EVEX_Vpmovq2m_k_ymm, Register.K2, Register.YMM19, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F2FE48 39 D3", 6, Code.EVEX_Vpmovq2m_k_zmm, Register.K2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 D2FE48 39 D3", 6, Code.EVEX_Vpmovq2m_k_zmm, Register.K2, Register.ZMM11, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B2FE48 39 D3", 6, Code.EVEX_Vpmovq2m_k_zmm, Register.K2, Register.ZMM19, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminuwV_VX_WX_1_Data))]
		void Test16_PminuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PminuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383A 08", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminuwV_VX_WX_2_Data))]
		void Test16_PminuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PminuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383A CD", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminuwV_VX_WX_1_Data))]
		void Test32_PminuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PminuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383A 08", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminuwV_VX_WX_2_Data))]
		void Test32_PminuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PminuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383A CD", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminuwV_VX_WX_1_Data))]
		void Test64_PminuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PminuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383A 08", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminuwV_VX_WX_2_Data))]
		void Test64_PminuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PminuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383A CD", 5, Code.Pminuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383A CD", 6, Code.Pminuw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383A CD", 6, Code.Pminuw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383A CD", 6, Code.Pminuw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuwV_VX_HX_WX_1_Data))]
		void Test16_VpminuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpminuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuwV_VX_HX_WX_2_Data))]
		void Test16_VpminuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpminuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuwV_VX_HX_WX_1_Data))]
		void Test32_VpminuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpminuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuwV_VX_HX_WX_2_Data))]
		void Test32_VpminuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpminuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuwV_VX_HX_WX_1_Data))]
		void Test64_VpminuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpminuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3A 10", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3A 10", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuwV_VX_HX_WX_2_Data))]
		void Test64_VpminuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpminuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3A D3", 5, Code.VEX_Vpminuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3A D3", 5, Code.VEX_Vpminuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpminuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpminuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpminuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpminuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpminuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpminuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpminuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3A 50 01", 7, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3A 50 01", 7, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3A 50 01", 7, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpminuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpminuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 3A D3", 6, Code.EVEX_Vpminuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 3A D3", 6, Code.EVEX_Vpminuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 3A D3", 6, Code.EVEX_Vpminuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpbroadcastmw2dVV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27E08 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_xmm_k, Register.XMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E08 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_xmm_k, Register.XMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E08 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_xmm_k, Register.XMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E28 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_ymm_k, Register.YMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E28 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_ymm_k, Register.YMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E28 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_ymm_k, Register.YMM18, Register.K3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E48 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_zmm_k, Register.ZMM2, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 727E48 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_zmm_k, Register.ZMM10, Register.K3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E48 3A D3", 6, Code.EVEX_Vpbroadcastmw2d_zmm_k, Register.ZMM18, Register.K3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminudV_VX_WX_1_Data))]
		void Test16_PminudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PminudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383B 08", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminudV_VX_WX_2_Data))]
		void Test16_PminudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PminudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383B CD", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminudV_VX_WX_1_Data))]
		void Test32_PminudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PminudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383B 08", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminudV_VX_WX_2_Data))]
		void Test32_PminudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PminudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383B CD", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminudV_VX_WX_1_Data))]
		void Test64_PminudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PminudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383B 08", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminudV_VX_WX_2_Data))]
		void Test64_PminudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PminudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383B CD", 5, Code.Pminud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383B CD", 6, Code.Pminud_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383B CD", 6, Code.Pminud_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383B CD", 6, Code.Pminud_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminudV_VX_HX_WX_1_Data))]
		void Test16_VpminudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpminudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminudV_VX_HX_WX_2_Data))]
		void Test16_VpminudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpminudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminudV_VX_HX_WX_1_Data))]
		void Test32_VpminudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpminudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminudV_VX_HX_WX_2_Data))]
		void Test32_VpminudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpminudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminudV_VX_HX_WX_1_Data))]
		void Test64_VpminudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpminudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3B 10", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3B 10", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminudV_VX_HX_WX_2_Data))]
		void Test64_VpminudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpminudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3B D3", 5, Code.VEX_Vpminud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3B D3", 5, Code.VEX_Vpminud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuV_VX_k1_HX_WX_1_Data))]
		void Test16_VpminuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminuV_VX_k1_HX_WX_2_Data))]
		void Test16_VpminuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuV_VX_k1_HX_WX_1_Data))]
		void Test32_VpminuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminuV_VX_k1_HX_WX_2_Data))]
		void Test32_VpminuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuV_VX_k1_HX_WX_1_Data))]
		void Test64_VpminuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3B 50 01", 7, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3B 50 01", 7, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3B 50 01", 7, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3B 50 01", 7, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3B 50 01", 7, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3B 50 01", 7, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminuV_VX_k1_HX_WX_2_Data))]
		void Test64_VpminuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 3B D3", 6, Code.EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 3B D3", 6, Code.EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 3B D3", 6, Code.EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 3B D3", 6, Code.EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 3B D3", 6, Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 3B D3", 6, Code.EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxsbV_VX_WX_1_Data))]
		void Test16_PmaxsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaxsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383C 08", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxsbV_VX_WX_2_Data))]
		void Test16_PmaxsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaxsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383C CD", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxsbV_VX_WX_1_Data))]
		void Test32_PmaxsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaxsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383C 08", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxsbV_VX_WX_2_Data))]
		void Test32_PmaxsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaxsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383C CD", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxsbV_VX_WX_1_Data))]
		void Test64_PmaxsbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaxsbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383C 08", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxsbV_VX_WX_2_Data))]
		void Test64_PmaxsbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaxsbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383C CD", 5, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383C CD", 6, Code.Pmaxsb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383C CD", 6, Code.Pmaxsb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383C CD", 6, Code.Pmaxsb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsbV_VX_HX_WX_1_Data))]
		void Test16_VpmaxsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaxsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsbV_VX_HX_WX_2_Data))]
		void Test16_VpmaxsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaxsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsbV_VX_HX_WX_1_Data))]
		void Test32_VpmaxsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaxsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsbV_VX_HX_WX_2_Data))]
		void Test32_VpmaxsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaxsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsbV_VX_HX_WX_1_Data))]
		void Test64_VpmaxsbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaxsbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 3C 10", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 3C 10", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsbV_VX_HX_WX_2_Data))]
		void Test64_VpmaxsbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaxsbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3C D3", 5, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3C D3", 5, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaxsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaxsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmaxsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaxsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaxsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmaxsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaxsbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxsbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 3C 50 01", 7, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 3C 50 01", 7, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 3C 50 01", 7, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaxsbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmaxsbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 3C D3", 6, Code.EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 3C D3", 6, Code.EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 3C D3", 6, Code.EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxsdV_VX_WX_1_Data))]
		void Test16_PmaxsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaxsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383D 08", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxsdV_VX_WX_2_Data))]
		void Test16_PmaxsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaxsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383D CD", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxsdV_VX_WX_1_Data))]
		void Test32_PmaxsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaxsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383D 08", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxsdV_VX_WX_2_Data))]
		void Test32_PmaxsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaxsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383D CD", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxsdV_VX_WX_1_Data))]
		void Test64_PmaxsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaxsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383D 08", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxsdV_VX_WX_2_Data))]
		void Test64_PmaxsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaxsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383D CD", 5, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383D CD", 6, Code.Pmaxsd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383D CD", 6, Code.Pmaxsd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383D CD", 6, Code.Pmaxsd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsdV_VX_HX_WX_1_Data))]
		void Test16_VpmaxsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaxsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsdV_VX_HX_WX_2_Data))]
		void Test16_VpmaxsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaxsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsdV_VX_HX_WX_1_Data))]
		void Test32_VpmaxsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaxsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsdV_VX_HX_WX_2_Data))]
		void Test32_VpmaxsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaxsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsdV_VX_HX_WX_1_Data))]
		void Test64_VpmaxsdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaxsdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 3D 10", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 3D 10", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsdV_VX_HX_WX_2_Data))]
		void Test64_VpmaxsdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaxsdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3D D3", 5, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3D D3", 5, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaxsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxsV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaxsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaxsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxsV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaxsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaxsV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxsV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 3D 50 01", 7, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 3D 50 01", 7, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 3D 50 01", 7, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 3D 50 01", 7, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 3D 50 01", 7, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 3D 50 01", 7, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxsV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaxsV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxsV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 3D D3", 6, Code.EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 3D D3", 6, Code.EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 3D D3", 6, Code.EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 3D D3", 6, Code.EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 3D D3", 6, Code.EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 3D D3", 6, Code.EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxuwV_VX_WX_1_Data))]
		void Test16_PmaxuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaxuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383E 08", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxuwV_VX_WX_2_Data))]
		void Test16_PmaxuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaxuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383E CD", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxuwV_VX_WX_1_Data))]
		void Test32_PmaxuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaxuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383E 08", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxuwV_VX_WX_2_Data))]
		void Test32_PmaxuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaxuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383E CD", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxuwV_VX_WX_1_Data))]
		void Test64_PmaxuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaxuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383E 08", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxuwV_VX_WX_2_Data))]
		void Test64_PmaxuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaxuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383E CD", 5, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383E CD", 6, Code.Pmaxuw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383E CD", 6, Code.Pmaxuw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383E CD", 6, Code.Pmaxuw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuwV_VX_HX_WX_1_Data))]
		void Test16_VpmaxuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaxuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuwV_VX_HX_WX_2_Data))]
		void Test16_VpmaxuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaxuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuwV_VX_HX_WX_1_Data))]
		void Test32_VpmaxuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaxuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuwV_VX_HX_WX_2_Data))]
		void Test32_VpmaxuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaxuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuwV_VX_HX_WX_1_Data))]
		void Test64_VpmaxuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaxuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E24D 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E2C9 3E 10", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2CD 3E 10", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuwV_VX_HX_WX_2_Data))]
		void Test64_VpmaxuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaxuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3E D3", 5, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3E D3", 5, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaxuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaxuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmaxuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaxuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaxuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmaxuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaxuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F24D8D 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F24D08 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0B 3E 50 01", 7, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F24D2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F24DAD 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F24D28 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2B 3E 50 01", 7, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F24D4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F24DCD 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F24D48 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4B 3E 50 01", 7, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaxuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmaxuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 3E D3", 6, Code.EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 3E D3", 6, Code.EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 3E D3", 6, Code.EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxudV_VX_WX_1_Data))]
		void Test16_PmaxudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaxudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383F 08", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxudV_VX_WX_2_Data))]
		void Test16_PmaxudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaxudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383F CD", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxudV_VX_WX_1_Data))]
		void Test32_PmaxudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaxudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383F 08", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxudV_VX_WX_2_Data))]
		void Test32_PmaxudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaxudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383F CD", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxudV_VX_WX_1_Data))]
		void Test64_PmaxudV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaxudV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F383F 08", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxudV_VX_WX_2_Data))]
		void Test64_PmaxudV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaxudV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F383F CD", 5, Code.Pmaxud_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F383F CD", 6, Code.Pmaxud_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F383F CD", 6, Code.Pmaxud_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F383F CD", 6, Code.Pmaxud_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxudV_VX_HX_WX_1_Data))]
		void Test16_VpmaxudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaxudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxudV_VX_HX_WX_2_Data))]
		void Test16_VpmaxudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaxudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxudV_VX_HX_WX_1_Data))]
		void Test32_VpmaxudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaxudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxudV_VX_HX_WX_2_Data))]
		void Test32_VpmaxudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaxudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxudV_VX_HX_WX_1_Data))]
		void Test64_VpmaxudV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaxudV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E2C9 3F 10", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2CD 3F 10", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxudV_VX_HX_WX_2_Data))]
		void Test64_VpmaxudV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaxudV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 3F D3", 5, Code.VEX_Vpmaxud_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 3F D3", 5, Code.VEX_Vpmaxud_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaxuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxuV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaxuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaxuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxuV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaxuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaxuV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxuV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 3F 50 01", 7, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 3F 50 01", 7, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 3F 50 01", 7, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 3F 50 01", 7, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 3F 50 01", 7, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 3F 50 01", 7, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxuV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaxuV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxuV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 3F D3", 6, Code.EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 3F D3", 6, Code.EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 3F D3", 6, Code.EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 3F D3", 6, Code.EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 3F D3", 6, Code.EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 3F D3", 6, Code.EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
