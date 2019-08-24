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
using System.Diagnostics;
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

#if !NO_ENCODER
		[Fact]
		void Verify_invalid_and_valid_lock_prefix() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				bool hasLock;
				bool canUseLock;

				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
					decoder.Decode(out var instr);
					Assert.Equal(info.Code, instr.Code);
					hasLock = instr.HasLockPrefix;
					var opCode = info.Code.ToOpCode();
					canUseLock = opCode.CanUseLockPrefix && HasModRMMemoryOperand(instr);

					switch (info.Code) {
					case Code.Mov_r32_cr:
					case Code.Mov_r64_cr:
						if (info.HexBytes == "F0 0F20 C1")
							continue;
						break;
					case Code.Mov_cr_r32:
					case Code.Mov_cr_r64:
						if (info.HexBytes == "F0 0F22 C1")
							continue;
						break;
					}
				}

				if (canUseLock) {
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options);
					decoder.Decode(out var instr);
					Assert.Equal(info.Code, instr.Code);
					Assert.True(instr.HasLockPrefix);
				}
				else {
					Debug.Assert(!hasLock);
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(Code.INVALID, instr.Code);
						Assert.False(instr.HasLockPrefix);
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(instr.HasLockPrefix);
					}
				}
			}

			static string AddLock(string hexBytes, bool hasLock) => hasLock ? hexBytes : "F0" + hexBytes;

			static bool HasModRMMemoryOperand(in Instruction instr) {
				int opCount = instr.OpCount;
				for (int i = 0; i < opCount; i++) {
					if (instr.GetOpKind(i) == OpKind.Memory)
						return true;
				}
				return false;
			}
		}
#endif

		[Fact]
		void Verify_invalid_REX_mandatory_prefixes_VEX_EVEX_XOP() {
			var prefixes1632 = new string[] { "66", "F3", "F2" };
			var prefixes64   = new string[] { "66", "F3", "F2",
											  "40", "41", "42", "43", "44", "45", "46", "47",
											  "48", "49", "4A", "4B", "4C", "4D", "4E", "4F" };
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				switch (info.Code.Encoding()) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					continue;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
					break;

				default:
					throw new InvalidOperationException();
				}

				string[] prefixes;
				switch (info.Bitness) {
				case 16:
				case 32:
					prefixes = prefixes1632;
					break;
				case 64:
					prefixes = prefixes64;
					break;
				default:
					throw new InvalidOperationException();
				}
				foreach (var prefix in prefixes) {
					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
						// Mandatory prefix must be right before the opcode. If it has a seg override, there's also
						// a test without a seg override so just skip this.
						if (origInstr.SegmentPrefix != Register.None)
							continue;
						int memRegSize = GetMemoryRegisterSize(origInstr);
						// 67h prefix
						if (memRegSize != 0 && memRegSize != info.Bitness)
							continue;
					}
					var hexBytes = prefix + info.HexBytes;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);

						instr.ByteLength--;
						instr.NextIP--;
						if (prefix == "F3") {
							Assert.True(instr.HasRepPrefix);
							Assert.True(instr.HasRepePrefix);
							instr.HasRepPrefix = false;
						}
						else if (prefix == "F2") {
							Assert.True(instr.HasRepnePrefix);
							instr.HasRepnePrefix = false;
						}
						Assert.True(Instruction.TEST_BitByBitEquals(instr, origInstr));
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(Code.INVALID, instr.Code);
					}
				}
			}
		}

		static int GetMemoryRegisterSize(in Instruction instr) {
			int opCount = instr.OpCount;
			for (int i = 0; i < opCount; i++) {
				switch (instr.GetOpKind(i)) {
				case OpKind.Register:
				case OpKind.NearBranch16:
				case OpKind.NearBranch32:
				case OpKind.NearBranch64:
				case OpKind.FarBranch16:
				case OpKind.FarBranch32:
				case OpKind.Immediate8:
				case OpKind.Immediate8_2nd:
				case OpKind.Immediate16:
				case OpKind.Immediate32:
				case OpKind.Immediate64:
				case OpKind.Immediate8to16:
				case OpKind.Immediate8to32:
				case OpKind.Immediate8to64:
				case OpKind.Immediate32to64:
					break;
				case OpKind.MemorySegSI:
				case OpKind.MemorySegDI:
				case OpKind.MemoryESDI:
					return 16;
				case OpKind.MemorySegESI:
				case OpKind.MemorySegEDI:
				case OpKind.MemoryESEDI:
					return 32;
				case OpKind.MemorySegRSI:
				case OpKind.MemorySegRDI:
				case OpKind.MemoryESRDI:
					return 64;
				case OpKind.Memory:
					var reg = instr.MemoryBase;
					if (reg == Register.None)
						reg = instr.MemoryIndex;
					if (reg != Register.None)
						return reg.GetInfo().Size * 8;
					if (instr.MemoryDisplSize == 4)
						return 32;
					if (instr.MemoryDisplSize == 8)
						return 64;
					break;
				case OpKind.Memory64:
					return 64;
				default:
					throw new InvalidOperationException();
				}
			}
			return 0;
		}

		[Fact]
		void Test_EVEX_reserved_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if (info.Code.Encoding() != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);
				for (int i = 1; i <= 3; i++) {
					bytes[evexIndex + 1] = (byte)((bytes[evexIndex + 1] & ~0x0C) | (i << 2));
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(Code.INVALID, instr.Code);
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options ^ DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instr);
						Assert.Equal(Code.INVALID, instr.Code);
					}
				}
			}
		}

		static int GetEvexIndex(byte[] bytes) {
			for (int i = 0; ; i++) {
				if (i >= bytes.Length)
					throw new InvalidOperationException();
				if (bytes[i] == 0x62)
					return i;
			}
		}

#if !NO_ENCODER
		static int GetVexXopIndex(byte[] bytes) {
			for (int i = 0; ; i++) {
				if (i >= bytes.Length)
					throw new InvalidOperationException();
				var b = bytes[i];
				if (b == 0xC4 || b == 0xC5 || b == 0x8F)
					return i;
			}
		}

		[Fact]
		void Test_WIG_instructions_ignore_W() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				var encoding = opCode.Encoding;
				bool isWIG = opCode.IsWIG || (opCode.IsWIG32 && info.Bitness != 64);
				if (encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);

					if (isWIG) {
						Instruction instr1, instr2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr1);
							Assert.Equal(info.Code, instr1.Code);
						}
						{
							bytes[evexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr2);
							Assert.Equal(info.Code, instr2.Code);
						}
						Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
						}
						{
							bytes[evexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.True(info.Code != instr.Code);
						}
					}
				}
				else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					if (bytes[vexIndex] == 0xC5)
						continue;

					if (isWIG) {
						Instruction instr1, instr2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr1);
							Assert.Equal(info.Code, instr1.Code);
						}
						{
							bytes[vexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr2);
							Assert.Equal(info.Code, instr2.Code);
						}
						Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
						}
						{
							bytes[vexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.True(info.Code != instr.Code);
						}
					}
				}
				else if (encoding == EncodingKind.Legacy || encoding == EncodingKind.D3NOW)
					continue;
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Test_LIG_instructions_ignore_L() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				var encoding = opCode.Encoding;
				if (encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);

					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					bool EVEX_b = (bytes[evexIndex + 3] & 0x10) != 0;
					if (opCode.CanUseRoundingControl && isRegOnly && EVEX_b)
						continue;
					bool isSae = opCode.CanSuppressAllExceptions && isRegOnly && EVEX_b;

					if (opCode.IsLIG) {
						Instruction instr1, instr2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr1);
							Assert.Equal(info.Code, instr1.Code);
						}
						var origByte = bytes[evexIndex + 3];
						for (int i = 1; i <= 3; i++) {
							bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr2);
							Assert.Equal(info.Code, instr2.Code);
							Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
						}
					}
					else {
						Instruction instr1;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr1);
							Assert.Equal(info.Code, instr1.Code);
						}
						var origByte = bytes[evexIndex + 3];
						for (int i = 1; i <= 3; i++) {
							bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr2);
							if (isSae) {
								Assert.Equal(info.Code, instr2.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
							}
							else
								Assert.False(info.Code == instr2.Code);
						}
					}
				}
				else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					int lIndex = bytes[vexIndex] == 0xC5 ? vexIndex + 1 : vexIndex + 2;

					if (opCode.IsLIG) {
						Instruction instr1, instr2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr1);
							Assert.Equal(info.Code, instr1.Code);
						}
						{
							bytes[lIndex] ^= 4;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instr2);
							Assert.Equal(info.Code, instr2.Code);
						}
						Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
						}
						{
							bytes[lIndex] ^= 4;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.True(info.Code != instr.Code);
						}
					}
				}
				else if (encoding == EncodingKind.Legacy || encoding == EncodingKind.D3NOW)
					continue;
				else
					throw new InvalidOperationException();
			}
		}

		static bool MustUseNonZeroOpMaskRegister(OpCodeInfo opCode) {
			switch (opCode.Code) {
			case Code.EVEX_Vpgatherdd_xmm_k1_vm32x:
			case Code.EVEX_Vpgatherdd_ymm_k1_vm32y:
			case Code.EVEX_Vpgatherdd_zmm_k1_vm32z:
			case Code.EVEX_Vpgatherdq_xmm_k1_vm32x:
			case Code.EVEX_Vpgatherdq_ymm_k1_vm32x:
			case Code.EVEX_Vpgatherdq_zmm_k1_vm32y:
			case Code.EVEX_Vpgatherqd_xmm_k1_vm64x:
			case Code.EVEX_Vpgatherqd_xmm_k1_vm64y:
			case Code.EVEX_Vpgatherqd_ymm_k1_vm64z:
			case Code.EVEX_Vpgatherqq_xmm_k1_vm64x:
			case Code.EVEX_Vpgatherqq_ymm_k1_vm64y:
			case Code.EVEX_Vpgatherqq_zmm_k1_vm64z:
			case Code.EVEX_Vgatherdps_xmm_k1_vm32x:
			case Code.EVEX_Vgatherdps_ymm_k1_vm32y:
			case Code.EVEX_Vgatherdps_zmm_k1_vm32z:
			case Code.EVEX_Vgatherdpd_xmm_k1_vm32x:
			case Code.EVEX_Vgatherdpd_ymm_k1_vm32x:
			case Code.EVEX_Vgatherdpd_zmm_k1_vm32y:
			case Code.EVEX_Vgatherqps_xmm_k1_vm64x:
			case Code.EVEX_Vgatherqps_xmm_k1_vm64y:
			case Code.EVEX_Vgatherqps_ymm_k1_vm64z:
			case Code.EVEX_Vgatherqpd_xmm_k1_vm64x:
			case Code.EVEX_Vgatherqpd_ymm_k1_vm64y:
			case Code.EVEX_Vgatherqpd_zmm_k1_vm64z:
			case Code.EVEX_Vpscatterdd_vm32x_k1_xmm:
			case Code.EVEX_Vpscatterdd_vm32y_k1_ymm:
			case Code.EVEX_Vpscatterdd_vm32z_k1_zmm:
			case Code.EVEX_Vpscatterdq_vm32x_k1_xmm:
			case Code.EVEX_Vpscatterdq_vm32x_k1_ymm:
			case Code.EVEX_Vpscatterdq_vm32y_k1_zmm:
			case Code.EVEX_Vpscatterqd_vm64x_k1_xmm:
			case Code.EVEX_Vpscatterqd_vm64y_k1_xmm:
			case Code.EVEX_Vpscatterqd_vm64z_k1_ymm:
			case Code.EVEX_Vpscatterqq_vm64x_k1_xmm:
			case Code.EVEX_Vpscatterqq_vm64y_k1_ymm:
			case Code.EVEX_Vpscatterqq_vm64z_k1_zmm:
			case Code.EVEX_Vscatterdps_vm32x_k1_xmm:
			case Code.EVEX_Vscatterdps_vm32y_k1_ymm:
			case Code.EVEX_Vscatterdps_vm32z_k1_zmm:
			case Code.EVEX_Vscatterdpd_vm32x_k1_xmm:
			case Code.EVEX_Vscatterdpd_vm32x_k1_ymm:
			case Code.EVEX_Vscatterdpd_vm32y_k1_zmm:
			case Code.EVEX_Vscatterqps_vm64x_k1_xmm:
			case Code.EVEX_Vscatterqps_vm64y_k1_xmm:
			case Code.EVEX_Vscatterqps_vm64z_k1_ymm:
			case Code.EVEX_Vscatterqpd_vm64x_k1_xmm:
			case Code.EVEX_Vscatterqpd_vm64y_k1_ymm:
			case Code.EVEX_Vscatterqpd_vm64z_k1_zmm:
			case Code.EVEX_Vgatherpf0dps_vm32z_k1:
			case Code.EVEX_Vgatherpf0dpd_vm32y_k1:
			case Code.EVEX_Vgatherpf1dps_vm32z_k1:
			case Code.EVEX_Vgatherpf1dpd_vm32y_k1:
			case Code.EVEX_Vscatterpf0dps_vm32z_k1:
			case Code.EVEX_Vscatterpf0dpd_vm32y_k1:
			case Code.EVEX_Vscatterpf1dps_vm32z_k1:
			case Code.EVEX_Vscatterpf1dpd_vm32y_k1:
			case Code.EVEX_Vgatherpf0qps_vm64z_k1:
			case Code.EVEX_Vgatherpf0qpd_vm64z_k1:
			case Code.EVEX_Vgatherpf1qps_vm64z_k1:
			case Code.EVEX_Vgatherpf1qpd_vm64z_k1:
			case Code.EVEX_Vscatterpf0qps_vm64z_k1:
			case Code.EVEX_Vscatterpf0qpd_vm64z_k1:
			case Code.EVEX_Vscatterpf1qps_vm64z_k1:
			case Code.EVEX_Vscatterpf1qpd_vm64z_k1:
				return true;
			default:
				return false;
			}
		}

		[Fact]
		void Test_EVEX_k1_z_bits() {
			var p2Values_k1z = new (bool valid, byte bits)[] { (true, 0x00), (true, 0x01), (false, 0x80), (true, 0x86) };
			var p2Values_k1 = new (bool valid, byte bits)[] { (true, 0x00), (true, 0x01), (false, 0x80), (false, 0x86) };
			var p2Values_k1_fk = new (bool valid, byte bits)[] { (false, 0x00), (true, 0x01), (false, 0x80), (false, 0x86) };
			var p2Values_nothing = new (bool valid, byte bits)[] { (true, 0x00), (false, 0x01), (false, 0x80), (false, 0x86) };
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);
				(bool valid, byte bits)[] p2Values;
				if (opCode.CanUseZeroingMasking) {
					Assert.True(opCode.CanUseOpMaskRegister);
					p2Values = p2Values_k1z;
				}
				else if (opCode.CanUseOpMaskRegister) {
					if (MustUseNonZeroOpMaskRegister(opCode))
						p2Values = p2Values_k1_fk;
					else
						p2Values = p2Values_k1;
				}
				else
					p2Values = p2Values_nothing;

				var b = bytes[evexIndex + 3];
				foreach (var p2v in p2Values) {
					for (int i = 0; i < 2; i++) {
						bytes[evexIndex + 3] = (byte)((b & ~0x87U) | p2v.bits);
						var options = info.Options;
						if (i == 1)
							options |= DecoderOptions.NoInvalidCheck;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), options);
						decoder.Decode(out var instr);
						if (p2v.valid || (options & DecoderOptions.NoInvalidCheck) != 0) {
							Assert.Equal(info.Code, instr.Code);
							Assert.Equal((p2v.bits & 0x80) != 0, instr.ZeroingMasking);
							if ((p2v.bits & 7) != 0)
								Assert.Equal(Register.K0 + (p2v.bits & 7), instr.OpMask);
							else
								Assert.Equal(Register.None, instr.OpMask);
						}
						else
							Assert.Equal(Code.INVALID, instr.Code);
					}
				}
			}
		}

		[Fact]
		void Test_EVEX_b_bit() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);

				bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
				bool isSaeOrEr = isRegOnly && (opCode.CanUseRoundingControl || opCode.CanSuppressAllExceptions);
				bool newCodeSaeOrEr = TryGetSaeErInstruction(opCode, out var newCode);

				if (opCode.CanBroadcast && !isRegOnly) {
					{
						bytes[evexIndex + 3] &= 0xEF;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.False(instr.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(instr.IsBroadcast);
					}
				}
				else {
					if (!isSaeOrEr) {
						bytes[evexIndex + 3] &= 0xEF;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.False(instr.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						if (isSaeOrEr)
							Assert.Equal(info.Code, instr.Code);
						else if (newCodeSaeOrEr && isRegOnly)
							Assert.Equal(newCode, instr.Code);
						else
							Assert.Equal(Code.INVALID, instr.Code);
						Assert.False(instr.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instr);
						if (newCodeSaeOrEr && isRegOnly)
							Assert.Equal(newCode, instr.Code);
						else
							Assert.Equal(info.Code, instr.Code);
						Assert.False(instr.IsBroadcast);
					}
				}
			}
		}

		static bool TryGetSaeErInstruction(OpCodeInfo opCode, out Code newCode) {
			if (opCode.Encoding == EncodingKind.EVEX && !(opCode.CanSuppressAllExceptions || opCode.CanUseRoundingControl)) {
				var mnemonic = opCode.Code.ToMnemonic();
				for (int i = (int)opCode.Code + 1, j = 1; i < Iced.Intel.DecoderConstants.NumberOfCodeValues && j <= 2; i++, j++) {
					var nextCode = (Code)i;
					if (nextCode.ToMnemonic() != mnemonic)
						break;
					var nextOpCode = nextCode.ToOpCode();
					if (nextOpCode.Encoding != opCode.Encoding)
						break;
					if (nextOpCode.CanSuppressAllExceptions || nextOpCode.CanUseRoundingControl) {
						newCode = nextCode;
						return true;
					}
				}
			}
			newCode = Code.INVALID;
			return false;
		}

		[Fact]
		void Verify_only_Full_ddd_and_Half_ddd_support_bcst() {
			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var opCode = ((Code)i).ToOpCode();
				bool expectedBcst;
				switch (opCode.TupleType) {
				case TupleType.Full_128:
				case TupleType.Full_256:
				case TupleType.Full_512:
				case TupleType.Half_128:
				case TupleType.Half_256:
				case TupleType.Half_512:
					expectedBcst = true;
					break;
				default:
					expectedBcst = false;
					break;
				}
				Assert.Equal(expectedBcst, opCode.CanBroadcast);
			}
		}
#endif
	}
}
