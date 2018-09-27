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
	public sealed class DecoderTest_3_0F3AC8_0F3ACF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test16_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test16_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test16_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test32_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };

			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test32_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test32_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test64_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test64_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test64_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "44 0F3ACC CD A5", 6, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "41 0F3ACC CD A5", 6, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "45 0F3ACC CD A5", 6, Code.Sha1rnds4_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8affineqbV_VX_WX_Ib_1_Data))]
		void Test16_Gf2p8affineqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Gf2p8affineqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACE 08 A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8affineqbV_VX_WX_Ib_2_Data))]
		void Test16_Gf2p8affineqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test16_Gf2p8affineqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACE CD A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8affineqbV_VX_WX_Ib_1_Data))]
		void Test32_Gf2p8affineqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Gf2p8affineqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACE 08 A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8affineqbV_VX_WX_Ib_2_Data))]
		void Test32_Gf2p8affineqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test32_Gf2p8affineqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACE CD A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8affineqbV_VX_WX_Ib_1_Data))]
		void Test64_Gf2p8affineqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Gf2p8affineqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACE 08 A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8affineqbV_VX_WX_Ib_2_Data))]
		void Test64_Gf2p8affineqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test64_Gf2p8affineqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACE CD A5", 6, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3ACE CD 5A", 7, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3ACE CD A5", 7, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3ACE CD 5A", 7, Code.Gf2p8affineqb_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data))]
		void Test16_Vgf2p8affineqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data))]
		void Test16_Vgf2p8affineqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E3CD CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data))]
		void Test32_Vgf2p8affineqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data))]
		void Test32_Vgf2p8affineqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E3CD CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data))]
		void Test64_Vgf2p8affineqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CE 10 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data))]
		void Test64_Vgf2p8affineqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C463C9 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E389 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C3C9 CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E3CD CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C463CD CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E38D CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C3CD CE D3 A5", 6, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CE 50 01 A5", 8, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B CE D3 A5", 7, Code.EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8affineinvqbV_VX_WX_Ib_1_Data))]
		void Test16_Gf2p8affineinvqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Gf2p8affineinvqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACF 08 A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Gf2p8affineinvqbV_VX_WX_Ib_2_Data))]
		void Test16_Gf2p8affineinvqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test16_Gf2p8affineinvqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACF CD A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8affineinvqbV_VX_WX_Ib_1_Data))]
		void Test32_Gf2p8affineinvqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Gf2p8affineinvqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACF 08 A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Gf2p8affineinvqbV_VX_WX_Ib_2_Data))]
		void Test32_Gf2p8affineinvqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test32_Gf2p8affineinvqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACF CD A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8affineinvqbV_VX_WX_Ib_1_Data))]
		void Test64_Gf2p8affineinvqbV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Gf2p8affineinvqbV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3ACF 08 A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Gf2p8affineinvqbV_VX_WX_Ib_2_Data))]
		void Test64_Gf2p8affineinvqbV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test64_Gf2p8affineinvqbV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3ACF CD A5", 6, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3ACF CD 5A", 7, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3ACF CD A5", 7, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3ACF CD 5A", 7, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data))]
		void Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data))]
		void Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E3CD CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data))]
		void Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data))]
		void Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E3CD CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data))]
		void Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E3C9 CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E3CD CF 10 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data))]
		void Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineinvqb_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E3C9 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C463C9 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E389 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C3C9 CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E3CD CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C463CD CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E38D CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C3CD CF D3 A5", 6, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 CF 50 01 A5", 8, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgf2p8affineinvqbV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD8B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B CF D3 A5", 7, Code.EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}
	}
}
