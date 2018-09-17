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
	public sealed class DecoderTest_3_0F3818_0F381F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VbroadcastssV_Reg_RegMem_1_Data))]
		void Test16_VbroadcastssV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VbroadcastssV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 18 10", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C4E27D 18 10", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM2, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastssV_Reg_RegMem_2_Data))]
		void Test16_VbroadcastssV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VbroadcastssV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastssV_Reg_RegMem_1_Data))]
		void Test32_VbroadcastssV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VbroadcastssV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 18 10", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C4E27D 18 10", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM2, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastssV_Reg_RegMem_2_Data))]
		void Test32_VbroadcastssV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VbroadcastssV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastssV_Reg_RegMem_1_Data))]
		void Test64_VbroadcastssV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VbroadcastssV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 18 10", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C4E27D 18 10", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM2, MemorySize.Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastssV_Reg_RegMem_2_Data))]
		void Test64_VbroadcastssV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VbroadcastssV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 18 CD", 5, Code.VEX_Vbroadcastss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 18 CD", 5, Code.VEX_Vbroadcastss_ymm_xmmm32, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastssV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VbroadcastssV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VbroadcastssV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27D8B 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D28 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DAB 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D48 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DCB 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.K3, MemorySize.Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastssV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VbroadcastssV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VbroadcastssV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastssV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VbroadcastssV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VbroadcastssV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27D8B 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D28 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DAB 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D48 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DCB 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.K3, MemorySize.Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastssV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VbroadcastssV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VbroadcastssV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastssV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VbroadcastssV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VbroadcastssV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27D8B 18 50 01", 7, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D28 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DAB 18 50 01", 7, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F27D48 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F27DCB 18 50 01", 7, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.K3, MemorySize.Float32, 4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastssV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VbroadcastssV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VbroadcastssV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 18 D3", 6, Code.EVEX_Vbroadcastss_xmm_k1z_xmmm32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 18 D3", 6, Code.EVEX_Vbroadcastss_ymm_k1z_xmmm32, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 18 D3", 6, Code.EVEX_Vbroadcastss_zmm_k1z_xmmm32, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastsdV_Reg_RegMem_1_Data))]
		void Test16_VbroadcastsdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VbroadcastsdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 19 10", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastsdV_Reg_RegMem_2_Data))]
		void Test16_VbroadcastsdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VbroadcastsdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E27D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastsdV_Reg_RegMem_1_Data))]
		void Test32_VbroadcastsdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VbroadcastsdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 19 10", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastsdV_Reg_RegMem_2_Data))]
		void Test32_VbroadcastsdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VbroadcastsdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E27D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastsdV_Reg_RegMem_1_Data))]
		void Test64_VbroadcastsdV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VbroadcastsdV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 19 10", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastsdV_Reg_RegMem_2_Data))]
		void Test64_VbroadcastsdV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VbroadcastsdV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E27D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 19 CD", 5, Code.VEX_Vbroadcastsd_ymm_xmmm64, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastsdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VbroadcastsdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VbroadcastsdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DAB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F27D48 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DCB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F2FD28 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDAB 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F2FD48 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDCB 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastsdV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VbroadcastsdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VbroadcastsdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D28 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastsdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VbroadcastsdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VbroadcastsdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DAB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F27D48 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DCB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F2FD28 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDAB 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F2FD48 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDCB 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastsdV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VbroadcastsdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VbroadcastsdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D28 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDCB 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastsdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VbroadcastsdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VbroadcastsdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DAB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F27D48 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Packed64_Float32, 8, false };
				yield return new object[] { "62 F27DCB 19 50 01", 7, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Packed64_Float32, 8, true };

				yield return new object[] { "62 F2FD28 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDAB 19 50 01", 7, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F2FD48 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2FDCB 19 50 01", 7, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastsdV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VbroadcastsdV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VbroadcastsdV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D28 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 19 D3", 6, Code.EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD28 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD2B 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDAB 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDAB 19 D3", 6, Code.EVEX_Vbroadcastsd_ymm_k1z_xmmm64, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD48 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD4B 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 32FDCB 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C2FDCB 19 D3", 6, Code.EVEX_Vbroadcastsd_zmm_k1z_xmmm64, Register.ZMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vbroadcastf128V_Reg_RegMem_1_Data))]
		void Test16_Vbroadcastf128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vbroadcastf128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 1A 10", 5, Code.VEX_Vbroadcastf128_ymm_m128, Register.YMM2, MemorySize.Float128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vbroadcastf128V_Reg_RegMem_1_Data))]
		void Test32_Vbroadcastf128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vbroadcastf128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 1A 10", 5, Code.VEX_Vbroadcastf128_ymm_m128, Register.YMM2, MemorySize.Float128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vbroadcastf128V_Reg_RegMem_1_Data))]
		void Test64_Vbroadcastf128V_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vbroadcastf128V_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E27D 1A 10", 5, Code.VEX_Vbroadcastf128_ymm_m128, Register.YMM2, MemorySize.Float128 };
				yield return new object[] { "C4627D 1A 10", 5, Code.VEX_Vbroadcastf128_ymm_m128, Register.YMM10, MemorySize.Float128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastfV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VbroadcastfV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VbroadcastfV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DAB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F27D48 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DCB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F2FD28 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDAB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F2FD48 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDCB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastfV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VbroadcastfV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VbroadcastfV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DAB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F27D48 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DCB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F2FD28 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDAB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F2FD48 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDCB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastfV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VbroadcastfV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VbroadcastfV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D28 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 327D28 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM10, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E27D28 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM18, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DAB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F27D48 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 327D48 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM10, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E27D48 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM18, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27DCB 1A 50 01", 7, Code.EVEX_Vbroadcastf32x4_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F2FD28 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 32FD28 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM10, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 E2FD28 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM18, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDAB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_ymm_k1z_m128, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F2FD48 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 32FD48 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM10, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 E2FD48 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM18, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FDCB 1A 50 01", 7, Code.EVEX_Vbroadcastf64x2_zmm_k1z_m128, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VbroadcastfV2_Reg_RegMem_EVEX_1_Data))]
		void Test16_VbroadcastfV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VbroadcastfV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DCB 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F2FD48 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDCB 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VbroadcastfV2_Reg_RegMem_EVEX_1_Data))]
		void Test32_VbroadcastfV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VbroadcastfV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DCB 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F2FD48 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDCB 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VbroadcastfV2_Reg_RegMem_EVEX_1_Data))]
		void Test64_VbroadcastfV2_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VbroadcastfV2_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D48 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 327D48 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM10, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 E27D48 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM18, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DCB 1B 50 01", 7, Code.EVEX_Vbroadcastf32x8_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F2FD48 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 32FD48 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM10, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 E2FD48 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM18, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDCB 1B 50 01", 7, Code.EVEX_Vbroadcastf64x4_zmm_k1z_m256, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabsbV_Reg_RegMem_1_Data))]
		void Test16_PabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381C 08", 4, Code.Pabsb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabsbV_Reg_RegMem_2_Data))]
		void Test16_PabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381C CD", 4, Code.Pabsb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabsbV_Reg_RegMem_1_Data))]
		void Test32_PabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381C 08", 4, Code.Pabsb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabsbV_Reg_RegMem_2_Data))]
		void Test32_PabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381C CD", 4, Code.Pabsb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabsbV_Reg_RegMem_1_Data))]
		void Test64_PabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381C 08", 4, Code.Pabsb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabsbV_Reg_RegMem_2_Data))]
		void Test64_PabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381C CD", 4, Code.Pabsb_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F381C CD", 5, Code.Pabsb_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsbV_Reg_RegMem_1_Data))]
		void Test16_VpabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381C 08", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E279 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2F9 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E27D 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2FD 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsbV_Reg_RegMem_2_Data))]
		void Test16_VpabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381C CD", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsbV_Reg_RegMem_1_Data))]
		void Test32_VpabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381C 08", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E279 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2F9 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E27D 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2FD 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsbV_Reg_RegMem_2_Data))]
		void Test32_VpabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381C CD", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsbV_Reg_RegMem_1_Data))]
		void Test64_VpabsbV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpabsbV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381C 08", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E279 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2F9 1C 10", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };

				yield return new object[] { "C4E27D 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2FD 1C 10", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsbV_Reg_RegMem_2_Data))]
		void Test64_VpabsbV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpabsbV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381C CD", 5, Code.Pabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F381C CD", 6, Code.Pabsb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F381C CD", 6, Code.Pabsb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F381C CD", 6, Code.Pabsb_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 1C CD", 5, Code.VEX_Vpabsb_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C4627D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C27D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4427D 1C CD", 5, Code.VEX_Vpabsb_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsbV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpabsbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpabsbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27D8B 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DAB 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F27D48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F27DCB 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F2FD48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsbV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpabsbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpabsbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsbV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpabsbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpabsbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27D8B 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DAB 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F27D48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F27DCB 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F2FD48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsbV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpabsbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpabsbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsbV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpabsbV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpabsbV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F27D8B 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F2FD08 1C 50 01", 7, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F27D28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F27DAB 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F2FD28 1C 50 01", 7, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F27D48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F27DCB 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F2FD48 1C 50 01", 7, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsbV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpabsbV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpabsbV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 1C D3", 6, Code.EVEX_Vpabsb_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 1C D3", 6, Code.EVEX_Vpabsb_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 1C D3", 6, Code.EVEX_Vpabsb_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabswV_Reg_RegMem_1_Data))]
		void Test16_PabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381D 08", 4, Code.Pabsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabswV_Reg_RegMem_2_Data))]
		void Test16_PabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381D CD", 4, Code.Pabsw_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabswV_Reg_RegMem_1_Data))]
		void Test32_PabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381D 08", 4, Code.Pabsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabswV_Reg_RegMem_2_Data))]
		void Test32_PabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381D CD", 4, Code.Pabsw_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabswV_Reg_RegMem_1_Data))]
		void Test64_PabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F381D 08", 4, Code.Pabsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabswV_Reg_RegMem_2_Data))]
		void Test64_PabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F381D CD", 4, Code.Pabsw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F381D CD", 5, Code.Pabsw_mm_mmm64, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabswV_Reg_RegMem_1_Data))]
		void Test16_VpabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381D 08", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E279 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2F9 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E27D 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2FD 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabswV_Reg_RegMem_2_Data))]
		void Test16_VpabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381D CD", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabswV_Reg_RegMem_1_Data))]
		void Test32_VpabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381D 08", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E279 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2F9 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E27D 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2FD 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabswV_Reg_RegMem_2_Data))]
		void Test32_VpabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381D CD", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabswV_Reg_RegMem_1_Data))]
		void Test64_VpabswV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpabswV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F381D 08", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E279 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2F9 1D 10", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };

				yield return new object[] { "C4E27D 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2FD 1D 10", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabswV_Reg_RegMem_2_Data))]
		void Test64_VpabswV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpabswV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F381D CD", 5, Code.Pabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F381D CD", 6, Code.Pabsw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F381D CD", 6, Code.Pabsw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F381D CD", 6, Code.Pabsw_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 1D CD", 5, Code.VEX_Vpabsw_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C4627D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C27D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4427D 1D CD", 5, Code.VEX_Vpabsw_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabswV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpabswV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpabswV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27D8B 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DAB 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F27D48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F27DCB 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2FD48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabswV_Reg_RegMem_EVEX_2_Data))]
		void Test16_VpabswV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpabswV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabswV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpabswV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpabswV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27D8B 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DAB 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F27D48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F27DCB 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2FD48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabswV_Reg_RegMem_EVEX_2_Data))]
		void Test32_VpabswV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpabswV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DCB 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabswV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpabswV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpabswV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F27D8B 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F2FD08 1D 50 01", 7, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F27D28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F27DAB 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F2FD28 1D 50 01", 7, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F27D48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F27DCB 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F2FD48 1D 50 01", 7, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabswV_Reg_RegMem_EVEX_2_Data))]
		void Test64_VpabswV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpabswV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D08 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D0B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327D8B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27D8B 1D D3", 6, Code.EVEX_Vpabsw_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D28 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D2B 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DAB 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DAB 1D D3", 6, Code.EVEX_Vpabsw_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D48 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D4B 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 327DCB 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C27DCB 1D D3", 6, Code.EVEX_Vpabsw_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabsdV_VX_WX_1_Data))]
		void Test16_PabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F381E 08", 4, Code.Pabsd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F381E 08", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PabsdV_VX_WX_2_Data))]
		void Test16_PabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F381E CD", 4, Code.Pabsd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F381E CD", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabsdV_VX_WX_1_Data))]
		void Test32_PabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F381E 08", 4, Code.Pabsd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F381E 08", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PabsdV_VX_WX_2_Data))]
		void Test32_PabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F381E CD", 4, Code.Pabsd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F381E CD", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabsdV_VX_WX_1_Data))]
		void Test64_PabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F381E 08", 4, Code.Pabsd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F381E 08", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PabsdV_VX_WX_2_Data))]
		void Test64_PabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F381E CD", 4, Code.Pabsd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F381E CD", 5, Code.Pabsd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F381E CD", 6, Code.Pabsd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F381E CD", 6, Code.Pabsd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F381E CD", 6, Code.Pabsd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsdV_VX_WX_1_Data))]
		void Test16_VpabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VpabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E279 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2F9 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E27D 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2FD 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsdV_VX_WX_2_Data))]
		void Test16_VpabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VpabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E279 1E D3", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C4E27D 1E D3", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsdV_VX_WX_1_Data))]
		void Test32_VpabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VpabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E279 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2F9 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E27D 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2FD 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsdV_VX_WX_2_Data))]
		void Test32_VpabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VpabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E279 1E D3", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C4E27D 1E D3", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsdV_VX_WX_1_Data))]
		void Test64_VpabsdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VpabsdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E279 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2F9 1E 10", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C4E27D 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2FD 1E 10", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsdV_VX_WX_2_Data))]
		void Test64_VpabsdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VpabsdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E279 1E D3", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C46279 1E D3", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C279 1E D3", 5, Code.VEX_Vpabsd_xmm_xmmm128, Register.XMM2, Register.XMM11 };

				yield return new object[] { "C4E27D 1E D3", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, Register.YMM3 };
				yield return new object[] { "C4627D 1E D3", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM10, Register.YMM3 };
				yield return new object[] { "C4C27D 1E D3", 5, Code.VEX_Vpabsd_ymm_ymmm256, Register.YMM2, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsdV_VX_k1_WX_1_Data))]
		void Test16_VpabsdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpabsdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsdV_VX_k1_WX_2_Data))]
		void Test16_VpabsdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpabsdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsdV_VX_k1_WX_1_Data))]
		void Test32_VpabsdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpabsdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsdV_VX_k1_WX_2_Data))]
		void Test32_VpabsdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpabsdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsdV_VX_k1_WX_1_Data))]
		void Test64_VpabsdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpabsdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 1E 50 01", 7, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 1E 50 01", 7, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 1E 50 01", 7, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsdV_VX_k1_WX_2_Data))]
		void Test64_VpabsdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpabsdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B 1E D3", 6, Code.EVEX_Vpabsd_xmm_k1z_xmmm128b32, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B 1E D3", 6, Code.EVEX_Vpabsd_ymm_k1z_ymmm256b32, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B 1E D3", 6, Code.EVEX_Vpabsd_zmm_k1z_zmmm512b32, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsqV_VX_k1_WX_1_Data))]
		void Test16_VpabsqV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpabsqV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F2FD0B 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpabsqV_VX_k1_WX_2_Data))]
		void Test16_VpabsqV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpabsqV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F2FD8B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsqV_VX_k1_WX_1_Data))]
		void Test32_VpabsqV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpabsqV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F2FD0B 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpabsqV_VX_k1_WX_2_Data))]
		void Test32_VpabsqV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpabsqV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F2FD8B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsqV_VX_k1_WX_1_Data))]
		void Test64_VpabsqV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpabsqV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F2FD0B 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 1F 50 01", 7, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 1F 50 01", 7, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 1F 50 01", 7, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpabsqV_VX_k1_WX_2_Data))]
		void Test64_VpabsqV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpabsqV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F2FD8B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B 1F D3", 6, Code.EVEX_Vpabsq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B 1F D3", 6, Code.EVEX_Vpabsq_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B 1F D3", 6, Code.EVEX_Vpabsq_zmm_k1z_zmmm512b64, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
