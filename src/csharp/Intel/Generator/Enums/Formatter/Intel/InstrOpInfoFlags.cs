// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter.Intel {
	[Enum(nameof(InstrOpInfoFlags), "IntelInstrOpInfoFlags", Flags = true, NoInitialize = true)]
	[Flags]
	internal enum InstrOpInfoFlags : ushort {
		None						= 0,

		// show no mem size
		MemSize_Nothing				= 1,

		// AlwaysShowMemorySize is disabled: always show memory size
		ShowNoMemSize_ForceSize		= 2,
		ShowMinMemSize_ForceSize	= 4,

		BranchSizeInfoShift			= 3,
		BranchSizeInfoMask			= 1,
		BranchSizeInfo_Short		= BranchSizeInfo.Short << (int)BranchSizeInfoShift,

		SizeOverrideMask			= 3,
		OpSizeShift					= 4,
		OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
		OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
		OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
		AddrSizeShift				= 6,
		AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
		AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
		AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,

		IgnoreOpMask				= 0x00000100,
		FarMnemonic					= 0x00000200,
		JccNotTaken					= 0x00000400,
		JccTaken					= 0x00000800,
		BndPrefix					= 0x00001000,
		IgnoreIndexReg				= 0x00002000,
		IgnoreSegmentPrefix			= 0x00004000,
		MnemonicIsDirective			= 0x00008000,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpInfoFlagsEnum {
		InstrOpInfoFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<BranchSizeInfo>((uint)InstrOpInfoFlags.BranchSizeInfoMask);
			ConstantUtils.VerifyMask<SizeOverride>((uint)InstrOpInfoFlags.SizeOverrideMask);
		}
	}
}
