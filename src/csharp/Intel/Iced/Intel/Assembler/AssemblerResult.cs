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

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;

namespace Iced.Intel {
	/// <summary>
	/// Result of <see cref="Assembler.Assemble"/>.
	/// </summary>
	public readonly struct AssemblerResult {
		internal AssemblerResult(BlockEncoderResult[] result) => Result = result;

		/// <summary>
		/// The associated block encoder result.
		/// </summary>
		public readonly BlockEncoderResult[] Result;

		/// <summary>
		/// Gets the RIP of the specified label.
		/// </summary>
		/// <param name="label">A label.</param>
		/// <param name="index">Result index</param>
		/// <returns>RIP of the label.</returns>
		public ulong GetLabelRIP(in Label label, int index = 0) {
			if (label.IsEmpty)
				throw new ArgumentException($"Invalid label. Must be created via {nameof(Assembler)}.{nameof(Assembler.CreateLabel)}", nameof(label));
			if (label.InstructionIndex < 0)
				throw new ArgumentException($"The label is not associated with an instruction index. It must be emitted via {nameof(Assembler)}.{nameof(Assembler.Label)}.", nameof(label));
			if (Result is null || (uint)index >= (uint)Result.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			var result = Result[index];
			if (result.NewInstructionOffsets is null || label.InstructionIndex >= result.NewInstructionOffsets.Length)
				throw new ArgumentOutOfRangeException(nameof(label), $"The label instruction index {label.InstructionIndex} is out of range of the instruction offsets results {result.NewInstructionOffsets?.Length ?? 0}. Did you forget to pass {nameof(BlockEncoderOptions)}.{nameof(BlockEncoderOptions.ReturnNewInstructionOffsets)} to {nameof(Assembler)}.{nameof(Assembler.Assemble)}/{nameof(Assembler.TryAssemble)}?");
			return result.RIP + result.NewInstructionOffsets[label.InstructionIndex];
		}
	}
}
#endif
