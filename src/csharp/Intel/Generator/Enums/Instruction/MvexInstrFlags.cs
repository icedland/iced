// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Instruction {
	[Enum("MvexInstrFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum MvexInstrFlags : uint {
		// MVEX instructions can only have at most 8-bit immediate operands so bits [31:8] can be used for our needs
		MvexRegMemConvShift		= 16,
		MvexRegMemConvMask		= 0x1F,
		EvictionHint			= 0x80000000,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class MvexInstrFlagsEnum {
		MvexInstrFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<MvexRegMemConv>((uint)MvexInstrFlags.MvexRegMemConvMask);
		}
	}
}
