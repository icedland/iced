/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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
namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Simple instruction that doesn't need fixing, i.e., it's not IP relative (no branch instruction, no IP relative memory operand)
	/// </summary>
	sealed class SimpleInstr : Instr {
		Instruction instruction;

		public SimpleInstr(BlockEncoder blockEncoder, ref Instruction instruction)
			: base(blockEncoder, instruction.IP64) {
			this.instruction = instruction;
			if (!blockEncoder.NullEncoder.TryEncode(ref instruction, instruction.IP64, out Size, out var errorMessage))
				Size = DecoderConstants.MaxInstructionLength;
		}

		public override void Initialize() { }
		public override bool Optimize() => false;

		public override string TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			isOriginalInstruction = true;
			if (!encoder.TryEncode(ref instruction, IP, out _, out var errorMessage)) {
				constantOffsets = default;
				return CreateErrorMessage(errorMessage, ref instruction);
			}
			constantOffsets = encoder.GetConstantOffsets();
			return null;
		}
	}
}
#endif
