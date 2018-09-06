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
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public abstract class BlockEncoderTest {
		internal static Instruction[] Decode(int bitness, ulong rip, byte[] data) {
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(data));
			decoder.InstructionPointer = rip;
			var list = new List<Instruction>();
			while ((decoder.InstructionPointer - rip) < (uint)data.Length)
				list.Add(decoder.Decode());
			if (decoder.InstructionPointer - rip != (uint)data.Length)
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

		protected void EncodeBase(int bitness, ulong origRip, byte[] originalData, ulong newRip, byte[] newData, BlockEncoderOptions options, uint[] expectedInstructionOffsets, RelocInfo[] expectedRelocInfos) {
			var origInstrs = Decode(bitness, origRip, originalData);
			var codeWriter = new CodeWriterImpl();
			var relocInfos = new List<RelocInfo>();
			var newInstructionOffsets = new uint[origInstrs.Length];
			var constantOffsets = new ConstantOffsets[origInstrs.Length];
			for (int i = 0; i < constantOffsets.Length; i++) {
				// Make sure each element gets initialized by Encode()
				constantOffsets[i] = new ConstantOffsets {
					DisplacementOffset = byte.MaxValue,
					ImmediateOffset = byte.MaxValue,
					DisplacementSize = byte.MaxValue,
					ImmediateSize = byte.MaxValue,
				};
			}
			var errorMessage = BlockEncoder.Encode(bitness, new InstructionBlock(codeWriter, origInstrs, newRip, relocInfos, newInstructionOffsets, constantOffsets), options);
			Assert.Null(errorMessage);
			Assert.Equal(Sort(new List<RelocInfo>(expectedRelocInfos)), Sort(relocInfos));
			Assert.Equal(expectedInstructionOffsets, newInstructionOffsets);

			var expectedConstantOffsets = new ConstantOffsets[constantOffsets.Length];
			var reader = new CodeReaderImpl(codeWriter.ToArray());
			var decoder = Decoder.Create(bitness, reader);
			for (int i = 0; i < newInstructionOffsets.Length; i++) {
				if (newInstructionOffsets[i] == uint.MaxValue)
					expectedConstantOffsets[i] = default;
				else {
					reader.Index = (int)newInstructionOffsets[i];
					decoder.InstructionPointer = newRip + newInstructionOffsets[i];
					decoder.Decode(out var instr);
					expectedConstantOffsets[i] = decoder.GetConstantOffsets(ref instr);
				}
			}
			Assert.Equal(expectedConstantOffsets, constantOffsets);
		}
	}
}
#endif
