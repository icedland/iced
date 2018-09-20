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
	public sealed class DecoderTest_1_90_97 : DecoderTest {
		[Fact]
		void Test16_Nopw_1() {
			var decoder = CreateDecoder16("90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Nopw_1() {
			var decoder = CreateDecoder32("66 90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("66 90", 2)]
		[InlineData("66 46 90", 3)]
		void Test64_Nopw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Nopd_1() {
			var decoder = CreateDecoder16("66 90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Nopd_1() {
			var decoder = CreateDecoder32("90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("90", 1)]
		[InlineData("46 90", 2)]
		void Test64_Nopd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("48 90", 2)]
		[InlineData("66 48 90", 3)]
		[InlineData("66 4E 90", 3)]
		void Test64_Nopq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Nopq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("91", 1, Code.Xchg_AX_r16, Register.CX)]
		[InlineData("92", 1, Code.Xchg_AX_r16, Register.DX)]
		[InlineData("93", 1, Code.Xchg_AX_r16, Register.BX)]
		[InlineData("94", 1, Code.Xchg_AX_r16, Register.SP)]
		[InlineData("95", 1, Code.Xchg_AX_r16, Register.BP)]
		[InlineData("96", 1, Code.Xchg_AX_r16, Register.SI)]
		[InlineData("97", 1, Code.Xchg_AX_r16, Register.DI)]
		void Test16_Xchg_Reg_AX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 91", 2, Code.Xchg_AX_r16, Register.CX)]
		[InlineData("66 92", 2, Code.Xchg_AX_r16, Register.DX)]
		[InlineData("66 93", 2, Code.Xchg_AX_r16, Register.BX)]
		[InlineData("66 94", 2, Code.Xchg_AX_r16, Register.SP)]
		[InlineData("66 95", 2, Code.Xchg_AX_r16, Register.BP)]
		[InlineData("66 96", 2, Code.Xchg_AX_r16, Register.SI)]
		[InlineData("66 97", 2, Code.Xchg_AX_r16, Register.DI)]
		void Test32_Xchg_Reg_AX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 91", 2, Code.Xchg_AX_r16, Register.CX)]
		[InlineData("66 92", 2, Code.Xchg_AX_r16, Register.DX)]
		[InlineData("66 93", 2, Code.Xchg_AX_r16, Register.BX)]
		[InlineData("66 94", 2, Code.Xchg_AX_r16, Register.SP)]
		[InlineData("66 95", 2, Code.Xchg_AX_r16, Register.BP)]
		[InlineData("66 96", 2, Code.Xchg_AX_r16, Register.SI)]
		[InlineData("66 97", 2, Code.Xchg_AX_r16, Register.DI)]
		[InlineData("66 41 90", 3, Code.Xchg_AX_r16, Register.R8W)]
		[InlineData("66 41 91", 3, Code.Xchg_AX_r16, Register.R9W)]
		[InlineData("66 41 92", 3, Code.Xchg_AX_r16, Register.R10W)]
		[InlineData("66 41 93", 3, Code.Xchg_AX_r16, Register.R11W)]
		[InlineData("66 41 94", 3, Code.Xchg_AX_r16, Register.R12W)]
		[InlineData("66 41 95", 3, Code.Xchg_AX_r16, Register.R13W)]
		[InlineData("66 41 96", 3, Code.Xchg_AX_r16, Register.R14W)]
		[InlineData("66 41 97", 3, Code.Xchg_AX_r16, Register.R15W)]
		[InlineData("66 46 91", 3, Code.Xchg_AX_r16, Register.CX)]
		[InlineData("66 46 92", 3, Code.Xchg_AX_r16, Register.DX)]
		[InlineData("66 46 93", 3, Code.Xchg_AX_r16, Register.BX)]
		[InlineData("66 46 94", 3, Code.Xchg_AX_r16, Register.SP)]
		[InlineData("66 46 95", 3, Code.Xchg_AX_r16, Register.BP)]
		[InlineData("66 46 96", 3, Code.Xchg_AX_r16, Register.SI)]
		[InlineData("66 46 97", 3, Code.Xchg_AX_r16, Register.DI)]
		[InlineData("66 47 90", 3, Code.Xchg_AX_r16, Register.R8W)]
		[InlineData("66 47 91", 3, Code.Xchg_AX_r16, Register.R9W)]
		[InlineData("66 47 92", 3, Code.Xchg_AX_r16, Register.R10W)]
		[InlineData("66 47 93", 3, Code.Xchg_AX_r16, Register.R11W)]
		[InlineData("66 47 94", 3, Code.Xchg_AX_r16, Register.R12W)]
		[InlineData("66 47 95", 3, Code.Xchg_AX_r16, Register.R13W)]
		[InlineData("66 47 96", 3, Code.Xchg_AX_r16, Register.R14W)]
		[InlineData("66 47 97", 3, Code.Xchg_AX_r16, Register.R15W)]
		void Test64_Xchg_Reg_AX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 91", 2, Code.Xchg_EAX_r32, Register.ECX)]
		[InlineData("66 92", 2, Code.Xchg_EAX_r32, Register.EDX)]
		[InlineData("66 93", 2, Code.Xchg_EAX_r32, Register.EBX)]
		[InlineData("66 94", 2, Code.Xchg_EAX_r32, Register.ESP)]
		[InlineData("66 95", 2, Code.Xchg_EAX_r32, Register.EBP)]
		[InlineData("66 96", 2, Code.Xchg_EAX_r32, Register.ESI)]
		[InlineData("66 97", 2, Code.Xchg_EAX_r32, Register.EDI)]
		void Test16_Xchg_Reg_EAX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("91", 1, Code.Xchg_EAX_r32, Register.ECX)]
		[InlineData("92", 1, Code.Xchg_EAX_r32, Register.EDX)]
		[InlineData("93", 1, Code.Xchg_EAX_r32, Register.EBX)]
		[InlineData("94", 1, Code.Xchg_EAX_r32, Register.ESP)]
		[InlineData("95", 1, Code.Xchg_EAX_r32, Register.EBP)]
		[InlineData("96", 1, Code.Xchg_EAX_r32, Register.ESI)]
		[InlineData("97", 1, Code.Xchg_EAX_r32, Register.EDI)]
		void Test32_Xchg_Reg_EAX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("91", 1, Code.Xchg_EAX_r32, Register.ECX)]
		[InlineData("92", 1, Code.Xchg_EAX_r32, Register.EDX)]
		[InlineData("93", 1, Code.Xchg_EAX_r32, Register.EBX)]
		[InlineData("94", 1, Code.Xchg_EAX_r32, Register.ESP)]
		[InlineData("95", 1, Code.Xchg_EAX_r32, Register.EBP)]
		[InlineData("96", 1, Code.Xchg_EAX_r32, Register.ESI)]
		[InlineData("97", 1, Code.Xchg_EAX_r32, Register.EDI)]
		[InlineData("41 90", 2, Code.Xchg_EAX_r32, Register.R8D)]
		[InlineData("41 91", 2, Code.Xchg_EAX_r32, Register.R9D)]
		[InlineData("41 92", 2, Code.Xchg_EAX_r32, Register.R10D)]
		[InlineData("41 93", 2, Code.Xchg_EAX_r32, Register.R11D)]
		[InlineData("41 94", 2, Code.Xchg_EAX_r32, Register.R12D)]
		[InlineData("41 95", 2, Code.Xchg_EAX_r32, Register.R13D)]
		[InlineData("41 96", 2, Code.Xchg_EAX_r32, Register.R14D)]
		[InlineData("41 97", 2, Code.Xchg_EAX_r32, Register.R15D)]
		[InlineData("46 91", 2, Code.Xchg_EAX_r32, Register.ECX)]
		[InlineData("46 92", 2, Code.Xchg_EAX_r32, Register.EDX)]
		[InlineData("46 93", 2, Code.Xchg_EAX_r32, Register.EBX)]
		[InlineData("46 94", 2, Code.Xchg_EAX_r32, Register.ESP)]
		[InlineData("46 95", 2, Code.Xchg_EAX_r32, Register.EBP)]
		[InlineData("46 96", 2, Code.Xchg_EAX_r32, Register.ESI)]
		[InlineData("46 97", 2, Code.Xchg_EAX_r32, Register.EDI)]
		[InlineData("47 90", 2, Code.Xchg_EAX_r32, Register.R8D)]
		[InlineData("47 91", 2, Code.Xchg_EAX_r32, Register.R9D)]
		[InlineData("47 92", 2, Code.Xchg_EAX_r32, Register.R10D)]
		[InlineData("47 93", 2, Code.Xchg_EAX_r32, Register.R11D)]
		[InlineData("47 94", 2, Code.Xchg_EAX_r32, Register.R12D)]
		[InlineData("47 95", 2, Code.Xchg_EAX_r32, Register.R13D)]
		[InlineData("47 96", 2, Code.Xchg_EAX_r32, Register.R14D)]
		[InlineData("47 97", 2, Code.Xchg_EAX_r32, Register.R15D)]
		void Test64_Xchg_Reg_EAX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 91", 2, Code.Xchg_RAX_r64, Register.RCX)]
		[InlineData("48 92", 2, Code.Xchg_RAX_r64, Register.RDX)]
		[InlineData("48 93", 2, Code.Xchg_RAX_r64, Register.RBX)]
		[InlineData("48 94", 2, Code.Xchg_RAX_r64, Register.RSP)]
		[InlineData("48 95", 2, Code.Xchg_RAX_r64, Register.RBP)]
		[InlineData("48 96", 2, Code.Xchg_RAX_r64, Register.RSI)]
		[InlineData("48 97", 2, Code.Xchg_RAX_r64, Register.RDI)]
		[InlineData("49 90", 2, Code.Xchg_RAX_r64, Register.R8)]
		[InlineData("49 91", 2, Code.Xchg_RAX_r64, Register.R9)]
		[InlineData("49 92", 2, Code.Xchg_RAX_r64, Register.R10)]
		[InlineData("49 93", 2, Code.Xchg_RAX_r64, Register.R11)]
		[InlineData("49 94", 2, Code.Xchg_RAX_r64, Register.R12)]
		[InlineData("49 95", 2, Code.Xchg_RAX_r64, Register.R13)]
		[InlineData("49 96", 2, Code.Xchg_RAX_r64, Register.R14)]
		[InlineData("49 97", 2, Code.Xchg_RAX_r64, Register.R15)]
		[InlineData("4E 91", 2, Code.Xchg_RAX_r64, Register.RCX)]
		[InlineData("4E 92", 2, Code.Xchg_RAX_r64, Register.RDX)]
		[InlineData("4E 93", 2, Code.Xchg_RAX_r64, Register.RBX)]
		[InlineData("4E 94", 2, Code.Xchg_RAX_r64, Register.RSP)]
		[InlineData("4E 95", 2, Code.Xchg_RAX_r64, Register.RBP)]
		[InlineData("4E 96", 2, Code.Xchg_RAX_r64, Register.RSI)]
		[InlineData("4E 97", 2, Code.Xchg_RAX_r64, Register.RDI)]
		[InlineData("4F 90", 2, Code.Xchg_RAX_r64, Register.R8)]
		[InlineData("4F 91", 2, Code.Xchg_RAX_r64, Register.R9)]
		[InlineData("4F 92", 2, Code.Xchg_RAX_r64, Register.R10)]
		[InlineData("4F 93", 2, Code.Xchg_RAX_r64, Register.R11)]
		[InlineData("4F 94", 2, Code.Xchg_RAX_r64, Register.R12)]
		[InlineData("4F 95", 2, Code.Xchg_RAX_r64, Register.R13)]
		[InlineData("4F 96", 2, Code.Xchg_RAX_r64, Register.R14)]
		[InlineData("4F 97", 2, Code.Xchg_RAX_r64, Register.R15)]
		void Test64_Xchg_Reg_RAX_1(string hexBytes, int byteLength, Code code, Register register) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RAX, instr.Op1Register);
		}

		[Fact]
		void Test16_Pause_1() {
			var decoder = CreateDecoder16("F3 90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pause, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Pause_1() {
			var decoder = CreateDecoder32("F3 90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pause, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test64_Pause_1() {
			var decoder = CreateDecoder64("F3 90");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pause, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
	}
}
