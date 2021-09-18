// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Instruction {
	/// <summary>
	/// [4:0]	= Not used
	/// [7:5]	= Segment register prefix: none, es, cs, ss, ds, fs, gs, reserved
	/// [11:8]	= db/dw/dd/dq element count (1-16, 1-8, 1-4, or 1-2)
	/// [14:12]	= <c>RoundingControl</c> enum
	/// [17:15]	= Opmask register or 0 if none
	/// [19:18]	= CodeSize
	/// [23:20]	= Not used
	/// [24]	= Broadcast memory
	/// [25]	= Suppress all exceptions
	/// [26]	= Zeroing masking
	/// [27]	= xacquire prefix
	/// [28]	= xrelease prefix
	/// [29]	= repe prefix
	/// [30]	= repne prefix
	/// [31]	= lock prefix
	/// </summary>
	[Enum(nameof(InstrFlags1), "InstrFlags1", Flags = true, NoInitialize = true)]
	[Flags]
	enum InstrFlags1 : uint {
		SegmentPrefixMask		= 7,
		SegmentPrefixShift		= 5,
		DataLengthMask			= 0xF,
		DataLengthShift			= 8,
		RoundingControlMask		= 7,
		RoundingControlShift	= 12,
		OpMaskMask				= 7,
		OpMaskShift				= 15,
		CodeSizeMask			= 3,
		CodeSizeShift			= 18,
		// Unused bits here
		Broadcast				= 0x04000000,
		SuppressAllExceptions	= 0x08000000,
		ZeroingMasking			= 0x10000000,
		RepePrefix				= 0x20000000,
		RepnePrefix				= 0x40000000,
		LockPrefix				= 0x80000000,

		// Bits ignored by Equals()
		EqualsIgnoreMask		= CodeSizeMask << (int)CodeSizeShift,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrFlags1Enum {
		InstrFlags1Enum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<RoundingControl>((uint)InstrFlags1.RoundingControlMask);
			ConstantUtils.VerifyMask<CodeSize>((uint)InstrFlags1.CodeSizeMask);
		}
	}
}
