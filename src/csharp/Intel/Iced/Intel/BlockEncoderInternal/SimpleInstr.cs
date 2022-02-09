// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Simple instruction that doesn't need fixing, i.e., it's not IP relative (no branch instruction, no IP relative memory operand)
	/// </summary>
	sealed class SimpleInstr : Instr {
		Instruction instruction;

		public SimpleInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			Done = true;
			this.instruction = instruction;
			Size = blockEncoder.GetInstructionSize(instruction, instruction.IP);
		}

		public override void Initialize(BlockEncoder blockEncoder) {}
		public override bool Optimize(ulong gained) => false;

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			isOriginalInstruction = true;
			if (!encoder.TryEncode(instruction, IP, out _, out var errorMessage)) {
				constantOffsets = default;
				return CreateErrorMessage(errorMessage, instruction);
			}
			constantOffsets = encoder.GetConstantOffsets();
			return null;
		}
	}
}
#endif
