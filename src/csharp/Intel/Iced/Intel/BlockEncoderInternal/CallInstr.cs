// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Call near instruction
	/// </summary>
	sealed class CallInstr : Instr {
		readonly byte bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		readonly byte origInstructionSize;
		BlockData? pointerData;
		bool useOrigInstruction;

		public CallInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			bitness = (byte)blockEncoder.Bitness;
			this.instruction = instruction;
			var instrCopy = instruction;
			instrCopy.NearBranch64 = 0;
			origInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);
			if (!blockEncoder.FixBranches) {
				Size = origInstructionSize;
				useOrigInstruction = true;
			}
			else if (blockEncoder.Bitness == 64) {
				// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
				Size = Math.Max(origInstructionSize, CallOrJmpPointerDataInstructionSize64);
			}
			else
				Size = origInstructionSize;
		}

		public override void Initialize(BlockEncoder blockEncoder) =>
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);

		public override bool Optimize(ulong gained) => TryOptimize(gained);

		bool TryOptimize(ulong gained) {
			if (Done || useOrigInstruction) {
				Done = true;
				return false;
			}

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useShort = bitness != 64 || targetInstr.IsInBlock(Block);
			if (!useShort) {
				var targetAddress = targetInstr.GetAddress();
				var nextRip = IP + origInstructionSize;
				long diff = (long)(targetAddress - nextRip);
				diff = CorrectDiff(targetInstr.IsInBlock(Block), diff, gained);
				useShort = int.MinValue <= diff && diff <= int.MaxValue;
			}

			if (useShort) {
				if (pointerData is not null)
					pointerData.IsValid = false;
				Size = origInstructionSize;
				useOrigInstruction = true;
				Done = true;
				return true;
			}

			if (pointerData is null)
				pointerData = Block.AllocPointerLocation();
			return false;
		}

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			if (useOrigInstruction) {
				isOriginalInstruction = true;
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(instruction, IP, out _, out var errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;
			}
			else {
				Debug2.Assert(pointerData is not null);
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				var errorMessage = EncodeBranchToPointerData(encoder, isCall: true, IP, pointerData, out _, Size);
				if (errorMessage is not null)
					return CreateErrorMessage(errorMessage, instruction);
				return null;
			}
		}
	}
}
#endif
