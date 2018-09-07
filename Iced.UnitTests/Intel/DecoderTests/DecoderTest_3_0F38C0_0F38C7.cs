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
	public sealed class DecoderTest_3_0F38C0_0F38C7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpconflictdV_VX_k1_WX_1_Data))]
		void Test16_VpconflictdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpconflictdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2FD0B C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpconflictdV_VX_k1_WX_2_Data))]
		void Test16_VpconflictdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpconflictdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpconflictdV_VX_k1_WX_1_Data))]
		void Test32_VpconflictdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpconflictdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2FD0B C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpconflictdV_VX_k1_WX_2_Data))]
		void Test32_VpconflictdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpconflictdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpconflictdV_VX_k1_WX_1_Data))]
		void Test64_VpconflictdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpconflictdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F27D9D C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F27D08 C4 50 01", 7, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F27D2B C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F27DBD C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F27D28 C4 50 01", 7, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F27D4B C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F27DDD C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F27D48 C4 50 01", 7, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2FD0B C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2FD9D C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2FD08 C4 50 01", 7, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2FD2B C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2FDBD C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2FD28 C4 50 01", 7, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2FD4B C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2FDDD C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2FD48 C4 50 01", 7, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpconflictdV_VX_k1_WX_2_Data))]
		void Test64_VpconflictdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpconflictdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B C4 D3", 6, Code.EVEX_Vpconflictd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B C4 D3", 6, Code.EVEX_Vpconflictd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B C4 D3", 6, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B C4 D3", 6, Code.EVEX_Vpconflictq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B C4 D3", 6, Code.EVEX_Vpconflictq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B C4 D3", 6, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vscattergather32V_RegMem_Reg_EVEX_1_Data))]
		void Test16_Vscattergather32V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_Vscattergather32V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D4B C6 4C A1 01", 9, Code.EVEX_Vgatherpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD4B C6 4C A1 01", 9, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "67 62 F27D4B C6 54 A1 01", 9, Code.EVEX_Vgatherpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD4B C6 54 A1 01", 9, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "67 62 F27D4B C6 6C A1 01", 9, Code.EVEX_Vscatterpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD4B C6 6C A1 01", 9, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "67 62 F27D4B C6 74 A1 01", 9, Code.EVEX_Vscatterpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD4B C6 74 A1 01", 9, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vscattergather32V_RegMem_Reg_EVEX_1_Data))]
		void Test32_Vscattergather32V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_Vscattergather32V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D4B C6 4C A1 01", 8, Code.EVEX_Vgatherpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD4B C6 4C A1 01", 8, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "62 F27D4B C6 54 A1 01", 8, Code.EVEX_Vgatherpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD4B C6 54 A1 01", 8, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "62 F27D4B C6 6C A1 01", 8, Code.EVEX_Vscatterpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD4B C6 6C A1 01", 8, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };

				yield return new object[] { "62 F27D4B C6 74 A1 01", 8, Code.EVEX_Vscatterpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD4B C6 74 A1 01", 8, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vscattergather32V_RegMem_Reg_EVEX_1_Data))]
		void Test64_Vscattergather32V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_Vscattergather32V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D4B C6 4C A1 01", 8, Code.EVEX_Vgatherpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 B27D43 C6 4C A9 01", 8, Code.EVEX_Vgatherpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 D27D43 C6 4C 8F 01", 8, Code.EVEX_Vgatherpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD4B C6 4C A1 01", 8, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 B2FD43 C6 4C A9 01", 8, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 D2FD43 C6 4C 8F 01", 8, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, false };

				yield return new object[] { "62 F27D4B C6 54 A1 01", 8, Code.EVEX_Vgatherpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 B27D43 C6 54 A9 01", 8, Code.EVEX_Vgatherpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 D27D43 C6 54 8F 01", 8, Code.EVEX_Vgatherpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD4B C6 54 A1 01", 8, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 B2FD43 C6 54 A9 01", 8, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 D2FD43 C6 54 8F 01", 8, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, false };

				yield return new object[] { "62 F27D4B C6 6C A1 01", 8, Code.EVEX_Vscatterpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 B27D43 C6 6C A9 01", 8, Code.EVEX_Vscatterpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 D27D43 C6 6C 8F 01", 8, Code.EVEX_Vscatterpf0dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD4B C6 6C A1 01", 8, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 B2FD43 C6 6C A9 01", 8, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 D2FD43 C6 6C 8F 01", 8, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, false };

				yield return new object[] { "62 F27D4B C6 74 A1 01", 8, Code.EVEX_Vscatterpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 B27D43 C6 74 A9 01", 8, Code.EVEX_Vscatterpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 D27D43 C6 74 8F 01", 8, Code.EVEX_Vscatterpf1dps_VM32Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD4B C6 74 A1 01", 8, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 B2FD43 C6 74 A9 01", 8, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 D2FD43 C6 74 8F 01", 8, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vscattergather64V_RegMem_Reg_EVEX_1_Data))]
		void Test16_Vscattergather64V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_Vscattergather64V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D4B C7 4C A1 01", 9, Code.EVEX_Vgatherpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD4B C7 4C A1 01", 9, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "67 62 F27D4B C7 54 A1 01", 9, Code.EVEX_Vgatherpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD4B C7 54 A1 01", 9, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "67 62 F27D4B C7 6C A1 01", 9, Code.EVEX_Vscatterpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD4B C7 6C A1 01", 9, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "67 62 F27D4B C7 74 A1 01", 9, Code.EVEX_Vscatterpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD4B C7 74 A1 01", 9, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vscattergather64V_RegMem_Reg_EVEX_1_Data))]
		void Test32_Vscattergather64V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_Vscattergather64V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D4B C7 4C A1 01", 8, Code.EVEX_Vgatherpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD4B C7 4C A1 01", 8, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "62 F27D4B C7 54 A1 01", 8, Code.EVEX_Vgatherpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD4B C7 54 A1 01", 8, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "62 F27D4B C7 6C A1 01", 8, Code.EVEX_Vscatterpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD4B C7 6C A1 01", 8, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };

				yield return new object[] { "62 F27D4B C7 74 A1 01", 8, Code.EVEX_Vscatterpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD4B C7 74 A1 01", 8, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vscattergather64V_RegMem_Reg_EVEX_1_Data))]
		void Test64_Vscattergather64V_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_Vscattergather64V_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D4B C7 4C A1 01", 8, Code.EVEX_Vgatherpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 B27D43 C7 4C A9 01", 8, Code.EVEX_Vgatherpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 D27D43 C7 4C 8F 01", 8, Code.EVEX_Vgatherpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD4B C7 4C A1 01", 8, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 B2FD43 C7 4C A9 01", 8, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 D2FD43 C7 4C 8F 01", 8, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.R15, Register.ZMM17, 8, true };

				yield return new object[] { "62 F27D4B C7 54 A1 01", 8, Code.EVEX_Vgatherpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 B27D43 C7 54 A9 01", 8, Code.EVEX_Vgatherpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 D27D43 C7 54 8F 01", 8, Code.EVEX_Vgatherpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD4B C7 54 A1 01", 8, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 B2FD43 C7 54 A9 01", 8, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 D2FD43 C7 54 8F 01", 8, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.R15, Register.ZMM17, 8, true };

				yield return new object[] { "62 F27D4B C7 6C A1 01", 8, Code.EVEX_Vscatterpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 B27D43 C7 6C A9 01", 8, Code.EVEX_Vscatterpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 D27D43 C7 6C 8F 01", 8, Code.EVEX_Vscatterpf0qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD4B C7 6C A1 01", 8, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 B2FD43 C7 6C A9 01", 8, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 D2FD43 C7 6C 8F 01", 8, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.R15, Register.ZMM17, 8, true };

				yield return new object[] { "62 F27D4B C7 74 A1 01", 8, Code.EVEX_Vscatterpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 B27D43 C7 74 A9 01", 8, Code.EVEX_Vscatterpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 D27D43 C7 74 8F 01", 8, Code.EVEX_Vscatterpf1qps_VM64Z_k1, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD4B C7 74 A1 01", 8, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 B2FD43 C7 74 A9 01", 8, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 D2FD43 C7 74 8F 01", 8, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, Register.K3, MemorySize.Float64, Register.R15, Register.ZMM17, 8, true };
			}
		}
	}
}
