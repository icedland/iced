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

			encoder = Encoder.Create16(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create32(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create64(new CodeWriterImpl());
			result = encoder.TryEncode(ref instr, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);
		}

		[Fact]
		void Encode_throws() {
			var instr = new Instruction { Code = Code.INVALID };
			var encoder = Encoder.Create64(new CodeWriterImpl());
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
	}
}
#endif
