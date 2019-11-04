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
	public abstract class BlockEncoderTest {
		protected const DecoderOptions decoderOptions = DecoderOptions.None;

		internal static Instruction[] Decode(int bitness, ulong rip, byte[] data, DecoderOptions options) {
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(data), options);
			decoder.IP = rip;
			var list = new List<Instruction>();
			while ((decoder.IP - rip) < (uint)data.Length)
				list.Add(decoder.Decode());
			if (decoder.IP - rip != (uint)data.Length)
				throw new InvalidOperationException();
			return list.ToArray();
		}

		static List<RelocInfo> Sort(List<RelocInfo> list) {
			list.Sort((a, b) => {
				int c = a.Address.CompareTo(b.Address);
				if (c != 0)
					return c;
				return (int)a.Kind - (int)b.Kind;
			});
			return list;
		}

		sealed class CodeReaderImpl : CodeReader {
			readonly byte[] data;
			public int Index;

			public CodeReaderImpl(byte[] data) => this.data = data;

			public override int ReadByte() {
				if ((uint)Index >= (uint)data.Length)
					return -1;
				return data[Index++];
			}
		}

		protected void EncodeBase(int bitness, ulong origRip, byte[] originalData, ulong newRip, byte[] newData, BlockEncoderOptions options, DecoderOptions decoderOptions, uint[] expectedInstructionOffsets, RelocInfo[] expectedRelocInfos) {
			var origInstrs = Decode(bitness, origRip, originalData, decoderOptions);
			var codeWriter = new CodeWriterImpl();
			var relocInfos = new List<RelocInfo>();
			var newInstructionOffsets = new uint[origInstrs.Length];
			var constantOffsets = new ConstantOffsets[origInstrs.Length];
			for (int i = 0; i < constantOffsets.Length; i++) {
				// Make sure each element gets initialized by Encode()
				constantOffsets[i] = new ConstantOffsets {
					DisplacementOffset = byte.MaxValue,
					ImmediateOffset = byte.MaxValue,
					ImmediateOffset2 = byte.MaxValue,
					DisplacementSize = byte.MaxValue,
					ImmediateSize = byte.MaxValue,
					ImmediateSize2 = byte.MaxValue,
				};
			}
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, origInstrs, newRip, relocInfos, newInstructionOffsets, constantOffsets), out var errorMessage, options);
			Assert.True(b);
			Assert.Null(errorMessage);
			Assert.Equal(Sort(new List<RelocInfo>(expectedRelocInfos)), Sort(relocInfos));
			Assert.Equal(expectedInstructionOffsets, newInstructionOffsets);

			var expectedConstantOffsets = new ConstantOffsets[constantOffsets.Length];
			var reader = new CodeReaderImpl(codeWriter.ToArray());
			var decoder = Decoder.Create(bitness, reader, decoderOptions);
			for (int i = 0; i < newInstructionOffsets.Length; i++) {
				if (newInstructionOffsets[i] == uint.MaxValue)
					expectedConstantOffsets[i] = default;
				else {
					reader.Index = (int)newInstructionOffsets[i];
					decoder.IP = newRip + newInstructionOffsets[i];
					decoder.Decode(out var instr);
					expectedConstantOffsets[i] = decoder.GetConstantOffsets(instr);
				}
			}
			Assert.Equal(expectedConstantOffsets, constantOffsets);
		}
	}
}
#endif
