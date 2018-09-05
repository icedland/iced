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
	public sealed class DecoderTest_2_0F90_0F97 : DecoderTest {
		[Theory]
		[InlineData("0F90 C0", 3, Code.Seto_Eb, Register.AL)]
		[InlineData("0F90 C1", 3, Code.Seto_Eb, Register.CL)]
		[InlineData("0F90 D2", 3, Code.Seto_Eb, Register.DL)]
		[InlineData("0F90 DB", 3, Code.Seto_Eb, Register.BL)]
		[InlineData("0F90 E4", 3, Code.Seto_Eb, Register.AH)]
		[InlineData("0F90 ED", 3, Code.Seto_Eb, Register.CH)]
		[InlineData("0F90 F6", 3, Code.Seto_Eb, Register.DH)]
		[InlineData("0F90 FF", 3, Code.Seto_Eb, Register.BH)]

		[InlineData("0F91 C0", 3, Code.Setno_Eb, Register.AL)]
		[InlineData("0F91 C1", 3, Code.Setno_Eb, Register.CL)]
		[InlineData("0F91 D2", 3, Code.Setno_Eb, Register.DL)]
		[InlineData("0F91 DB", 3, Code.Setno_Eb, Register.BL)]
		[InlineData("0F91 E4", 3, Code.Setno_Eb, Register.AH)]
		[InlineData("0F91 ED", 3, Code.Setno_Eb, Register.CH)]
		[InlineData("0F91 F6", 3, Code.Setno_Eb, Register.DH)]
		[InlineData("0F91 FF", 3, Code.Setno_Eb, Register.BH)]

		[InlineData("0F92 C0", 3, Code.Setb_Eb, Register.AL)]
		[InlineData("0F92 C1", 3, Code.Setb_Eb, Register.CL)]
		[InlineData("0F92 D2", 3, Code.Setb_Eb, Register.DL)]
		[InlineData("0F92 DB", 3, Code.Setb_Eb, Register.BL)]
		[InlineData("0F92 E4", 3, Code.Setb_Eb, Register.AH)]
		[InlineData("0F92 ED", 3, Code.Setb_Eb, Register.CH)]
		[InlineData("0F92 F6", 3, Code.Setb_Eb, Register.DH)]
		[InlineData("0F92 FF", 3, Code.Setb_Eb, Register.BH)]

		[InlineData("0F93 C0", 3, Code.Setae_Eb, Register.AL)]
		[InlineData("0F93 C1", 3, Code.Setae_Eb, Register.CL)]
		[InlineData("0F93 D2", 3, Code.Setae_Eb, Register.DL)]
		[InlineData("0F93 DB", 3, Code.Setae_Eb, Register.BL)]
		[InlineData("0F93 E4", 3, Code.Setae_Eb, Register.AH)]
		[InlineData("0F93 ED", 3, Code.Setae_Eb, Register.CH)]
		[InlineData("0F93 F6", 3, Code.Setae_Eb, Register.DH)]
		[InlineData("0F93 FF", 3, Code.Setae_Eb, Register.BH)]

		[InlineData("0F94 C0", 3, Code.Sete_Eb, Register.AL)]
		[InlineData("0F94 C1", 3, Code.Sete_Eb, Register.CL)]
		[InlineData("0F94 D2", 3, Code.Sete_Eb, Register.DL)]
		[InlineData("0F94 DB", 3, Code.Sete_Eb, Register.BL)]
		[InlineData("0F94 E4", 3, Code.Sete_Eb, Register.AH)]
		[InlineData("0F94 ED", 3, Code.Sete_Eb, Register.CH)]
		[InlineData("0F94 F6", 3, Code.Sete_Eb, Register.DH)]
		[InlineData("0F94 FF", 3, Code.Sete_Eb, Register.BH)]

		[InlineData("0F95 C0", 3, Code.Setne_Eb, Register.AL)]
		[InlineData("0F95 C1", 3, Code.Setne_Eb, Register.CL)]
		[InlineData("0F95 D2", 3, Code.Setne_Eb, Register.DL)]
		[InlineData("0F95 DB", 3, Code.Setne_Eb, Register.BL)]
		[InlineData("0F95 E4", 3, Code.Setne_Eb, Register.AH)]
		[InlineData("0F95 ED", 3, Code.Setne_Eb, Register.CH)]
		[InlineData("0F95 F6", 3, Code.Setne_Eb, Register.DH)]
		[InlineData("0F95 FF", 3, Code.Setne_Eb, Register.BH)]

		[InlineData("0F96 C0", 3, Code.Setbe_Eb, Register.AL)]
		[InlineData("0F96 C1", 3, Code.Setbe_Eb, Register.CL)]
		[InlineData("0F96 D2", 3, Code.Setbe_Eb, Register.DL)]
		[InlineData("0F96 DB", 3, Code.Setbe_Eb, Register.BL)]
		[InlineData("0F96 E4", 3, Code.Setbe_Eb, Register.AH)]
		[InlineData("0F96 ED", 3, Code.Setbe_Eb, Register.CH)]
		[InlineData("0F96 F6", 3, Code.Setbe_Eb, Register.DH)]
		[InlineData("0F96 FF", 3, Code.Setbe_Eb, Register.BH)]

		[InlineData("0F97 C0", 3, Code.Seta_Eb, Register.AL)]
		[InlineData("0F97 C1", 3, Code.Seta_Eb, Register.CL)]
		[InlineData("0F97 D2", 3, Code.Seta_Eb, Register.DL)]
		[InlineData("0F97 DB", 3, Code.Seta_Eb, Register.BL)]
		[InlineData("0F97 E4", 3, Code.Seta_Eb, Register.AH)]
		[InlineData("0F97 ED", 3, Code.Seta_Eb, Register.CH)]
		[InlineData("0F97 F6", 3, Code.Seta_Eb, Register.DH)]
		[InlineData("0F97 FF", 3, Code.Seta_Eb, Register.BH)]
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
		[InlineData("0F90 C0", 3, Code.Seto_Eb, Register.AL)]
		[InlineData("0F90 C1", 3, Code.Seto_Eb, Register.CL)]
		[InlineData("0F90 D2", 3, Code.Seto_Eb, Register.DL)]
		[InlineData("0F90 DB", 3, Code.Seto_Eb, Register.BL)]
		[InlineData("0F90 E4", 3, Code.Seto_Eb, Register.AH)]
		[InlineData("0F90 ED", 3, Code.Seto_Eb, Register.CH)]
		[InlineData("0F90 F6", 3, Code.Seto_Eb, Register.DH)]
		[InlineData("0F90 FF", 3, Code.Seto_Eb, Register.BH)]

		[InlineData("0F91 C0", 3, Code.Setno_Eb, Register.AL)]
		[InlineData("0F91 C1", 3, Code.Setno_Eb, Register.CL)]
		[InlineData("0F91 D2", 3, Code.Setno_Eb, Register.DL)]
		[InlineData("0F91 DB", 3, Code.Setno_Eb, Register.BL)]
		[InlineData("0F91 E4", 3, Code.Setno_Eb, Register.AH)]
		[InlineData("0F91 ED", 3, Code.Setno_Eb, Register.CH)]
		[InlineData("0F91 F6", 3, Code.Setno_Eb, Register.DH)]
		[InlineData("0F91 FF", 3, Code.Setno_Eb, Register.BH)]

		[InlineData("0F92 C0", 3, Code.Setb_Eb, Register.AL)]
		[InlineData("0F92 C1", 3, Code.Setb_Eb, Register.CL)]
		[InlineData("0F92 D2", 3, Code.Setb_Eb, Register.DL)]
		[InlineData("0F92 DB", 3, Code.Setb_Eb, Register.BL)]
		[InlineData("0F92 E4", 3, Code.Setb_Eb, Register.AH)]
		[InlineData("0F92 ED", 3, Code.Setb_Eb, Register.CH)]
		[InlineData("0F92 F6", 3, Code.Setb_Eb, Register.DH)]
		[InlineData("0F92 FF", 3, Code.Setb_Eb, Register.BH)]

		[InlineData("0F93 C0", 3, Code.Setae_Eb, Register.AL)]
		[InlineData("0F93 C1", 3, Code.Setae_Eb, Register.CL)]
		[InlineData("0F93 D2", 3, Code.Setae_Eb, Register.DL)]
		[InlineData("0F93 DB", 3, Code.Setae_Eb, Register.BL)]
		[InlineData("0F93 E4", 3, Code.Setae_Eb, Register.AH)]
		[InlineData("0F93 ED", 3, Code.Setae_Eb, Register.CH)]
		[InlineData("0F93 F6", 3, Code.Setae_Eb, Register.DH)]
		[InlineData("0F93 FF", 3, Code.Setae_Eb, Register.BH)]

		[InlineData("0F94 C0", 3, Code.Sete_Eb, Register.AL)]
		[InlineData("0F94 C1", 3, Code.Sete_Eb, Register.CL)]
		[InlineData("0F94 D2", 3, Code.Sete_Eb, Register.DL)]
		[InlineData("0F94 DB", 3, Code.Sete_Eb, Register.BL)]
		[InlineData("0F94 E4", 3, Code.Sete_Eb, Register.AH)]
		[InlineData("0F94 ED", 3, Code.Sete_Eb, Register.CH)]
		[InlineData("0F94 F6", 3, Code.Sete_Eb, Register.DH)]
		[InlineData("0F94 FF", 3, Code.Sete_Eb, Register.BH)]

		[InlineData("0F95 C0", 3, Code.Setne_Eb, Register.AL)]
		[InlineData("0F95 C1", 3, Code.Setne_Eb, Register.CL)]
		[InlineData("0F95 D2", 3, Code.Setne_Eb, Register.DL)]
		[InlineData("0F95 DB", 3, Code.Setne_Eb, Register.BL)]
		[InlineData("0F95 E4", 3, Code.Setne_Eb, Register.AH)]
		[InlineData("0F95 ED", 3, Code.Setne_Eb, Register.CH)]
		[InlineData("0F95 F6", 3, Code.Setne_Eb, Register.DH)]
		[InlineData("0F95 FF", 3, Code.Setne_Eb, Register.BH)]

		[InlineData("0F96 C0", 3, Code.Setbe_Eb, Register.AL)]
		[InlineData("0F96 C1", 3, Code.Setbe_Eb, Register.CL)]
		[InlineData("0F96 D2", 3, Code.Setbe_Eb, Register.DL)]
		[InlineData("0F96 DB", 3, Code.Setbe_Eb, Register.BL)]
		[InlineData("0F96 E4", 3, Code.Setbe_Eb, Register.AH)]
		[InlineData("0F96 ED", 3, Code.Setbe_Eb, Register.CH)]
		[InlineData("0F96 F6", 3, Code.Setbe_Eb, Register.DH)]
		[InlineData("0F96 FF", 3, Code.Setbe_Eb, Register.BH)]

		[InlineData("0F97 C0", 3, Code.Seta_Eb, Register.AL)]
		[InlineData("0F97 C1", 3, Code.Seta_Eb, Register.CL)]
		[InlineData("0F97 D2", 3, Code.Seta_Eb, Register.DL)]
		[InlineData("0F97 DB", 3, Code.Seta_Eb, Register.BL)]
		[InlineData("0F97 E4", 3, Code.Seta_Eb, Register.AH)]
		[InlineData("0F97 ED", 3, Code.Seta_Eb, Register.CH)]
		[InlineData("0F97 F6", 3, Code.Seta_Eb, Register.DH)]
		[InlineData("0F97 FF", 3, Code.Seta_Eb, Register.BH)]
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
		[InlineData("0F90 C0", 3, Code.Seto_Eb, Register.AL)]
		[InlineData("0F90 C1", 3, Code.Seto_Eb, Register.CL)]
		[InlineData("0F90 D2", 3, Code.Seto_Eb, Register.DL)]
		[InlineData("0F90 DB", 3, Code.Seto_Eb, Register.BL)]
		[InlineData("0F90 E4", 3, Code.Seto_Eb, Register.AH)]
		[InlineData("0F90 ED", 3, Code.Seto_Eb, Register.CH)]
		[InlineData("0F90 F6", 3, Code.Seto_Eb, Register.DH)]
		[InlineData("0F90 FF", 3, Code.Seto_Eb, Register.BH)]

		[InlineData("40 0F90 C0", 4, Code.Seto_Eb, Register.AL)]
		[InlineData("40 0F90 C1", 4, Code.Seto_Eb, Register.CL)]
		[InlineData("40 0F90 D2", 4, Code.Seto_Eb, Register.DL)]
		[InlineData("40 0F90 DB", 4, Code.Seto_Eb, Register.BL)]
		[InlineData("40 0F90 E4", 4, Code.Seto_Eb, Register.SPL)]
		[InlineData("40 0F90 ED", 4, Code.Seto_Eb, Register.BPL)]
		[InlineData("40 0F90 F6", 4, Code.Seto_Eb, Register.SIL)]
		[InlineData("40 0F90 FF", 4, Code.Seto_Eb, Register.DIL)]
		[InlineData("45 0F90 C0", 4, Code.Seto_Eb, Register.R8L)]
		[InlineData("45 0F90 C1", 4, Code.Seto_Eb, Register.R9L)]
		[InlineData("45 0F90 D2", 4, Code.Seto_Eb, Register.R10L)]
		[InlineData("45 0F90 DB", 4, Code.Seto_Eb, Register.R11L)]
		[InlineData("45 0F90 E4", 4, Code.Seto_Eb, Register.R12L)]
		[InlineData("45 0F90 ED", 4, Code.Seto_Eb, Register.R13L)]
		[InlineData("45 0F90 F6", 4, Code.Seto_Eb, Register.R14L)]
		[InlineData("45 0F90 FF", 4, Code.Seto_Eb, Register.R15L)]

		[InlineData("0F91 C0", 3, Code.Setno_Eb, Register.AL)]
		[InlineData("0F91 C1", 3, Code.Setno_Eb, Register.CL)]
		[InlineData("0F91 D2", 3, Code.Setno_Eb, Register.DL)]
		[InlineData("0F91 DB", 3, Code.Setno_Eb, Register.BL)]
		[InlineData("0F91 E4", 3, Code.Setno_Eb, Register.AH)]
		[InlineData("0F91 ED", 3, Code.Setno_Eb, Register.CH)]
		[InlineData("0F91 F6", 3, Code.Setno_Eb, Register.DH)]
		[InlineData("0F91 FF", 3, Code.Setno_Eb, Register.BH)]

		[InlineData("40 0F91 C0", 4, Code.Setno_Eb, Register.AL)]
		[InlineData("40 0F91 C1", 4, Code.Setno_Eb, Register.CL)]
		[InlineData("40 0F91 D2", 4, Code.Setno_Eb, Register.DL)]
		[InlineData("40 0F91 DB", 4, Code.Setno_Eb, Register.BL)]
		[InlineData("40 0F91 E4", 4, Code.Setno_Eb, Register.SPL)]
		[InlineData("40 0F91 ED", 4, Code.Setno_Eb, Register.BPL)]
		[InlineData("40 0F91 F6", 4, Code.Setno_Eb, Register.SIL)]
		[InlineData("40 0F91 FF", 4, Code.Setno_Eb, Register.DIL)]
		[InlineData("45 0F91 C0", 4, Code.Setno_Eb, Register.R8L)]
		[InlineData("45 0F91 C1", 4, Code.Setno_Eb, Register.R9L)]
		[InlineData("45 0F91 D2", 4, Code.Setno_Eb, Register.R10L)]
		[InlineData("45 0F91 DB", 4, Code.Setno_Eb, Register.R11L)]
		[InlineData("45 0F91 E4", 4, Code.Setno_Eb, Register.R12L)]
		[InlineData("45 0F91 ED", 4, Code.Setno_Eb, Register.R13L)]
		[InlineData("45 0F91 F6", 4, Code.Setno_Eb, Register.R14L)]
		[InlineData("45 0F91 FF", 4, Code.Setno_Eb, Register.R15L)]

		[InlineData("0F92 C0", 3, Code.Setb_Eb, Register.AL)]
		[InlineData("0F92 C1", 3, Code.Setb_Eb, Register.CL)]
		[InlineData("0F92 D2", 3, Code.Setb_Eb, Register.DL)]
		[InlineData("0F92 DB", 3, Code.Setb_Eb, Register.BL)]
		[InlineData("0F92 E4", 3, Code.Setb_Eb, Register.AH)]
		[InlineData("0F92 ED", 3, Code.Setb_Eb, Register.CH)]
		[InlineData("0F92 F6", 3, Code.Setb_Eb, Register.DH)]
		[InlineData("0F92 FF", 3, Code.Setb_Eb, Register.BH)]

		[InlineData("40 0F92 C0", 4, Code.Setb_Eb, Register.AL)]
		[InlineData("40 0F92 C1", 4, Code.Setb_Eb, Register.CL)]
		[InlineData("40 0F92 D2", 4, Code.Setb_Eb, Register.DL)]
		[InlineData("40 0F92 DB", 4, Code.Setb_Eb, Register.BL)]
		[InlineData("40 0F92 E4", 4, Code.Setb_Eb, Register.SPL)]
		[InlineData("40 0F92 ED", 4, Code.Setb_Eb, Register.BPL)]
		[InlineData("40 0F92 F6", 4, Code.Setb_Eb, Register.SIL)]
		[InlineData("40 0F92 FF", 4, Code.Setb_Eb, Register.DIL)]
		[InlineData("45 0F92 C0", 4, Code.Setb_Eb, Register.R8L)]
		[InlineData("45 0F92 C1", 4, Code.Setb_Eb, Register.R9L)]
		[InlineData("45 0F92 D2", 4, Code.Setb_Eb, Register.R10L)]
		[InlineData("45 0F92 DB", 4, Code.Setb_Eb, Register.R11L)]
		[InlineData("45 0F92 E4", 4, Code.Setb_Eb, Register.R12L)]
		[InlineData("45 0F92 ED", 4, Code.Setb_Eb, Register.R13L)]
		[InlineData("45 0F92 F6", 4, Code.Setb_Eb, Register.R14L)]
		[InlineData("45 0F92 FF", 4, Code.Setb_Eb, Register.R15L)]

		[InlineData("0F93 C0", 3, Code.Setae_Eb, Register.AL)]
		[InlineData("0F93 C1", 3, Code.Setae_Eb, Register.CL)]
		[InlineData("0F93 D2", 3, Code.Setae_Eb, Register.DL)]
		[InlineData("0F93 DB", 3, Code.Setae_Eb, Register.BL)]
		[InlineData("0F93 E4", 3, Code.Setae_Eb, Register.AH)]
		[InlineData("0F93 ED", 3, Code.Setae_Eb, Register.CH)]
		[InlineData("0F93 F6", 3, Code.Setae_Eb, Register.DH)]
		[InlineData("0F93 FF", 3, Code.Setae_Eb, Register.BH)]

		[InlineData("40 0F93 C0", 4, Code.Setae_Eb, Register.AL)]
		[InlineData("40 0F93 C1", 4, Code.Setae_Eb, Register.CL)]
		[InlineData("40 0F93 D2", 4, Code.Setae_Eb, Register.DL)]
		[InlineData("40 0F93 DB", 4, Code.Setae_Eb, Register.BL)]
		[InlineData("40 0F93 E4", 4, Code.Setae_Eb, Register.SPL)]
		[InlineData("40 0F93 ED", 4, Code.Setae_Eb, Register.BPL)]
		[InlineData("40 0F93 F6", 4, Code.Setae_Eb, Register.SIL)]
		[InlineData("40 0F93 FF", 4, Code.Setae_Eb, Register.DIL)]
		[InlineData("45 0F93 C0", 4, Code.Setae_Eb, Register.R8L)]
		[InlineData("45 0F93 C1", 4, Code.Setae_Eb, Register.R9L)]
		[InlineData("45 0F93 D2", 4, Code.Setae_Eb, Register.R10L)]
		[InlineData("45 0F93 DB", 4, Code.Setae_Eb, Register.R11L)]
		[InlineData("45 0F93 E4", 4, Code.Setae_Eb, Register.R12L)]
		[InlineData("45 0F93 ED", 4, Code.Setae_Eb, Register.R13L)]
		[InlineData("45 0F93 F6", 4, Code.Setae_Eb, Register.R14L)]
		[InlineData("45 0F93 FF", 4, Code.Setae_Eb, Register.R15L)]

		[InlineData("0F94 C0", 3, Code.Sete_Eb, Register.AL)]
		[InlineData("0F94 C1", 3, Code.Sete_Eb, Register.CL)]
		[InlineData("0F94 D2", 3, Code.Sete_Eb, Register.DL)]
		[InlineData("0F94 DB", 3, Code.Sete_Eb, Register.BL)]
		[InlineData("0F94 E4", 3, Code.Sete_Eb, Register.AH)]
		[InlineData("0F94 ED", 3, Code.Sete_Eb, Register.CH)]
		[InlineData("0F94 F6", 3, Code.Sete_Eb, Register.DH)]
		[InlineData("0F94 FF", 3, Code.Sete_Eb, Register.BH)]

		[InlineData("40 0F94 C0", 4, Code.Sete_Eb, Register.AL)]
		[InlineData("40 0F94 C1", 4, Code.Sete_Eb, Register.CL)]
		[InlineData("40 0F94 D2", 4, Code.Sete_Eb, Register.DL)]
		[InlineData("40 0F94 DB", 4, Code.Sete_Eb, Register.BL)]
		[InlineData("40 0F94 E4", 4, Code.Sete_Eb, Register.SPL)]
		[InlineData("40 0F94 ED", 4, Code.Sete_Eb, Register.BPL)]
		[InlineData("40 0F94 F6", 4, Code.Sete_Eb, Register.SIL)]
		[InlineData("40 0F94 FF", 4, Code.Sete_Eb, Register.DIL)]
		[InlineData("45 0F94 C0", 4, Code.Sete_Eb, Register.R8L)]
		[InlineData("45 0F94 C1", 4, Code.Sete_Eb, Register.R9L)]
		[InlineData("45 0F94 D2", 4, Code.Sete_Eb, Register.R10L)]
		[InlineData("45 0F94 DB", 4, Code.Sete_Eb, Register.R11L)]
		[InlineData("45 0F94 E4", 4, Code.Sete_Eb, Register.R12L)]
		[InlineData("45 0F94 ED", 4, Code.Sete_Eb, Register.R13L)]
		[InlineData("45 0F94 F6", 4, Code.Sete_Eb, Register.R14L)]
		[InlineData("45 0F94 FF", 4, Code.Sete_Eb, Register.R15L)]

		[InlineData("0F95 C0", 3, Code.Setne_Eb, Register.AL)]
		[InlineData("0F95 C1", 3, Code.Setne_Eb, Register.CL)]
		[InlineData("0F95 D2", 3, Code.Setne_Eb, Register.DL)]
		[InlineData("0F95 DB", 3, Code.Setne_Eb, Register.BL)]
		[InlineData("0F95 E4", 3, Code.Setne_Eb, Register.AH)]
		[InlineData("0F95 ED", 3, Code.Setne_Eb, Register.CH)]
		[InlineData("0F95 F6", 3, Code.Setne_Eb, Register.DH)]
		[InlineData("0F95 FF", 3, Code.Setne_Eb, Register.BH)]

		[InlineData("40 0F95 C0", 4, Code.Setne_Eb, Register.AL)]
		[InlineData("40 0F95 C1", 4, Code.Setne_Eb, Register.CL)]
		[InlineData("40 0F95 D2", 4, Code.Setne_Eb, Register.DL)]
		[InlineData("40 0F95 DB", 4, Code.Setne_Eb, Register.BL)]
		[InlineData("40 0F95 E4", 4, Code.Setne_Eb, Register.SPL)]
		[InlineData("40 0F95 ED", 4, Code.Setne_Eb, Register.BPL)]
		[InlineData("40 0F95 F6", 4, Code.Setne_Eb, Register.SIL)]
		[InlineData("40 0F95 FF", 4, Code.Setne_Eb, Register.DIL)]
		[InlineData("45 0F95 C0", 4, Code.Setne_Eb, Register.R8L)]
		[InlineData("45 0F95 C1", 4, Code.Setne_Eb, Register.R9L)]
		[InlineData("45 0F95 D2", 4, Code.Setne_Eb, Register.R10L)]
		[InlineData("45 0F95 DB", 4, Code.Setne_Eb, Register.R11L)]
		[InlineData("45 0F95 E4", 4, Code.Setne_Eb, Register.R12L)]
		[InlineData("45 0F95 ED", 4, Code.Setne_Eb, Register.R13L)]
		[InlineData("45 0F95 F6", 4, Code.Setne_Eb, Register.R14L)]
		[InlineData("45 0F95 FF", 4, Code.Setne_Eb, Register.R15L)]

		[InlineData("0F96 C0", 3, Code.Setbe_Eb, Register.AL)]
		[InlineData("0F96 C1", 3, Code.Setbe_Eb, Register.CL)]
		[InlineData("0F96 D2", 3, Code.Setbe_Eb, Register.DL)]
		[InlineData("0F96 DB", 3, Code.Setbe_Eb, Register.BL)]
		[InlineData("0F96 E4", 3, Code.Setbe_Eb, Register.AH)]
		[InlineData("0F96 ED", 3, Code.Setbe_Eb, Register.CH)]
		[InlineData("0F96 F6", 3, Code.Setbe_Eb, Register.DH)]
		[InlineData("0F96 FF", 3, Code.Setbe_Eb, Register.BH)]

		[InlineData("40 0F96 C0", 4, Code.Setbe_Eb, Register.AL)]
		[InlineData("40 0F96 C1", 4, Code.Setbe_Eb, Register.CL)]
		[InlineData("40 0F96 D2", 4, Code.Setbe_Eb, Register.DL)]
		[InlineData("40 0F96 DB", 4, Code.Setbe_Eb, Register.BL)]
		[InlineData("40 0F96 E4", 4, Code.Setbe_Eb, Register.SPL)]
		[InlineData("40 0F96 ED", 4, Code.Setbe_Eb, Register.BPL)]
		[InlineData("40 0F96 F6", 4, Code.Setbe_Eb, Register.SIL)]
		[InlineData("40 0F96 FF", 4, Code.Setbe_Eb, Register.DIL)]
		[InlineData("45 0F96 C0", 4, Code.Setbe_Eb, Register.R8L)]
		[InlineData("45 0F96 C1", 4, Code.Setbe_Eb, Register.R9L)]
		[InlineData("45 0F96 D2", 4, Code.Setbe_Eb, Register.R10L)]
		[InlineData("45 0F96 DB", 4, Code.Setbe_Eb, Register.R11L)]
		[InlineData("45 0F96 E4", 4, Code.Setbe_Eb, Register.R12L)]
		[InlineData("45 0F96 ED", 4, Code.Setbe_Eb, Register.R13L)]
		[InlineData("45 0F96 F6", 4, Code.Setbe_Eb, Register.R14L)]
		[InlineData("45 0F96 FF", 4, Code.Setbe_Eb, Register.R15L)]

		[InlineData("0F97 C0", 3, Code.Seta_Eb, Register.AL)]
		[InlineData("0F97 C1", 3, Code.Seta_Eb, Register.CL)]
		[InlineData("0F97 D2", 3, Code.Seta_Eb, Register.DL)]
		[InlineData("0F97 DB", 3, Code.Seta_Eb, Register.BL)]
		[InlineData("0F97 E4", 3, Code.Seta_Eb, Register.AH)]
		[InlineData("0F97 ED", 3, Code.Seta_Eb, Register.CH)]
		[InlineData("0F97 F6", 3, Code.Seta_Eb, Register.DH)]
		[InlineData("0F97 FF", 3, Code.Seta_Eb, Register.BH)]

		[InlineData("40 0F97 C0", 4, Code.Seta_Eb, Register.AL)]
		[InlineData("40 0F97 C1", 4, Code.Seta_Eb, Register.CL)]
		[InlineData("40 0F97 D2", 4, Code.Seta_Eb, Register.DL)]
		[InlineData("40 0F97 DB", 4, Code.Seta_Eb, Register.BL)]
		[InlineData("40 0F97 E4", 4, Code.Seta_Eb, Register.SPL)]
		[InlineData("40 0F97 ED", 4, Code.Seta_Eb, Register.BPL)]
		[InlineData("40 0F97 F6", 4, Code.Seta_Eb, Register.SIL)]
		[InlineData("40 0F97 FF", 4, Code.Seta_Eb, Register.DIL)]
		[InlineData("45 0F97 C0", 4, Code.Seta_Eb, Register.R8L)]
		[InlineData("45 0F97 C1", 4, Code.Seta_Eb, Register.R9L)]
		[InlineData("45 0F97 D2", 4, Code.Seta_Eb, Register.R10L)]
		[InlineData("45 0F97 DB", 4, Code.Seta_Eb, Register.R11L)]
		[InlineData("45 0F97 E4", 4, Code.Seta_Eb, Register.R12L)]
		[InlineData("45 0F97 ED", 4, Code.Seta_Eb, Register.R13L)]
		[InlineData("45 0F97 F6", 4, Code.Seta_Eb, Register.R14L)]
		[InlineData("45 0F97 FF", 4, Code.Seta_Eb, Register.R15L)]
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
		void Test16_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 90 08", 4, Code.VEX_Kmovw_VK_WK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 90 08", 4, Code.VEX_Kmovb_VK_WK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 90 08", 5, Code.VEX_Kmovq_VK_WK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 90 08", 5, Code.VEX_Kmovd_VK_WK, Register.K1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mask_VK_RK_2_Data))]
		void Test16_Mask_VK_RK_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Mask_VK_RK_2_Data {
			get {
				yield return new object[] { "C5F8 90 D3", 4, Code.VEX_Kmovw_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 90 D3", 4, Code.VEX_Kmovb_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 90 D3", 5, Code.VEX_Kmovq_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 90 D3", 5, Code.VEX_Kmovd_VK_WK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 92 D3", 4, Code.VEX_Kmovw_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C5F9 92 D3", 4, Code.VEX_Kmovb_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C5FB 92 D3", 4, Code.VEX_Kmovd_VK_Rd, Register.K2, Register.EBX };

				yield return new object[] { "C5F8 93 D3", 4, Code.VEX_Kmovw_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C5F9 93 D3", 4, Code.VEX_Kmovb_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C5FB 93 D3", 4, Code.VEX_Kmovd_Gd_RK, Register.EDX, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_RK_1_Data))]
		void Test32_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 90 08", 4, Code.VEX_Kmovw_VK_WK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 90 08", 4, Code.VEX_Kmovb_VK_WK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 90 08", 5, Code.VEX_Kmovq_VK_WK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 90 08", 5, Code.VEX_Kmovd_VK_WK, Register.K1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_RK_2_Data))]
		void Test32_Mask_VK_RK_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Mask_VK_RK_2_Data {
			get {
				yield return new object[] { "C5F8 90 D3", 4, Code.VEX_Kmovw_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 90 D3", 4, Code.VEX_Kmovb_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 90 D3", 5, Code.VEX_Kmovq_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 90 D3", 5, Code.VEX_Kmovd_VK_WK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 92 D3", 4, Code.VEX_Kmovw_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C5F9 92 D3", 4, Code.VEX_Kmovb_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C5FB 92 D3", 4, Code.VEX_Kmovd_VK_Rd, Register.K2, Register.EBX };

				yield return new object[] { "C5F8 93 D3", 4, Code.VEX_Kmovw_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C5F9 93 D3", 4, Code.VEX_Kmovb_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C5FB 93 D3", 4, Code.VEX_Kmovd_Gd_RK, Register.EDX, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_RK_1_Data))]
		void Test64_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 90 08", 4, Code.VEX_Kmovw_VK_WK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 90 08", 4, Code.VEX_Kmovb_VK_WK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 90 08", 5, Code.VEX_Kmovq_VK_WK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 90 08", 5, Code.VEX_Kmovd_VK_WK, Register.K1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_RK_2_Data))]
		void Test64_Mask_VK_RK_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Mask_VK_RK_2_Data {
			get {
				yield return new object[] { "C5F8 90 D3", 4, Code.VEX_Kmovw_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 90 D3", 4, Code.VEX_Kmovb_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 90 D3", 5, Code.VEX_Kmovq_VK_WK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 90 D3", 5, Code.VEX_Kmovd_VK_WK, Register.K2, Register.K3 };

				yield return new object[] { "C5F8 92 D3", 4, Code.VEX_Kmovw_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C4C178 92 D3", 5, Code.VEX_Kmovw_VK_Rd, Register.K2, Register.R11D };
				yield return new object[] { "C5F9 92 D3", 4, Code.VEX_Kmovb_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C4C179 92 D3", 5, Code.VEX_Kmovb_VK_Rd, Register.K2, Register.R11D };
				yield return new object[] { "C4E1FB 92 D3", 5, Code.VEX_Kmovq_VK_Rq, Register.K2, Register.RBX };
				yield return new object[] { "C4C1FB 92 D3", 5, Code.VEX_Kmovq_VK_Rq, Register.K2, Register.R11 };
				yield return new object[] { "C5FB 92 D3", 4, Code.VEX_Kmovd_VK_Rd, Register.K2, Register.EBX };
				yield return new object[] { "C4C17B 92 D3", 5, Code.VEX_Kmovd_VK_Rd, Register.K2, Register.R11D };

				yield return new object[] { "C5F8 93 D3", 4, Code.VEX_Kmovw_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C46178 93 D3", 5, Code.VEX_Kmovw_Gd_RK, Register.R10D, Register.K3 };
				yield return new object[] { "C5F9 93 D3", 4, Code.VEX_Kmovb_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C46179 93 D3", 5, Code.VEX_Kmovb_Gd_RK, Register.R10D, Register.K3 };
				yield return new object[] { "C4E1FB 93 D3", 5, Code.VEX_Kmovq_Gq_RK, Register.RDX, Register.K3 };
				yield return new object[] { "C461FB 93 D3", 5, Code.VEX_Kmovq_Gq_RK, Register.R10, Register.K3 };
				yield return new object[] { "C5FB 93 D3", 4, Code.VEX_Kmovd_Gd_RK, Register.EDX, Register.K3 };
				yield return new object[] { "C4617B 93 D3", 5, Code.VEX_Kmovd_Gd_RK, Register.R10D, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mask_MK_VK_1_Data))]
		void Test16_Mask_MK_VK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Mask_MK_VK_1_Data {
			get {
				yield return new object[] { "C5F8 91 08", 4, Code.VEX_Kmovw_MK_VK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 91 08", 4, Code.VEX_Kmovb_MK_VK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 91 08", 5, Code.VEX_Kmovq_MK_VK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 91 08", 5, Code.VEX_Kmovd_MK_VK, Register.K1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_MK_VK_1_Data))]
		void Test32_Mask_MK_VK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Mask_MK_VK_1_Data {
			get {
				yield return new object[] { "C5F8 91 08", 4, Code.VEX_Kmovw_MK_VK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 91 08", 4, Code.VEX_Kmovb_MK_VK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 91 08", 5, Code.VEX_Kmovq_MK_VK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 91 08", 5, Code.VEX_Kmovd_MK_VK, Register.K1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_MK_VK_1_Data))]
		void Test64_Mask_MK_VK_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_Mask_MK_VK_1_Data {
			get {
				yield return new object[] { "C5F8 91 08", 4, Code.VEX_Kmovw_MK_VK, Register.K1, MemorySize.UInt16 };
				yield return new object[] { "C5F9 91 08", 4, Code.VEX_Kmovb_MK_VK, Register.K1, MemorySize.UInt8 };
				yield return new object[] { "C4E1F8 91 08", 5, Code.VEX_Kmovq_MK_VK, Register.K1, MemorySize.UInt64 };
				yield return new object[] { "C4E1F9 91 08", 5, Code.VEX_Kmovd_MK_VK, Register.K1, MemorySize.UInt32 };
			}
		}
	}
}
