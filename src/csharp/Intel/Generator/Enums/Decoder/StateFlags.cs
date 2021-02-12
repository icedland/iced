// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

namespace Generator.Enums.Decoder {
	[Enum("StateFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum StateFlags : uint {
		// Only used by Debug.Assert()
		EncodingMask			= 7,
		HasRex					= 0x00000008,
		b						= 0x00000010,
		z						= 0x00000020,
		IsInvalid				= 0x00000040,
		W						= 0x00000080,
		NoImm					= 0x00000100,
		Addr64					= 0x00000200,
		BranchImm8				= 0x00000400,
		Xbegin					= 0x00000800,
		Lock					= 0x00001000,
		AllowLock				= 0x00002000,
		NoMoreBytes				= 0x00004000,
		Has66					= 0x00008000,
		IpRel					= 0x00010000,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class StateFlagsEnum {
		StateFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<EncodingKind>((uint)StateFlags.EncodingMask);
		}
	}
}
