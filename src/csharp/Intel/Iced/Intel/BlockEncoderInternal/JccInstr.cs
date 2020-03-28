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
	/// Jcc instruction
	/// </summary>
	sealed class JccInstr : Instr {
		readonly int bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		BlockData? pointerData;
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

		public JccInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
			: base(block, instruction.IP) {
			bitness = blockEncoder.Bitness;
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
				shortInstructionSize = blockEncoder.GetInstructionSize(instrCopy, 0);

				instrCopy = instruction;
				instrCopy.InternalSetCodeNoCheck(instruction.Code.ToNearBranch());
				instrCopy.NearBranch64 = 0;
				nearInstructionSize = blockEncoder.GetInstructionSize(instrCopy, 0);

				if (blockEncoder.Bitness == 64) {
					// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
					Size = Math.Max(nearInstructionSize, longInstructionSize64);
				}
				else
					Size = nearInstructionSize;
			}
		}

		public override void Initialize(BlockEncoder blockEncoder) {
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
				if (!(pointerData is null))
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
				if (!(pointerData is null))
					pointerData.IsValid = false;
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
				Debug2.Assert(!(pointerData is null));
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();
				var instr = new Instruction();
				instr.InternalSetCodeNoCheck(ShortJccToNativeJcc(instruction.Code.NegateConditionCode().ToShortBranch(), encoder.Bitness));
				instr.Op0Kind = OpKind.NearBranch64;
				Debug.Assert(encoder.Bitness == 64);
				Debug.Assert(longInstructionSize64 <= sbyte.MaxValue);
				instr.NearBranch64 = IP + longInstructionSize64;
				if (!encoder.TryEncode(instr, IP, out uint instrLen, out errorMessage))
					return CreateErrorMessage(errorMessage, instruction);
				errorMessage = EncodeBranchToPointerData(encoder, isCall: false, IP + (uint)instrLen, pointerData, out _, Size - (uint)instrLen);
				if (!(errorMessage is null))
					return CreateErrorMessage(errorMessage, instruction);
				return null;

			case InstrKind.Uninitialized:
			default:
				throw new InvalidOperationException();
			}
		}

		static Code ShortJccToNativeJcc(Code code, int bitness) {
			Code c16, c32, c64;
			switch (code) {
			case Code.Jo_rel8_16:
			case Code.Jo_rel8_32:
			case Code.Jo_rel8_64:
				c16 = Code.Jo_rel8_16;
				c32 = Code.Jo_rel8_32;
				c64 = Code.Jo_rel8_64;
				break;

			case Code.Jno_rel8_16:
			case Code.Jno_rel8_32:
			case Code.Jno_rel8_64:
				c16 = Code.Jno_rel8_16;
				c32 = Code.Jno_rel8_32;
				c64 = Code.Jno_rel8_64;
				break;

			case Code.Jb_rel8_16:
			case Code.Jb_rel8_32:
			case Code.Jb_rel8_64:
				c16 = Code.Jb_rel8_16;
				c32 = Code.Jb_rel8_32;
				c64 = Code.Jb_rel8_64;
				break;

			case Code.Jae_rel8_16:
			case Code.Jae_rel8_32:
			case Code.Jae_rel8_64:
				c16 = Code.Jae_rel8_16;
				c32 = Code.Jae_rel8_32;
				c64 = Code.Jae_rel8_64;
				break;

			case Code.Je_rel8_16:
			case Code.Je_rel8_32:
			case Code.Je_rel8_64:
				c16 = Code.Je_rel8_16;
				c32 = Code.Je_rel8_32;
				c64 = Code.Je_rel8_64;
				break;

			case Code.Jne_rel8_16:
			case Code.Jne_rel8_32:
			case Code.Jne_rel8_64:
				c16 = Code.Jne_rel8_16;
				c32 = Code.Jne_rel8_32;
				c64 = Code.Jne_rel8_64;
				break;

			case Code.Jbe_rel8_16:
			case Code.Jbe_rel8_32:
			case Code.Jbe_rel8_64:
				c16 = Code.Jbe_rel8_16;
				c32 = Code.Jbe_rel8_32;
				c64 = Code.Jbe_rel8_64;
				break;

			case Code.Ja_rel8_16:
			case Code.Ja_rel8_32:
			case Code.Ja_rel8_64:
				c16 = Code.Ja_rel8_16;
				c32 = Code.Ja_rel8_32;
				c64 = Code.Ja_rel8_64;
				break;

			case Code.Js_rel8_16:
			case Code.Js_rel8_32:
			case Code.Js_rel8_64:
				c16 = Code.Js_rel8_16;
				c32 = Code.Js_rel8_32;
				c64 = Code.Js_rel8_64;
				break;

			case Code.Jns_rel8_16:
			case Code.Jns_rel8_32:
			case Code.Jns_rel8_64:
				c16 = Code.Jns_rel8_16;
				c32 = Code.Jns_rel8_32;
				c64 = Code.Jns_rel8_64;
				break;

			case Code.Jp_rel8_16:
			case Code.Jp_rel8_32:
			case Code.Jp_rel8_64:
				c16 = Code.Jp_rel8_16;
				c32 = Code.Jp_rel8_32;
				c64 = Code.Jp_rel8_64;
				break;

			case Code.Jnp_rel8_16:
			case Code.Jnp_rel8_32:
			case Code.Jnp_rel8_64:
				c16 = Code.Jnp_rel8_16;
				c32 = Code.Jnp_rel8_32;
				c64 = Code.Jnp_rel8_64;
				break;

			case Code.Jl_rel8_16:
			case Code.Jl_rel8_32:
			case Code.Jl_rel8_64:
				c16 = Code.Jl_rel8_16;
				c32 = Code.Jl_rel8_32;
				c64 = Code.Jl_rel8_64;
				break;

			case Code.Jge_rel8_16:
			case Code.Jge_rel8_32:
			case Code.Jge_rel8_64:
				c16 = Code.Jge_rel8_16;
				c32 = Code.Jge_rel8_32;
				c64 = Code.Jge_rel8_64;
				break;

			case Code.Jle_rel8_16:
			case Code.Jle_rel8_32:
			case Code.Jle_rel8_64:
				c16 = Code.Jle_rel8_16;
				c32 = Code.Jle_rel8_32;
				c64 = Code.Jle_rel8_64;
				break;

			case Code.Jg_rel8_16:
			case Code.Jg_rel8_32:
			case Code.Jg_rel8_64:
				c16 = Code.Jg_rel8_16;
				c32 = Code.Jg_rel8_32;
				c64 = Code.Jg_rel8_64;
				break;


			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}

			return bitness switch {
				16 => c16,
				32 => c32,
				64 => c64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};
		}
	}
}
#endif
