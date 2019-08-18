/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MiscTests : DecoderTest {
		[Fact]
		void Verify_NumberOfCodeValues() {
			int numValues = -1;
			foreach (var f in typeof(Code).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numValues + 1, value);
					numValues = value;
				}
			}
			numValues++;
			Assert.Equal(Iced.Intel.DecoderConstants.NumberOfCodeValues, numValues);
		}

		[Fact]
		void Verify_NumberOfRegisters() {
			int numValues = -1;
			foreach (var f in typeof(Register).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numValues + 1, value);
					numValues = value;
				}
			}
			numValues++;
			Assert.Equal(Iced.Intel.DecoderConstants.NumberOfRegisters, numValues);
		}

		[Fact]
		void Verify_NumberOfMemorySizes() {
			int numValues = -1;
			foreach (var f in typeof(MemorySize).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numValues + 1, value);
					numValues = value;
				}
			}
			numValues++;
			Assert.Equal(Iced.Intel.DecoderConstants.NumberOfMemorySizes, numValues);
		}

		[Fact]
		void Test16_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder16("26 26 26 26 26 26 26 26 26 26 26 26 26 66 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test16_almost_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder16("26 26 26 26 26 26 26 26 26 26 26 26 66 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_rm32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.ES, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test32_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder32("26 26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test32_almost_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder32("26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_rm32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.ES, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test64_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder64("26 26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.INVALID, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Fact]
		void Test64_almost_too_long_instruction() {
			Assert.Equal(15, Iced.Intel.DecoderConstants.MaxInstructionLength);

			var decoder = CreateDecoder64("26 26 26 26 26 26 26 26 26 26 26 26 26 01 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Add_rm32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(15, instr.ByteLength);
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.ES, instr.SegmentPrefix);

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
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
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
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
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
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
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
			var decoderDict16 = new Dictionary<DecoderOptions, Decoder>();
			var decoderDict32 = new Dictionary<DecoderOptions, Decoder>();
			var decoderDict64 = new Dictionary<DecoderOptions, Decoder>();
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: true)) {
				var data = HexUtils.ToByteArray(info.HexBytes);
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(data), info.Options);
				Decoder decoderAll;
				switch (info.Bitness) {
				case 16:
					decoder.IP = DecoderConstants.DEFAULT_IP16;
					reader16.SetArray(data);
					if (!decoderDict16.TryGetValue(info.Options, out decoderAll))
						decoderDict16.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader16, info.Options));
					break;
				case 32:
					decoder.IP = DecoderConstants.DEFAULT_IP32;
					reader32.SetArray(data);
					if (!decoderDict32.TryGetValue(info.Options, out decoderAll))
						decoderDict32.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader32, info.Options));
					break;
				case 64:
					decoder.IP = DecoderConstants.DEFAULT_IP64;
					reader64.SetArray(data);
					if (!decoderDict64.TryGetValue(info.Options, out decoderAll))
						decoderDict64.Add(info.Options, decoderAll = Decoder.Create(info.Bitness, reader64, info.Options));
					break;
				default:
					throw new InvalidOperationException();
				}
				decoderAll.IP = decoder.IP;
				var instr1 = decoder.Decode();
				var instr2 = decoderAll.Decode();
				Assert.Equal(info.Code, instr1.Code);
				Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
			}
		}

		[Theory]
		[MemberData(nameof(Test_all_mandatory_prefixes_Data))]
		void Test64_all_mandatory_prefixes(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal(byteLength, instr.ByteLength);
		}
		public static IEnumerable<object[]> Test_all_mandatory_prefixes_Data {
			get {
				yield return new object[] { "0F10 08", 3, Code.Movups_xmm_xmmm128 };
				yield return new object[] { "66 0F10 08", 4, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "F2 0F10 08", 4, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 0F10 08", 4, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 66 0F10 08", 5, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "66 F2 0F10 08", 5, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F3 0F10 08", 5, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 66 0F10 08", 5, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 0F10 08", 5, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F3 0F10 08", 5, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 0F10 08", 5, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F2 0F10 08", 5, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F3 0F10 08", 5, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 66 66 0F10 08", 6, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "66 66 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 66 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F2 66 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F2 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F2 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F3 66 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F3 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F3 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 66 66 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 66 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 66 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F2 66 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F3 66 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F3 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F3 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 66 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 66 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F2 66 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F2 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F2 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F3 66 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F3 F2 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F3 F3 0F10 08", 6, Code.Movss_xmm_xmmm32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_all_mandatory_prefixes_segoverride_Data))]
		void Test64_all_mandatory_prefixes_segoverride(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal(byteLength, instr.ByteLength);
		}
		public static IEnumerable<object[]> Test64_all_mandatory_prefixes_segoverride_Data {
			get {
				yield return new object[] { "64 0F10 08", 4, Code.Movups_xmm_xmmm128 };
				yield return new object[] { "66 64 0F10 08", 5, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "F2 64 0F10 08", 5, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 64 0F10 08", 5, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 66 64 0F10 08", 6, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "66 F2 64 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F3 64 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 66 64 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 64 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F3 64 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 64 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F2 64 0F10 08", 6, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F3 64 0F10 08", 6, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 66 66 64 0F10 08", 7, Code.Movupd_xmm_xmmm128 };
				yield return new object[] { "66 66 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 66 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F2 66 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F2 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F2 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F3 66 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "66 F3 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "66 F3 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 66 66 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 66 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 66 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F2 66 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F2 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F3 66 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F2 F3 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F2 F3 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 66 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 66 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 66 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F2 66 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F2 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F2 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F3 66 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
				yield return new object[] { "F3 F3 F2 64 0F10 08", 7, Code.Movsd_xmm_xmmm64 };
				yield return new object[] { "F3 F3 F3 64 0F10 08", 7, Code.Movss_xmm_xmmm32 };
			}
		}
	}
}
