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
	public sealed class DecoderTest_2_0FD0_0FD7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_AddsubV_VX_WX_1_Data))]
		void Test16_AddsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_AddsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0FD0 08", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0FD0 08", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AddsubV_VX_WX_2_Data))]
		void Test16_AddsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AddsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0FD0 CD", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0FD0 CD", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AddsubV_VX_WX_1_Data))]
		void Test32_AddsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_AddsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0FD0 08", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0FD0 08", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AddsubV_VX_WX_2_Data))]
		void Test32_AddsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AddsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0FD0 CD", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0FD0 CD", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AddsubV_VX_WX_1_Data))]
		void Test64_AddsubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_AddsubV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0FD0 08", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F2 0FD0 08", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AddsubV_VX_WX_2_Data))]
		void Test64_AddsubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AddsubV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0FD0 CD", 4, Code.Addsubpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD0 CD", 5, Code.Addsubpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD0 CD", 5, Code.Addsubpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD0 CD", 5, Code.Addsubpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0FD0 CD", 4, Code.Addsubps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0FD0 CD", 5, Code.Addsubps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0FD0 CD", 5, Code.Addsubps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0FD0 CD", 5, Code.Addsubps_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddsubV_VX_HX_WX_1_Data))]
		void Test16_VaddsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VaddsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D0 10", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD D0 10", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 D0 10", 5, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD D0 10", 5, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB D0 10", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF D0 10", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB D0 10", 5, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF D0 10", 5, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaddsubV_VX_HX_WX_2_Data))]
		void Test16_VaddsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VaddsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D0 D3", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D0 D3", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB D0 D3", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF D0 D3", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddsubV_VX_HX_WX_1_Data))]
		void Test32_VaddsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VaddsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D0 10", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD D0 10", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 D0 10", 5, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD D0 10", 5, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB D0 10", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF D0 10", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB D0 10", 5, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF D0 10", 5, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaddsubV_VX_HX_WX_2_Data))]
		void Test32_VaddsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VaddsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D0 D3", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D0 D3", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5CB D0 D3", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF D0 D3", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddsubV_VX_HX_WX_1_Data))]
		void Test64_VaddsubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VaddsubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D0 10", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD D0 10", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 D0 10", 5, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD D0 10", 5, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5CB D0 10", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CF D0 10", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1CB D0 10", 5, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CF D0 10", 5, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaddsubV_VX_HX_WX_2_Data))]
		void Test64_VaddsubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VaddsubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D0 D3", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D0 D3", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 D0 D3", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D0 D3", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 D0 D3", 4, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D0 D3", 4, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 D0 D3", 5, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D0 D3", 5, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5CB D0 D3", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CF D0 D3", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C54B D0 D3", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54F D0 D3", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C58B D0 D3", 4, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58F D0 D3", 4, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C14B D0 D3", 5, Code.VEX_Vaddsubps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14F D0 D3", 5, Code.VEX_Vaddsubps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrlwV_VX_WX_1_Data))]
		void Test16_PsrlwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsrlwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD1 08", 3, Code.Psrlw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD1 08", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrlwV_VX_WX_2_Data))]
		void Test16_PsrlwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsrlwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD1 CD", 3, Code.Psrlw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD1 CD", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrlwV_VX_WX_1_Data))]
		void Test32_PsrlwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsrlwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD1 08", 3, Code.Psrlw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD1 08", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrlwV_VX_WX_2_Data))]
		void Test32_PsrlwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsrlwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD1 CD", 3, Code.Psrlw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD1 CD", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrlwV_VX_WX_1_Data))]
		void Test64_PsrlwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsrlwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD1 08", 3, Code.Psrlw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD1 08", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrlwV_VX_WX_2_Data))]
		void Test64_PsrlwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsrlwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD1 CD", 3, Code.Psrlw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD1 CD", 4, Code.Psrlw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD1 CD", 4, Code.Psrlw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD1 CD", 5, Code.Psrlw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD1 CD", 5, Code.Psrlw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD1 CD", 5, Code.Psrlw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlwV_VX_HX_WX_1_Data))]
		void Test16_VpsrlwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsrlwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D1 10", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D1 10", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D1 10", 5, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D1 10", 5, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlwV_VX_HX_WX_2_Data))]
		void Test16_VpsrlwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsrlwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D1 D3", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D1 D3", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlwV_VX_HX_WX_1_Data))]
		void Test32_VpsrlwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsrlwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D1 10", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D1 10", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D1 10", 5, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D1 10", 5, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlwV_VX_HX_WX_2_Data))]
		void Test32_VpsrlwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsrlwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D1 D3", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D1 D3", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlwV_VX_HX_WX_1_Data))]
		void Test64_VpsrlwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsrlwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D1 10", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D1 10", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D1 10", 5, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D1 10", 5, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlwV_VX_HX_WX_2_Data))]
		void Test64_VpsrlwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsrlwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D1 D3", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D1 D3", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 D1 D3", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D1 D3", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 D1 D3", 4, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D1 D3", 4, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 D1 D3", 5, Code.VEX_Vpsrlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D1 D3", 5, Code.VEX_Vpsrlw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsrlwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrlwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsrlwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsrlwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsrlwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrlwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsrlwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsrlwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsrlwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrlwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B D1 50 01", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B D1 50 01", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B D1 50 01", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsrlwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsrlwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B D1 D3", 6, Code.EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B D1 D3", 6, Code.EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B D1 D3", 6, Code.EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrldV_VX_WX_1_Data))]
		void Test16_PsrldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsrldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD2 08", 3, Code.Psrld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD2 08", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrldV_VX_WX_2_Data))]
		void Test16_PsrldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsrldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD2 CD", 3, Code.Psrld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD2 CD", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrldV_VX_WX_1_Data))]
		void Test32_PsrldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsrldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD2 08", 3, Code.Psrld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD2 08", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrldV_VX_WX_2_Data))]
		void Test32_PsrldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsrldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD2 CD", 3, Code.Psrld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD2 CD", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrldV_VX_WX_1_Data))]
		void Test64_PsrldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsrldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD2 08", 3, Code.Psrld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD2 08", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrldV_VX_WX_2_Data))]
		void Test64_PsrldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsrldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD2 CD", 3, Code.Psrld_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD2 CD", 4, Code.Psrld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD2 CD", 4, Code.Psrld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD2 CD", 5, Code.Psrld_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD2 CD", 5, Code.Psrld_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD2 CD", 5, Code.Psrld_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrldV_VX_HX_WX_1_Data))]
		void Test16_VpsrldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsrldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D2 10", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D2 10", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D2 10", 5, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D2 10", 5, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrldV_VX_HX_WX_2_Data))]
		void Test16_VpsrldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsrldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D2 D3", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D2 D3", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrldV_VX_HX_WX_1_Data))]
		void Test32_VpsrldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsrldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D2 10", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D2 10", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D2 10", 5, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D2 10", 5, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrldV_VX_HX_WX_2_Data))]
		void Test32_VpsrldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsrldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D2 D3", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D2 D3", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrldV_VX_HX_WX_1_Data))]
		void Test64_VpsrldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsrldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D2 10", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D2 10", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D2 10", 5, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D2 10", 5, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrldV_VX_HX_WX_2_Data))]
		void Test64_VpsrldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsrldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D2 D3", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D2 D3", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 D2 D3", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D2 D3", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 D2 D3", 4, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D2 D3", 4, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 D2 D3", 5, Code.VEX_Vpsrld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D2 D3", 5, Code.VEX_Vpsrld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrldV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsrldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrldV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsrldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsrldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrldV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsrldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrldV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsrldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsrldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrldV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsrldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 D2 50 01", 7, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 D2 50 01", 7, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 D2 50 01", 7, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrldV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsrldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsrldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B D2 D3", 6, Code.EVEX_Vpsrld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B D2 D3", 6, Code.EVEX_Vpsrld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B D2 D3", 6, Code.EVEX_Vpsrld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrlqV_VX_WX_1_Data))]
		void Test16_PsrlqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsrlqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD3 08", 3, Code.Psrlq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD3 08", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrlqV_VX_WX_2_Data))]
		void Test16_PsrlqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsrlqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD3 CD", 3, Code.Psrlq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD3 CD", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrlqV_VX_WX_1_Data))]
		void Test32_PsrlqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsrlqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD3 08", 3, Code.Psrlq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD3 08", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrlqV_VX_WX_2_Data))]
		void Test32_PsrlqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsrlqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD3 CD", 3, Code.Psrlq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD3 CD", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrlqV_VX_WX_1_Data))]
		void Test64_PsrlqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsrlqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD3 08", 3, Code.Psrlq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD3 08", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrlqV_VX_WX_2_Data))]
		void Test64_PsrlqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsrlqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD3 CD", 3, Code.Psrlq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD3 CD", 4, Code.Psrlq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD3 CD", 4, Code.Psrlq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD3 CD", 5, Code.Psrlq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD3 CD", 5, Code.Psrlq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD3 CD", 5, Code.Psrlq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlqV_VX_HX_WX_1_Data))]
		void Test16_VpsrlqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsrlqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D3 10", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D3 10", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D3 10", 5, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D3 10", 5, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlqV_VX_HX_WX_2_Data))]
		void Test16_VpsrlqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsrlqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D3 D3", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D3 D3", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlqV_VX_HX_WX_1_Data))]
		void Test32_VpsrlqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsrlqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D3 10", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D3 10", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D3 10", 5, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D3 10", 5, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlqV_VX_HX_WX_2_Data))]
		void Test32_VpsrlqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsrlqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D3 D3", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D3 D3", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlqV_VX_HX_WX_1_Data))]
		void Test64_VpsrlqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsrlqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D3 10", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D3 10", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 D3 10", 5, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D3 10", 5, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlqV_VX_HX_WX_2_Data))]
		void Test64_VpsrlqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsrlqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D3 D3", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D3 D3", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 D3 D3", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D3 D3", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 D3 D3", 4, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D3 D3", 4, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 D3 D3", 5, Code.VEX_Vpsrlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D3 D3", 5, Code.VEX_Vpsrlq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsrlqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrlqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsrlqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsrlqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsrlqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrlqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsrlqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsrlqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsrlqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrlqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 D3 50 01", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 D3 50 01", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 D3 50 01", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsrlqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsrlqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18D8B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD03 D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD0B D3 D3", 6, Code.EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD2B D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DAB D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD23 D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD2B D3 D3", 6, Code.EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD4B D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DCB D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD43 D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD4B D3 D3", 6, Code.EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddqV_VX_WX_1_Data))]
		void Test16_PaddqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PaddqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD4 08", 3, Code.Paddq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD4 08", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddqV_VX_WX_2_Data))]
		void Test16_PaddqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PaddqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD4 CD", 3, Code.Paddq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD4 CD", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddqV_VX_WX_1_Data))]
		void Test32_PaddqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PaddqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD4 08", 3, Code.Paddq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD4 08", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddqV_VX_WX_2_Data))]
		void Test32_PaddqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PaddqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD4 CD", 3, Code.Paddq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD4 CD", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddqV_VX_WX_1_Data))]
		void Test64_PaddqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PaddqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD4 08", 3, Code.Paddq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FD4 08", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddqV_VX_WX_2_Data))]
		void Test64_PaddqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PaddqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD4 CD", 3, Code.Paddq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD4 CD", 4, Code.Paddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD4 CD", 5, Code.Paddq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD4 CD", 5, Code.Paddq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD4 CD", 5, Code.Paddq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddqV_VX_HX_WX_1_Data))]
		void Test16_VpaddqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpaddqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D4 10", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D4 10", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 D4 10", 5, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D4 10", 5, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddqV_VX_HX_WX_2_Data))]
		void Test16_VpaddqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpaddqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D4 D3", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D4 D3", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddqV_VX_HX_WX_1_Data))]
		void Test32_VpaddqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpaddqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D4 10", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D4 10", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 D4 10", 5, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D4 10", 5, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddqV_VX_HX_WX_2_Data))]
		void Test32_VpaddqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpaddqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D4 D3", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D4 D3", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddqV_VX_HX_WX_1_Data))]
		void Test64_VpaddqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpaddqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D4 10", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD D4 10", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 D4 10", 5, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD D4 10", 5, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddqV_VX_HX_WX_2_Data))]
		void Test64_VpaddqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpaddqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D4 D3", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D4 D3", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 D4 D3", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D4 D3", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 D4 D3", 4, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D4 D3", 4, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 D4 D3", 5, Code.VEX_Vpaddq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D4 D3", 5, Code.VEX_Vpaddq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpaddqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpaddqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpaddqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpaddqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpaddqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpaddqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpaddqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpaddqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpaddqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpaddqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 D4 50 01", 7, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 D4 50 01", 7, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 D4 50 01", 7, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpaddqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpaddqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B D4 D3", 6, Code.EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B D4 D3", 6, Code.EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B D4 D3", 6, Code.EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmullwV_VX_WX_1_Data))]
		void Test16_PmullwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmullwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD5 08", 3, Code.Pmullw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FD5 08", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmullwV_VX_WX_2_Data))]
		void Test16_PmullwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmullwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD5 CD", 3, Code.Pmullw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD5 CD", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmullwV_VX_WX_1_Data))]
		void Test32_PmullwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmullwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD5 08", 3, Code.Pmullw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FD5 08", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmullwV_VX_WX_2_Data))]
		void Test32_PmullwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmullwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD5 CD", 3, Code.Pmullw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD5 CD", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmullwV_VX_WX_1_Data))]
		void Test64_PmullwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmullwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD5 08", 3, Code.Pmullw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FD5 08", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmullwV_VX_WX_2_Data))]
		void Test64_PmullwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmullwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD5 CD", 3, Code.Pmullw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD5 CD", 4, Code.Pmullw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD5 CD", 4, Code.Pmullw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD5 CD", 5, Code.Pmullw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD5 CD", 5, Code.Pmullw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD5 CD", 5, Code.Pmullw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmullwV_VX_HX_WX_1_Data))]
		void Test16_VpmullwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmullwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D5 10", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD D5 10", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 D5 10", 5, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD D5 10", 5, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmullwV_VX_HX_WX_2_Data))]
		void Test16_VpmullwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmullwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D5 D3", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D5 D3", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmullwV_VX_HX_WX_1_Data))]
		void Test32_VpmullwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmullwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D5 10", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD D5 10", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 D5 10", 5, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD D5 10", 5, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmullwV_VX_HX_WX_2_Data))]
		void Test32_VpmullwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmullwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D5 D3", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D5 D3", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmullwV_VX_HX_WX_1_Data))]
		void Test64_VpmullwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmullwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D5 10", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD D5 10", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 D5 10", 5, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD D5 10", 5, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmullwV_VX_HX_WX_2_Data))]
		void Test64_VpmullwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmullwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D5 D3", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D5 D3", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 D5 D3", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D5 D3", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 D5 D3", 4, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D5 D3", 4, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 D5 D3", 5, Code.VEX_Vpmullw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D5 D3", 5, Code.VEX_Vpmullw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmullwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmullwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmullwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DaD D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DcD D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmullwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmullwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmullwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmullwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmullwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmullwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DaD D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DcD D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmullwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmullwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmullwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmullwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmullwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmullwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B D5 50 01", 7, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DaD D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B D5 50 01", 7, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DcD D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B D5 50 01", 7, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmullwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmullwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmullwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B D5 D3", 6, Code.EVEX_Vpmullw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B D5 D3", 6, Code.EVEX_Vpmullw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B D5 D3", 6, Code.EVEX_Vpmullw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Movdq2q_M_X_1_Data))]
		void Test16_Movdq2q_M_X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Movdq2q_M_X_1_Data {
			get {
				yield return new object[] { "F3 0FD6 CD", 4, Code.Movq2dq_xmm_mm, Register.XMM1, Register.MM5 };

				yield return new object[] { "F2 0FD6 CD", 4, Code.Movdq2q_mm_xmm, Register.MM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Movdq2q_M_X_1_Data))]
		void Test32_Movdq2q_M_X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Movdq2q_M_X_1_Data {
			get {
				yield return new object[] { "F3 0FD6 CD", 4, Code.Movq2dq_xmm_mm, Register.XMM1, Register.MM5 };

				yield return new object[] { "F2 0FD6 CD", 4, Code.Movdq2q_mm_xmm, Register.MM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Movdq2q_M_X_1_Data))]
		void Test64_Movdq2q_M_X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Movdq2q_M_X_1_Data {
			get {
				yield return new object[] { "F3 0FD6 CD", 4, Code.Movq2dq_xmm_mm, Register.XMM1, Register.MM5 };
				yield return new object[] { "F3 44 0FD6 CD", 5, Code.Movq2dq_xmm_mm, Register.XMM9, Register.MM5 };

				yield return new object[] { "F2 0FD6 CD", 4, Code.Movdq2q_mm_xmm, Register.MM1, Register.XMM5 };
				yield return new object[] { "F2 41 0FD6 CD", 5, Code.Movdq2q_mm_xmm, Register.MM1, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_1_Data))]
		void Test16_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0FD6 08", 4, Code.Movq_xmmm64_xmm, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5F9 D6 10", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 D6 10", 5, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_2_Data))]
		void Test16_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0FD6 CD", 4, Code.Movq_xmmm64_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F9 D6 CD", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM5, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_1_Data))]
		void Test32_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0FD6 08", 4, Code.Movq_xmmm64_xmm, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5F9 D6 10", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 D6 10", 5, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_2_Data))]
		void Test32_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0FD6 CD", 4, Code.Movq_xmmm64_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F9 D6 CD", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM5, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_1_Data))]
		void Test64_MovqV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "66 0FD6 08", 4, Code.Movq_xmmm64_xmm, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5F9 D6 10", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 D6 10", 5, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM2, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_2_Data))]
		void Test64_MovqV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0FD6 CD", 4, Code.Movq_xmmm64_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "66 44 0FD6 CD", 5, Code.Movq_xmmm64_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "66 41 0FD6 CD", 5, Code.Movq_xmmm64_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "66 45 0FD6 CD", 5, Code.Movq_xmmm64_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "C5F9 D6 CD", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C579 D6 CD", 4, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C179 D6 CD", 5, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44179 D6 CD", 5, Code.VEX_Vmovq_xmmm64_xmm, Register.XMM13, Register.XMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 50 01", 7, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_RegMem_Reg_EVEX_2_Data))]
		void Test16_MovqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_MovqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 D3", 6, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 50 01", 7, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_RegMem_Reg_EVEX_2_Data))]
		void Test32_MovqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_MovqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 D3", 6, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovqV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 50 01", 7, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM2, Register.None, MemorySize.UInt64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_RegMem_Reg_EVEX_2_Data))]
		void Test64_MovqV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_MovqV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD08 D6 D3", 6, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD08 D6 D3", 6, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM19, Register.XMM10, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD08 D6 D3", 6, Code.EVEX_Vmovq_xmmm64_xmm, Register.XMM11, Register.XMM18, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmovmskbV_Reg_Reg_1_Data))]
		void Test16_PmovmskbV_Reg_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmovmskbV_Reg_Reg_1_Data {
			get {
				yield return new object[] { "0FD7 CD", 3, Code.Pmovmskb_r32_mm, Register.ECX, Register.MM5 };

				yield return new object[] { "66 0FD7 CD", 4, Code.Pmovmskb_r32_xmm, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5F9 D7 CD", 4, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM5 };
				yield return new object[] { "C4E1F9 D7 CD", 5, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FD D7 CD", 4, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM5 };
				yield return new object[] { "C4E1FD D7 CD", 5, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmovmskbV_Reg_Reg_1_Data))]
		void Test32_PmovmskbV_Reg_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmovmskbV_Reg_Reg_1_Data {
			get {
				yield return new object[] { "0FD7 CD", 3, Code.Pmovmskb_r32_mm, Register.ECX, Register.MM5 };

				yield return new object[] { "66 0FD7 CD", 4, Code.Pmovmskb_r32_xmm, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5F9 D7 CD", 4, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM5 };
				yield return new object[] { "C4E1F9 D7 CD", 5, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FD D7 CD", 4, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM5 };
				yield return new object[] { "C4E1FD D7 CD", 5, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmovmskbV_Reg_Reg_1_Data))]
		void Test64_PmovmskbV_Reg_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmovmskbV_Reg_Reg_1_Data {
			get {
				yield return new object[] { "0FD7 CD", 3, Code.Pmovmskb_r32_mm, Register.ECX, Register.MM5 };
				yield return new object[] { "44 0FD7 CD", 4, Code.Pmovmskb_r32_mm, Register.R9D, Register.MM5 };

				yield return new object[] { "48 0FD7 CD", 4, Code.Pmovmskb_r64_mm, Register.RCX, Register.MM5 };
				yield return new object[] { "4C 0FD7 CD", 4, Code.Pmovmskb_r64_mm, Register.R9, Register.MM5 };

				yield return new object[] { "66 0FD7 CD", 4, Code.Pmovmskb_r32_xmm, Register.ECX, Register.XMM5 };
				yield return new object[] { "66 44 0FD7 CD", 5, Code.Pmovmskb_r32_xmm, Register.R9D, Register.XMM5 };
				yield return new object[] { "66 41 0FD7 CD", 5, Code.Pmovmskb_r32_xmm, Register.ECX, Register.XMM13 };
				yield return new object[] { "66 45 0FD7 CD", 5, Code.Pmovmskb_r32_xmm, Register.R9D, Register.XMM13 };

				yield return new object[] { "66 48 0FD7 CD", 5, Code.Pmovmskb_r64_xmm, Register.RCX, Register.XMM5 };
				yield return new object[] { "66 4C 0FD7 CD", 5, Code.Pmovmskb_r64_xmm, Register.R9, Register.XMM5 };
				yield return new object[] { "66 49 0FD7 CD", 5, Code.Pmovmskb_r64_xmm, Register.RCX, Register.XMM13 };
				yield return new object[] { "66 4D 0FD7 CD", 5, Code.Pmovmskb_r64_xmm, Register.R9, Register.XMM13 };

				yield return new object[] { "C5F9 D7 CD", 4, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM5 };
				yield return new object[] { "C579 D7 CD", 4, Code.VEX_Vpmovmskb_r32_xmm, Register.R9D, Register.XMM5 };
				yield return new object[] { "C4C179 D7 CD", 5, Code.VEX_Vpmovmskb_r32_xmm, Register.ECX, Register.XMM13 };
				yield return new object[] { "C44179 D7 CD", 5, Code.VEX_Vpmovmskb_r32_xmm, Register.R9D, Register.XMM13 };

				yield return new object[] { "C4E1F9 D7 CD", 5, Code.VEX_Vpmovmskb_r64_xmm, Register.RCX, Register.XMM5 };
				yield return new object[] { "C461F9 D7 CD", 5, Code.VEX_Vpmovmskb_r64_xmm, Register.R9, Register.XMM5 };
				yield return new object[] { "C4C1F9 D7 CD", 5, Code.VEX_Vpmovmskb_r64_xmm, Register.RCX, Register.XMM13 };
				yield return new object[] { "C441F9 D7 CD", 5, Code.VEX_Vpmovmskb_r64_xmm, Register.R9, Register.XMM13 };

				yield return new object[] { "C5FD D7 CD", 4, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM5 };
				yield return new object[] { "C57D D7 CD", 4, Code.VEX_Vpmovmskb_r32_ymm, Register.R9D, Register.YMM5 };
				yield return new object[] { "C4C17D D7 CD", 5, Code.VEX_Vpmovmskb_r32_ymm, Register.ECX, Register.YMM13 };
				yield return new object[] { "C4417D D7 CD", 5, Code.VEX_Vpmovmskb_r32_ymm, Register.R9D, Register.YMM13 };

				yield return new object[] { "C4E1FD D7 CD", 5, Code.VEX_Vpmovmskb_r64_ymm, Register.RCX, Register.YMM5 };
				yield return new object[] { "C461FD D7 CD", 5, Code.VEX_Vpmovmskb_r64_ymm, Register.R9, Register.YMM5 };
				yield return new object[] { "C4C1FD D7 CD", 5, Code.VEX_Vpmovmskb_r64_ymm, Register.RCX, Register.YMM13 };
				yield return new object[] { "C441FD D7 CD", 5, Code.VEX_Vpmovmskb_r64_ymm, Register.R9, Register.YMM13 };
			}
		}
	}
}
