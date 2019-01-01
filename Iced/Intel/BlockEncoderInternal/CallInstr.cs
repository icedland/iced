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
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Call near instruction
	/// </summary>
	sealed class CallInstr : Instr {
		readonly int bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		readonly uint origInstructionSize;
		BlockData pointerData;
		bool useOrigInstruction;
		bool done;

		public CallInstr(BlockEncoder blockEncoder, ref Instruction instruction)
			: base(blockEncoder, instruction.IP64) {
			bitness = blockEncoder.Bitness;
			this.instruction = instruction;
			var instrCopy = instruction;
			instrCopy.NearBranch64 = 0;
			if (!blockEncoder.NullEncoder.TryEncode(ref instrCopy, 0, out origInstructionSize, out var errorMessage))
				origInstructionSize = DecoderConstants.MaxInstructionLength;
			if (!blockEncoder.FixBranches) {
				Size = origInstructionSize;
				useOrigInstruction = true;
				done = true;
			}
			else if (blockEncoder.Bitness == 64) {
				// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
				Size = Math.Max(origInstructionSize, CallOrJmpPointerDataInstructionSize64);
			}
			else
				Size = origInstructionSize;
		}

		public override void Initialize() {
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);
			TryOptimize();
		}

		public override bool Optimize() => TryOptimize();

		bool TryOptimize() {
			if (done)
				return false;

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useShort = bitness != 64 || targetInstr.IsInBlock(Block);
			if (!useShort) {
				var targetAddress = targetInstr.GetAddress();
				var nextRip = IP + origInstructionSize;
				long diff = (long)(targetAddress - nextRip);
				useShort = int.MinValue <= diff && diff <= int.MaxValue;
			}

			if (useShort) {
				if (pointerData != null)
					pointerData.IsValid = false;
				Size = origInstructionSize;
				useOrigInstruction = true;
				done = true;
				return true;
			}

			if (pointerData == null)
				pointerData = Block.AllocPointerLocation();
			return false;
		}

		public override string TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			if (useOrigInstruction) {
				isOriginalInstruction = true;
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(ref instruction, IP, out _, out var errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, ref instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;
			}
			else {
				Debug.Assert(pointerData != null);
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				var errorMessage = EncodeBranchToPointerData(encoder, isCall: true, IP, pointerData, out var size, Size);
				if (errorMessage != null)
					return CreateErrorMessage(errorMessage, ref instruction);
				return null;
			}
		}
	}
}
#endif
