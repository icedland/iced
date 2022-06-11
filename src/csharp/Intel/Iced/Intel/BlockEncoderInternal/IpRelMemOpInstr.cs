// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Instruction with a memory operand that is RIP/EIP relative
	/// </summary>
	sealed class IpRelMemOpInstr : Instr {
		Instruction instruction;
		InstrKind instrKind;
		readonly byte eipInstructionSize;
		readonly byte ripInstructionSize;
		TargetInstr targetInstr;

		enum InstrKind : byte {
			Unchanged,
			Rip,
			Eip,
			Long,
			Uninitialized,
		}

		public IpRelMemOpInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			Debug.Assert(instruction.IsIPRelativeMemoryOperand);
			this.instruction = instruction;
			instrKind = InstrKind.Uninitialized;

			var instrCopy = instruction;
			instrCopy.MemoryBase = Register.RIP;
			instrCopy.MemoryDisplacement64 = 0;
			ripInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, instrCopy.IPRelativeMemoryAddress);

			instrCopy.MemoryBase = Register.EIP;
			eipInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, instrCopy.IPRelativeMemoryAddress);

			Debug.Assert(eipInstructionSize >= ripInstructionSize);
			Size = eipInstructionSize;
		}

		public override void Initialize(BlockEncoder blockEncoder) =>
			targetInstr = blockEncoder.GetTarget(instruction.IPRelativeMemoryAddress);

		public override bool Optimize(ulong gained) => TryOptimize(gained);

		bool TryOptimize(ulong gained) {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Rip || instrKind == InstrKind.Eip) {
				Done = true;
				return false;
			}

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useRip = targetInstr.IsInBlock(Block);
			var targetAddress = targetInstr.GetAddress();
			if (!useRip) {
				var nextRip = IP + ripInstructionSize;
				long diff = (long)(targetAddress - nextRip);
				diff = CorrectDiff(targetInstr.IsInBlock(Block), diff, gained);
				useRip = int.MinValue <= diff && diff <= int.MaxValue;
			}

			if (useRip) {
				Size = ripInstructionSize;
				instrKind = InstrKind.Rip;
				Done = true;
				return true;
			}

			// If it's in the low 4GB we can use EIP relative addressing
			if (targetAddress <= uint.MaxValue) {
				Size = eipInstructionSize;
				instrKind = InstrKind.Eip;
				Done = true;
				return true;
			}

			instrKind = InstrKind.Long;
			return false;
		}

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			switch (instrKind) {
			case InstrKind.Unchanged:
			case InstrKind.Rip:
			case InstrKind.Eip:
				isOriginalInstruction = true;

				if (instrKind == InstrKind.Rip)
					instruction.MemoryBase = Register.RIP;
				else if (instrKind == InstrKind.Eip)
					instruction.MemoryBase = Register.EIP;
				else
					Debug.Assert(instrKind == InstrKind.Unchanged);

				var targetAddress = targetInstr.GetAddress();
				instruction.MemoryDisplacement64 = targetAddress;
				encoder.TryEncode(instruction, IP, out _, out var errorMessage);
				bool b = instruction.IPRelativeMemoryAddress == (instruction.MemoryBase == Register.EIP ? (uint)targetAddress : targetAddress);
				Debug.Assert(b);
				if (!b)
					errorMessage = "Invalid IP relative address";
				if (errorMessage is not null) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;

			case InstrKind.Long:
				isOriginalInstruction = false;
				constantOffsets = default;
				return "IP relative memory operand is too far away and isn't currently supported. " +
					"Try to allocate memory close to the original instruction (+/-2GB).";

			case InstrKind.Uninitialized:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
