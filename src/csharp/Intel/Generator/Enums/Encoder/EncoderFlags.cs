// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Encoder {
	[Enum("EncoderFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum EncoderFlags : uint {
		None				= 0,
		B					= 0x00000001,
		X					= 0x00000002,
		R					= 0x00000004,
		W					= 0x00000008,

		ModRM				= 0x00000010,
		Sib					= 0x00000020,
		REX					= 0x00000040,
		P66					= 0x00000080,
		P67					= 0x00000100,

		[Comment("#(c:EVEX.R')#")]
		R2					= 0x00000200,
		Broadcast			= 0x00000400,
		HighLegacy8BitRegs	= 0x00000800,
		Displ				= 0x00001000,
		PF0					= 0x00002000,
		RegIsMemory			= 0x00004000,
		MustUseSib			= 0x00008000,

		VvvvvShift			= 27,// 5 bits
		VvvvvMask			= 0x1F,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class EncoderFlagsEnum {
		EncoderFlagsEnum(GenTypes genTypes) {
			if ((uint)EncoderFlags.VvvvvShift + 5 > 32)
				throw new InvalidOperationException();
		}
	}
}
