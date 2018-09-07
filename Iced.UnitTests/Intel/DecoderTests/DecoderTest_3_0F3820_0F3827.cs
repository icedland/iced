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
	public sealed class DecoderTest_3_0F3820_0F3827 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpmovsxbwV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3820 08", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E279 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2F9 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E27D 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2FD 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbwV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3820 CD", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbwV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3820 08", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E279 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2F9 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E27D 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2FD 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbwV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3820 CD", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbwV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxbwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3820 08", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E279 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2F9 20 10", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM2, MemorySize.Packed64_Int8 };

				yield return new object[] { "C4E27D 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2FD 20 10", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM2, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbwV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxbwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3820 CD", 5, Code.Pmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3820 CD", 6, Code.Pmovsxbw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3820 CD", 6, Code.Pmovsxbw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3820 CD", 6, Code.Pmovsxbw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 20 CD", 5, Code.VEX_Vpmovsxbw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 20 CD", 5, Code.VEX_Vpmovsxbw_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27D8B 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DAB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DCB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27D8B 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DAB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DCB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbwV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxbwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxbwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27D8B 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD08 20 50 01", 7, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DAB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD28 20 50 01", 7, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DCB 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD48 20 50 01", 7, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbwV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxbwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 20 D3", 6, Code.EVEX_Vpmovsxbw_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 20 D3", 6, Code.EVEX_Vpmovsxbw_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 20 D3", 6, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovswbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovswbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovswbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovswbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovswbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E08 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E2B 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E28 20 50 01", 7, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27E4B 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27E48 20 50 01", 7, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovswbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 20 D3", 6, Code.EVEX_Vpmovswb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 20 D3", 6, Code.EVEX_Vpmovswb_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbdV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3821 08", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E279 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2F9 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E27D 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2FD 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbdV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3821 CD", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbdV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3821 08", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E279 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2F9 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E27D 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2FD 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbdV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3821 CD", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbdV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxbdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3821 08", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E279 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2F9 21 10", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM2, MemorySize.Packed32_Int8 };

				yield return new object[] { "C4E27D 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
				yield return new object[] { "C4E2FD 21 10", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM2, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbdV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxbdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3821 CD", 5, Code.Pmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3821 CD", 6, Code.Pmovsxbd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3821 CD", 6, Code.Pmovsxbd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3821 CD", 6, Code.Pmovsxbd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 21 CD", 5, Code.VEX_Vpmovsxbd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 21 CD", 5, Code.VEX_Vpmovsxbd_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27D8B 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DAB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DCB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27D8B 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DAB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DCB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxbdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxbdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27D8B 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD08 21 50 01", 7, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DAB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD28 21 50 01", 7, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27D48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27DCB 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD48 21 50 01", 7, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxbdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 21 D3", 6, Code.EVEX_Vpmovsxbd_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 21 D3", 6, Code.EVEX_Vpmovsxbd_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 21 D3", 6, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsdbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovsdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsdbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovsdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsdbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovsdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsdbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovsdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsdbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovsdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E08 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E2B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E28 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int8, 8, false };

				yield return new object[] { "62 F27E4B 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27E48 21 50 01", 7, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsdbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovsdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 21 D3", 6, Code.EVEX_Vpmovsdb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbqV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3822 08", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E279 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };
				yield return new object[] { "C4E2F9 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E27D 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2FD 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbqV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3822 CD", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbqV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3822 08", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E279 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };
				yield return new object[] { "C4E2F9 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E27D 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2FD 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbqV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3822 CD", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbqV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxbqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3822 08", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E279 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };
				yield return new object[] { "C4E2F9 22 10", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM2, MemorySize.Packed16_Int8 };

				yield return new object[] { "C4E27D 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
				yield return new object[] { "C4E2FD 22 10", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM2, MemorySize.Packed32_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbqV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxbqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3822 CD", 5, Code.Pmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3822 CD", 6, Code.Pmovsxbq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3822 CD", 6, Code.Pmovsxbq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3822 CD", 6, Code.Pmovsxbq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 22 CD", 5, Code.VEX_Vpmovsxbq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 22 CD", 5, Code.VEX_Vpmovsxbq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27D8B 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, true };
				yield return new object[] { "62 F2FD08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27D28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27DAB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DCB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27D8B 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, true };
				yield return new object[] { "62 F2FD08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27D28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27DAB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DCB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxbqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxbqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27D8B 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, true };
				yield return new object[] { "62 F2FD08 22 50 01", 7, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27D28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27DAB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, true };
				yield return new object[] { "62 F2FD28 22 50 01", 7, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27D48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27DCB 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, true };
				yield return new object[] { "62 F2FD48 22 50 01", 7, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxbqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxbqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxbqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 22 D3", 6, Code.EVEX_Vpmovsxbq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 22 D3", 6, Code.EVEX_Vpmovsxbq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 22 D3", 6, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovsqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovsqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovsqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovsqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovsqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_Int8, 2, false };
				yield return new object[] { "62 F27E08 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_Int8, 2, false };

				yield return new object[] { "62 F27E2B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_Int8, 4, false };
				yield return new object[] { "62 F27E28 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_Int8, 4, false };

				yield return new object[] { "62 F27E4B 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_Int8, 8, false };
				yield return new object[] { "62 F27E48 22 50 01", 7, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_Int8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovsqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 22 D3", 6, Code.EVEX_Vpmovsqb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwdV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3823 08", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E279 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2F9 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E27D 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2FD 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwdV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3823 CD", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwdV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3823 08", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E279 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2F9 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E27D 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2FD 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwdV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3823 CD", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwdV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxwdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3823 08", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E279 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2F9 23 10", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM2, MemorySize.Packed64_Int16 };

				yield return new object[] { "C4E27D 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2FD 23 10", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM2, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwdV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxwdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3823 CD", 5, Code.Pmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3823 CD", 6, Code.Pmovsxwd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3823 CD", 6, Code.Pmovsxwd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3823 CD", 6, Code.Pmovsxwd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 23 CD", 5, Code.VEX_Vpmovsxwd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 23 CD", 5, Code.VEX_Vpmovsxwd_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27D8B 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DAB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DCB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27D8B 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DAB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DCB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxwdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxwdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27D8B 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD08 23 50 01", 7, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DAB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD28 23 50 01", 7, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DCB 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD48 23 50 01", 7, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxwdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 23 D3", 6, Code.EVEX_Vpmovsxwd_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 23 D3", 6, Code.EVEX_Vpmovsxwd_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 23 D3", 6, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsdwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovsdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsdwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovsdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsdwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovsdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsdwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovsdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsdwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovsdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E08 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E2B 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E28 23 50 01", 7, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27E4B 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27E48 23 50 01", 7, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsdwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovsdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 23 D3", 6, Code.EVEX_Vpmovsdw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 23 D3", 6, Code.EVEX_Vpmovsdw_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwqV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3824 08", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E279 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };
				yield return new object[] { "C4E2F9 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E27D 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2FD 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwqV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3824 CD", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwqV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3824 08", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E279 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };
				yield return new object[] { "C4E2F9 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E27D 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2FD 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwqV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3824 CD", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwqV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxwqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3824 08", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E279 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };
				yield return new object[] { "C4E2F9 24 10", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM2, MemorySize.Packed32_Int16 };

				yield return new object[] { "C4E27D 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
				yield return new object[] { "C4E2FD 24 10", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM2, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwqV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxwqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3824 CD", 5, Code.Pmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3824 CD", 6, Code.Pmovsxwq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3824 CD", 6, Code.Pmovsxwq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3824 CD", 6, Code.Pmovsxwq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 24 CD", 5, Code.VEX_Vpmovsxwq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 24 CD", 5, Code.VEX_Vpmovsxwq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27D8B 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, true };
				yield return new object[] { "62 F2FD08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27D28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27DAB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DCB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27D8B 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, true };
				yield return new object[] { "62 F2FD08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27D28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27DAB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DCB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxwqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxwqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27D8B 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, true };
				yield return new object[] { "62 F2FD08 24 50 01", 7, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27D28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27DAB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, true };
				yield return new object[] { "62 F2FD28 24 50 01", 7, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27D48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27DCB 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD48 24 50 01", 7, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxwqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxwqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxwqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 24 D3", 6, Code.EVEX_Vpmovsxwq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 24 D3", 6, Code.EVEX_Vpmovsxwq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 24 D3", 6, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovsqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovsqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovsqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovsqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovsqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_Int16, 4, false };
				yield return new object[] { "62 F27E08 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_Int16, 4, false };

				yield return new object[] { "62 F27E2B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_Int16, 8, false };
				yield return new object[] { "62 F27E28 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_Int16, 8, false };

				yield return new object[] { "62 F27E4B 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27E48 24 50 01", 7, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovsqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 24 D3", 6, Code.EVEX_Vpmovsqw_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxdqV_Reg_RegMem_1_Data))]
		void Test16_VpmovsxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmovsxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3825 08", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E279 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E2F9 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E27D 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2FD 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxdqV_Reg_RegMem_2_Data))]
		void Test16_VpmovsxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpmovsxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3825 CD", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxdqV_Reg_RegMem_1_Data))]
		void Test32_VpmovsxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmovsxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3825 08", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E279 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E2F9 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E27D 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2FD 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxdqV_Reg_RegMem_2_Data))]
		void Test32_VpmovsxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpmovsxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3825 CD", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxdqV_Reg_RegMem_1_Data))]
		void Test64_VpmovsxdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmovsxdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3825 08", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E279 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E2F9 25 10", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C4E27D 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2FD 25 10", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxdqV_Reg_RegMem_2_Data))]
		void Test64_VpmovsxdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpmovsxdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3825 CD", 5, Code.Pmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3825 CD", 6, Code.Pmovsxdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3825 CD", 6, Code.Pmovsxdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3825 CD", 6, Code.Pmovsxdq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 25 CD", 5, Code.VEX_Vpmovsxdq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 25 CD", 5, Code.VEX_Vpmovsxdq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpmovsxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpmovsxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpmovsxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpmovsxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxdqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpmovsxdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsxdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 25 50 01", 7, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 25 50 01", 7, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsxdqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpmovsxdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsxdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 25 D3", 6, Code.EVEX_Vpmovsxdq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 25 D3", 6, Code.EVEX_Vpmovsxdq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 25 D3", 6, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, Register.ZMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqdV_WX_k1z_VX_1_Data))]
		void Test16_VpmovsqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovsqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovsqdV_WX_k1z_VX_2_Data))]
		void Test16_VpmovsqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmovsqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqdV_WX_k1z_VX_1_Data))]
		void Test32_VpmovsqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovsqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovsqdV_WX_k1z_VX_2_Data))]
		void Test32_VpmovsqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmovsqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqdV_WX_k1z_VX_1_Data))]
		void Test64_VpmovsqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovsqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27E08 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };

				yield return new object[] { "62 F27E2B 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27E28 25 50 01", 7, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27E4B 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27E48 25 50 01", 7, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovsqdV_WX_k1z_VX_2_Data))]
		void Test64_VpmovsqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmovsqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 25 D3", 6, Code.EVEX_Vpmovsqd_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 25 D3", 6, Code.EVEX_Vpmovsqd_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestmbV_VX_k1_HX_WX_1_Data))]
		void Test16_VptestmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CD0B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD08 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD28 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD48 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestmbV_VX_k1_HX_WX_2_Data))]
		void Test16_VptestmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VptestmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestmbV_VX_k1_HX_WX_1_Data))]
		void Test32_VptestmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CD0B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD08 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD28 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD48 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestmbV_VX_k1_HX_WX_2_Data))]
		void Test32_VptestmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VptestmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestmbV_VX_k1_HX_WX_1_Data))]
		void Test64_VptestmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D0D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D08 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D2D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24D28 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D4D 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24D48 26 50 01", 7, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CD0B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD0D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD08 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD2D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CD28 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD4D 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CD48 26 50 01", 7, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestmbV_VX_k1_HX_WX_2_Data))]
		void Test64_VptestmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VptestmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D0B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D03 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D2B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D23 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20D4B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924D43 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 26 D3", 6, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28D0B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CD03 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28D2B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CD23 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28D4B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CD43 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 26 D3", 6, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestnmbV_VX_k1_HX_WX_1_Data))]
		void Test16_VptestnmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestnmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E0D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E08 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24E2B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E2D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E28 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24E4B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E4D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E48 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CE0B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE0D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE08 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CE2B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE2D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE28 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CE4B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE4D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE48 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestnmbV_VX_k1_HX_WX_2_Data))]
		void Test16_VptestnmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VptestnmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E2B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E4B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE0B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE2B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE4B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestnmbV_VX_k1_HX_WX_1_Data))]
		void Test32_VptestnmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestnmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E0D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E08 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24E2B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E2D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E28 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24E4B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E4D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E48 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CE0B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE0D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE08 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CE2B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE2D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE28 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CE4B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE4D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE48 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestnmbV_VX_k1_HX_WX_2_Data))]
		void Test32_VptestnmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VptestnmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E2B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E4B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE0B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE2B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE4B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestnmbV_VX_k1_HX_WX_1_Data))]
		void Test64_VptestnmbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestnmbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E0D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24E08 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24E2B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E2D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24E28 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24E4B 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E4D 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24E48 26 50 01", 7, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };

				yield return new object[] { "62 F2CE0B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE0D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CE08 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CE2B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE2D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CE28 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CE4B 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE4D 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CE48 26 50 01", 7, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestnmbV_VX_k1_HX_WX_2_Data))]
		void Test64_VptestnmbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VptestnmbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20E0B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924E03 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24E0B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E2B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20E2B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924E23 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24E2B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24E4B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F20E4B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 924E43 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24E4B 26 D3", 6, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE0B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28E0B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CE03 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CE0B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HX_WX, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE2B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28E2B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CE23 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CE2B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HY_WY, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CE4B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F28E4B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 92CE43 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CE4B 26 D3", 6, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestmdV_VX_k1_HX_WX_1_Data))]
		void Test16_VptestmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D1D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24D08 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24D3D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24D28 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24D5D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24D48 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD1D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CD08 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CD3D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CD28 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CD5D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CD48 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestmdV_VX_k1_HX_WX_2_Data))]
		void Test16_VptestmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24D2B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24D4B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD0B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CD2B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CD4B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestmdV_VX_k1_HX_WX_1_Data))]
		void Test32_VptestmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D1D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24D08 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24D3D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24D28 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24D5D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24D48 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD1D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CD08 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CD3D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CD28 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CD5D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CD48 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestmdV_VX_k1_HX_WX_2_Data))]
		void Test32_VptestmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24D2B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24D4B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD0B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CD2B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CD4B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestmdV_VX_k1_HX_WX_1_Data))]
		void Test64_VptestmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D1D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24D08 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24D3D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24D28 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24D5D 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24D48 27 50 01", 7, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD1D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CD08 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CD3D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CD28 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CD5D 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CD48 27 50 01", 7, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestmdV_VX_k1_HX_WX_2_Data))]
		void Test64_VptestmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F20D0B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 924D03 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };

				yield return new object[] { "62 F24D2B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F20D2B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 924D23 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };

				yield return new object[] { "62 F24D4B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F20D4B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 924D43 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 27 D3", 6, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };

				yield return new object[] { "62 F2CD0B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F28D0B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 92CD03 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };

				yield return new object[] { "62 F2CD2B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F28D2B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 92CD23 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };

				yield return new object[] { "62 F2CD4B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F28D4B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 92CD43 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 27 D3", 6, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestnmdV_VX_k1_HX_WX_1_Data))]
		void Test16_VptestnmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestnmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24E1D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24E08 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24E2B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24E3D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24E28 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24E4B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24E5D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24E48 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CE0B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CE1D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CE08 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CE2B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CE3D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CE28 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CE4B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CE5D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CE48 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestnmdV_VX_k1_HX_WX_2_Data))]
		void Test16_VptestnmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VptestnmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24E2B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24E4B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CE0B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CE2B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CE4B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestnmdV_VX_k1_HX_WX_1_Data))]
		void Test32_VptestnmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestnmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24E1D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24E08 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24E2B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24E3D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24E28 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24E4B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24E5D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24E48 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CE0B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CE1D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CE08 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CE2B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CE3D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CE28 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CE4B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CE5D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CE48 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestnmdV_VX_k1_HX_WX_2_Data))]
		void Test32_VptestnmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VptestnmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24E2B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24E4B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CE0B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CE2B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CE4B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestnmdV_VX_k1_HX_WX_1_Data))]
		void Test64_VptestnmdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestnmdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24E0B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24E1D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false };
				yield return new object[] { "62 F24E08 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24E2B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24E3D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false };
				yield return new object[] { "62 F24E28 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24E4B 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24E5D 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false };
				yield return new object[] { "62 F24E48 27 50 01", 7, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CE0B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CE1D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false };
				yield return new object[] { "62 F2CE08 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CE2B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CE3D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false };
				yield return new object[] { "62 F2CE28 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CE4B 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CE5D 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false };
				yield return new object[] { "62 F2CE48 27 50 01", 7, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestnmdV_VX_k1_HX_WX_2_Data))]
		void Test64_VptestnmdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VptestnmdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24E0B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F20E0B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 924E03 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24E0B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };

				yield return new object[] { "62 F24E2B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F20E2B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 924E23 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24E2B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };

				yield return new object[] { "62 F24E4B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F20E4B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 924E43 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24E4B 27 D3", 6, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };

				yield return new object[] { "62 F2CE0B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F28E0B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 92CE03 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CE0B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };

				yield return new object[] { "62 F2CE2B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F28E2B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 92CE23 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CE2B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };

				yield return new object[] { "62 F2CE4B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F28E4B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 92CE43 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CE4B 27 D3", 6, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };
			}
		}
	}
}
