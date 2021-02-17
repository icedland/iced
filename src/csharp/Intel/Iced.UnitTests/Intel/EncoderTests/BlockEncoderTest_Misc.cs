// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest_Misc {
		[Fact]
		void Encode_zero_blocks() {
			bool b;
			string errorMessage;
			BlockEncoderResult[] result;

			b = BlockEncoder.TryEncode(16, new InstructionBlock[0], out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.NotNull(result);
			Assert.True(result.Length == 0);

			b = BlockEncoder.TryEncode(32, new InstructionBlock[0], out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.NotNull(result);
			Assert.True(result.Length == 0);

			b = BlockEncoder.TryEncode(64, new InstructionBlock[0], out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.NotNull(result);
			Assert.True(result.Length == 0);
		}

		[Fact]
		void Encode_zero_instructions() {
			bool b;
			string errorMessage;
			BlockEncoderResult result;
			var codeWriter = new CodeWriterImpl();

			b = BlockEncoder.TryEncode(16, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0), out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());
			Assert.Equal(0UL, result.RIP);
			Assert.Null(result.RelocInfos);
			Assert.NotNull(result.NewInstructionOffsets);
			Assert.True(result.NewInstructionOffsets.Length == 0);
			Assert.NotNull(result.ConstantOffsets);
			Assert.True(result.ConstantOffsets.Length == 0);

			b = BlockEncoder.TryEncode(32, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0), out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());
			Assert.Equal(0UL, result.RIP);
			Assert.Null(result.RelocInfos);
			Assert.NotNull(result.NewInstructionOffsets);
			Assert.True(result.NewInstructionOffsets.Length == 0);
			Assert.NotNull(result.ConstantOffsets);
			Assert.True(result.ConstantOffsets.Length == 0);

			b = BlockEncoder.TryEncode(64, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0), out errorMessage, out result, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());
			Assert.Equal(0UL, result.RIP);
			Assert.Null(result.RelocInfos);
			Assert.NotNull(result.NewInstructionOffsets);
			Assert.True(result.NewInstructionOffsets.Length == 0);
			Assert.NotNull(result.ConstantOffsets);
			Assert.True(result.ConstantOffsets.Length == 0);
		}

		[Fact]
		void DefaultArgs() {
			const int bitness = 64;
			const ulong origRip = 0x123456789ABCDE00;
			const ulong newRip = 0x8000000000000000;

			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 123456789ABCDE0Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x03, 0x00, 0x00, 0x00,// jmp near ptr 123456789ABCDE0Eh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var instructions = BlockEncoderTest.Decode(bitness, origRip, originalData, DecoderOptions.None);
			var codeWriter = new CodeWriterImpl();
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, instructions, newRip), out var errorMessage, out var result);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Equal(newRip, result.RIP);
			Assert.Equal(0x28, codeWriter.ToArray().Length);
			Assert.Null(result.RelocInfos);
			Assert.NotNull(result.NewInstructionOffsets);
			Assert.True(result.NewInstructionOffsets.Length == 0);
			Assert.NotNull(result.ConstantOffsets);
			Assert.True(result.ConstantOffsets.Length == 0);
		}

		[Theory]
		[InlineData(BlockEncoderOptions.ReturnRelocInfos)]
		[InlineData(BlockEncoderOptions.ReturnNewInstructionOffsets)]
		[InlineData(BlockEncoderOptions.ReturnConstantOffsets)]
		void VerifyResultArrays(BlockEncoderOptions options) {
			const int bitness = 64;
			const ulong origRip1 = 0x123456789ABCDE00;
			const ulong origRip2 = 0x223456789ABCDE00;
			const ulong newRip1 = 0x8000000000000000;
			const ulong newRip2 = 0x9000000000000000;

			{
				var instructions1 = BlockEncoderTest.Decode(bitness, origRip1, new byte[] { 0xE9, 0x56, 0x78, 0xA5, 0x5A }, DecoderOptions.None);
				var codeWriter1 = new CodeWriterImpl();
				bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter1, instructions1, newRip1), out var errorMessage, out var result, options);
				Assert.True(b);
				Assert.Null(errorMessage);
				Assert.Equal(newRip1, result.RIP);
				if ((options & BlockEncoderOptions.ReturnRelocInfos) != 0) {
					Assert.NotNull(result.RelocInfos);
					Assert.True(result.RelocInfos.Count == 1);
				}
				else
					Assert.Null(result.RelocInfos);
				if ((options & BlockEncoderOptions.ReturnNewInstructionOffsets) != 0) {
					Assert.NotNull(result.NewInstructionOffsets);
					Assert.True(result.NewInstructionOffsets.Length == 1);
				}
				else {
					Assert.NotNull(result.NewInstructionOffsets);
					Assert.True(result.NewInstructionOffsets.Length == 0);
				}
				if ((options & BlockEncoderOptions.ReturnConstantOffsets) != 0) {
					Assert.NotNull(result.ConstantOffsets);
					Assert.True(result.ConstantOffsets.Length == 1);
				}
				else {
					Assert.NotNull(result.ConstantOffsets);
					Assert.True(result.ConstantOffsets.Length == 0);
				}
			}
			{
				var instructions1 = BlockEncoderTest.Decode(bitness, origRip1, new byte[] { 0xE9, 0x56, 0x78, 0xA5, 0x5A }, DecoderOptions.None);
				var instructions2 = BlockEncoderTest.Decode(bitness, origRip2, new byte[] { 0x90, 0xE9, 0x56, 0x78, 0xA5, 0x5A }, DecoderOptions.None);
				var codeWriter1 = new CodeWriterImpl();
				var codeWriter2 = new CodeWriterImpl();
				var block1 = new InstructionBlock(codeWriter1, instructions1, newRip1);
				var block2 = new InstructionBlock(codeWriter2, instructions2, newRip2);
				bool b = BlockEncoder.TryEncode(bitness, new[] { block1, block2 }, out var errorMessage, out var resultArray, options);
				Assert.True(b);
				Assert.Null(errorMessage);
				Assert.NotNull(resultArray);
				Assert.Equal(2, resultArray.Length);
				Assert.Equal(newRip1, resultArray[0].RIP);
				Assert.Equal(newRip2, resultArray[1].RIP);
				if ((options & BlockEncoderOptions.ReturnRelocInfos) != 0) {
					Assert.NotNull(resultArray[0].RelocInfos);
					Assert.NotNull(resultArray[1].RelocInfos);
					Assert.True(resultArray[0].RelocInfos.Count == 1);
					Assert.True(resultArray[1].RelocInfos.Count == 1);
				}
				else {
					Assert.Null(resultArray[0].RelocInfos);
					Assert.Null(resultArray[1].RelocInfos);
				}
				if ((options & BlockEncoderOptions.ReturnNewInstructionOffsets) != 0) {
					Assert.NotNull(resultArray[0].NewInstructionOffsets);
					Assert.NotNull(resultArray[1].NewInstructionOffsets);
					Assert.True(resultArray[0].NewInstructionOffsets.Length == 1);
					Assert.True(resultArray[1].NewInstructionOffsets.Length == 2);
				}
				else {
					Assert.NotNull(resultArray[0].NewInstructionOffsets);
					Assert.True(resultArray[0].NewInstructionOffsets.Length == 0);
					Assert.NotNull(resultArray[1].NewInstructionOffsets);
					Assert.True(resultArray[1].NewInstructionOffsets.Length == 0);
				}
				if ((options & BlockEncoderOptions.ReturnConstantOffsets) != 0) {
					Assert.NotNull(resultArray[0].ConstantOffsets);
					Assert.NotNull(resultArray[1].ConstantOffsets);
					Assert.True(resultArray[0].ConstantOffsets.Length == 1);
					Assert.True(resultArray[1].ConstantOffsets.Length == 2);
				}
				else {
					Assert.NotNull(resultArray[0].ConstantOffsets);
					Assert.True(resultArray[0].ConstantOffsets.Length == 0);
					Assert.NotNull(resultArray[1].ConstantOffsets);
					Assert.True(resultArray[1].ConstantOffsets.Length == 0);
				}
			}
		}

		[Theory]
		[InlineData("5A")]
		[InlineData("F0 D2 7A 18 A0")]
		[InlineData("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08")]
		void EncodeDeclareByte(string hexBytes) {
			const int bitness = 64;
			const ulong newRip = 0x8000000000000000;

			var data = HexUtils.ToByteArray(hexBytes);
			var instructions = new Instruction[] {
				Instruction.Create(Code.Nopd),
				Instruction.CreateDeclareByte(data),
				Instruction.Create(Code.Nopd),
			};

			var expectedData = new byte[data.Length + 2];
			expectedData[0] = 0x90;
			Array.Copy(data, 0, expectedData, 1, data.Length);
			expectedData[expectedData.Length - 1] = 0x90;

			var codeWriter = new CodeWriterImpl();
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, instructions, newRip), out var errorMessage, out var result);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Equal(expectedData, codeWriter.ToArray());
			Assert.Equal(newRip, result.RIP);
			Assert.Null(result.RelocInfos);
			Assert.NotNull(result.NewInstructionOffsets);
			Assert.True(result.NewInstructionOffsets.Length == 0);
			Assert.NotNull(result.ConstantOffsets);
			Assert.True(result.ConstantOffsets.Length == 0);
		}

		[Fact]
		void TryEncode_with_default_InstructionBlock_throws() {
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, default(InstructionBlock), out _, out _));
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock[3], out _, out _));
		}

		[Fact]
		void TryEncode_with_null_array_throws() =>
			Assert.Throws<ArgumentNullException>(() => BlockEncoder.TryEncode(64, null, out _, out _));

		[Fact]
		void TryEncode_with_invalid_bitness_throws() {
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => BlockEncoder.TryEncode(bitness, new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0), out _, out _));
			foreach (var bitness in BitnessUtils.GetInvalidBitnessValues())
				Assert.Throws<ArgumentOutOfRangeException>(() => BlockEncoder.TryEncode(bitness, new[] { new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0) }, out _, out _));
		}

		[Fact]
		void InstructionBlock_throws_if_invalid_input() {
			Assert.Throws<ArgumentNullException>(() => new InstructionBlock(null, Array.Empty<Instruction>(), 0));
			Assert.Throws<ArgumentNullException>(() => new InstructionBlock(new CodeWriterImpl(), null, 0));
		}

		[Fact]
		void EncodeRipRelMemOp() {
			var instr = Instruction.Create(Code.Add_r32_rm32, Register.ECX, new MemoryOperand(Register.RIP, Register.None, 1, 0x1234_5678_9ABC_DEF1, 8, false, Register.None));
			var codeWriter = new CodeWriterImpl();
			bool b = BlockEncoder.TryEncode(64, new InstructionBlock(codeWriter, new[] { instr }, 0x1234_5678_ABCD_EF02), out var errorMessage, out _);
			Assert.True(b, $"Couldn't encode it: {errorMessage}");
			var encoded = codeWriter.ToArray();
			var expected = new byte[] { 0x03, 0x0D, 0xE9, 0xEF, 0xEE, 0xEE };
			Assert.Equal(expected, encoded);
		}
	}
}
#endif
