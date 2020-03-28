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

#if ENCODER && BLOCK_ENCODER
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
		BlockData? pointerData;
		bool useOrigInstruction;
		bool done;

		public CallInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			bitness = blockEncoder.Bitness;
			this.instruction = instruction;
			var instrCopy = instruction;
			instrCopy.NearBranch64 = 0;
			origInstructionSize = blockEncoder.GetInstructionSize(instrCopy, 0);
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

		public override void Initialize(BlockEncoder blockEncoder) {
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
				if (!(pointerData is null))
					pointerData.IsValid = false;
				Size = origInstructionSize;
				useOrigInstruction = true;
				done = true;
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
				Debug2.Assert(!(pointerData is null));
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				var errorMessage = EncodeBranchToPointerData(encoder, isCall: true, IP, pointerData, out _, Size);
				if (!(errorMessage is null))
					return CreateErrorMessage(errorMessage, instruction);
				return null;
			}
		}
	}
}
#endif
