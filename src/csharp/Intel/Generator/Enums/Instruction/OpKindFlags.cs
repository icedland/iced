// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

namespace Generator.Enums.Instruction {
	/// <summary>
	/// [4:0]	= Operand #0's <c>OpKind</c>
	/// [9:5]	= Operand #1's <c>OpKind</c>
	/// [14:10]	= Operand #2's <c>OpKind</c>
	/// [19:15]	= Operand #3's <c>OpKind</c>
	/// [23:20]	= db/dw/dd/dq element count (1-16, 1-8, 1-4, or 1-2)
	/// [29:24]	= Not used
	/// [31:30]	= CodeSize
	/// </summary>
	[Enum(nameof(OpKindFlags), "Instruction_OpKindFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum OpKindFlags : uint {
		OpKindBits				= 5,
		OpKindMask				= (1 << (int)OpKindBits) - 1,
		Op1KindShift			= 5,
		Op2KindShift			= 10,
		Op3KindShift			= 15,
		DataLengthMask			= 0xF,
		DataLengthShift			= 20,
		// Unused bits here
		CodeSizeMask			= 3,
		CodeSizeShift			= 30,

		// Bits ignored by Equals()
		EqualsIgnoreMask		= CodeSizeMask << (int)CodeSizeShift,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class OpKindFlagsEnum {
		OpKindFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<OpKind>((uint)OpKindFlags.OpKindMask);
			ConstantUtils.VerifyMask<CodeSize>((uint)OpKindFlags.CodeSizeMask);
		}
	}
}
