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
	public sealed class DecoderTest_1_E8_EF : DecoderTest {
		[Theory]
		[InlineData("E8 5AA5", 3, Code.Call_Jw16, DecoderConstants.DEFAULT_IP16 + 3 + 0xA55A)]
		[InlineData("E8 A55A", 3, Code.Call_Jw16, DecoderConstants.DEFAULT_IP16 + 3 + 0x5AA5)]

		[InlineData("E9 5AA5", 3, Code.Jmp_Jw16, DecoderConstants.DEFAULT_IP16 + 3 + 0xA55A)]
		[InlineData("E9 A55A", 3, Code.Jmp_Jw16, DecoderConstants.DEFAULT_IP16 + 3 + 0x5AA5)]
		void Test16_CallwJmp_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 E8 5AA5", 4, Code.Call_Jw16, DecoderConstants.DEFAULT_IP32 + 4 + 0xA55A)]
		[InlineData("66 E8 A55A", 4, Code.Call_Jw16, DecoderConstants.DEFAULT_IP32 + 4 + 0x5AA5)]

		[InlineData("66 E9 5AA5", 4, Code.Jmp_Jw16, DecoderConstants.DEFAULT_IP32 + 4 + 0xA55A)]
		[InlineData("66 E9 A55A", 4, Code.Jmp_Jw16, DecoderConstants.DEFAULT_IP32 + 4 + 0x5AA5)]
		void Test32_CallwJmp_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("66 E8 12345AA5", 6, Code.Call_Jd32, DecoderConstants.DEFAULT_IP16 + 6 + 0xA55A3412)]
		[InlineData("66 E8 5678A55A", 6, Code.Call_Jd32, DecoderConstants.DEFAULT_IP16 + 6 + 0x5AA57856)]

		[InlineData("66 E9 12345AA5", 6, Code.Jmp_Jd32, DecoderConstants.DEFAULT_IP16 + 6 + 0xA55A3412)]
		[InlineData("66 E9 5678A55A", 6, Code.Jmp_Jd32, DecoderConstants.DEFAULT_IP16 + 6 + 0x5AA57856)]
		void Test16_CalldJmp_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("E8 12345AA5", 5, Code.Call_Jd32, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A3412)]
		[InlineData("E8 5678A55A", 5, Code.Call_Jd32, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA57856)]

		[InlineData("E9 12345AA5", 5, Code.Jmp_Jd32, DecoderConstants.DEFAULT_IP32 + 5 + 0xA55A3412)]
		[InlineData("E9 5678A55A", 5, Code.Jmp_Jd32, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA57856)]
		void Test32_CalldJmp_1(string hexBytes, int byteLength, Code code, ulong target) {
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
		[InlineData("E8 12345AA5", 5, Code.Call_Jd64, DecoderConstants.DEFAULT_IP64 + 5 - 0x5AA5CBEE)]
		[InlineData("E8 5678A55A", 5, Code.Call_Jd64, DecoderConstants.DEFAULT_IP64 + 5 + 0x5AA57856)]
		[InlineData("66 E8 5678A55A", 6, Code.Call_Jd64, DecoderConstants.DEFAULT_IP64 + 6 + 0x5AA57856)]
		[InlineData("4F E8 5678A55A", 6, Code.Call_Jd64, DecoderConstants.DEFAULT_IP64 + 6 + 0x5AA57856)]
		[InlineData("66 4F E8 5678A55A", 7, Code.Call_Jd64, DecoderConstants.DEFAULT_IP64 + 7 + 0x5AA57856)]

		[InlineData("E9 12345AA5", 5, Code.Jmp_Jd64, DecoderConstants.DEFAULT_IP64 + 5 - 0x5AA5CBEE)]
		[InlineData("E9 5678A55A", 5, Code.Jmp_Jd64, DecoderConstants.DEFAULT_IP64 + 5 + 0x5AA57856)]
		[InlineData("66 E9 5678A55A", 6, Code.Jmp_Jd64, DecoderConstants.DEFAULT_IP64 + 6 + 0x5AA57856)]
		[InlineData("4F E9 5678A55A", 6, Code.Jmp_Jd64, DecoderConstants.DEFAULT_IP64 + 6 + 0x5AA57856)]
		[InlineData("66 4F E9 5678A55A", 7, Code.Jmp_Jd64, DecoderConstants.DEFAULT_IP64 + 7 + 0x5AA57856)]
		void Test64_CalldJmp_1(string hexBytes, int byteLength, Code code, ulong target) {
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

		[Fact]
		void Test16_Jmp_Aww_1() {
			var decoder = CreateDecoder16("EA 1234 5678");
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Aww, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.FarBranch16, instr.Op0Kind);
			Assert.Equal(0x3412, instr.FarBranch16Target);
			Assert.Equal(0x7856, instr.FarBranchSelector);
		}

		[Fact]
		void Test32_Jmp_Aww_1() {
			var decoder = CreateDecoder32("66 EA 1234 5678");
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Aww, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.FarBranch16, instr.Op0Kind);
			Assert.Equal(0x3412, instr.FarBranch16Target);
			Assert.Equal(0x7856, instr.FarBranchSelector);
		}

		[Fact]
		void Test16_Jmp_Adw_1() {
			var decoder = CreateDecoder16("66 EA 12345678 EABC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Adw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(8, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.FarBranch32, instr.Op0Kind);
			Assert.Equal(0x78563412U, instr.FarBranch32Target);
			Assert.Equal(0xBCEA, instr.FarBranchSelector);
		}

		[Fact]
		void Test32_Jmp_Adw_1() {
			var decoder = CreateDecoder32("EA 12345678 EABC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Adw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(7, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.FarBranch32, instr.Op0Kind);
			Assert.Equal(0x78563412U, instr.FarBranch32Target);
			Assert.Equal(0xBCEA, instr.FarBranchSelector);
		}

		[Theory]
		[InlineData("EB 5A", 2, DecoderConstants.DEFAULT_IP16 + 2 + 0x5A)]
		[InlineData("EB A5", 2, DecoderConstants.DEFAULT_IP16 + 2 + 0xFFA5)]
		void Test16_Jmp_Jb16_1(string hexBytes, int byteLength, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Jb16, instr.Code);
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
		[InlineData("66 EB 5A", 3, DecoderConstants.DEFAULT_IP32 + 3 + 0x5A)]
		[InlineData("66 EB A5", 3, DecoderConstants.DEFAULT_IP32 + 3 + 0xFFA5)]
		void Test32_Jmp_Jb16_1(string hexBytes, int byteLength, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Jb16, instr.Code);
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
		[InlineData("66 EB 5A", 3, DecoderConstants.DEFAULT_IP16 + 3 + 0x5A)]
		[InlineData("66 EB A5", 3, DecoderConstants.DEFAULT_IP16 + 3 + 0xFFFFFFA5)]
		void Test16_Jmp_Jb32_1(string hexBytes, int byteLength, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Jb32, instr.Code);
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
		[InlineData("EB 5A", 2, DecoderConstants.DEFAULT_IP32 + 2 + 0x5A)]
		[InlineData("EB A5", 2, DecoderConstants.DEFAULT_IP32 + 2 + 0xFFFFFFA5)]
		void Test32_Jmp_Jb32_1(string hexBytes, int byteLength, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Jb32, instr.Code);
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
		[InlineData("EB 5A", 2, DecoderConstants.DEFAULT_IP64 + 2 + 0x5A)]
		[InlineData("EB A5", 2, DecoderConstants.DEFAULT_IP64 + 2 - 0x5B)]
		[InlineData("66 EB 5A", 3, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("66 EB A5", 3, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("4F EB 5A", 3, DecoderConstants.DEFAULT_IP64 + 3 + 0x5A)]
		[InlineData("4F EB A5", 3, DecoderConstants.DEFAULT_IP64 + 3 - 0x5B)]
		[InlineData("66 4F EB 5A", 4, DecoderConstants.DEFAULT_IP64 + 4 + 0x5A)]
		[InlineData("66 4F EB A5", 4, DecoderConstants.DEFAULT_IP64 + 4 - 0x5B)]
		void Test64_Jmp_Jb64_1(string hexBytes, int byteLength, ulong target) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Jmp_Jb64, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch64, instr.Op0Kind);
			Assert.Equal(target, instr.NearBranch64Target);
		}

		[Fact]
		void Test16_In_AL_DX_1() {
			var decoder = CreateDecoder16("EC");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test32_In_AL_DX_1() {
			var decoder = CreateDecoder32("EC");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("EC", 1)]
		[InlineData("4F EC", 2)]
		void Test64_In_AL_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AL_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test16_In_AX_DX_1() {
			var decoder = CreateDecoder16("ED");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test32_In_AX_DX_1() {
			var decoder = CreateDecoder32("66 ED");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 ED", 2)]
		[InlineData("66 47 ED", 3)]
		void Test64_In_AX_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_AX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test16_In_EAX_DX_1() {
			var decoder = CreateDecoder16("66 ED");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test32_In_EAX_DX_1() {
			var decoder = CreateDecoder32("ED");
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("ED", 1)]
		[InlineData("47 ED", 2)]
		[InlineData("4F ED", 2)]
		[InlineData("66 4F ED", 3)]
		void Test64_In_EAX_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.In_EAX_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Fact]
		void Test16_Out_DX_AL_1() {
			var decoder = CreateDecoder16("EE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Fact]
		void Test32_Out_DX_AL_1() {
			var decoder = CreateDecoder32("EE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("EE", 1)]
		[InlineData("4F EE", 2)]
		void Test64_Out_DX_AL_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Fact]
		void Test16_Out_DX_AX_1() {
			var decoder = CreateDecoder16("EF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Fact]
		void Test32_Out_DX_AX_1() {
			var decoder = CreateDecoder32("66 EF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 EF", 2)]
		[InlineData("66 47 EF", 3)]
		void Test64_Out_DX_AX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Fact]
		void Test16_Out_DX_EAX_1() {
			var decoder = CreateDecoder16("66 EF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Fact]
		void Test32_Out_DX_EAX_1() {
			var decoder = CreateDecoder32("EF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("EF", 1)]
		[InlineData("47 EF", 2)]
		[InlineData("4F EF", 2)]
		[InlineData("66 4F EF", 3)]
		void Test64_Out_DX_EAX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Out_DX_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}
	}
}
