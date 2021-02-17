// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter.Gas {
	[Enum(nameof(InstrOpInfoFlags), "GasInstrOpInfoFlags", Flags = true, NoInitialize = true)]
	[Flags]
	internal enum InstrOpInfoFlags : ushort {
		None						= 0,
		MnemonicSuffixIfMem			= 1,
		SizeOverrideMask			= 3,
		OpSizeShift					= 1,
		OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
		OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
		OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
		AddrSizeShift				= 3,
		AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
		AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
		AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,
		IndirectOperand				= 0x0020,
		OpSizeIsByteDirective		= 0x0040,
		KeepOperandOrder			= 0x0080,
		JccNotTaken					= 0x0100,
		JccTaken					= 0x0200,
		BndPrefix					= 0x0400,
		IgnoreIndexReg				= 0x0800,
		MnemonicIsDirective			= 0x1000,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpInfoFlagsEnum {
		InstrOpInfoFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<SizeOverride>((uint)InstrOpInfoFlags.SizeOverrideMask);
		}
	}
}
