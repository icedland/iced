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
	public sealed class DecoderTest_2_0F08_0F0F : DecoderTest {
		[Theory]
		[InlineData("0F08", 2, Code.Invd, DecoderOptions.None)]
		[InlineData("0F09", 2, Code.Wbinvd, DecoderOptions.None)]
		[InlineData("F3 0F09", 3, Code.Wbnoinvd, DecoderOptions.None)]
		[InlineData("0F0A", 2, Code.Cflsh, DecoderOptions.Cflsh)]
		[InlineData("0F0A", 2, Code.Cl1invmb, DecoderOptions.Cl1invmb)]
		[InlineData("0F0B", 2, Code.Ud2, DecoderOptions.None)]
		[InlineData("0F0E", 2, Code.Femms, DecoderOptions.None)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F08", 2, Code.Invd, DecoderOptions.None)]
		[InlineData("0F09", 2, Code.Wbinvd, DecoderOptions.None)]
		[InlineData("F3 0F09", 3, Code.Wbnoinvd, DecoderOptions.None)]
		[InlineData("0F0A", 2, Code.Cflsh, DecoderOptions.Cflsh)]
		[InlineData("0F0A", 2, Code.Cl1invmb, DecoderOptions.Cl1invmb)]
		[InlineData("0F0B", 2, Code.Ud2, DecoderOptions.None)]
		[InlineData("0F0E", 2, Code.Femms, DecoderOptions.None)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F08", 2, Code.Invd, DecoderOptions.None)]
		[InlineData("0F09", 2, Code.Wbinvd, DecoderOptions.None)]
		[InlineData("F3 0F09", 3, Code.Wbnoinvd, DecoderOptions.None)]
		[InlineData("0F0A", 2, Code.Cl1invmb, DecoderOptions.Cl1invmb)]
		[InlineData("0F0B", 2, Code.Ud2, DecoderOptions.None)]
		[InlineData("0F0E", 2, Code.Femms, DecoderOptions.None)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_m8)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_m8)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_m8)]
		[InlineData("0F0D 18", 3, Code.Prefetch_m8_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_m8_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_m8_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_m8_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_m8_r7)]
		void Test16_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_m8)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_m8)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_m8)]
		[InlineData("0F0D 18", 3, Code.Prefetch_m8_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_m8_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_m8_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_m8_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_m8_r7)]
		void Test32_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_m8)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_m8)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_m8)]
		[InlineData("0F0D 18", 3, Code.Prefetch_m8_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_m8_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_m8_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_m8_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_m8_r7)]
		void Test64_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[MemberData(nameof(Test16_ReservedNop_E_G_1_Data))]
		void Test16_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "0F0D CE", 3, Code.ReservedNop_rm16_r16_0F0D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F0D CE", 4, Code.ReservedNop_rm32_r32_0F0D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ReservedNop_E_G_2_Data))]
		void Test16_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "0F0D 18", 3, Code.ReservedNop_rm16_r16_0F0D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F0D 18", 4, Code.ReservedNop_rm32_r32_0F0D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ReservedNop_E_G_1_Data))]
		void Test32_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "66 0F0D CE", 4, Code.ReservedNop_rm16_r16_0F0D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F0D CE", 3, Code.ReservedNop_rm32_r32_0F0D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ReservedNop_E_G_2_Data))]
		void Test32_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "66 0F0D 18", 4, Code.ReservedNop_rm16_r16_0F0D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F0D 18", 3, Code.ReservedNop_rm32_r32_0F0D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ReservedNop_E_G_1_Data))]
		void Test64_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "66 0F0D CE", 4, Code.ReservedNop_rm16_r16_0F0D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F0D C5", 5, Code.ReservedNop_rm16_r16_0F0D, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F0D D6", 5, Code.ReservedNop_rm16_r16_0F0D, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F0D D0", 5, Code.ReservedNop_rm16_r16_0F0D, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F0D D9", 5, Code.ReservedNop_rm16_r16_0F0D, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F0D EC", 5, Code.ReservedNop_rm16_r16_0F0D, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F0D CE", 3, Code.ReservedNop_rm32_r32_0F0D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F0D C5", 4, Code.ReservedNop_rm32_r32_0F0D, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F0D D6", 4, Code.ReservedNop_rm32_r32_0F0D, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F0D D0", 4, Code.ReservedNop_rm32_r32_0F0D, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F0D D9", 4, Code.ReservedNop_rm32_r32_0F0D, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F0D EC", 4, Code.ReservedNop_rm32_r32_0F0D, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F0D CE", 4, Code.ReservedNop_rm64_r64_0F0D, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F0D C5", 4, Code.ReservedNop_rm64_r64_0F0D, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F0D D6", 4, Code.ReservedNop_rm64_r64_0F0D, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F0D D0", 4, Code.ReservedNop_rm64_r64_0F0D, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F0D D9", 4, Code.ReservedNop_rm64_r64_0F0D, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F0D EC", 4, Code.ReservedNop_rm64_r64_0F0D, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ReservedNop_E_G_2_Data))]
		void Test64_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "66 0F0D 18", 4, Code.ReservedNop_rm16_r16_0F0D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F0D 18", 3, Code.ReservedNop_rm32_r32_0F0D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F0D 18", 4, Code.ReservedNop_rm64_r64_0F0D, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };
			}
		}
	}
}
