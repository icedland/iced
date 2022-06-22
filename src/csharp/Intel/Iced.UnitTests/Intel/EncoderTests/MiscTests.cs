// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
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
			var instruction = new Instruction { Code = Code.INVALID };
			string errorMessage;
			bool result;
			uint instrLen;

			encoder = Encoder.Create(16, new CodeWriterImpl());
			result = encoder.TryEncode(instruction, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(32, new CodeWriterImpl());
			result = encoder.TryEncode(instruction, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);

			encoder = Encoder.Create(64, new CodeWriterImpl());
			result = encoder.TryEncode(instruction, 0, out instrLen, out errorMessage);
			Assert.False(result);
			Assert.Equal(InvalidHandler.ERROR_MESSAGE, errorMessage);
			Assert.Equal(0U, instrLen);
		}

		[Fact]
		void Encode_throws() {
			var instruction = new Instruction { Code = Code.INVALID };
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			Assert.Throws<EncoderException>(() => {
				var instrCopy = instruction;
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
				var memory32 = new MemoryOperand(Register.ESI, 0x12345678, 1);
				var memory64 = new MemoryOperand(Register.R14, 0x12345678, 1);

				yield return new object[] { 16, "0F10 8C 3412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 32, "0F10 8E 78563412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 64, "41 0F10 8E 78563412", rip, Instruction.Create(Code.Movups_xmm_xmmm128, Register.XMM1, memory64) };

#if !NO_VEX
				yield return new object[] { 16, "C5F8 10 8C 3412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 32, "C5F8 10 8E 78563412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 64, "C4C178 10 8E 78563412", rip, Instruction.Create(Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, memory64) };
#endif

#if !NO_EVEX
				yield return new object[] { 16, "62 F17C08 10 8C 3412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory16) };
				yield return new object[] { 32, "62 F17C08 10 8E 78563412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory32) };
				yield return new object[] { 64, "62 D17C08 10 8E 78563412", rip, Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM1, memory64) };
#endif

#if !NO_XOP
				yield return new object[] { 16, "8F E878C0 8C 3412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory16, 0xA5) };
				yield return new object[] { 32, "8F E878C0 8E 78563412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory32, 0xA5) };
				yield return new object[] { 64, "8F C878C0 8E 78563412 A5", rip, Instruction.Create(Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM1, memory64, 0xA5) };
#endif

#if !NO_D3NOW
				yield return new object[] { 16, "0F0F 8C 3412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory16) };
				yield return new object[] { 32, "0F0F 8E 78563412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory32) };
				yield return new object[] { 64, "0F0F 8E 78563412 0C", rip, Instruction.Create(Code.D3NOW_Pi2fw_mm_mmm64, Register.MM1, memory64) };
#endif

#if MVEX
				yield return new object[] { 64, "62 D17808 28 8E 78563412", rip, Instruction.Create(Code.MVEX_Vmovaps_zmm_k1_zmmmt, Register.ZMM1, memory64) };
#endif

				// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
				Static.Assert(IcedConstants.EncodingKindEnumCount == 6 ? 0 : -1);
			}
		}

		[Fact]
		void Encode_BP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(16, writer);
			var instruction = Instruction.Create(Code.Mov_r16_rm16, Register.AX, new MemoryOperand(Register.BP));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x8B, 0x46, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_EBP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(32, writer);
			var instruction = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.EBP));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_EBP_EDX_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(32, writer);
			var instruction = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.EBP, Register.EDX));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x8B, 0x44, 0x15, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13D_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.R13D));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x67, 0x41, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13D_EDX_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r32_rm32, Register.EAX, new MemoryOperand(Register.R13D, Register.EDX));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x67, 0x41, 0x8B, 0x44, 0x15, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_RBP_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.RBP));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x48, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_RBP_RDX_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.RBP, Register.RDX));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x48, 0x8B, 0x44, 0x15, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.R13));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x49, 0x8B, 0x45, 0x00 };
			var actual = writer.ToArray();
			Assert.Equal(actual.Length, (int)len);
			Assert.Equal(expected, actual);
		}

		[Fact]
		void Encode_R13_RDX_with_no_displ() {
			var writer = new CodeWriterImpl();
			var encoder = Encoder.Create(64, writer);
			var instruction = Instruction.Create(Code.Mov_r64_rm64, Register.RAX, new MemoryOperand(Register.R13, Register.RDX));
			uint len = encoder.Encode(instruction, 0);
			var expected = new byte[] { 0x49, 0x8B, 0x44, 0x15, 0x00 };
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
#if MVEX
			Assert.Equal(0U, encoder.MVEX_WIG);
#endif
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

#if MVEX
			encoder.MVEX_WIG = 0;
			Assert.Equal(0U, encoder.MVEX_WIG);
			encoder.MVEX_WIG = 1;
			Assert.Equal(1U, encoder.MVEX_WIG);

			encoder.MVEX_WIG = 0xFFFFFFFE;
			Assert.Equal(0U, encoder.MVEX_WIG);
			encoder.MVEX_WIG = 0xFFFFFFFF;
			Assert.Equal(1U, encoder.MVEX_WIG);
#endif
		}

#if !NO_VEX
		[Theory]
		[InlineData("C5FC 10 10", "C4E17C 10 10", Code.VEX_Vmovups_ymm_ymmm256, true)]
		[InlineData("C5FC 10 10", "C5FC 10 10", Code.VEX_Vmovups_ymm_ymmm256, false)]
		void Prevent_VEX2_encoding(string hexBytes, string expectedBytes, Code code, bool preventVEX2) {
			var decoder = Decoder.Create(64, new ByteArrayCodeReader(hexBytes));
			decoder.IP = DecoderConstants.DEFAULT_IP64;
			decoder.Decode(out var instruction);
			Assert.Equal(code, instruction.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.PreventVEX2 = preventVEX2;
			encoder.Encode(instruction, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}
#endif

#if !NO_VEX
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
			decoder.Decode(out var instruction);
			Assert.Equal(code, instruction.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.VEX_WIG = wig;
			encoder.VEX_LIG = lig;
			encoder.Encode(instruction, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}
#endif

#if !NO_EVEX
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
			decoder.Decode(out var instruction);
			Assert.Equal(code, instruction.Code);
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(decoder.Bitness, codeWriter);
			encoder.EVEX_WIG = wig;
			encoder.EVEX_LIG = lig;
			encoder.Encode(instruction, DecoderConstants.DEFAULT_IP64);
			var encodedBytes = codeWriter.ToArray();
			var expectedBytesArray = HexUtils.ToByteArray(expectedBytes);
			Assert.Equal(expectedBytesArray, encodedBytes);
		}
#endif

		[Fact]
		void Test_Encoder_Create_throws() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => Encoder.Create(bitness, new CodeWriterImpl()));

			foreach (var bitness in new[] { 16, 32, 64 })
				Assert.Throws<ArgumentNullException>(() => Encoder.Create(bitness, null));
		}

#if OPCODE_INFO
		[Fact]
		void ToOpCode_throws_if_input_is_invalid() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)int.MinValue).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)(-1)).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)IcedConstants.CodeEnumCount).ToOpCode());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((Code)int.MaxValue).ToOpCode());
		}
#endif

		[Fact]
		void Verify_MemoryOperand_ctors() {
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4, -0x1234_5678_9ABC_DEF1, 8, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
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
				var op = new MemoryOperand(Register.RCX, -0x1234_5678_9ABC_DEF1, 8, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RSI, 4, -0x1234_5678_9ABC_DEF1, 8, true, Register.FS);
				Assert.Equal(Register.None, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, -0x1234_5678_9ABC_DEF1, true, Register.FS);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(1, op.DisplSize);
				Assert.True(op.IsBroadcast);
				Assert.Equal(Register.FS, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, Register.RSI, 4, -0x1234_5678_9ABC_DEF1, 8);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
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
				var op = new MemoryOperand(Register.RCX, Register.RSI);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0, op.Displacement);
				Assert.Equal(0, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, -0x1234_5678_9ABC_DEF1, 8);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RSI, 4, -0x1234_5678_9ABC_DEF1, 8);
				Assert.Equal(Register.None, op.Base);
				Assert.Equal(Register.RSI, op.Index);
				Assert.Equal(4, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
			{
				var op = new MemoryOperand(Register.RCX, -0x1234_5678_9ABC_DEF1);
				Assert.Equal(Register.RCX, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(-0x1234_5678_9ABC_DEF1, op.Displacement);
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
			{
				var op = new MemoryOperand(0x1234_5678_9ABC_DEF1, 8);
				Assert.Equal(Register.None, op.Base);
				Assert.Equal(Register.None, op.Index);
				Assert.Equal(1, op.Scale);
				Assert.Equal(0x1234_5678_9ABC_DEF1, op.Displacement);
				Assert.Equal(8, op.DisplSize);
				Assert.False(op.IsBroadcast);
				Assert.Equal(Register.None, op.SegmentPrefix);
			}
		}

#if OPCODE_INFO
		[Fact]
		void OpCodeInfo_IsAvailableInMode_throws_if_invalid_bitness() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => Code.Nopd.ToOpCode().IsAvailableInMode(bitness));
		}
#endif

		[Fact]
		void WriteByte_works() {
			var codeWriter = new CodeWriterImpl();
			var encoder = Encoder.Create(64, codeWriter);
			var instruction = Instruction.Create(Code.Add_r64_rm64, Register.R8, Register.RBP);
			encoder.WriteByte(0x90);
			encoder.Encode(instruction, 0x55555555);
			encoder.WriteByte(0xCC);
			Assert.Equal(new byte[] { 0x90, 0x4C, 0x03, 0xC5, 0xCC }, codeWriter.ToArray());
		}

		[Theory]
		[MemberData(nameof(EncodeInvalidRegOpSize_Data))]
		void EncodeInvalidRegOpSize(int bitness, Instruction instruction) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());
			bool b = encoder.TryEncode(instruction, 0, out _, out var errorMessage);
			Assert.False(b);
			Assert.NotNull(errorMessage);
			Assert.Contains("Register operand size must equal memory addressing mode (16/32/64)", errorMessage);
		}
		public static IEnumerable<object[]> EncodeInvalidRegOpSize_Data {
			get {
				yield return new object[] { 16, Instruction.Create(Code.Movdir64b_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };
				yield return new object[] { 32, Instruction.Create(Code.Movdir64b_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };

				yield return new object[] { 16, Instruction.Create(Code.Movdir64b_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 32, Instruction.Create(Code.Movdir64b_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 64, Instruction.Create(Code.Movdir64b_r32_m512, Register.ECX, new MemoryOperand(Register.RBX)) };

				yield return new object[] { 64, Instruction.Create(Code.Movdir64b_r64_m512, Register.RCX, new MemoryOperand(Register.EBX)) };

				yield return new object[] { 16, Instruction.Create(Code.Enqcmds_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };
				yield return new object[] { 32, Instruction.Create(Code.Enqcmds_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };

				yield return new object[] { 16, Instruction.Create(Code.Enqcmds_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 32, Instruction.Create(Code.Enqcmds_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 64, Instruction.Create(Code.Enqcmds_r32_m512, Register.ECX, new MemoryOperand(Register.RBX)) };

				yield return new object[] { 64, Instruction.Create(Code.Enqcmds_r64_m512, Register.RCX, new MemoryOperand(Register.EBX)) };

				yield return new object[] { 16, Instruction.Create(Code.Enqcmd_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };
				yield return new object[] { 32, Instruction.Create(Code.Enqcmd_r16_m512, Register.CX, new MemoryOperand(Register.EBX)) };

				yield return new object[] { 16, Instruction.Create(Code.Enqcmd_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 32, Instruction.Create(Code.Enqcmd_r32_m512, Register.ECX, new MemoryOperand(Register.BX)) };
				yield return new object[] { 64, Instruction.Create(Code.Enqcmd_r32_m512, Register.ECX, new MemoryOperand(Register.RBX)) };

				yield return new object[] { 64, Instruction.Create(Code.Enqcmd_r64_m512, Register.RCX, new MemoryOperand(Register.EBX)) };
			}
		}

		static bool EncodeOk(int bitness, Instruction instruction) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());
			return encoder.TryEncode(instruction, 0x1234, out _, out _);
		}

		static bool EncodeErr(int bitness, Instruction instruction) {
			var encoder = Encoder.Create(bitness, new CodeWriterImpl());
			return !encoder.TryEncode(instruction, 0x1234, out _, out _);
		}

		[Fact]
		void InvalidDispl16() {
			const int bitness = 16;

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_0000UL, 2))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_FFFFUL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x1_0000UL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 2))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(0x0_0000UL, 2))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(0x0_FFFFUL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(0x1_0000UL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 2))));

			foreach (var displSize in new[] { 1, 2 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -0x0_8000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0x0_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -0x0_8001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0x1_0000, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -0x8000_0000_0000_0000, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0x7FFF_FFFF_FFFF_FFFF, displSize))));
			}

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, 0))));

			foreach (var displSize in new[] { 1, 4 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0xFFFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0x1_0000_0000, displSize))));
			}
		}

		[Fact]
		void InvalidDispl32() {
			const int bitness = 32;

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_0000UL, 2))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_FFFFUL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x1_0000UL, 2))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 2))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_0000_0000UL, 4))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_FFFF_FFFFUL, 4))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x1_0000_0000UL, 4))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 4))));

			foreach (var displSize in new[] { 1, 4 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0xFFFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0x1_0000_0000, displSize))));
			}

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, 0))));

			foreach (var displSize in new[] { 1, 4 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0xFFFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0x1_0000_0000, displSize))));
			}
		}

		[Fact]
		void InvalidDispl64() {
			const int bitness = 64;

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_0000_0000UL, 4))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0_FFFF_FFFFUL, 4))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x1_0000_0000UL, 4))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 4))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0x0000_0000_0000_0000UL, 8))));
			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFUL, 8))));

			foreach (var displSize in new[] { 1, 8 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0x7FFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, 0x8000_0000, displSize))));
			}

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13D, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13D, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13D, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBP, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBP, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBP, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.R13, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, 0))));

			Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 0L, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 1, 0))));
			Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, -1, 0))));

			foreach (var displSize in new[] { 1, 4 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0xFFFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EBX, 0x1_0000_0000, displSize))));
			}

			foreach (var displSize in new[] { 1, 8 }) {
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 0L, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, -1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 1, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, -0x8000_0000, displSize))));
				Assert.True(EncodeOk(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 0x7FFF_FFFF, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, -0x8000_0001, displSize))));
				Assert.True(EncodeErr(bitness, Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RBX, 0x8000_0000, displSize))));
			}
		}

		[Fact]
		void TestUnsupportedBitness() {
			{
				var encoder = Encoder.Create(16, new CodeWriterImpl());
				Assert.False(encoder.TryEncode(Instruction.Create(Code.Mov_r64_rm64, Register.RAX, Register.RCX), 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(32, new CodeWriterImpl());
				Assert.False(encoder.TryEncode(Instruction.Create(Code.Mov_r64_rm64, Register.RAX, Register.RCX), 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				Assert.False(encoder.TryEncode(Instruction.Create(Code.Pushad), 0, out _, out _));
			}
		}

		[Fact]
		void TestTooLongInstruction() {
			var encoder = Encoder.Create(16, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Add_rm32_imm32,
				new MemoryOperand(Register.ESP, Register.None, 1, 0x1234_5678, 4, false, Register.SS), 0x1234_5678);
			instr.HasXacquirePrefix = true;
			instr.HasLockPrefix = true;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestWrongOpKind() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Push_r64, Register.RAX);
			instr.Op0Kind = OpKind.Immediate16;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestWrongImpliedRegister() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.In_AL_DX, Register.RAX, Register.EDX);
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestWrongRegister() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Push_r64, Register.EAX);
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestInvalidMaskmov() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateMaskmovq(16, Register.MM0, Register.MM1, Register.None), OpKind.MemorySegRDI),
				(16, Instruction.CreateMaskmovdqu(16, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegRDI),
				(16, Instruction.CreateVmaskmovdqu(16, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegRDI),
				(32, Instruction.CreateMaskmovq(32, Register.MM0, Register.MM1, Register.None), OpKind.MemorySegRDI),
				(32, Instruction.CreateMaskmovdqu(32, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegRDI),
				(32, Instruction.CreateVmaskmovdqu(32, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegRDI),
				(64, Instruction.CreateMaskmovq(64, Register.MM0, Register.MM1, Register.None), OpKind.MemorySegDI),
				(64, Instruction.CreateMaskmovdqu(64, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegDI),
				(64, Instruction.CreateVmaskmovdqu(64, Register.XMM0, Register.XMM1, Register.None), OpKind.MemorySegDI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidOuts() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateOutsb(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(16, Instruction.CreateOutsw(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(16, Instruction.CreateOutsd(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateOutsb(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateOutsw(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateOutsd(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(64, Instruction.CreateOutsb(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
				(64, Instruction.CreateOutsw(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
				(64, Instruction.CreateOutsd(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op1Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op1Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidMovs() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind1, OpKind badOpKind0)[] {
				(16, Instruction.CreateMovsb(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateMovsw(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateMovsd(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateMovsq(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateMovsb(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateMovsw(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateMovsd(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateMovsq(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(64, Instruction.CreateMovsb(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateMovsw(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateMovsd(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateMovsq(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
			};
			foreach (var (bitness, instr2, badOpKind1, badOpKind0) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					instr.Op1Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}

				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind1;
					instr.Op1Kind = badOpKind1;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op1Kind = badOpKind1;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}

				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind0;
					instr.Op1Kind = badOpKind0;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind0;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidCmps() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind1, OpKind badOpKind0)[] {
				(16, Instruction.CreateCmpsb(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateCmpsw(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateCmpsd(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(16, Instruction.CreateCmpsq(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateCmpsb(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateCmpsw(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateCmpsd(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(32, Instruction.CreateCmpsq(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI, OpKind.MemoryESRDI),
				(64, Instruction.CreateCmpsb(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateCmpsw(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateCmpsd(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
				(64, Instruction.CreateCmpsq(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI, OpKind.MemoryESDI),
			};
			foreach (var (bitness, instr2, badOpKind1, badOpKind0) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					instr.Op1Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}

				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind1;
					instr.Op1Kind = badOpKind1;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op1Kind = badOpKind1;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}

				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind0;
					instr.Op1Kind = badOpKind0;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind0;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidLods() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateLodsb(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(16, Instruction.CreateLodsw(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(16, Instruction.CreateLodsd(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(16, Instruction.CreateLodsq(16, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateLodsb(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateLodsw(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateLodsd(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(32, Instruction.CreateLodsq(32, Register.None, RepPrefixKind.None), OpKind.MemorySegRSI),
				(64, Instruction.CreateLodsb(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
				(64, Instruction.CreateLodsw(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
				(64, Instruction.CreateLodsd(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
				(64, Instruction.CreateLodsq(64, Register.None, RepPrefixKind.None), OpKind.MemorySegSI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					instr.Op1Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind;
					instr.Op1Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidIns() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateInsb(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateInsw(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateInsd(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateInsb(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateInsw(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateInsd(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(64, Instruction.CreateInsb(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateInsw(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateInsd(64, RepPrefixKind.None), OpKind.MemoryESDI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidStos() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateStosb(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateStosw(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateStosd(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateStosq(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateStosb(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateStosw(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateStosd(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateStosq(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(64, Instruction.CreateStosb(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateStosw(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateStosd(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateStosq(64, RepPrefixKind.None), OpKind.MemoryESDI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidScas() {
			var tests = new (int bitness, Instruction instr, OpKind badOpKind)[] {
				(16, Instruction.CreateScasb(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateScasw(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateScasd(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(16, Instruction.CreateScasq(16, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateScasb(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateScasw(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateScasd(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(32, Instruction.CreateScasq(32, RepPrefixKind.None), OpKind.MemoryESRDI),
				(64, Instruction.CreateScasb(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateScasw(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateScasd(64, RepPrefixKind.None), OpKind.MemoryESDI),
				(64, Instruction.CreateScasq(64, RepPrefixKind.None), OpKind.MemoryESDI),
			};
			foreach (var (bitness, instr2, badOpKind) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = OpKind.FarBranch16;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Kind = badOpKind;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidXlatb() {
			var tests = new (int bitness, Instruction instr, Register invalidRbx)[] {
				(16, Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.BX, Register.AL, 1, 0, 0, false, Register.None)), Register.RBX),
				(32, Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.EBX, Register.AL, 1, 0, 0, false, Register.None)), Register.RBX),
				(64, Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.RBX, Register.AL, 1, 0, 0, false, Register.None)), Register.BX),
			};
			foreach (var (bitness, instr2, invalidRbx) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					Assert.True(encoder.TryEncode(instr2, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryBase = invalidRbx;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryBase = Register.ESI;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryIndex = Register.AX;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryIndex = Register.None;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				foreach (var scale in new[] { 2, 4, 8 }) {
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryIndexScale = scale;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				var invalidDisplSize = bitness == 64 ? 4 : 8;
				foreach (var (displ, displSize) in new (ulong displ, int displSize)[] { (0, 1), (1, invalidDisplSize), (1, 1) }) {
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryDisplacement64 = displ;
					instr.MemoryDisplSize = displSize;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidConstImmOp() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Rol_rm8_1, Register.AL, 0);
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

#if !NO_VEX
		[Fact]
		void TestInvalidIs5ImmOp() {
			for (int imm = 0; imm < 0x100; imm++) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM0, Register.XMM1, Register.XMM2, Register.XMM3, imm);
				if (imm <= 0x0F)
					Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				else
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}
#endif

		[Fact]
		void TestEncodeInvalidInstr() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			Instruction instr = default;
			Assert.Equal(Code.INVALID, instr.Code);
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestHighR8RegWithRexPrefix() {
			foreach (var reg in new[] { Register.AH, Register.CH, Register.DH, Register.BH }) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Movzx_r64_rm8, Register.RAX, reg);
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

#if !NO_EVEX
		[Fact]
		void TestEvexInvalidK1() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM0, Register.XMM1);
			instr.OpMask = Register.K1;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}
#endif

#if !NO_EVEX
		[Fact]
		void EncodeWithoutRequiredOpMaskRegister() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM0, new MemoryOperand(Register.RAX, Register.XMM1, 1, 0x10, 1, false, Register.None));
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			instr.OpMask = Register.K1;
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
		}
#endif

#if !NO_EVEX
		[Fact]
		void EncodeInvalidSae() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM0, Register.XMM1);
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.SuppressAllExceptions = true;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}
#endif

#if !NO_EVEX
		[Fact]
		void EncodeInvalidEr() {
			for (int i = 0; i < IcedConstants.RoundingControlEnumCount; i++) {
				var er = (RoundingControl)i;
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM0, Register.XMM1);
				instr.RoundingControl = er;
				if (er == RoundingControl.None)
					Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				else
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}
#endif

#if !NO_EVEX
		[Fact]
		void EncodeInvalidBcst() {
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM0, new MemoryOperand(Register.RAX));
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				instr.IsBroadcast = true;
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM0, Register.XMM1, new MemoryOperand(Register.RAX));
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				instr.IsBroadcast = true;
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM0, Register.XMM1, Register.XMM2);
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				instr.IsBroadcast = true;
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}
#endif

#if !NO_EVEX
		[Fact]
		void EncodeInvalidZmsk() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.EVEX_Vmovss_m32_k1_xmm, new MemoryOperand(Register.RAX), Register.XMM1);
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.ZeroingMasking = true;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			instr.OpMask = Register.K1;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}
#endif

		[Fact]
		void EncodeInvalidAbsAddress() {
			var tests1 = new (int bitness, ulong address, int displSize)[] {
				(16, 0x1234, 2),
				(16, 0x1234_5678, 4),
				(32, 0x1234, 2),
				(32, 0x1234_5678, 4),
				(64, 0x1234_5678, 4),
				(64, 0x1234_5678_9ABC_DEF0, 8),
			};
			foreach (var (bitness, address, displSize) in tests1) {
				var memReg = displSize switch {
					2 => Register.BX,
					4 => Register.EBX,
					8 => Register.RBX,
					_ => throw new InvalidOperationException(),
				};

				var instr2 = Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(address, displSize));
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					Assert.True(encoder.TryEncode(instr2, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryBase = memReg;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryIndex = memReg;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				foreach (var scale in new[] { 2, 4, 8 }) {
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.MemoryIndexScale = scale;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op1Kind = OpKind.Immediate8;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}

			var tests2 = new (int bitness, ulong address, int displSize)[] {
				(16, 0x1234, 8),
				(32, 0x1234, 8),
				(64, 0x1234, 2),
			};
			foreach (var (bitness, address, displSize) in tests2) {
				var encoder = Encoder.Create(bitness, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Mov_EAX_moffs32, Register.EAX, new MemoryOperand(address, displSize));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

		[Fact]
		void TestRegOpNotAllowed() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr1 = Instruction.Create(Code.Lea_r32_m, Register.EAX, new MemoryOperand(Register.RAX));
			Assert.True(encoder.TryEncode(instr1, 0, out _, out _));
			var instr2 = Instruction.Create(Code.Lea_r32_m, Register.EAX, Register.ECX);
			Assert.False(encoder.TryEncode(instr2, 0, out _, out _));
		}

		[Fact]
		void TestMemOpNotAllowed() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr1 = Instruction.Create(Code.Movhlps_xmm_xmm, Register.XMM0, Register.XMM1);
			Assert.True(encoder.TryEncode(instr1, 0, out _, out _));
			var instr2 = Instruction.Create(Code.Movhlps_xmm_xmm, Register.XMM0, new MemoryOperand(Register.RAX));
			Assert.False(encoder.TryEncode(instr2, 0, out _, out _));
		}

		[Fact]
		void TestRegmemOpIsWrongSize() {
			var tests = new (int bitness, Instruction instr, Register invalidReg)[] {
				(16, Instruction.Create(Code.Enqcmd_r16_m512, Register.AX, new MemoryOperand(Register.BX)), Register.EAX),
				(16, Instruction.Create(Code.Enqcmd_r32_m512, Register.EAX, new MemoryOperand(Register.EAX)), Register.AX),
				(16, Instruction.Create(Code.Enqcmd_r32_m512, Register.EAX, new MemoryOperand(Register.EAX)), Register.RAX),
				(32, Instruction.Create(Code.Enqcmd_r16_m512, Register.AX, new MemoryOperand(Register.BX)), Register.EAX),
				(32, Instruction.Create(Code.Enqcmd_r32_m512, Register.EAX, new MemoryOperand(Register.EAX)), Register.AX),
				(32, Instruction.Create(Code.Enqcmd_r32_m512, Register.EAX, new MemoryOperand(Register.EAX)), Register.RAX),
				(64, Instruction.Create(Code.Enqcmd_r32_m512, Register.EAX, new MemoryOperand(Register.EAX)), Register.RAX),
				(64, Instruction.Create(Code.Enqcmd_r64_m512, Register.RAX, new MemoryOperand(Register.RAX)), Register.EAX),
				(64, Instruction.Create(Code.Enqcmd_r64_m512, Register.RAX, new MemoryOperand(Register.RAX)), Register.AX),
			};
			foreach (var (bitness, instr2, invalidReg) in tests) {
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					Assert.True(encoder.TryEncode(instr2, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = instr2;
					instr.Op0Register = invalidReg;
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

#if !NO_EVEX
		[Fact]
		void TestVsib16bitAddr() {
			foreach (var bitness in new[] { 16, 32, 64 }) {
				var encoder = Encoder.Create(bitness, new CodeWriterImpl());
				var instr = Instruction.Create(Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM0, new MemoryOperand(Register.EAX, Register.XMM1, 1, 0x10, 1, false, Register.None));
				instr.OpMask = Register.K1;
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				instr.MemoryBase = Register.BX;
				instr.MemoryIndex = Register.SI;
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}
#endif

		[Fact]
		void TestExpectedRegOrMemOpKind() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Add_rm8_imm8, Register.AL, 123);
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.Op0Kind = OpKind.Immediate8;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void Test16bitAddrIn64bitMode() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Lea_r32_m, Register.EAX, new MemoryOperand(Register.BX));
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void Test64bitAddrIn1632bitMode() {
			foreach (var bitness in new[] { 16, 32 }) {
				var encoder = Encoder.Create(bitness, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Lea_r32_m, Register.EAX, new MemoryOperand(Register.RAX));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

		[Fact]
		void TestInvalid16bitMemRegs() {
			var tests = new (Register @base, Register index)[] {
				(Register.AX, Register.None),
				(Register.R8W, Register.None),
				(Register.BL, Register.None),
				(Register.None, Register.CX),
				(Register.None, Register.R9W),
				(Register.None, Register.SIL),
				(Register.BX, Register.BP),
				(Register.BP, Register.BX),
			};
			foreach (var (@base, index) in tests) {
				var encoder = Encoder.Create(16, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(@base, index));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

		[Fact]
		void TestInvalid16bitDisplSize() {
			var encoder = Encoder.Create(16, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.BX, 1));
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 4;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 8;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestInvalid32bitDisplSize() {
			var encoder = Encoder.Create(32, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EAX, 1));
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 2;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 8;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestInvalid64bitDisplSize() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RAX, 1));
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 2;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryDisplSize = 4;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}

		[Fact]
		void TestInvalidIpRelMemory() {
			foreach (var (ipReg, invalidIndex) in new[] { (Register.EIP, Register.EDI), (Register.RIP, Register.RDI) }) {
				{
					var encoder = Encoder.Create(64, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, Register.None, 1, 0, 8, false, Register.None));
					Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				}
				foreach (var displSize in new[] { 0, 1, 4, 8 }) {
					var encoder = Encoder.Create(64, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, Register.None, 1, 0, displSize, false, Register.None));
					Assert.True(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(64, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, Register.None, 1, 0, 2, false, Register.None));
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				{
					var encoder = Encoder.Create(64, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, invalidIndex, 1, 0, 8, false, Register.None));
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
				foreach (var scale in new[] { 2, 4, 8 }) {
					var encoder = Encoder.Create(64, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, Register.None, scale, 0, 8, false, Register.None));
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidIpRelMemory1632() {
			foreach (var bitness in new[] { 16, 32 }) {
				foreach (var (ipReg, displSize) in new[] { (Register.EIP, 4), (Register.RIP, 8) }) {
					var encoder = Encoder.Create(bitness, new CodeWriterImpl());
					var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(ipReg, Register.None, 1, 0, displSize, false, Register.None));
					Assert.False(encoder.TryEncode(instr, 0, out _, out _));
				}
			}
		}

		[Fact]
		void TestInvalidIpRelMemorySibRequired() {
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.VEX_Tileloaddt1_tmm_sibmem, Register.TMM1, new MemoryOperand(Register.RCX, Register.RDX, 1, 0x1234_5678, 8, false, Register.None));
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.VEX_Tileloaddt1_tmm_sibmem, Register.TMM1, new MemoryOperand(Register.RIP, Register.None, 1, 0x1234_5678, 8, false, Register.None));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.VEX_Tileloaddt1_tmm_sibmem, Register.TMM1, new MemoryOperand(Register.ECX, Register.EDX, 1, 0x1234_5678, 4, false, Register.None));
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			}
			{
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.VEX_Tileloaddt1_tmm_sibmem, Register.TMM1, new MemoryOperand(Register.EIP, Register.None, 1, 0x1234_5678, 4, false, Register.None));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

		[Fact]
		void TestInvalidEipRelMemTargetAddr() {
			foreach (var target in new ulong[] { 0, 0x7FFF_FFFF, 0xFFFF_FFFF }) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EIP, Register.None, 1, (long)target, 4, false, Register.None));
				Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			}
			foreach (var target in new[] { 0x1_0000_0000UL, 0xFFFF_FFFF_FFFF_FFFF }) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.EIP, Register.None, 1, (long)target, 4, false, Register.None));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

#if !NO_EVEX
		[Fact]
		void TestVsibWithOffsetOnlyMem() {
			var encoder = Encoder.Create(64, new CodeWriterImpl());
			var instr = Instruction.Create(Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM0, new MemoryOperand(Register.RAX, Register.XMM1, 1, 0x1234_5678, 8, false, Register.None));
			instr.OpMask = Register.K1;
			Assert.True(encoder.TryEncode(instr, 0, out _, out _));
			instr.MemoryBase = Register.None;
			instr.MemoryIndex = Register.None;
			Assert.False(encoder.TryEncode(instr, 0, out _, out _));
		}
#endif

		[Fact]
		void TestInvalidEspRspIndexRegs() {
			foreach (var spReg in new[] { Register.ESP, Register.RSP }) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.None, spReg, 2));
				Assert.False(encoder.TryEncode(instr, 0, out _, out _));
			}
		}

		[Fact]
		void TestRipRelDistTooFarAway() {
			const uint instrLen = 6;
			const ulong instrAddr = 0x1234_5678_9ABC_DEF0;
			foreach (var diff in new long[] { int.MinValue, int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 }) {
				var writer = new CodeWriterImpl();
				var encoder = Encoder.Create(64, writer);
				var target = (long)(instrAddr + instrLen) + diff;
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RIP, Register.None, 1, target, 8, false, Register.None));
				Assert.True(encoder.TryEncode(instr, instrAddr, out var encodedLen, out _));
				Assert.Equal(instrLen, encodedLen);

				var bytes = writer.ToArray();
				var decoded = Decoder.Create(64, new ByteArrayCodeReader(bytes), instrAddr, DecoderOptions.None).Decode();
				Assert.Equal(Code.Not_rm8, decoded.Code);
				Assert.Equal(Register.RIP, decoded.MemoryBase);
				Assert.Equal((ulong)target, decoded.MemoryDisplacement64);
			}
			foreach (var diff in new long[] { (long)int.MinValue - 1, (long)int.MaxValue + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, long.MinValue, long.MaxValue }) {
				var encoder = Encoder.Create(64, new CodeWriterImpl());
				var target = (long)(instrAddr + instrLen) + diff;
				var instr = Instruction.Create(Code.Not_rm8, new MemoryOperand(Register.RIP, Register.None, 1, target, 8, false, Register.None));
				Assert.False(encoder.TryEncode(instr, instrAddr, out _, out _));
			}
		}

		[Fact]
		void TestInvalidJccRel8_16() {
			var validDiffs = new long[] { (long)sbyte.MinValue, (long)sbyte.MaxValue, -1, 0, 1, -0x12, 0x12 };
			var invalidDiffs = new long[] { (long)sbyte.MinValue - 1, (long)sbyte.MaxValue + 1, -0x1234, 0x1234, short.MinValue, short.MaxValue };
			TestInvalidJcc(16, Code.Je_rel8_16, 0x1234, 2, 0xFFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidJccRel8_32() {
			var validDiffs = new long[] { (long)sbyte.MinValue, (long)sbyte.MaxValue, -1, 0, 1, -0x12, 0x12 };
			var invalidDiffs = new long[] { (long)sbyte.MinValue - 1, (long)sbyte.MaxValue + 1, -0x1234_5678, 0x1234_5678, int.MinValue, int.MaxValue };
			TestInvalidJcc(32, Code.Je_rel8_32, 0x1234_5678, 2, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidJccRel8_64() {
			var validDiffs = new long[] { (long)sbyte.MinValue, (long)sbyte.MaxValue, -1, 0, 1, -0x12, 0x12 };
			var invalidDiffs = new long[] { (long)sbyte.MinValue - 1, (long)sbyte.MaxValue + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, long.MinValue, long.MaxValue };
			TestInvalidJcc(64, Code.Je_rel8_64, 0x1234_5678_9ABC_DEF0, 2, 0xFFFF_FFFF_FFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidJccRel16_16() {
			var validDiffs = new long[] { (long)short.MinValue, (long)short.MaxValue, -1, 0, 1, -0x1234, 0x1234 };
			var invalidDiffs = new long[] { };
			TestInvalidJcc(16, Code.Je_rel16, 0x1234, 4, 0xFFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidJccRel32_32() {
			var validDiffs = new long[] { (long)int.MinValue, (long)int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 };
			var invalidDiffs = new long[] { };
			TestInvalidJcc(32, Code.Je_rel32_32, 0x1234_5678, 6, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidJccRel32_64() {
			var validDiffs = new long[] { (long)int.MinValue, (long)int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 };
			var invalidDiffs = new long[] { (long)int.MinValue - 1, (long)int.MaxValue + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, long.MinValue, long.MaxValue };
			TestInvalidJcc(64, Code.Je_rel32_64, 0x1234_5678_9ABC_DEF0, 6, 0xFFFF_FFFF_FFFF_FFFF, validDiffs, invalidDiffs);
		}

		static void TestInvalidJcc(int bitness, Code code, ulong instrAddr, uint instrLen, ulong addrMask, long[] validDiffs, long[] invalidDiffs) =>
			TestInvalidBr(bitness, code, instrAddr, instrLen, addrMask, validDiffs, invalidDiffs, (code, _, target) => Instruction.CreateBranch(code, target));

		[Fact]
		void TestInvalidXbeginRel16_16() {
			var validDiffs = new long[] { (long)short.MinValue, (long)short.MaxValue, -1, 0, 1, -0x1234, 0x1234 };
			var invalidDiffs = new long[] { (long)short.MinValue - 1, (long)short.MaxValue + 1, -0x1234_5678, 0x1234_5678, int.MinValue, int.MaxValue };
			TestInvalidXbegin(16, Code.Xbegin_rel16, 0x1234, 4, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidXbeginRel32_16() {
			var validDiffs = new long[] { (long)int.MinValue, (long)int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 };
			var invalidDiffs = new long[] { };
			TestInvalidXbegin(16, Code.Xbegin_rel32, 0x1234, 7, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidXbeginRel16_32() {
			var validDiffs = new long[] { (long)short.MinValue, (long)short.MaxValue, -1, 0, 1, -0x1234, 0x1234 };
			var invalidDiffs = new long[] { (long)short.MinValue - 1, (long)short.MaxValue + 1, -0x1234_5678, 0x1234_5678, int.MinValue, int.MaxValue };
			TestInvalidXbegin(32, Code.Xbegin_rel16, 0x1234_5678, 5, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidXbeginRel32_32() {
			var validDiffs = new long[] { (long)int.MinValue, (long)int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 };
			var invalidDiffs = new long[] { };
			TestInvalidXbegin(32, Code.Xbegin_rel32, 0x1234_5678, 6, 0xFFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidXbeginRel16_64() {
			var validDiffs = new long[] { (long)short.MinValue, (long)short.MaxValue, -1, 0, 1, -0x1234, 0x1234 };
			var invalidDiffs = new long[] { (long)short.MinValue - 1, (long)short.MaxValue + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, long.MinValue, long.MaxValue };
			TestInvalidXbegin(64, Code.Xbegin_rel16, 0x1234_5678_9ABC_DEF0, 5, 0xFFFF_FFFF_FFFF_FFFF, validDiffs, invalidDiffs);
		}

		[Fact]
		void TestInvalidXbeginRel32_64() {
			var validDiffs = new long[] { (long)int.MinValue, (long)int.MaxValue, -1, 0, 1, -0x1234_5678, 0x1234_5678 };
			var invalidDiffs = new long[] { (long)int.MinValue - 1, (long)int.MaxValue + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, long.MinValue, long.MaxValue };
			TestInvalidXbegin(64, Code.Xbegin_rel32, 0x1234_5678_9ABC_DEF0, 6, 0xFFFF_FFFF_FFFF_FFFF, validDiffs, invalidDiffs);
		}

		static void TestInvalidXbegin(int bitness, Code code, ulong instrAddr, uint instrLen, ulong addrMask, long[] validDiffs, long[] invalidDiffs) =>
			TestInvalidBr(bitness, code, instrAddr, instrLen, addrMask, validDiffs, invalidDiffs, (code, _, target) => {
				var instr = Instruction.CreateXbegin(bitness, target);
				instr.Code = code;
				return instr;
			});

		static void TestInvalidBr(int bitness, Code code, ulong instrAddr, uint instrLen, ulong addrMask, long[] validDiffs, long[] invalidDiffs,
			Func<Code, int, ulong, Instruction> createInstr) {
			foreach (var diff in validDiffs) {
				var writer = new CodeWriterImpl();
				var encoder = Encoder.Create(bitness, writer);
				var target = (instrAddr + instrLen + (ulong)diff) & addrMask;
				var instr = createInstr(code, bitness, target);
				Assert.True(encoder.TryEncode(instr, instrAddr, out var decodedLen, out _));
				Assert.Equal(instrLen, decodedLen);

				var bytes = writer.ToArray();
				var decoded = Decoder.Create(bitness, new ByteArrayCodeReader(bytes), instrAddr, DecoderOptions.None).Decode();
				Assert.Equal(code, decoded.Code);
				Assert.Equal(target, decoded.NearBranch64);
			}
			foreach (var diff in invalidDiffs) {
				var encoder = Encoder.Create(bitness, new CodeWriterImpl());
				var target = (instrAddr + instrLen + (ulong)diff) & addrMask;
				var instr = createInstr(code, bitness, target);
				Assert.False(encoder.TryEncode(instr, instrAddr, out _, out _));
			}
		}
	}
}
#endif
