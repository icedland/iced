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
	/// Xbegin instruction
	/// </summary>
	sealed class XbeginInstr : Instr {
		readonly ulong targetAddr;
		Instruction instruction;
		TargetInstr targetInstr;

		public XbeginInstr(BlockEncoder blockEncoder, ref Instruction instruction)
			: base(blockEncoder, instruction.IP64) {
			this.instruction = instruction;

			switch (blockEncoder.Bitness) {
			case 16: targetAddr = instruction.NearBranch16Target; break;
			case 32: targetAddr = instruction.NearBranch32Target; break;
			case 64: targetAddr = instruction.NearBranch64Target; break;
			default: throw new InvalidOperationException();
			}

			if (blockEncoder.FixBranches) {
				if (blockEncoder.Bitness == 16)
					this.instruction.Code = Code.Xbegin_Jw16;
				else {
					Debug.Assert(blockEncoder.Bitness == 32 || blockEncoder.Bitness == 64);
					this.instruction.Code = Code.Xbegin_Jd32;
				}
			}
			Size = (uint)blockEncoder.NullEncoder.Encode(ref this.instruction, instruction.IP64, out var errorMessage);
			if (errorMessage != null)
				Size = DecoderConstants.MaxInstructionLength;
		}

		public override void Initialize() => targetInstr = blockEncoder.GetTarget(targetAddr);

		public override bool Optimize() => false;

		public override string TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction) {
			isOriginalInstruction = true;
			instruction.NearBranch64Target = targetInstr.GetAddress();
			encoder.Encode(ref instruction, IP, out var errorMessage);
			if (errorMessage != null) {
				constantOffsets = default;
				return CreateErrorMessage(errorMessage, ref instruction);
			}
			constantOffsets = encoder.GetConstantOffsets();
			return null;
		}
	}
}
#endif
