// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums.Decoder;

namespace Generator.Enums.Encoder {
	[Enum("EncFlags2", Flags = true, NoInitialize = true)]
	enum EncFlags2 : uint {
		None					= 0,
		OpCodeShift				= 0,
		// [15:0] = opcode (1 or 2 bytes)
		OpCodeIs2Bytes			= 0x00010000,
		/// <summary>
		/// <see cref="LegacyOpCodeTable"/>
		/// <see cref="VexOpCodeTable"/>
		/// <see cref="XopOpCodeTable"/>
		/// <see cref="EvexOpCodeTable"/>
		/// <see cref="MvexOpCodeTable"/>
		/// </summary>
		TableShift				= 17,
		TableMask				= 7,
		/// <summary><see cref="MandatoryPrefixByte"/></summary>
		MandatoryPrefixShift	= 20,
		MandatoryPrefixMask		= 3,
		/// <summary><see cref="WBit"/></summary>
		WBitShift				= 22,
		WBitMask				= 3,
		/// <summary><see cref="LBit"/></summary>
		LBitShift				= 24,
		LBitMask				= 7,
		GroupIndexShift			= 27,
		GroupIndexMask			= 7,
		HasMandatoryPrefix		= 0x40000000,
		HasGroupIndex			= 0x80000000,
	}
}
