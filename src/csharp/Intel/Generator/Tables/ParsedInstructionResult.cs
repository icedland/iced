// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics;
using Generator.Enums;

namespace Generator.Tables {
	[Flags]
	enum ParsedInstructionOperandFlags : uint {
		None					= 0,
		/// <summary>
		/// Implied register (not encoded in the instruction), eg. es, eax, dx
		/// </summary>
		ImpliedRegister			= 0x00000001,
		/// <summary>
		/// mm1, r32, etc
		/// </summary>
		Register				= 0x00000002,
		/// <summary>
		/// m128
		/// </summary>
		Memory					= 0x00000004,
		/// <summary>
		/// m128bcst
		/// </summary>
		Broadcast				= 0x00000008,
		/// <summary>
		/// k1+1
		/// </summary>
		RegPlus1				= 0x00000010,
		/// <summary>
		/// xmm1+3
		/// </summary>
		RegPlus3				= 0x00000020,
		/// <summary>
		/// 1
		/// </summary>
		ConstImmediate			= 0x00000040,
		/// <summary>
		/// disp16/disp32
		/// </summary>
		DispBranch				= 0x00000080,
		/// <summary>
		/// imm4/imm8/imm16/imm32/imm64
		/// </summary>
		Immediate				= 0x00000100,
		/// <summary>
		/// moffs
		/// </summary>
		MemoryOffset			= 0x00000200,
		/// <summary>
		/// vm32x, vm64y, etc
		/// </summary>
		Vsib					= 0x00000400,
		/// <summary>
		/// ptr16:16, ptr16:32
		/// </summary>
		FarBranch				= 0x00000800,
		/// <summary>
		/// rel8, rel16, rel32
		/// </summary>
		RelBranch				= 0x00001000,
		/// <summary>
		/// mib
		/// </summary>
		MIB						= 0x00002000,
		/// <summary>
		/// sibmem (SIB required)
		/// </summary>
		Sibmem					= 0x00004000,
		/// <summary>
		/// Operand is present in the defs.txt file but hidden (eg. some FPU instructions with a hidden ST(0) operand)
		/// </summary>
		HiddenOperand			= 0x00008000,
	}

	[DebuggerDisplay("{Register} {SizeBits} {Flags}")]
	readonly struct ParsedInstructionOperand {
		public readonly ParsedInstructionOperandFlags Flags;

		/// <summary>
		/// Implied register or base register (eg. <see cref="Register.EAX"/>, <see cref="Register.MM0"/>, etc)
		/// </summary>
		public readonly Register Register;

		/// <summary>
		/// Size in bits of immediate/branch<br/>
		/// Vsib size in bits
		/// </summary>
		public readonly int SizeBits;

		/// <summary>
		/// Memory size in bits
		/// </summary>
		public readonly int MemSizeBits;

		/// <summary>
		/// Memory broadcast size in bits
		/// </summary>
		public readonly int MemSize2Bits;
		
		public readonly MvexConvFn MvexConvFn;

		public ParsedInstructionOperand(ParsedInstructionOperandFlags flags, Register register, int sizeBits, int memSizeBits, int memSize2Bits,
			MvexConvFn mvexConvFn) {
			Flags = flags;
			Register = register;
			SizeBits = sizeBits;
			MemSizeBits = memSizeBits;
			MemSize2Bits = memSize2Bits;
			MvexConvFn = mvexConvFn;
		}
	}

	[Flags]
	enum ParsedInstructionFlags : uint {
		None					= 0,
		/// <summary>
		/// {k1}
		/// </summary>
		OpMask					= 0x00000001,
		/// <summary>
		/// {z}
		/// </summary>
		ZeroingMasking			= 0x00000002,
		/// <summary>
		/// {sae}
		/// </summary>
		SuppressAllExceptions	= 0x00000004,
		/// <summary>
		/// {er}
		/// </summary>
		RoundingControl			= 0x00000008,
		/// <summary>
		/// m64bcst
		/// </summary>
		Broadcast				= 0x00000010,
		/// <summary>
		/// {eh}
		/// </summary>
		EvictionHint			= 0x00000020,
	}

	[DebuggerDisplay("{Mnemonic} {Flags}")]
	readonly struct ParsedInstructionResult {
		public readonly string InstructionStr;
		public readonly ParsedInstructionFlags Flags;
		public readonly string Mnemonic;
		public readonly ParsedInstructionOperand[] Operands;
		public readonly InstrStrImpliedOp[] ImpliedOps;

		public ParsedInstructionResult(string instructionStr, ParsedInstructionFlags flags, string mnemonic, ParsedInstructionOperand[] operands, InstrStrImpliedOp[] impliedOps) {
			foreach (var op in operands) {
				if ((op.Flags & ParsedInstructionOperandFlags.Broadcast) != 0) {
					flags |= ParsedInstructionFlags.Broadcast;
					break;
				}
			}
			InstructionStr = instructionStr;
			Flags = flags;
			Mnemonic = mnemonic;
			Operands = operands;
			ImpliedOps = impliedOps;
		}
	}
}
