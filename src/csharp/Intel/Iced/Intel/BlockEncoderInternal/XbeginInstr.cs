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
	/// Xbegin instruction
	/// </summary>
	sealed class XbeginInstr : Instr {
		Instruction instruction;
		TargetInstr targetInstr;
		InstrKind instrKind;
		readonly uint shortInstructionSize;
		readonly uint nearInstructionSize;

		enum InstrKind {
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
				shortInstructionSize = blockEncoder.GetInstructionSize(instrCopy, 0);

				instrCopy = instruction;
				instrCopy.InternalSetCodeNoCheck(Code.Xbegin_rel32);
				instrCopy.NearBranch64 = 0;
				nearInstructionSize = blockEncoder.GetInstructionSize(instrCopy, 0);

				Size = nearInstructionSize;
			}
		}

		public override void Initialize(BlockEncoder blockEncoder) {
			targetInstr = blockEncoder.GetTarget(instruction.NearBranchTarget);
			TryOptimize();
		}

		public override bool Optimize() => TryOptimize();

		bool TryOptimize() {
			if (instrKind == InstrKind.Unchanged || instrKind == InstrKind.Rel16)
				return false;

			var targetAddress = targetInstr.GetAddress();
			var nextRip = IP + shortInstructionSize;
			long diff = (long)(targetAddress - nextRip);
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
