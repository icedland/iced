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
			case Code.Jo_Jb16:
			case Code.Jo_Jb32:
			case Code.Jo_Jb64:
			case Code.Jno_Jb16:
			case Code.Jno_Jb32:
			case Code.Jno_Jb64:
			case Code.Jb_Jb16:
			case Code.Jb_Jb32:
			case Code.Jb_Jb64:
			case Code.Jae_Jb16:
			case Code.Jae_Jb32:
			case Code.Jae_Jb64:
			case Code.Je_Jb16:
			case Code.Je_Jb32:
			case Code.Je_Jb64:
			case Code.Jne_Jb16:
			case Code.Jne_Jb32:
			case Code.Jne_Jb64:
			case Code.Jbe_Jb16:
			case Code.Jbe_Jb32:
			case Code.Jbe_Jb64:
			case Code.Ja_Jb16:
			case Code.Ja_Jb32:
			case Code.Ja_Jb64:

			case Code.Js_Jb16:
			case Code.Js_Jb32:
			case Code.Js_Jb64:
			case Code.Jns_Jb16:
			case Code.Jns_Jb32:
			case Code.Jns_Jb64:
			case Code.Jp_Jb16:
			case Code.Jp_Jb32:
			case Code.Jp_Jb64:
			case Code.Jnp_Jb16:
			case Code.Jnp_Jb32:
			case Code.Jnp_Jb64:
			case Code.Jl_Jb16:
			case Code.Jl_Jb32:
			case Code.Jl_Jb64:
			case Code.Jge_Jb16:
			case Code.Jge_Jb32:
			case Code.Jge_Jb64:
			case Code.Jle_Jb16:
			case Code.Jle_Jb32:
			case Code.Jle_Jb64:
			case Code.Jg_Jb16:
			case Code.Jg_Jb32:
			case Code.Jg_Jb64:

			case Code.Jo_Jw16:
			case Code.Jo_Jd32:
			case Code.Jo_Jd64:
			case Code.Jno_Jw16:
			case Code.Jno_Jd32:
			case Code.Jno_Jd64:
			case Code.Jb_Jw16:
			case Code.Jb_Jd32:
			case Code.Jb_Jd64:
			case Code.Jae_Jw16:
			case Code.Jae_Jd32:
			case Code.Jae_Jd64:
			case Code.Je_Jw16:
			case Code.Je_Jd32:
			case Code.Je_Jd64:
			case Code.Jne_Jw16:
			case Code.Jne_Jd32:
			case Code.Jne_Jd64:
			case Code.Jbe_Jw16:
			case Code.Jbe_Jd32:
			case Code.Jbe_Jd64:
			case Code.Ja_Jw16:
			case Code.Ja_Jd32:
			case Code.Ja_Jd64:

			case Code.Js_Jw16:
			case Code.Js_Jd32:
			case Code.Js_Jd64:
			case Code.Jns_Jw16:
			case Code.Jns_Jd32:
			case Code.Jns_Jd64:
			case Code.Jp_Jw16:
			case Code.Jp_Jd32:
			case Code.Jp_Jd64:
			case Code.Jnp_Jw16:
			case Code.Jnp_Jd32:
			case Code.Jnp_Jd64:
			case Code.Jl_Jw16:
			case Code.Jl_Jd32:
			case Code.Jl_Jd64:
			case Code.Jge_Jw16:
			case Code.Jge_Jd32:
			case Code.Jge_Jd64:
			case Code.Jle_Jw16:
			case Code.Jle_Jd32:
			case Code.Jle_Jd64:
			case Code.Jg_Jw16:
			case Code.Jg_Jd32:
			case Code.Jg_Jd64:
				return new JccInstr(blockEncoder, ref instruction);

			case Code.Loopne_Jb16_CX:
			case Code.Loopne_Jb32_CX:
			case Code.Loopne_Jb16_ECX:
			case Code.Loopne_Jb32_ECX:
			case Code.Loopne_Jb64_ECX:
			case Code.Loopne_Jb64_RCX:
			case Code.Loope_Jb16_CX:
			case Code.Loope_Jb32_CX:
			case Code.Loope_Jb16_ECX:
			case Code.Loope_Jb32_ECX:
			case Code.Loope_Jb64_ECX:
			case Code.Loope_Jb64_RCX:
			case Code.Loop_Jb16_CX:
			case Code.Loop_Jb32_CX:
			case Code.Loop_Jb16_ECX:
			case Code.Loop_Jb32_ECX:
			case Code.Loop_Jb64_ECX:
			case Code.Loop_Jb64_RCX:
			case Code.Jcxz_Jb16:
			case Code.Jcxz_Jb32:
			case Code.Jecxz_Jb16:
			case Code.Jecxz_Jb32:
			case Code.Jecxz_Jb64:
			case Code.Jrcxz_Jb64:
				return new SimpleBranchInstr(blockEncoder, ref instruction);

			case Code.Call_Jw16:
			case Code.Call_Jd32:
			case Code.Call_Jd64:
				return new CallInstr(blockEncoder, ref instruction);

			case Code.Jmp_Jw16:
			case Code.Jmp_Jd32:
			case Code.Jmp_Jd64:
			case Code.Jmp_Jb16:
			case Code.Jmp_Jb32:
			case Code.Jmp_Jb64:
				return new JmpInstr(blockEncoder, ref instruction);

			case Code.Xbegin_Jw16:
			case Code.Xbegin_Jd32:
			case Code.Xbegin_Jd64:
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
			instr.OpCount = 1;
			instr.Op0Kind = OpKind.Memory;
			instr.MemoryDisplSize = encoder.Bitness / 8;
			RelocKind relocKind;
			switch (encoder.Bitness) {
			case 64:
				instr.Code = isCall ? Code.Call_Eq : Code.Jmp_Eq;
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

			size = (uint)encoder.Encode(ref instr, ip, out var errorMessage);
			if (errorMessage != null)
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
