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
	public sealed class DecoderTest_3_0F3A78_0F3A7F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VfnmaddpV_W0_1_Data))]
		void Test16_VfnmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmaddpV_W0_2_Data))]
		void Test16_VfnmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmaddpV_W0_1_Data))]
		void Test32_VfnmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmaddpV_W0_2_Data))]
		void Test32_VfnmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmaddpV_W0_1_Data))]
		void Test64_VfnmaddpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmaddpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmaddpV_W0_2_Data))]
		void Test64_VfnmaddpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmaddpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 78 D3 C0", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 78 D3 D0", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 78 D3 E0", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 78 D3 80", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 79 D3 C0", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 79 D3 D0", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 79 D3 E0", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 79 D3 80", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 7A D3 C0", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 7A D3 E0", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 7B D3 C0", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 7B D3 E0", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmaddpV_W1_1_Data))]
		void Test16_VfnmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VfnmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmaddpV_W1_2_Data))]
		void Test16_VfnmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmaddpV_W1_1_Data))]
		void Test32_VfnmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VfnmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmaddpV_W1_2_Data))]
		void Test32_VfnmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmaddpV_W1_1_Data))]
		void Test64_VfnmaddpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VfnmaddpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 78 10 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 78 10 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 79 10 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 79 10 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7A 10 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7B 10 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmaddpV_W1_2_Data))]
		void Test64_VfnmaddpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmaddpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 78 D3 C0", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 78 D3 D0", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 78 D3 E0", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 78 D3 80", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 78 D3 40", 6, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 78 D3 50", 6, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 79 D3 C0", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 79 D3 D0", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 79 D3 E0", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 79 D3 80", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 79 D3 40", 6, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 79 D3 50", 6, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 7A D3 C0", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 7A D3 E0", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 7A D3 40", 6, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 7B D3 C0", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 7B D3 E0", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 7B D3 40", 6, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmsubpV_W0_1_Data))]
		void Test16_VfnmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmsubpV_W0_2_Data))]
		void Test16_VfnmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmsubpV_W0_1_Data))]
		void Test32_VfnmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmsubpV_W0_2_Data))]
		void Test32_VfnmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };

				yield return new object[] { "C4E349 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmsubpV_W0_1_Data))]
		void Test64_VfnmsubpV_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmsubpV_W0_1_Data {
			get {
				yield return new object[] { "C4E349 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, Register.YMM5 };

				yield return new object[] { "C4E349 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, Register.YMM5 };

				yield return new object[] { "C4E349 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };
				yield return new object[] { "C4E34D 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.XMM4 };

				yield return new object[] { "C4E349 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
				yield return new object[] { "C4E34D 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmsubpV_W0_2_Data))]
		void Test64_VfnmsubpV_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmsubpV_W0_2_Data {
			get {
				yield return new object[] { "C4E349 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 7C D3 C0", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 7C D3 D0", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 7C D3 E0", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 7C D3 80", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C4E34D 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "C46349 7D D3 C0", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4634D 7D D3 D0", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "C4E309 7D D3 E0", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4E30D 7D D3 80", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "C4C349 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4C34D 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };

				yield return new object[] { "C4E349 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 7E D3 C0", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 7E D3 E0", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };

				yield return new object[] { "C4E349 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "C46349 7F D3 C0", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "C4E309 7F D3 E0", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "C4C349 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "C4E34D 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmsubpV_W1_1_Data))]
		void Test16_VfnmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VfnmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfnmsubpV_W1_2_Data))]
		void Test16_VfnmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test16_VfnmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmsubpV_W1_1_Data))]
		void Test32_VfnmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VfnmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfnmsubpV_W1_2_Data))]
		void Test32_VfnmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test32_VfnmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };

				yield return new object[] { "C4E3C9 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmsubpV_W1_1_Data))]
		void Test64_VfnmsubpV_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VfnmsubpV_W1_1_Data {
			get {
				yield return new object[] { "C4E3C9 7C 10 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E3CD 7C 10 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E3C9 7D 10 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E3CD 7D 10 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.Packed256_Float64 };

				yield return new object[] { "C4E3C9 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };
				yield return new object[] { "C4E3CD 7E 10 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float32 };

				yield return new object[] { "C4E3C9 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
				yield return new object[] { "C4E3CD 7F 10 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfnmsubpV_W1_2_Data))]
		void Test64_VfnmsubpV_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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
		public static IEnumerable<object[]> Test64_VfnmsubpV_W1_2_Data {
			get {
				yield return new object[] { "C4E3C9 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 7C D3 C0", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 7C D3 D0", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 7C D3 E0", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 7C D3 80", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 7C D3 40", 6, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 7C D3 50", 6, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C4E3CD 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "C463C9 7D D3 C0", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C463CD 7D D3 D0", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "C4E389 7D D3 E0", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E38D 7D D3 80", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "C4C3C9 7D D3 40", 6, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4C3CD 7D D3 50", 6, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };

				yield return new object[] { "C4E3C9 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 7E D3 C0", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 7E D3 E0", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 7E D3 40", 6, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };

				yield return new object[] { "C4E3C9 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "C463C9 7F D3 C0", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "C4E389 7F D3 E0", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C3C9 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "C4E3Cd 7F D3 40", 6, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}
	}
}
