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
	public sealed class DecoderTest_3_0F3A58_0F3A5F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VfmaddsubpV_W0_1_Data))]
		void Test16_VfmaddsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddsubpV_W0_2_Data))]
		void Test16_VfmaddsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddsubpV_W0_1_Data))]
		void Test32_VfmaddsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddsubpV_W0_2_Data))]
		void Test32_VfmaddsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddsubpV_W0_1_Data))]
		void Test64_VfmaddsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddsubpV_W0_2_Data))]
		void Test64_VfmaddsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 5C D3 C0", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 5C D3 D0", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 5C D3 E0", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 5C D3 80", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 5D D3 C0", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 5D D3 D0", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 5D D3 E0", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 5D D3 80", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddsubpV_W1_1_Data))]
		void Test16_VfmaddsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VfmaddsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddsubpV_W1_2_Data))]
		void Test16_VfmaddsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddsubpV_W1_1_Data))]
		void Test32_VfmaddsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VfmaddsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddsubpV_W1_2_Data))]
		void Test32_VfmaddsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddsubpV_W1_1_Data))]
		void Test64_VfmaddsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VfmaddsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5C 10 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5C 10 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5D 10 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5D 10 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddsubpV_W1_2_Data))]
		void Test64_VfmaddsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 5C D3 C0", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 5C D3 D0", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 5C D3 E0", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 5C D3 80", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 5C D3 40", 6, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 5C D3 50", 6, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 5D D3 C0", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 5D D3 D0", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 5D D3 E0", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 5D D3 80", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 5D D3 40", 6, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 5D D3 50", 6, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubaddpV_W0_1_Data))]
		void Test16_VfmsubaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubaddpV_W0_2_Data))]
		void Test16_VfmsubaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubaddpV_W0_1_Data))]
		void Test32_VfmsubaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubaddpV_W0_2_Data))]
		void Test32_VfmsubaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubaddpV_W0_1_Data))]
		void Test64_VfmsubaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubaddpV_W0_2_Data))]
		void Test64_VfmsubaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 5E D3 C0", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 5E D3 D0", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 5E D3 E0", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 5E D3 80", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 5F D3 C0", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 5F D3 D0", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 5F D3 E0", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 5F D3 80", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubaddpV_W1_1_Data))]
		void Test16_VfmsubaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VfmsubaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubaddpV_W1_2_Data))]
		void Test16_VfmsubaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubaddpV_W1_1_Data))]
		void Test32_VfmsubaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VfmsubaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubaddpV_W1_2_Data))]
		void Test32_VfmsubaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubaddpV_W1_1_Data))]
		void Test64_VfmsubaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VfmsubaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 5E 10 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 5E 10 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 5F 10 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 5F 10 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubaddpV_W1_2_Data))]
		void Test64_VfmsubaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 5E D3 C0", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 5E D3 D0", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 5E D3 E0", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 5E D3 80", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 5E D3 40", 6, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 5E D3 50", 6, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 5F D3 C0", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 5F D3 D0", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 5F D3 E0", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 5F D3 80", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 5F D3 40", 6, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 5F D3 50", 6, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };
			}
		}
	}
}
