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
	public sealed class DecoderTest_2_0F88_0F8F : DecoderTest {
		[Theory]
		[InlineData("0F88 5AA5", 4, Code.Js_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F88 A55A", 4, Code.Js_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F89 5AA5", 4, Code.Jns_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F89 A55A", 4, Code.Jns_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8A 5AA5", 4, Code.Jp_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8A A55A", 4, Code.Jp_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8B 5AA5", 4, Code.Jnp_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8B A55A", 4, Code.Jnp_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8C 5AA5", 4, Code.Jl_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8C A55A", 4, Code.Jl_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8D 5AA5", 4, Code.Jge_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8D A55A", 4, Code.Jge_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8E 5AA5", 4, Code.Jle_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8E A55A", 4, Code.Jle_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		[InlineData("0F8F 5AA5", 4, Code.Jg_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("0F8F A55A", 4, Code.Jg_rel16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
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
			Assert.Equal((ushort)target, instr.NearBranch16Target);
		}

		[Theory]
		[InlineData("66 0F88 5AA5", 5, Code.Js_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F88 A55A", 5, Code.Js_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F89 5AA5", 5, Code.Jns_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F89 A55A", 5, Code.Jns_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8A 5AA5", 5, Code.Jp_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8A A55A", 5, Code.Jp_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8B 5AA5", 5, Code.Jnp_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8B A55A", 5, Code.Jnp_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8C 5AA5", 5, Code.Jl_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8C A55A", 5, Code.Jl_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8D 5AA5", 5, Code.Jge_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8D A55A", 5, Code.Jge_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8E 5AA5", 5, Code.Jle_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8E A55A", 5, Code.Jle_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		[InlineData("66 0F8F 5AA5", 5, Code.Jg_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A)]
		[InlineData("66 0F8F A55A", 5, Code.Jg_rel16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
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
			Assert.Equal((ushort)target, instr.NearBranch16Target);
		}

		[Theory]
		[InlineData("66 0F88 5AA51234", 7, Code.Js_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F88 A56789AB", 7, Code.Js_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F89 5AA51234", 7, Code.Jns_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F89 A56789AB", 7, Code.Jns_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8A 5AA51234", 7, Code.Jp_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8A A56789AB", 7, Code.Jp_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8B 5AA51234", 7, Code.Jnp_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8B A56789AB", 7, Code.Jnp_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8C 5AA51234", 7, Code.Jl_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8C A56789AB", 7, Code.Jl_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8D 5AA51234", 7, Code.Jge_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8D A56789AB", 7, Code.Jge_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8E 5AA51234", 7, Code.Jle_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8E A56789AB", 7, Code.Jle_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		[InlineData("66 0F8F 5AA51234", 7, Code.Jg_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 0F8F A56789AB", 7, Code.Jg_rel32_32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
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
			Assert.Equal((uint)target, instr.NearBranch32Target);
		}

		[Theory]
		[InlineData("0F88 5AA51234", 6, Code.Js_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F88 A56789AB", 6, Code.Js_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F89 5AA51234", 6, Code.Jns_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F89 A56789AB", 6, Code.Jns_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8A 5AA51234", 6, Code.Jp_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8A A56789AB", 6, Code.Jp_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8B 5AA51234", 6, Code.Jnp_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8B A56789AB", 6, Code.Jnp_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8C 5AA51234", 6, Code.Jl_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8C A56789AB", 6, Code.Jl_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8D 5AA51234", 6, Code.Jge_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8D A56789AB", 6, Code.Jge_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8E 5AA51234", 6, Code.Jle_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8E A56789AB", 6, Code.Jle_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		[InlineData("0F8F 5AA51234", 6, Code.Jg_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("0F8F A56789AB", 6, Code.Jg_rel32_32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
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
			Assert.Equal((uint)target, instr.NearBranch32Target);
		}

		[Theory]
		[InlineData("0F88 5AA51234", 6, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F88 A56789AB", 6, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F89 5AA51234", 6, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F89 A56789AB", 6, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8A 5AA51234", 6, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8A A56789AB", 6, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8B 5AA51234", 6, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8B A56789AB", 6, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8C 5AA51234", 6, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8C A56789AB", 6, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8D 5AA51234", 6, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8D A56789AB", 6, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8E 5AA51234", 6, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8E A56789AB", 6, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		[InlineData("0F8F 5AA51234", 6, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("0F8F A56789AB", 6, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]

		[InlineData("66 0F88 5AA51234", 7, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F88 A56789AB", 7, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F89 5AA51234", 7, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F89 A56789AB", 7, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8A 5AA51234", 7, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8A A56789AB", 7, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8B 5AA51234", 7, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8B A56789AB", 7, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8C 5AA51234", 7, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8C A56789AB", 7, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8D 5AA51234", 7, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8D A56789AB", 7, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8E 5AA51234", 7, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8E A56789AB", 7, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("66 0F8F 5AA51234", 7, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("66 0F8F A56789AB", 7, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]

		[InlineData("4F 0F88 5AA51234", 7, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F88 A56789AB", 7, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F89 5AA51234", 7, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F89 A56789AB", 7, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8A 5AA51234", 7, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8A A56789AB", 7, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8B 5AA51234", 7, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8B A56789AB", 7, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8C 5AA51234", 7, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8C A56789AB", 7, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8D 5AA51234", 7, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8D A56789AB", 7, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8E 5AA51234", 7, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8E A56789AB", 7, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]
		[InlineData("4F 0F8F 5AA51234", 7, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("4F 0F8F A56789AB", 7, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]

		[InlineData("66 4F 0F88 5AA51234", 8, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F88 A56789AB", 8, Code.Js_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F89 5AA51234", 8, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F89 A56789AB", 8, Code.Jns_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8A 5AA51234", 8, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8A A56789AB", 8, Code.Jp_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8B 5AA51234", 8, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8B A56789AB", 8, Code.Jnp_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8C 5AA51234", 8, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8C A56789AB", 8, Code.Jl_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8D 5AA51234", 8, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8D A56789AB", 8, Code.Jge_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8E 5AA51234", 8, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8E A56789AB", 8, Code.Jle_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		[InlineData("66 4F 0F8F 5AA51234", 8, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F 0F8F A56789AB", 8, Code.Jg_rel32_64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
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
			Assert.Equal(target, instr.NearBranch64Target);
		}
	}
}
