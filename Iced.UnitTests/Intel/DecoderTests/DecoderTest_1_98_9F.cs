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
	public sealed class DecoderTest_1_98_9F : DecoderTest {
		[Fact]
		void Test16_Cbw_1() {
			var decoder = CreateDecoder16("98");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cbw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Cbw_1() {
			var decoder = CreateDecoder32("66 98");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cbw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("66 98", 2)]
		[InlineData("66 47 98", 3)]
		void Test64_Cbw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cbw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Cwde_1() {
			var decoder = CreateDecoder16("66 98");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwde, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Cwde_1() {
			var decoder = CreateDecoder32("98");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwde, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("98", 1)]
		[InlineData("47 98", 2)]
		void Test64_Cwde_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwde, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("48 98", 2)]
		[InlineData("4F 98", 2)]
		void Test64_Cdqe_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cdqe, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Cwd_1() {
			var decoder = CreateDecoder16("99");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Cwd_1() {
			var decoder = CreateDecoder32("66 99");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("66 99", 2)]
		[InlineData("66 47 99", 3)]
		void Test64_Cwd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cwd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Cdq_1() {
			var decoder = CreateDecoder16("66 99");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cdq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Cdq_1() {
			var decoder = CreateDecoder32("99");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cdq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("99", 1)]
		[InlineData("47 99", 2)]
		void Test64_Cdq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cdq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("48 99", 2)]
		[InlineData("4F 99", 2)]
		void Test64_Cqo_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cqo, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Call_ptr1616_1() {
			var decoder = CreateDecoder16("9A 1234 5678");
			var instr = decoder.Decode();

			Assert.Equal(Code.Call_ptr1616, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.FarBranch16, instr.Op0Kind);
			Assert.Equal(0x3412, instr.FarBranch16);
			Assert.Equal(0x7856, instr.FarBranchSelector);
		}

		[Fact]
		void Test32_Call_ptr1616_1() {
			var decoder = CreateDecoder32("66 9A 1234 5678");
			var instr = decoder.Decode();

			Assert.Equal(Code.Call_ptr1616, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.FarBranch16, instr.Op0Kind);
			Assert.Equal(0x3412, instr.FarBranch16);
			Assert.Equal(0x7856, instr.FarBranchSelector);
		}

		[Fact]
		void Test16_Call_ptr3216_1() {
			var decoder = CreateDecoder16("66 9A 12345678 9ABC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Call_ptr3216, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(8, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.FarBranch32, instr.Op0Kind);
			Assert.Equal(0x78563412U, instr.FarBranch32);
			Assert.Equal(0xBC9A, instr.FarBranchSelector);
		}

		[Fact]
		void Test32_Call_ptr3216_1() {
			var decoder = CreateDecoder32("9A 12345678 9ABC");
			var instr = decoder.Decode();

			Assert.Equal(Code.Call_ptr3216, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(7, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.FarBranch32, instr.Op0Kind);
			Assert.Equal(0x78563412U, instr.FarBranch32);
			Assert.Equal(0xBC9A, instr.FarBranchSelector);
		}

		[Theory]
		[InlineData("9B", 1)]
		[InlineData("66 9B", 2)]
		void Test16_Wait_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Wait, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9B", 1)]
		[InlineData("66 9B", 2)]
		void Test32_Wait_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Wait, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9B", 1)]
		[InlineData("66 9B", 2)]
		[InlineData("4F 9B", 2)]
		[InlineData("66 4F 9B", 3)]
		void Test64_Wait_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Wait, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Pushfw_1() {
			var decoder = CreateDecoder16("9C");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Pushfw_1() {
			var decoder = CreateDecoder32("66 9C");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("66 9C", 2)]
		[InlineData("66 47 9C", 3)]
		void Test64_Pushfw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Pushfd_1() {
			var decoder = CreateDecoder16("66 9C");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Pushfd_1() {
			var decoder = CreateDecoder32("9C");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test64_Pushfq_1() {
			var decoder = CreateDecoder64("9C");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushfq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Popfw_1() {
			var decoder = CreateDecoder16("9D");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Popfw_1() {
			var decoder = CreateDecoder32("66 9D");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("66 9D", 2)]
		[InlineData("66 47 9D", 3)]
		void Test64_Popfw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_Popfd_1() {
			var decoder = CreateDecoder16("66 9D");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_Popfd_1() {
			var decoder = CreateDecoder32("9D");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test64_Popfq_1() {
			var decoder = CreateDecoder64("9D");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popfq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9E", 1)]
		[InlineData("66 9E", 2)]
		void Test16_Sahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Sahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9E", 1)]
		[InlineData("66 9E", 2)]
		void Test32_Sahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Sahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9E", 1)]
		[InlineData("66 9E", 2)]
		[InlineData("4F 9E", 2)]
		[InlineData("66 4F 9E", 3)]
		void Test64_Sahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Sahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9F", 1)]
		[InlineData("66 9F", 2)]
		void Test16_Lahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9F", 1)]
		[InlineData("66 9F", 2)]
		void Test32_Lahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("9F", 1)]
		[InlineData("66 9F", 2)]
		[InlineData("4F 9F", 2)]
		[InlineData("66 4F 9F", 3)]
		void Test64_Lahf_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lahf, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
	}
}
