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
using System.Collections.Generic;
using Iced.Intel;
using Iced.Intel.EncoderInternal;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class MiscTests : EncoderTest {
		[Fact]
		void Verify_Handlers_table_Code_values() {
			var handlers = OpCodeHandlers.Handlers;
			for (int i = 0; i < handlers.Length; i++)
				Assert.Equal((Code)i, handlers[i].TEST_Code);
			Assert.Equal(Iced.Intel.DecoderConstants.NumberOfCodeValues, handlers.Length);
		}

		[Fact]
		void Encode_INVALID_Code_value_is_an_error() {
			Encoder encoder;
			var instr = new Instruction { Code = Code.INVALID };
			string errorMessage;
			bool result;
			uint instrLen;

			encoder = Encoder.Create(16, new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(32, new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(64, new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
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
				encoder.Encode(ref instrCopy, 0);
			});
		}

		[Theory]
		[MemberData(nameof(DisplSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte_Data))]
		void DisplSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte(int bitness, string hexBytes, ulong rip, Instruction instruction) {
			var expectedBytes = HexUtils.ToByteArray(hexBytes);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(bitness, codeWriter);
			Assert.True(encoder.TryEncode(ref instruction, rip, out uint encodedLength, out string errorMessage), $"Could not encode {instruction}, error: {errorMessage}");
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
				Assert.Equal(5, GetNumEncodings());

				int GetNumEncodings() {
					int count = 0;
					foreach (var field in typeof(EncodingKind).GetFields()) {
						if (!field.IsStatic || !field.IsLiteral)
							continue;
						var encoding = (EncodingKind)field.GetValue(null);
						Assert.Equal((EncodingKind)count, encoding);
						count++;
					}
					return count;
				}
			}
		}

		[Fact]
		void Encode_BP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(16, writer);
			var instr = Instruction.Create(Code.Mov_r16_rm16, Register.AX, new MemoryOperand(Register.BP));
			uint len = encoder.Encode(ref instr, 0);
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
			uint len = encoder.Encode(ref instr, 0);
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
			uint len = encoder.Encode(ref instr, 0);
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
			uint len = encoder.Encode(ref instr, 0);
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
			uint len = encoder.Encode(ref instr, 0);
			var expected = new byte[] { 0x49, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		// Some AMD CPUs support LOCK MOV CR0 = MOV CR8. Make sure we never encode MOV CR8 in 64-bit mode with a LOCK prefix
		[Theory]
		[InlineData("44 0F20 C1", Code.Mov_r64_cr, "44 0F20 C1")]
		[InlineData("44 0F22 C1", Code.Mov_cr_r64, "44 0F22 C1")]
		[InlineData("F0 0F20 C1", Code.Mov_r64_cr, "44 0F20 C1")]
		[InlineData("F0 0F22 C1", Code.Mov_cr_r64, "44 0F22 C1")]
		void Encode_MOV_CR8_in_64bit_mode_does_not_add_LOCK(string hexBytes, Code code, string encodedBytes) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var writer = new CodeWriterImpl();
			var encoder = decoder.CreateEncoder(writer);
			encoder.Encode(ref instr, 0);
			var expectedBytes = HexUtils.ToByteArray(encodedBytes);
			var actualBytes = writer.ToArray();
			Assert.Equal(expectedBytes, actualBytes);
		}

		[Theory]
		[InlineData(16)]
		[InlineData(32)]
		[InlineData(64)]
		void Verify_encoder_options(int bitness) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());
			Assert.False(encoder.PreventVEX2);
		}

		[Theory]
		[InlineData("C5FC 10 10", "C4E17C 10 10", Code.VEX_Vmovups_ymm_ymmm256, true)]
		[InlineData("C5FC 10 10", "C5FC 10 10", Code.VEX_Vmovups_ymm_ymmm256, false)]
		void Prevent_VEX2_encoding(string hexBytes, string expectedBytes, Code code, bool preventVEX2) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64;
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = decoder.CreateEncoder(codeWriter);
			encoder.PreventVEX2 = preventVEX2;
			encoder.Encode(ref instr, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}
	}
}
#endif
