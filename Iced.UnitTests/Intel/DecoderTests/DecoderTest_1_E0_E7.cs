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
	public sealed class DecoderTest_1_E0_E7 : DecoderTest {
		[Theory]
		[InlineData("E0 5A", 2, Code.Loopne_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0x005A)]
		[InlineData("E0 A5", 2, Code.Loopne_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("67 E0 5A", 3, Code.Loopne_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0x005A)]
		[InlineData("67 E0 A5", 3, Code.Loopne_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFA5)]

		[InlineData("E1 5A", 2, Code.Loope_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0x005A)]
		[InlineData("E1 A5", 2, Code.Loope_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("67 E1 5A", 3, Code.Loope_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0x005A)]
		[InlineData("67 E1 A5", 3, Code.Loope_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFA5)]

		[InlineData("E2 5A", 2, Code.Loop_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0x005A)]
		[InlineData("E2 A5", 2, Code.Loop_rel8_16_CX, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("67 E2 5A", 3, Code.Loop_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0x005A)]
		[InlineData("67 E2 A5", 3, Code.Loop_rel8_16_ECX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFA5)]

		[InlineData("E3 5A", 2, Code.Jcxz_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0x005A)]
		[InlineData("E3 A5", 2, Code.Jcxz_rel8_16, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		[InlineData("67 E3 5A", 3, Code.Jecxz_rel8_16, DecoderConstants.DEFAULT_IP16 + 3 + 0x005A)]
		[InlineData("67 E3 A5", 3, Code.Jecxz_rel8_16, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFA5)]
		void Test16_Loopw_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 67 E0 5A", 4, Code.Loopne_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0x005A)]
		[InlineData("66 67 E0 A5", 4, Code.Loopne_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0xFFA5)]
		[InlineData("66 E0 5A", 3, Code.Loopne_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0x005A)]
		[InlineData("66 E0 A5", 3, Code.Loopne_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]

		[InlineData("66 67 E1 5A", 4, Code.Loope_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0x005A)]
		[InlineData("66 67 E1 A5", 4, Code.Loope_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0xFFA5)]
		[InlineData("66 E1 5A", 3, Code.Loope_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0x005A)]
		[InlineData("66 E1 A5", 3, Code.Loope_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]

		[InlineData("66 67 E2 5A", 4, Code.Loop_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0x005A)]
		[InlineData("66 67 E2 A5", 4, Code.Loop_rel8_16_CX, DecoderConstants.DEFAULT_IP32 + 4 + 0xFFA5)]
		[InlineData("66 E2 5A", 3, Code.Loop_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0x005A)]
		[InlineData("66 E2 A5", 3, Code.Loop_rel8_16_ECX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]

		[InlineData("66 67 E3 5A", 4, Code.Jcxz_rel8_16, DecoderConstants.DEFAULT_IP32 + 4 + 0x005A)]
		[InlineData("66 67 E3 A5", 4, Code.Jcxz_rel8_16, DecoderConstants.DEFAULT_IP32 + 4 + 0xFFA5)]
		[InlineData("66 E3 5A", 3, Code.Jecxz_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0x005A)]
		[InlineData("66 E3 A5", 3, Code.Jecxz_rel8_16, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		void Test32_Loopw_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 67 E0 5A", 4, Code.Loopne_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0x0000005A)]
		[InlineData("66 67 E0 A5", 4, Code.Loopne_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0xFFFFFFA5)]
		[InlineData("66 E0 5A", 3, Code.Loopne_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0x0000005A)]
		[InlineData("66 E0 A5", 3, Code.Loopne_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]

		[InlineData("66 67 E1 5A", 4, Code.Loope_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0x0000005A)]
		[InlineData("66 67 E1 A5", 4, Code.Loope_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0xFFFFFFA5)]
		[InlineData("66 E1 5A", 3, Code.Loope_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0x0000005A)]
		[InlineData("66 E1 A5", 3, Code.Loope_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]

		[InlineData("66 67 E2 5A", 4, Code.Loop_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0x0000005A)]
		[InlineData("66 67 E2 A5", 4, Code.Loop_rel8_32_ECX, DecoderConstants.DEFAULT_IP16 + 4 + 0xFFFFFFA5)]
		[InlineData("66 E2 5A", 3, Code.Loop_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0x0000005A)]
		[InlineData("66 E2 A5", 3, Code.Loop_rel8_32_CX, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]

		[InlineData("66 67 E3 5A", 4, Code.Jecxz_rel8_32, DecoderConstants.DEFAULT_IP16 + 4 + 0x0000005A)]
		[InlineData("66 67 E3 A5", 4, Code.Jecxz_rel8_32, DecoderConstants.DEFAULT_IP16 + 4 + 0xFFFFFFA5)]
		[InlineData("66 E3 5A", 3, Code.Jcxz_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0x0000005A)]
		[InlineData("66 E3 A5", 3, Code.Jcxz_rel8_32, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		void Test16_Loopd_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("E0 5A", 2, Code.Loopne_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0x0000005A)]
		[InlineData("E0 A5", 2, Code.Loopne_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("67 E0 5A", 3, Code.Loopne_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0x0000005A)]
		[InlineData("67 E0 A5", 3, Code.Loopne_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFFFFFA5)]

		[InlineData("E1 5A", 2, Code.Loope_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0x0000005A)]
		[InlineData("E1 A5", 2, Code.Loope_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("67 E1 5A", 3, Code.Loope_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0x0000005A)]
		[InlineData("67 E1 A5", 3, Code.Loope_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFFFFFA5)]

		[InlineData("E2 5A", 2, Code.Loop_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0x0000005A)]
		[InlineData("E2 A5", 2, Code.Loop_rel8_32_ECX, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("67 E2 5A", 3, Code.Loop_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0x0000005A)]
		[InlineData("67 E2 A5", 3, Code.Loop_rel8_32_CX, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFFFFFA5)]

		[InlineData("E3 5A", 2, Code.Jecxz_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0x0000005A)]
		[InlineData("E3 A5", 2, Code.Jecxz_rel8_32, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		[InlineData("67 E3 5A", 3, Code.Jcxz_rel8_32, DecoderConstants.DEFAULT_IP32 + 3 + 0x0000005A)]
		[InlineData("67 E3 A5", 3, Code.Jcxz_rel8_32, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFFFFFA5)]
		void Test32_Loopd_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("E0 5A", 2, Code.Loopne_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("E0 A5", 2, Code.Loopne_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("67 E0 5A", 3, Code.Loopne_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("67 E0 A5", 3, Code.Loopne_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]

		[InlineData("E1 5A", 2, Code.Loope_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("E1 A5", 2, Code.Loope_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("67 E1 5A", 3, Code.Loope_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("67 E1 A5", 3, Code.Loope_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]

		[InlineData("E2 5A", 2, Code.Loop_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("E2 A5", 2, Code.Loop_rel8_64_RCX, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("67 E2 5A", 3, Code.Loop_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("67 E2 A5", 3, Code.Loop_rel8_64_ECX, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]

		[InlineData("E3 5A", 2, Code.Jrcxz_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("E3 A5", 2, Code.Jrcxz_rel8_64, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("67 E3 5A", 3, Code.Jecxz_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("67 E3 A5", 3, Code.Jecxz_rel8_64, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		void Test64_Loopq_1(string hexBytes, int byteLength, Code code, ulong target) {
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

		[Theory]
		[InlineData("E4 5A", 2, 0x5A)]
		[InlineData("E4 A5", 2, 0xA5)]
		void Test16_In_AL_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E4 5A", 2, 0x5A)]
		[InlineData("E4 A5", 2, 0xA5)]
		void Test32_In_AL_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E4 5A", 2, 0x5A)]
		[InlineData("E4 A5", 2, 0xA5)]
		[InlineData("4F E4 5A", 3, 0x5A)]
		void Test64_In_AL_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E5 5A", 2, 0x5A)]
		[InlineData("E5 A5", 2, 0xA5)]
		void Test16_In_AX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 E5 5A", 3, 0x5A)]
		[InlineData("66 E5 A5", 3, 0xA5)]
		void Test32_In_AX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 E5 5A", 3, 0x5A)]
		[InlineData("66 E5 A5", 3, 0xA5)]
		[InlineData("66 47 E5 5A", 4, 0x5A)]
		void Test64_In_AX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 E5 5A", 3, 0x5A)]
		[InlineData("66 E5 A5", 3, 0xA5)]
		void Test16_In_EAX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E5 5A", 2, 0x5A)]
		[InlineData("E5 A5", 2, 0xA5)]
		void Test32_In_EAX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E5 5A", 2, 0x5A)]
		[InlineData("E5 A5", 2, 0xA5)]
		[InlineData("47 E5 A5", 3, 0xA5)]
		[InlineData("4F E5 5A", 3, 0x5A)]
		[InlineData("66 4F E5 A5", 4, 0xA5)]
		void Test64_In_EAX_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_imm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("E6 5A", 2, 0x5A)]
		[InlineData("E6 A5", 2, 0xA5)]
		void Test16_Out_imm8_AL_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("E6 5A", 2, 0x5A)]
		[InlineData("E6 A5", 2, 0xA5)]
		void Test32_Out_imm8_AL_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("E6 5A", 2, 0x5A)]
		[InlineData("E6 A5", 2, 0xA5)]
		[InlineData("4F E6 5A", 3, 0x5A)]
		void Test64_Out_imm8_AL_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("E7 5A", 2, 0x5A)]
		[InlineData("E7 A5", 2, 0xA5)]
		void Test16_Out_imm8_AX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 E7 5A", 3, 0x5A)]
		[InlineData("66 E7 A5", 3, 0xA5)]
		void Test32_Out_imm8_AX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 E7 5A", 3, 0x5A)]
		[InlineData("66 E7 A5", 3, 0xA5)]
		[InlineData("66 47 E7 5A", 4, 0x5A)]
		void Test64_Out_imm8_AX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 E7 5A", 3, 0x5A)]
		[InlineData("66 E7 A5", 3, 0xA5)]
		void Test16_Out_imm8_EAX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("E7 5A", 2, 0x5A)]
		[InlineData("E7 A5", 2, 0xA5)]
		void Test32_Out_imm8_EAX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("E7 5A", 2, 0x5A)]
		[InlineData("E7 A5", 2, 0xA5)]
		[InlineData("47 E7 A5", 3, 0xA5)]
		[InlineData("4F E7 5A", 3, 0x5A)]
		[InlineData("66 4F E7 A5", 4, 0xA5)]
		void Test64_Out_imm8_EAX_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_imm8_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}
	}
}
