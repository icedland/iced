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
#nullable enable
using System;

namespace Iced.Intel
{
	/// <summary>
	/// Result of <see cref="Assembler.Encode"/>.
	/// </summary>
	public readonly struct AssemblerResult {
		
		/// <summary>
		/// Creates a new instance of 
		/// </summary>
		/// <param name="baseRIP">Base RIP address.</param>
		/// <param name="blockEncoderResult"></param>
		public AssemblerResult(ulong baseRIP, BlockEncoderResult blockEncoderResult) {
			BaseRIP = baseRIP;
			BlockEncoderResult = blockEncoderResult;
		}

		/// <summary>
		/// The base RIP address.
		/// </summary>
		public readonly ulong BaseRIP;
		
		/// <summary>
		/// The associated block encoder result.
		/// </summary>
		public readonly BlockEncoderResult BlockEncoderResult;

		/// <summary>
		/// Gets the RIP of the specified label.
		/// </summary>
		/// <param name="label">A label.</param>
		/// <returns>RIP of the label.</returns>
		/// <exception cref="ArgumentException">Invalid label not created via <see cref="Assembler.CreateLabel"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The label is not associated with an instruction index or the instruction index is out of bounds.</exception>
		public ulong GetLabelRIP(in Label label) {
			if (label.IsEmpty) throw new ArgumentException($"Invalid label. Must be created via {nameof(Assembler)}.{nameof(Assembler.CreateLabel)}", nameof(label));
			if (label.InstructionIndex < 0) throw new ArgumentException($"The label is not associated with an instruction index. It must be emitted via {nameof(Assembler)}.{nameof(Assembler.Label)}.", nameof(label));
			if (BlockEncoderResult.NewInstructionOffsets == null || label.InstructionIndex >= BlockEncoderResult.NewInstructionOffsets.Length) throw new ArgumentOutOfRangeException(nameof(label), $"The label instruction index {label.InstructionIndex} is out of range of the instruction offsets results {BlockEncoderResult.NewInstructionOffsets?.Length ?? 0}. Did you forget to pass {nameof(BlockEncoderOptions)}.{nameof(BlockEncoderOptions.ReturnNewInstructionOffsets)} to {nameof(Assembler)}.{nameof(Assembler.Encode)}/{nameof(Assembler.TryEncode)}?");
			return BaseRIP + BlockEncoderResult.NewInstructionOffsets[label.InstructionIndex];
		}
	}
}
#endif
