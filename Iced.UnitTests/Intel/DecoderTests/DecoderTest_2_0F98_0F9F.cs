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
	public sealed class DecoderTest_2_0F98_0F9F : DecoderTest {
		[Theory]
		[InlineData("0F98 C0", 3, Code.Sets_Eb, Register.AL)]
		[InlineData("0F98 C9", 3, Code.Sets_Eb, Register.CL)]
		[InlineData("0F98 D2", 3, Code.Sets_Eb, Register.DL)]
		[InlineData("0F98 DB", 3, Code.Sets_Eb, Register.BL)]
		[InlineData("0F98 E4", 3, Code.Sets_Eb, Register.AH)]
		[InlineData("0F98 ED", 3, Code.Sets_Eb, Register.CH)]
		[InlineData("0F98 F6", 3, Code.Sets_Eb, Register.DH)]
		[InlineData("0F98 FF", 3, Code.Sets_Eb, Register.BH)]

		[InlineData("0F99 C0", 3, Code.Setns_Eb, Register.AL)]
		[InlineData("0F99 C9", 3, Code.Setns_Eb, Register.CL)]
		[InlineData("0F99 D2", 3, Code.Setns_Eb, Register.DL)]
		[InlineData("0F99 DB", 3, Code.Setns_Eb, Register.BL)]
		[InlineData("0F99 E4", 3, Code.Setns_Eb, Register.AH)]
		[InlineData("0F99 ED", 3, Code.Setns_Eb, Register.CH)]
		[InlineData("0F99 F6", 3, Code.Setns_Eb, Register.DH)]
		[InlineData("0F99 FF", 3, Code.Setns_Eb, Register.BH)]

		[InlineData("0F9A C0", 3, Code.Setp_Eb, Register.AL)]
		[InlineData("0F9A C9", 3, Code.Setp_Eb, Register.CL)]
		[InlineData("0F9A D2", 3, Code.Setp_Eb, Register.DL)]
		[InlineData("0F9A DB", 3, Code.Setp_Eb, Register.BL)]
		[InlineData("0F9A E4", 3, Code.Setp_Eb, Register.AH)]
		[InlineData("0F9A ED", 3, Code.Setp_Eb, Register.CH)]
		[InlineData("0F9A F6", 3, Code.Setp_Eb, Register.DH)]
		[InlineData("0F9A FF", 3, Code.Setp_Eb, Register.BH)]

		[InlineData("0F9B C0", 3, Code.Setnp_Eb, Register.AL)]
		[InlineData("0F9B C9", 3, Code.Setnp_Eb, Register.CL)]
		[InlineData("0F9B D2", 3, Code.Setnp_Eb, Register.DL)]
		[InlineData("0F9B DB", 3, Code.Setnp_Eb, Register.BL)]
		[InlineData("0F9B E4", 3, Code.Setnp_Eb, Register.AH)]
		[InlineData("0F9B ED", 3, Code.Setnp_Eb, Register.CH)]
		[InlineData("0F9B F6", 3, Code.Setnp_Eb, Register.DH)]
		[InlineData("0F9B FF", 3, Code.Setnp_Eb, Register.BH)]

		[InlineData("0F9C C0", 3, Code.Setl_Eb, Register.AL)]
		[InlineData("0F9C C9", 3, Code.Setl_Eb, Register.CL)]
		[InlineData("0F9C D2", 3, Code.Setl_Eb, Register.DL)]
		[InlineData("0F9C DB", 3, Code.Setl_Eb, Register.BL)]
		[InlineData("0F9C E4", 3, Code.Setl_Eb, Register.AH)]
		[InlineData("0F9C ED", 3, Code.Setl_Eb, Register.CH)]
		[InlineData("0F9C F6", 3, Code.Setl_Eb, Register.DH)]
		[InlineData("0F9C FF", 3, Code.Setl_Eb, Register.BH)]

		[InlineData("0F9D C0", 3, Code.Setge_Eb, Register.AL)]
		[InlineData("0F9D C9", 3, Code.Setge_Eb, Register.CL)]
		[InlineData("0F9D D2", 3, Code.Setge_Eb, Register.DL)]
		[InlineData("0F9D DB", 3, Code.Setge_Eb, Register.BL)]
		[InlineData("0F9D E4", 3, Code.Setge_Eb, Register.AH)]
		[InlineData("0F9D ED", 3, Code.Setge_Eb, Register.CH)]
		[InlineData("0F9D F6", 3, Code.Setge_Eb, Register.DH)]
		[InlineData("0F9D FF", 3, Code.Setge_Eb, Register.BH)]

		[InlineData("0F9E C0", 3, Code.Setle_Eb, Register.AL)]
		[InlineData("0F9E C9", 3, Code.Setle_Eb, Register.CL)]
		[InlineData("0F9E D2", 3, Code.Setle_Eb, Register.DL)]
		[InlineData("0F9E DB", 3, Code.Setle_Eb, Register.BL)]
		[InlineData("0F9E E4", 3, Code.Setle_Eb, Register.AH)]
		[InlineData("0F9E ED", 3, Code.Setle_Eb, Register.CH)]
		[InlineData("0F9E F6", 3, Code.Setle_Eb, Register.DH)]
		[InlineData("0F9E FF", 3, Code.Setle_Eb, Register.BH)]

		[InlineData("0F9F C0", 3, Code.Setg_Eb, Register.AL)]
		[InlineData("0F9F C9", 3, Code.Setg_Eb, Register.CL)]
		[InlineData("0F9F D2", 3, Code.Setg_Eb, Register.DL)]
		[InlineData("0F9F DB", 3, Code.Setg_Eb, Register.BL)]
		[InlineData("0F9F E4", 3, Code.Setg_Eb, Register.AH)]
		[InlineData("0F9F ED", 3, Code.Setg_Eb, Register.CH)]
		[InlineData("0F9F F6", 3, Code.Setg_Eb, Register.DH)]
		[InlineData("0F9F FF", 3, Code.Setg_Eb, Register.BH)]
		void Test16_Setcc_Eb_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F98 C0", 3, Code.Sets_Eb, Register.AL)]
		[InlineData("0F98 C9", 3, Code.Sets_Eb, Register.CL)]
		[InlineData("0F98 D2", 3, Code.Sets_Eb, Register.DL)]
		[InlineData("0F98 DB", 3, Code.Sets_Eb, Register.BL)]
		[InlineData("0F98 E4", 3, Code.Sets_Eb, Register.AH)]
		[InlineData("0F98 ED", 3, Code.Sets_Eb, Register.CH)]
		[InlineData("0F98 F6", 3, Code.Sets_Eb, Register.DH)]
		[InlineData("0F98 FF", 3, Code.Sets_Eb, Register.BH)]

		[InlineData("0F99 C0", 3, Code.Setns_Eb, Register.AL)]
		[InlineData("0F99 C9", 3, Code.Setns_Eb, Register.CL)]
		[InlineData("0F99 D2", 3, Code.Setns_Eb, Register.DL)]
		[InlineData("0F99 DB", 3, Code.Setns_Eb, Register.BL)]
		[InlineData("0F99 E4", 3, Code.Setns_Eb, Register.AH)]
		[InlineData("0F99 ED", 3, Code.Setns_Eb, Register.CH)]
		[InlineData("0F99 F6", 3, Code.Setns_Eb, Register.DH)]
		[InlineData("0F99 FF", 3, Code.Setns_Eb, Register.BH)]

		[InlineData("0F9A C0", 3, Code.Setp_Eb, Register.AL)]
		[InlineData("0F9A C9", 3, Code.Setp_Eb, Register.CL)]
		[InlineData("0F9A D2", 3, Code.Setp_Eb, Register.DL)]
		[InlineData("0F9A DB", 3, Code.Setp_Eb, Register.BL)]
		[InlineData("0F9A E4", 3, Code.Setp_Eb, Register.AH)]
		[InlineData("0F9A ED", 3, Code.Setp_Eb, Register.CH)]
		[InlineData("0F9A F6", 3, Code.Setp_Eb, Register.DH)]
		[InlineData("0F9A FF", 3, Code.Setp_Eb, Register.BH)]

		[InlineData("0F9B C0", 3, Code.Setnp_Eb, Register.AL)]
		[InlineData("0F9B C9", 3, Code.Setnp_Eb, Register.CL)]
		[InlineData("0F9B D2", 3, Code.Setnp_Eb, Register.DL)]
		[InlineData("0F9B DB", 3, Code.Setnp_Eb, Register.BL)]
		[InlineData("0F9B E4", 3, Code.Setnp_Eb, Register.AH)]
		[InlineData("0F9B ED", 3, Code.Setnp_Eb, Register.CH)]
		[InlineData("0F9B F6", 3, Code.Setnp_Eb, Register.DH)]
		[InlineData("0F9B FF", 3, Code.Setnp_Eb, Register.BH)]

		[InlineData("0F9C C0", 3, Code.Setl_Eb, Register.AL)]
		[InlineData("0F9C C9", 3, Code.Setl_Eb, Register.CL)]
		[InlineData("0F9C D2", 3, Code.Setl_Eb, Register.DL)]
		[InlineData("0F9C DB", 3, Code.Setl_Eb, Register.BL)]
		[InlineData("0F9C E4", 3, Code.Setl_Eb, Register.AH)]
		[InlineData("0F9C ED", 3, Code.Setl_Eb, Register.CH)]
		[InlineData("0F9C F6", 3, Code.Setl_Eb, Register.DH)]
		[InlineData("0F9C FF", 3, Code.Setl_Eb, Register.BH)]

		[InlineData("0F9D C0", 3, Code.Setge_Eb, Register.AL)]
		[InlineData("0F9D C9", 3, Code.Setge_Eb, Register.CL)]
		[InlineData("0F9D D2", 3, Code.Setge_Eb, Register.DL)]
		[InlineData("0F9D DB", 3, Code.Setge_Eb, Register.BL)]
		[InlineData("0F9D E4", 3, Code.Setge_Eb, Register.AH)]
		[InlineData("0F9D ED", 3, Code.Setge_Eb, Register.CH)]
		[InlineData("0F9D F6", 3, Code.Setge_Eb, Register.DH)]
		[InlineData("0F9D FF", 3, Code.Setge_Eb, Register.BH)]

		[InlineData("0F9E C0", 3, Code.Setle_Eb, Register.AL)]
		[InlineData("0F9E C9", 3, Code.Setle_Eb, Register.CL)]
		[InlineData("0F9E D2", 3, Code.Setle_Eb, Register.DL)]
		[InlineData("0F9E DB", 3, Code.Setle_Eb, Register.BL)]
		[InlineData("0F9E E4", 3, Code.Setle_Eb, Register.AH)]
		[InlineData("0F9E ED", 3, Code.Setle_Eb, Register.CH)]
		[InlineData("0F9E F6", 3, Code.Setle_Eb, Register.DH)]
		[InlineData("0F9E FF", 3, Code.Setle_Eb, Register.BH)]

		[InlineData("0F9F C0", 3, Code.Setg_Eb, Register.AL)]
		[InlineData("0F9F C9", 3, Code.Setg_Eb, Register.CL)]
		[InlineData("0F9F D2", 3, Code.Setg_Eb, Register.DL)]
		[InlineData("0F9F DB", 3, Code.Setg_Eb, Register.BL)]
		[InlineData("0F9F E4", 3, Code.Setg_Eb, Register.AH)]
		[InlineData("0F9F ED", 3, Code.Setg_Eb, Register.CH)]
		[InlineData("0F9F F6", 3, Code.Setg_Eb, Register.DH)]
		[InlineData("0F9F FF", 3, Code.Setg_Eb, Register.BH)]
		void Test32_Setcc_Eb_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F98 C0", 3, Code.Sets_Eb, Register.AL)]
		[InlineData("0F98 C9", 3, Code.Sets_Eb, Register.CL)]
		[InlineData("0F98 D2", 3, Code.Sets_Eb, Register.DL)]
		[InlineData("0F98 DB", 3, Code.Sets_Eb, Register.BL)]
		[InlineData("0F98 E4", 3, Code.Sets_Eb, Register.AH)]
		[InlineData("0F98 ED", 3, Code.Sets_Eb, Register.CH)]
		[InlineData("0F98 F6", 3, Code.Sets_Eb, Register.DH)]
		[InlineData("0F98 FF", 3, Code.Sets_Eb, Register.BH)]

		[InlineData("40 0F98 C0", 4, Code.Sets_Eb, Register.AL)]
		[InlineData("40 0F98 C9", 4, Code.Sets_Eb, Register.CL)]
		[InlineData("40 0F98 D2", 4, Code.Sets_Eb, Register.DL)]
		[InlineData("40 0F98 DB", 4, Code.Sets_Eb, Register.BL)]
		[InlineData("40 0F98 E4", 4, Code.Sets_Eb, Register.SPL)]
		[InlineData("40 0F98 ED", 4, Code.Sets_Eb, Register.BPL)]
		[InlineData("40 0F98 F6", 4, Code.Sets_Eb, Register.SIL)]
		[InlineData("40 0F98 FF", 4, Code.Sets_Eb, Register.DIL)]
		[InlineData("45 0F98 C0", 4, Code.Sets_Eb, Register.R8L)]
		[InlineData("45 0F98 C9", 4, Code.Sets_Eb, Register.R9L)]
		[InlineData("45 0F98 D2", 4, Code.Sets_Eb, Register.R10L)]
		[InlineData("45 0F98 DB", 4, Code.Sets_Eb, Register.R11L)]
		[InlineData("45 0F98 E4", 4, Code.Sets_Eb, Register.R12L)]
		[InlineData("45 0F98 ED", 4, Code.Sets_Eb, Register.R13L)]
		[InlineData("45 0F98 F6", 4, Code.Sets_Eb, Register.R14L)]
		[InlineData("45 0F98 FF", 4, Code.Sets_Eb, Register.R15L)]

		[InlineData("0F99 C0", 3, Code.Setns_Eb, Register.AL)]
		[InlineData("0F99 C9", 3, Code.Setns_Eb, Register.CL)]
		[InlineData("0F99 D2", 3, Code.Setns_Eb, Register.DL)]
		[InlineData("0F99 DB", 3, Code.Setns_Eb, Register.BL)]
		[InlineData("0F99 E4", 3, Code.Setns_Eb, Register.AH)]
		[InlineData("0F99 ED", 3, Code.Setns_Eb, Register.CH)]
		[InlineData("0F99 F6", 3, Code.Setns_Eb, Register.DH)]
		[InlineData("0F99 FF", 3, Code.Setns_Eb, Register.BH)]

		[InlineData("40 0F99 C0", 4, Code.Setns_Eb, Register.AL)]
		[InlineData("40 0F99 C9", 4, Code.Setns_Eb, Register.CL)]
		[InlineData("40 0F99 D2", 4, Code.Setns_Eb, Register.DL)]
		[InlineData("40 0F99 DB", 4, Code.Setns_Eb, Register.BL)]
		[InlineData("40 0F99 E4", 4, Code.Setns_Eb, Register.SPL)]
		[InlineData("40 0F99 ED", 4, Code.Setns_Eb, Register.BPL)]
		[InlineData("40 0F99 F6", 4, Code.Setns_Eb, Register.SIL)]
		[InlineData("40 0F99 FF", 4, Code.Setns_Eb, Register.DIL)]
		[InlineData("45 0F99 C0", 4, Code.Setns_Eb, Register.R8L)]
		[InlineData("45 0F99 C9", 4, Code.Setns_Eb, Register.R9L)]
		[InlineData("45 0F99 D2", 4, Code.Setns_Eb, Register.R10L)]
		[InlineData("45 0F99 DB", 4, Code.Setns_Eb, Register.R11L)]
		[InlineData("45 0F99 E4", 4, Code.Setns_Eb, Register.R12L)]
		[InlineData("45 0F99 ED", 4, Code.Setns_Eb, Register.R13L)]
		[InlineData("45 0F99 F6", 4, Code.Setns_Eb, Register.R14L)]
		[InlineData("45 0F99 FF", 4, Code.Setns_Eb, Register.R15L)]

		[InlineData("0F9A C0", 3, Code.Setp_Eb, Register.AL)]
		[InlineData("0F9A C9", 3, Code.Setp_Eb, Register.CL)]
		[InlineData("0F9A D2", 3, Code.Setp_Eb, Register.DL)]
		[InlineData("0F9A DB", 3, Code.Setp_Eb, Register.BL)]
		[InlineData("0F9A E4", 3, Code.Setp_Eb, Register.AH)]
		[InlineData("0F9A ED", 3, Code.Setp_Eb, Register.CH)]
		[InlineData("0F9A F6", 3, Code.Setp_Eb, Register.DH)]
		[InlineData("0F9A FF", 3, Code.Setp_Eb, Register.BH)]

		[InlineData("40 0F9A C0", 4, Code.Setp_Eb, Register.AL)]
		[InlineData("40 0F9A C9", 4, Code.Setp_Eb, Register.CL)]
		[InlineData("40 0F9A D2", 4, Code.Setp_Eb, Register.DL)]
		[InlineData("40 0F9A DB", 4, Code.Setp_Eb, Register.BL)]
		[InlineData("40 0F9A E4", 4, Code.Setp_Eb, Register.SPL)]
		[InlineData("40 0F9A ED", 4, Code.Setp_Eb, Register.BPL)]
		[InlineData("40 0F9A F6", 4, Code.Setp_Eb, Register.SIL)]
		[InlineData("40 0F9A FF", 4, Code.Setp_Eb, Register.DIL)]
		[InlineData("45 0F9A C0", 4, Code.Setp_Eb, Register.R8L)]
		[InlineData("45 0F9A C9", 4, Code.Setp_Eb, Register.R9L)]
		[InlineData("45 0F9A D2", 4, Code.Setp_Eb, Register.R10L)]
		[InlineData("45 0F9A DB", 4, Code.Setp_Eb, Register.R11L)]
		[InlineData("45 0F9A E4", 4, Code.Setp_Eb, Register.R12L)]
		[InlineData("45 0F9A ED", 4, Code.Setp_Eb, Register.R13L)]
		[InlineData("45 0F9A F6", 4, Code.Setp_Eb, Register.R14L)]
		[InlineData("45 0F9A FF", 4, Code.Setp_Eb, Register.R15L)]

		[InlineData("0F9B C0", 3, Code.Setnp_Eb, Register.AL)]
		[InlineData("0F9B C9", 3, Code.Setnp_Eb, Register.CL)]
		[InlineData("0F9B D2", 3, Code.Setnp_Eb, Register.DL)]
		[InlineData("0F9B DB", 3, Code.Setnp_Eb, Register.BL)]
		[InlineData("0F9B E4", 3, Code.Setnp_Eb, Register.AH)]
		[InlineData("0F9B ED", 3, Code.Setnp_Eb, Register.CH)]
		[InlineData("0F9B F6", 3, Code.Setnp_Eb, Register.DH)]
		[InlineData("0F9B FF", 3, Code.Setnp_Eb, Register.BH)]

		[InlineData("40 0F9B C0", 4, Code.Setnp_Eb, Register.AL)]
		[InlineData("40 0F9B C9", 4, Code.Setnp_Eb, Register.CL)]
		[InlineData("40 0F9B D2", 4, Code.Setnp_Eb, Register.DL)]
		[InlineData("40 0F9B DB", 4, Code.Setnp_Eb, Register.BL)]
		[InlineData("40 0F9B E4", 4, Code.Setnp_Eb, Register.SPL)]
		[InlineData("40 0F9B ED", 4, Code.Setnp_Eb, Register.BPL)]
		[InlineData("40 0F9B F6", 4, Code.Setnp_Eb, Register.SIL)]
		[InlineData("40 0F9B FF", 4, Code.Setnp_Eb, Register.DIL)]
		[InlineData("45 0F9B C0", 4, Code.Setnp_Eb, Register.R8L)]
		[InlineData("45 0F9B C9", 4, Code.Setnp_Eb, Register.R9L)]
		[InlineData("45 0F9B D2", 4, Code.Setnp_Eb, Register.R10L)]
		[InlineData("45 0F9B DB", 4, Code.Setnp_Eb, Register.R11L)]
		[InlineData("45 0F9B E4", 4, Code.Setnp_Eb, Register.R12L)]
		[InlineData("45 0F9B ED", 4, Code.Setnp_Eb, Register.R13L)]
		[InlineData("45 0F9B F6", 4, Code.Setnp_Eb, Register.R14L)]
		[InlineData("45 0F9B FF", 4, Code.Setnp_Eb, Register.R15L)]

		[InlineData("0F9C C0", 3, Code.Setl_Eb, Register.AL)]
		[InlineData("0F9C C9", 3, Code.Setl_Eb, Register.CL)]
		[InlineData("0F9C D2", 3, Code.Setl_Eb, Register.DL)]
		[InlineData("0F9C DB", 3, Code.Setl_Eb, Register.BL)]
		[InlineData("0F9C E4", 3, Code.Setl_Eb, Register.AH)]
		[InlineData("0F9C ED", 3, Code.Setl_Eb, Register.CH)]
		[InlineData("0F9C F6", 3, Code.Setl_Eb, Register.DH)]
		[InlineData("0F9C FF", 3, Code.Setl_Eb, Register.BH)]

		[InlineData("40 0F9C C0", 4, Code.Setl_Eb, Register.AL)]
		[InlineData("40 0F9C C9", 4, Code.Setl_Eb, Register.CL)]
		[InlineData("40 0F9C D2", 4, Code.Setl_Eb, Register.DL)]
		[InlineData("40 0F9C DB", 4, Code.Setl_Eb, Register.BL)]
		[InlineData("40 0F9C E4", 4, Code.Setl_Eb, Register.SPL)]
		[InlineData("40 0F9C ED", 4, Code.Setl_Eb, Register.BPL)]
		[InlineData("40 0F9C F6", 4, Code.Setl_Eb, Register.SIL)]
		[InlineData("40 0F9C FF", 4, Code.Setl_Eb, Register.DIL)]
		[InlineData("45 0F9C C0", 4, Code.Setl_Eb, Register.R8L)]
		[InlineData("45 0F9C C9", 4, Code.Setl_Eb, Register.R9L)]
		[InlineData("45 0F9C D2", 4, Code.Setl_Eb, Register.R10L)]
		[InlineData("45 0F9C DB", 4, Code.Setl_Eb, Register.R11L)]
		[InlineData("45 0F9C E4", 4, Code.Setl_Eb, Register.R12L)]
		[InlineData("45 0F9C ED", 4, Code.Setl_Eb, Register.R13L)]
		[InlineData("45 0F9C F6", 4, Code.Setl_Eb, Register.R14L)]
		[InlineData("45 0F9C FF", 4, Code.Setl_Eb, Register.R15L)]

		[InlineData("0F9D C0", 3, Code.Setge_Eb, Register.AL)]
		[InlineData("0F9D C9", 3, Code.Setge_Eb, Register.CL)]
		[InlineData("0F9D D2", 3, Code.Setge_Eb, Register.DL)]
		[InlineData("0F9D DB", 3, Code.Setge_Eb, Register.BL)]
		[InlineData("0F9D E4", 3, Code.Setge_Eb, Register.AH)]
		[InlineData("0F9D ED", 3, Code.Setge_Eb, Register.CH)]
		[InlineData("0F9D F6", 3, Code.Setge_Eb, Register.DH)]
		[InlineData("0F9D FF", 3, Code.Setge_Eb, Register.BH)]

		[InlineData("40 0F9D C0", 4, Code.Setge_Eb, Register.AL)]
		[InlineData("40 0F9D C9", 4, Code.Setge_Eb, Register.CL)]
		[InlineData("40 0F9D D2", 4, Code.Setge_Eb, Register.DL)]
		[InlineData("40 0F9D DB", 4, Code.Setge_Eb, Register.BL)]
		[InlineData("40 0F9D E4", 4, Code.Setge_Eb, Register.SPL)]
		[InlineData("40 0F9D ED", 4, Code.Setge_Eb, Register.BPL)]
		[InlineData("40 0F9D F6", 4, Code.Setge_Eb, Register.SIL)]
		[InlineData("40 0F9D FF", 4, Code.Setge_Eb, Register.DIL)]
		[InlineData("45 0F9D C0", 4, Code.Setge_Eb, Register.R8L)]
		[InlineData("45 0F9D C9", 4, Code.Setge_Eb, Register.R9L)]
		[InlineData("45 0F9D D2", 4, Code.Setge_Eb, Register.R10L)]
		[InlineData("45 0F9D DB", 4, Code.Setge_Eb, Register.R11L)]
		[InlineData("45 0F9D E4", 4, Code.Setge_Eb, Register.R12L)]
		[InlineData("45 0F9D ED", 4, Code.Setge_Eb, Register.R13L)]
		[InlineData("45 0F9D F6", 4, Code.Setge_Eb, Register.R14L)]
		[InlineData("45 0F9D FF", 4, Code.Setge_Eb, Register.R15L)]

		[InlineData("0F9E C0", 3, Code.Setle_Eb, Register.AL)]
		[InlineData("0F9E C9", 3, Code.Setle_Eb, Register.CL)]
		[InlineData("0F9E D2", 3, Code.Setle_Eb, Register.DL)]
		[InlineData("0F9E DB", 3, Code.Setle_Eb, Register.BL)]
		[InlineData("0F9E E4", 3, Code.Setle_Eb, Register.AH)]
		[InlineData("0F9E ED", 3, Code.Setle_Eb, Register.CH)]
		[InlineData("0F9E F6", 3, Code.Setle_Eb, Register.DH)]
		[InlineData("0F9E FF", 3, Code.Setle_Eb, Register.BH)]

		[InlineData("40 0F9E C0", 4, Code.Setle_Eb, Register.AL)]
		[InlineData("40 0F9E C9", 4, Code.Setle_Eb, Register.CL)]
		[InlineData("40 0F9E D2", 4, Code.Setle_Eb, Register.DL)]
		[InlineData("40 0F9E DB", 4, Code.Setle_Eb, Register.BL)]
		[InlineData("40 0F9E E4", 4, Code.Setle_Eb, Register.SPL)]
		[InlineData("40 0F9E ED", 4, Code.Setle_Eb, Register.BPL)]
		[InlineData("40 0F9E F6", 4, Code.Setle_Eb, Register.SIL)]
		[InlineData("40 0F9E FF", 4, Code.Setle_Eb, Register.DIL)]
		[InlineData("45 0F9E C0", 4, Code.Setle_Eb, Register.R8L)]
		[InlineData("45 0F9E C9", 4, Code.Setle_Eb, Register.R9L)]
		[InlineData("45 0F9E D2", 4, Code.Setle_Eb, Register.R10L)]
		[InlineData("45 0F9E DB", 4, Code.Setle_Eb, Register.R11L)]
		[InlineData("45 0F9E E4", 4, Code.Setle_Eb, Register.R12L)]
		[InlineData("45 0F9E ED", 4, Code.Setle_Eb, Register.R13L)]
		[InlineData("45 0F9E F6", 4, Code.Setle_Eb, Register.R14L)]
		[InlineData("45 0F9E FF", 4, Code.Setle_Eb, Register.R15L)]

		[InlineData("0F9F C0", 3, Code.Setg_Eb, Register.AL)]
		[InlineData("0F9F C9", 3, Code.Setg_Eb, Register.CL)]
		[InlineData("0F9F D2", 3, Code.Setg_Eb, Register.DL)]
		[InlineData("0F9F DB", 3, Code.Setg_Eb, Register.BL)]
		[InlineData("0F9F E4", 3, Code.Setg_Eb, Register.AH)]
		[InlineData("0F9F ED", 3, Code.Setg_Eb, Register.CH)]
		[InlineData("0F9F F6", 3, Code.Setg_Eb, Register.DH)]
		[InlineData("0F9F FF", 3, Code.Setg_Eb, Register.BH)]

		[InlineData("40 0F9F C0", 4, Code.Setg_Eb, Register.AL)]
		[InlineData("40 0F9F C9", 4, Code.Setg_Eb, Register.CL)]
		[InlineData("40 0F9F D2", 4, Code.Setg_Eb, Register.DL)]
		[InlineData("40 0F9F DB", 4, Code.Setg_Eb, Register.BL)]
		[InlineData("40 0F9F E4", 4, Code.Setg_Eb, Register.SPL)]
		[InlineData("40 0F9F ED", 4, Code.Setg_Eb, Register.BPL)]
		[InlineData("40 0F9F F6", 4, Code.Setg_Eb, Register.SIL)]
		[InlineData("40 0F9F FF", 4, Code.Setg_Eb, Register.DIL)]
		[InlineData("45 0F9F C0", 4, Code.Setg_Eb, Register.R8L)]
		[InlineData("45 0F9F C9", 4, Code.Setg_Eb, Register.R9L)]
		[InlineData("45 0F9F D2", 4, Code.Setg_Eb, Register.R10L)]
		[InlineData("45 0F9F DB", 4, Code.Setg_Eb, Register.R11L)]
		[InlineData("45 0F9F E4", 4, Code.Setg_Eb, Register.R12L)]
		[InlineData("45 0F9F ED", 4, Code.Setg_Eb, Register.R13L)]
		[InlineData("45 0F9F F6", 4, Code.Setg_Eb, Register.R14L)]
		[InlineData("45 0F9F FF", 4, Code.Setg_Eb, Register.R15L)]
		void Test64_Setcc_Eb_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[MemberData(nameof(Test16_Mask_VK_RK_1_Data))]
		void Test16_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 98 D3", 4, Code.VEX_Kortestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 98 D3", 4, Code.VEX_Kortestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 98 D3", 5, Code.VEX_Kortestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 98 D3", 5, Code.VEX_Kortestd_VK_RK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 99 D3", 4, Code.VEX_Ktestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 99 D3", 4, Code.VEX_Ktestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 99 D3", 5, Code.VEX_Ktestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 99 D3", 5, Code.VEX_Ktestd_VK_RK, Register.K2, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_RK_1_Data))]
		void Test32_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 98 D3", 4, Code.VEX_Kortestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 98 D3", 4, Code.VEX_Kortestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 98 D3", 5, Code.VEX_Kortestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 98 D3", 5, Code.VEX_Kortestd_VK_RK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 99 D3", 4, Code.VEX_Ktestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 99 D3", 4, Code.VEX_Ktestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 99 D3", 5, Code.VEX_Ktestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 99 D3", 5, Code.VEX_Ktestd_VK_RK, Register.K2, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_RK_1_Data))]
		void Test64_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 98 D3", 4, Code.VEX_Kortestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 98 D3", 4, Code.VEX_Kortestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 98 D3", 5, Code.VEX_Kortestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 98 D3", 5, Code.VEX_Kortestd_VK_RK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 99 D3", 4, Code.VEX_Ktestw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 99 D3", 4, Code.VEX_Ktestb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 99 D3", 5, Code.VEX_Ktestq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 99 D3", 5, Code.VEX_Ktestd_VK_RK, Register.K2, Register.K3 };
			}
		}
	}
}
