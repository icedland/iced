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
	public sealed class DecoderTest_1_B8_BF : DecoderTest {
		[Theory]
		[InlineData("B8 5AA5", 3, Code.Mov_AX_Iw, Register.AX, 0xA55A)]
		[InlineData("B9 A55A", 3, Code.Mov_CX_Iw, Register.CX, 0x5AA5)]
		[InlineData("BA 5AA5", 3, Code.Mov_DX_Iw, Register.DX, 0xA55A)]
		[InlineData("BB A55A", 3, Code.Mov_BX_Iw, Register.BX, 0x5AA5)]
		[InlineData("BC 5AA5", 3, Code.Mov_SP_Iw, Register.SP, 0xA55A)]
		[InlineData("BD A55A", 3, Code.Mov_BP_Iw, Register.BP, 0x5AA5)]
		[InlineData("BE 5AA5", 3, Code.Mov_SI_Iw, Register.SI, 0xA55A)]
		[InlineData("BF A55A", 3, Code.Mov_DI_Iw, Register.DI, 0x5AA5)]
		void Test16_Movw_Reg16Iw_1(string hexBytes, int byteLength, Code code, Register register, ushort immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 B8 5AA5", 4, Code.Mov_AX_Iw, Register.AX, 0xA55A)]
		[InlineData("66 B9 A55A", 4, Code.Mov_CX_Iw, Register.CX, 0x5AA5)]
		[InlineData("66 BA 5AA5", 4, Code.Mov_DX_Iw, Register.DX, 0xA55A)]
		[InlineData("66 BB A55A", 4, Code.Mov_BX_Iw, Register.BX, 0x5AA5)]
		[InlineData("66 BC 5AA5", 4, Code.Mov_SP_Iw, Register.SP, 0xA55A)]
		[InlineData("66 BD A55A", 4, Code.Mov_BP_Iw, Register.BP, 0x5AA5)]
		[InlineData("66 BE 5AA5", 4, Code.Mov_SI_Iw, Register.SI, 0xA55A)]
		[InlineData("66 BF A55A", 4, Code.Mov_DI_Iw, Register.DI, 0x5AA5)]
		void Test32_Movw_Reg16Iw_1(string hexBytes, int byteLength, Code code, Register register, ushort immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 B8 5AA5", 4, Code.Mov_AX_Iw, Register.AX, 0xA55A)]
		[InlineData("66 B9 A55A", 4, Code.Mov_CX_Iw, Register.CX, 0x5AA5)]
		[InlineData("66 BA 5AA5", 4, Code.Mov_DX_Iw, Register.DX, 0xA55A)]
		[InlineData("66 BB A55A", 4, Code.Mov_BX_Iw, Register.BX, 0x5AA5)]
		[InlineData("66 BC 5AA5", 4, Code.Mov_SP_Iw, Register.SP, 0xA55A)]
		[InlineData("66 BD A55A", 4, Code.Mov_BP_Iw, Register.BP, 0x5AA5)]
		[InlineData("66 BE 5AA5", 4, Code.Mov_SI_Iw, Register.SI, 0xA55A)]
		[InlineData("66 BF A55A", 4, Code.Mov_DI_Iw, Register.DI, 0x5AA5)]
		[InlineData("66 41 B8 5AA5", 5, Code.Mov_R8W_Iw, Register.R8W, 0xA55A)]
		[InlineData("66 41 B9 A55A", 5, Code.Mov_R9W_Iw, Register.R9W, 0x5AA5)]
		[InlineData("66 41 BA 5AA5", 5, Code.Mov_R10W_Iw, Register.R10W, 0xA55A)]
		[InlineData("66 41 BB A55A", 5, Code.Mov_R11W_Iw, Register.R11W, 0x5AA5)]
		[InlineData("66 41 BC 5AA5", 5, Code.Mov_R12W_Iw, Register.R12W, 0xA55A)]
		[InlineData("66 41 BD A55A", 5, Code.Mov_R13W_Iw, Register.R13W, 0x5AA5)]
		[InlineData("66 41 BE 5AA5", 5, Code.Mov_R14W_Iw, Register.R14W, 0xA55A)]
		[InlineData("66 41 BF A55A", 5, Code.Mov_R15W_Iw, Register.R15W, 0x5AA5)]

		[InlineData("66 46 B8 5AA5", 5, Code.Mov_AX_Iw, Register.AX, 0xA55A)]
		[InlineData("66 46 B9 A55A", 5, Code.Mov_CX_Iw, Register.CX, 0x5AA5)]
		[InlineData("66 46 BA 5AA5", 5, Code.Mov_DX_Iw, Register.DX, 0xA55A)]
		[InlineData("66 46 BB A55A", 5, Code.Mov_BX_Iw, Register.BX, 0x5AA5)]
		[InlineData("66 46 BC 5AA5", 5, Code.Mov_SP_Iw, Register.SP, 0xA55A)]
		[InlineData("66 46 BD A55A", 5, Code.Mov_BP_Iw, Register.BP, 0x5AA5)]
		[InlineData("66 46 BE 5AA5", 5, Code.Mov_SI_Iw, Register.SI, 0xA55A)]
		[InlineData("66 46 BF A55A", 5, Code.Mov_DI_Iw, Register.DI, 0x5AA5)]
		[InlineData("66 47 B8 5AA5", 5, Code.Mov_R8W_Iw, Register.R8W, 0xA55A)]
		[InlineData("66 47 B9 A55A", 5, Code.Mov_R9W_Iw, Register.R9W, 0x5AA5)]
		[InlineData("66 47 BA 5AA5", 5, Code.Mov_R10W_Iw, Register.R10W, 0xA55A)]
		[InlineData("66 47 BB A55A", 5, Code.Mov_R11W_Iw, Register.R11W, 0x5AA5)]
		[InlineData("66 47 BC 5AA5", 5, Code.Mov_R12W_Iw, Register.R12W, 0xA55A)]
		[InlineData("66 47 BD A55A", 5, Code.Mov_R13W_Iw, Register.R13W, 0x5AA5)]
		[InlineData("66 47 BE 5AA5", 5, Code.Mov_R14W_Iw, Register.R14W, 0xA55A)]
		[InlineData("66 47 BF A55A", 5, Code.Mov_R15W_Iw, Register.R15W, 0x5AA5)]
		void Test64_Movw_Reg16Iw_1(string hexBytes, int byteLength, Code code, Register register, ushort immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 B8 5AA51234", 6, Code.Mov_EAX_Id, Register.EAX, 0x3412A55A)]
		[InlineData("66 B9 A55A5678", 6, Code.Mov_ECX_Id, Register.ECX, 0x78565AA5)]
		[InlineData("66 BA 5AA51234", 6, Code.Mov_EDX_Id, Register.EDX, 0x3412A55A)]
		[InlineData("66 BB A55A5678", 6, Code.Mov_EBX_Id, Register.EBX, 0x78565AA5)]
		[InlineData("66 BC 5AA51234", 6, Code.Mov_ESP_Id, Register.ESP, 0x3412A55A)]
		[InlineData("66 BD A55A5678", 6, Code.Mov_EBP_Id, Register.EBP, 0x78565AA5)]
		[InlineData("66 BE 5AA51234", 6, Code.Mov_ESI_Id, Register.ESI, 0x3412A55A)]
		[InlineData("66 BF A55A5678", 6, Code.Mov_EDI_Id, Register.EDI, 0x78565AA5)]
		void Test16_Movd_Reg32Id_1(string hexBytes, int byteLength, Code code, Register register, uint immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate32);
		}

		[Theory]
		[InlineData("B8 5AA51234", 5, Code.Mov_EAX_Id, Register.EAX, 0x3412A55A)]
		[InlineData("B9 A55A5678", 5, Code.Mov_ECX_Id, Register.ECX, 0x78565AA5)]
		[InlineData("BA 5AA51234", 5, Code.Mov_EDX_Id, Register.EDX, 0x3412A55A)]
		[InlineData("BB A55A5678", 5, Code.Mov_EBX_Id, Register.EBX, 0x78565AA5)]
		[InlineData("BC 5AA51234", 5, Code.Mov_ESP_Id, Register.ESP, 0x3412A55A)]
		[InlineData("BD A55A5678", 5, Code.Mov_EBP_Id, Register.EBP, 0x78565AA5)]
		[InlineData("BE 5AA51234", 5, Code.Mov_ESI_Id, Register.ESI, 0x3412A55A)]
		[InlineData("BF A55A5678", 5, Code.Mov_EDI_Id, Register.EDI, 0x78565AA5)]
		void Test32_Movd_Reg32Id_1(string hexBytes, int byteLength, Code code, Register register, uint immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate32);
		}

		[Theory]
		[InlineData("B8 5AA51234", 5, Code.Mov_EAX_Id, Register.EAX, 0x3412A55A)]
		[InlineData("B9 A55A5678", 5, Code.Mov_ECX_Id, Register.ECX, 0x78565AA5)]
		[InlineData("BA 5AA51234", 5, Code.Mov_EDX_Id, Register.EDX, 0x3412A55A)]
		[InlineData("BB A55A5678", 5, Code.Mov_EBX_Id, Register.EBX, 0x78565AA5)]
		[InlineData("BC 5AA51234", 5, Code.Mov_ESP_Id, Register.ESP, 0x3412A55A)]
		[InlineData("BD A55A5678", 5, Code.Mov_EBP_Id, Register.EBP, 0x78565AA5)]
		[InlineData("BE 5AA51234", 5, Code.Mov_ESI_Id, Register.ESI, 0x3412A55A)]
		[InlineData("BF A55A5678", 5, Code.Mov_EDI_Id, Register.EDI, 0x78565AA5)]
		[InlineData("41 B8 5AA51234", 6, Code.Mov_R8D_Id, Register.R8D, 0x3412A55A)]
		[InlineData("41 B9 A55A5678", 6, Code.Mov_R9D_Id, Register.R9D, 0x78565AA5)]
		[InlineData("41 BA 5AA51234", 6, Code.Mov_R10D_Id, Register.R10D, 0x3412A55A)]
		[InlineData("41 BB A55A5678", 6, Code.Mov_R11D_Id, Register.R11D, 0x78565AA5)]
		[InlineData("41 BC 5AA51234", 6, Code.Mov_R12D_Id, Register.R12D, 0x3412A55A)]
		[InlineData("41 BD A55A5678", 6, Code.Mov_R13D_Id, Register.R13D, 0x78565AA5)]
		[InlineData("41 BE 5AA51234", 6, Code.Mov_R14D_Id, Register.R14D, 0x3412A55A)]
		[InlineData("41 BF A55A5678", 6, Code.Mov_R15D_Id, Register.R15D, 0x78565AA5)]

		[InlineData("46 B8 5AA51234", 6, Code.Mov_EAX_Id, Register.EAX, 0x3412A55A)]
		[InlineData("46 B9 A55A5678", 6, Code.Mov_ECX_Id, Register.ECX, 0x78565AA5)]
		[InlineData("46 BA 5AA51234", 6, Code.Mov_EDX_Id, Register.EDX, 0x3412A55A)]
		[InlineData("46 BB A55A5678", 6, Code.Mov_EBX_Id, Register.EBX, 0x78565AA5)]
		[InlineData("46 BC 5AA51234", 6, Code.Mov_ESP_Id, Register.ESP, 0x3412A55A)]
		[InlineData("46 BD A55A5678", 6, Code.Mov_EBP_Id, Register.EBP, 0x78565AA5)]
		[InlineData("46 BE 5AA51234", 6, Code.Mov_ESI_Id, Register.ESI, 0x3412A55A)]
		[InlineData("46 BF A55A5678", 6, Code.Mov_EDI_Id, Register.EDI, 0x78565AA5)]
		[InlineData("47 B8 5AA51234", 6, Code.Mov_R8D_Id, Register.R8D, 0x3412A55A)]
		[InlineData("47 B9 A55A5678", 6, Code.Mov_R9D_Id, Register.R9D, 0x78565AA5)]
		[InlineData("47 BA 5AA51234", 6, Code.Mov_R10D_Id, Register.R10D, 0x3412A55A)]
		[InlineData("47 BB A55A5678", 6, Code.Mov_R11D_Id, Register.R11D, 0x78565AA5)]
		[InlineData("47 BC 5AA51234", 6, Code.Mov_R12D_Id, Register.R12D, 0x3412A55A)]
		[InlineData("47 BD A55A5678", 6, Code.Mov_R13D_Id, Register.R13D, 0x78565AA5)]
		[InlineData("47 BE 5AA51234", 6, Code.Mov_R14D_Id, Register.R14D, 0x3412A55A)]
		[InlineData("47 BF A55A5678", 6, Code.Mov_R15D_Id, Register.R15D, 0x78565AA5)]
		void Test64_Movd_Reg32Id_1(string hexBytes, int byteLength, Code code, Register register, uint immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 B8 041526375AA51234", 10, Code.Mov_RAX_Iq, Register.RAX, 0x3412A55A37261504UL)]
		[InlineData("48 B9 04152637A55A5678", 10, Code.Mov_RCX_Iq, Register.RCX, 0x78565AA537261504UL)]
		[InlineData("48 BA 041526375AA51234", 10, Code.Mov_RDX_Iq, Register.RDX, 0x3412A55A37261504UL)]
		[InlineData("48 BB 04152637A55A5678", 10, Code.Mov_RBX_Iq, Register.RBX, 0x78565AA537261504UL)]
		[InlineData("48 BC 041526375AA51234", 10, Code.Mov_RSP_Iq, Register.RSP, 0x3412A55A37261504UL)]
		[InlineData("48 BD 04152637A55A5678", 10, Code.Mov_RBP_Iq, Register.RBP, 0x78565AA537261504UL)]
		[InlineData("48 BE 041526375AA51234", 10, Code.Mov_RSI_Iq, Register.RSI, 0x3412A55A37261504UL)]
		[InlineData("48 BF 04152637A55A5678", 10, Code.Mov_RDI_Iq, Register.RDI, 0x78565AA537261504UL)]
		[InlineData("49 B8 041526375AA51234", 10, Code.Mov_R8_Iq, Register.R8, 0x3412A55A37261504UL)]
		[InlineData("49 B9 04152637A55A5678", 10, Code.Mov_R9_Iq, Register.R9, 0x78565AA537261504UL)]
		[InlineData("49 BA 041526375AA51234", 10, Code.Mov_R10_Iq, Register.R10, 0x3412A55A37261504UL)]
		[InlineData("49 BB 04152637A55A5678", 10, Code.Mov_R11_Iq, Register.R11, 0x78565AA537261504UL)]
		[InlineData("49 BC 041526375AA51234", 10, Code.Mov_R12_Iq, Register.R12, 0x3412A55A37261504UL)]
		[InlineData("49 BD 04152637A55A5678", 10, Code.Mov_R13_Iq, Register.R13, 0x78565AA537261504UL)]
		[InlineData("49 BE 041526375AA51234", 10, Code.Mov_R14_Iq, Register.R14, 0x3412A55A37261504UL)]
		[InlineData("49 BF 04152637A55A5678", 10, Code.Mov_R15_Iq, Register.R15, 0x78565AA537261504UL)]

		[InlineData("66 4E B8 041526375AA51234", 11, Code.Mov_RAX_Iq, Register.RAX, 0x3412A55A37261504UL)]
		[InlineData("66 4E B9 04152637A55A5678", 11, Code.Mov_RCX_Iq, Register.RCX, 0x78565AA537261504UL)]
		[InlineData("66 4E BA 041526375AA51234", 11, Code.Mov_RDX_Iq, Register.RDX, 0x3412A55A37261504UL)]
		[InlineData("66 4E BB 04152637A55A5678", 11, Code.Mov_RBX_Iq, Register.RBX, 0x78565AA537261504UL)]
		[InlineData("66 4E BC 041526375AA51234", 11, Code.Mov_RSP_Iq, Register.RSP, 0x3412A55A37261504UL)]
		[InlineData("66 4E BD 04152637A55A5678", 11, Code.Mov_RBP_Iq, Register.RBP, 0x78565AA537261504UL)]
		[InlineData("66 4E BE 041526375AA51234", 11, Code.Mov_RSI_Iq, Register.RSI, 0x3412A55A37261504UL)]
		[InlineData("66 4E BF 04152637A55A5678", 11, Code.Mov_RDI_Iq, Register.RDI, 0x78565AA537261504UL)]
		[InlineData("66 4F B8 041526375AA51234", 11, Code.Mov_R8_Iq, Register.R8, 0x3412A55A37261504UL)]
		[InlineData("66 4F B9 04152637A55A5678", 11, Code.Mov_R9_Iq, Register.R9, 0x78565AA537261504UL)]
		[InlineData("66 4F BA 041526375AA51234", 11, Code.Mov_R10_Iq, Register.R10, 0x3412A55A37261504UL)]
		[InlineData("66 4F BB 04152637A55A5678", 11, Code.Mov_R11_Iq, Register.R11, 0x78565AA537261504UL)]
		[InlineData("66 4F BC 041526375AA51234", 11, Code.Mov_R12_Iq, Register.R12, 0x3412A55A37261504UL)]
		[InlineData("66 4F BD 04152637A55A5678", 11, Code.Mov_R13_Iq, Register.R13, 0x78565AA537261504UL)]
		[InlineData("66 4F BE 041526375AA51234", 11, Code.Mov_R14_Iq, Register.R14, 0x3412A55A37261504UL)]
		[InlineData("66 4F BF 04152637A55A5678", 11, Code.Mov_R15_Iq, Register.R15, 0x78565AA537261504UL)]
		void Test64_Movq_Reg64Id_1(string hexBytes, int byteLength, Code code, Register register, ulong immediate) {
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
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate64, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate64);
		}
	}
}
