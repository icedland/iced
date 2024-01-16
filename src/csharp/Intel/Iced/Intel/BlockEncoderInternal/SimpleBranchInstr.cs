// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using System.Diagnostics;

namespace Iced.Intel.BlockEncoderInternal {
	/// <summary>
	/// Simple branch instruction that only has one code value, eg. loopcc, jrcxz
	/// </summary>
	sealed class SimpleBranchInstr : Instr {
		readonly byte bitness;
		Instruction instruction;
		TargetInstr targetInstr;
		BlockData? pointerData;
		InstrKind instrKind;
		readonly byte shortInstructionSize;
		readonly byte nearInstructionSize;
		readonly byte longInstructionSize;
		readonly byte nativeInstructionSize;
		readonly Code nativeCode;

		enum InstrKind : byte {
			Unchanged,
			Short,
			Near,
			Long,
			Uninitialized,
		}

		public SimpleBranchInstr(BlockEncoder blockEncoder, Block block, in Instruction instruction)
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
				instrCopy.NearBranch64 = 0;
				shortInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);

				nativeCode = ToNativeBranchCode(instruction.Code, blockEncoder.Bitness);
				if (nativeCode == instruction.Code)
					nativeInstructionSize = shortInstructionSize;
				else {
					instrCopy = instruction;
					instrCopy.InternalSetCodeNoCheck(nativeCode);
					instrCopy.NearBranch64 = 0;
					nativeInstructionSize = (byte)blockEncoder.GetInstructionSize(instrCopy, 0);
				}

				nearInstructionSize = (byte)(blockEncoder.Bitness switch {
					16 => nativeInstructionSize + 2 + 3,
					32 or 64 => nativeInstructionSize + 2 + 5,
					_ => throw new InvalidOperationException(),
				});

				if (blockEncoder.Bitness == 64) {
					longInstructionSize = (byte)(nativeInstructionSize + 2 + CallOrJmpPointerDataInstructionSize64);
					Size = Math.Max(Math.Max(shortInstructionSize, nearInstructionSize), longInstructionSize);
				}
				else
					Size = Math.Max(shortInstructionSize, nearInstructionSize);
			}
		}

		static Code ToNativeBranchCode(Code code, int bitness) {
			Code c16, c32, c64;
			switch (code) {
			case Code.Loopne_rel8_16_CX:
			case Code.Loopne_rel8_32_CX:
				c16 = Code.Loopne_rel8_16_CX;
				c32 = Code.Loopne_rel8_32_CX;
				c64 = Code.INVALID;
				break;

			case Code.Loopne_rel8_16_ECX:
			case Code.Loopne_rel8_32_ECX:
			case Code.Loopne_rel8_64_ECX:
				c16 = Code.Loopne_rel8_16_ECX;
				c32 = Code.Loopne_rel8_32_ECX;
				c64 = Code.Loopne_rel8_64_ECX;
				break;

			case Code.Loopne_rel8_16_RCX:
			case Code.Loopne_rel8_64_RCX:
				c16 = Code.Loopne_rel8_16_RCX;
				c32 = Code.INVALID;
				c64 = Code.Loopne_rel8_64_RCX;
				break;

			case Code.Loope_rel8_16_CX:
			case Code.Loope_rel8_32_CX:
				c16 = Code.Loope_rel8_16_CX;
				c32 = Code.Loope_rel8_32_CX;
				c64 = Code.INVALID;
				break;

			case Code.Loope_rel8_16_ECX:
			case Code.Loope_rel8_32_ECX:
			case Code.Loope_rel8_64_ECX:
				c16 = Code.Loope_rel8_16_ECX;
				c32 = Code.Loope_rel8_32_ECX;
				c64 = Code.Loope_rel8_64_ECX;
				break;

			case Code.Loope_rel8_16_RCX:
			case Code.Loope_rel8_64_RCX:
				c16 = Code.Loope_rel8_16_RCX;
				c32 = Code.INVALID;
				c64 = Code.Loope_rel8_64_RCX;
				break;

			case Code.Loop_rel8_16_CX:
			case Code.Loop_rel8_32_CX:
				c16 = Code.Loop_rel8_16_CX;
				c32 = Code.Loop_rel8_32_CX;
				c64 = Code.INVALID;
				break;

			case Code.Loop_rel8_16_ECX:
			case Code.Loop_rel8_32_ECX:
			case Code.Loop_rel8_64_ECX:
				c16 = Code.Loop_rel8_16_ECX;
				c32 = Code.Loop_rel8_32_ECX;
				c64 = Code.Loop_rel8_64_ECX;
				break;

			case Code.Loop_rel8_16_RCX:
			case Code.Loop_rel8_64_RCX:
				c16 = Code.Loop_rel8_16_RCX;
				c32 = Code.INVALID;
				c64 = Code.Loop_rel8_64_RCX;
				break;

			case Code.Jcxz_rel8_16:
			case Code.Jcxz_rel8_32:
				c16 = Code.Jcxz_rel8_16;
				c32 = Code.Jcxz_rel8_32;
				c64 = Code.INVALID;
				break;

			case Code.Jecxz_rel8_16:
			case Code.Jecxz_rel8_32:
			case Code.Jecxz_rel8_64:
				c16 = Code.Jecxz_rel8_16;
				c32 = Code.Jecxz_rel8_32;
				c64 = Code.Jecxz_rel8_64;
				break;

			case Code.Jrcxz_rel8_16:
			case Code.Jrcxz_rel8_64:
				c16 = Code.Jrcxz_rel8_16;
				c32 = Code.INVALID;
				c64 = Code.Jrcxz_rel8_64;
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
			Instruction instr;
			uint size;
			uint instrLen;
			switch (instrKind) {
			case InstrKind.Unchanged:
			case InstrKind.Short:
				isOriginalInstruction = true;
				instruction.NearBranch64 = targetInstr.GetAddress();
				if (!encoder.TryEncode(instruction, IP, out _, out errorMessage)) {
					constantOffsets = default;
					return CreateErrorMessage(errorMessage, instruction);
				}
				constantOffsets = encoder.GetConstantOffsets();
				return null;

			case InstrKind.Near:
				isOriginalInstruction = false;
				constantOffsets = default;

				// Code:
				//		brins tmp		; nativeInstructionSize
				//		jmp short skip	; 2
				//	tmp:
				//		jmp near target	; 3/5/5
				//	skip:

				instr = instruction;
				instr.InternalSetCodeNoCheck(nativeCode);
				instr.NearBranch64 = IP + nativeInstructionSize + 2;
				if (!encoder.TryEncode(instr, IP, out size, out errorMessage))
					return CreateErrorMessage(errorMessage, instruction);

				instr = new Instruction();
				instr.NearBranch64 = IP + nearInstructionSize;
				Code codeNear;
				switch (encoder.Bitness) {
				case 16:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_16);
					codeNear = Code.Jmp_rel16;
					instr.Op0Kind = OpKind.NearBranch16;
					break;

				case 32:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_32);
					codeNear = Code.Jmp_rel32_32;
					instr.Op0Kind = OpKind.NearBranch32;
					break;

				case 64:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_64);
					codeNear = Code.Jmp_rel32_64;
					instr.Op0Kind = OpKind.NearBranch64;
					break;

				default:
					throw new InvalidOperationException();
				}
				if (!encoder.TryEncode(instr, IP + size, out instrLen, out errorMessage))
					return CreateErrorMessage(errorMessage, instruction);
				size += instrLen;

				instr.InternalSetCodeNoCheck(codeNear);
				instr.NearBranch64 = targetInstr.GetAddress();
				encoder.TryEncode(instr, IP + size, out instrLen, out errorMessage);
				if (errorMessage is not null)
					return CreateErrorMessage(errorMessage, instruction);
				return null;

			case InstrKind.Long:
				Debug.Assert(encoder.Bitness == 64);
				Debug2.Assert(pointerData is not null);
				isOriginalInstruction = false;
				constantOffsets = default;
				pointerData.Data = targetInstr.GetAddress();

				// Code:
				//		brins tmp		; nativeInstructionSize
				//		jmp short skip	; 2
				//	tmp:
				//		jmp [mem_loc]	; 6
				//	skip:

				instr = instruction;
				instr.InternalSetCodeNoCheck(nativeCode);
				instr.NearBranch64 = IP + nativeInstructionSize + 2;
				if (!encoder.TryEncode(instr, IP, out instrLen, out errorMessage))
					return CreateErrorMessage(errorMessage, instruction);
				size = instrLen;

				instr = new Instruction();
				instr.NearBranch64 = IP + longInstructionSize;
				switch (encoder.Bitness) {
				case 16:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_16);
					instr.Op0Kind = OpKind.NearBranch16;
					break;

				case 32:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_32);
					instr.Op0Kind = OpKind.NearBranch32;
					break;

				case 64:
					instr.InternalSetCodeNoCheck(Code.Jmp_rel8_64);
					instr.Op0Kind = OpKind.NearBranch64;
					break;

				default:
					throw new InvalidOperationException();
				}
				if (!encoder.TryEncode(instr, IP + size, out instrLen, out errorMessage))
					return CreateErrorMessage(errorMessage, instruction);
				size += instrLen;

				errorMessage = EncodeBranchToPointerData(encoder, isCall: false, IP + size, pointerData, out _, Size - size);
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
