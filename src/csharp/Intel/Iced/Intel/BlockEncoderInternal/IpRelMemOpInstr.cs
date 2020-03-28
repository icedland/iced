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
	/// Instruction with a memory operand that is RIP/EIP relative
	/// </summary>
	sealed class IpRelMemOpInstr : Instr {
		Instruction instruction;
		InstrKind instrKind;
		readonly uint eipInstructionSize;
		readonly uint ripInstructionSize;
		TargetInstr targetInstr;

		enum InstrKind {
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
			ripInstructionSize = blockEncoder.GetInstructionSize(instrCopy, instrCopy.IP);

			instrCopy.MemoryBase = Register.EIP;
			eipInstructionSize = blockEncoder.GetInstructionSize(instrCopy, instrCopy.IP);

			Debug.Assert(eipInstructionSize >= ripInstructionSize);
			Size = eipInstructionSize;
		}

		public override void Initialize(BlockEncoder blockEncoder) {
			targetInstr = blockEncoder.GetTarget(instruction.IPRelativeMemoryAddress);
			TryOptimize();
		}

		public override bool Optimize() => TryOptimize();

		bool TryOptimize() {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Rip || instrKind == InstrKind.Eip)
				return false;

			// If it's in the same block, we assume the target is at most 2GB away.
			bool useRip = targetInstr.IsInBlock(Block);
			var targetAddress = targetInstr.GetAddress();
			if (!useRip) {
				var nextRip = IP + ripInstructionSize;
				long diff = (long)(targetAddress - nextRip);
				useRip = int.MinValue <= diff && diff <= int.MaxValue;
			}

			if (useRip) {
				Size = ripInstructionSize;
				instrKind = InstrKind.Rip;
				return true;
			}

			// If it's in the lower 4GB we can use EIP relative addressing
			if (targetAddress <= uint.MaxValue) {
				Size = eipInstructionSize;
				instrKind = InstrKind.Eip;
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

				uint instrSize;
				if (instrKind == InstrKind.Rip) {
					instrSize = ripInstructionSize;
					instruction.MemoryBase = Register.RIP;
				}
				else if (instrKind == InstrKind.Eip) {
					instrSize = eipInstructionSize;
					instruction.MemoryBase = Register.EIP;
				}
				else {
					Debug.Assert(instrKind == InstrKind.Unchanged);
					instrSize = instruction.MemoryBase == Register.EIP ? eipInstructionSize : ripInstructionSize;
				}

				var targetAddress = targetInstr.GetAddress();
				var nextRip = IP + instrSize;
				instruction.NextIP = nextRip;
				instruction.MemoryDisplacement = (uint)targetAddress - (uint)nextRip;
				encoder.TryEncode(instruction, IP, out _, out var errorMessage);
				bool b = instruction.IPRelativeMemoryAddress == (instruction.MemoryBase == Register.EIP ? (uint)targetAddress : targetAddress);
				Debug.Assert(b);
				if (!b)
					errorMessage = "Invalid IP relative address";
				if (!(errorMessage is null)) {
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
