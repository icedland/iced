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
	public sealed class DecoderTest_3_0F3A68_0F3A6F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VfmaddpV_W0_1_Data))]
		void Test16_VfmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddpV_W0_2_Data))]
		void Test16_VfmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddpV_W0_1_Data))]
		void Test32_VfmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddpV_W0_2_Data))]
		void Test32_VfmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddpV_W0_1_Data))]
		void Test64_VfmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddpV_W0_2_Data))]
		void Test64_VfmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 68 D3 C0", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 68 D3 D0", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 68 D3 E0", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 68 D3 80", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 69 D3 C0", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 69 D3 D0", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 69 D3 E0", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 69 D3 80", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 6A D3 C0", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 6A D3 E0", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 6B D3 C0", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 6B D3 E0", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddpV_W1_1_Data))]
		void Test16_VfmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VfmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmaddpV_W1_2_Data))]
		void Test16_VfmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddpV_W1_1_Data))]
		void Test32_VfmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VfmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmaddpV_W1_2_Data))]
		void Test32_VfmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddpV_W1_1_Data))]
		void Test64_VfmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VfmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 68 10 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 68 10 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 69 10 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 69 10 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6A 10 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6B 10 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmaddpV_W1_2_Data))]
		void Test64_VfmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 68 D3 C0", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 68 D3 D0", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 68 D3 E0", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 68 D3 80", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 68 D3 40", 6, Code.VEX_Vfmaddps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 68 D3 50", 6, Code.VEX_Vfmaddps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 69 D3 C0", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 69 D3 D0", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 69 D3 E0", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 69 D3 80", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 69 D3 40", 6, Code.VEX_Vfmaddpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 69 D3 50", 6, Code.VEX_Vfmaddpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 6A D3 C0", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 6A D3 E0", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 6A D3 40", 6, Code.VEX_Vfmaddss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 6B D3 C0", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 6B D3 E0", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 6B D3 40", 6, Code.VEX_Vfmaddsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubpV_W0_1_Data))]
		void Test16_VfmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubpV_W0_2_Data))]
		void Test16_VfmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubpV_W0_1_Data))]
		void Test32_VfmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubpV_W0_2_Data))]
		void Test32_VfmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubpV_W0_1_Data))]
		void Test64_VfmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubpV_W0_2_Data))]
		void Test64_VfmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 6C D3 C0", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 6C D3 D0", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 6C D3 E0", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 6C D3 80", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 6D D3 C0", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 6D D3 D0", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 6D D3 E0", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 6D D3 80", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_WY_Is4Y, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 6E D3 C0", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 6E D3 E0", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 6F D3 C0", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 6F D3 E0", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_WX_Is4X, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubpV_W1_1_Data))]
		void Test16_VfmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VfmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfmsubpV_W1_2_Data))]
		void Test16_VfmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubpV_W1_1_Data))]
		void Test32_VfmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VfmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfmsubpV_W1_2_Data))]
		void Test32_VfmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubpV_W1_1_Data))]
		void Test64_VfmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VfmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 6C 10 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 6C 10 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 6D 10 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 6D 10 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 6E 10 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 6F 10 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfmsubpV_W1_2_Data))]
		void Test64_VfmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 6C D3 C0", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 6C D3 D0", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 6C D3 E0", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 6C D3 80", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 6C D3 40", 6, Code.VEX_Vfmsubps_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 6C D3 50", 6, Code.VEX_Vfmsubps_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 6D D3 C0", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 6D D3 D0", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 6D D3 E0", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 6D D3 80", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 6D D3 40", 6, Code.VEX_Vfmsubpd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 6D D3 50", 6, Code.VEX_Vfmsubpd_VY_HY_Is4Y_WY, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 6E D3 C0", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 6E D3 E0", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 6E D3 40", 6, Code.VEX_Vfmsubss_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 6F D3 C0", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 6F D3 E0", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 6F D3 40", 6, Code.VEX_Vfmsubsd_VX_HX_Is4X_WX, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}
	}
}
