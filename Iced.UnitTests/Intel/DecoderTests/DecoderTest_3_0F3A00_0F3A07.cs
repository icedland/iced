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
	public sealed class DecoderTest_3_0F3A00_0F3A07 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpermqV_VX_WX_1_Data))]
		void Test16_VpermqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpermqV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 00 10 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Int64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermqV_VX_WX_2_Data))]
		void Test16_VpermqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpermqV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 00 D3 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermqV_VX_WX_1_Data))]
		void Test32_VpermqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpermqV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 00 10 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Int64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermqV_VX_WX_2_Data))]
		void Test32_VpermqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpermqV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 00 D3 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermqV_VX_WX_1_Data))]
		void Test64_VpermqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpermqV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 00 10 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Int64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermqV_VX_WX_2_Data))]
		void Test64_VpermqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpermqV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 00 D3 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C463FD 00 D3 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C3FD 00 D3 A5", 6, Code.VEX_Vpermq_VY_WY_Ib, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermq_VX_k1z_WX_Ib_1_Data))]
		void Test16_Vpermq_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermq_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermq_VX_k1z_WX_Ib_2_Data))]
		void Test16_Vpermq_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermq_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermq_VX_k1z_WX_Ib_1_Data))]
		void Test32_Vpermq_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermq_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermq_VX_k1z_WX_Ib_2_Data))]
		void Test32_Vpermq_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermq_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermq_VX_k1z_WX_Ib_1_Data))]
		void Test64_Vpermq_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermq_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 00 50 01 A5", 8, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 00 50 01 A5", 8, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermq_VX_k1z_WX_Ib_2_Data))]
		void Test64_Vpermq_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermq_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD2B 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD2B 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD28 00 D3 A5", 7, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD4B 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD4B 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD48 00 D3 A5", 7, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpdV_VX_WX_1_Data))]
		void Test16_VpermpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpermpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 01 10 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpdV_VX_WX_2_Data))]
		void Test16_VpermpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpermpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 01 D3 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpdV_VX_WX_1_Data))]
		void Test32_VpermpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpermpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 01 10 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpdV_VX_WX_2_Data))]
		void Test32_VpermpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpermpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 01 D3 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpdV_VX_WX_1_Data))]
		void Test64_VpermpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpermpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E3FD 01 10 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpdV_VX_WX_2_Data))]
		void Test64_VpermpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpermpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E3FD 01 D3 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C463FD 01 D3 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C3FD 01 D3 A5", 6, Code.VEX_Vpermpd_VY_WY_Ib, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermpd_VX_k1z_WX_Ib_1_Data))]
		void Test16_Vpermpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermpd_VX_k1z_WX_Ib_2_Data))]
		void Test16_Vpermpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermpd_VX_k1z_WX_Ib_1_Data))]
		void Test32_Vpermpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermpd_VX_k1z_WX_Ib_2_Data))]
		void Test32_Vpermpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermpd_VX_k1z_WX_Ib_1_Data))]
		void Test64_Vpermpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 01 50 01 A5", 8, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 01 50 01 A5", 8, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermpd_VX_k1z_WX_Ib_2_Data))]
		void Test64_Vpermpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FDAB 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD2B 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD2B 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD28 01 D3 A5", 7, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD4B 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD4B 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD48 01 D3 A5", 7, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblenddV_VX_HX_WX_Ib_1_Data))]
		void Test16_VpblenddV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpblenddV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 02 10 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C4E34D 02 10 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpblenddV_VX_HX_WX_Ib_2_Data))]
		void Test16_VpblenddV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpblenddV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E34D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblenddV_VX_HX_WX_Ib_1_Data))]
		void Test32_VpblenddV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpblenddV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 02 10 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C4E34D 02 10 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpblenddV_VX_HX_WX_Ib_2_Data))]
		void Test32_VpblenddV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpblenddV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E34D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblenddV_VX_HX_WX_Ib_1_Data))]
		void Test64_VpblenddV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpblenddV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 02 10 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C4E34D 02 10 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpblenddV_VX_HX_WX_Ib_2_Data))]
		void Test64_VpblenddV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpblenddV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E309 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 02 D3 A5", 6, Code.VEX_Vpblendd_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E34D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4634D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E30D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C34D 02 D3 A5", 6, Code.VEX_Vpblendd_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ValigndV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_ValigndV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_ValigndV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ValigndV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_ValigndV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_ValigndV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ValigndV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_ValigndV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_ValigndV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ValigndV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_ValigndV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_ValigndV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ValigndV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_ValigndV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_ValigndV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 03 50 01 A5", 8, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 03 50 01 A5", 8, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 03 50 01 A5", 8, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 03 50 01 A5", 8, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 03 50 01 A5", 8, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 03 50 01 A5", 8, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ValigndV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_ValigndV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_ValigndV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D0B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D03 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 03 D3 A5", 7, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 03 D3 A5", 7, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 03 D3 A5", 7, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 03 D3 A5", 7, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 03 D3 A5", 7, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 03 D3 A5", 7, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermilpsV_VX_WX_1_Data))]
		void Test16_VpermilpsV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpermilpsV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 04 10 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 04 10 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermilpsV_VX_WX_2_Data))]
		void Test16_VpermilpsV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpermilpsV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 04 D3 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 04 D3 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermilpsV_VX_WX_1_Data))]
		void Test32_VpermilpsV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpermilpsV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 04 10 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 04 10 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermilpsV_VX_WX_2_Data))]
		void Test32_VpermilpsV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpermilpsV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 04 D3 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 04 D3 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermilpsV_VX_WX_1_Data))]
		void Test64_VpermilpsV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpermilpsV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 04 10 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "C4E37D 04 10 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermilpsV_VX_WX_2_Data))]
		void Test64_VpermilpsV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpermilpsV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 04 D3 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 04 D3 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 04 D3 A5", 6, Code.VEX_Vpermilps_VX_WX_Ib, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E37D 04 D3 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C4637D 04 D3 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C37D 04 D3 A5", 6, Code.VEX_Vpermilps_VY_WY_Ib, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermilps_VX_k1z_WX_Ib_1_Data))]
		void Test16_Vpermilps_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermilps_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D8B 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D1D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D3D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true, 0xA5 };
				yield return new object[] { "62 F37D5D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermilps_VX_k1z_WX_Ib_2_Data))]
		void Test16_Vpermilps_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermilps_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D8B 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D08 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D28 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D48 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermilps_VX_k1z_WX_Ib_1_Data))]
		void Test32_Vpermilps_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermilps_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D8B 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D1D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D3D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true, 0xA5 };
				yield return new object[] { "62 F37D5D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermilps_VX_k1z_WX_Ib_2_Data))]
		void Test32_Vpermilps_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermilps_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D8B 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D08 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D28 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F37D48 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermilps_VX_k1z_WX_Ib_1_Data))]
		void Test64_Vpermilps_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermilps_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D8B 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D1D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 04 50 01 A5", 8, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D3D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 04 50 01 A5", 8, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true, 0xA5 };
				yield return new object[] { "62 F37D5D 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 04 50 01 A5", 8, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermilps_VX_k1z_WX_Ib_2_Data))]
		void Test64_Vpermilps_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermilps_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D8B 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E37D0B 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 137D0B 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B37D08 04 D3 A5", 7, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DAB 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E37D2B 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 137D2B 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B37D28 04 D3 A5", 7, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F37DCB 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E37D4B 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 137D4B 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B37D48 04 D3 A5", 7, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermilpdV_VX_WX_1_Data))]
		void Test16_VpermilpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpermilpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 05 10 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 05 10 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermilpdV_VX_WX_2_Data))]
		void Test16_VpermilpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpermilpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 05 D3 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 05 D3 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermilpdV_VX_WX_1_Data))]
		void Test32_VpermilpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpermilpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 05 10 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 05 10 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermilpdV_VX_WX_2_Data))]
		void Test32_VpermilpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpermilpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 05 D3 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E37D 05 D3 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermilpdV_VX_WX_1_Data))]
		void Test64_VpermilpdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpermilpdV_VX_WX_1_Data {
			get {
				yield return new object[] { "C4E379 05 10 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, MemorySize.Packed128_Float64, 0xA5 };

				yield return new object[] { "C4E37D 05 10 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermilpdV_VX_WX_2_Data))]
		void Test64_VpermilpdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpermilpdV_VX_WX_2_Data {
			get {
				yield return new object[] { "C4E379 05 D3 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 05 D3 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 05 D3 A5", 6, Code.VEX_Vpermilpd_VX_WX_Ib, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E37D 05 D3 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C4637D 05 D3 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C37D 05 D3 A5", 6, Code.VEX_Vpermilpd_VY_WY_Ib, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermilpd_VX_k1z_WX_Ib_1_Data))]
		void Test16_Vpermilpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermilpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD1D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpermilpd_VX_k1z_WX_Ib_2_Data))]
		void Test16_Vpermilpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpermilpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD08 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermilpd_VX_k1z_WX_Ib_1_Data))]
		void Test32_Vpermilpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermilpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD1D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpermilpd_VX_k1z_WX_Ib_2_Data))]
		void Test32_Vpermilpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpermilpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD08 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD28 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F3FD48 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermilpd_VX_k1z_WX_Ib_1_Data))]
		void Test64_Vpermilpd_VX_k1z_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermilpd_VX_k1z_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD1D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD3D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true, 0xA5 };
				yield return new object[] { "62 F3FD5D 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 05 50 01 A5", 8, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpermilpd_VX_k1z_WX_Ib_2_Data))]
		void Test64_Vpermilpd_VX_k1z_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpermilpd_VX_k1z_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3FD8B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD0B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD0B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD08 05 D3 A5", 7, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDAB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD2B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD2B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD28 05 D3 A5", 7, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F3FDCB 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E3FD4B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 13FD4B 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B3FD48 05 D3 A5", 7, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vperm2f128V_VX_HX_WX_Ib_1_Data))]
		void Test16_Vperm2f128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vperm2f128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 06 10 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vperm2f128V_VX_HX_WX_Ib_2_Data))]
		void Test16_Vperm2f128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vperm2f128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vperm2f128V_VX_HX_WX_Ib_1_Data))]
		void Test32_Vperm2f128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vperm2f128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 06 10 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vperm2f128V_VX_HX_WX_Ib_2_Data))]
		void Test32_Vperm2f128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vperm2f128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vperm2f128V_VX_HX_WX_Ib_1_Data))]
		void Test64_Vperm2f128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vperm2f128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 06 10 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vperm2f128V_VX_HX_WX_Ib_2_Data))]
		void Test64_Vperm2f128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vperm2f128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4634D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E30D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C34D 06 D3 A5", 6, Code.VEX_Vperm2f128_VY_HY_WY_Ib, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}
	}
}
