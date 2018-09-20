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

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest_1_70_77 : DecoderTest {
		[Theory]
		[InlineData("70 5A", 2, Code.Jo_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("70 A5", 2, Code.Jo_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("71 5A", 2, Code.Jno_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("71 A5", 2, Code.Jno_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("72 5A", 2, Code.Jb_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("72 A5", 2, Code.Jb_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("73 5A", 2, Code.Jae_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("73 A5", 2, Code.Jae_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("74 5A", 2, Code.Je_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("74 A5", 2, Code.Je_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("75 5A", 2, Code.Jne_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("75 A5", 2, Code.Jne_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("76 5A", 2, Code.Jbe_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("76 A5", 2, Code.Jbe_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("77 5A", 2, Code.Ja_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("77 A5", 2, Code.Ja_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		void Test16_Jcc_Jb16_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch16, instr.Op0Kind);
			Assert.Equal((ushort)target, instr.NearBranch16);
		}

		[Theory]
		[InlineData("66 70 5A", 3, Code.Jo_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 70 A5", 3, Code.Jo_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 71 5A", 3, Code.Jno_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 71 A5", 3, Code.Jno_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 72 5A", 3, Code.Jb_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 72 A5", 3, Code.Jb_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 73 5A", 3, Code.Jae_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 73 A5", 3, Code.Jae_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 74 5A", 3, Code.Je_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 74 A5", 3, Code.Je_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 75 5A", 3, Code.Jne_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 75 A5", 3, Code.Jne_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 76 5A", 3, Code.Jbe_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 76 A5", 3, Code.Jbe_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 77 5A", 3, Code.Ja_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 77 A5", 3, Code.Ja_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		void Test32_Jcc_Jb16_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch16, instr.Op0Kind);
			Assert.Equal((ushort)target, instr.NearBranch16);
		}

		[Theory]
		[InlineData("66 70 5A", 3, Code.Jo_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 70 A5", 3, Code.Jo_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 71 5A", 3, Code.Jno_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 71 A5", 3, Code.Jno_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 72 5A", 3, Code.Jb_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 72 A5", 3, Code.Jb_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 73 5A", 3, Code.Jae_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 73 A5", 3, Code.Jae_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 74 5A", 3, Code.Je_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 74 A5", 3, Code.Je_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 75 5A", 3, Code.Jne_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 75 A5", 3, Code.Jne_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 76 5A", 3, Code.Jbe_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 76 A5", 3, Code.Jbe_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 77 5A", 3, Code.Ja_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 77 A5", 3, Code.Ja_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		void Test16_Jcc_Jb32_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch32, instr.Op0Kind);
			Assert.Equal((uint)target, instr.NearBranch32);
		}

		[Theory]
		[InlineData("70 5A", 2, Code.Jo_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("70 A5", 2, Code.Jo_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("71 5A", 2, Code.Jno_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("71 A5", 2, Code.Jno_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("72 5A", 2, Code.Jb_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("72 A5", 2, Code.Jb_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("73 5A", 2, Code.Jae_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("73 A5", 2, Code.Jae_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("74 5A", 2, Code.Je_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("74 A5", 2, Code.Je_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("75 5A", 2, Code.Jne_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("75 A5", 2, Code.Jne_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("76 5A", 2, Code.Jbe_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("76 A5", 2, Code.Jbe_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("77 5A", 2, Code.Ja_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("77 A5", 2, Code.Ja_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		void Test32_Jcc_Jb32_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch32, instr.Op0Kind);
			Assert.Equal((uint)target, instr.NearBranch32);
		}

		[Theory]
		[InlineData("70 5A", 2, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("70 A5", 2, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("71 5A", 2, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("71 A5", 2, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("72 5A", 2, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("72 A5", 2, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("73 5A", 2, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("73 A5", 2, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("74 5A", 2, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("74 A5", 2, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("75 5A", 2, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("75 A5", 2, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("76 5A", 2, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("76 A5", 2, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("77 5A", 2, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("77 A5", 2, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]

		[InlineData("66 70 5A", 3, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 70 A5", 3, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 71 5A", 3, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 71 A5", 3, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 72 5A", 3, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 72 A5", 3, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 73 5A", 3, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 73 A5", 3, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 74 5A", 3, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 74 A5", 3, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 75 5A", 3, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 75 A5", 3, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 76 5A", 3, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 76 A5", 3, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 77 5A", 3, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 77 A5", 3, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]

		[InlineData("4F 70 5A", 3, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 70 A5", 3, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 71 5A", 3, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 71 A5", 3, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 72 5A", 3, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 72 A5", 3, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 73 5A", 3, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 73 A5", 3, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 74 5A", 3, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 74 A5", 3, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 75 5A", 3, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 75 A5", 3, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 76 5A", 3, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 76 A5", 3, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F 77 5A", 3, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F 77 A5", 3, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]

		[InlineData("66 4F 70 5A", 4, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 70 A5", 4, Code.Jo_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 71 5A", 4, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 71 A5", 4, Code.Jno_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 72 5A", 4, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 72 A5", 4, Code.Jb_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 73 5A", 4, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 73 A5", 4, Code.Jae_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 74 5A", 4, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 74 A5", 4, Code.Je_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 75 5A", 4, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 75 A5", 4, Code.Jne_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 76 5A", 4, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 76 A5", 4, Code.Jbe_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 77 5A", 4, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 77 A5", 4, Code.Ja_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		void Test64_Jcc_Jb64_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch64, instr.Op0Kind);
			Assert.Equal(target, instr.NearBranch64);
		}
	}
}
