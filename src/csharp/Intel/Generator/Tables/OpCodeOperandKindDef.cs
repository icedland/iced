// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics;
using Generator.Enums;

namespace Generator.Tables {
	/// <summary>
	/// Operand encoding and kind
	/// </summary>
	enum OperandEncoding {
		/// <summary>
		/// Not an operand
		/// </summary>
		None,
		/// <summary>
		/// Signed {8,16,32}-bit relative branch
		/// </summary>
		NearBranch,
		/// <summary>
		/// <c>XBEGIN</c> signed {16,32}-bit relative branch
		/// </summary>
		Xbegin,
		/// <summary>
		/// <c>JMPE</c> unsigned {16,32}-bit offset branch (not a near relative branch)
		/// </summary>
		AbsNearBranch,
		/// <summary>
		/// Far branch (unsigned {16,32}-bit offset + unsigned 16-bit segment/selector)
		/// </summary>
		FarBranch,
		/// <summary>
		/// {8,16,32,64}-bit immediate that may or may not be sign extended
		/// </summary>
		Immediate,
		/// <summary>
		/// Implied constant (eg. <c>1</c>) and not encoded in the instruction
		/// </summary>
		ImpliedConst,
		/// <summary>
		/// Implied register (eg. <c>AX</c>) and not encoded in the instruction
		/// </summary>
		ImpliedRegister,
		/// <summary>
		/// <c>seg:[rBX]</c> memory operand. The base register depends on the effective address size and can be overridden with the <c>67h</c> prefix.
		/// </summary>
		SegRBX,
		/// <summary>
		/// <c>seg:[rSI]</c> memory operand. The base register depends on the effective address size and can be overridden with the <c>67h</c> prefix.
		/// </summary>
		SegRSI,
		/// <summary>
		/// <c>seg:[rDI]</c> memory operand. The base register depends on the effective address size and can be overridden with the <c>67h</c> prefix.
		/// </summary>
		SegRDI,
		/// <summary>
		/// <c>ES:[rDI]</c> memory operand. The base register depends on the effective address size and can be overridden with the <c>67h</c> prefix.
		/// </summary>
		ESRDI,
		/// <summary>
		/// <c>/is4</c> and <c>/is5</c> operand: the register operand is stored in the upper 4 bits of an 8-bit immediate
		/// </summary>
		RegImm,
		/// <summary>
		/// The register operand is encoded in the low 3 bits of the opcode (64-bit: REX.B is the 4th bit)
		/// </summary>
		RegOpCode,
		/// <summary>
		/// The register operand is encoded in <c>modrm.reg</c> (64-bit: REX.R is the 4th bit)
		/// </summary>
		RegModrmReg,
		/// <summary>
		/// The register operand is encoded in <c>modrm.mod==11b</c> + <c>modrm.rm</c> (no mem operand allowed) (64-bit: REX.B is the 4th bit)
		/// </summary>
		RegModrmRm,
		/// <summary>
		/// The register/memory operand is encoded in <c>modrm.mod</c> + <c>modrm.rm</c> (64-bit: REX.B is the 4th bit)
		/// </summary>
		RegMemModrmRm,
		/// <summary>
		/// The register operand is encoded in <c>V'vvvv</c>
		/// </summary>
		RegVvvvv,
		/// <summary>
		/// The memory operand is encoded in <c>modrm.mod!=11b</c> + <c>modrm.rm</c> (no register operand allowed)
		/// </summary>
		MemModrmRm,
		/// <summary>
		/// The memory operand is an unsigned {16,32,64}-bit offset (no modrm byte). The size of the offset depends on the effective
		/// address size and can be overridden with the <c>67h</c> prefix.
		/// </summary>
		MemOffset,
	}

	[Flags]
	enum OpCodeOperandKindDefFlags : uint {
		/// <summary>
		/// No bit is set
		/// </summary>
		None				= 0,
		/// <summary>
		/// The <c>LOCK</c> bit can be used as an extra register bit
		/// </summary>
		LockBit				= 0x00000001,
		/// <summary>
		/// 2 regs are used (bit <c>[0]</c> is ignored) (eg. <c>k1+1</c>)
		/// </summary>
		RegPlus1			= 0x00000002,
		/// <summary>
		/// 4 regs are used (bits <c>[1:0]</c> are ignored) (eg. <c>xmm1+3</c>)
		/// </summary>
		RegPlus3			= 0x00000004,
		/// <summary>
		/// It's a memory operand
		/// </summary>
		Memory				= 0x00000008,
		/// <summary>
		/// MPX memory operand. Must be 32-bit addressing in 16/32-bit mode and is forced to be 64-bit addressing in 64-bit mode. RIP-relative memory operands aren't allowed.
		/// </summary>
		MPX					= 0x00000010,
		/// <summary>
		/// MIB memory operand. Must be 32-bit addressing in 16/32-bit mode and is forced to be 64-bit addressing in 64-bit mode. RIP-relative memory operands aren't allowed.
		/// </summary>
		MIB					= 0x00000020,
		/// <summary>
		/// A SIB byte must be present
		/// </summary>
		SibRequired			= 0x00000040,
		/// <summary>
		/// It's a VSIB32 memory operand
		/// </summary>
		Vsib32				= 0x00000080,
		/// <summary>
		/// It's a VSIB64 memory operand
		/// </summary>
		Vsib64				= 0x00000100,
		/// <summary>
		/// It's encoded in the modrm byte
		/// </summary>
		Modrm				= 0x00000200,
		/// <summary>
		/// <c>/is5</c> instructions: 4-bit immediate stored in the low 4 bits of an 8-bit immediate (upper 4 bits is the
		/// register bits, see <see cref="OperandEncoding.RegImm"/>)
		/// </summary>
		M2Z					= 0x00000400,
		/// <summary>
		/// (if <see cref="OperandEncoding.RegImm"/>): <c>/is5</c> register operand, else <c>/is4</c>
		/// </summary>
		Is5					= 0x00000800,
	}

	[DebuggerDisplay("{EnumValue.RawName} {OperandEncoding} {Flags}")]
	sealed class OpCodeOperandKindDef {
		public EnumValue EnumValue { get; }
		public OpCodeOperandKindDefFlags Flags { get; }
		public OperandEncoding OperandEncoding { get; }

		public bool LockBit => (Flags & OpCodeOperandKindDefFlags.LockBit) != 0;
		public bool RegPlus1 => (Flags & OpCodeOperandKindDefFlags.RegPlus1) != 0;
		public bool RegPlus3 => (Flags & OpCodeOperandKindDefFlags.RegPlus3) != 0;
		public bool Memory => (Flags & OpCodeOperandKindDefFlags.Memory) != 0;
		public bool MPX => (Flags & OpCodeOperandKindDefFlags.MPX) != 0;
		public bool MIB => (Flags & OpCodeOperandKindDefFlags.MIB) != 0;
		public bool SibRequired => (Flags & OpCodeOperandKindDefFlags.SibRequired) != 0;
		public bool Vsib => (Flags & (OpCodeOperandKindDefFlags.Vsib32 | OpCodeOperandKindDefFlags.Vsib64)) != 0;
		public bool Vsib32 => (Flags & OpCodeOperandKindDefFlags.Vsib32) != 0;
		public bool Vsib64 => (Flags & OpCodeOperandKindDefFlags.Vsib64) != 0;
		public bool Modrm => (Flags & OpCodeOperandKindDefFlags.Modrm) != 0;
		public bool M2Z => (Flags & OpCodeOperandKindDefFlags.M2Z) != 0;

		readonly int arg1, arg2;
		readonly Register register;

		public OpCodeOperandKindDef(EnumValue enumValue, OpCodeOperandKindDefFlags flags, OperandEncoding encoding, int arg1, int arg2, Register register) {
			EnumValue = enumValue;
			Flags = flags;
			OperandEncoding = encoding;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.register = register;
		}

		internal int Arg1 => arg1;
		internal int Arg2 => arg2;

		/// <summary>
		/// Used if <see cref="OperandEncoding"/> is
		/// <see cref="OperandEncoding.NearBranch"/>,
		/// <see cref="OperandEncoding.Xbegin"/>,
		/// <see cref="OperandEncoding.AbsNearBranch"/>,
		/// <see cref="OperandEncoding.FarBranch"/>
		/// <br/>
		/// <br/>
		/// Size in bits of the branch displacement. This is 8, 16 or 32 bits if it's a
		/// near branch else it's 16 or 32 bits.
		/// </summary>
		public int BranchOffsetSize => arg1;

		/// <summary>
		/// Used if <see cref="OperandEncoding"/> == <see cref="OperandEncoding.NearBranch"/><br/>
		/// <br/>
		/// Operand size in bits (16, 32 or 64 bits)
		/// </summary>
		public int NearBranchOpSize => arg2;

		/// <summary>
		/// Used if <see cref="OperandEncoding"/> == <see cref="OperandEncoding.Immediate"/><br/>
		/// <br/>
		/// Size in bits of the immediate encoded in the instruction (2, 8, 16, 32, 64 bits)
		/// </summary>
		public int ImmediateSize => arg1;
		/// <summary>
		/// Used if <see cref="OperandEncoding"/> == <see cref="OperandEncoding.Immediate"/><br/>
		/// <br/>
		/// Size in bits of the immediate after being sign extended (2, 8, 16, 32, 64 bits) and this value is &gt;= <see cref="ImmediateSize"/>
		/// </summary>
		public int ImmediateSignExtSize => arg2;

		/// <summary>
		/// Used if <see cref="OperandEncoding"/> == <see cref="OperandEncoding.ImpliedConst"/><br/>
		/// <br/>
		/// Implied value not encoded in the instruction
		/// </summary>
		public int ImpliedConst => arg1;

		/// <summary>
		/// Used if <see cref="OperandEncoding"/> is
		/// <see cref="OperandEncoding.ImpliedRegister"/>,
		/// <see cref="OperandEncoding.RegImm"/>,
		/// <see cref="OperandEncoding.RegOpCode"/>,
		/// <see cref="OperandEncoding.RegModrmReg"/>,
		/// <see cref="OperandEncoding.RegModrmRm"/>,
		/// <see cref="OperandEncoding.RegMemModrmRm"/>,
		/// <see cref="OperandEncoding.RegVvvvv"/>,
		/// <see cref="OperandEncoding.MemModrmRm"/> (if <see cref="Vsib"/> only)
		/// <br/>
		/// <br/>
		/// If <see cref="OperandEncoding.ImpliedRegister"/>, it's the implied register (not encoded in the
		/// instruction), else it's the base register that's encoded in the instruction (eg.
		/// <see cref="Register.EAX"/>, <see cref="Register.YMM0"/>, <see cref="Register.K0"/>).
		/// </summary>
		public Register Register => register;

		/// <summary>
		/// <see langword="true"/> if <see cref="Register"/> is valid
		/// </summary>
		public bool HasRegister =>
			OperandEncoding switch {
				OperandEncoding.ImpliedRegister or OperandEncoding.RegImm or OperandEncoding.RegOpCode or OperandEncoding.RegModrmReg or
				OperandEncoding.RegModrmRm or OperandEncoding.RegMemModrmRm or OperandEncoding.RegVvvvv => true,
				OperandEncoding.MemModrmRm => Vsib,
				_ => false,
			};
	}
}
