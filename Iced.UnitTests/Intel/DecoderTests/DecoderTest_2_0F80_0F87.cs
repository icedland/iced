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
	public sealed class DecoderTest_2_0F80_0F87 : DecoderTest {
		[Theory]
		[InlineData("0F80 5AA5", 4, Code.Jo_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F80 A55A", 4, Code.Jo_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F81 5AA5", 4, Code.Jno_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F81 A55A", 4, Code.Jno_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F82 5AA5", 4, Code.Jb_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F82 A55A", 4, Code.Jb_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F83 5AA5", 4, Code.Jae_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F83 A55A", 4, Code.Jae_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F84 5AA5", 4, Code.Je_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F84 A55A", 4, Code.Je_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F85 5AA5", 4, Code.Jne_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F85 A55A", 4, Code.Jne_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F86 5AA5", 4, Code.Jbe_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F86 A55A", 4, Code.Jbe_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F87 5AA5", 4, Code.Ja_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F87 A55A", 4, Code.Ja_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		void Test16_Jcc_Jw16_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 0F80 5AA5", 5, Code.Jo_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F80 A55A", 5, Code.Jo_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F81 5AA5", 5, Code.Jno_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F81 A55A", 5, Code.Jno_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F82 5AA5", 5, Code.Jb_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F82 A55A", 5, Code.Jb_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F83 5AA5", 5, Code.Jae_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F83 A55A", 5, Code.Jae_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F84 5AA5", 5, Code.Je_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F84 A55A", 5, Code.Je_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F85 5AA5", 5, Code.Jne_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F85 A55A", 5, Code.Jne_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F86 5AA5", 5, Code.Jbe_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F86 A55A", 5, Code.Jbe_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F87 5AA5", 5, Code.Ja_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F87 A55A", 5, Code.Ja_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		void Test32_Jcc_Jw16_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 0F80 5AA51234", 7, Code.Jo_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F80 A56789AB", 7, Code.Jo_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F81 5AA51234", 7, Code.Jno_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F81 A56789AB", 7, Code.Jno_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F82 5AA51234", 7, Code.Jb_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F82 A56789AB", 7, Code.Jb_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F83 5AA51234", 7, Code.Jae_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F83 A56789AB", 7, Code.Jae_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F84 5AA51234", 7, Code.Je_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F84 A56789AB", 7, Code.Je_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F85 5AA51234", 7, Code.Jne_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F85 A56789AB", 7, Code.Jne_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F86 5AA51234", 7, Code.Jbe_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F86 A56789AB", 7, Code.Jbe_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F87 5AA51234", 7, Code.Ja_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F87 A56789AB", 7, Code.Ja_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		void Test16_Jcc_Jd32_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("0F80 5AA51234", 6, Code.Jo_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F80 A56789AB", 6, Code.Jo_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F81 5AA51234", 6, Code.Jno_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F81 A56789AB", 6, Code.Jno_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F82 5AA51234", 6, Code.Jb_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F82 A56789AB", 6, Code.Jb_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F83 5AA51234", 6, Code.Jae_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F83 A56789AB", 6, Code.Jae_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F84 5AA51234", 6, Code.Je_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F84 A56789AB", 6, Code.Je_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F85 5AA51234", 6, Code.Jne_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F85 A56789AB", 6, Code.Jne_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F86 5AA51234", 6, Code.Jbe_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F86 A56789AB", 6, Code.Jbe_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F87 5AA51234", 6, Code.Ja_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F87 A56789AB", 6, Code.Ja_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		void Test32_Jcc_Jd32_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("0F80 5AA51234", 6, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F80 A56789AB", 6, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F81 5AA51234", 6, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F81 A56789AB", 6, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F82 5AA51234", 6, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F82 A56789AB", 6, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F83 5AA51234", 6, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F83 A56789AB", 6, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F84 5AA51234", 6, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F84 A56789AB", 6, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F85 5AA51234", 6, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F85 A56789AB", 6, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F86 5AA51234", 6, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F86 A56789AB", 6, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F87 5AA51234", 6, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F87 A56789AB", 6, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]

		[InlineData("66 0F80 5AA51234", 7, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F80 A56789AB", 7, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F81 5AA51234", 7, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F81 A56789AB", 7, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F82 5AA51234", 7, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F82 A56789AB", 7, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F83 5AA51234", 7, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F83 A56789AB", 7, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F84 5AA51234", 7, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F84 A56789AB", 7, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F85 5AA51234", 7, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F85 A56789AB", 7, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F86 5AA51234", 7, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F86 A56789AB", 7, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F87 5AA51234", 7, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F87 A56789AB", 7, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]

		[InlineData("4F 0F80 5AA51234", 7, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F80 A56789AB", 7, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F81 5AA51234", 7, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F81 A56789AB", 7, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F82 5AA51234", 7, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F82 A56789AB", 7, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F83 5AA51234", 7, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F83 A56789AB", 7, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F84 5AA51234", 7, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F84 A56789AB", 7, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F85 5AA51234", 7, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F85 A56789AB", 7, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F86 5AA51234", 7, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F86 A56789AB", 7, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F87 5AA51234", 7, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F87 A56789AB", 7, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]

		[InlineData("66 4F 0F80 5AA51234", 8, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F80 A56789AB", 8, Code.Jo_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F81 5AA51234", 8, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F81 A56789AB", 8, Code.Jno_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F82 5AA51234", 8, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F82 A56789AB", 8, Code.Jb_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F83 5AA51234", 8, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F83 A56789AB", 8, Code.Jae_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F84 5AA51234", 8, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F84 A56789AB", 8, Code.Je_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F85 5AA51234", 8, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F85 A56789AB", 8, Code.Jne_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F86 5AA51234", 8, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F86 A56789AB", 8, Code.Jbe_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F87 5AA51234", 8, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F87 A56789AB", 8, Code.Ja_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		void Test64_Jcc_Jd64_1(string hexBytes, int byteLength, Code code, ulong target) {
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
