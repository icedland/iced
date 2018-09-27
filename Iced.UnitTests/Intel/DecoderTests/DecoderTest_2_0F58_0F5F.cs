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
	public sealed class DecoderTest_2_0F58_0F5F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_AddV_VX_WX_1_Data))]
		void Test16_AddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_AddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F58 08", 3, Code.Addps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F58 08", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F58 08", 4, Code.Addss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F58 08", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AddV_VX_WX_2_Data))]
		void Test16_AddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F58 CD", 3, Code.Addps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F58 CD", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F58 CD", 4, Code.Addss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F58 CD", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AddV_VX_WX_1_Data))]
		void Test32_AddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_AddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F58 08", 3, Code.Addps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F58 08", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F58 08", 4, Code.Addss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F58 08", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AddV_VX_WX_2_Data))]
		void Test32_AddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F58 CD", 3, Code.Addps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F58 CD", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F58 CD", 4, Code.Addss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F58 CD", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AddV_VX_WX_1_Data))]
		void Test64_AddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_AddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F58 08", 3, Code.Addps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F58 08", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F58 08", 4, Code.Addss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F58 08", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AddV_VX_WX_2_Data))]
		void Test64_AddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F58 CD", 3, Code.Addps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F58 CD", 4, Code.Addps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F58 CD", 4, Code.Addps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F58 CD", 4, Code.Addps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F58 CD", 4, Code.Addpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F58 CD", 5, Code.Addpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F58 CD", 5, Code.Addpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F58 CD", 5, Code.Addpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F58 CD", 4, Code.Addss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F58 CD", 5, Code.Addss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F58 CD", 5, Code.Addss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F58 CD", 5, Code.Addss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F58 CD", 4, Code.Addsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F58 CD", 5, Code.Addsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F58 CD", 5, Code.Addsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F58 CD", 5, Code.Addsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddV_VX_HX_WX_1_Data))]
		void Test16_VaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 58 10", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 58 10", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 58 10", 5, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 58 10", 5, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 58 10", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 58 10", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 58 10", 5, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 58 10", 5, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddV_VX_HX_WX_2_Data))]
		void Test16_VaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 58 D3", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 58 D3", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 58 D3", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 58 D3", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 58 D3", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 58 D3", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddV_VX_HX_WX_1_Data))]
		void Test32_VaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 58 10", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 58 10", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 58 10", 5, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 58 10", 5, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 58 10", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 58 10", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 58 10", 5, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 58 10", 5, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddV_VX_HX_WX_2_Data))]
		void Test32_VaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 58 D3", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 58 D3", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 58 D3", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 58 D3", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 58 D3", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 58 D3", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddV_VX_HX_WX_1_Data))]
		void Test64_VaddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VaddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 58 10", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 58 10", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 58 10", 5, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 58 10", 5, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 58 10", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 58 10", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 58 10", 5, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 58 10", 5, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 58 10", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 58 10", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 58 10", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 58 10", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddV_VX_HX_WX_2_Data))]
		void Test64_VaddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VaddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 58 D3", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 58 D3", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 58 D3", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 58 D3", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 58 D3", 4, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 58 D3", 4, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 58 D3", 5, Code.VEX_Vaddps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 58 D3", 5, Code.VEX_Vaddps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 58 D3", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 58 D3", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 58 D3", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 58 D3", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 58 D3", 4, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 58 D3", 4, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 58 D3", 5, Code.VEX_Vaddpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 58 D3", 5, Code.VEX_Vaddpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 58 D3", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 58 D3", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 58 D3", 4, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 58 D3", 5, Code.VEX_Vaddss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 58 D3", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 58 D3", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 58 D3", 4, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 58 D3", 5, Code.VEX_Vaddsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddV_VX_k1_HX_WX_1_Data))]
		void Test16_VaddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VaddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddV_VX_k1_HX_WX_2_Data))]
		void Test16_VaddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VaddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddV_VX_k1_HX_WX_1_Data))]
		void Test32_VaddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VaddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddV_VX_k1_HX_WX_2_Data))]
		void Test32_VaddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VaddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddV_VX_k1_HX_WX_1_Data))]
		void Test64_VaddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VaddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 58 50 01", 7, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 58 50 01", 7, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 58 50 01", 7, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 58 50 01", 7, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 58 50 01", 7, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 58 50 01", 7, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 58 50 01", 7, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 58 50 01", 7, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddV_VX_k1_HX_WX_2_Data))]
		void Test64_VaddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VaddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C0B 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C03 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C0B 58 D3", 6, Code.EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C2B 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C23 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C2B 58 D3", 6, Code.EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C4B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C43 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C4B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 58 D3", 6, Code.EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D0B 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD03 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD0B 58 D3", 6, Code.EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CDDB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D2B 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD23 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD2B 58 D3", 6, Code.EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD1B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D4B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD43 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD4B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD7B 58 D3", 6, Code.EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10E0B 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114E03 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14E0B 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 58 D3", 6, Code.EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18F0B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CF03 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CF0B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CF7B 58 D3", 6, Code.EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MulV_VX_WX_1_Data))]
		void Test16_MulV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MulV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F59 08", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F59 08", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F59 08", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F59 08", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MulV_VX_WX_2_Data))]
		void Test16_MulV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MulV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F59 CD", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F59 CD", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F59 CD", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F59 CD", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MulV_VX_WX_1_Data))]
		void Test32_MulV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MulV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F59 08", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F59 08", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F59 08", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F59 08", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MulV_VX_WX_2_Data))]
		void Test32_MulV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MulV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F59 CD", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F59 CD", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F59 CD", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F59 CD", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MulV_VX_WX_1_Data))]
		void Test64_MulV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MulV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F59 08", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F59 08", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F59 08", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F59 08", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MulV_VX_WX_2_Data))]
		void Test64_MulV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MulV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F59 CD", 3, Code.Mulps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F59 CD", 4, Code.Mulps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F59 CD", 4, Code.Mulps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F59 CD", 4, Code.Mulps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F59 CD", 4, Code.Mulpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F59 CD", 5, Code.Mulpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F59 CD", 5, Code.Mulpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F59 CD", 5, Code.Mulpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F59 CD", 4, Code.Mulss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F59 CD", 5, Code.Mulss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F59 CD", 5, Code.Mulss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F59 CD", 5, Code.Mulss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F59 CD", 4, Code.Mulsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F59 CD", 5, Code.Mulsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F59 CD", 5, Code.Mulsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F59 CD", 5, Code.Mulsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmulV_VX_HX_WX_1_Data))]
		void Test16_VmulV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmulV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 59 10", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 59 10", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 59 10", 5, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 59 10", 5, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 59 10", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 59 10", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 59 10", 5, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 59 10", 5, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmulV_VX_HX_WX_2_Data))]
		void Test16_VmulV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VmulV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 59 D3", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 59 D3", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 59 D3", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 59 D3", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 59 D3", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 59 D3", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmulV_VX_HX_WX_1_Data))]
		void Test32_VmulV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmulV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 59 10", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 59 10", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 59 10", 5, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 59 10", 5, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 59 10", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 59 10", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 59 10", 5, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 59 10", 5, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmulV_VX_HX_WX_2_Data))]
		void Test32_VmulV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VmulV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 59 D3", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 59 D3", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 59 D3", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 59 D3", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 59 D3", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 59 D3", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmulV_VX_HX_WX_1_Data))]
		void Test64_VmulV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmulV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 59 10", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 59 10", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 59 10", 5, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 59 10", 5, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 59 10", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 59 10", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 59 10", 5, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 59 10", 5, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 59 10", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 59 10", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 59 10", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 59 10", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmulV_VX_HX_WX_2_Data))]
		void Test64_VmulV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VmulV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 59 D3", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 59 D3", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 59 D3", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 59 D3", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 59 D3", 4, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 59 D3", 4, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 59 D3", 5, Code.VEX_Vmulps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 59 D3", 5, Code.VEX_Vmulps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 59 D3", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 59 D3", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 59 D3", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 59 D3", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 59 D3", 4, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 59 D3", 4, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 59 D3", 5, Code.VEX_Vmulpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 59 D3", 5, Code.VEX_Vmulpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 59 D3", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 59 D3", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 59 D3", 4, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 59 D3", 5, Code.VEX_Vmulss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 59 D3", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 59 D3", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 59 D3", 4, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 59 D3", 5, Code.VEX_Vmulsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmulV_VX_k1_HX_WX_1_Data))]
		void Test16_VmulV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VmulV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmulV_VX_k1_HX_WX_2_Data))]
		void Test16_VmulV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VmulV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmulV_VX_k1_HX_WX_1_Data))]
		void Test32_VmulV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VmulV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmulV_VX_k1_HX_WX_2_Data))]
		void Test32_VmulV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VmulV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmulV_VX_k1_HX_WX_1_Data))]
		void Test64_VmulV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VmulV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 59 50 01", 7, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 59 50 01", 7, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 59 50 01", 7, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 59 50 01", 7, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 59 50 01", 7, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 59 50 01", 7, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 59 50 01", 7, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 59 50 01", 7, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmulV_VX_k1_HX_WX_2_Data))]
		void Test64_VmulV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VmulV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C0B 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C03 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C0B 59 D3", 6, Code.EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C2B 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C23 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C2B 59 D3", 6, Code.EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C4B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C43 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C4B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 59 D3", 6, Code.EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D0B 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD03 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD0B 59 D3", 6, Code.EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CDDB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D2B 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD23 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD2B 59 D3", 6, Code.EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD1B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D4B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD43 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD4B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD7B 59 D3", 6, Code.EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10E0B 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114E03 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14E0B 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 59 D3", 6, Code.EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18F0B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CF03 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CF0B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CF7B 59 D3", 6, Code.EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_1_Data))]
		void Test16_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F5A 08", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F5A 08", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5A 08", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5A 08", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F5B 08", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "66 0F5B 08", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F5B 08", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F8 5A 10", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 5A 10", 5, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5FC 5A 10", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 5A 10", 5, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F9 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5F8 5B 10", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F8 5B 10", 5, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FC 5B 10", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FC 5B 10", 5, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "C5F9 5B 10", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F9 5B 10", 5, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FD 5B 10", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FD 5B 10", 5, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5FA 5B 10", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 5B 10", 5, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 5B 10", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 5B 10", 5, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_2_Data))]
		void Test16_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F5A CD", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5A CD", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5A CD", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5A CD", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F5B CD", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5B CD", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5B CD", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 5A CD", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 5A CD", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };

				yield return new object[] { "C5F9 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM1, Register.YMM5 };

				yield return new object[] { "C5F8 5B CD", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 5B CD", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 5B CD", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 5B CD", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5FA 5B CD", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 5B CD", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_1_Data))]
		void Test32_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F5A 08", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F5A 08", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5A 08", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5A 08", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F5B 08", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "66 0F5B 08", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F5B 08", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F8 5A 10", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 5A 10", 5, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5FC 5A 10", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 5A 10", 5, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F9 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5F8 5B 10", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F8 5B 10", 5, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FC 5B 10", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FC 5B 10", 5, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "C5F9 5B 10", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F9 5B 10", 5, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FD 5B 10", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FD 5B 10", 5, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5FA 5B 10", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 5B 10", 5, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 5B 10", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 5B 10", 5, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_2_Data))]
		void Test32_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F5A CD", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5A CD", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5A CD", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5A CD", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F5B CD", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5B CD", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5B CD", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 5A CD", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 5A CD", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };

				yield return new object[] { "C5F9 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM1, Register.YMM5 };

				yield return new object[] { "C5F8 5B CD", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 5B CD", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 5B CD", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 5B CD", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5FA 5B CD", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 5B CD", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_1_Data))]
		void Test64_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F5A 08", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F5A 08", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5A 08", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5A 08", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F5B 08", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "66 0F5B 08", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F5B 08", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F8 5A 10", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 5A 10", 5, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5FC 5A 10", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 5A 10", 5, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5F9 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 5A 10", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 5A 10", 5, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5F8 5B 10", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F8 5B 10", 5, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FC 5B 10", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FC 5B 10", 5, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "C5F9 5B 10", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F9 5B 10", 5, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FD 5B 10", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FD 5B 10", 5, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5FA 5B 10", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 5B 10", 5, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 5B 10", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 5B 10", 5, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_2_Data))]
		void Test64_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F5A CD", 3, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5A CD", 4, Code.Cvtps2pd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5A CD", 4, Code.Cvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5A CD", 4, Code.Cvtps2pd_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5A CD", 4, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5A CD", 5, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5A CD", 5, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5A CD", 5, Code.Cvtpd2ps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5A CD", 4, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5A CD", 5, Code.Cvtss2sd_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5A CD", 5, Code.Cvtss2sd_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5A CD", 5, Code.Cvtss2sd_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F5A CD", 4, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F5A CD", 5, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F5A CD", 5, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F5A CD", 5, Code.Cvtsd2ss_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F5B CD", 3, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5B CD", 4, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5B CD", 4, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5B CD", 4, Code.Cvtdq2ps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5B CD", 4, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5B CD", 5, Code.Cvtps2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5B CD", 5, Code.Cvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5B CD", 5, Code.Cvtps2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5B CD", 4, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5B CD", 5, Code.Cvttps2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5B CD", 5, Code.Cvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5B CD", 5, Code.Cvttps2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 5A CD", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C578 5A CD", 4, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C178 5A CD", 5, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44178 5A CD", 5, Code.VEX_Vcvtps2pd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FC 5A CD", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C57C 5A CD", 4, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C17C 5A CD", 5, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4417C 5A CD", 5, Code.VEX_Vcvtps2pd_ymm_xmmm128, Register.YMM9, Register.XMM13 };

				yield return new object[] { "C5F9 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 5A CD", 5, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 5A CD", 5, Code.VEX_Vcvtpd2ps_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FD 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM1, Register.YMM5 };
				yield return new object[] { "C57D 5A CD", 4, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM9, Register.YMM5 };
				yield return new object[] { "C4C17D 5A CD", 5, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM1, Register.YMM13 };
				yield return new object[] { "C4417D 5A CD", 5, Code.VEX_Vcvtpd2ps_xmm_ymmm256, Register.XMM9, Register.YMM13 };

				yield return new object[] { "C5F8 5B CD", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C578 5B CD", 4, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C178 5B CD", 5, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44178 5B CD", 5, Code.VEX_Vcvtdq2ps_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FC 5B CD", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57C 5B CD", 4, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17C 5B CD", 5, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417C 5B CD", 5, Code.VEX_Vcvtdq2ps_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "C5F9 5B CD", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 5B CD", 4, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 5B CD", 5, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 5B CD", 5, Code.VEX_Vcvtps2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FD 5B CD", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57D 5B CD", 4, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17D 5B CD", 5, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417D 5B CD", 5, Code.VEX_Vcvtps2dq_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "C5FA 5B CD", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A 5B CD", 4, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A 5B CD", 5, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A 5B CD", 5, Code.VEX_Vcvttps2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FE 5B CD", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57E 5B CD", 4, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17E 5B CD", 5, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417E 5B CD", 5, Code.VEX_Vcvttps2dq_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CE 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CE 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5A 10", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5A 10", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5A 10", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5A 10", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CE 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54E 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58E 5A D3", 4, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 5A D3", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14E 5A D3", 5, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54F 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58F 5A D3", 4, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 5A D3", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14F 5A D3", 5, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14E4B 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EEB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF08 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CF4B 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFEB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E0B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E9B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F14E3B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F14EDB 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F14E7B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CF0B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF9B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1CF3B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1CFDB 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1CF7B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14E4B 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EEB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF08 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CF4B 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFEB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E0B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E9B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F14E3B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F14EDB 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F14E7B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CF0B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF9B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1CF3B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1CFDB 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1CF7B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14E4B 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EEB 5A 50 01", 7, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF08 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CF4B 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFEB 5A 50 01", 7, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E10E9B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 714E33 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM10, Register.XMM22, Register.XMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 D14EDB 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM11, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F14E7B 5A D3", 6, Code.EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CF08 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E18F9B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 71CF33 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 D1CFDB 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1CF7B 5A D3", 6, Code.EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test16_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17C9B 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17C28 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17CBB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C48 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CDB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17C08 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17C9B 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17C28 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17CBB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F17C48 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17CDB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int32, 4, true };

				yield return new object[] { "62 F1FC08 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FC9B 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FC28 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FCBB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FC48 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FCDB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F17D08 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17D9B 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D28 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DBB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17D48 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17DDB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F17E08 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E9B 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17E28 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EBB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17E48 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17EDB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test16_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CDB 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17C1B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C3B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17C0B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17C3B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17CDB 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FC3B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FCDB 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17E0B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E9B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17E1B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E3B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E5B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E7B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test32_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17C9B 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17C28 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17CBB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C48 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CDB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17C08 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17C9B 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17C28 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17CBB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F17C48 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17CDB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int32, 4, true };

				yield return new object[] { "62 F1FC08 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FC9B 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FC28 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FCBB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FC48 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FCDB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F17D08 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17D9B 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D28 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DBB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17D48 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17DDB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F17E08 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E9B 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17E28 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EBB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17E48 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17EDB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test32_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CDB 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17C1B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C3B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FD3B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FDDB 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17C0B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17C3B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17CDB 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FC3B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FCDB 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F17D3B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F17DDB 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17E0B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E9B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F17E1B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E3B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E5B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E7B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test64_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F17C9B 5A 50 01", 7, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Float32, 4, true };

				yield return new object[] { "62 F17C28 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17CBB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17C48 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CDB 5A 50 01", 7, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F1FD08 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB 5A 50 01", 7, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17C08 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17C9B 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17C28 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17CBB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F17C48 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17CDB 5B 50 01", 7, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int32, 4, true };

				yield return new object[] { "62 F1FC08 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FC9B 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FC28 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FCBB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FC48 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FCDB 5B 50 01", 7, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F17D08 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17D9B 5B 50 01", 7, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17D28 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17DBB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17D48 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17DDB 5B 50 01", 7, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F17E08 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E9B 5B 50 01", 7, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F17E28 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EBB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F17E48 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17EDB 5B 50 01", 7, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test64_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C0B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C0B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17C8B 5A D3", 6, Code.EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C2B 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17CAB 5A D3", 6, Code.EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CDB 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 317C1B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C17C3B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C5B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17C7B 5A D3", 6, Code.EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1FD0B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB 5A D3", 6, Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD9B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FD3B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FDDB 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FD7B 5A D3", 6, Code.EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17C0B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C0B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17C8B 5B D3", 6, Code.EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C2B 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317C2B 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17CAB 5B D3", 6, Code.EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C4B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C9B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317C3B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17CDB 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17C7B 5B D3", 6, Code.EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FC0B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC8B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC0B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FC8B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC2B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FCAB 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FC2B 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FCAB 5B D3", 6, Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FC4B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FC9B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FC3B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FCDB 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FC7B 5B D3", 6, Code.EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17D0B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D0B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17D8B 5B D3", 6, Code.EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D2B 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317D2B 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17DAB 5B D3", 6, Code.EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D4B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D9B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 317D3B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C17DDB 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F17D7B 5B D3", 6, Code.EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F17E0B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E0B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17E8B 5B D3", 6, Code.EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E2B 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17EAB 5B D3", 6, Code.EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E9B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 317E1B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C17E3B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E5B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F17E7B 5B D3", 6, Code.EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_SubV_VX_WX_1_Data))]
		void Test16_SubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_SubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5C 08", 3, Code.Subps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5C 08", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5C 08", 4, Code.Subss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5C 08", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_SubV_VX_WX_2_Data))]
		void Test16_SubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_SubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5C CD", 3, Code.Subps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5C CD", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5C CD", 4, Code.Subss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5C CD", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_SubV_VX_WX_1_Data))]
		void Test32_SubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_SubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5C 08", 3, Code.Subps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5C 08", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5C 08", 4, Code.Subss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5C 08", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_SubV_VX_WX_2_Data))]
		void Test32_SubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_SubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5C CD", 3, Code.Subps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5C CD", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5C CD", 4, Code.Subss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5C CD", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_SubV_VX_WX_1_Data))]
		void Test64_SubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_SubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5C 08", 3, Code.Subps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5C 08", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5C 08", 4, Code.Subss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5C 08", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_SubV_VX_WX_2_Data))]
		void Test64_SubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_SubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5C CD", 3, Code.Subps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5C CD", 4, Code.Subps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5C CD", 4, Code.Subps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5C CD", 4, Code.Subps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5C CD", 4, Code.Subpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5C CD", 5, Code.Subpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5C CD", 5, Code.Subpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5C CD", 5, Code.Subpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5C CD", 4, Code.Subss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5C CD", 5, Code.Subss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5C CD", 5, Code.Subss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5C CD", 5, Code.Subss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F5C CD", 4, Code.Subsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F5C CD", 5, Code.Subsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F5C CD", 5, Code.Subsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F5C CD", 5, Code.Subsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsubV_VX_HX_WX_1_Data))]
		void Test16_VsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5C 10", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5C 10", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5C 10", 5, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5C 10", 5, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5C 10", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5C 10", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5C 10", 5, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5C 10", 5, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsubV_VX_HX_WX_2_Data))]
		void Test16_VsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5C D3", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5C D3", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5C D3", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5C D3", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5C D3", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5C D3", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsubV_VX_HX_WX_1_Data))]
		void Test32_VsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5C 10", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5C 10", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5C 10", 5, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5C 10", 5, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5C 10", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5C 10", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5C 10", 5, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5C 10", 5, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsubV_VX_HX_WX_2_Data))]
		void Test32_VsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5C D3", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5C D3", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5C D3", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5C D3", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5C D3", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5C D3", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsubV_VX_HX_WX_1_Data))]
		void Test64_VsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5C 10", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5C 10", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5C 10", 5, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5C 10", 5, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5C 10", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5C 10", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5C 10", 5, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5C 10", 5, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5C 10", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5C 10", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5C 10", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5C 10", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsubV_VX_HX_WX_2_Data))]
		void Test64_VsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5C D3", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5C D3", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 5C D3", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 5C D3", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 5C D3", 4, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 5C D3", 4, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 5C D3", 5, Code.VEX_Vsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 5C D3", 5, Code.VEX_Vsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 5C D3", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5C D3", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 5C D3", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 5C D3", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 5C D3", 4, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 5C D3", 4, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 5C D3", 5, Code.VEX_Vsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 5C D3", 5, Code.VEX_Vsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 5C D3", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 5C D3", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 5C D3", 4, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 5C D3", 5, Code.VEX_Vsubss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 5C D3", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 5C D3", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 5C D3", 4, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 5C D3", 5, Code.VEX_Vsubsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsubV_VX_k1_HX_WX_1_Data))]
		void Test16_VsubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VsubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsubV_VX_k1_HX_WX_2_Data))]
		void Test16_VsubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VsubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsubV_VX_k1_HX_WX_1_Data))]
		void Test32_VsubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VsubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsubV_VX_k1_HX_WX_2_Data))]
		void Test32_VsubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VsubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsubV_VX_k1_HX_WX_1_Data))]
		void Test64_VsubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VsubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5C 50 01", 7, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5C 50 01", 7, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5C 50 01", 7, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5C 50 01", 7, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5C 50 01", 7, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5C 50 01", 7, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5C 50 01", 7, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5C 50 01", 7, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsubV_VX_k1_HX_WX_2_Data))]
		void Test64_VsubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VsubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C0B 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C03 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C0B 5C D3", 6, Code.EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C2B 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C23 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C2B 5C D3", 6, Code.EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C4B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C43 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C4B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5C D3", 6, Code.EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D0B 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD03 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD0B 5C D3", 6, Code.EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CDDB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D2B 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD23 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD2B 5C D3", 6, Code.EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD1B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D4B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD43 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD4B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD7B 5C D3", 6, Code.EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10E0B 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114E03 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14E0B 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5C D3", 6, Code.EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18F0B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CF03 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CF0B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CF7B 5C D3", 6, Code.EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_DivV_VX_WX_1_Data))]
		void Test16_DivV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_DivV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5E 08", 3, Code.Divps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5E 08", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5E 08", 4, Code.Divss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5E 08", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_DivV_VX_WX_2_Data))]
		void Test16_DivV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_DivV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5E CD", 3, Code.Divps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5E CD", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5E CD", 4, Code.Divss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5E CD", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_DivV_VX_WX_1_Data))]
		void Test32_DivV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_DivV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5E 08", 3, Code.Divps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5E 08", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5E 08", 4, Code.Divss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5E 08", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_DivV_VX_WX_2_Data))]
		void Test32_DivV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_DivV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5E CD", 3, Code.Divps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5E CD", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5E CD", 4, Code.Divss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5E CD", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_DivV_VX_WX_1_Data))]
		void Test64_DivV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_DivV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5E 08", 3, Code.Divps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5E 08", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5E 08", 4, Code.Divss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5E 08", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_DivV_VX_WX_2_Data))]
		void Test64_DivV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_DivV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5E CD", 3, Code.Divps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5E CD", 4, Code.Divps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5E CD", 4, Code.Divps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5E CD", 4, Code.Divps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5E CD", 4, Code.Divpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5E CD", 5, Code.Divpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5E CD", 5, Code.Divpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5E CD", 5, Code.Divpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5E CD", 4, Code.Divss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5E CD", 5, Code.Divss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5E CD", 5, Code.Divss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5E CD", 5, Code.Divss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F5E CD", 4, Code.Divsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F5E CD", 5, Code.Divsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F5E CD", 5, Code.Divsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F5E CD", 5, Code.Divsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdivV_VX_HX_WX_1_Data))]
		void Test16_VdivV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VdivV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5E 10", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5E 10", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5E 10", 5, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5E 10", 5, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5E 10", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5E 10", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5E 10", 5, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5E 10", 5, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdivV_VX_HX_WX_2_Data))]
		void Test16_VdivV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VdivV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5E D3", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5E D3", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5E D3", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5E D3", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5E D3", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5E D3", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdivV_VX_HX_WX_1_Data))]
		void Test32_VdivV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VdivV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5E 10", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5E 10", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5E 10", 5, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5E 10", 5, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5E 10", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5E 10", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5E 10", 5, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5E 10", 5, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdivV_VX_HX_WX_2_Data))]
		void Test32_VdivV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VdivV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5E D3", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5E D3", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5E D3", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5E D3", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5E D3", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5E D3", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdivV_VX_HX_WX_1_Data))]
		void Test64_VdivV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VdivV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5E 10", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5E 10", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5E 10", 5, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5E 10", 5, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5E 10", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5E 10", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5E 10", 5, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5E 10", 5, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5E 10", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5E 10", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5E 10", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5E 10", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdivV_VX_HX_WX_2_Data))]
		void Test64_VdivV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VdivV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5E D3", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5E D3", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 5E D3", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 5E D3", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 5E D3", 4, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 5E D3", 4, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 5E D3", 5, Code.VEX_Vdivps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 5E D3", 5, Code.VEX_Vdivps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 5E D3", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5E D3", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 5E D3", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 5E D3", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 5E D3", 4, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 5E D3", 4, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 5E D3", 5, Code.VEX_Vdivpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 5E D3", 5, Code.VEX_Vdivpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 5E D3", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 5E D3", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 5E D3", 4, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 5E D3", 5, Code.VEX_Vdivss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 5E D3", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 5E D3", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 5E D3", 4, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 5E D3", 5, Code.VEX_Vdivsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdivV_VX_k1_HX_WX_1_Data))]
		void Test16_VdivV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VdivV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdivV_VX_k1_HX_WX_2_Data))]
		void Test16_VdivV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_VdivV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdivV_VX_k1_HX_WX_1_Data))]
		void Test32_VdivV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VdivV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdivV_VX_k1_HX_WX_2_Data))]
		void Test32_VdivV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_VdivV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CDDB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD1B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CD7B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdivV_VX_k1_HX_WX_1_Data))]
		void Test64_VdivV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VdivV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5E 50 01", 7, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5E 50 01", 7, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5E 50 01", 7, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5E 50 01", 7, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5E 50 01", 7, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5E 50 01", 7, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5E 50 01", 7, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5E 50 01", 7, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdivV_VX_k1_HX_WX_2_Data))]
		void Test64_VdivV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_VdivV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C0B 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C03 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C0B 5E D3", 6, Code.EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14CDB 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F14C2B 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C2B 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C23 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C2B 5E D3", 6, Code.EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C1B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F14C4B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10C4B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114C43 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14C4B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14C3B 5E D3", 6, Code.EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1CD8B 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D0B 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD03 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD0B 5E D3", 6, Code.EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CDDB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CDAB 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D2B 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD23 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD2B 5E D3", 6, Code.EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD1B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1CDCB 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18D4B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CD43 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CD4B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CD7B 5E D3", 6, Code.EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14E0B 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10E0B 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114E03 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14E0B 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 5E D3", 6, Code.EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18F0B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CF03 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CF0B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CF7B 5E D3", 6, Code.EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MaxV_VX_WX_1_Data))]
		void Test16_MaxV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MaxV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5F 08", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5F 08", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5F 08", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5F 08", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MaxV_VX_WX_2_Data))]
		void Test16_MaxV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MaxV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5F CD", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5F CD", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5F CD", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5F CD", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MaxV_VX_WX_1_Data))]
		void Test32_MaxV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MaxV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5F 08", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5F 08", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5F 08", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5F 08", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MaxV_VX_WX_2_Data))]
		void Test32_MaxV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MaxV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5F CD", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5F CD", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5F CD", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5F CD", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MaxV_VX_WX_1_Data))]
		void Test64_MaxV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MaxV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5F 08", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5F 08", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5F 08", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5F 08", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MaxV_VX_WX_2_Data))]
		void Test64_MaxV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MaxV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5F CD", 3, Code.Maxps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5F CD", 4, Code.Maxps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5F CD", 4, Code.Maxps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5F CD", 4, Code.Maxps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5F CD", 4, Code.Maxpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5F CD", 5, Code.Maxpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5F CD", 5, Code.Maxpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5F CD", 5, Code.Maxpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5F CD", 4, Code.Maxss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5F CD", 5, Code.Maxss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5F CD", 5, Code.Maxss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5F CD", 5, Code.Maxss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F5F CD", 4, Code.Maxsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F5F CD", 5, Code.Maxsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F5F CD", 5, Code.Maxsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F5F CD", 5, Code.Maxsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaxV_VX_HX_WX_1_Data))]
		void Test16_VmaxV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VmaxV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5F 10", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5F 10", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5F 10", 5, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5F 10", 5, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5F 10", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5F 10", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5F 10", 5, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5F 10", 5, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaxV_VX_HX_WX_2_Data))]
		void Test16_VmaxV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VmaxV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5F D3", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5F D3", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5F D3", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5F D3", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5F D3", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5F D3", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaxV_VX_HX_WX_1_Data))]
		void Test32_VmaxV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VmaxV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5F 10", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5F 10", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5F 10", 5, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5F 10", 5, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5F 10", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5F 10", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5F 10", 5, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5F 10", 5, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaxV_VX_HX_WX_2_Data))]
		void Test32_VmaxV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VmaxV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5F D3", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5F D3", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5F D3", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5F D3", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5F D3", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5F D3", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaxV_VX_HX_WX_1_Data))]
		void Test64_VmaxV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VmaxV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5F 10", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5F 10", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5F 10", 5, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5F 10", 5, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5F 10", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5F 10", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5F 10", 5, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5F 10", 5, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5F 10", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5F 10", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5F 10", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5F 10", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaxV_VX_HX_WX_2_Data))]
		void Test64_VmaxV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VmaxV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5F D3", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5F D3", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 5F D3", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 5F D3", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 5F D3", 4, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 5F D3", 4, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 5F D3", 5, Code.VEX_Vmaxps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 5F D3", 5, Code.VEX_Vmaxps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 5F D3", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5F D3", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 5F D3", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 5F D3", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 5F D3", 4, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 5F D3", 4, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 5F D3", 5, Code.VEX_Vmaxpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 5F D3", 5, Code.VEX_Vmaxpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 5F D3", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 5F D3", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 5F D3", 4, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 5F D3", 5, Code.VEX_Vmaxss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 5F D3", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 5F D3", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 5F D3", 4, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 5F D3", 5, Code.VEX_Vmaxsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaxV_VX_k1_HX_WX_1_Data))]
		void Test16_VmaxV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VmaxV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmaxV_VX_k1_HX_WX_2_Data))]
		void Test16_VmaxV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VmaxV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CDDB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD1B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD7B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF7B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaxV_VX_k1_HX_WX_1_Data))]
		void Test32_VmaxV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VmaxV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmaxV_VX_k1_HX_WX_2_Data))]
		void Test32_VmaxV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VmaxV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CDDB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD1B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD7B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF7B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaxV_VX_k1_HX_WX_1_Data))]
		void Test64_VmaxV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VmaxV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5F 50 01", 7, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5F 50 01", 7, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5F 50 01", 7, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5F 50 01", 7, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5F 50 01", 7, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5F 50 01", 7, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5F 50 01", 7, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5F 50 01", 7, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmaxV_VX_k1_HX_WX_2_Data))]
		void Test64_VmaxV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VmaxV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C0B 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C03 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C0B 5F D3", 6, Code.EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C2B 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C23 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C2B 5F D3", 6, Code.EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C4B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C43 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C4B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5F D3", 6, Code.EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D0B 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD03 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD0B 5F D3", 6, Code.EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDDB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D2B 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD23 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD2B 5F D3", 6, Code.EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD1B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D4B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD43 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD4B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD7B 5F D3", 6, Code.EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10E0B 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114E03 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14E0B 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5F D3", 6, Code.EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18F0B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CF03 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CF0B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF7B 5F D3", 6, Code.EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MinV_VX_WX_1_Data))]
		void Test16_MinV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MinV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5D 08", 3, Code.Minps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5D 08", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5D 08", 4, Code.Minss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5D 08", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MinV_VX_WX_2_Data))]
		void Test16_MinV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MinV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5D CD", 3, Code.Minps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5D CD", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5D CD", 4, Code.Minss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5D CD", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MinV_VX_WX_1_Data))]
		void Test32_MinV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MinV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5D 08", 3, Code.Minps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5D 08", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5D 08", 4, Code.Minss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5D 08", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MinV_VX_WX_2_Data))]
		void Test32_MinV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MinV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5D CD", 3, Code.Minps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F5D CD", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F5D CD", 4, Code.Minss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F5D CD", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MinV_VX_WX_1_Data))]
		void Test64_MinV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MinV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F5D 08", 3, Code.Minps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F5D 08", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F5D 08", 4, Code.Minss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F5D 08", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MinV_VX_WX_2_Data))]
		void Test64_MinV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MinV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F5D CD", 3, Code.Minps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F5D CD", 4, Code.Minps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F5D CD", 4, Code.Minps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F5D CD", 4, Code.Minps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F5D CD", 4, Code.Minpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F5D CD", 5, Code.Minpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F5D CD", 5, Code.Minpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F5D CD", 5, Code.Minpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F5D CD", 4, Code.Minss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F5D CD", 5, Code.Minss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F5D CD", 5, Code.Minss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F5D CD", 5, Code.Minss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F5D CD", 4, Code.Minsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F5D CD", 5, Code.Minsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F5D CD", 5, Code.Minsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F5D CD", 5, Code.Minsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VminV_VX_HX_WX_1_Data))]
		void Test16_VminV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VminV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5D 10", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5D 10", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5D 10", 5, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5D 10", 5, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5D 10", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5D 10", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5D 10", 5, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5D 10", 5, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VminV_VX_HX_WX_2_Data))]
		void Test16_VminV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VminV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5D D3", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5D D3", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5D D3", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5D D3", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5D D3", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5D D3", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VminV_VX_HX_WX_1_Data))]
		void Test32_VminV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VminV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5D 10", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5D 10", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5D 10", 5, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5D 10", 5, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5D 10", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5D 10", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5D 10", 5, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5D 10", 5, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VminV_VX_HX_WX_2_Data))]
		void Test32_VminV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VminV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5D D3", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5D D3", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 5D D3", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5D D3", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CA 5D D3", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 5D D3", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VminV_VX_HX_WX_1_Data))]
		void Test64_VminV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VminV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 5D 10", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 5D 10", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 5D 10", 5, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 5D 10", 5, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 5D 10", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 5D 10", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 5D 10", 5, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 5D 10", 5, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CA 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 5D 10", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 5D 10", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 5D 10", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 5D 10", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VminV_VX_HX_WX_2_Data))]
		void Test64_VminV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VminV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 5D D3", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 5D D3", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 5D D3", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 5D D3", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 5D D3", 4, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 5D D3", 4, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 5D D3", 5, Code.VEX_Vminps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 5D D3", 5, Code.VEX_Vminps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 5D D3", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 5D D3", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 5D D3", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 5D D3", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 5D D3", 4, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 5D D3", 4, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 5D D3", 5, Code.VEX_Vminpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 5D D3", 5, Code.VEX_Vminpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CA 5D D3", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 5D D3", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 5D D3", 4, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 5D D3", 5, Code.VEX_Vminss_xmm_xmm_xmmm32, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 5D D3", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 5D D3", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 5D D3", 4, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 5D D3", 5, Code.VEX_Vminsd_xmm_xmm_xmmm64, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VminV_VX_k1_HX_WX_1_Data))]
		void Test16_VminV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VminV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VminV_VX_k1_HX_WX_2_Data))]
		void Test16_VminV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VminV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CDDB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD1B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD7B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF7B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VminV_VX_k1_HX_WX_1_Data))]
		void Test32_VminV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VminV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VminV_VX_k1_HX_WX_2_Data))]
		void Test32_VminV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VminV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CDDB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD1B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD7B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF7B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VminV_VX_k1_HX_WX_1_Data))]
		void Test64_VminV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VminV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 5D 50 01", 7, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 5D 50 01", 7, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 5D 50 01", 7, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 5D 50 01", 7, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 5D 50 01", 7, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 5D 50 01", 7, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };

				yield return new object[] { "62 F14E0B 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 5D 50 01", 7, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 5D 50 01", 7, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VminV_VX_k1_HX_WX_2_Data))]
		void Test64_VminV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VminV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C0B 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C03 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C0B 5D D3", 6, Code.EVEX_Vminps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14CDB 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F14C2B 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C2B 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C23 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C2B 5D D3", 6, Code.EVEX_Vminps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C1B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14C4B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10C4B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114C43 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14C4B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14C3B 5D D3", 6, Code.EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CD8B 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D0B 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD03 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD0B 5D D3", 6, Code.EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDDB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CDAB 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D2B 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD23 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD2B 5D D3", 6, Code.EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD1B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F1CDCB 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18D4B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CD43 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD4B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD7B 5D D3", 6, Code.EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F14E0B 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10E0B 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 114E03 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14E0B 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14EDB 5D D3", 6, Code.EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F1CF8B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E18F0B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 11CF03 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CF0B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF7B 5D D3", 6, Code.EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}
	}
}
