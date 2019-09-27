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
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest_Misc {
		[Fact]
		void Encode_zero_blocks() {
			bool b;
			string errorMessage;

			b = BlockEncoder.TryEncode(16, new InstructionBlock[0], out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);

			b = BlockEncoder.TryEncode(32, new InstructionBlock[0], out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);

			b = BlockEncoder.TryEncode(64, new InstructionBlock[0], out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
		}

		[Fact]
		void Encode_zero_instructions() {
			bool b;
			string errorMessage;
			var codeWriter = new CodeWriterImpl();

			b = BlockEncoder.TryEncode(16, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0, Array.Empty<RelocInfo>(), Array.Empty<uint>(), Array.Empty<ConstantOffsets>()), out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());

			b = BlockEncoder.TryEncode(32, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0, Array.Empty<RelocInfo>(), Array.Empty<uint>(), Array.Empty<ConstantOffsets>()), out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());

			b = BlockEncoder.TryEncode(64, new InstructionBlock(codeWriter, Array.Empty<Instruction>(), 0, Array.Empty<RelocInfo>(), Array.Empty<uint>(), Array.Empty<ConstantOffsets>()), out errorMessage, BlockEncoderOptions.None);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Empty(codeWriter.ToArray());
		}

		[Fact]
		void Invalid_NewInstructionOffsets_Throws() {
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock(new CodeWriterImpl(), new Instruction[0], 0, Array.Empty<RelocInfo>(), new uint[1], null), out _, BlockEncoderOptions.None));
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0, Array.Empty<RelocInfo>(), new uint[0], null), out _, BlockEncoderOptions.None));
		}

		[Fact]
		void Invalid_ConstantOffsets_Throws() {
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock(new CodeWriterImpl(), new Instruction[0], 0, Array.Empty<RelocInfo>(), null, new ConstantOffsets[1]), out _, BlockEncoderOptions.None));
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0, Array.Empty<RelocInfo>(), null, new ConstantOffsets[0]), out _, BlockEncoderOptions.None));
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
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, instructions, newRip), out var errorMessage);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Equal(0x28, codeWriter.ToArray().Length);
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
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, instructions, newRip), out var errorMessage);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Equal(expectedData, codeWriter.ToArray());
		}

		[Fact]
		void TryEncode_with_default_InstructionBlock_throws() {
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, default(InstructionBlock), out _));
			Assert.Throws<ArgumentException>(() => BlockEncoder.TryEncode(64, new InstructionBlock[3], out _));
		}

		[Fact]
		void TryEncode_with_null_array_throws() =>
			Assert.Throws<ArgumentNullException>(() => BlockEncoder.TryEncode(64, null, out _));

		[Fact]
		void TryEncode_with_invalid_bitness_throws() {
			static IEnumerable<int> GetEncoderBitness() {
				yield return int.MinValue;
				yield return int.MaxValue;
				for (int bitness = -1; bitness <= 128; bitness++) {
					if (bitness == 16 || bitness == 32 || bitness == 64)
						continue;
					yield return bitness;
				}
			}
			foreach (var bitness in GetEncoderBitness())
				Assert.Throws<ArgumentOutOfRangeException>(() => BlockEncoder.TryEncode(bitness, new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0), out _));
			foreach (var bitness in GetEncoderBitness())
				Assert.Throws<ArgumentOutOfRangeException>(() => BlockEncoder.TryEncode(bitness, new[] { new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0) }, out _));
		}

		[Fact]
		void InstructionBlock_throws_if_invalid_input() {
			Assert.Throws<ArgumentNullException>(() => new InstructionBlock(null, Array.Empty<Instruction>(), 0));
			Assert.Throws<ArgumentNullException>(() => new InstructionBlock(new CodeWriterImpl(), null, 0));
			Assert.Throws<ArgumentException>(() => new InstructionBlock(new CodeWriterImpl(), new Instruction[2], 0, null, new uint[1]));
			Assert.Throws<ArgumentException>(() => new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0, null, new uint[2]));
			Assert.Throws<ArgumentException>(() => new InstructionBlock(new CodeWriterImpl(), new Instruction[1], 0, null, null, new ConstantOffsets[2]));
			Assert.Throws<ArgumentException>(() => new InstructionBlock(new CodeWriterImpl(), new Instruction[2], 0, null, null, new ConstantOffsets[1]));
		}
	}
}
#endif
