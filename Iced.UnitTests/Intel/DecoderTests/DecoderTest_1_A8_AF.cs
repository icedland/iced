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
	public sealed class DecoderTest_1_A8_AF : DecoderTest {
		[Fact]
		void Test16_Test_AL_Ib_1() {
			var decoder = CreateDecoder16("A8 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Test_AL_Ib_1() {
			var decoder = CreateDecoder32("A8 A5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0xA5, instr.Immediate8);
		}

		[Theory]
		[InlineData("A8 A5", 2)]
		[InlineData("4F A8 A5", 3)]
		void Test64_Test_AL_Ib_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0xA5, instr.Immediate8);
		}

		[Fact]
		void Test16_Test_AX_Iw_1() {
			var decoder = CreateDecoder16("A9 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test32_Test_AX_Iw_1() {
			var decoder = CreateDecoder32("66 A9 A55A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0x5AA5, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 A9 A55A", 4)]
		[InlineData("66 47 A9 A55A", 5)]
		void Test64_Test_AX_Iw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0x5AA5, instr.Immediate16);
		}

		[Fact]
		void Test16_Test_EAX_Id_1() {
			var decoder = CreateDecoder16("66 A9 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test32_Test_EAX_Id_1() {
			var decoder = CreateDecoder32("A9 A55A3412");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x12345AA5U, instr.Immediate32);
		}

		[Theory]
		[InlineData("A9 A55A3412", 5)]
		[InlineData("47 A9 A55A3412", 6)]
		void Test64_Test_EAX_Id_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x12345AA5U, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 A9 A55A34A2", 6)]
		[InlineData("4F A9 A55A34A2", 6)]
		void Test64_Test_RAX_Id64_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_RAX_Id64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal(0xFFFFFFFFA2345AA5UL, (ulong)instr.Immediate32to64);
		}

		[Theory]
		[InlineData("AA", 1)]
		void Test16_Stosb_Yb_AL_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AA", 2)]
		void Test16_Stosb_Yb_AL_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AA", 2)]
		void Test32_Stosb_Yb_AL_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("AA", 1)]
		void Test32_Stosb_Yb_AL_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AA", 2)]
		[InlineData("66 67 4F AA", 4)]
		void Test64_Stosb_Yb_AL_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("AA", 1)]
		[InlineData("66 4F AA", 3)]
		void Test64_Stosb_Yb_AL_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosb_Yb_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("AB", 1)]
		void Test16_Stosw_Yw_AX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AB", 2)]
		void Test16_Stosw_Yw_AX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 AB", 3)]
		void Test32_Stosw_Yw_AX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 AB", 2)]
		void Test32_Stosw_Yw_AX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 AB", 3)]
		[InlineData("66 67 47 AB", 4)]
		void Test64_Stosw_Yw_AX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 AB", 2)]
		[InlineData("66 47 AB", 3)]
		void Test64_Stosw_Yw_AX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosw_Yw_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 AB", 2)]
		void Test16_Stosd_Yd_EAX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 AB", 3)]
		void Test16_Stosd_Yd_EAX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AB", 2)]
		void Test32_Stosd_Yd_EAX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("AB", 1)]
		void Test32_Stosd_Yd_EAX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 AB", 2)]
		[InlineData("67 47 AB", 3)]
		void Test64_Stosd_Yd_EAX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("AB", 1)]
		[InlineData("47 AB", 2)]
		void Test64_Stosd_Yd_EAX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosd_Yd_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 48 AB", 3)]
		[InlineData("67 4F AB", 3)]
		void Test64_Stosq_Yq_RAX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosq_Yq_RAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 AB", 2)]
		[InlineData("4F AB", 2)]
		void Test64_Stosq_Yq_RAX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Stosq_Yq_RAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("AC", 1, Register.DS, Register.None)]
		[InlineData("26 AC", 2, Register.ES, Register.ES)]
		[InlineData("2E AC", 2, Register.CS, Register.CS)]
		[InlineData("36 AC", 2, Register.SS, Register.SS)]
		[InlineData("3E AC", 2, Register.DS, Register.DS)]
		[InlineData("64 AC", 2, Register.FS, Register.FS)]
		[InlineData("65 AC", 2, Register.GS, Register.GS)]
		void Test16_Lodsb_AL_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AC", 2, Register.DS, Register.None)]
		[InlineData("26 67 AC", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AC", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AC", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AC", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AC", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AC", 3, Register.GS, Register.GS)]
		void Test16_Lodsb_AL_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AC", 2, Register.DS, Register.None)]
		[InlineData("26 67 AC", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AC", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AC", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AC", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AC", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AC", 3, Register.GS, Register.GS)]
		void Test32_Lodsb_AL_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AC", 1, Register.DS, Register.None)]
		[InlineData("26 AC", 2, Register.ES, Register.ES)]
		[InlineData("2E AC", 2, Register.CS, Register.CS)]
		[InlineData("36 AC", 2, Register.SS, Register.SS)]
		[InlineData("3E AC", 2, Register.DS, Register.DS)]
		[InlineData("64 AC", 2, Register.FS, Register.FS)]
		[InlineData("65 AC", 2, Register.GS, Register.GS)]
		void Test32_Lodsb_AL_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AC", 2, Register.DS, Register.None)]
		[InlineData("26 67 AC", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AC", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AC", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AC", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AC", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AC", 3, Register.GS, Register.GS)]
		[InlineData("66 67 4F AC", 4, Register.DS, Register.None)]
		void Test64_Lodsb_AL_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AC", 1, Register.DS, Register.None)]
		[InlineData("26 AC", 2, Register.ES, Register.ES)]
		[InlineData("2E AC", 2, Register.CS, Register.CS)]
		[InlineData("36 AC", 2, Register.SS, Register.SS)]
		[InlineData("3E AC", 2, Register.DS, Register.DS)]
		[InlineData("64 AC", 2, Register.FS, Register.FS)]
		[InlineData("65 AC", 2, Register.GS, Register.GS)]
		[InlineData("66 4F AC", 3, Register.DS, Register.None)]
		void Test64_Lodsb_AL_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsb_AL_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AD", 1, Register.DS, Register.None)]
		[InlineData("26 AD", 2, Register.ES, Register.ES)]
		[InlineData("2E AD", 2, Register.CS, Register.CS)]
		[InlineData("36 AD", 2, Register.SS, Register.SS)]
		[InlineData("3E AD", 2, Register.DS, Register.DS)]
		[InlineData("64 AD", 2, Register.FS, Register.FS)]
		[InlineData("65 AD", 2, Register.GS, Register.GS)]
		void Test16_Lodsw_AX_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AD", 2, Register.DS, Register.None)]
		[InlineData("26 67 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AD", 3, Register.GS, Register.GS)]
		void Test16_Lodsw_AX_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 AD", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 AD", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 AD", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 AD", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 AD", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 AD", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 AD", 4, Register.GS, Register.GS)]
		void Test32_Lodsw_AX_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 AD", 2, Register.DS, Register.None)]
		[InlineData("26 66 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 66 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 66 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 66 AD", 3, Register.GS, Register.GS)]
		void Test32_Lodsw_AX_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 AD", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 AD", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 AD", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 AD", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 AD", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 AD", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 AD", 4, Register.GS, Register.GS)]
		[InlineData("66 67 47 AD", 4, Register.DS, Register.None)]
		void Test64_Lodsw_AX_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 AD", 2, Register.DS, Register.None)]
		[InlineData("26 66 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 66 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 66 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 66 AD", 3, Register.GS, Register.GS)]
		[InlineData("66 47 AD", 3, Register.DS, Register.None)]
		void Test64_Lodsw_AX_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsw_AX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 AD", 2, Register.DS, Register.None)]
		[InlineData("26 66 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 66 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 66 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 66 AD", 3, Register.GS, Register.GS)]
		void Test16_Lodsd_EAX_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 AD", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 AD", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 AD", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 AD", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 AD", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 AD", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 AD", 4, Register.GS, Register.GS)]
		void Test16_Lodsd_EAX_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AD", 2, Register.DS, Register.None)]
		[InlineData("26 67 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AD", 3, Register.GS, Register.GS)]
		void Test32_Lodsd_EAX_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AD", 1, Register.DS, Register.None)]
		[InlineData("26 AD", 2, Register.ES, Register.ES)]
		[InlineData("2E AD", 2, Register.CS, Register.CS)]
		[InlineData("36 AD", 2, Register.SS, Register.SS)]
		[InlineData("3E AD", 2, Register.DS, Register.DS)]
		[InlineData("64 AD", 2, Register.FS, Register.FS)]
		[InlineData("65 AD", 2, Register.GS, Register.GS)]
		void Test32_Lodsd_EAX_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 AD", 2, Register.DS, Register.None)]
		[InlineData("26 67 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 67 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 67 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 67 AD", 3, Register.GS, Register.GS)]
		[InlineData("67 47 AD", 3, Register.DS, Register.None)]
		void Test64_Lodsd_EAX_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AD", 1, Register.DS, Register.None)]
		[InlineData("26 AD", 2, Register.ES, Register.ES)]
		[InlineData("2E AD", 2, Register.CS, Register.CS)]
		[InlineData("36 AD", 2, Register.SS, Register.SS)]
		[InlineData("3E AD", 2, Register.DS, Register.DS)]
		[InlineData("64 AD", 2, Register.FS, Register.FS)]
		[InlineData("65 AD", 2, Register.GS, Register.GS)]
		[InlineData("47 AD", 2, Register.DS, Register.None)]
		void Test64_Lodsd_EAX_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsd_EAX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 48 AD", 3, Register.DS, Register.None)]
		[InlineData("26 67 48 AD", 4, Register.ES, Register.ES)]
		[InlineData("2E 67 48 AD", 4, Register.CS, Register.CS)]
		[InlineData("36 67 48 AD", 4, Register.SS, Register.SS)]
		[InlineData("3E 67 48 AD", 4, Register.DS, Register.DS)]
		[InlineData("64 67 48 AD", 4, Register.FS, Register.FS)]
		[InlineData("65 67 48 AD", 4, Register.GS, Register.GS)]
		[InlineData("67 4F AD", 3, Register.DS, Register.None)]
		void Test64_Lodsq_RAX_Xq_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsq_RAX_Xq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("48 AD", 2, Register.DS, Register.None)]
		[InlineData("26 48 AD", 3, Register.ES, Register.ES)]
		[InlineData("2E 48 AD", 3, Register.CS, Register.CS)]
		[InlineData("36 48 AD", 3, Register.SS, Register.SS)]
		[InlineData("3E 48 AD", 3, Register.DS, Register.DS)]
		[InlineData("64 48 AD", 3, Register.FS, Register.FS)]
		[InlineData("65 48 AD", 3, Register.GS, Register.GS)]
		[InlineData("4F AD", 2, Register.DS, Register.None)]
		void Test64_Lodsq_RAX_Xq_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lodsq_RAX_Xq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("AE", 1)]
		void Test16_Scasb_AL_Yb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AE", 2)]
		void Test16_Scasb_AL_Yb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AE", 2)]
		void Test32_Scasb_AL_Yb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("AE", 1)]
		void Test32_Scasb_AL_Yb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AE", 2)]
		[InlineData("66 67 4F AE", 4)]
		void Test64_Scasb_AL_Yb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("AE", 1)]
		[InlineData("66 4F AE", 3)]
		void Test64_Scasb_AL_Yb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasb_AL_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("AF", 1)]
		void Test16_Scasw_AX_Yw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AF", 2)]
		void Test16_Scasw_AX_Yw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 AF", 3)]
		void Test32_Scasw_AX_Yw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 AF", 2)]
		void Test32_Scasw_AX_Yw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 AF", 3)]
		[InlineData("66 67 47 AF", 4)]
		void Test64_Scasw_AX_Yw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 AF", 2)]
		[InlineData("66 47 AF", 3)]
		void Test64_Scasw_AX_Yw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasw_AX_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 AF", 2)]
		void Test16_Scasd_EAX_Yd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 AF", 3)]
		void Test16_Scasd_EAX_Yd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AF", 2)]
		void Test32_Scasd_EAX_Yd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("AF", 1)]
		void Test32_Scasd_EAX_Yd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 AF", 2)]
		[InlineData("67 47 AF", 3)]
		void Test64_Scasd_EAX_Yd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("AF", 1)]
		[InlineData("47 AF", 2)]
		void Test64_Scasd_EAX_Yd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasd_EAX_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 48 AF", 3)]
		[InlineData("67 4F AF", 3)]
		void Test64_Scasq_RAX_Yq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasq_RAX_Yq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
		}

		[Theory]
		[InlineData("48 AF", 2)]
		[InlineData("4F AF", 2)]
		void Test64_Scasq_RAX_Yq_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Scasq_RAX_Yq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
		}
	}
}
