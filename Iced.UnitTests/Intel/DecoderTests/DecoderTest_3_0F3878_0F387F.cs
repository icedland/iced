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
	public sealed class DecoderTest_3_0F3878_0F387F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpbroadcastbV_Reg_RegMem_1_Data))]
		void Test16_VpbroadcastbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 78 10", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM2, MemorySize.Int8 };

				yield return new object[] { "C4E27D 78 10", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM2, MemorySize.Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastbV_Reg_RegMem_2_Data))]
		void Test16_VpbroadcastbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastbV_Reg_RegMem_1_Data))]
		void Test32_VpbroadcastbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 78 10", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM2, MemorySize.Int8 };

				yield return new object[] { "C4E27D 78 10", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM2, MemorySize.Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastbV_Reg_RegMem_2_Data))]
		void Test32_VpbroadcastbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastbV_Reg_RegMem_1_Data))]
		void Test64_VpbroadcastbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 78 10", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM2, MemorySize.Int8 };

				yield return new object[] { "C4E27D 78 10", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM2, MemorySize.Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastbV_Reg_RegMem_2_Data))]
		void Test64_VpbroadcastbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 78 CD", 5, Code.VEX_Vpbroadcastb_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 78 CD", 5, Code.VEX_Vpbroadcastb_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastbV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27D8B 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D28 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DAB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D48 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DCB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int8, 1, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastbV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpbroadcastbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastbV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27D8B 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D28 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DAB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D48 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DCB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int8, 1, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastbV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpbroadcastbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastbV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27D8B 78 50 01", 7, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D28 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DAB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int8, 1, true };

				yield return new object[] { "62 F27D48 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int8, 1, false };
				yield return new object[] { "62 F27DCB 78 50 01", 7, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int8, 1, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastbV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpbroadcastbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 78 D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 78 D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 78 D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV_Reg_RegMem_1_Data))]
		void Test16_VpbroadcastwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 79 10", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM2, MemorySize.Int16 };

				yield return new object[] { "C4E27D 79 10", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM2, MemorySize.Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV_Reg_RegMem_2_Data))]
		void Test16_VpbroadcastwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV_Reg_RegMem_1_Data))]
		void Test32_VpbroadcastwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 79 10", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM2, MemorySize.Int16 };

				yield return new object[] { "C4E27D 79 10", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM2, MemorySize.Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV_Reg_RegMem_2_Data))]
		void Test32_VpbroadcastwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV_Reg_RegMem_1_Data))]
		void Test64_VpbroadcastwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 79 10", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM2, MemorySize.Int16 };

				yield return new object[] { "C4E27D 79 10", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM2, MemorySize.Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV_Reg_RegMem_2_Data))]
		void Test64_VpbroadcastwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 79 CD", 5, Code.VEX_Vpbroadcastw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 79 CD", 5, Code.VEX_Vpbroadcastw_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27D8B 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D28 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DAB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D48 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DCB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpbroadcastwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27D8B 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D28 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DAB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D48 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DCB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpbroadcastwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastwV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27D8B 79 50 01", 7, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D28 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DAB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Int16, 2, true };

				yield return new object[] { "62 F27D48 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.None, MemorySize.Int16, 2, false };
				yield return new object[] { "62 F27DCB 79 50 01", 7, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.K3, MemorySize.Int16, 2, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpbroadcastwV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 79 D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 79 D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_WX, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 79 D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastwV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastwV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastwV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D0B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27D8B 7A D3", 6, Code.EVEX_Vpbroadcastb_VX_k1z_Rd, Register.XMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D2B 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DAB 7A D3", 6, Code.EVEX_Vpbroadcastb_VY_k1z_Rd, Register.YMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D4B 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DCB 7A D3", 6, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd, Register.ZMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastwV3_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastwV3_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastwV3_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastwV3_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D0B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27D8B 7B D3", 6, Code.EVEX_Vpbroadcastw_VX_k1z_Rd, Register.XMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D2B 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DAB 7B D3", 6, Code.EVEX_Vpbroadcastw_VY_k1z_Rd, Register.YMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D4B 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DCB 7B D3", 6, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd, Register.ZMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpbroadcastdV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpbroadcastdV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD08 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D28 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD28 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F27D48 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2FD48 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpbroadcastdV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpbroadcastdV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D0B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27D8B 7C D3", 6, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Register.XMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D2B 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DAB 7C D3", 6, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Register.YMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM2, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 727D4B 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM10, Register.EBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DCB 7C D3", 6, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Register.ZMM18, Register.R11D, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD08 7C D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_Rq, Register.XMM2, Register.RBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD0B 7C D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_Rq, Register.XMM2, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 72FD0B 7C D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_Rq, Register.XMM10, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C2FD8B 7C D3", 6, Code.EVEX_Vpbroadcastq_VX_k1z_Rq, Register.XMM18, Register.R11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 7C D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_Rq, Register.YMM2, Register.RBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 7C D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_Rq, Register.YMM2, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 72FD2B 7C D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_Rq, Register.YMM10, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C2FDAB 7C D3", 6, Code.EVEX_Vpbroadcastq_VY_k1z_Rq, Register.YMM18, Register.R11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 7C D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_Rq, Register.ZMM2, Register.RBX, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 7C D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_Rq, Register.ZMM2, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 72FD4B 7C D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_Rq, Register.ZMM10, Register.RBX, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C2FDCB 7C D3", 6, Code.EVEX_Vpbroadcastq_VZ_k1z_Rq, Register.ZMM18, Register.R11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2bV_VX_k1_HX_WX_1_Data))]
		void Test16_Vpermt2bV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vpermt2bV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2bV_VX_k1_HX_WX_2_Data))]
		void Test16_Vpermt2bV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_Vpermt2bV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2bV_VX_k1_HX_WX_1_Data))]
		void Test32_Vpermt2bV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vpermt2bV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2bV_VX_k1_HX_WX_2_Data))]
		void Test32_Vpermt2bV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_Vpermt2bV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D2B 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F24D4B 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2bV_VX_k1_HX_WX_1_Data))]
		void Test64_Vpermt2bV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vpermt2bV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8B 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 7D 50 01", 7, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAB 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 7D 50 01", 7, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCB 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 7D 50 01", 7, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };

				yield return new object[] { "62 F2CD0B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F2CD8B 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2CD08 7D 50 01", 7, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F2CD2B 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F2CDAB 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2CD28 7D 50 01", 7, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F2CD4B 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F2CDCB 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2CD48 7D 50 01", 7, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2bV_VX_k1_HX_WX_2_Data))]
		void Test64_Vpermt2bV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_Vpermt2bV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 7D D3", 6, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 7D D3", 6, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 7D D3", 6, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 7D D3", 6, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 7D D3", 6, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 7D D3", 6, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2dV_VX_k1_HX_WX_1_Data))]
		void Test16_Vpermt2dV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vpermt2dV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2dV_VX_k1_HX_WX_2_Data))]
		void Test16_Vpermt2dV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_Vpermt2dV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2dV_VX_k1_HX_WX_1_Data))]
		void Test32_Vpermt2dV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vpermt2dV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2dV_VX_k1_HX_WX_2_Data))]
		void Test32_Vpermt2dV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_Vpermt2dV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2dV_VX_k1_HX_WX_1_Data))]
		void Test64_Vpermt2dV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vpermt2dV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 7E 50 01", 7, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 7E 50 01", 7, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 7E 50 01", 7, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 7E 50 01", 7, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 7E 50 01", 7, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 7E 50 01", 7, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2dV_VX_k1_HX_WX_2_Data))]
		void Test64_Vpermt2dV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_Vpermt2dV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 7E D3", 6, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 7E D3", 6, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 7E D3", 6, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 7E D3", 6, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 7E D3", 6, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 7E D3", 6, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vpermt2psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vpermt2psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermt2psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vpermt2psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_Vpermt2psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vpermt2psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vpermt2psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermt2psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vpermt2psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_Vpermt2psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vpermt2psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vpermt2psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 7F 50 01", 7, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 7F 50 01", 7, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 7F 50 01", 7, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 7F 50 01", 7, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 7F 50 01", 7, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 7F 50 01", 7, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermt2psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vpermt2psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_Vpermt2psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 7F D3", 6, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 7F D3", 6, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 7F D3", 6, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 7F D3", 6, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 7F D3", 6, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 7F D3", 6, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
