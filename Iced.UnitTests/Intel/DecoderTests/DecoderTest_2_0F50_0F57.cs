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
	public sealed class DecoderTest_2_0F50_0F57 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_MovmskV_Gd_RX_2_Data))]
		void Test16_MovmskV_Gd_RX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovmskV_Gd_RX_2_Data {
			get {
				yield return new object[] { "0F50 CD", 3, Code.Movmskps_Gd_RX, Register.ECX, Register.XMM5 };

				yield return new object[] { "66 0F50 CD", 4, Code.Movmskpd_Gd_RX, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5F8 50 D3", 4, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C4E1F8 50 D3", 5, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM3 };

				yield return new object[] { "C5FC 50 D3", 4, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C4E1FC 50 D3", 5, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM3 };

				yield return new object[] { "C5F9 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C4E1F9 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM3 };

				yield return new object[] { "C5FD 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C4E1FD 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovmskV_Gd_RX_2_Data))]
		void Test32_MovmskV_Gd_RX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovmskV_Gd_RX_2_Data {
			get {
				yield return new object[] { "0F50 CD", 3, Code.Movmskps_Gd_RX, Register.ECX, Register.XMM5 };

				yield return new object[] { "66 0F50 CD", 4, Code.Movmskpd_Gd_RX, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5F8 50 D3", 4, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C4E1F8 50 D3", 5, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM3 };

				yield return new object[] { "C5FC 50 D3", 4, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C4E1FC 50 D3", 5, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM3 };

				yield return new object[] { "C5F9 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C4E1F9 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM3 };

				yield return new object[] { "C5FD 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C4E1FD 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovmskV_Gd_RX_2_Data))]
		void Test64_MovmskV_Gd_RX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovmskV_Gd_RX_2_Data {
			get {
				yield return new object[] { "0F50 CD", 3, Code.Movmskps_Gd_RX, Register.ECX, Register.XMM5 };
				yield return new object[] { "44 0F50 CD", 4, Code.Movmskps_Gd_RX, Register.R9D, Register.XMM5 };
				yield return new object[] { "41 0F50 CD", 4, Code.Movmskps_Gd_RX, Register.ECX, Register.XMM13 };
				yield return new object[] { "45 0F50 CD", 4, Code.Movmskps_Gd_RX, Register.R9D, Register.XMM13 };

				yield return new object[] { "48 0F50 CD", 4, Code.Movmskps_Gq_RX, Register.RCX, Register.XMM5 };
				yield return new object[] { "4C 0F50 CD", 4, Code.Movmskps_Gq_RX, Register.R9, Register.XMM5 };
				yield return new object[] { "49 0F50 CD", 4, Code.Movmskps_Gq_RX, Register.RCX, Register.XMM13 };
				yield return new object[] { "4D 0F50 CD", 4, Code.Movmskps_Gq_RX, Register.R9, Register.XMM13 };

				yield return new object[] { "66 0F50 CD", 4, Code.Movmskpd_Gd_RX, Register.ECX, Register.XMM5 };
				yield return new object[] { "66 44 0F50 CD", 5, Code.Movmskpd_Gd_RX, Register.R9D, Register.XMM5 };
				yield return new object[] { "66 41 0F50 CD", 5, Code.Movmskpd_Gd_RX, Register.ECX, Register.XMM13 };
				yield return new object[] { "66 45 0F50 CD", 5, Code.Movmskpd_Gd_RX, Register.R9D, Register.XMM13 };

				yield return new object[] { "66 48 0F50 CD", 5, Code.Movmskpd_Gq_RX, Register.RCX, Register.XMM5 };
				yield return new object[] { "66 4C 0F50 CD", 5, Code.Movmskpd_Gq_RX, Register.R9, Register.XMM5 };
				yield return new object[] { "66 49 0F50 CD", 5, Code.Movmskpd_Gq_RX, Register.RCX, Register.XMM13 };
				yield return new object[] { "66 4D 0F50 CD", 5, Code.Movmskpd_Gq_RX, Register.R9, Register.XMM13 };

				yield return new object[] { "C5F8 50 D3", 4, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C578 50 D3", 4, Code.VEX_Vmovmskps_Gd_RX, Register.R10D, Register.XMM3 };
				yield return new object[] { "C4C178 50 D3", 5, Code.VEX_Vmovmskps_Gd_RX, Register.EDX, Register.XMM11 };
				yield return new object[] { "C44178 50 D3", 5, Code.VEX_Vmovmskps_Gd_RX, Register.R10D, Register.XMM11 };

				yield return new object[] { "C4E1F8 50 D3", 5, Code.VEX_Vmovmskps_Gq_RX, Register.RDX, Register.XMM3 };
				yield return new object[] { "C461F8 50 D3", 5, Code.VEX_Vmovmskps_Gq_RX, Register.R10, Register.XMM3 };
				yield return new object[] { "C4C1F8 50 D3", 5, Code.VEX_Vmovmskps_Gq_RX, Register.RDX, Register.XMM11 };
				yield return new object[] { "C441F8 50 D3", 5, Code.VEX_Vmovmskps_Gq_RX, Register.R10, Register.XMM11 };

				yield return new object[] { "C5FC 50 D3", 4, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C57C 50 D3", 4, Code.VEX_Vmovmskps_Gd_RY, Register.R10D, Register.YMM3 };
				yield return new object[] { "C4C17C 50 D3", 5, Code.VEX_Vmovmskps_Gd_RY, Register.EDX, Register.YMM11 };
				yield return new object[] { "C4417C 50 D3", 5, Code.VEX_Vmovmskps_Gd_RY, Register.R10D, Register.YMM11 };

				yield return new object[] { "C4E1FC 50 D3", 5, Code.VEX_Vmovmskps_Gq_RY, Register.RDX, Register.YMM3 };
				yield return new object[] { "C461FC 50 D3", 5, Code.VEX_Vmovmskps_Gq_RY, Register.R10, Register.YMM3 };
				yield return new object[] { "C4C1FC 50 D3", 5, Code.VEX_Vmovmskps_Gq_RY, Register.RDX, Register.YMM11 };
				yield return new object[] { "C441FC 50 D3", 5, Code.VEX_Vmovmskps_Gq_RY, Register.R10, Register.YMM11 };

				yield return new object[] { "C5F9 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM3 };
				yield return new object[] { "C579 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RX, Register.R10D, Register.XMM3 };
				yield return new object[] { "C4C179 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RX, Register.EDX, Register.XMM11 };
				yield return new object[] { "C44179 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RX, Register.R10D, Register.XMM11 };

				yield return new object[] { "C4E1F9 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RX, Register.RDX, Register.XMM3 };
				yield return new object[] { "C461F9 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RX, Register.R10, Register.XMM3 };
				yield return new object[] { "C4C1F9 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RX, Register.RDX, Register.XMM11 };
				yield return new object[] { "C441F9 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RX, Register.R10, Register.XMM11 };

				yield return new object[] { "C5FD 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM3 };
				yield return new object[] { "C57D 50 D3", 4, Code.VEX_Vmovmskpd_Gd_RY, Register.R10D, Register.YMM3 };
				yield return new object[] { "C4C17D 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RY, Register.EDX, Register.YMM11 };
				yield return new object[] { "C4417D 50 D3", 5, Code.VEX_Vmovmskpd_Gd_RY, Register.R10D, Register.YMM11 };

				yield return new object[] { "C4E1FD 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RY, Register.RDX, Register.YMM3 };
				yield return new object[] { "C461FD 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RY, Register.R10, Register.YMM3 };
				yield return new object[] { "C4C1FD 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RY, Register.RDX, Register.YMM11 };
				yield return new object[] { "C441FD 50 D3", 5, Code.VEX_Vmovmskpd_Gq_RY, Register.R10, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_SqrtV_VX_WX_1_Data))]
		void Test16_SqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_SqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F51 08", 3, Code.Sqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F51 08", 4, Code.Sqrtpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F51 08", 4, Code.Sqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F51 08", 4, Code.Sqrtsd_VX_WX, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 51 10", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 51 10", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 51 10", 5, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 51 10", 5, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 51 10", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5FD 51 10", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1F9 51 10", 5, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FD 51 10", 5, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_SqrtV_VX_WX_2_Data))]
		void Test16_SqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_SqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F51 CD", 3, Code.Sqrtps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F51 CD", 4, Code.Sqrtpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F51 CD", 4, Code.Sqrtss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F51 CD", 4, Code.Sqrtsd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 51 D3", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 51 D3", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, Register.YMM3 };

				yield return new object[] { "C5F9 51 D3", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FD 51 D3", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_SqrtV_VX_WX_1_Data))]
		void Test32_SqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_SqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F51 08", 3, Code.Sqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F51 08", 4, Code.Sqrtpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F51 08", 4, Code.Sqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F51 08", 4, Code.Sqrtsd_VX_WX, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 51 10", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 51 10", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 51 10", 5, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 51 10", 5, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 51 10", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5FD 51 10", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1F9 51 10", 5, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FD 51 10", 5, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_SqrtV_VX_WX_2_Data))]
		void Test32_SqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_SqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F51 CD", 3, Code.Sqrtps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F51 CD", 4, Code.Sqrtpd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F51 CD", 4, Code.Sqrtss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F51 CD", 4, Code.Sqrtsd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 51 D3", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 51 D3", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, Register.YMM3 };

				yield return new object[] { "C5F9 51 D3", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FD 51 D3", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_SqrtV_VX_WX_1_Data))]
		void Test64_SqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_SqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F51 08", 3, Code.Sqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F51 08", 4, Code.Sqrtpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F51 08", 4, Code.Sqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F51 08", 4, Code.Sqrtsd_VX_WX, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 51 10", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 51 10", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 51 10", 5, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 51 10", 5, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 51 10", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5FD 51 10", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1F9 51 10", 5, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FD 51 10", 5, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_SqrtV_VX_WX_2_Data))]
		void Test64_SqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_SqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F51 CD", 3, Code.Sqrtps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F51 CD", 4, Code.Sqrtps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F51 CD", 4, Code.Sqrtps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F51 CD", 4, Code.Sqrtps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F51 CD", 4, Code.Sqrtpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F51 CD", 5, Code.Sqrtpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F51 CD", 5, Code.Sqrtpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F51 CD", 5, Code.Sqrtpd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F51 CD", 4, Code.Sqrtss_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F51 CD", 5, Code.Sqrtss_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F51 CD", 5, Code.Sqrtss_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F51 CD", 5, Code.Sqrtss_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F51 CD", 4, Code.Sqrtsd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F51 CD", 5, Code.Sqrtsd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F51 CD", 5, Code.Sqrtsd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F51 CD", 5, Code.Sqrtsd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 51 D3", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 51 D3", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, Register.YMM3 };
				yield return new object[] { "C578 51 D3", 4, Code.VEX_Vsqrtps_VX_WX, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C57C 51 D3", 4, Code.VEX_Vsqrtps_VY_WY, Register.YMM10, Register.YMM3 };
				yield return new object[] { "C4C178 51 D3", 5, Code.VEX_Vsqrtps_VX_WX, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C4C17C 51 D3", 5, Code.VEX_Vsqrtps_VY_WY, Register.YMM2, Register.YMM11 };

				yield return new object[] { "C5F9 51 D3", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FD 51 D3", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, Register.YMM3 };
				yield return new object[] { "C579 51 D3", 4, Code.VEX_Vsqrtpd_VX_WX, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C57D 51 D3", 4, Code.VEX_Vsqrtpd_VY_WY, Register.YMM10, Register.YMM3 };
				yield return new object[] { "C4C179 51 D3", 5, Code.VEX_Vsqrtpd_VX_WX, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C4C17D 51 D3", 5, Code.VEX_Vsqrtpd_VY_WY, Register.YMM2, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_HX_WX_1_Data))]
		void Test16_VsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_HX_WX_2_Data))]
		void Test16_VsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 51 D3", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 51 D3", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_HX_WX_1_Data))]
		void Test32_VsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_HX_WX_2_Data))]
		void Test32_VsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 51 D3", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5CB 51 D3", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_HX_WX_1_Data))]
		void Test64_VsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 51 10", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 51 10", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C5CB 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CB 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C5CF 51 10", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1CF 51 10", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_HX_WX_2_Data))]
		void Test64_VsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 51 D3", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 51 D3", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 51 D3", 4, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 51 D3", 5, Code.VEX_Vsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C5CB 51 D3", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54B 51 D3", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58B 51 D3", 4, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14B 51 D3", 5, Code.VEX_Vsqrtsd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_k1_HX_WX_1_Data))]
		void Test16_VsqrtV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VsqrtV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17C0B 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9D 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F17C08 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C2B 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBD 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F17C28 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C4B 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDD 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F17C48 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD0B 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9D 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1FD08 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD2B 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBD 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1FD28 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD4B 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDD 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1FD48 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_k1_HX_WX_2_Data))]
		void Test16_VsqrtV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VsqrtV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14E0B 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_k1_HX_WX_3_Data))]
		void Test16_VsqrtV_VX_k1_HX_WX_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VsqrtV_VX_k1_HX_WX_3_Data {
			get {
				yield return new object[] { "62 F17C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17CDB 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F17C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C1B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F17C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C3B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1FD8B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FDDB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1FDAB 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FD1B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1FDCB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FD7B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VsqrtV_VX_k1_HX_WX_4_Data))]
		void Test16_VsqrtV_VX_k1_HX_WX_4(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VsqrtV_VX_k1_HX_WX_4_Data {
			get {
				yield return new object[] { "62 F14E0B 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_k1_HX_WX_1_Data))]
		void Test32_VsqrtV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VsqrtV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17C0B 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9D 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F17C08 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C2B 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBD 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F17C28 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C4B 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDD 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F17C48 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD0B 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9D 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1FD08 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD2B 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBD 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1FD28 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD4B 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDD 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1FD48 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_k1_HX_WX_2_Data))]
		void Test32_VsqrtV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VsqrtV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14E0B 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_k1_HX_WX_3_Data))]
		void Test32_VsqrtV_VX_k1_HX_WX_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VsqrtV_VX_k1_HX_WX_3_Data {
			get {
				yield return new object[] { "62 F17C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17CDB 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F17C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C1B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F17C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C3B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1FD8B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FDDB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1FDAB 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FD1B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1FDCB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1FD7B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VsqrtV_VX_k1_HX_WX_4_Data))]
		void Test32_VsqrtV_VX_k1_HX_WX_4(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VsqrtV_VX_k1_HX_WX_4_Data {
			get {
				yield return new object[] { "62 F14E0B 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F1CF7B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_k1_HX_WX_1_Data))]
		void Test64_VsqrtV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VsqrtV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17C0B 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C9D 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F17C08 51 50 01", 7, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C2B 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CBD 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F17C28 51 50 01", 7, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C4B 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CDD 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F17C48 51 50 01", 7, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD0B 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9D 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1FD08 51 50 01", 7, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD2B 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBD 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1FD28 51 50 01", 7, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD4B 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDD 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1FD48 51 50 01", 7, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_k1_HX_WX_2_Data))]
		void Test64_VsqrtV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VsqrtV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14E0B 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14E08 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F14EAB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14ECB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F14EEB 51 50 01", 7, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1CF0B 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CF08 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1CFAB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFCB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1CFEB 51 50 01", 7, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_k1_HX_WX_3_Data))]
		void Test64_VsqrtV_VX_k1_HX_WX_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VsqrtV_VX_k1_HX_WX_3_Data {
			get {
				yield return new object[] { "62 F17C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E17C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 117C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B17C0B 51 D3", 6, Code.EVEX_Vsqrtps_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17CDB 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F17C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E17C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 117C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B17C2B 51 D3", 6, Code.EVEX_Vsqrtps_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C1B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F17C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E17C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 117C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B17C4B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F17C3B 51 D3", 6, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false };

				yield return new object[] { "62 F1FD8B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E1FD0B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11FD0B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1FD0B 51 D3", 6, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1FDDB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1FDAB 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E1FD2B 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11FD2B 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1FD2B 51 D3", 6, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1FD1B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F1FDCB 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E1FD4B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11FD4B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1FD4B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1FD7B 51 D3", 6, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VsqrtV_VX_k1_HX_WX_4_Data))]
		void Test64_VsqrtV_VX_k1_HX_WX_4(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VsqrtV_VX_k1_HX_WX_4_Data {
			get {
				yield return new object[] { "62 F14E0B 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 E10E0B 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 114E03 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B14E0B 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F14EDB 51 D3", 6, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F1CF8B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E18F0B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 11CF03 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B1CF0B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F1CF7B 51 D3", 6, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RsqrtV_VX_WX_1_Data))]
		void Test16_RsqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_RsqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F52 08", 3, Code.Rsqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F52 08", 4, Code.Rsqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 52 10", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 52 10", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 52 10", 5, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 52 10", 5, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RsqrtV_VX_WX_2_Data))]
		void Test16_RsqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_RsqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F52 CD", 3, Code.Rsqrtps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F52 CD", 4, Code.Rsqrtss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 52 D3", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 52 D3", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RsqrtV_VX_WX_1_Data))]
		void Test32_RsqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_RsqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F52 08", 3, Code.Rsqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F52 08", 4, Code.Rsqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 52 10", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 52 10", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 52 10", 5, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 52 10", 5, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RsqrtV_VX_WX_2_Data))]
		void Test32_RsqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_RsqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F52 CD", 3, Code.Rsqrtps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F52 CD", 4, Code.Rsqrtss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 52 D3", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 52 D3", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RsqrtV_VX_WX_1_Data))]
		void Test64_RsqrtV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_RsqrtV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F52 08", 3, Code.Rsqrtps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F52 08", 4, Code.Rsqrtss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 52 10", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 52 10", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 52 10", 5, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 52 10", 5, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RsqrtV_VX_WX_2_Data))]
		void Test64_RsqrtV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_RsqrtV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F52 CD", 3, Code.Rsqrtps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F52 CD", 4, Code.Rsqrtps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F52 CD", 4, Code.Rsqrtps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F52 CD", 4, Code.Rsqrtps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F52 CD", 4, Code.Rsqrtss_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F52 CD", 5, Code.Rsqrtss_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F52 CD", 5, Code.Rsqrtss_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F52 CD", 5, Code.Rsqrtss_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 52 D3", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 52 D3", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, Register.YMM3 };
				yield return new object[] { "C578 52 D3", 4, Code.VEX_Vrsqrtps_VX_WX, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C57C 52 D3", 4, Code.VEX_Vrsqrtps_VY_WY, Register.YMM10, Register.YMM3 };
				yield return new object[] { "C4C178 52 D3", 5, Code.VEX_Vrsqrtps_VX_WX, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C4C17C 52 D3", 5, Code.VEX_Vrsqrtps_VY_WY, Register.YMM2, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrsqrtV_VX_HX_WX_1_Data))]
		void Test16_VrsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VrsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrsqrtV_VX_HX_WX_2_Data))]
		void Test16_VrsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VrsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 52 D3", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrsqrtV_VX_HX_WX_1_Data))]
		void Test32_VrsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VrsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrsqrtV_VX_HX_WX_2_Data))]
		void Test32_VrsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VrsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 52 D3", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrsqrtV_VX_HX_WX_1_Data))]
		void Test64_VrsqrtV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VrsqrtV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 52 10", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 52 10", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrsqrtV_VX_HX_WX_2_Data))]
		void Test64_VrsqrtV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VrsqrtV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 52 D3", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 52 D3", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 52 D3", 4, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 52 D3", 5, Code.VEX_Vrsqrtss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RcpV_VX_WX_1_Data))]
		void Test16_RcpV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_RcpV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F53 08", 3, Code.Rcpps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F53 08", 4, Code.Rcpss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 53 10", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 53 10", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 53 10", 5, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 53 10", 5, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RcpV_VX_WX_2_Data))]
		void Test16_RcpV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_RcpV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F53 CD", 3, Code.Rcpps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F53 CD", 4, Code.Rcpss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 53 D3", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 53 D3", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RcpV_VX_WX_1_Data))]
		void Test32_RcpV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_RcpV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F53 08", 3, Code.Rcpps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F53 08", 4, Code.Rcpss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 53 10", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 53 10", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 53 10", 5, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 53 10", 5, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RcpV_VX_WX_2_Data))]
		void Test32_RcpV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_RcpV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F53 CD", 3, Code.Rcpps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0F53 CD", 4, Code.Rcpss_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 53 D3", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 53 D3", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RcpV_VX_WX_1_Data))]
		void Test64_RcpV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_RcpV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F53 08", 3, Code.Rcpps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "F3 0F53 08", 4, Code.Rcpss_VX_WX, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "C5F8 53 10", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5FC 53 10", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1F8 53 10", 5, Code.VEX_Vrcpps_VX_WX, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FC 53 10", 5, Code.VEX_Vrcpps_VY_WY, Register.YMM2, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RcpV_VX_WX_2_Data))]
		void Test64_RcpV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_RcpV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F53 CD", 3, Code.Rcpps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F53 CD", 4, Code.Rcpps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F53 CD", 4, Code.Rcpps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F53 CD", 4, Code.Rcpps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0F53 CD", 4, Code.Rcpss_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F53 CD", 5, Code.Rcpss_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F53 CD", 5, Code.Rcpss_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F53 CD", 5, Code.Rcpss_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 53 D3", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C5FC 53 D3", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM2, Register.YMM3 };
				yield return new object[] { "C578 53 D3", 4, Code.VEX_Vrcpps_VX_WX, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C57C 53 D3", 4, Code.VEX_Vrcpps_VY_WY, Register.YMM10, Register.YMM3 };
				yield return new object[] { "C4C178 53 D3", 5, Code.VEX_Vrcpps_VX_WX, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C4C17C 53 D3", 5, Code.VEX_Vrcpps_VY_WY, Register.YMM2, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrcpV_VX_HX_WX_1_Data))]
		void Test16_VrcpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VrcpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrcpV_VX_HX_WX_2_Data))]
		void Test16_VrcpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VrcpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 53 D3", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrcpV_VX_HX_WX_1_Data))]
		void Test32_VrcpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VrcpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrcpV_VX_HX_WX_2_Data))]
		void Test32_VrcpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VrcpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 53 D3", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrcpV_VX_HX_WX_1_Data))]
		void Test64_VrcpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VrcpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5CA 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CA 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C5CE 53 10", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E1CE 53 10", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrcpV_VX_HX_WX_2_Data))]
		void Test64_VrcpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VrcpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5CA 53 D3", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54A 53 D3", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C58A 53 D3", 4, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C14A 53 D3", 5, Code.VEX_Vrcpss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AndV_VX_WX_1_Data))]
		void Test16_AndV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_AndV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F54 08", 3, Code.Andps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F54 08", 4, Code.Andpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AndV_VX_WX_2_Data))]
		void Test16_AndV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AndV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F54 CD", 3, Code.Andps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F54 CD", 4, Code.Andpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AndV_VX_WX_1_Data))]
		void Test32_AndV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_AndV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F54 08", 3, Code.Andps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F54 08", 4, Code.Andpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AndV_VX_WX_2_Data))]
		void Test32_AndV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AndV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F54 CD", 3, Code.Andps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F54 CD", 4, Code.Andpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AndV_VX_WX_1_Data))]
		void Test64_AndV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_AndV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F54 08", 3, Code.Andps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F54 08", 4, Code.Andpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AndV_VX_WX_2_Data))]
		void Test64_AndV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AndV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F54 CD", 3, Code.Andps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F54 CD", 4, Code.Andps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F54 CD", 4, Code.Andps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F54 CD", 4, Code.Andps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F54 CD", 4, Code.Andpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F54 CD", 5, Code.Andpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F54 CD", 5, Code.Andpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F54 CD", 5, Code.Andpd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandV_VX_HX_WX_1_Data))]
		void Test16_VandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 54 10", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 54 10", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 54 10", 5, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 54 10", 5, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 54 10", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 54 10", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 54 10", 5, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 54 10", 5, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandV_VX_HX_WX_2_Data))]
		void Test16_VandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 54 D3", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 54 D3", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 54 D3", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 54 D3", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandV_VX_HX_WX_1_Data))]
		void Test32_VandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 54 10", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 54 10", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 54 10", 5, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 54 10", 5, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 54 10", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 54 10", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 54 10", 5, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 54 10", 5, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandV_VX_HX_WX_2_Data))]
		void Test32_VandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 54 D3", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 54 D3", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 54 D3", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 54 D3", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandV_VX_HX_WX_1_Data))]
		void Test64_VandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 54 10", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 54 10", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 54 10", 5, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 54 10", 5, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 54 10", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 54 10", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 54 10", 5, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 54 10", 5, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandV_VX_HX_WX_2_Data))]
		void Test64_VandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 54 D3", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 54 D3", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 54 D3", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 54 D3", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 54 D3", 4, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 54 D3", 4, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 54 D3", 5, Code.VEX_Vandps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 54 D3", 5, Code.VEX_Vandps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 54 D3", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 54 D3", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 54 D3", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 54 D3", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 54 D3", 4, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 54 D3", 4, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 54 D3", 5, Code.VEX_Vandpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 54 D3", 5, Code.VEX_Vandpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandV_VX_k1_HX_WX_1_Data))]
		void Test16_VandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandV_VX_k1_HX_WX_2_Data))]
		void Test16_VandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandV_VX_k1_HX_WX_1_Data))]
		void Test32_VandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandV_VX_k1_HX_WX_2_Data))]
		void Test32_VandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandV_VX_k1_HX_WX_1_Data))]
		void Test64_VandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 54 50 01", 7, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 54 50 01", 7, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 54 50 01", 7, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 54 50 01", 7, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 54 50 01", 7, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 54 50 01", 7, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandV_VX_k1_HX_WX_2_Data))]
		void Test64_VandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 54 D3", 6, Code.EVEX_Vandps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 54 D3", 6, Code.EVEX_Vandps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 54 D3", 6, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 54 D3", 6, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 54 D3", 6, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 54 D3", 6, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AndnV_VX_WX_1_Data))]
		void Test16_AndnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_AndnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F55 08", 3, Code.Andnps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F55 08", 4, Code.Andnpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AndnV_VX_WX_2_Data))]
		void Test16_AndnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AndnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F55 CD", 3, Code.Andnps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F55 CD", 4, Code.Andnpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AndnV_VX_WX_1_Data))]
		void Test32_AndnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_AndnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F55 08", 3, Code.Andnps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F55 08", 4, Code.Andnpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AndnV_VX_WX_2_Data))]
		void Test32_AndnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AndnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F55 CD", 3, Code.Andnps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F55 CD", 4, Code.Andnpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AndnV_VX_WX_1_Data))]
		void Test64_AndnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_AndnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F55 08", 3, Code.Andnps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F55 08", 4, Code.Andnpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AndnV_VX_WX_2_Data))]
		void Test64_AndnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AndnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F55 CD", 3, Code.Andnps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F55 CD", 4, Code.Andnps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F55 CD", 4, Code.Andnps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F55 CD", 4, Code.Andnps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F55 CD", 4, Code.Andnpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F55 CD", 5, Code.Andnpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F55 CD", 5, Code.Andnpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F55 CD", 5, Code.Andnpd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandnV_VX_HX_WX_1_Data))]
		void Test16_VandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 55 10", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 55 10", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 55 10", 5, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 55 10", 5, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 55 10", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 55 10", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 55 10", 5, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 55 10", 5, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandnV_VX_HX_WX_2_Data))]
		void Test16_VandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 55 D3", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 55 D3", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 55 D3", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 55 D3", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandnV_VX_HX_WX_1_Data))]
		void Test32_VandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 55 10", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 55 10", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 55 10", 5, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 55 10", 5, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 55 10", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 55 10", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 55 10", 5, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 55 10", 5, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandnV_VX_HX_WX_2_Data))]
		void Test32_VandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 55 D3", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 55 D3", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 55 D3", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 55 D3", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandnV_VX_HX_WX_1_Data))]
		void Test64_VandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 55 10", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 55 10", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 55 10", 5, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 55 10", 5, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 55 10", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 55 10", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 55 10", 5, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 55 10", 5, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandnV_VX_HX_WX_2_Data))]
		void Test64_VandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 55 D3", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 55 D3", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 55 D3", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 55 D3", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 55 D3", 4, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 55 D3", 4, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 55 D3", 5, Code.VEX_Vandnps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 55 D3", 5, Code.VEX_Vandnps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 55 D3", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 55 D3", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 55 D3", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 55 D3", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 55 D3", 4, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 55 D3", 4, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 55 D3", 5, Code.VEX_Vandnpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 55 D3", 5, Code.VEX_Vandnpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandnV_VX_k1_HX_WX_1_Data))]
		void Test16_VandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VandnV_VX_k1_HX_WX_2_Data))]
		void Test16_VandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandnV_VX_k1_HX_WX_1_Data))]
		void Test32_VandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VandnV_VX_k1_HX_WX_2_Data))]
		void Test32_VandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandnV_VX_k1_HX_WX_1_Data))]
		void Test64_VandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 55 50 01", 7, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 55 50 01", 7, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 55 50 01", 7, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 55 50 01", 7, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 55 50 01", 7, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 55 50 01", 7, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VandnV_VX_k1_HX_WX_2_Data))]
		void Test64_VandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 55 D3", 6, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 55 D3", 6, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 55 D3", 6, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 55 D3", 6, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 55 D3", 6, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 55 D3", 6, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_OrV_VX_WX_1_Data))]
		void Test16_OrV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_OrV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F56 08", 3, Code.Orps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F56 08", 4, Code.Orpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_OrV_VX_WX_2_Data))]
		void Test16_OrV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_OrV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F56 CD", 3, Code.Orps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F56 CD", 4, Code.Orpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_OrV_VX_WX_1_Data))]
		void Test32_OrV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_OrV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F56 08", 3, Code.Orps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F56 08", 4, Code.Orpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_OrV_VX_WX_2_Data))]
		void Test32_OrV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_OrV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F56 CD", 3, Code.Orps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F56 CD", 4, Code.Orpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_OrV_VX_WX_1_Data))]
		void Test64_OrV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_OrV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F56 08", 3, Code.Orps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F56 08", 4, Code.Orpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_OrV_VX_WX_2_Data))]
		void Test64_OrV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_OrV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F56 CD", 3, Code.Orps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F56 CD", 4, Code.Orps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F56 CD", 4, Code.Orps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F56 CD", 4, Code.Orps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F56 CD", 4, Code.Orpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F56 CD", 5, Code.Orpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F56 CD", 5, Code.Orpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F56 CD", 5, Code.Orpd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VorV_VX_HX_WX_1_Data))]
		void Test16_VorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 56 10", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 56 10", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 56 10", 5, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 56 10", 5, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 56 10", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 56 10", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 56 10", 5, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 56 10", 5, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VorV_VX_HX_WX_2_Data))]
		void Test16_VorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 56 D3", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 56 D3", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 56 D3", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 56 D3", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VorV_VX_HX_WX_1_Data))]
		void Test32_VorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 56 10", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 56 10", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 56 10", 5, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 56 10", 5, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 56 10", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 56 10", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 56 10", 5, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 56 10", 5, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VorV_VX_HX_WX_2_Data))]
		void Test32_VorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 56 D3", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 56 D3", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 56 D3", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 56 D3", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VorV_VX_HX_WX_1_Data))]
		void Test64_VorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 56 10", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 56 10", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 56 10", 5, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 56 10", 5, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 56 10", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 56 10", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 56 10", 5, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 56 10", 5, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VorV_VX_HX_WX_2_Data))]
		void Test64_VorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 56 D3", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 56 D3", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 56 D3", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 56 D3", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 56 D3", 4, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 56 D3", 4, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 56 D3", 5, Code.VEX_Vorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 56 D3", 5, Code.VEX_Vorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 56 D3", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 56 D3", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 56 D3", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 56 D3", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 56 D3", 4, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 56 D3", 4, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 56 D3", 5, Code.VEX_Vorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 56 D3", 5, Code.VEX_Vorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VorV_VX_k1_HX_WX_1_Data))]
		void Test16_VorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VorV_VX_k1_HX_WX_2_Data))]
		void Test16_VorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VorV_VX_k1_HX_WX_1_Data))]
		void Test32_VorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VorV_VX_k1_HX_WX_2_Data))]
		void Test32_VorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VorV_VX_k1_HX_WX_1_Data))]
		void Test64_VorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 56 50 01", 7, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 56 50 01", 7, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 56 50 01", 7, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 56 50 01", 7, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 56 50 01", 7, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 56 50 01", 7, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VorV_VX_k1_HX_WX_2_Data))]
		void Test64_VorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 56 D3", 6, Code.EVEX_Vorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 56 D3", 6, Code.EVEX_Vorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 56 D3", 6, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 56 D3", 6, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 56 D3", 6, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 56 D3", 6, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XorV_VX_WX_1_Data))]
		void Test16_XorV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_XorV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F57 08", 3, Code.Xorps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F57 08", 4, Code.Xorpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XorV_VX_WX_2_Data))]
		void Test16_XorV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_XorV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F57 CD", 3, Code.Xorps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F57 CD", 4, Code.Xorpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XorV_VX_WX_1_Data))]
		void Test32_XorV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_XorV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F57 08", 3, Code.Xorps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F57 08", 4, Code.Xorpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XorV_VX_WX_2_Data))]
		void Test32_XorV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_XorV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F57 CD", 3, Code.Xorps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F57 CD", 4, Code.Xorpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XorV_VX_WX_1_Data))]
		void Test64_XorV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_XorV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F57 08", 3, Code.Xorps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F57 08", 4, Code.Xorpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XorV_VX_WX_2_Data))]
		void Test64_XorV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_XorV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F57 CD", 3, Code.Xorps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F57 CD", 4, Code.Xorps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F57 CD", 4, Code.Xorps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F57 CD", 4, Code.Xorps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F57 CD", 4, Code.Xorpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F57 CD", 5, Code.Xorpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F57 CD", 5, Code.Xorpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F57 CD", 5, Code.Xorpd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VxorV_VX_HX_WX_1_Data))]
		void Test16_VxorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VxorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 57 10", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 57 10", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 57 10", 5, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 57 10", 5, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 57 10", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 57 10", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 57 10", 5, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 57 10", 5, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VxorV_VX_HX_WX_2_Data))]
		void Test16_VxorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VxorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 57 D3", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 57 D3", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 57 D3", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 57 D3", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VxorV_VX_HX_WX_1_Data))]
		void Test32_VxorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VxorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 57 10", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 57 10", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 57 10", 5, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 57 10", 5, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 57 10", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 57 10", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 57 10", 5, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 57 10", 5, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VxorV_VX_HX_WX_2_Data))]
		void Test32_VxorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VxorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 57 D3", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 57 D3", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 57 D3", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 57 D3", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VxorV_VX_HX_WX_1_Data))]
		void Test64_VxorV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VxorV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 57 10", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 57 10", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 57 10", 5, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 57 10", 5, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 57 10", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 57 10", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 57 10", 5, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 57 10", 5, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VxorV_VX_HX_WX_2_Data))]
		void Test64_VxorV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VxorV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 57 D3", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 57 D3", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 57 D3", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 57 D3", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 57 D3", 4, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 57 D3", 4, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 57 D3", 5, Code.VEX_Vxorps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 57 D3", 5, Code.VEX_Vxorps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 57 D3", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 57 D3", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 57 D3", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 57 D3", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 57 D3", 4, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 57 D3", 4, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 57 D3", 5, Code.VEX_Vxorpd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 57 D3", 5, Code.VEX_Vxorpd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VxorV_VX_k1_HX_WX_1_Data))]
		void Test16_VxorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VxorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VxorV_VX_k1_HX_WX_2_Data))]
		void Test16_VxorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VxorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VxorV_VX_k1_HX_WX_1_Data))]
		void Test32_VxorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VxorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VxorV_VX_k1_HX_WX_2_Data))]
		void Test32_VxorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VxorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VxorV_VX_k1_HX_WX_1_Data))]
		void Test64_VxorV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VxorV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 57 50 01", 7, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 57 50 01", 7, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 57 50 01", 7, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 57 50 01", 7, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 57 50 01", 7, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 57 50 01", 7, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VxorV_VX_k1_HX_WX_2_Data))]
		void Test64_VxorV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VxorV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 57 D3", 6, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 57 D3", 6, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 57 D3", 6, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 57 D3", 6, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 57 D3", 6, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 57 D3", 6, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
