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
	public sealed class DecoderTest_3_0F3A38_0F3A3F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vinserti128V_VX_HX_WX_Ib_1_Data))]
		void Test16_Vinserti128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vinserti128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 38 10 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinserti128V_VX_HX_WX_Ib_2_Data))]
		void Test16_Vinserti128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vinserti128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti128V_VX_HX_WX_Ib_1_Data))]
		void Test32_Vinserti128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vinserti128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 38 10 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti128V_VX_HX_WX_Ib_2_Data))]
		void Test32_Vinserti128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vinserti128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti128V_VX_HX_WX_Ib_1_Data))]
		void Test64_Vinserti128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vinserti128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 38 10 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti128V_VX_HX_WX_Ib_2_Data))]
		void Test64_Vinserti128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vinserti128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM10, Register.YMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C34D 38 D3 A5", 6, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 38 50 01 A5", 8, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 38 50 01 A5", 8, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vinserti32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DAB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D23 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 38 D3 A5", 7, Code.EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DAB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD23 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 38 D3 A5", 7, Code.EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti128V_WX_VY_Ib_1_Data))]
		void Test16_Vextracti128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vextracti128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 39 10 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti128V_WX_VY_Ib_2_Data))]
		void Test16_Vextracti128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vextracti128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 39 D3 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti128V_WX_VY_Ib_1_Data))]
		void Test32_Vextracti128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vextracti128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 39 10 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti128V_WX_VY_Ib_2_Data))]
		void Test32_Vextracti128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vextracti128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 39 D3 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti128V_WX_VY_Ib_1_Data))]
		void Test64_Vextracti128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vextracti128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 39 10 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti128V_WX_VY_Ib_2_Data))]
		void Test64_Vextracti128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vextracti128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 39 D3 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
				yield return new object[] { "C4637D 39 D3 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM10, 0xA5 };
				yield return new object[] { "C4C37D 39 D3 A5", 6, Code.VEX_Vextracti128_xmmm128_ymm_imm8, Register.XMM11, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test16_Vextracti32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextracti32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test16_Vextracti32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextracti32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test32_Vextracti32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextracti32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test32_Vextracti32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextracti32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test64_Vextracti32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextracti32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 39 50 01 A5", 8, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Int64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 39 50 01 A5", 8, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test64_Vextracti32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextracti32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DAB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D2B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D2B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F37D4B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DCB 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D4B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D4B 39 D3 A5", 7, Code.EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDAB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD2B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD2B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDCB 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD4B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD4B 39 D3 A5", 7, Code.EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 3A 50 01 A5", 8, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 3A 50 01 A5", 8, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vinserti32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM18, Register.ZMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM10, Register.ZMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 3A D3 A5", 7, Code.EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM18, Register.ZMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM10, Register.ZMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 3A D3 A5", 7, Code.EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test16_Vextracti32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test16_Vextracti32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test32_Vextracti32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test32_Vextracti32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test64_Vextracti32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextracti32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 3B 50 01 A5", 8, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 3B 50 01 A5", 8, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test64_Vextracti32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextracti32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DCB 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D4B 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D4B 3B D3 A5", 7, Code.EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDCB 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD4B 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD4B 3B D3 A5", 7, Code.EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpubV_VX_k1_HX_WX_1_Data))]
		void Test16_VpcmpubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpubV_VX_k1_HX_WX_2_Data))]
		void Test16_VpcmpubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpubV_VX_k1_HX_WX_1_Data))]
		void Test32_VpcmpubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpubV_VX_k1_HX_WX_2_Data))]
		void Test32_VpcmpubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpubV_VX_k1_HX_WX_1_Data))]
		void Test64_VpcmpubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3E 50 01 A5", 8, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3E 50 01 A5", 8, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpubV_VX_k1_HX_WX_2_Data))]
		void Test64_VpcmpubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D0B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D03 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D2B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D23 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D4B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D43 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 3E D3 A5", 7, Code.EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D0B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD03 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D2B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD23 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D4B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD43 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 3E D3 A5", 7, Code.EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpcmpbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpcmpbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpcmpbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpcmpbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpcmpbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F34D0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D08 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false, 0xA5 };
				yield return new object[] { "62 F34D28 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false, 0xA5 };
				yield return new object[] { "62 F34D48 3F 50 01 A5", 8, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD0D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD08 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD2D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD28 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD4D 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD48 3F 50 01 A5", 8, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpcmpbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F34D0B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D0B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D03 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D2B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D23 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F30D4B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 934D43 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 3F D3 A5", 7, Code.EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D0B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD03 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D2B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD23 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F38D4B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 93CD43 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 3F D3 A5", 7, Code.EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}
	}
}
