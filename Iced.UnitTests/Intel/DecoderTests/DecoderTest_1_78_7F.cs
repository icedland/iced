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
	public sealed class DecoderTest_1_78_7F : DecoderTest {
		[Theory]
		[InlineData("78 5A", 2, Code.Js_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("78 A5", 2, Code.Js_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("79 5A", 2, Code.Jns_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("79 A5", 2, Code.Jns_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7A 5A", 2, Code.Jp_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7A A5", 2, Code.Jp_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7B 5A", 2, Code.Jnp_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7B A5", 2, Code.Jnp_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7C 5A", 2, Code.Jl_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7C A5", 2, Code.Jl_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7D 5A", 2, Code.Jge_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7D A5", 2, Code.Jge_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7E 5A", 2, Code.Jle_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7E A5", 2, Code.Jle_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("7F 5A", 2, Code.Jg_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("7F A5", 2, Code.Jg_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
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
		[InlineData("66 78 5A", 3, Code.Js_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 78 A5", 3, Code.Js_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 79 5A", 3, Code.Jns_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 79 A5", 3, Code.Jns_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7A 5A", 3, Code.Jp_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7A A5", 3, Code.Jp_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7B 5A", 3, Code.Jnp_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7B A5", 3, Code.Jnp_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7C 5A", 3, Code.Jl_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7C A5", 3, Code.Jl_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7D 5A", 3, Code.Jge_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7D A5", 3, Code.Jge_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7E 5A", 3, Code.Jle_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7E A5", 3, Code.Jle_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		[InlineData("66 7F 5A", 3, Code.Jg_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 7F A5", 3, Code.Jg_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
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
		[InlineData("66 78 5A", 3, Code.Js_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 78 A5", 3, Code.Js_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 79 5A", 3, Code.Jns_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 79 A5", 3, Code.Jns_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7A 5A", 3, Code.Jp_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7A A5", 3, Code.Jp_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7B 5A", 3, Code.Jnp_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7B A5", 3, Code.Jnp_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7C 5A", 3, Code.Jl_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7C A5", 3, Code.Jl_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7D 5A", 3, Code.Jge_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7D A5", 3, Code.Jge_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7E 5A", 3, Code.Jle_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7E A5", 3, Code.Jle_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		[InlineData("66 7F 5A", 3, Code.Jg_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 7F A5", 3, Code.Jg_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
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
		[InlineData("78 5A", 2, Code.Js_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("78 A5", 2, Code.Js_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("79 5A", 2, Code.Jns_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("79 A5", 2, Code.Jns_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7A 5A", 2, Code.Jp_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7A A5", 2, Code.Jp_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7B 5A", 2, Code.Jnp_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7B A5", 2, Code.Jnp_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7C 5A", 2, Code.Jl_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7C A5", 2, Code.Jl_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7D 5A", 2, Code.Jge_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7D A5", 2, Code.Jge_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7E 5A", 2, Code.Jle_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7E A5", 2, Code.Jle_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("7F 5A", 2, Code.Jg_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("7F A5", 2, Code.Jg_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
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
		[InlineData("78 5A", 2, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("78 A5", 2, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("79 5A", 2, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("79 A5", 2, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7A 5A", 2, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7A A5", 2, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7B 5A", 2, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7B A5", 2, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7C 5A", 2, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7C A5", 2, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7D 5A", 2, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7D A5", 2, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7E 5A", 2, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7E A5", 2, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("7F 5A", 2, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("7F A5", 2, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("66 78 5A", 3, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 78 A5", 3, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 79 5A", 3, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 79 A5", 3, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7A 5A", 3, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7A A5", 3, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7B 5A", 3, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7B A5", 3, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7C 5A", 3, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7C A5", 3, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7D 5A", 3, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7D A5", 3, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7E 5A", 3, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7E A5", 3, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 7F 5A", 3, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 7F A5", 3, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 4F 78 5A", 4, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 78 A5", 4, Code.Js_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 79 5A", 4, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 79 A5", 4, Code.Jns_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7A 5A", 4, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7A A5", 4, Code.Jp_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7B 5A", 4, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7B A5", 4, Code.Jnp_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7C 5A", 4, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7C A5", 4, Code.Jl_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7D 5A", 4, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7D A5", 4, Code.Jge_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7E 5A", 4, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7E A5", 4, Code.Jle_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		[InlineData("66 4F 7F 5A", 4, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F 7F A5", 4, Code.Jg_rel8_64, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
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
