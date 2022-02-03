// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Iced.Intel;

namespace IcedFuzzer.Core {
	public static class OpCodeTableIndexes {
		public const int D3nowTable = 1;

		public const int LegacyTable_Normal = 0;
		public const int LegacyTable_0F = 1;
		public const int LegacyTable_0F38 = 2;
		public const int LegacyTable_0F3A = 3;
		// Not used, so any index can be used
		public const int LegacyTable_0F39 = 4;
		public const int LegacyTable_0F3B = 5;
		public const int LegacyTable_0F3C = 6;
		public const int LegacyTable_0F3D = 7;
		public const int LegacyTable_0F3E = 8;
		public const int LegacyTable_0F3F = 9;
		public const int LegacyTable_MaxUsed = LegacyTable_0F3A;
		public const int LegacyTable_Max = LegacyTable_0F3F;
	}

	[DebuggerDisplay("{" + nameof(Encoding) + "} {" + nameof(TableIndex) + ",d}")]
	public readonly struct FuzzerOpCodeTable : IEquatable<FuzzerOpCodeTable> {
		public readonly EncodingKind Encoding;
		public readonly int TableIndex;
		public FuzzerOpCodeTable(EncodingKind encoding, int tableIndex) {
			Encoding = encoding;
			TableIndex = tableIndex;
		}

		public static bool operator ==(FuzzerOpCodeTable left, FuzzerOpCodeTable right) => left.Equals(right);
		public static bool operator !=(FuzzerOpCodeTable left, FuzzerOpCodeTable right) => !left.Equals(right);
		public override bool Equals(object? obj) => obj is FuzzerOpCodeTable table && Equals(table);
		public bool Equals(FuzzerOpCodeTable other) => Encoding == other.Encoding && TableIndex == other.TableIndex;
		public override int GetHashCode() => (int)Encoding ^ (TableIndex << 8);
	}

	public readonly struct OpCode : IEquatable<OpCode> {
		public readonly byte Byte0;
		public readonly byte Byte1;
		public readonly bool IsTwobyte;
		public readonly bool IsOneByte => !IsTwobyte;

		public OpCode(byte byte0) {
			Byte0 = byte0;
			Byte1 = 0;
			IsTwobyte = false;
		}

		public OpCode(byte byte0, byte byte1) {
			// The 2nd byte is a modrm byte and must be the reg form (mod==11b)
			Assert.True(byte1 >= 0xC0);
			Byte0 = byte0;
			Byte1 = byte1;
			IsTwobyte = true;
		}

		public static OpCode CreateFromUInt32(uint opCode, int length) =>
			length switch {
				1 => new OpCode((byte)opCode),
				2 => new OpCode((byte)(opCode >> 8), (byte)opCode),
				_ => throw ThrowHelpers.Unreachable,
			};

		public override string ToString() {
			if (IsTwobyte)
				return $"{Byte0:X2}{Byte1:X2}";
			return Byte0.ToString("X2");
		}

		public static bool operator ==(OpCode left, OpCode right) => left.Equals(right);
		public static bool operator !=(OpCode left, OpCode right) => !left.Equals(right);
		public override bool Equals(object? obj) => obj is OpCode code && Equals(code);
		public bool Equals(OpCode other) => Byte0 == other.Byte0 && Byte1 == other.Byte1 && IsTwobyte == other.IsTwobyte;
		public override int GetHashCode() => (Byte0 << 24) | (Byte1 << 16);
	}

	[Flags]
	enum FuzzerInstructionFlags : uint {
		None							= 0,
		// Used by legacy instructions without a mandatory prefix, eg. 'add reg,reg/mem'
		DontUsePrefix66					= 0x00000001,
		DontUsePrefixF3					= 0x00000002,
		DontUsePrefixF2					= 0x00000004,
		DontUsePrefix67					= 0x00000008,
		DontUsePrefixREXW				= 0x00000010,
		// This is set if there's an L=2 instruction that uses {er} or {sae}.
		// Only that instruction can use the EVEX.b bit to test {er} and {sae}.
		// If this is clear, the EVEX.b bit can be used to test valid/invalid encodings
		// (i.e., it's the L=2 instruction or there's no L=2 instruction that uses {er}/{sae})
		DontUseEvexBcstBit				= 0x00000020,
		CanBroadcast					= 0x00000040,
		CanUseRoundingControl			= 0x00000080,
		CanSuppressAllExceptions		= 0x00000100,
		CanUseLockPrefix				= 0x00000200,
		RequireOpMaskRegister			= 0x00000400,
		IsXchgRegAcc					= 0x00000800,
		IsNop							= 0x00001000,
		IsVsib							= 0x00002000,
		CanUseZeroingMasking			= 0x00004000,
		CanUseOpMaskRegister			= 0x00008000,
		NFx								= 0x00010000,
		No66							= 0x00020000,
		AmdLockRegBit					= 0x00040000,
		RequiresUniqueRegNums			= 0x00080000,
		IgnoresModBits					= 0x00100000,
		ReservedNop						= 0x00200000,
		DefaultOperandSize64			= 0x00400000,
		RequiresUniqueDestRegNum		= 0x00800000,
		RequiresAddressSize32			= 0x01000000,
	}

	[DebuggerDisplay("Mem={" + nameof(IsModrmMemory) + "} {" + nameof(MandatoryPrefix) + "} L{" + nameof(L) + ",d} W{" + nameof(W) + ",d} {" + nameof(Code) + "}")]
	public sealed class FuzzerInstruction {
		public bool IsValid => Code != Code.INVALID;
		public bool DontUsePrefix66 => (Flags & FuzzerInstructionFlags.DontUsePrefix66) != 0;
		public bool DontUsePrefixF3 => (Flags & FuzzerInstructionFlags.DontUsePrefixF3) != 0;
		public bool DontUsePrefixF2 => (Flags & FuzzerInstructionFlags.DontUsePrefixF2) != 0;
		public bool DontUsePrefix67 => (Flags & FuzzerInstructionFlags.DontUsePrefix67) != 0;
		public bool DontUsePrefixREXW => (Flags & FuzzerInstructionFlags.DontUsePrefixREXW) != 0;
		public bool DontUseEvexBcstBit => (Flags & FuzzerInstructionFlags.DontUseEvexBcstBit) != 0;
		public bool CanBroadcast => (Flags & FuzzerInstructionFlags.CanBroadcast) != 0;
		public bool CanUseRoundingControl => (Flags & FuzzerInstructionFlags.CanUseRoundingControl) != 0;
		public bool CanSuppressAllExceptions => (Flags & FuzzerInstructionFlags.CanSuppressAllExceptions) != 0;
		public bool CanUseLockPrefix => (Flags & FuzzerInstructionFlags.CanUseLockPrefix) != 0;
		public bool RequireOpMaskRegister => (Flags & FuzzerInstructionFlags.RequireOpMaskRegister) != 0;
		public bool IsXchgRegAcc => (Flags & FuzzerInstructionFlags.IsXchgRegAcc) != 0;
		public bool IsNop => (Flags & FuzzerInstructionFlags.IsNop) != 0;
		// This is only true if it has a memory operand with VSIB addressing
		public bool IsVsib => (Flags & FuzzerInstructionFlags.IsVsib) != 0;
		public bool CanUseZeroingMasking => (Flags & FuzzerInstructionFlags.CanUseZeroingMasking) != 0;
		public bool CanUseOpMaskRegister => (Flags & FuzzerInstructionFlags.CanUseOpMaskRegister) != 0;
		public bool NFx => (Flags & FuzzerInstructionFlags.NFx) != 0;
		public bool No66 => (Flags & FuzzerInstructionFlags.No66) != 0;
		public bool AmdLockRegBit => (Flags & FuzzerInstructionFlags.AmdLockRegBit) != 0;
		public bool RequiresUniqueRegNums => (Flags & FuzzerInstructionFlags.RequiresUniqueRegNums) != 0;
		public bool IgnoresModBits => (Flags & FuzzerInstructionFlags.IgnoresModBits) != 0;
		public bool IsReservedNop => (Flags & FuzzerInstructionFlags.ReservedNop) != 0;
		public bool DefaultOperandSize64 => (Flags & FuzzerInstructionFlags.DefaultOperandSize64) != 0;
		public bool RequiresUniqueDestRegNum => (Flags & FuzzerInstructionFlags.RequiresUniqueDestRegNum) != 0;
		public bool RequiresAddressSize32 => (Flags & FuzzerInstructionFlags.RequiresAddressSize32) != 0;

		public readonly Code Code;
		internal FuzzerInstructionFlags Flags;
		// VEX/XOP/EVEX/MVEX: 0-1
		public readonly uint W;
		// VEX/XOP/EVEX/MVEX: 0 (MVEX) 0-1 (VEX/XOP) or 0-3 (EVEX)
		public readonly uint L;
		public readonly MandatoryPrefix MandatoryPrefix;
		public readonly FuzzerOpCodeTable Table;
		public readonly OpCode OpCode;
		public readonly int GroupIndex;
		public readonly int RmGroupIndex;
		// false if it's not a modrm instruction or if it has no modrm mem op. Also false if it has a non-modrm memory op (eg. `mov al,[moffs]`).
		// true if it's a modrm instruction with a memory operand.
		// Each `reg,reg/mem` instruction is split into two: `reg,reg` and `reg,mem`.
		public readonly bool IsModrmMemory;
		// Legacy: 0, 16, 32, 64
		public readonly int OperandSize;
		// Legacy: 0, 16, 32, 64
		public readonly int AddressSize;
		internal readonly FuzzerOperand[] Operands;
		internal readonly ImmediateFuzzerOperand[] ImmediateOperands;
		internal readonly FuzzerOperand[] MemOffsOperands;
		internal readonly FuzzerOperand[] ImpliedMemOperands;
		internal readonly ModrmMemoryFuzzerOperand[] ModrmMemoryOperands;
		internal readonly RegisterFuzzerOperand[] RegisterOperands;

		FuzzerInstruction(Code code, FuzzerInstructionFlags flags, uint w, uint l, MandatoryPrefix mandatoryPrefix, FuzzerOpCodeTable table, OpCode opCode, int groupIndex, int rmGroupIndex, bool isModrmMemory, int operandSize, int addressSize) {
			// Should be a 2-byte opcode instead (groupIndex = modrm.reg and rmGroupIndex = modrm.rm bits)
			Assert.True(groupIndex < 0 || rmGroupIndex < 0);
			Assert.True(groupIndex >= -1 && groupIndex <= 7);
			Assert.True(rmGroupIndex >= -1 && rmGroupIndex <= 7);
			var opc = code.ToOpCode();
			Assert.True(opc.IsInstruction || code == Code.INVALID);

			if (isModrmMemory) {
				if (opc.CanBroadcast)
					flags |= FuzzerInstructionFlags.CanBroadcast;
				if (opc.CanUseLockPrefix)
					flags |= FuzzerInstructionFlags.CanUseLockPrefix;
			}
			else {
				if (opc.CanUseRoundingControl)
					flags |= FuzzerInstructionFlags.CanUseRoundingControl;
				if (opc.CanSuppressAllExceptions)
					flags |= FuzzerInstructionFlags.CanSuppressAllExceptions;
			}
			if (opc.RequireOpMaskRegister)
				flags |= FuzzerInstructionFlags.RequireOpMaskRegister;
			if (opc.CanUseZeroingMasking)
				flags |= FuzzerInstructionFlags.CanUseZeroingMasking;
			if (opc.CanUseOpMaskRegister)
				flags |= FuzzerInstructionFlags.CanUseOpMaskRegister;
			if (opc.NFx) {
				flags |= FuzzerInstructionFlags.NFx;
				Assert.True(mandatoryPrefix == MandatoryPrefix.None);
				mandatoryPrefix = MandatoryPrefix.PNP;
			}
			if (opc.No66)
				flags |= FuzzerInstructionFlags.No66;
			if (opc.AmdLockRegBit)
				flags |= FuzzerInstructionFlags.AmdLockRegBit;
			if (opc.RequiresUniqueRegNums)
				flags |= FuzzerInstructionFlags.RequiresUniqueRegNums;
			if (opc.IgnoresModBits)
				flags |= FuzzerInstructionFlags.IgnoresModBits;
			if (opc.IsReservedNop)
				flags |= FuzzerInstructionFlags.ReservedNop;
			if (opc.DefaultOpSize64)
				flags |= FuzzerInstructionFlags.DefaultOperandSize64;
			if (opc.RequiresUniqueDestRegNum)
				flags |= FuzzerInstructionFlags.RequiresUniqueDestRegNum;
			switch (code) {
			case Code.Xchg_r16_AX:
			case Code.Xchg_r32_EAX:
			case Code.Xchg_r64_RAX:
				flags |= FuzzerInstructionFlags.IsXchgRegAcc;
				break;
			case Code.Nopw:
			case Code.Nopd:
			case Code.Nopq:
				flags |= FuzzerInstructionFlags.IsNop;
				break;
			case Code.Montmul_16:
			case Code.Montmul_32:
			case Code.Montmul_64:
				flags |= FuzzerInstructionFlags.RequiresAddressSize32;
				break;
			}

			Code = code;
			Flags = flags;
			W = w;
			L = l;
			MandatoryPrefix = mandatoryPrefix;
			Table = table;
			OpCode = opCode;
			GroupIndex = groupIndex;
			RmGroupIndex = rmGroupIndex;
			IsModrmMemory = isModrmMemory;
			OperandSize = operandSize;
			AddressSize = addressSize;

			if (MandatoryPrefix == MandatoryPrefix.P66 && operandSize != 64 && opc.IsReservedNop)
				MandatoryPrefix = MandatoryPrefix.None;

			FuzzerOperand[]? operands = null;
			// Special support for reserved nop instructions since we may have transformed them to
			// a 2-byte opcode (no ops) or a reg or rm group (one operand).
			if (opc.IsReservedNop) {
				// Verify our assumptions
				Assert.True(opc.OpCount == 2);
				const int RM_INDEX = 0;
				const int REG_INDEX = 1;
				switch (opc.GetOpKind(RM_INDEX)) {
				case OpCodeOperandKind.r16_or_mem:
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r64_or_mem:
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
				switch (opc.GetOpKind(REG_INDEX)) {
				case OpCodeOperandKind.r16_reg:
				case OpCodeOperandKind.r32_reg:
				case OpCodeOperandKind.r64_reg:
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}

				if (OpCode.IsTwobyte) {
					Assert.True(groupIndex < 0 && rmGroupIndex < 0);
					operands = Array.Empty<FuzzerOperand>();
				}
				else if (groupIndex >= 0) {
					Assert.True(rmGroupIndex < 0);
					// reg bits are hard coded, rm bits can be used
					operands = new FuzzerOperand[1] {
						FuzzerOperands.GetOperand(opc.GetOpKind(RM_INDEX), isModrmMemory)
					};
				}
				else if (rmGroupIndex >= 0) {
					// rm bits are hard coded, reg bits can be used
					operands = new FuzzerOperand[1] {
						FuzzerOperands.GetOperand(opc.GetOpKind(REG_INDEX), isModrmMemory)
					};
				}
			}
			if (operands is null) {
				int opCount = opc.OpCount + (opc.CanUseOpMaskRegister ? 1 : 0);
				operands = opCount == 0 ? Array.Empty<FuzzerOperand>() : new FuzzerOperand[opCount];
				int i;
				for (i = 0; i < opc.OpCount; i++)
					operands[i] = FuzzerOperands.GetOperand(opc.GetOpKind(i), isModrmMemory);
				if (opc.CanUseOpMaskRegister)
					operands[i++] = FuzzerOperands.OpMaskRegister;
				Assert.True(i == operands.Length);
			}
			operands = operands.Where(a => a.Kind != FuzzerOperandKind.None).ToArray();
			Operands = operands;
			Assert.True(!opc.CanUseOpMaskRegister || (operands.Length > 0 && operands[^1] == FuzzerOperands.OpMaskRegister));
			ImmediateOperands = operands.OfType<ImmediateFuzzerOperand>().ToArray();
			MemOffsOperands = operands.Where(a => a.Kind == FuzzerOperandKind.MemOffs).ToArray();
			ImpliedMemOperands = operands.Where(a => a.Kind == FuzzerOperandKind.ImpliedMem).ToArray();
			ModrmMemoryOperands = operands.OfType<ModrmMemoryFuzzerOperand>().ToArray();
			RegisterOperands = operands.OfType<RegisterFuzzerOperand>().ToArray();
			// There are fuzzers that test only some of these op kinds, so if there's a new op kind, a new fuzzer would need
			// to be added, see eg AllMemOffsFuzzerGen
			Assert.True(Operands.Length == ImmediateOperands.Length + MemOffsOperands.Length + ImpliedMemOperands.Length + ModrmMemoryOperands.Length + RegisterOperands.Length);

			foreach (var memOp in ModrmMemoryOperands) {
				if (memOp.IsVSIB) {
					Flags |= FuzzerInstructionFlags.IsVsib;
					break;
				}
			}
		}

		internal static FuzzerInstruction CreateInvalidLegacy(FuzzerOpCodeTable table, OpCode opCode, int groupIndex, bool isModrmMemory, MandatoryPrefix mandatoryPrefix) =>
			new FuzzerInstruction(Code.INVALID, FuzzerInstructionFlags.None, 0, 0, mandatoryPrefix, table, opCode, groupIndex, -1, isModrmMemory, 0, 0);
		internal static FuzzerInstruction CreateInvalid3dnow(OpCode opCode, bool isModrmMemory) =>
			new FuzzerInstruction(Code.INVALID, FuzzerInstructionFlags.None, 0, 0, MandatoryPrefix.None, new FuzzerOpCodeTable(EncodingKind.D3NOW, OpCodeTableIndexes.D3nowTable), opCode, -1, -1, isModrmMemory, 0, 0);
		internal static FuzzerInstruction CreateInvalidVec(FuzzerOpCodeTable table, OpCode opCode, int groupIndex, int rmGroupIndex, bool isModrmMemory, MandatoryPrefix mandatoryPrefix, uint w, uint l, FuzzerInstructionFlags flags) =>
			new FuzzerInstruction(Code.INVALID, flags, w, l, mandatoryPrefix, table, opCode, groupIndex, rmGroupIndex, isModrmMemory, 0, 0);

		internal static FuzzerInstruction CreateValid(Code code, bool isModrmMemory, uint w, uint l, MandatoryPrefix mandatoryPrefix, int groupIndex, OpCode? opCode = null) {
			var opc = code.ToOpCode();
			var table = GetTable(opc.Encoding, opc.Table);
			var realOpCode = opCode ?? OpCode.CreateFromUInt32(opc.OpCode, opc.OpCodeLength);
			const FuzzerInstructionFlags flags = FuzzerInstructionFlags.None;
			return new FuzzerInstruction(code, flags, w, l, mandatoryPrefix, table, realOpCode, groupIndex, opc.RmGroupIndex, isModrmMemory, opc.OperandSize, opc.AddressSize);
		}

		static FuzzerOpCodeTable GetTable(EncodingKind encoding, OpCodeTableKind table) =>
			encoding switch {
				EncodingKind.Legacy => table switch {
					OpCodeTableKind.Normal => new FuzzerOpCodeTable(encoding, OpCodeTableIndexes.LegacyTable_Normal),
					OpCodeTableKind.T0F => new FuzzerOpCodeTable(encoding, OpCodeTableIndexes.LegacyTable_0F),
					OpCodeTableKind.T0F38 => new FuzzerOpCodeTable(encoding, OpCodeTableIndexes.LegacyTable_0F38),
					OpCodeTableKind.T0F3A => new FuzzerOpCodeTable(encoding, OpCodeTableIndexes.LegacyTable_0F3A),
					_ => throw ThrowHelpers.Unreachable,
				},
				EncodingKind.VEX => table switch {
					OpCodeTableKind.Normal => new FuzzerOpCodeTable(encoding, 0),
					OpCodeTableKind.T0F => new FuzzerOpCodeTable(encoding, 1),
					OpCodeTableKind.T0F38 => new FuzzerOpCodeTable(encoding, 2),
					OpCodeTableKind.T0F3A => new FuzzerOpCodeTable(encoding, 3),
					_ => throw ThrowHelpers.Unreachable,
				},
				EncodingKind.EVEX => table switch {
					OpCodeTableKind.T0F => new FuzzerOpCodeTable(encoding, 1),
					OpCodeTableKind.T0F38 => new FuzzerOpCodeTable(encoding, 2),
					OpCodeTableKind.T0F3A => new FuzzerOpCodeTable(encoding, 3),
					OpCodeTableKind.MAP5 => new FuzzerOpCodeTable(encoding, 5),
					OpCodeTableKind.MAP6 => new FuzzerOpCodeTable(encoding, 6),
					_ => throw ThrowHelpers.Unreachable,
				},
				EncodingKind.XOP => table switch {
					OpCodeTableKind.MAP8 => new FuzzerOpCodeTable(encoding, 8),
					OpCodeTableKind.MAP9 => new FuzzerOpCodeTable(encoding, 9),
					OpCodeTableKind.MAP10 => new FuzzerOpCodeTable(encoding, 10),
					_ => throw ThrowHelpers.Unreachable,
				},
				EncodingKind.D3NOW => table switch {
					OpCodeTableKind.T0F => new FuzzerOpCodeTable(encoding, OpCodeTableIndexes.D3nowTable),
					_ => throw ThrowHelpers.Unreachable,
				},
				EncodingKind.MVEX => table switch {
					OpCodeTableKind.T0F => new FuzzerOpCodeTable(encoding, 1),
					OpCodeTableKind.T0F38 => new FuzzerOpCodeTable(encoding, 2),
					OpCodeTableKind.T0F3A => new FuzzerOpCodeTable(encoding, 3),
					_ => throw ThrowHelpers.Unreachable,
				},
				_ => throw ThrowHelpers.Unreachable,
			};

		internal byte GetByteOpCode() {
			if (OpCode.IsOneByte)
				return OpCode.Byte0;
			if (OpCode.IsTwobyte) {
				// It should be a group opcode
				Assert.True(OpCode.Byte1 >= 0xC0);
				return OpCode.Byte0;
			}
			throw ThrowHelpers.Unreachable;
		}

		internal FuzzerInstruction WithGroup(OpCode opCode, int groupIndex) =>
			CreateValid(Code, IsModrmMemory, W, L, MandatoryPrefix, groupIndex, opCode);
	}

	public sealed class FuzzerOpCode {
		public readonly List<FuzzerInstruction> Instructions;
		public readonly FuzzerOpCodeTable Table;
		public readonly byte OpCode;
		public readonly bool IsModrmMemory;

		public FuzzerOpCode(FuzzerOpCodeTable table, byte opCode, bool isModrmMemory) {
			Instructions = new List<FuzzerInstruction>();
			Table = table;
			OpCode = opCode;
			IsModrmMemory = isModrmMemory;
		}
	}
}
