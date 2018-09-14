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
	public sealed class DecoderTest_2_0F08_0F0F : DecoderTest {
		[Theory]
		[InlineData("0F08", 2, Code.Invd)]
		[InlineData("0F09", 2, Code.Wbinvd)]
		[InlineData("0F0B", 2, Code.Ud2)]
		[InlineData("0F0E", 2, Code.Femms)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0F08", 2, Code.Invd)]
		[InlineData("0F09", 2, Code.Wbinvd)]
		[InlineData("0F0B", 2, Code.Ud2)]
		[InlineData("0F0E", 2, Code.Femms)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0F08", 2, Code.Invd)]
		[InlineData("0F09", 2, Code.Wbinvd)]
		[InlineData("0F0B", 2, Code.Ud2)]
		[InlineData("0F0E", 2, Code.Femms)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_Mb)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_Mb)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_Mb)]
		[InlineData("0F0D 18", 3, Code.Prefetch_Mb_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_Mb_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_Mb_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_Mb_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_Mb_r7)]
		void Test16_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_Mb)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_Mb)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_Mb)]
		[InlineData("0F0D 18", 3, Code.Prefetch_Mb_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_Mb_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_Mb_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_Mb_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_Mb_r7)]
		void Test32_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F0D 00", 3, Code.Prefetch_Mb)]
		[InlineData("0F0D 08", 3, Code.Prefetchw_Mb)]
		[InlineData("0F0D 10", 3, Code.Prefetchwt1_Mb)]
		[InlineData("0F0D 18", 3, Code.Prefetch_Mb_r3)]
		[InlineData("0F0D 20", 3, Code.Prefetch_Mb_r4)]
		[InlineData("0F0D 28", 3, Code.Prefetch_Mb_r5)]
		[InlineData("0F0D 30", 3, Code.Prefetch_Mb_r6)]
		[InlineData("0F0D 38", 3, Code.Prefetch_Mb_r7)]
		void Test64_PrefetchX_Mb_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
	}
}
