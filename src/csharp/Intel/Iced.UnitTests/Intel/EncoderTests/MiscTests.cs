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

#if !NO_ENCODER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Iced.Intel.EncoderInternal;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class MiscTests : EncoderTest {
		[Fact]
		void Encode_INVALID_Code_value_is_an_error() {
			Encoder encoder;
			var instr = new Instruction { Code = Code.INVALID };
			string errorMessage;
			bool result;
			uint instrLen;

			encoder = Encoder.Create(16, new CodeWriterImpl());
			result = encoder.TryEncode(instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(32, new CodeWriterImpl());
			result = encoder.TryEncode(instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(64, new CodeWriterImpl());
			result = encoder.TryEncode(instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);
		}

		[Fact]
		void Encode_throws() {
			var instr = new Instruction { Code = Code.INVALID };
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			Assert.Throws<EncoderException>(() => {
				var instrCopy = instr;
				encoder.Encode(instrCopy, 0);
			});
		}

		[Theory]
		[MemberData(nameof(DisplSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte_Data))]
		void DisplSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte(int bitness, string hexBytes, ulong rip, Instruction instruction) {
			var expectedBytes = HexUtils.ToByteArray(hexBytes);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(bitness, codeWriter);
			Assert.True(encoder.TryEncode(instruction, rip, out uint encodedLength, out string errorMessage), $"Could not encode {instruction}, error: {errorMessage}");
			Assert.Equal(expectedBytes, codeWriter.ToArray());
			Assert.Equal((uint)expectedBytes.Length, encodedLength);
		}
		public static IEnumerable<object[]> DisplSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte_Data {
			get {
				const ulong rip = 0UL;

				var memory16 = new MemoryOperand(Register.SI, 0x1234, 1);
				yield return new object[] { 16, "0F10 8C 3412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 16, "C5F8 10 8C 3412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 16, "62 F17C08 10 8C 3412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 16, "8F E878C0 8C 3412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory16, 0xA5) };
				yield return new object[] { 16, "0F0F 8C 3412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory16) };

				var memory32 = new MemoryOperand(Register.ESI, 0x12345678, 1);
				yield return new object[] { 32, "0F10 8E 78563412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 32, "C5F8 10 8E 78563412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 32, "62 F17C08 10 8E 78563412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 32, "8F E878C0 8E 78563412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory32, 0xA5) };
				yield return new object[] { 32, "0F0F 8E 78563412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory32) };

				var memory64 = new MemoryOperand(Register.R14, 0x12345678, 1);
				yield return new object[] { 64, "41 0F10 8E 78563412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory64) };
				yield return new object[] { 64, "C4C178 10 8E 78563412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory64) };
				yield return new object[] { 64, "62 D17C08 10 8E 78563412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory64) };
				yield return new object[] { 64, "8F C878C0 8E 78563412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory64, 0xA5) };
				yield return new object[] { 64, "0F0F 8E 78563412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory64) };

				// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
				Static.Assert(IcedConstants.NumberOfEncodingKinds == 5 ? 0 : -1);
			}
		}

		[Fact]
		void Encode_BP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(16, writer);
			var instr = Instruction.Create(Code.Mov_r16_rm16, Register.AX, new MemoryOperand(Register.BP));
			uint len = encoder.Encode(instr, 0);
			var expected = new byte[] { 0x8B, 0x46, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_EBP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(32, writer);
			var instr = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.EBP));
			uint len = encoder.Encode(instr, 0);
			var expected = new byte[] { 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13D_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instr = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.R13D));
			uint len = encoder.Encode(instr, 0);
			var expected = new byte[] { 0x67, 0x41, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_RBP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instr = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.RBP));
			uint len = encoder.Encode(instr, 0);
			var expected = new byte[] { 0x48, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instr = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.R13));
			uint len = encoder.Encode(instr, 0);
			var expected = new byte[] { 0x49, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(16)]
		[InlineData(32)]
		[InlineData(64)]
		void Verify_encoder_options(int bitness) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());
			Assert.False(encoder.PreventVEX2);
			Assert.Equal(0U, encoder.VEX_WIG);
			Assert.Equal(0U, encoder.VEX_LIG);
			Assert.Equal(0U, encoder.EVEX_WIG);
			Assert.Equal(0U, encoder.EVEX_LIG);
		}

		[Theory]
		[InlineData(16)]
		[InlineData(32)]
		[InlineData(64)]
		void GetSet_WIG_LIG_options(int bitness) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());

			encoder.VEX_LIG = 1;
			encoder.VEX_WIG = 0;
			Assert.Equal(0U, encoder.VEX_WIG);
			Assert.Equal(1U, encoder.VEX_LIG);
			encoder.VEX_WIG = 1;
			Assert.Equal(1U, encoder.VEX_WIG);
			Assert.Equal(1U, encoder.VEX_LIG);

			encoder.VEX_WIG = 0xFFFFFFFE;
			Assert.Equal(0U, encoder.VEX_WIG);
			Assert.Equal(1U, encoder.VEX_LIG);
			encoder.VEX_WIG = 0xFFFFFFFF;
			Assert.Equal(1U, encoder.VEX_WIG);
			Assert.Equal(1U, encoder.VEX_LIG);

			encoder.VEX_WIG = 1;
			encoder.VEX_LIG = 0;
			Assert.Equal(0U, encoder.VEX_LIG);
			Assert.Equal(1U, encoder.VEX_WIG);
			encoder.VEX_LIG = 1;
			Assert.Equal(1U, encoder.VEX_LIG);
			Assert.Equal(1U, encoder.VEX_WIG);

			encoder.VEX_LIG = 0xFFFFFFFE;
			Assert.Equal(0U, encoder.VEX_LIG);
			Assert.Equal(1U, encoder.VEX_WIG);
			encoder.VEX_LIG = 0xFFFFFFFF;
			Assert.Equal(1U, encoder.VEX_LIG);
			Assert.Equal(1U, encoder.VEX_WIG);

			encoder.EVEX_LIG = 3;
			encoder.EVEX_WIG = 0;
			Assert.Equal(0U, encoder.EVEX_WIG);
			Assert.Equal(3U, encoder.EVEX_LIG);
			encoder.EVEX_WIG = 1;
			Assert.Equal(1U, encoder.EVEX_WIG);
			Assert.Equal(3U, encoder.EVEX_LIG);

			encoder.EVEX_WIG = 0xFFFFFFFE;
			Assert.Equal(0U, encoder.EVEX_WIG);
			Assert.Equal(3U, encoder.EVEX_LIG);
			encoder.EVEX_WIG = 0xFFFFFFFF;
			Assert.Equal(1U, encoder.EVEX_WIG);
			Assert.Equal(3U, encoder.EVEX_LIG);

			encoder.EVEX_WIG = 1;
			encoder.EVEX_LIG = 0;
			Assert.Equal(0U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 1;
			Assert.Equal(1U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 2;
			Assert.Equal(2U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 3;
			Assert.Equal(3U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);

			encoder.EVEX_LIG = 0xFFFFFFFC;
			Assert.Equal(0U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 0xFFFFFFFD;
			Assert.Equal(1U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 0xFFFFFFFE;
			Assert.Equal(2U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
			encoder.EVEX_LIG = 0xFFFFFFFF;
			Assert.Equal(3U, encoder.EVEX_LIG);
			Assert.Equal(1U, encoder.EVEX_WIG);
		}

		[Theory]
		[InlineData("C5FC 10 10", "C4E17C 10 10", Code.VEX_Vmovups_ymm_ymmm256, true)]
		[InlineData("C5FC 10 10", "C5FC 10 10", Code.VEX_Vmovups_ymm_ymmm256, false)]
		void Prevent_VEX2_encoding(string hexBytes, string expectedBytes, Code code, bool preventVEX2) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.IP = DecoderConstants.DEFAULT_IP64;
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.PreventVEX2 = preventVEX2;
			encoder.Encode(instr, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}

		[Theory]
		[InlineData("C5CA 10 CD", "C5CA 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 0, 0)]
		[InlineData("C5CA 10 CD", "C5CE 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 0, 1)]
		[InlineData("C5CA 10 CD", "C5CA 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 1, 0)]
		[InlineData("C5CA 10 CD", "C5CE 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 1, 1)]

		[InlineData("C4414A 10 CD", "C4414A 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 0, 0)]
		[InlineData("C4414A 10 CD", "C4414E 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 0, 1)]
		[InlineData("C4414A 10 CD", "C441CA 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 1, 0)]
		[InlineData("C4414A 10 CD", "C441CE 10 CD", Code.VEX_Vmovss_xmm_xmm_xmm, 1, 1)]

		[InlineData("C5F9 50 D3", "C5F9 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 0, 0)]
		[InlineData("C5F9 50 D3", "C5F9 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 0, 1)]
		[InlineData("C5F9 50 D3", "C5F9 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 1, 0)]
		[InlineData("C5F9 50 D3", "C5F9 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 1, 1)]

		[InlineData("C4C179 50 D3", "C4C179 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 0, 0)]
		[InlineData("C4C179 50 D3", "C4C179 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 0, 1)]
		[InlineData("C4C179 50 D3", "C4C179 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 1, 0)]
		[InlineData("C4C179 50 D3", "C4C179 50 D3", Code.VEX_Vmovmskpd_r32_xmm, 1, 1)]
		void Test_VEX_WIG_LIG(string hexBytes, string expectedBytes, Code code, uint wig, uint lig) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.IP = DecoderConstants.DEFAULT_IP64;
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.VEX_WIG = wig;
			encoder.VEX_LIG = lig;
			encoder.Encode(instr, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}

		[Theory]
		[InlineData("62 F14E08 10 D3", "62 F14E08 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 0)]
		[InlineData("62 F14E08 10 D3", "62 F14E28 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 1)]
		[InlineData("62 F14E08 10 D3", "62 F14E48 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 2)]
		[InlineData("62 F14E08 10 D3", "62 F14E68 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 3)]

		[InlineData("62 F14E08 10 D3", "62 F14E08 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 0)]
		[InlineData("62 F14E08 10 D3", "62 F14E28 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 1)]
		[InlineData("62 F14E08 10 D3", "62 F14E48 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 2)]
		[InlineData("62 F14E08 10 D3", "62 F14E68 10 D3", Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 3)]

		[InlineData("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 0)]
		[InlineData("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 1)]
		[InlineData("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 2)]
		[InlineData("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 3)]

		[InlineData("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 0)]
		[InlineData("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 1)]
		[InlineData("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 2)]
		[InlineData("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 3)]

		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 0)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 1)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 2)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 3)]

		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 0)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 1)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 2)]
		[InlineData("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 3)]
		void Test_EVEX_WIG_LIG(string hexBytes, string expectedBytes, Code code, uint wig, uint lig) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.IP = DecoderConstants.DEFAULT_IP64;
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.EVEX_WIG = wig;
			encoder.EVEX_LIG = lig;
			encoder.Encode(instr, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}

		[Fact]
		void Test_Encoder_Create_throws() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => Encoder.Create(bitness, new CodeWriterImpl()));

			foreach (var bitness in new[] { 16, 32, 64 })
				Assert.Throws<ArgumentNullException>(() => Encoder.Create(bitness, null));
		}

		[Fact]
		void ToOpCode_throws_if_input_is_invalid() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)int.MinValue).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.NumberOfCodeValues).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)int.MaxValue).ToOpCode());
		}

		[Fact]
		void Verify_MemoryOperand_ctors() {
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4, 0x12345678, 8, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0, op.Displacement);
				Assert.Equal(0, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, 0x12345678, 8, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RSI, 4, 0x12345678, 8, true, Register.FS);
				Assert.Equal(Register.None, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, 0x12345678, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(1, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4, 0x12345678, 8);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0, op.Displacement);
				Assert.Equal(0, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, 0x12345678, 8);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RSI, 4, 0x12345678, 8);
				Assert.Equal(Register.None, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, 0x12345678);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0x12345678, op.Displacement);
				Assert.Equal(1, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0, op.Displacement);
				Assert.Equal(0, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
		}

		[Fact]
		void OpCodeInfo_IsAvailableInMode_throws_if_invalid_bitness() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => Code.Nopd.ToOpCode().IsAvailableInMode(bitness));
		}
	}
}
#endif
