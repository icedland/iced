// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Xbegin instruction
	/// </summary>
	sealed class XbeginInstr : Instr {
		Instruction instruction;
		TargetInstr targetInstr;
		InstrKind instrKind;
		readonly byte shortInstructionSize;
		readonly byte nearInstructionSize;

		enum InstrKind : byte {
			Unchanged,
			Rel16,
			Rel32,
			Uninitialized,
		}

		public XbeginInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			this.instruction = instruction;
			instrKind = InstrKind.Uninitialized;

			Instruction instrCopy;

			if (!blockEncoder.FixBranches) {
				instrKind = InstrKind.Unchanged;
				instrCopy = instruction;
				instrCopy.NearBranch64 = 0;
				Size = blockEncoder.GetInstructionSize(instrCopy, 0);
			}
			else {
				instrCopy = instruction;
				instrCopy.InternalSetCodeNoCheck(Code.Xbegin_rel16);
				instrCopy.NearBranch64 = 0;
				shortInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);

				instrCopy = instruction;
				instrCopy.InternalSetCodeNoCheck(Code.Xbegin_rel32);
				instrCopy.NearBranch64 = 0;
				nearInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);

				Size = nearInstructionSize;
			}
		}

		public override void Initialize(BlockEncoder blockEncoder) =>
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);

		public override bool Optimize(ulong gained) => TryOptimize(gained);

		bool TryOptimize(ulong gained) {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Rel16) {
				Done = true;
				return false;
			}

			var targetAddress = targetInstr.GetAddress();
			var nextRip = IP + shortInstructionSize;
			long diff = (long)(targetAddress - nextRip);
			diff = CorrectDiff(targetInstr.IsInBlock(Block), diff, gained);
			if (short.MinValue <= diff && diff <= short.MaxValue) {
				instrKind = InstrKind.Rel16;
				Size = shortInstructionSize;
				return true;
			}

			instrKind = InstrKind.Rel32;
			Size = nearInstructionSize;
			return false;
		}

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			switch (instrKind) {
			case InstrKind.Unchanged:
			case InstrKind.Rel16:
			case InstrKind.Rel32:
				isOriginalInstruction = true;
				if (instrKind == InstrKind.Unchanged) {
					// nothing
				}
				else if (instrKind == InstrKind.Rel16)
					instruction.InternalSetCodeNoCheck(Code.Xbegin_rel16);
				else {
					Debug.Assert(instrKind == InstrKind.Rel32);
					instruction.InternalSetCodeNoCheck(Code.Xbegin_rel32);
				}
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(instruction, IP, out _, out var errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;

			case InstrKind.Uninitialized:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
