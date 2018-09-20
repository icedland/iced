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

namespace Iced.Intel.BlockEncoderInternal {
	abstract class Instr {
		protected readonly BlockEncoder blockEncoder;

		public Block Block;
		public uint Size;
		public ulong IP;
		public readonly ulong OrigIP;

		// 6 = FF 15 XXXXXXXX = call qword ptr [rip+mem_target]
		protected const uint CallOrJmpPointerDataInstructionSize64 = 6;

		protected Instr(BlockEncoder blockEncoder, ulong origIp) {
			this.blockEncoder = blockEncoder ?? throw new ArgumentNullException(nameof(blockEncoder));
			OrigIP = origIp;
		}

		/// <summary>
		/// Initializes the target address and tries to optimize the instruction
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// Returns true if the instruction was updated to a shorter instruction, false if nothing changed
		/// </summary>
		/// <returns></returns>
		public abstract bool Optimize();

		public abstract string TryEncode(Encoder encoder, out ConstantOffsets constantOffsets, out bool isOriginalInstruction);

		protected string CreateErrorMessage(string errorMessage, ref Instruction instruction) =>
			$"{errorMessage} : 0x{instruction.IP64:X} {instruction.ToString()}";

		public static Instr Create(BlockEncoder blockEncoder, ref Instruction instruction) {
			switch (instruction.Code) {
			case Code.Jo_rel8_16:
			case Code.Jo_rel8_32:
			case Code.Jo_rel8_64:
			case Code.Jno_rel8_16:
			case Code.Jno_rel8_32:
			case Code.Jno_rel8_64:
			case Code.Jb_rel8_16:
			case Code.Jb_rel8_32:
			case Code.Jb_rel8_64:
			case Code.Jae_rel8_16:
			case Code.Jae_rel8_32:
			case Code.Jae_rel8_64:
			case Code.Je_rel8_16:
			case Code.Je_rel8_32:
			case Code.Je_rel8_64:
			case Code.Jne_rel8_16:
			case Code.Jne_rel8_32:
			case Code.Jne_rel8_64:
			case Code.Jbe_rel8_16:
			case Code.Jbe_rel8_32:
			case Code.Jbe_rel8_64:
			case Code.Ja_rel8_16:
			case Code.Ja_rel8_32:
			case Code.Ja_rel8_64:

			case Code.Js_rel8_16:
			case Code.Js_rel8_32:
			case Code.Js_rel8_64:
			case Code.Jns_rel8_16:
			case Code.Jns_rel8_32:
			case Code.Jns_rel8_64:
			case Code.Jp_rel8_16:
			case Code.Jp_rel8_32:
			case Code.Jp_rel8_64:
			case Code.Jnp_rel8_16:
			case Code.Jnp_rel8_32:
			case Code.Jnp_rel8_64:
			case Code.Jl_rel8_16:
			case Code.Jl_rel8_32:
			case Code.Jl_rel8_64:
			case Code.Jge_rel8_16:
			case Code.Jge_rel8_32:
			case Code.Jge_rel8_64:
			case Code.Jle_rel8_16:
			case Code.Jle_rel8_32:
			case Code.Jle_rel8_64:
			case Code.Jg_rel8_16:
			case Code.Jg_rel8_32:
			case Code.Jg_rel8_64:

			case Code.Jo_rel16:
			case Code.Jo_rel32_32:
			case Code.Jo_rel32_64:
			case Code.Jno_rel16:
			case Code.Jno_rel32_32:
			case Code.Jno_rel32_64:
			case Code.Jb_rel16:
			case Code.Jb_rel32_32:
			case Code.Jb_rel32_64:
			case Code.Jae_rel16:
			case Code.Jae_rel32_32:
			case Code.Jae_rel32_64:
			case Code.Je_rel16:
			case Code.Je_rel32_32:
			case Code.Je_rel32_64:
			case Code.Jne_rel16:
			case Code.Jne_rel32_32:
			case Code.Jne_rel32_64:
			case Code.Jbe_rel16:
			case Code.Jbe_rel32_32:
			case Code.Jbe_rel32_64:
			case Code.Ja_rel16:
			case Code.Ja_rel32_32:
			case Code.Ja_rel32_64:

			case Code.Js_rel16:
			case Code.Js_rel32_32:
			case Code.Js_rel32_64:
			case Code.Jns_rel16:
			case Code.Jns_rel32_32:
			case Code.Jns_rel32_64:
			case Code.Jp_rel16:
			case Code.Jp_rel32_32:
			case Code.Jp_rel32_64:
			case Code.Jnp_rel16:
			case Code.Jnp_rel32_32:
			case Code.Jnp_rel32_64:
			case Code.Jl_rel16:
			case Code.Jl_rel32_32:
			case Code.Jl_rel32_64:
			case Code.Jge_rel16:
			case Code.Jge_rel32_32:
			case Code.Jge_rel32_64:
			case Code.Jle_rel16:
			case Code.Jle_rel32_32:
			case Code.Jle_rel32_64:
			case Code.Jg_rel16:
			case Code.Jg_rel32_32:
			case Code.Jg_rel32_64:
				return new JccInstr(blockEncoder, ref instruction);

			case Code.Loopne_rel8_16_CX:
			case Code.Loopne_rel8_32_CX:
			case Code.Loopne_rel8_16_ECX:
			case Code.Loopne_rel8_32_ECX:
			case Code.Loopne_rel8_64_ECX:
			case Code.Loopne_rel8_64_RCX:
			case Code.Loope_rel8_16_CX:
			case Code.Loope_rel8_32_CX:
			case Code.Loope_rel8_16_ECX:
			case Code.Loope_rel8_32_ECX:
			case Code.Loope_rel8_64_ECX:
			case Code.Loope_rel8_64_RCX:
			case Code.Loop_rel8_16_CX:
			case Code.Loop_rel8_32_CX:
			case Code.Loop_rel8_16_ECX:
			case Code.Loop_rel8_32_ECX:
			case Code.Loop_rel8_64_ECX:
			case Code.Loop_rel8_64_RCX:
			case Code.Jcxz_rel8_16:
			case Code.Jcxz_rel8_32:
			case Code.Jecxz_rel8_16:
			case Code.Jecxz_rel8_32:
			case Code.Jecxz_rel8_64:
			case Code.Jrcxz_rel8_64:
				return new SimpleBranchInstr(blockEncoder, ref instruction);

			case Code.Call_rel16:
			case Code.Call_rel32_32:
			case Code.Call_rel32_64:
				return new CallInstr(blockEncoder, ref instruction);

			case Code.Jmp_rel16:
			case Code.Jmp_rel32_32:
			case Code.Jmp_rel32_64:
			case Code.Jmp_rel8_16:
			case Code.Jmp_rel8_32:
			case Code.Jmp_rel8_64:
				return new JmpInstr(blockEncoder, ref instruction);

			case Code.Xbegin_rel16:
			case Code.Xbegin_rel32:
			case Code.Xbegin_rel32_REXW:
				return new XbeginInstr(blockEncoder, ref instruction);
			}

			if (blockEncoder.Bitness == 64) {
				int ops = instruction.OpCount;
				for (int i = 0; i < ops; i++) {
					if (instruction.GetOpKind(i) == OpKind.Memory) {
						if (instruction.IsIPRelativeMemoryOp)
							return new IpRelMemOpInstr(blockEncoder, ref instruction);
						break;
					}
				}
			}

			return new SimpleInstr(blockEncoder, ref instruction);
		}

		protected string EncodeBranchToPointerData(Encoder encoder, bool isCall, ulong ip, BlockData pointerData, out uint size, uint minSize) {
			if (minSize > int.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(minSize));

			var instr = new Instruction();
			instr.Op0Kind = OpKind.Memory;
			instr.MemoryDisplSize = encoder.Bitness / 8;
			RelocKind relocKind;
			switch (encoder.Bitness) {
			case 64:
				instr.Code = isCall ? Code.Call_rm64 : Code.Jmp_rm64;
				instr.MemoryBase = Register.RIP;
				var nextRip = ip + CallOrJmpPointerDataInstructionSize64;
				instr.NextIP64 = nextRip;
				long diff = (long)(pointerData.Address - nextRip);
				instr.MemoryDisplacement = (uint)diff;
				if (!(int.MinValue <= diff && diff <= int.MaxValue)) {
					size = 0;
					return "Block is too big";
				}
				relocKind = RelocKind.Offset64;
				break;

			default:
				throw new InvalidOperationException();
			}

			if (!encoder.TryEncode(ref instr, ip, out size, out var errorMessage))
				return errorMessage;
			if (Block.CanAddRelocInfos && relocKind != RelocKind.Offset64) {
				var constantOffsets = encoder.GetConstantOffsets();
				if (!constantOffsets.HasDisplacement)
					return "Internal error: no displ";
				Block.AddRelocInfo(new RelocInfo(relocKind, IP + constantOffsets.DisplacementOffset));
			}
			while (size < minSize) {
				size++;
				Block.CodeWriter.WriteByte(0x90);
			}
			return null;
		}
	}
}
#endif
