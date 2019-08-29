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
				var co1 = decoder.GetConstantOffsets(instr1);
				var co2 = decoderAll.GetConstantOffsets(instr2);
				Assert.Equal(info.Code, instr1.Code);
				Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
				VerifyConstantOffsets(co1, co2);
			}
		}

		[Theory]
		[MemberData(nameof(Test64_all_mandatory_prefixes_Data))]
		void Test64_all_mandatory_prefixes(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal(byteLength, instr.ByteLength);
		}
		public static IEnumerable<object[]> Test64_all_mandatory_prefixes_Data {
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

		[Fact]
		void Verify_invalid_vvvv() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
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

				Get_Vvvvv_info(opCode, out var uses_vvvv, out var isVsib, out var vvvv_mask);

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					int b2i = vexIndex + 1;
					if (bytes[vexIndex] != 0xC5)
						b2i++;
					var b2 = bytes[b2i];
					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					uint b2_mask = (byte)(info.Bitness == 64 || !isVEX2 ? 0x78 : 0x38);
					if (uses_vvvv) {
						bytes[b2i] = (byte)((b2 & ~b2_mask) | (b2_mask & ~(vvvv_mask << 3)));
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
						}
						if (info.Bitness != 64 && !isVEX2) {
							// vvvv[3] is ignored in 16/32-bit modes, clear it (it's inverted, so 'set' it)
							bytes[b2i] = (byte)(b2 & ~0x40);
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						if (info.Bitness == 64 && vvvv_mask != 0xF) {
							bytes[b2i] = (byte)(b2 & ~b2_mask);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(Code.INVALID, instr.Code);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
							}
						}
					}
					else {
						bytes[b2i] = (byte)(b2 & ~b2_mask);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(Code.INVALID, instr.Code);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						if (info.Bitness != 64 && !isVEX2) {
							// vvvv[3] is ignored in 16/32-bit modes, clear it (it's inverted, so 'set' it)
							bytes[b2i] = (byte)(b2 & ~0x40);
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX) {
					Debug.Assert(vvvv_mask == 0x1F);
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					var b2 = bytes[evexIndex + 2];
					var b3 = bytes[evexIndex + 3];
					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					bytes[evexIndex + 2] = (byte)(b2 & 0x87);
					if (!isVsib)
						bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						if (uses_vvvv)
							Assert.Equal(info.Code, instr.Code);
						else {
							Assert.Equal(Code.INVALID, instr.Code);
							decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out instr);
							Assert.Equal(info.Code, instr.Code);
						}
					}
					if (!uses_vvvv) {
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
					// V'vvvv[4:3] is ignored in 16/32-bit modes (vvvv[3] if it's a vsib instruction)
					bytes[evexIndex + 2] = (byte)(b2 & ~0x40);
					if (!isVsib)
						bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
					if (info.Bitness != 64) {
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		static void Get_Vvvvv_info(OpCodeInfo opCode, out bool uses_vvvv, out bool isVsib, out uint vvvv_mask) {
			uses_vvvv = false;
			isVsib = false;
			switch (opCode.Encoding) {
			case EncodingKind.EVEX:
				vvvv_mask = 0x1F;
				break;
			case EncodingKind.VEX:
			case EncodingKind.XOP:
				vvvv_mask = 0xF;
				break;
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
			default:
				throw new InvalidOperationException();
			}
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64z:
					isVsib = true;
					break;
				case OpCodeOperandKind.k_vvvv:
					uses_vvvv = true;
					vvvv_mask = 0x7;
					break;
				case OpCodeOperandKind.r32_vvvv:
				case OpCodeOperandKind.r64_vvvv:
				case OpCodeOperandKind.xmm_vvvv:
				case OpCodeOperandKind.xmmp3_vvvv:
				case OpCodeOperandKind.ymm_vvvv:
				case OpCodeOperandKind.zmm_vvvv:
				case OpCodeOperandKind.zmmp3_vvvv:
					uses_vvvv = true;
					break;
				}
			}
		}

		[Fact]
		void Verify_GPR_RRXB_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
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

				bool uses_rm = false;
				bool uses_reg = false;
				bool other_rm = false;
				bool other_reg = false;
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
						uses_rm = true;
						break;
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
						uses_reg = true;
						break;
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
						other_rm = true;
						break;
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
						other_reg = true;
						break;
					}
				}
				if (!uses_rm && !uses_reg && opCode.OpCount > 0)
					continue;

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
					bool isRegOnly = mrmi >= bytes.Length ? true : (bytes[mrmi] >> 6) == 3;
					var b1 = bytes[vexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
					else if (!other_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
					if (uses_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly)
								Assert.False(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
					else if (!other_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x80;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					var b1 = bytes[evexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
					else if (!other_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
					if (uses_reg) {
						if (info.Bitness == 64) {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(Code.INVALID, instr.Code);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
							}
						}
						else {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_K_reg_RRXB_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
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

				bool uses_rm = false;
				bool maybe_uses_rm = false;
				bool uses_reg = false;
				bool other_rm = false;
				bool other_reg = false;
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
						maybe_uses_rm = true;
						break;
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.k_rm:
						uses_rm = true;
						break;
					case OpCodeOperandKind.k_reg:
						uses_reg = true;
						break;

					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
						other_rm = true;
						break;
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
						other_reg = true;
						break;
					}
				}
				if (uses_reg && maybe_uses_rm)
					uses_rm = true;
				if (!uses_rm && !uses_reg && opCode.OpCount > 0)
					continue;

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
					bool isRegOnly = mrmi >= bytes.Length ? true : (bytes[mrmi] >> 6) == 3;
					var b1 = bytes[vexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
					else if (!other_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
					if (uses_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness == 64) {
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(Code.INVALID, instr.Code);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								if (isRegOnly)
									Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
						}
					}
					else if (!other_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x80;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					var b1 = bytes[evexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							if (isRegOnly)
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
						}
					}
					else if (!other_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instr);
						Assert.Equal(info.Code, instr.Code);
						Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
					}
					if (uses_reg) {
						if (info.Bitness == 64) {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(Code.INVALID, instr.Code);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(Code.INVALID, instr.Code);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
						}
						else {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instr);
								Assert.Equal(info.Code, instr.Code);
								Assert.True(Instruction.TEST_BitByBitEquals(origInstr, instr));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_vsib_with_invalid_index_register() {
			var codeValues = new HashSet<Code> {
				Code.EVEX_Vpgatherdd_xmm_k1_vm32x,
				Code.EVEX_Vpgatherdd_ymm_k1_vm32y,
				Code.EVEX_Vpgatherdd_zmm_k1_vm32z,
				Code.EVEX_Vpgatherdq_xmm_k1_vm32x,
				Code.EVEX_Vpgatherdq_ymm_k1_vm32x,
				Code.EVEX_Vpgatherdq_zmm_k1_vm32y,
				Code.EVEX_Vpgatherqd_xmm_k1_vm64x,
				Code.EVEX_Vpgatherqd_xmm_k1_vm64y,
				Code.EVEX_Vpgatherqd_ymm_k1_vm64z,
				Code.EVEX_Vpgatherqq_xmm_k1_vm64x,
				Code.EVEX_Vpgatherqq_ymm_k1_vm64y,
				Code.EVEX_Vpgatherqq_zmm_k1_vm64z,
				Code.EVEX_Vgatherdps_xmm_k1_vm32x,
				Code.EVEX_Vgatherdps_ymm_k1_vm32y,
				Code.EVEX_Vgatherdps_zmm_k1_vm32z,
				Code.EVEX_Vgatherdpd_xmm_k1_vm32x,
				Code.EVEX_Vgatherdpd_ymm_k1_vm32x,
				Code.EVEX_Vgatherdpd_zmm_k1_vm32y,
				Code.EVEX_Vgatherqps_xmm_k1_vm64x,
				Code.EVEX_Vgatherqps_xmm_k1_vm64y,
				Code.EVEX_Vgatherqps_ymm_k1_vm64z,
				Code.EVEX_Vgatherqpd_xmm_k1_vm64x,
				Code.EVEX_Vgatherqpd_ymm_k1_vm64y,
				Code.EVEX_Vgatherqpd_zmm_k1_vm64z,
			};
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var opCode = info.Code.ToOpCode();
				Assert.Equal(CanHaveInvalidIndexRegister(opCode), codeValues.Contains(info.Code));
				if (!CanHaveInvalidIndexRegister(opCode))
					continue;

				if (opCode.Encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					var p0 = bytes[evexIndex + 1];
					var p2 = bytes[evexIndex + 3];
					var m = bytes[evexIndex + 5];
					var s = bytes[evexIndex + 6];
					for (int i = 0; i < 32; i++) {
						int regNum = info.Bitness == 64 ? i : i & 7;
						int t = i ^ 0x1F;
						// reg  = R' R modrm.reg
						// vidx = V' X sib.index
						bytes[evexIndex + 1] = (byte)((p0 & ~0xD0) | /*R'*/(t & 0x10) | /*R*/((t & 0x08) << 4) | /*X*/((t & 0x08) << 3));
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0xC0;
						bytes[evexIndex + 3] = (byte)((p2 & ~0x08) | /*V'*/((t & 0x10) >> 1));
						bytes[evexIndex + 5] = (byte)((m & 0xC7) | /*modrm.reg*/((i & 7) << 3));
						bytes[evexIndex + 6] = (byte)((s & 0xC7) | /*sib.index*/((i & 7) << 3));

						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instr);
							Assert.Equal(Code.INVALID, instr.Code);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instr);
							Assert.Equal(info.Code, instr.Code);
							Assert.Equal(OpKind.Register, instr.Op0Kind);
							Assert.Equal(OpKind.Memory, instr.Op1Kind);
							Assert.NotEqual(Register.None, instr.MemoryIndex);
							Assert.Equal(regNum, instr.Op0Register.GetInfo().Number);
							Assert.Equal(regNum, instr.MemoryIndex.GetInfo().Number);
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		// All Vk_VSIB instructions, eg. EVEX_Vpgatherdd_xmm_k1_vm32x
		static bool CanHaveInvalidIndexRegister(OpCodeInfo opCode) {
			if (opCode.Encoding != EncodingKind.EVEX)
				return false;

			switch (opCode.Op0Kind) {
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.zmm_reg:
				break;
			default:
				return false;
			}

			int vsibIndex = -1;
			for (int i = 1; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib64z:
					vsibIndex = i;
					i = int.MaxValue - 1;
					break;
				}
			}
			if (vsibIndex <= 0)
				return false;

			return true;
		}

		static bool IsVsib(OpCodeInfo opCode) => TryGetVsib(opCode, out _, out _);

		static bool TryGetVsib(OpCodeInfo opCode, out bool isVsib32, out bool isVsib64) {
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib32z:
					isVsib32 = true;
					isVsib64 = false;
					return true;

				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib64z:
					isVsib32 = false;
					isVsib64 = true;
					return true;
				}
			}

			isVsib32 = false;
			isVsib64 = false;
			return false;
		}

		[Fact]
		void Test_vsib_props() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instr);
				Assert.Equal(info.Code, instr.Code);

				bool isVsib = TryGetVsib(info.Code.ToOpCode(), out var isVsib32, out var isVsib64);
				Assert.Equal(instr.IsVsib, isVsib);
				Assert.Equal(instr.IsVsib32, isVsib32);
				Assert.Equal(instr.IsVsib64, isVsib64);
			}
		}

		struct TestedInfo {
			// VEX/XOP.L and EVEX.L'L values
			public uint LBits;// bit 0 = L0/L128, bit 1 = L1/L256, etc
			public uint VEX2_LBits;

			// VEX/XOP/EVEX/MVEX: W values
			public uint WBits;// bit 0 = W0, bit 1 = W1

			// REX/VEX/XOP/EVEX/MVEX.R
			public uint RBits;
			public uint VEX2_RBits;
			// REX/VEX/XOP/EVEX/MVEX.X
			public uint XBits;
			// REX/VEX/XOP/EVEX/MVEX.B
			public uint BBits;
			// EVEX/MVEX.R'
			public uint R2Bits;
			// EVEX/MVEX.V'
			public uint V2Bits;

			// mod=11
			public bool RegReg;
			// mod=00,01,10
			public bool RegMem;

			// EVEX/MVEX only
			public bool MemDisp8;

			// Tested VEX2 prefix
			public bool VEX2;
			// Tested VEX3 prefix
			public bool VEX3;

			// EVEX/MVEX: tested opmask
			public bool OpMask;
			// EVEX/MVEX: tested no opmask
			public bool NoOpMask;

			public bool PrefixXacquire;
			public bool PrefixNoXacquire;
			public bool PrefixXrelease;
			public bool PrefixNoXrelease;
			public bool PrefixLock;
			public bool PrefixNoLock;
			public bool PrefixHnt;
			public bool PrefixNoHnt;
			public bool PrefixHt;
			public bool PrefixNoHt;
			public bool PrefixRep;
			public bool PrefixNoRep;
			public bool PrefixRepne;
			public bool PrefixNoRepne;
			public bool PrefixNotrack;
			public bool PrefixNoNotrack;
			public bool PrefixBnd;
			public bool PrefixNoBnd;
		}
		[Fact]
		void Verify_that_test_cases_test_enough_bits() {
			var testedInfos16 = new TestedInfo[Iced.Intel.DecoderConstants.NumberOfCodeValues];
			var testedInfos32 = new TestedInfo[Iced.Intel.DecoderConstants.NumberOfCodeValues];
			var testedInfos64 = new TestedInfo[Iced.Intel.DecoderConstants.NumberOfCodeValues];

			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var testedInfos = info.Bitness switch {
					16 => testedInfos16,
					32 => testedInfos32,
					64 => testedInfos64,
					_ => throw new InvalidOperationException(),
				};

				var opCode = info.Code.ToOpCode();
				ref var tested = ref testedInfos[(int)info.Code];

				var bytes = HexUtils.ToByteArray(info.HexBytes);
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
				decoder.Decode(out var instr);
				Assert.Equal(info.Code, instr.Code);

				if (opCode.Encoding == EncodingKind.EVEX) {
					int evexIndex = GetEvexIndex(bytes);
					if (instr.RoundingControl == RoundingControl.None)
						tested.LBits |= 1U << ((bytes[evexIndex + 3] >> 5) & 3);
					tested.WBits |= 1U << (bytes[evexIndex + 2] >> 7);
					tested.RBits |= 1U << ((bytes[evexIndex + 1] >> 7) ^ 1);
					tested.XBits |= 1U << (((bytes[evexIndex + 1] >> 6) & 1) ^ 1);
					tested.BBits |= 1U << (((bytes[evexIndex + 1] >> 5) & 1) ^ 1);
					tested.R2Bits |= 1U << (((bytes[evexIndex + 1] >> 4) & 1) ^ 1);
					tested.V2Bits |= 1U << (((bytes[evexIndex + 3] >> 3) & 1) ^ 1);
					if ((bytes[evexIndex + 5] >> 6) != 3) {
						tested.RegMem = true;
						if (instr.MemoryDisplSize == 1 && instr.MemoryDisplacement != 0)
							tested.MemDisp8 = true;
					}
					else
						tested.RegReg = true;
					if (instr.OpMask != Register.None)
						tested.OpMask = true;
					else
						tested.NoOpMask = true;
				}
				else if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					int vexIndex = GetVexXopIndex(bytes);
					int mrmi;
					if (bytes[vexIndex] == 0xC5) {
						mrmi = vexIndex + 3;
						tested.VEX2 = true;
						tested.VEX2_RBits |= 1U << ((bytes[vexIndex + 1] >> 7) ^ 1);
						tested.VEX2_LBits |= 1U << ((bytes[vexIndex + 1] >> 2) & 1);
					}
					else {
						mrmi = vexIndex + 4;
						if (opCode.Encoding == EncodingKind.VEX)
							tested.VEX3 = true;
						tested.RBits |= 1U << ((bytes[vexIndex + 1] >> 7) ^ 1);
						tested.XBits |= 1U << (((bytes[vexIndex + 1] >> 6) & 1) ^ 1);
						tested.BBits |= 1U << (((bytes[vexIndex + 1] >> 5) & 1) ^ 1);
						tested.WBits |= 1U << (bytes[vexIndex + 2] >> 7);
						tested.LBits |= 1U << ((bytes[vexIndex + 2] >> 2) & 1);
					}
					if (HasModRM(opCode)) {
						if ((bytes[mrmi] >> 6) != 3)
							tested.RegMem = true;
						else
							tested.RegReg = true;
					}
				}
				else if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW) {
					uint rex = 0;
					int i;
					for (i = 0; i < bytes.Length; i++) {
						byte b = bytes[i];
						bool isPrefix;
						switch (b) {
						case 0x26:
						case 0x2E:
						case 0x36:
						case 0x3E:
						case 0x64:
						case 0x65:
						case 0x66:
						case 0x67:
						case 0xF0:
						case 0xF2:
						case 0xF3:
							isPrefix = true;
							rex = 0;
							break;
						default:
							if (info.Bitness == 64 && (b & 0xF0) == 0x40) {
								isPrefix = true;
								rex = b;
							}
							else
								isPrefix = false;
							break;
						}
						if (!isPrefix)
							break;
					}
					if (info.Bitness == 64) {
						tested.RBits |= 1U << (int)((rex >> 2) & 1);
						tested.XBits |= 1U << (int)((rex >> 1) & 1);
						tested.BBits |= 1U << (int)(rex & 1);
					}
					else {
						tested.RBits |= 1;
						tested.XBits |= 1;
						tested.BBits |= 1;
					}
					if (HasModRM(opCode)) {
						switch (opCode.Table) {
						case OpCodeTableKind.Normal:
							break;
						case OpCodeTableKind.T0F:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							break;
						case OpCodeTableKind.T0F38:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							if (bytes[i++] != 0x38)
								throw new InvalidOperationException();
							break;
						case OpCodeTableKind.T0F3A:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							if (bytes[i++] != 0x3A)
								throw new InvalidOperationException();
							break;
						default:
							throw new InvalidOperationException();
						}
						i++;
						if ((bytes[i] >> 6) != 3)
							tested.RegMem = true;
						else
							tested.RegReg = true;
					}
					if (opCode.CanUseXacquirePrefix) {
						if (instr.HasXacquirePrefix)
							tested.PrefixXacquire = true;
						else
							tested.PrefixNoXacquire = true;
					}
					if (opCode.CanUseXreleasePrefix) {
						if (instr.HasXreleasePrefix)
							tested.PrefixXrelease = true;
						else
							tested.PrefixNoXrelease = true;
					}
					if (opCode.CanUseLockPrefix) {
						if (instr.HasLockPrefix)
							tested.PrefixLock = true;
						else
							tested.PrefixNoLock = true;
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (instr.SegmentPrefix == Register.CS)
							tested.PrefixHnt = true;
						else
							tested.PrefixNoHnt = true;
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (instr.SegmentPrefix == Register.DS)
							tested.PrefixHt = true;
						else
							tested.PrefixNoHt = true;
					}
					if (opCode.CanUseRepPrefix) {
						if (instr.HasRepPrefix)
							tested.PrefixRep = true;
						else
							tested.PrefixNoRep = true;
					}
					if (opCode.CanUseRepnePrefix) {
						if (instr.HasRepnePrefix)
							tested.PrefixRepne = true;
						else
							tested.PrefixNoRepne = true;
					}
					if (opCode.CanUseNotrackPrefix) {
						if (instr.SegmentPrefix == Register.DS)
							tested.PrefixNotrack = true;
						else
							tested.PrefixNoNotrack = true;
					}
					if (opCode.CanUseBndPrefix) {
						if (instr.HasRepnePrefix)
							tested.PrefixBnd = true;
						else
							tested.PrefixNoBnd = true;
					}
				}
				else
					throw new InvalidOperationException();
			}

			var wig32_16 = new List<Code>();
			var wig32_32 = new List<Code>();

			var wig_16 = new List<Code>();
			var wig_32 = new List<Code>();
			var wig_64 = new List<Code>();

			var lig_16 = new List<Code>();
			var lig_32 = new List<Code>();
			var lig_64 = new List<Code>();

			var vex2_lig_16 = new List<Code>();
			var vex2_lig_32 = new List<Code>();
			var vex2_lig_64 = new List<Code>();

			var rr_16 = new List<Code>();
			var rr_32 = new List<Code>();
			var rr_64 = new List<Code>();

			var rm_16 = new List<Code>();
			var rm_32 = new List<Code>();
			var rm_64 = new List<Code>();

			var disp8_16 = new List<Code>();
			var disp8_32 = new List<Code>();
			var disp8_64 = new List<Code>();

			var vex2_16 = new List<Code>();
			var vex2_32 = new List<Code>();
			var vex2_64 = new List<Code>();

			var vex3_16 = new List<Code>();
			var vex3_32 = new List<Code>();
			var vex3_64 = new List<Code>();

			var opmask_16 = new List<Code>();
			var opmask_32 = new List<Code>();
			var opmask_64 = new List<Code>();

			var noopmask_16 = new List<Code>();
			var noopmask_32 = new List<Code>();
			var noopmask_64 = new List<Code>();

			var b_16 = new List<Code>();
			var b_32 = new List<Code>();
			var b_64 = new List<Code>();

			var r2_16 = new List<Code>();
			var r2_32 = new List<Code>();
			var r2_64 = new List<Code>();

			var r_64 = new List<Code>();
			var vex2_r_64 = new List<Code>();
			var x_64 = new List<Code>();
			var v2_64 = new List<Code>();

			var pfx_xacquire_16 = new List<Code>();
			var pfx_xacquire_32 = new List<Code>();
			var pfx_xacquire_64 = new List<Code>();

			var pfx_xrelease_16 = new List<Code>();
			var pfx_xrelease_32 = new List<Code>();
			var pfx_xrelease_64 = new List<Code>();

			var pfx_lock_16 = new List<Code>();
			var pfx_lock_32 = new List<Code>();
			var pfx_lock_64 = new List<Code>();

			var pfx_hnt_16 = new List<Code>();
			var pfx_hnt_32 = new List<Code>();
			var pfx_hnt_64 = new List<Code>();

			var pfx_ht_16 = new List<Code>();
			var pfx_ht_32 = new List<Code>();
			var pfx_ht_64 = new List<Code>();

			var pfx_rep_16 = new List<Code>();
			var pfx_rep_32 = new List<Code>();
			var pfx_rep_64 = new List<Code>();

			var pfx_repne_16 = new List<Code>();
			var pfx_repne_32 = new List<Code>();
			var pfx_repne_64 = new List<Code>();

			var pfx_notrack_16 = new List<Code>();
			var pfx_notrack_32 = new List<Code>();
			var pfx_notrack_64 = new List<Code>();

			var pfx_bnd_16 = new List<Code>();
			var pfx_bnd_32 = new List<Code>();
			var pfx_bnd_64 = new List<Code>();

			var pfx_no_xacquire_16 = new List<Code>();
			var pfx_no_xacquire_32 = new List<Code>();
			var pfx_no_xacquire_64 = new List<Code>();

			var pfx_no_xrelease_16 = new List<Code>();
			var pfx_no_xrelease_32 = new List<Code>();
			var pfx_no_xrelease_64 = new List<Code>();

			var pfx_no_lock_16 = new List<Code>();
			var pfx_no_lock_32 = new List<Code>();
			var pfx_no_lock_64 = new List<Code>();

			var pfx_no_hnt_16 = new List<Code>();
			var pfx_no_hnt_32 = new List<Code>();
			var pfx_no_hnt_64 = new List<Code>();

			var pfx_no_ht_16 = new List<Code>();
			var pfx_no_ht_32 = new List<Code>();
			var pfx_no_ht_64 = new List<Code>();

			var pfx_no_rep_16 = new List<Code>();
			var pfx_no_rep_32 = new List<Code>();
			var pfx_no_rep_64 = new List<Code>();

			var pfx_no_repne_16 = new List<Code>();
			var pfx_no_repne_32 = new List<Code>();
			var pfx_no_repne_64 = new List<Code>();

			var pfx_no_notrack_16 = new List<Code>();
			var pfx_no_notrack_32 = new List<Code>();
			var pfx_no_notrack_64 = new List<Code>();

			var pfx_no_bnd_16 = new List<Code>();
			var pfx_no_bnd_32 = new List<Code>();
			var pfx_no_bnd_64 = new List<Code>();

			foreach (var bitness in new int[] { 16, 32, 64 }) {
				var testedInfos = bitness switch {
					16 => testedInfos16,
					32 => testedInfos32,
					64 => testedInfos64,
					_ => throw new InvalidOperationException(),
				};

				for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
					var code = (Code)i;
					var opCode = code.ToOpCode();
					if (!opCode.IsInstruction || opCode.Code == Code.Popw_CS)
						continue;
					if (opCode.Fwait)
						continue;

					switch (bitness) {
					case 16:
						if (!opCode.Mode16)
							continue;
						break;
					case 32:
						if (!opCode.Mode32)
							continue;
						break;
					case 64:
						if (!opCode.Mode64)
							continue;
						break;
					default:
						throw new InvalidOperationException();
					}

					ref var tested = ref testedInfos[i];

					if ((bitness == 16 || bitness == 32) && opCode.IsWIG32) {
						if (tested.WBits != 3)
							GetList2(bitness, wig32_16, wig32_32).Add(code);
					}
					if (opCode.IsWIG) {
						if (tested.WBits != 3)
							GetList(bitness, wig_16, wig_32, wig_64).Add(code);
					}
					if (opCode.IsLIG) {
						uint allLBits;
						switch (opCode.Encoding) {
						case EncodingKind.VEX:
						case EncodingKind.XOP:
							allLBits = 3;// 1 bit = 2 values
							break;

						case EncodingKind.EVEX:
							allLBits = 0xF;// 2 bits = 4 values
							break;

						case EncodingKind.Legacy:
						case EncodingKind.D3NOW:
						default:
							throw new InvalidOperationException();
						}
						if (tested.LBits != allLBits)
							GetList(bitness, lig_16, lig_32, lig_64).Add(code);
					}
					if (opCode.IsLIG && opCode.Encoding == EncodingKind.VEX) {
						if (tested.VEX2_LBits != 3 && CanUseVEX2(opCode))
							GetList(bitness, vex2_lig_16, vex2_lig_32, vex2_lig_64).Add(code);
					}
					if (CanUseModRM_rm_mem(opCode)) {
						if (!tested.RegMem)
							GetList(bitness, rm_16, rm_32, rm_64).Add(code);
					}
					if (CanUseModRM_rm_reg(opCode)) {
						if (!tested.RegReg)
							GetList(bitness, rr_16, rr_32, rr_64).Add(code);
					}
					switch (opCode.Encoding) {
					case EncodingKind.Legacy:
					case EncodingKind.VEX:
					case EncodingKind.XOP:
					case EncodingKind.D3NOW:
						break;
					case EncodingKind.EVEX:
						if (!tested.MemDisp8 && CanUseModRM_rm_mem(opCode))
							GetList(bitness, disp8_16, disp8_32, disp8_64).Add(code);
						break;
					default:
						throw new InvalidOperationException();
					}
					if (opCode.Encoding == EncodingKind.VEX) {
						if (!tested.VEX3)
							GetList(bitness, vex3_16, vex3_32, vex3_64).Add(code);
						if (!tested.VEX2 && CanUseVEX2(opCode))
							GetList(bitness, vex2_16, vex2_32, vex2_64).Add(code);
					}
					if (opCode.CanUseOpMaskRegister) {
						if (!tested.OpMask)
							GetList(bitness, opmask_16, opmask_32, opmask_64).Add(code);
						if (!tested.NoOpMask && !MustUseNonZeroOpMaskRegister(opCode))
							GetList(bitness, noopmask_16, noopmask_32, noopmask_64).Add(code);
					}
					if (CanUseB(bitness, opCode)) {
						if (tested.BBits != 3)
							GetList(bitness, b_16, b_32, b_64).Add(code);
					}
					else {
						if ((tested.BBits & 1) == 0)
							GetList(bitness, b_16, b_32, b_64).Add(code);
					}
					switch (opCode.Encoding) {
					case EncodingKind.EVEX:
						if (CanUseR2(opCode)) {
							if (tested.R2Bits != 3)
								GetList(bitness, r2_16, r2_32, r2_64).Add(code);
						}
						else {
							if ((tested.R2Bits & 1) == 0)
								GetList(bitness, r2_16, r2_32, r2_64).Add(code);
						}
						break;
					case EncodingKind.Legacy:
					case EncodingKind.VEX:
					case EncodingKind.XOP:
					case EncodingKind.D3NOW:
						break;
					default:
						throw new InvalidOperationException();
					}
					if (bitness == 64 && opCode.Mode64) {
						if (tested.VEX2_RBits != 3 && opCode.Encoding == EncodingKind.VEX && CanUseVEX2(opCode) && CanUseR(opCode))
							vex2_r_64.Add(code);
						if (CanUseR(opCode)) {
							if (tested.RBits != 3)
								r_64.Add(code);
						}
						else {
							if ((tested.RBits & 1) == 0)
								r_64.Add(code);
						}
						if (IsVsib(opCode)) {
							// The memory tests test vsib memory operands
						}
						else if (CanUseX(opCode)) {
							if (tested.XBits != 3)
								x_64.Add(code);
						}
						else {
							if ((tested.XBits & 1) == 0)
								x_64.Add(code);
						}
						switch (opCode.Encoding) {
						case EncodingKind.EVEX:
							if (IsVsib(opCode)) {
								// The memory tests test vsib memory operands
							}
							else if (CanUseV2(opCode)) {
								if (tested.V2Bits != 3)
									v2_64.Add(code);
							}
							else {
								if ((tested.V2Bits & 1) == 0)
									v2_64.Add(code);
							}
							break;
						case EncodingKind.Legacy:
						case EncodingKind.VEX:
						case EncodingKind.XOP:
						case EncodingKind.D3NOW:
							break;
						default:
							throw new InvalidOperationException();
						}
					}
					if (opCode.CanUseXacquirePrefix) {
						if (!tested.PrefixXacquire)
							GetList(bitness, pfx_xacquire_16, pfx_xacquire_32, pfx_xacquire_64).Add(code);
						if (!tested.PrefixNoXacquire)
							GetList(bitness, pfx_no_xacquire_16, pfx_no_xacquire_32, pfx_no_xacquire_64).Add(code);
					}
					if (opCode.CanUseXreleasePrefix) {
						if (!tested.PrefixXrelease)
							GetList(bitness, pfx_xrelease_16, pfx_xrelease_32, pfx_xrelease_64).Add(code);
						if (!tested.PrefixNoXrelease)
							GetList(bitness, pfx_no_xrelease_16, pfx_no_xrelease_32, pfx_no_xrelease_64).Add(code);
					}
					if (opCode.CanUseLockPrefix) {
						if (!tested.PrefixLock)
							GetList(bitness, pfx_lock_16, pfx_lock_32, pfx_lock_64).Add(code);
						if (!tested.PrefixNoLock)
							GetList(bitness, pfx_no_lock_16, pfx_no_lock_32, pfx_no_lock_64).Add(code);
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (!tested.PrefixHnt)
							GetList(bitness, pfx_hnt_16, pfx_hnt_32, pfx_hnt_64).Add(code);
						if (!tested.PrefixNoHnt)
							GetList(bitness, pfx_no_hnt_16, pfx_no_hnt_32, pfx_no_hnt_64).Add(code);
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (!tested.PrefixHt)
							GetList(bitness, pfx_ht_16, pfx_ht_32, pfx_ht_64).Add(code);
						if (!tested.PrefixNoHt)
							GetList(bitness, pfx_no_ht_16, pfx_no_ht_32, pfx_no_ht_64).Add(code);
					}
					if (opCode.CanUseRepPrefix) {
						if (!tested.PrefixRep)
							GetList(bitness, pfx_rep_16, pfx_rep_32, pfx_rep_64).Add(code);
						if (!tested.PrefixNoRep)
							GetList(bitness, pfx_no_rep_16, pfx_no_rep_32, pfx_no_rep_64).Add(code);
					}
					if (opCode.CanUseRepnePrefix) {
						if (!tested.PrefixRepne)
							GetList(bitness, pfx_repne_16, pfx_repne_32, pfx_repne_64).Add(code);
						if (!tested.PrefixNoRepne)
							GetList(bitness, pfx_no_repne_16, pfx_no_repne_32, pfx_no_repne_64).Add(code);
					}
					if (opCode.CanUseNotrackPrefix) {
						if (!tested.PrefixNotrack)
							GetList(bitness, pfx_notrack_16, pfx_notrack_32, pfx_notrack_64).Add(code);
						if (!tested.PrefixNoNotrack)
							GetList(bitness, pfx_no_notrack_16, pfx_no_notrack_32, pfx_no_notrack_64).Add(code);
					}
					if (opCode.CanUseBndPrefix) {
						if (!tested.PrefixBnd)
							GetList(bitness, pfx_bnd_16, pfx_bnd_32, pfx_bnd_64).Add(code);
						if (!tested.PrefixNoBnd)
							GetList(bitness, pfx_no_bnd_16, pfx_no_bnd_32, pfx_no_bnd_64).Add(code);
					}
				}
			}

			Assert.Equal("wig32_16:", "wig32_16:" + string.Join(",", wig32_16.ToArray()));
			Assert.Equal("wig32_32:", "wig32_32:" + string.Join(",", wig32_32.ToArray()));
			Assert.Equal("wig_16:", "wig_16:" + string.Join(",", wig_16.ToArray()));
			Assert.Equal("wig_32:", "wig_32:" + string.Join(",", wig_32.ToArray()));
			Assert.Equal("wig_64:", "wig_64:" + string.Join(",", wig_64.ToArray()));
			Assert.Equal("lig_16:", "lig_16:" + string.Join(",", lig_16.ToArray()));
			Assert.Equal("lig_32:", "lig_32:" + string.Join(",", lig_32.ToArray()));
			Assert.Equal("lig_64:", "lig_64:" + string.Join(",", lig_64.ToArray()));
			Assert.Equal("vex2_lig_16:", "vex2_lig_16:" + string.Join(",", vex2_lig_16.ToArray()));
			Assert.Equal("vex2_lig_32:", "vex2_lig_32:" + string.Join(",", vex2_lig_32.ToArray()));
			Assert.Equal("vex2_lig_64:", "vex2_lig_64:" + string.Join(",", vex2_lig_64.ToArray()));
			Assert.Equal("rr_16:", "rr_16:" + string.Join(",", rr_16.ToArray()));
			Assert.Equal("rr_32:", "rr_32:" + string.Join(",", rr_32.ToArray()));
			Assert.Equal("rr_64:", "rr_64:" + string.Join(",", rr_64.ToArray()));
			Assert.Equal("rm_16:", "rm_16:" + string.Join(",", rm_16.ToArray()));
			Assert.Equal("rm_32:", "rm_32:" + string.Join(",", rm_32.ToArray()));
			Assert.Equal("rm_64:", "rm_64:" + string.Join(",", rm_64.ToArray()));
			Assert.Equal("disp8_16:", "disp8_16:" + string.Join(",", disp8_16.ToArray()));
			Assert.Equal("disp8_32:", "disp8_32:" + string.Join(",", disp8_32.ToArray()));
			Assert.Equal("disp8_64:", "disp8_64:" + string.Join(",", disp8_64.ToArray()));
			Assert.Equal("vex2_16:", "vex2_16:" + string.Join(",", vex2_16.ToArray()));
			Assert.Equal("vex2_32:", "vex2_32:" + string.Join(",", vex2_32.ToArray()));
			Assert.Equal("vex2_64:", "vex2_64:" + string.Join(",", vex2_64.ToArray()));
			Assert.Equal("vex3_16:", "vex3_16:" + string.Join(",", vex3_16.ToArray()));
			Assert.Equal("vex3_32:", "vex3_32:" + string.Join(",", vex3_32.ToArray()));
			Assert.Equal("vex3_64:", "vex3_64:" + string.Join(",", vex3_64.ToArray()));
			Assert.Equal("opmask_16:", "opmask_16:" + string.Join(",", opmask_16.ToArray()));
			Assert.Equal("opmask_32:", "opmask_32:" + string.Join(",", opmask_32.ToArray()));
			Assert.Equal("opmask_64:", "opmask_64:" + string.Join(",", opmask_64.ToArray()));
			Assert.Equal("noopmask_16:", "noopmask_16:" + string.Join(",", noopmask_16.ToArray()));
			Assert.Equal("noopmask_32:", "noopmask_32:" + string.Join(",", noopmask_32.ToArray()));
			Assert.Equal("noopmask_64:", "noopmask_64:" + string.Join(",", noopmask_64.ToArray()));
			Assert.Equal("b_16:", "b_16:" + string.Join(",", b_16.ToArray()));
			Assert.Equal("b_32:", "b_32:" + string.Join(",", b_32.ToArray()));
			Assert.Equal("b_64:", "b_64:" + string.Join(",", b_64.ToArray()));
			Assert.Equal("r2_16:", "r2_16:" + string.Join(",", r2_16.ToArray()));
			Assert.Equal("r2_32:", "r2_32:" + string.Join(",", r2_32.ToArray()));
			Assert.Equal("r2_64:", "r2_64:" + string.Join(",", r2_64.ToArray()));
			Assert.Equal("r_64:", "r_64:" + string.Join(",", r_64.ToArray()));
			Assert.Equal("vex2_r_64:", "vex2_r_64:" + string.Join(",", vex2_r_64.ToArray()));
			Assert.Equal("x_64:", "x_64:" + string.Join(",", x_64.ToArray()));
			Assert.Equal("v2_64:", "v2_64:" + string.Join(",", v2_64.ToArray()));
			Assert.Equal("pfx_xacquire_16:", "pfx_xacquire_16:" + string.Join(",", pfx_xacquire_16.ToArray()));
			Assert.Equal("pfx_xacquire_32:", "pfx_xacquire_32:" + string.Join(",", pfx_xacquire_32.ToArray()));
			Assert.Equal("pfx_xacquire_64:", "pfx_xacquire_64:" + string.Join(",", pfx_xacquire_64.ToArray()));
			Assert.Equal("pfx_xrelease_16:", "pfx_xrelease_16:" + string.Join(",", pfx_xrelease_16.ToArray()));
			Assert.Equal("pfx_xrelease_32:", "pfx_xrelease_32:" + string.Join(",", pfx_xrelease_32.ToArray()));
			Assert.Equal("pfx_xrelease_64:", "pfx_xrelease_64:" + string.Join(",", pfx_xrelease_64.ToArray()));
			Assert.Equal("pfx_lock_16:", "pfx_lock_16:" + string.Join(",", pfx_lock_16.ToArray()));
			Assert.Equal("pfx_lock_32:", "pfx_lock_32:" + string.Join(",", pfx_lock_32.ToArray()));
			Assert.Equal("pfx_lock_64:", "pfx_lock_64:" + string.Join(",", pfx_lock_64.ToArray()));
			Assert.Equal("pfx_hnt_16:", "pfx_hnt_16:" + string.Join(",", pfx_hnt_16.ToArray()));
			Assert.Equal("pfx_hnt_32:", "pfx_hnt_32:" + string.Join(",", pfx_hnt_32.ToArray()));
			Assert.Equal("pfx_hnt_64:", "pfx_hnt_64:" + string.Join(",", pfx_hnt_64.ToArray()));
			Assert.Equal("pfx_ht_16:", "pfx_ht_16:" + string.Join(",", pfx_ht_16.ToArray()));
			Assert.Equal("pfx_ht_32:", "pfx_ht_32:" + string.Join(",", pfx_ht_32.ToArray()));
			Assert.Equal("pfx_ht_64:", "pfx_ht_64:" + string.Join(",", pfx_ht_64.ToArray()));
			Assert.Equal("pfx_rep_16:", "pfx_rep_16:" + string.Join(",", pfx_rep_16.ToArray()));
			Assert.Equal("pfx_rep_32:", "pfx_rep_32:" + string.Join(",", pfx_rep_32.ToArray()));
			Assert.Equal("pfx_rep_64:", "pfx_rep_64:" + string.Join(",", pfx_rep_64.ToArray()));
			Assert.Equal("pfx_repne_16:", "pfx_repne_16:" + string.Join(",", pfx_repne_16.ToArray()));
			Assert.Equal("pfx_repne_32:", "pfx_repne_32:" + string.Join(",", pfx_repne_32.ToArray()));
			Assert.Equal("pfx_repne_64:", "pfx_repne_64:" + string.Join(",", pfx_repne_64.ToArray()));
			Assert.Equal("pfx_notrack_16:", "pfx_notrack_16:" + string.Join(",", pfx_notrack_16.ToArray()));
			Assert.Equal("pfx_notrack_32:", "pfx_notrack_32:" + string.Join(",", pfx_notrack_32.ToArray()));
			Assert.Equal("pfx_notrack_64:", "pfx_notrack_64:" + string.Join(",", pfx_notrack_64.ToArray()));
			Assert.Equal("pfx_bnd_16:", "pfx_bnd_16:" + string.Join(",", pfx_bnd_16.ToArray()));
			Assert.Equal("pfx_bnd_32:", "pfx_bnd_32:" + string.Join(",", pfx_bnd_32.ToArray()));
			Assert.Equal("pfx_bnd_64:", "pfx_bnd_64:" + string.Join(",", pfx_bnd_64.ToArray()));
			Assert.Equal("pfx_no_xacquire_16:", "pfx_no_xacquire_16:" + string.Join(",", pfx_no_xacquire_16.ToArray()));
			Assert.Equal("pfx_no_xacquire_32:", "pfx_no_xacquire_32:" + string.Join(",", pfx_no_xacquire_32.ToArray()));
			Assert.Equal("pfx_no_xacquire_64:", "pfx_no_xacquire_64:" + string.Join(",", pfx_no_xacquire_64.ToArray()));
			Assert.Equal("pfx_no_xrelease_16:", "pfx_no_xrelease_16:" + string.Join(",", pfx_no_xrelease_16.ToArray()));
			Assert.Equal("pfx_no_xrelease_32:", "pfx_no_xrelease_32:" + string.Join(",", pfx_no_xrelease_32.ToArray()));
			Assert.Equal("pfx_no_xrelease_64:", "pfx_no_xrelease_64:" + string.Join(",", pfx_no_xrelease_64.ToArray()));
			Assert.Equal("pfx_no_lock_16:", "pfx_no_lock_16:" + string.Join(",", pfx_no_lock_16.ToArray()));
			Assert.Equal("pfx_no_lock_32:", "pfx_no_lock_32:" + string.Join(",", pfx_no_lock_32.ToArray()));
			Assert.Equal("pfx_no_lock_64:", "pfx_no_lock_64:" + string.Join(",", pfx_no_lock_64.ToArray()));
			Assert.Equal("pfx_no_hnt_16:", "pfx_no_hnt_16:" + string.Join(",", pfx_no_hnt_16.ToArray()));
			Assert.Equal("pfx_no_hnt_32:", "pfx_no_hnt_32:" + string.Join(",", pfx_no_hnt_32.ToArray()));
			Assert.Equal("pfx_no_hnt_64:", "pfx_no_hnt_64:" + string.Join(",", pfx_no_hnt_64.ToArray()));
			Assert.Equal("pfx_no_ht_16:", "pfx_no_ht_16:" + string.Join(",", pfx_no_ht_16.ToArray()));
			Assert.Equal("pfx_no_ht_32:", "pfx_no_ht_32:" + string.Join(",", pfx_no_ht_32.ToArray()));
			Assert.Equal("pfx_no_ht_64:", "pfx_no_ht_64:" + string.Join(",", pfx_no_ht_64.ToArray()));
			Assert.Equal("pfx_no_rep_16:", "pfx_no_rep_16:" + string.Join(",", pfx_no_rep_16.ToArray()));
			Assert.Equal("pfx_no_rep_32:", "pfx_no_rep_32:" + string.Join(",", pfx_no_rep_32.ToArray()));
			Assert.Equal("pfx_no_rep_64:", "pfx_no_rep_64:" + string.Join(",", pfx_no_rep_64.ToArray()));
			Assert.Equal("pfx_no_repne_16:", "pfx_no_repne_16:" + string.Join(",", pfx_no_repne_16.ToArray()));
			Assert.Equal("pfx_no_repne_32:", "pfx_no_repne_32:" + string.Join(",", pfx_no_repne_32.ToArray()));
			Assert.Equal("pfx_no_repne_64:", "pfx_no_repne_64:" + string.Join(",", pfx_no_repne_64.ToArray()));
			Assert.Equal("pfx_no_notrack_16:", "pfx_no_notrack_16:" + string.Join(",", pfx_no_notrack_16.ToArray()));
			Assert.Equal("pfx_no_notrack_32:", "pfx_no_notrack_32:" + string.Join(",", pfx_no_notrack_32.ToArray()));
			Assert.Equal("pfx_no_notrack_64:", "pfx_no_notrack_64:" + string.Join(",", pfx_no_notrack_64.ToArray()));
			Assert.Equal("pfx_no_bnd_16:", "pfx_no_bnd_16:" + string.Join(",", pfx_no_bnd_16.ToArray()));
			Assert.Equal("pfx_no_bnd_32:", "pfx_no_bnd_32:" + string.Join(",", pfx_no_bnd_32.ToArray()));
			Assert.Equal("pfx_no_bnd_64:", "pfx_no_bnd_64:" + string.Join(",", pfx_no_bnd_64.ToArray()));

			static bool CanUseModRM_rm_reg(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.r8_mem:
					case OpCodeOperandKind.r16_mem:
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.mm_mem:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
					case OpCodeOperandKind.bnd_mem_mpx:
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
						return true;
					}
				}
				return false;
			}

			static bool CanUseModRM_rm_mem(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.r8_mem:
					case OpCodeOperandKind.r16_mem:
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.mm_mem:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
					case OpCodeOperandKind.bnd_mem_mpx:
					case OpCodeOperandKind.k_mem:
						return true;
					}
				}
				return false;
			}

			static bool CanUseVEX2(OpCodeInfo opCode) => opCode.Table == OpCodeTableKind.T0F && opCode.W == 0;

			static bool CanUseB(int bitness, OpCodeInfo opCode) {
				if (opCode.Code == Code.Nopw || opCode.Code == Code.Nopd || opCode.Code == Code.Nopq)
					return false;
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:

					case OpCodeOperandKind.bnd_mem_mpx:
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.mm_mem:
					case OpCodeOperandKind.r16_mem:
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.r8_mem:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
						if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW)
							return bitness == 64;
						return true;

					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}
				if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW)
					return bitness == 64;
				return true;
			}

			static bool CanUseX(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:

					case OpCodeOperandKind.bnd_mem_mpx:
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.mm_mem:
					case OpCodeOperandKind.r16_mem:
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.r8_mem:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
						return true;

					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}
				return true;
			}

			static bool CanUseR(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.tr_reg:
						return false;

					case OpCodeOperandKind.bnd_reg:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.seg_reg:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
						return true;
					}
				}
				return true;
			}

			static bool CanUseR2(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.bnd_reg:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.seg_reg:
						return false;

					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
						return true;
					}
				}
				return true;
			}

			static bool CanUseV2(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_vvvv:
					case OpCodeOperandKind.r32_vvvv:
					case OpCodeOperandKind.r64_vvvv:
						return false;

					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmmp3_vvvv:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.zmm_vvvv:
					case OpCodeOperandKind.zmmp3_vvvv:
						return true;

					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}
				return false;
			}

			static bool HasModRM(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.r8_mem:
					case OpCodeOperandKind.r16_mem:
					case OpCodeOperandKind.r32_mem:
					case OpCodeOperandKind.r32_mem_mpx:
					case OpCodeOperandKind.r64_mem:
					case OpCodeOperandKind.r64_mem_mpx:
					case OpCodeOperandKind.mm_mem:
					case OpCodeOperandKind.xmm_mem:
					case OpCodeOperandKind.ymm_mem:
					case OpCodeOperandKind.zmm_mem:
					case OpCodeOperandKind.bnd_mem_mpx:
					case OpCodeOperandKind.k_mem:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.seg_reg:
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.bnd_reg:
						return true;
					}
				}
				return false;
			}

			static List<Code> GetList2(int bitness, List<Code> l16, List<Code> l32) =>
				bitness switch {
					16 => l16,
					32 => l32,
					_ => throw new InvalidOperationException(),
				};

			static List<Code> GetList(int bitness, List<Code> l16, List<Code> l32, List<Code> l64) =>
				bitness switch {
					16 => l16,
					32 => l32,
					64 => l64,
					_ => throw new InvalidOperationException(),
				};
		}
#endif
	}
}
