// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Jmp instruction
	/// </summary>
	sealed class JmpInstr : Instr {
		readonly byte bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		BlockData? pointerData;
		InstrKind instrKind;
		readonly byte shortInstructionSize;
		readonly byte nearInstructionSize;

		enum InstrKind : byte {
			Unchanged,
			Short,
			Near,
			Long,
			Uninitialized,
		}

		public JmpInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			bitness = (byte)blockEncoder.Bitness;
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
				instrCopy.InternalSetCodeNoCheck(instruction.Code.ToShortBranch());
				instrCopy.NearBranch64 = 0;
				shortInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);

				instrCopy = instruction;
				instrCopy.InternalSetCodeNoCheck(instruction.Code.ToNearBranch());
				instrCopy.NearBranch64 = 0;
				nearInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);

				if (blockEncoder.Bitness == 64) {
					// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
					Size = Math.Max(nearInstructionSize, CallOrJmpPointerDataInstructionSize64);
				}
				else
					Size = nearInstructionSize;
			}
		}

		public override void Initialize(BlockEncoder blockEncoder) =>
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);

		public override bool Optimize(ulong gained) => TryOptimize(gained);

		bool TryOptimize(ulong gained) {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Short) {
				Done = true;
				return false;
			}

			var targetAddress = targetInstr.GetAddress();
			var nextRip = IP + shortInstructionSize;
			long diff = (long)(targetAddress - nextRip);
			diff = ConvertDiffToBitnessDiff(bitness, CorrectDiff(targetInstr.IsInBlock(Block), diff, gained));
			if (sbyte.MinValue <= diff && diff <= sbyte.MaxValue) {
				if (pointerData is not null)
					pointerData.IsValid = false;
				instrKind = InstrKind.Short;
				Size = shortInstructionSize;
				Done = true;
				return true;
			}

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useNear = bitness != 64 || targetInstr.IsInBlock(Block);
			if (!useNear) {
				targetAddress = targetInstr.GetAddress();
				nextRip = IP + nearInstructionSize;
				diff = (long)(targetAddress - nextRip);
				diff = ConvertDiffToBitnessDiff(bitness, CorrectDiff(targetInstr.IsInBlock(Block), diff, gained));
				useNear = int.MinValue <= diff && diff <= int.MaxValue;
			}
			if (useNear) {
				if (pointerData is not null)
					pointerData.IsValid = false;
				if (diff < (long)IcedConstants.MaxInstructionLength * sbyte.MinValue ||
					diff > (long)IcedConstants.MaxInstructionLength * sbyte.MaxValue) {
					Done = true;
				}
				instrKind = InstrKind.Near;
				Size = nearInstructionSize;
				return true;
			}

			if (pointerData is null)
				pointerData = Block.AllocPointerLocation();
			instrKind = InstrKind.Long;
			return false;
		}

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			string? errorMessage;
			switch (instrKind) {
			case InstrKind.Unchanged:
			case InstrKind.Short:
			case InstrKind.Near:
				isOriginalInstruction = true;
				if (instrKind == InstrKind.Unchanged) {
					// nothing
				}
				else if (instrKind == InstrKind.Short)
					instruction.InternalSetCodeNoCheck(instruction.Code.ToShortBranch());
				else {
					Debug.Assert(instrKind == InstrKind.Near);
					instruction.InternalSetCodeNoCheck(instruction.Code.ToNearBranch());
				}
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(instruction, IP, out _, out errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;

			case InstrKind.Long:
				Debug2.Assert(pointerData is not null);
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				errorMessage = EncodeBranchToPointerData(encoder, isCall: false, IP, pointerData, out _, Size);
				if (errorMessage is not null)
					return CreateErrorMessage(errorMessage, instruction);
				return null;

			case InstrKind.Uninitialized:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
