// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
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
			options |= BlockEncoderOptions.ReturnRelocInfos | BlockEncoderOptions.ReturnNewInstructionOffsets | BlockEncoderOptions.ReturnConstantOffsets;
			bool b = BlockEncoder.TryEncode(bitness, new InstructionBlock(codeWriter, origInstrs, newRip), out var errorMessage, out var result, options);
			Assert.True(b);
			Assert.Null(errorMessage);
			var encodedBytes = codeWriter.ToArray();
			Assert.Equal(newData, encodedBytes);
			Assert.Equal(newRip, result.RIP);
			var relocInfos = result.RelocInfos;
			var newInstructionOffsets = result.NewInstructionOffsets;
			var constantOffsets = result.ConstantOffsets;
			Assert.NotNull(relocInfos);
			Assert.NotNull(newInstructionOffsets);
			Assert.Equal(origInstrs.Length, newInstructionOffsets.Length);
			Assert.NotNull(constantOffsets);
			Assert.Equal(origInstrs.Length, constantOffsets.Length);
			Assert.Equal(Sort(new List<RelocInfo>(expectedRelocInfos)), Sort(relocInfos));
			Assert.Equal(expectedInstructionOffsets, newInstructionOffsets);

			var expectedConstantOffsets = new ConstantOffsets[constantOffsets.Length];
			var reader = new CodeReaderImpl(encodedBytes);
			var decoder = Decoder.Create(bitness, reader, decoderOptions);
			for (int i = 0; i < newInstructionOffsets.Length; i++) {
				if (newInstructionOffsets[i] == uint.MaxValue)
					expectedConstantOffsets[i] = default;
				else {
					reader.Index = (int)newInstructionOffsets[i];
					decoder.IP = newRip + newInstructionOffsets[i];
					decoder.Decode(out var instruction);
					expectedConstantOffsets[i] = decoder.GetConstantOffsets(instruction);
				}
			}
			Assert.Equal(expectedConstantOffsets, constantOffsets);
		}
	}
}
#endif
