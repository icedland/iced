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

using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MiscTests : DecoderTest {
		[Fact]
		void Test16_too_long_instruction() {
			var decoder = CreateDecoder16("26 26 26 26 26 26 26 26 26 26 26 26 26 66 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_almost_too_long_instruction() {
			var decoder = CreateDecoder16("26 26 26 26 26 26 26 26 26 26 26 26 66 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.ES, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test32_too_long_instruction() {
			var decoder = CreateDecoder32("26 26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_almost_too_long_instruction() {
			var decoder = CreateDecoder32("26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.ES, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test64_too_long_instruction() {
			var decoder = CreateDecoder64("26 26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test64_almost_too_long_instruction() {
			var decoder = CreateDecoder64("26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.ES, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Theory]
		[InlineData("", 0)]
		[InlineData("66", 1)]
		[InlineData("01", 1)]
		void Test16_too_short_instruction(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("", 0)]
		[InlineData("66", 1)]
		[InlineData("01", 1)]
		void Test32_too_short_instruction(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("", 0)]
		[InlineData("66", 1)]
		[InlineData("01", 1)]
		void Test64_too_short_instruction(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		sealed class DecodeMultipleCodeReader : CodeReader {
			byte[] data;
			int offset;

			public void SetArray(byte[] data) {
				this.data = data;
				offset = 0;
			}

			public override int ReadByte() {
				if (offset >= data.Length)
					return -1;
				return data[offset++];
			}
		}

		[Fact]
		void Decode_multiple_instrs_with_one_instance() {
			var reader16 = new DecodeMultipleCodeReader();
			var reader32 = new DecodeMultipleCodeReader();
			var reader64 = new DecodeMultipleCodeReader();
			var decoderAll16 = Decoder.Create16(reader16);
			var decoderAll32 = Decoder.Create32(reader32);
			var decoderAll64 = Decoder.Create64(reader64);
			foreach (var info in DecoderTestUtils.GetDecoderTests(needHexBytes: true, includeOtherTests: false)) {
				var data = HexUtils.ToByteArray(info.HexBytes);
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(data));
				Decoder decoderAll;
				switch (info.Bitness) {
				case 16:
					decoder.InstructionPointer = DecoderConstants.DEFAULT_IP16;
					reader16.SetArray(data);
					decoderAll = decoderAll16;
					break;
				case 32:
					decoder.InstructionPointer = DecoderConstants.DEFAULT_IP32;
					reader32.SetArray(data);
					decoderAll = decoderAll32;
					break;
				case 64:
					decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64;
					reader64.SetArray(data);
					decoderAll = decoderAll64;
					break;
				default:
					throw new InvalidOperationException();
				}
				decoderAll.InstructionPointer = decoder.InstructionPointer;
				var instr1 = decoder.Decode();
				var instr2 = decoderAll.Decode();
				Assert.Equal(info.Code, instr1.Code);
				Assert.True(Instruction.TEST_BitByBitEquals(ref instr1, ref instr2));
			}
		}
	}
}
