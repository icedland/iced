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
	public sealed class DecoderTest_1_C8_CF : DecoderTest {
		[Theory]
		[InlineData("C8 5AA5 A6", 4, 0xA55A, 0xA6)]
		[InlineData("C8 A55A 6A", 4, 0x5AA5, 0x6A)]
		void Test16_Enterw_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterw_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Theory]
		[InlineData("66 C8 5AA5 A6", 5, 0xA55A, 0xA6)]
		[InlineData("66 C8 A55A 6A", 5, 0x5AA5, 0x6A)]
		void Test32_Enterw_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterw_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Theory]
		[InlineData("66 C8 5AA5 A6", 5, 0xA55A, 0xA6)]
		[InlineData("66 C8 A55A 6A", 5, 0x5AA5, 0x6A)]
		[InlineData("66 47 C8 5AA5 A6", 6, 0xA55A, 0xA6)]
		[InlineData("66 47 C8 A55A 6A", 6, 0x5AA5, 0x6A)]
		void Test64_Enterw_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterw_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Theory]
		[InlineData("66 C8 5AA5 A6", 5, 0xA55A, 0xA6)]
		[InlineData("66 C8 A55A 6A", 5, 0x5AA5, 0x6A)]
		void Test16_Enterd_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterd_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Theory]
		[InlineData("C8 5AA5 A6", 4, 0xA55A, 0xA6)]
		[InlineData("C8 A55A 6A", 4, 0x5AA5, 0x6A)]
		void Test32_Enterd_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterd_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Theory]
		[InlineData("C8 5AA5 A6", 4, 0xA55A, 0xA6)]
		[InlineData("C8 A55A 6A", 4, 0x5AA5, 0x6A)]
		[InlineData("47 C8 5AA5 A6", 5, 0xA55A, 0xA6)]
		[InlineData("47 C8 A55A 6A", 5, 0x5AA5, 0x6A)]
		[InlineData("4F C8 5AA5 A6", 5, 0xA55A, 0xA6)]
		[InlineData("4F C8 A55A 6A", 5, 0x5AA5, 0x6A)]
		void Test64_Enterq_Iw_Ib_1(string hexBytes, int byteLength, ushort immediate16, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Enterq_Iw_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate16, instr.Immediate16);

			Assert.Equal(OpKind.Immediate8_2nd, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8_2nd);
		}

		[Fact]
		void Test16_Leavew_1() {
			var decoder = CreateDecoder16("C9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Leavew, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Leavew_1() {
			var decoder = CreateDecoder32("66 C9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Leavew, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("66 C9", 2)]
		[InlineData("66 47 C9", 3)]
		void Test64_Leavew_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Leavew, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Leaved_1() {
			var decoder = CreateDecoder16("66 C9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Leaved, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Leaved_1() {
			var decoder = CreateDecoder32("C9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Leaved, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("C9", 1)]
		[InlineData("47 C9", 2)]
		[InlineData("4F C9", 2)]
		void Test64_Leaveq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Leaveq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("CA 5AA5", 3, 0xA55A)]
		[InlineData("CA A55A", 3, 0x5AA5)]
		void Test16_Retfw_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 CA 5AA5", 4, 0xA55A)]
		[InlineData("66 CA A55A", 4, 0x5AA5)]
		void Test32_Retfw_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 CA 5AA5", 4, 0xA55A)]
		[InlineData("66 CA A55A", 4, 0x5AA5)]
		[InlineData("66 47 CA 5AA5", 5, 0xA55A)]
		[InlineData("66 47 CA A55A", 5, 0x5AA5)]
		void Test64_Retfw_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 CA 5AA5", 4, 0xA55A)]
		[InlineData("66 CA A55A", 4, 0x5AA5)]
		void Test16_Retfd_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("CA 5AA5", 3, 0xA55A)]
		[InlineData("CA A55A", 3, 0x5AA5)]
		void Test32_Retfd_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("CA 5AA5", 3, 0xA55A)]
		[InlineData("CA A55A", 3, 0x5AA5)]
		[InlineData("47 CA 5AA5", 4, 0xA55A)]
		[InlineData("47 CA A55A", 4, 0x5AA5)]
		void Test64_Retfd_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("48 CA 5AA5", 4, 0xA55A)]
		[InlineData("48 CA A55A", 4, 0x5AA5)]
		[InlineData("4F CA 5AA5", 4, 0xA55A)]
		[InlineData("4F CA A55A", 4, 0x5AA5)]
		void Test64_Retfq_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfq_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Fact]
		void Test16_Retfw_1() {
			var decoder = CreateDecoder16("CB");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Retfw_1() {
			var decoder = CreateDecoder32("66 CB");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("66 CB", 2)]
		[InlineData("66 47 CB", 3)]
		void Test64_Retfw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Retfd_1() {
			var decoder = CreateDecoder16("66 CB");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Retfd_1() {
			var decoder = CreateDecoder32("CB");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("CB", 1)]
		[InlineData("47 CB", 2)]
		void Test64_Retfd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("48 CB", 2)]
		[InlineData("4F CB", 2)]
		void Test64_Retfq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retfq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Int3_1() {
			var decoder = CreateDecoder16("CC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Int3, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Int3_1() {
			var decoder = CreateDecoder32("CC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Int3, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test64_Int3_1() {
			var decoder = CreateDecoder64("CC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Int3, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("CD 5A", 2, 0x5A)]
		[InlineData("CD A5", 2, 0xA5)]
		void Test16_Int_Ib_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Int_Ib, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("CD 5A", 2, 0x5A)]
		[InlineData("CD A5", 2, 0xA5)]
		void Test32_Int_Ib_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Int_Ib, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("CD 5A", 2, 0x5A)]
		[InlineData("CD A5", 2, 0xA5)]
		void Test64_Int_Ib_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Int_Ib, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Fact]
		void Test16_Into_1() {
			var decoder = CreateDecoder16("CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Into, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Into_1() {
			var decoder = CreateDecoder32("CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Into, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Iretw_1() {
			var decoder = CreateDecoder16("CF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Iretw_1() {
			var decoder = CreateDecoder32("66 CF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("66 CF", 2)]
		[InlineData("66 47 CF", 3)]
		void Test64_Iretw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Iretd_1() {
			var decoder = CreateDecoder16("66 CF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Iretd_1() {
			var decoder = CreateDecoder32("CF");
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("CF", 1)]
		[InlineData("47 CF", 2)]
		void Test64_Iretd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("48 CF", 2)]
		[InlineData("4F CF", 2)]
		void Test64_Iretq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Iretq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
	}
}
