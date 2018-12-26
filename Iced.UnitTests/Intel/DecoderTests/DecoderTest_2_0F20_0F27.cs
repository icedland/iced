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
	public sealed class DecoderTest_2_0F20_0F27 : DecoderTest {
		[Theory]
		[InlineData("0F20 DE", 3, Register.ESI, Register.CR3)]
		[InlineData("0F20 B1", 3, Register.ECX, Register.CR6)]
		[InlineData("0F20 5E", 3, Register.ESI, Register.CR3)]
		[InlineData("0F20 31", 3, Register.ECX, Register.CR6)]

		[InlineData("66 0F20 DE", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F20 B1", 4, Register.ECX, Register.CR6)]
		[InlineData("66 0F20 5E", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F20 31", 4, Register.ECX, Register.CR6)]

		[InlineData("F0 0F20 C1", 4, Register.ECX, Register.CR8)]
		void Test16_Mov_r32_cr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_cr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F20 DE", 3, Register.ESI, Register.CR3)]
		[InlineData("0F20 B1", 3, Register.ECX, Register.CR6)]
		[InlineData("0F20 5E", 3, Register.ESI, Register.CR3)]
		[InlineData("0F20 31", 3, Register.ECX, Register.CR6)]

		[InlineData("66 0F20 DE", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F20 B1", 4, Register.ECX, Register.CR6)]
		[InlineData("66 0F20 5E", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F20 31", 4, Register.ECX, Register.CR6)]

		[InlineData("F0 0F20 C1", 4, Register.ECX, Register.CR8)]
		void Test32_Mov_r32_cr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_cr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F20 DE", 3, Register.RSI, Register.CR3)]
		[InlineData("0F20 B1", 3, Register.RCX, Register.CR6)]
		[InlineData("0F20 5E", 3, Register.RSI, Register.CR3)]
		[InlineData("0F20 31", 3, Register.RCX, Register.CR6)]

		[InlineData("41 0F20 31", 4, Register.R9, Register.CR6)]
		[InlineData("41 0F20 5E", 4, Register.R14, Register.CR3)]
		[InlineData("41 0F20 B1", 4, Register.R9, Register.CR6)]
		[InlineData("41 0F20 DE", 4, Register.R14, Register.CR3)]

		[InlineData("44 0F20 31", 4, Register.RCX, Register.CR14)]
		[InlineData("44 0F20 5E", 4, Register.RSI, Register.CR11)]
		[InlineData("44 0F20 B1", 4, Register.RCX, Register.CR14)]
		[InlineData("44 0F20 DE", 4, Register.RSI, Register.CR11)]

		[InlineData("45 0F20 31", 4, Register.R9, Register.CR14)]
		[InlineData("45 0F20 5E", 4, Register.R14, Register.CR11)]
		[InlineData("45 0F20 B1", 4, Register.R9, Register.CR14)]
		[InlineData("45 0F20 DE", 4, Register.R14, Register.CR11)]

		[InlineData("66 0F20 31", 4, Register.RCX, Register.CR6)]
		[InlineData("66 0F20 5E", 4, Register.RSI, Register.CR3)]
		[InlineData("66 0F20 B1", 4, Register.RCX, Register.CR6)]
		[InlineData("66 0F20 DE", 4, Register.RSI, Register.CR3)]

		[InlineData("66 41 0F20 31", 5, Register.R9, Register.CR6)]
		[InlineData("66 41 0F20 5E", 5, Register.R14, Register.CR3)]
		[InlineData("66 41 0F20 B1", 5, Register.R9, Register.CR6)]
		[InlineData("66 41 0F20 DE", 5, Register.R14, Register.CR3)]

		[InlineData("66 44 0F20 31", 5, Register.RCX, Register.CR14)]
		[InlineData("66 44 0F20 5E", 5, Register.RSI, Register.CR11)]
		[InlineData("66 44 0F20 B1", 5, Register.RCX, Register.CR14)]
		[InlineData("66 44 0F20 DE", 5, Register.RSI, Register.CR11)]

		[InlineData("66 45 0F20 31", 5, Register.R9, Register.CR14)]
		[InlineData("66 45 0F20 5E", 5, Register.R14, Register.CR11)]
		[InlineData("66 45 0F20 B1", 5, Register.R9, Register.CR14)]
		[InlineData("66 45 0F20 DE", 5, Register.R14, Register.CR11)]

		[InlineData("66 4A 0F20 31", 5, Register.RCX, Register.CR6)]
		[InlineData("66 4A 0F20 5E", 5, Register.RSI, Register.CR3)]
		[InlineData("66 4A 0F20 B1", 5, Register.RCX, Register.CR6)]
		[InlineData("66 4A 0F20 DE", 5, Register.RSI, Register.CR3)]

		[InlineData("66 4B 0F20 31", 5, Register.R9, Register.CR6)]
		[InlineData("66 4B 0F20 5E", 5, Register.R14, Register.CR3)]
		[InlineData("66 4B 0F20 B1", 5, Register.R9, Register.CR6)]
		[InlineData("66 4B 0F20 DE", 5, Register.R14, Register.CR3)]

		[InlineData("66 4E 0F20 31", 5, Register.RCX, Register.CR14)]
		[InlineData("66 4E 0F20 5E", 5, Register.RSI, Register.CR11)]
		[InlineData("66 4E 0F20 B1", 5, Register.RCX, Register.CR14)]
		[InlineData("66 4E 0F20 DE", 5, Register.RSI, Register.CR11)]

		[InlineData("66 4F 0F20 31", 5, Register.R9, Register.CR14)]
		[InlineData("66 4F 0F20 5E", 5, Register.R14, Register.CR11)]
		[InlineData("66 4F 0F20 B1", 5, Register.R9, Register.CR14)]
		[InlineData("66 4F 0F20 DE", 5, Register.R14, Register.CR11)]

		[InlineData("F0 0F20 C1", 4, Register.RCX, Register.CR8)]
		void Test64_Mov_r64_cr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r64_cr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F22 DE", 3, Register.ESI, Register.CR3)]
		[InlineData("0F22 B1", 3, Register.ECX, Register.CR6)]
		[InlineData("0F22 5E", 3, Register.ESI, Register.CR3)]
		[InlineData("0F22 31", 3, Register.ECX, Register.CR6)]

		[InlineData("66 0F22 DE", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F22 B1", 4, Register.ECX, Register.CR6)]
		[InlineData("66 0F22 5E", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F22 31", 4, Register.ECX, Register.CR6)]

		[InlineData("F0 0F22 C1", 4, Register.ECX, Register.CR8)]
		void Test16_Mov_cr_r32_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_cr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F22 DE", 3, Register.ESI, Register.CR3)]
		[InlineData("0F22 B1", 3, Register.ECX, Register.CR6)]
		[InlineData("0F22 5E", 3, Register.ESI, Register.CR3)]
		[InlineData("0F22 31", 3, Register.ECX, Register.CR6)]

		[InlineData("66 0F22 DE", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F22 B1", 4, Register.ECX, Register.CR6)]
		[InlineData("66 0F22 5E", 4, Register.ESI, Register.CR3)]
		[InlineData("66 0F22 31", 4, Register.ECX, Register.CR6)]

		[InlineData("F0 0F22 C1", 4, Register.ECX, Register.CR8)]
		void Test32_Mov_cr_r32_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_cr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F22 DE", 3, Register.RSI, Register.CR3)]
		[InlineData("0F22 B1", 3, Register.RCX, Register.CR6)]
		[InlineData("0F22 5E", 3, Register.RSI, Register.CR3)]
		[InlineData("0F22 31", 3, Register.RCX, Register.CR6)]

		[InlineData("41 0F22 31", 4, Register.R9, Register.CR6)]
		[InlineData("41 0F22 5E", 4, Register.R14, Register.CR3)]
		[InlineData("41 0F22 B1", 4, Register.R9, Register.CR6)]
		[InlineData("41 0F22 DE", 4, Register.R14, Register.CR3)]

		[InlineData("44 0F22 31", 4, Register.RCX, Register.CR14)]
		[InlineData("44 0F22 5E", 4, Register.RSI, Register.CR11)]
		[InlineData("44 0F22 B1", 4, Register.RCX, Register.CR14)]
		[InlineData("44 0F22 DE", 4, Register.RSI, Register.CR11)]

		[InlineData("45 0F22 31", 4, Register.R9, Register.CR14)]
		[InlineData("45 0F22 5E", 4, Register.R14, Register.CR11)]
		[InlineData("45 0F22 B1", 4, Register.R9, Register.CR14)]
		[InlineData("45 0F22 DE", 4, Register.R14, Register.CR11)]

		[InlineData("66 0F22 31", 4, Register.RCX, Register.CR6)]
		[InlineData("66 0F22 5E", 4, Register.RSI, Register.CR3)]
		[InlineData("66 0F22 B1", 4, Register.RCX, Register.CR6)]
		[InlineData("66 0F22 DE", 4, Register.RSI, Register.CR3)]

		[InlineData("66 41 0F22 31", 5, Register.R9, Register.CR6)]
		[InlineData("66 41 0F22 5E", 5, Register.R14, Register.CR3)]
		[InlineData("66 41 0F22 B1", 5, Register.R9, Register.CR6)]
		[InlineData("66 41 0F22 DE", 5, Register.R14, Register.CR3)]

		[InlineData("66 44 0F22 31", 5, Register.RCX, Register.CR14)]
		[InlineData("66 44 0F22 5E", 5, Register.RSI, Register.CR11)]
		[InlineData("66 44 0F22 B1", 5, Register.RCX, Register.CR14)]
		[InlineData("66 44 0F22 DE", 5, Register.RSI, Register.CR11)]

		[InlineData("66 45 0F22 31", 5, Register.R9, Register.CR14)]
		[InlineData("66 45 0F22 5E", 5, Register.R14, Register.CR11)]
		[InlineData("66 45 0F22 B1", 5, Register.R9, Register.CR14)]
		[InlineData("66 45 0F22 DE", 5, Register.R14, Register.CR11)]

		[InlineData("66 4A 0F22 31", 5, Register.RCX, Register.CR6)]
		[InlineData("66 4A 0F22 5E", 5, Register.RSI, Register.CR3)]
		[InlineData("66 4A 0F22 B1", 5, Register.RCX, Register.CR6)]
		[InlineData("66 4A 0F22 DE", 5, Register.RSI, Register.CR3)]

		[InlineData("66 4B 0F22 31", 5, Register.R9, Register.CR6)]
		[InlineData("66 4B 0F22 5E", 5, Register.R14, Register.CR3)]
		[InlineData("66 4B 0F22 B1", 5, Register.R9, Register.CR6)]
		[InlineData("66 4B 0F22 DE", 5, Register.R14, Register.CR3)]

		[InlineData("66 4E 0F22 31", 5, Register.RCX, Register.CR14)]
		[InlineData("66 4E 0F22 5E", 5, Register.RSI, Register.CR11)]
		[InlineData("66 4E 0F22 B1", 5, Register.RCX, Register.CR14)]
		[InlineData("66 4E 0F22 DE", 5, Register.RSI, Register.CR11)]

		[InlineData("66 4F 0F22 31", 5, Register.R9, Register.CR14)]
		[InlineData("66 4F 0F22 5E", 5, Register.R14, Register.CR11)]
		[InlineData("66 4F 0F22 B1", 5, Register.R9, Register.CR14)]
		[InlineData("66 4F 0F22 DE", 5, Register.R14, Register.CR11)]

		[InlineData("F0 0F22 C1", 4, Register.RCX, Register.CR8)]
		void Test64_Mov_cr_r64_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_cr_r64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F21 DE", 3, Register.ESI, Register.DR3)]
		[InlineData("0F21 B1", 3, Register.ECX, Register.DR6)]
		[InlineData("0F21 5E", 3, Register.ESI, Register.DR3)]
		[InlineData("0F21 31", 3, Register.ECX, Register.DR6)]

		[InlineData("66 0F21 DE", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F21 B1", 4, Register.ECX, Register.DR6)]
		[InlineData("66 0F21 5E", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F21 31", 4, Register.ECX, Register.DR6)]
		void Test16_Mov_r32_dr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_dr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F21 DE", 3, Register.ESI, Register.DR3)]
		[InlineData("0F21 B1", 3, Register.ECX, Register.DR6)]
		[InlineData("0F21 5E", 3, Register.ESI, Register.DR3)]
		[InlineData("0F21 31", 3, Register.ECX, Register.DR6)]

		[InlineData("66 0F21 DE", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F21 B1", 4, Register.ECX, Register.DR6)]
		[InlineData("66 0F21 5E", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F21 31", 4, Register.ECX, Register.DR6)]
		void Test32_Mov_r32_dr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_dr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F21 DE", 3, Register.RSI, Register.DR3)]
		[InlineData("0F21 B1", 3, Register.RCX, Register.DR6)]
		[InlineData("0F21 5E", 3, Register.RSI, Register.DR3)]
		[InlineData("0F21 31", 3, Register.RCX, Register.DR6)]

		[InlineData("41 0F21 31", 4, Register.R9, Register.DR6)]
		[InlineData("41 0F21 5E", 4, Register.R14, Register.DR3)]
		[InlineData("41 0F21 B1", 4, Register.R9, Register.DR6)]
		[InlineData("41 0F21 DE", 4, Register.R14, Register.DR3)]

		[InlineData("44 0F21 31", 4, Register.RCX, Register.DR14)]
		[InlineData("44 0F21 5E", 4, Register.RSI, Register.DR11)]
		[InlineData("44 0F21 B1", 4, Register.RCX, Register.DR14)]
		[InlineData("44 0F21 DE", 4, Register.RSI, Register.DR11)]

		[InlineData("45 0F21 31", 4, Register.R9, Register.DR14)]
		[InlineData("45 0F21 5E", 4, Register.R14, Register.DR11)]
		[InlineData("45 0F21 B1", 4, Register.R9, Register.DR14)]
		[InlineData("45 0F21 DE", 4, Register.R14, Register.DR11)]

		[InlineData("66 0F21 31", 4, Register.RCX, Register.DR6)]
		[InlineData("66 0F21 5E", 4, Register.RSI, Register.DR3)]
		[InlineData("66 0F21 B1", 4, Register.RCX, Register.DR6)]
		[InlineData("66 0F21 DE", 4, Register.RSI, Register.DR3)]

		[InlineData("66 41 0F21 31", 5, Register.R9, Register.DR6)]
		[InlineData("66 41 0F21 5E", 5, Register.R14, Register.DR3)]
		[InlineData("66 41 0F21 B1", 5, Register.R9, Register.DR6)]
		[InlineData("66 41 0F21 DE", 5, Register.R14, Register.DR3)]

		[InlineData("66 44 0F21 31", 5, Register.RCX, Register.DR14)]
		[InlineData("66 44 0F21 5E", 5, Register.RSI, Register.DR11)]
		[InlineData("66 44 0F21 B1", 5, Register.RCX, Register.DR14)]
		[InlineData("66 44 0F21 DE", 5, Register.RSI, Register.DR11)]

		[InlineData("66 45 0F21 31", 5, Register.R9, Register.DR14)]
		[InlineData("66 45 0F21 5E", 5, Register.R14, Register.DR11)]
		[InlineData("66 45 0F21 B1", 5, Register.R9, Register.DR14)]
		[InlineData("66 45 0F21 DE", 5, Register.R14, Register.DR11)]

		[InlineData("66 4A 0F21 31", 5, Register.RCX, Register.DR6)]
		[InlineData("66 4A 0F21 5E", 5, Register.RSI, Register.DR3)]
		[InlineData("66 4A 0F21 B1", 5, Register.RCX, Register.DR6)]
		[InlineData("66 4A 0F21 DE", 5, Register.RSI, Register.DR3)]

		[InlineData("66 4B 0F21 31", 5, Register.R9, Register.DR6)]
		[InlineData("66 4B 0F21 5E", 5, Register.R14, Register.DR3)]
		[InlineData("66 4B 0F21 B1", 5, Register.R9, Register.DR6)]
		[InlineData("66 4B 0F21 DE", 5, Register.R14, Register.DR3)]

		[InlineData("66 4E 0F21 31", 5, Register.RCX, Register.DR14)]
		[InlineData("66 4E 0F21 5E", 5, Register.RSI, Register.DR11)]
		[InlineData("66 4E 0F21 B1", 5, Register.RCX, Register.DR14)]
		[InlineData("66 4E 0F21 DE", 5, Register.RSI, Register.DR11)]

		[InlineData("66 4F 0F21 31", 5, Register.R9, Register.DR14)]
		[InlineData("66 4F 0F21 5E", 5, Register.R14, Register.DR11)]
		[InlineData("66 4F 0F21 B1", 5, Register.R9, Register.DR14)]
		[InlineData("66 4F 0F21 DE", 5, Register.R14, Register.DR11)]
		void Test64_Mov_r64_dr_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r64_dr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(crReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F23 DE", 3, Register.ESI, Register.DR3)]
		[InlineData("0F23 B1", 3, Register.ECX, Register.DR6)]
		[InlineData("0F23 5E", 3, Register.ESI, Register.DR3)]
		[InlineData("0F23 31", 3, Register.ECX, Register.DR6)]

		[InlineData("66 0F23 DE", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F23 B1", 4, Register.ECX, Register.DR6)]
		[InlineData("66 0F23 5E", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F23 31", 4, Register.ECX, Register.DR6)]
		void Test16_Mov_dr_r32_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_dr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F23 DE", 3, Register.ESI, Register.DR3)]
		[InlineData("0F23 B1", 3, Register.ECX, Register.DR6)]
		[InlineData("0F23 5E", 3, Register.ESI, Register.DR3)]
		[InlineData("0F23 31", 3, Register.ECX, Register.DR6)]

		[InlineData("66 0F23 DE", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F23 B1", 4, Register.ECX, Register.DR6)]
		[InlineData("66 0F23 5E", 4, Register.ESI, Register.DR3)]
		[InlineData("66 0F23 31", 4, Register.ECX, Register.DR6)]
		void Test32_Mov_dr_r32_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_dr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F23 DE", 3, Register.RSI, Register.DR3)]
		[InlineData("0F23 B1", 3, Register.RCX, Register.DR6)]
		[InlineData("0F23 5E", 3, Register.RSI, Register.DR3)]
		[InlineData("0F23 31", 3, Register.RCX, Register.DR6)]

		[InlineData("41 0F23 31", 4, Register.R9, Register.DR6)]
		[InlineData("41 0F23 5E", 4, Register.R14, Register.DR3)]
		[InlineData("41 0F23 B1", 4, Register.R9, Register.DR6)]
		[InlineData("41 0F23 DE", 4, Register.R14, Register.DR3)]

		[InlineData("44 0F23 31", 4, Register.RCX, Register.DR14)]
		[InlineData("44 0F23 5E", 4, Register.RSI, Register.DR11)]
		[InlineData("44 0F23 B1", 4, Register.RCX, Register.DR14)]
		[InlineData("44 0F23 DE", 4, Register.RSI, Register.DR11)]

		[InlineData("45 0F23 31", 4, Register.R9, Register.DR14)]
		[InlineData("45 0F23 5E", 4, Register.R14, Register.DR11)]
		[InlineData("45 0F23 B1", 4, Register.R9, Register.DR14)]
		[InlineData("45 0F23 DE", 4, Register.R14, Register.DR11)]

		[InlineData("66 0F23 31", 4, Register.RCX, Register.DR6)]
		[InlineData("66 0F23 5E", 4, Register.RSI, Register.DR3)]
		[InlineData("66 0F23 B1", 4, Register.RCX, Register.DR6)]
		[InlineData("66 0F23 DE", 4, Register.RSI, Register.DR3)]

		[InlineData("66 41 0F23 31", 5, Register.R9, Register.DR6)]
		[InlineData("66 41 0F23 5E", 5, Register.R14, Register.DR3)]
		[InlineData("66 41 0F23 B1", 5, Register.R9, Register.DR6)]
		[InlineData("66 41 0F23 DE", 5, Register.R14, Register.DR3)]

		[InlineData("66 44 0F23 31", 5, Register.RCX, Register.DR14)]
		[InlineData("66 44 0F23 5E", 5, Register.RSI, Register.DR11)]
		[InlineData("66 44 0F23 B1", 5, Register.RCX, Register.DR14)]
		[InlineData("66 44 0F23 DE", 5, Register.RSI, Register.DR11)]

		[InlineData("66 45 0F23 31", 5, Register.R9, Register.DR14)]
		[InlineData("66 45 0F23 5E", 5, Register.R14, Register.DR11)]
		[InlineData("66 45 0F23 B1", 5, Register.R9, Register.DR14)]
		[InlineData("66 45 0F23 DE", 5, Register.R14, Register.DR11)]

		[InlineData("66 4A 0F23 31", 5, Register.RCX, Register.DR6)]
		[InlineData("66 4A 0F23 5E", 5, Register.RSI, Register.DR3)]
		[InlineData("66 4A 0F23 B1", 5, Register.RCX, Register.DR6)]
		[InlineData("66 4A 0F23 DE", 5, Register.RSI, Register.DR3)]

		[InlineData("66 4B 0F23 31", 5, Register.R9, Register.DR6)]
		[InlineData("66 4B 0F23 5E", 5, Register.R14, Register.DR3)]
		[InlineData("66 4B 0F23 B1", 5, Register.R9, Register.DR6)]
		[InlineData("66 4B 0F23 DE", 5, Register.R14, Register.DR3)]

		[InlineData("66 4E 0F23 31", 5, Register.RCX, Register.DR14)]
		[InlineData("66 4E 0F23 5E", 5, Register.RSI, Register.DR11)]
		[InlineData("66 4E 0F23 B1", 5, Register.RCX, Register.DR14)]
		[InlineData("66 4E 0F23 DE", 5, Register.RSI, Register.DR11)]

		[InlineData("66 4F 0F23 31", 5, Register.R9, Register.DR14)]
		[InlineData("66 4F 0F23 5E", 5, Register.R14, Register.DR11)]
		[InlineData("66 4F 0F23 B1", 5, Register.R9, Register.DR14)]
		[InlineData("66 4F 0F23 DE", 5, Register.R14, Register.DR11)]
		void Test64_Mov_dr_r64_1(string hexBytes, int byteLength, Register gpReg, Register crReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_dr_r64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(crReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F24 DE", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F24 B1", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("0F24 5E", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F24 31", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]

		[InlineData("66 0F24 DE", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F24 B1", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("66 0F24 5E", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F24 31", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		void Test16_Mov_r32_tr_1(string hexBytes, int byteLength, Register gpReg, Register trReg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_tr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(trReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F24 DE", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F24 B1", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("0F24 5E", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F24 31", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]

		[InlineData("66 0F24 DE", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F24 B1", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("66 0F24 5E", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F24 31", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		void Test32_Mov_r32_tr_1(string hexBytes, int byteLength, Register gpReg, Register trReg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_r32_tr, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(gpReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(trReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F26 DE", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F26 B1", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("0F26 5E", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F26 31", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]

		[InlineData("66 0F26 DE", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F26 B1", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("66 0F26 5E", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F26 31", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		void Test16_Mov_tr_r32_1(string hexBytes, int byteLength, Register gpReg, Register trReg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_tr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(trReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F26 DE", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F26 B1", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("0F26 5E", 3, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("0F26 31", 3, Register.ECX, Register.TR6, DecoderOptions.MovTr)]

		[InlineData("66 0F26 DE", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F26 B1", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		[InlineData("66 0F26 5E", 4, Register.ESI, Register.TR3, DecoderOptions.MovTr)]
		[InlineData("66 0F26 31", 4, Register.ECX, Register.TR6, DecoderOptions.MovTr)]
		void Test32_Mov_tr_r32_1(string hexBytes, int byteLength, Register gpReg, Register trReg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_tr_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(trReg, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(gpReg, instr.Op1Register);
		}
	}
}
