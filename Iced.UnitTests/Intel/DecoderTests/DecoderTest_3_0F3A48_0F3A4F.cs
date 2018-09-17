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
	public sealed class DecoderTest_3_0F3A48_0F3A4F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vpermil2pV_W0_1_Data))]
		void Test16_Vpermil2pV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4, byte imm8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpermil2pV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5, 2 };

				yield return new object[] { "C4E349 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermil2pV_W0_2_Data))]
		void Test16_Vpermil2pV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpermil2pV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 48 D3 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 48 D3 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 2 };

				yield return new object[] { "C4E349 49 D3 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 49 D3 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermil2pV_W0_1_Data))]
		void Test32_Vpermil2pV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4, byte imm8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpermil2pV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5, 2 };

				yield return new object[] { "C4E349 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermil2pV_W0_2_Data))]
		void Test32_Vpermil2pV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpermil2pV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 48 D3 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 48 D3 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 2 };

				yield return new object[] { "C4E349 49 D3 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 49 D3 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermil2pV_W0_1_Data))]
		void Test64_Vpermil2pV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4, byte imm8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpermil2pV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5, 2 };

				yield return new object[] { "C4E349 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4, 1 };
				yield return new object[] { "C4E34D 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermil2pV_W0_2_Data))]
		void Test64_Vpermil2pV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpermil2pV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 48 D3 40", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 0 };
				yield return new object[] { "C4E34D 48 D3 50", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 0 };
				yield return new object[] { "C46349 48 D3 C1", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12, 1 };
				yield return new object[] { "C4634D 48 D3 D1", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13, 1 };
				yield return new object[] { "C4E309 48 D3 E2", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14, 2 };
				yield return new object[] { "C4E30D 48 D3 82", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8, 2 };
				yield return new object[] { "C4C349 48 D3 43", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4, 3 };
				yield return new object[] { "C4C34D 48 D3 53", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5, 3 };

				yield return new object[] { "C4E349 49 D3 40", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4, 0 };
				yield return new object[] { "C4E34D 49 D3 50", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5, 0 };
				yield return new object[] { "C46349 49 D3 C1", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12, 1 };
				yield return new object[] { "C4634D 49 D3 D1", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13, 1 };
				yield return new object[] { "C4E309 49 D3 E2", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14, 2 };
				yield return new object[] { "C4E30D 49 D3 82", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8, 2 };
				yield return new object[] { "C4C349 49 D3 43", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4, 3 };
				yield return new object[] { "C4C34D 49 D3 53", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5, 3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermil2pV_W1_1_Data))]
		void Test16_Vpermil2pV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize, byte imm8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpermil2pV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32, 1 };
				yield return new object[] { "C4E3CD 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32, 2 };

				yield return new object[] { "C4E3C9 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64, 1 };
				yield return new object[] { "C4E3CD 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermil2pV_W1_2_Data))]
		void Test16_Vpermil2pV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpermil2pV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 48 D3 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 1 };
				yield return new object[] { "C4E3CD 48 D3 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 2 };

				yield return new object[] { "C4E3C9 49 D3 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 1 };
				yield return new object[] { "C4E3CD 49 D3 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermil2pV_W1_1_Data))]
		void Test32_Vpermil2pV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize, byte imm8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpermil2pV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32, 1 };
				yield return new object[] { "C4E3CD 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32, 2 };

				yield return new object[] { "C4E3C9 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64, 1 };
				yield return new object[] { "C4E3CD 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermil2pV_W1_2_Data))]
		void Test32_Vpermil2pV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpermil2pV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 48 D3 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 1 };
				yield return new object[] { "C4E3CD 48 D3 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 2 };

				yield return new object[] { "C4E3C9 49 D3 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 1 };
				yield return new object[] { "C4E3CD 49 D3 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermil2pV_W1_1_Data))]
		void Test64_Vpermil2pV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize, byte imm8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpermil2pV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 48 10 41", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32, 1 };
				yield return new object[] { "C4E3CD 48 10 52", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32, 2 };

				yield return new object[] { "C4E3C9 49 10 41", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64, 1 };
				yield return new object[] { "C4E3CD 49 10 52", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64, 2 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermil2pV_W1_2_Data))]
		void Test64_Vpermil2pV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4, byte imm8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(5, instr.OpCount);
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);

			Assert.Equal(OpKind.Immediate8, instr.Op4Kind);
			Assert.Equal(imm8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpermil2pV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 48 D3 40", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 0 };
				yield return new object[] { "C4E3CD 48 D3 50", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 0 };
				yield return new object[] { "C463C9 48 D3 C1", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3, 1 };
				yield return new object[] { "C463CD 48 D3 D1", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3, 1 };
				yield return new object[] { "C4E389 48 D3 E2", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3, 2 };
				yield return new object[] { "C4E38D 48 D3 82", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3, 2 };
				yield return new object[] { "C4C3C9 48 D3 43", 6, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11, 3 };
				yield return new object[] { "C4C3CD 48 D3 53", 6, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11, 3 };

				yield return new object[] { "C4E3C9 49 D3 40", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3, 0 };
				yield return new object[] { "C4E3CD 49 D3 50", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3, 0 };
				yield return new object[] { "C463C9 49 D3 C1", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3, 1 };
				yield return new object[] { "C463CD 49 D3 D1", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3, 1 };
				yield return new object[] { "C4E389 49 D3 E2", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3, 2 };
				yield return new object[] { "C4E38D 49 D3 82", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3, 2 };
				yield return new object[] { "C4C3C9 49 D3 43", 6, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11, 3 };
				yield return new object[] { "C4C3CD 49 D3 53", 6, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11, 3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendvV_VX_HX_WX_Is4X_1_Data))]
		void Test16_VblendvV_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_VblendvV_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "C4E349 4A 10 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 4A 10 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 4B 10 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 4B 10 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 4C 10 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, Register.XMM4 };
				yield return new object[] { "C4E34D 4C 10 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VblendvV_VX_HX_WX_Is4X_2_Data))]
		void Test16_VblendvV_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_VblendvV_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "C4E349 4A D3 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4A D3 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 4B D3 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4B D3 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 4C D3 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4C D3 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendvV_VX_HX_WX_Is4X_1_Data))]
		void Test32_VblendvV_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_VblendvV_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "C4E349 4A 10 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 4A 10 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 4B 10 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 4B 10 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 4C 10 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, Register.XMM4 };
				yield return new object[] { "C4E34D 4C 10 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VblendvV_VX_HX_WX_Is4X_2_Data))]
		void Test32_VblendvV_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_VblendvV_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "C4E349 4A D3 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4A D3 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 4B D3 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4B D3 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 4C D3 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4C D3 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendvV_VX_HX_WX_Is4X_1_Data))]
		void Test64_VblendvV_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_VblendvV_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "C4E349 4A 10 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 4A 10 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 4B 10 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 4B 10 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 4C 10 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, Register.XMM4 };
				yield return new object[] { "C4E34D 4C 10 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VblendvV_VX_HX_WX_Is4X_2_Data))]
		void Test64_VblendvV_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_VblendvV_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "C4E349 4A D3 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4A D3 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 4A D3 C0", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 4A D3 D0", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 4A D3 E0", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 4A D3 80", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 4A D3 40", 6, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 4A D3 50", 6, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 4B D3 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4B D3 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 4B D3 C0", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 4B D3 D0", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 4B D3 E0", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 4B D3 80", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 4B D3 40", 6, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 4B D3 50", 6, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 4C D3 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 4C D3 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 4C D3 C0", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 4C D3 D0", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 4C D3 E0", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 4C D3 80", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 4C D3 40", 6, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 4C D3 50", 6, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };
			}
		}
	}
}
