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
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Xbegin instruction
	/// </summary>
	sealed class XbeginInstr : Instr {
		readonly ulong targetAddr;
		Instruction instruction;
		TargetInstr targetInstr;

		public XbeginInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			this.instruction = instruction;

			targetAddr = instruction.NearBranchTarget;
			if (blockEncoder.FixBranches) {
				if (blockEncoder.Bitness == 16)
					this.instruction.InternalSetCodeNoCheck(Code.Xbegin_rel16);
				else {
					Debug.Assert(blockEncoder.Bitness == 32 || blockEncoder.Bitness == 64);
					this.instruction.InternalSetCodeNoCheck(Code.Xbegin_rel32);
				}
			}
			var instrCopy = this.instruction;
			instrCopy.NearBranch64 = 0;
			Size = blockEncoder.GetInstructionSize(instrCopy, 0);
		}

		public override void Initialize(BlockEncoder blockEncoder) => targetInstr = blockEncoder.GetTarget(targetAddr);

		public override bool Optimize() => false;

		public override string? TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			isOriginalInstruction = true;
			instruction.NearBranch64 = targetInstr.GetAddress();
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
