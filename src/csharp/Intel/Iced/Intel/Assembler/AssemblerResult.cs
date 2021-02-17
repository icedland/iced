// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
