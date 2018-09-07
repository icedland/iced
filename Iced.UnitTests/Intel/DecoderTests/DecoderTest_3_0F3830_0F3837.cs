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
	public sealed class DecoderTest_3_0F3830_0F3837 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpmovzxbwV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3830 08", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E279 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2F9 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E27D 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2FD 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbwV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3830 CD", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbwV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3830 08", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E279 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2F9 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E27D 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2FD 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbwV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3830 CD", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbwV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3830 08", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E279 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2F9 30 10", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM2, MemorySize.Packed64_UInt8 };

				yield return new object[] { "C4E27D 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2FD 30 10", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM2, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbwV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3830 CD", 5, Code.Pmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3830 CD", 6, Code.Pmovzxbw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3830 CD", 6, Code.Pmovzxbw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3830 CD", 6, Code.Pmovzxbw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 30 CD", 5, Code.VEX_Vpmovzxbw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 30 CD", 5, Code.VEX_Vpmovzxbw_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27D8B 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DAB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27D48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DCB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F2FD48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27D8B 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DAB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27D48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DCB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F2FD48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27D8B 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD08 30 50 01", 7, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DAB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD28 30 50 01", 7, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27D48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27DCB 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F2FD48 30 50 01", 7, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 30 D3", 6, Code.EVEX_Vpmovzxbw_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 30 D3", 6, Code.EVEX_Vpmovzxbw_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 30 D3", 6, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovwbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovwbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovwbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovwbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovwbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovwbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovwbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovwbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovwbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovwbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovwbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovwbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovwbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovwbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovwbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 30 50 01", 7, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 30 50 01", 7, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovwbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovwbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovwbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 30 D3", 6, Code.EVEX_Vpmovwb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 30 D3", 6, Code.EVEX_Vpmovwb_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbdV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3831 08", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E279 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2F9 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E27D 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2FD 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbdV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3831 CD", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbdV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3831 08", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E279 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2F9 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E27D 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2FD 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbdV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3831 CD", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbdV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3831 08", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E279 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2F9 31 10", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM2, MemorySize.Packed32_UInt8 };

				yield return new object[] { "C4E27D 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
				yield return new object[] { "C4E2FD 31 10", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM2, MemorySize.Packed64_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbdV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3831 CD", 5, Code.Pmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3831 CD", 6, Code.Pmovzxbd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3831 CD", 6, Code.Pmovzxbd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3831 CD", 6, Code.Pmovzxbd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 31 CD", 5, Code.VEX_Vpmovzxbd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 31 CD", 5, Code.VEX_Vpmovzxbd_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27D8B 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DAB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DCB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27D8B 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DAB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DCB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27D8B 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD08 31 50 01", 7, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DAB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD28 31 50 01", 7, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27D48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27DCB 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F2FD48 31 50 01", 7, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 31 D3", 6, Code.EVEX_Vpmovzxbd_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 31 D3", 6, Code.EVEX_Vpmovzxbd_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 31 D3", 6, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovdbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovdbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovdbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovdbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovdbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 31 50 01", 7, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovdbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 31 D3", 6, Code.EVEX_Vpmovdb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbqV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3832 08", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E279 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };
				yield return new object[] { "C4E2F9 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E27D 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2FD 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbqV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3832 CD", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbqV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3832 08", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E279 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };
				yield return new object[] { "C4E2F9 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E27D 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2FD 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbqV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3832 CD", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbqV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3832 08", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E279 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };
				yield return new object[] { "C4E2F9 32 10", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM2, MemorySize.Packed16_UInt8 };

				yield return new object[] { "C4E27D 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
				yield return new object[] { "C4E2FD 32 10", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM2, MemorySize.Packed32_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbqV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3832 CD", 5, Code.Pmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3832 CD", 6, Code.Pmovzxbq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3832 CD", 6, Code.Pmovzxbq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3832 CD", 6, Code.Pmovzxbq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 32 CD", 5, Code.VEX_Vpmovzxbq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 32 CD", 5, Code.VEX_Vpmovzxbq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27D8B 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, true };
				yield return new object[] { "62 F2FD08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27D28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27DAB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DCB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27D8B 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, true };
				yield return new object[] { "62 F2FD08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27D28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27DAB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DCB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27D8B 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, true };
				yield return new object[] { "62 F2FD08 32 50 01", 7, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27D28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27DAB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, true };
				yield return new object[] { "62 F2FD28 32 50 01", 7, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27D48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27DCB 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, true };
				yield return new object[] { "62 F2FD48 32 50 01", 7, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 32 D3", 6, Code.EVEX_Vpmovzxbq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 32 D3", 6, Code.EVEX_Vpmovzxbq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 32 D3", 6, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 32 50 01", 7, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 32 D3", 6, Code.EVEX_Vpmovqb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwdV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3833 08", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E279 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2F9 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E27D 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2FD 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwdV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3833 CD", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwdV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3833 08", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E279 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2F9 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E27D 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2FD 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwdV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3833 CD", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwdV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3833 08", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E279 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2F9 33 10", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM2, MemorySize.Packed64_UInt16 };

				yield return new object[] { "C4E27D 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2FD 33 10", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwdV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3833 CD", 5, Code.Pmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3833 CD", 6, Code.Pmovzxwd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3833 CD", 6, Code.Pmovzxwd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3833 CD", 6, Code.Pmovzxwd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 33 CD", 5, Code.VEX_Vpmovzxwd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 33 CD", 5, Code.VEX_Vpmovzxwd_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27D8B 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DAB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27D48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27DCB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2FD48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27D8B 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DAB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27D48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27DCB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2FD48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27D8B 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD08 33 50 01", 7, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DAB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD28 33 50 01", 7, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27D48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27DCB 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2FD48 33 50 01", 7, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 33 D3", 6, Code.EVEX_Vpmovzxwd_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 33 D3", 6, Code.EVEX_Vpmovzxwd_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 33 D3", 6, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovdwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovdwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovdwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovdwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovdwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 33 50 01", 7, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 33 50 01", 7, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovdwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 33 D3", 6, Code.EVEX_Vpmovdw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 33 D3", 6, Code.EVEX_Vpmovdw_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwqV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3834 08", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E279 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };
				yield return new object[] { "C4E2F9 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E27D 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2FD 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwqV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3834 CD", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwqV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3834 08", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E279 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };
				yield return new object[] { "C4E2F9 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E27D 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2FD 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwqV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3834 CD", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwqV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3834 08", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E279 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };
				yield return new object[] { "C4E2F9 34 10", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM2, MemorySize.Packed32_UInt16 };

				yield return new object[] { "C4E27D 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
				yield return new object[] { "C4E2FD 34 10", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM2, MemorySize.Packed64_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwqV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3834 CD", 5, Code.Pmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3834 CD", 6, Code.Pmovzxwq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3834 CD", 6, Code.Pmovzxwq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3834 CD", 6, Code.Pmovzxwq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 34 CD", 5, Code.VEX_Vpmovzxwq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 34 CD", 5, Code.VEX_Vpmovzxwq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27D8B 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, true };
				yield return new object[] { "62 F2FD08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27D28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27DAB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DCB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27D8B 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, true };
				yield return new object[] { "62 F2FD08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27D28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27DAB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DCB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27D8B 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, true };
				yield return new object[] { "62 F2FD08 34 50 01", 7, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27D28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27DAB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, true };
				yield return new object[] { "62 F2FD28 34 50 01", 7, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27D48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27DCB 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2FD48 34 50 01", 7, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 34 D3", 6, Code.EVEX_Vpmovzxwq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 34 D3", 6, Code.EVEX_Vpmovzxwq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 34 D3", 6, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 34 50 01", 7, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 34 D3", 6, Code.EVEX_Vpmovqw_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxdqV_Reg_RegMem_1_Data))]
		void Test16_VpmovzxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovzxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3835 08", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E279 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };
				yield return new object[] { "C4E2F9 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E27D 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2FD 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxdqV_Reg_RegMem_2_Data))]
		void Test16_VpmovzxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovzxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3835 CD", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxdqV_Reg_RegMem_1_Data))]
		void Test32_VpmovzxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovzxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3835 08", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E279 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };
				yield return new object[] { "C4E2F9 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E27D 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2FD 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxdqV_Reg_RegMem_2_Data))]
		void Test32_VpmovzxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovzxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3835 CD", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxdqV_Reg_RegMem_1_Data))]
		void Test64_VpmovzxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovzxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3835 08", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E279 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };
				yield return new object[] { "C4E2F9 35 10", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM2, MemorySize.Packed64_UInt32 };

				yield return new object[] { "C4E27D 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E2FD 35 10", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM2, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxdqV_Reg_RegMem_2_Data))]
		void Test64_VpmovzxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovzxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3835 CD", 5, Code.Pmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3835 CD", 6, Code.Pmovzxdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3835 CD", 6, Code.Pmovzxdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3835 CD", 6, Code.Pmovzxdq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 35 CD", 5, Code.VEX_Vpmovzxdq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 35 CD", 5, Code.VEX_Vpmovzxdq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovzxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27D8B 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, true };
				yield return new object[] { "62 F2FD08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27D28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27DAB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, true };
				yield return new object[] { "62 F2FD28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DCB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, true };
				yield return new object[] { "62 F2FD48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovzxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovzxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovzxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovzxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27D8B 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, true };
				yield return new object[] { "62 F2FD08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27D28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27DAB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, true };
				yield return new object[] { "62 F2FD28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DCB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, true };
				yield return new object[] { "62 F2FD48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovzxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovzxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovzxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovzxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27D8B 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, true };
				yield return new object[] { "62 F2FD08 35 50 01", 7, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27D28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27DAB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, true };
				yield return new object[] { "62 F2FD28 35 50 01", 7, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DCB 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, true };
				yield return new object[] { "62 F2FD48 35 50 01", 7, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovzxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovzxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovzxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 35 D3", 6, Code.EVEX_Vpmovzxdq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 35 D3", 6, Code.EVEX_Vpmovzxdq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 35 D3", 6, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqdV_WX_k1z_VX_1_Data))]
		void Test16_VpmovqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovqdV_WX_k1z_VX_2_Data))]
		void Test16_VpmovqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqdV_WX_k1z_VX_1_Data))]
		void Test32_VpmovqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovqdV_WX_k1z_VX_2_Data))]
		void Test32_VpmovqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqdV_WX_k1z_VX_1_Data))]
		void Test64_VpmovqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 35 50 01", 7, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 35 50 01", 7, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovqdV_WX_k1z_VX_2_Data))]
		void Test64_VpmovqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 35 D3", 6, Code.EVEX_Vpmovqd_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 35 D3", 6, Code.EVEX_Vpmovqd_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_HX_WX_1_Data))]
		void Test16_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 36 10", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_HX_WX_2_Data))]
		void Test16_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_HX_WX_1_Data))]
		void Test32_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 36 10", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_HX_WX_2_Data))]
		void Test32_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_HX_WX_1_Data))]
		void Test64_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 36 10", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_HX_WX_2_Data))]
		void Test64_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 36 D3", 5, Code.VEX_Vpermd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test16_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD2B 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test16_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test32_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD2B 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test32_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test64_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 36 50 01", 7, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 36 50 01", 7, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD2B 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 36 50 01", 7, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 36 50 01", 7, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test64_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 36 D3", 6, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 36 D3", 6, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 36 D3", 6, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 36 D3", 6, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpgtqV_VX_WX_1_Data))]
		void Test16_PcmpgtqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PcmpgtqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3837 08", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpgtqV_VX_WX_2_Data))]
		void Test16_PcmpgtqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PcmpgtqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3837 CD", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpgtqV_VX_WX_1_Data))]
		void Test32_PcmpgtqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PcmpgtqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3837 08", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpgtqV_VX_WX_2_Data))]
		void Test32_PcmpgtqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PcmpgtqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3837 CD", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpgtqV_VX_WX_1_Data))]
		void Test64_PcmpgtqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PcmpgtqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3837 08", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpgtqV_VX_WX_2_Data))]
		void Test64_PcmpgtqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PcmpgtqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3837 CD", 5, Code.Pcmpgtq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3837 CD", 6, Code.Pcmpgtq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3837 CD", 6, Code.Pcmpgtq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3837 CD", 6, Code.Pcmpgtq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpgtqV_VX_HX_WX_1_Data))]
		void Test16_VpcmpgtqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpcmpgtqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpgtqV_VX_HX_WX_2_Data))]
		void Test16_VpcmpgtqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpcmpgtqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpgtqV_VX_HX_WX_1_Data))]
		void Test32_VpcmpgtqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpcmpgtqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpgtqV_VX_HX_WX_2_Data))]
		void Test32_VpcmpgtqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpcmpgtqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpgtqV_VX_HX_WX_1_Data))]
		void Test64_VpcmpgtqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpcmpgtqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E2C9 37 10", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };

				yield return new object[] { "C4E24D 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E2CD 37 10", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpgtqV_VX_HX_WX_2_Data))]
		void Test64_VpcmpgtqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpcmpgtqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 37 D3", 5, Code.VEX_Vpcmpgtq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 37 D3", 5, Code.VEX_Vpcmpgtq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpgtqV_K_k1_HX_WX_1_Data))]
		void Test16_VpcmpgtqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpgtqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpgtqV_K_k1_HX_WX_2_Data))]
		void Test16_VpcmpgtqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpcmpgtqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F2CD08 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F2CD28 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F2CD48 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpgtqV_K_k1_HX_WX_1_Data))]
		void Test32_VpcmpgtqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpgtqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpgtqV_K_k1_HX_WX_2_Data))]
		void Test32_VpcmpgtqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpcmpgtqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F2CD08 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F2CD28 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F2CD48 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpgtqV_K_k1_HX_WX_1_Data))]
		void Test64_VpcmpgtqV_K_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpgtqV_K_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD08 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD1D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false };
				yield return new object[] { "62 F2CD0B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD28 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CD3D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false };
				yield return new object[] { "62 F2CD2B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD48 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CD5D 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false };
				yield return new object[] { "62 F2CD4B 37 50 01", 7, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpgtqV_K_k1_HX_WX_2_Data))]
		void Test64_VpcmpgtqV_K_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpcmpgtqV_K_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F28D0B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 92CD03 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD08 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.None, false };

				yield return new object[] { "62 F2CD2B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F28D2B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 92CD23 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD28 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.None, false };

				yield return new object[] { "62 F2CD4B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F28D4B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 92CD43 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD48 37 D3", 6, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.None, false };
			}
		}
	}
}
