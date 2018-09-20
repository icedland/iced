/*
    Copyright (C) 2018 de4dot@gmail.com

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
	/// Jcc instruction
	/// </summary>
	sealed class JccInstr : Instr {
		readonly int bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		BlockData pointerData;
		InstrKind instrKind;
		readonly uint shortInstructionSize;
		readonly uint nearInstructionSize;
		// Code:
		//		!jcc short skip		; negated jcc opcode
		//		jmp qword ptr [rip+mem]
		//	skip:
		const uint longInstructionSize64 = 2 + CallOrJmpPointerDataInstructionSize64;

		enum InstrKind {
			Unchanged,
			Short,
			Near,
			Long,
			Uninitialized,
		}

		public JccInstr(BlockEncoder blockEncoder, ref Instruction instruction)
			: base(blockEncoder, instruction.IP64) {
			bitness = blockEncoder.Bitness;
			this.instruction = instruction;
			instrKind = InstrKind.Uninitialized;

			string errorMessage;

			if (!blockEncoder.FixBranches) {
				instrKind = InstrKind.Unchanged;
				if (!blockEncoder.NullEncoder.TryEncode(ref instruction, instruction.IP64, out Size, out errorMessage))
					Size = DecoderConstants.MaxInstructionLength;
			}
			else {
				Instruction instrCopy;

				instrCopy = instruction;
				instrCopy.Code = instruction.Code.ToShortBranchCode();
				instrCopy.NearBranch64 = 0;
				if (!blockEncoder.NullEncoder.TryEncode(ref instrCopy, 0, out shortInstructionSize, out errorMessage))
					shortInstructionSize = DecoderConstants.MaxInstructionLength;

				instrCopy = instruction;
				instrCopy.Code = instruction.Code.ToNearBranchCode();
				instrCopy.NearBranch64 = 0;
				if (!blockEncoder.NullEncoder.TryEncode(ref instrCopy, 0, out nearInstructionSize, out errorMessage))
					nearInstructionSize = DecoderConstants.MaxInstructionLength;

				if (blockEncoder.Bitness == 64) {
					// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
					Size = Math.Max(nearInstructionSize, longInstructionSize64);
				}
				else
					Size = nearInstructionSize;
			}
		}

		public override void Initialize() {
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);
			TryOptimize();
		}

		public override bool Optimize() => TryOptimize();

		bool TryOptimize() {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Short)
				return false;

			var targetAddress = targetInstr.GetAddress();
			var nextRip = IP + shortInstructionSize;
			long diff = (long)(targetAddress - nextRip);
			if (sbyte.MinValue <= diff && diff <= sbyte.MaxValue) {
				if (pointerData != null)
					pointerData.IsValid = false;
				instrKind = InstrKind.Short;
				Size = shortInstructionSize;
				return true;
			}

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useNear = bitness != 64 || targetInstr.IsInBlock(Block);
			if (!useNear) {
				targetAddress = targetInstr.GetAddress();
				nextRip = IP + nearInstructionSize;
				diff = (long)(targetAddress - nextRip);
				useNear = int.MinValue <= diff && diff <= int.MaxValue;
			}
			if (useNear) {
				if (pointerData != null)
					pointerData.IsValid = false;
				instrKind = InstrKind.Near;
				Size = nearInstructionSize;
				return true;
			}

			if (pointerData == null)
				pointerData = Block.AllocPointerLocation();
			instrKind = InstrKind.Long;
			return false;
		}

		public override string TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			string errorMessage;
			switch (instrKind) {
			case InstrKind.Unchanged:
			case InstrKind.Short:
			case InstrKind.Near:
				isOriginalInstruction = true;
				if (instrKind == InstrKind.Unchanged) {
					// nothing
				}
				else if (instrKind == InstrKind.Short)
					instruction.Code = instruction.Code.ToShortBranchCode();
				else {
					Debug.Assert(instrKind == InstrKind.Near);
					instruction.Code = instruction.Code.ToNearBranchCode();
				}
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(ref instruction, IP, out _, out errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, ref instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;

			case InstrKind.Long:
				Debug.Assert(pointerData != null);
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				var instr = new Instruction();
				instr.Code = instruction.Code.ToNegatedJcc().ToShortBranchCode().ShortJccToNativeJcc(encoder.Bitness);
				instr.Op0Kind = OpKind.NearBranch64;
				Debug.Assert(encoder.Bitness == 64);
				Debug.Assert(longInstructionSize64 <= sbyte.MaxValue);
				instr.NearBranch64 = IP + longInstructionSize64;
				if (!encoder.TryEncode(ref instr, IP, out uint instrLen, out errorMessage))
					return CreateErrorMessage(errorMessage, ref instruction);
				errorMessage = EncodeBranchToPointerData(encoder, isCall: false, IP + (uint)instrLen, pointerData, out _, Size - (uint)instrLen);
				if (errorMessage != null)
					return CreateErrorMessage(errorMessage, ref instruction);
				return null;

			case InstrKind.Uninitialized:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
