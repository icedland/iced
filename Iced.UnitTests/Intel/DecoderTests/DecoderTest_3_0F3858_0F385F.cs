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
	public sealed class DecoderTest_3_0F3858_0F385F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpbroadcastdV_Reg_RegMem_1_Data))]
		void Test16_VpbroadcastdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 58 10", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM2, MemorySize.Int32 };

				yield return new object[] { "C4E27D 58 10", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM2, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastdV_Reg_RegMem_2_Data))]
		void Test16_VpbroadcastdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastdV_Reg_RegMem_1_Data))]
		void Test32_VpbroadcastdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 58 10", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM2, MemorySize.Int32 };

				yield return new object[] { "C4E27D 58 10", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM2, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastdV_Reg_RegMem_2_Data))]
		void Test32_VpbroadcastdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastdV_Reg_RegMem_1_Data))]
		void Test64_VpbroadcastdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 58 10", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM2, MemorySize.Int32 };

				yield return new object[] { "C4E27D 58 10", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM2, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastdV_Reg_RegMem_2_Data))]
		void Test64_VpbroadcastdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 58 CD", 5, Code.VEX_Vpbroadcastd_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 58 CD", 5, Code.VEX_Vpbroadcastd_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27D8B 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D28 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DAB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D48 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DCB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpbroadcastdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27D8B 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D28 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DAB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D48 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DCB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpbroadcastdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27D8B 58 50 01", 7, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D28 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DAB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int32, 4, true };

				yield return new object[] { "62 F27D48 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F27DCB 58 50 01", 7, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpbroadcastdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 58 D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 58 D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 58 D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastqV_Reg_RegMem_1_Data))]
		void Test16_VpbroadcastqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 59 10", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM2, MemorySize.Int64 };

				yield return new object[] { "C4E27D 59 10", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM2, MemorySize.Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastqV_Reg_RegMem_2_Data))]
		void Test16_VpbroadcastqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastqV_Reg_RegMem_1_Data))]
		void Test32_VpbroadcastqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 59 10", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM2, MemorySize.Int64 };

				yield return new object[] { "C4E27D 59 10", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM2, MemorySize.Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastqV_Reg_RegMem_2_Data))]
		void Test32_VpbroadcastqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastqV_Reg_RegMem_1_Data))]
		void Test64_VpbroadcastqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 59 10", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM2, MemorySize.Int64 };

				yield return new object[] { "C4E27D 59 10", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM2, MemorySize.Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastqV_Reg_RegMem_2_Data))]
		void Test64_VpbroadcastqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 59 CD", 5, Code.VEX_Vpbroadcastq_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 59 CD", 5, Code.VEX_Vpbroadcastq_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DAB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D48 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DCB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F2FD08 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FD8B 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD28 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDAB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD48 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDCB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpbroadcastqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DAB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D48 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DCB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F2FD08 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FD8B 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD28 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDAB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD48 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDCB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpbroadcastqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27D8B 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D28 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DAB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F27D48 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F27DCB 59 50 01", 7, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Packed64_Int32, 8, true };

				yield return new object[] { "62 F2FD08 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FD8B 59 50 01", 7, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD28 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDAB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int64, 8, true };

				yield return new object[] { "62 F2FD48 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F2FDCB 59 50 01", 7, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpbroadcastqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 59 D3", 6, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FD8B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FD8B 59 D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 59 D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 59 D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vbroadcasti128V_Reg_RegMem_1_Data))]
		void Test16_Vbroadcasti128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vbroadcasti128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 5A 10", 5, Code.VEX_Vbroadcasti128_VY_M, Register.YMM2, MemorySize.Int128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vbroadcasti128V_Reg_RegMem_1_Data))]
		void Test32_Vbroadcasti128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vbroadcasti128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 5A 10", 5, Code.VEX_Vbroadcasti128_VY_M, Register.YMM2, MemorySize.Int128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vbroadcasti128V_Reg_RegMem_1_Data))]
		void Test64_Vbroadcasti128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vbroadcasti128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 5A 10", 5, Code.VEX_Vbroadcasti128_VY_M, Register.YMM2, MemorySize.Int128 };
				yield return new object[] { "C4627D 5A 10", 5, Code.VEX_Vbroadcasti128_VY_M, Register.YMM10, MemorySize.Int128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data))]
		void Test16_Vbroadcasti32x4V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DCB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F2FD28 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDAB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F2FD48 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDCB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data))]
		void Test32_Vbroadcasti32x4V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DCB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F2FD28 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDAB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F2FD48 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDCB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data))]
		void Test64_Vbroadcasti32x4V_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vbroadcasti32x4V_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 327D28 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM10, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 E27D28 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM18, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DAB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F27D48 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 327D48 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM10, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 E27D48 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM18, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27DCB 5A 50 01", 7, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F2FD28 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 32FD28 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM10, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 E2FD28 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM18, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDAB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F2FD48 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 32FD48 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM10, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 E2FD48 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM18, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FDCB 5A 50 01", 7, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastiV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VbroadcastiV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VbroadcastiV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F2FD48 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDCB 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastiV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VbroadcastiV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VbroadcastiV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F2FD48 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDCB 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastiV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VbroadcastiV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VbroadcastiV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 327D48 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM10, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 E27D48 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM18, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DCB 5B 50 01", 7, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F2FD48 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 32FD48 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM10, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 E2FD48 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM18, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDCB 5B 50 01", 7, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };
			}
		}
	}
}
