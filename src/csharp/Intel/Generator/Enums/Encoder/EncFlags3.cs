// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Encoder {
	[Enum("EncFlags3", Flags = true, NoInitialize = true)]
	enum EncFlags3 : uint {
		None					= 0,
		/// <summary><see cref="EncodingKind"/></summary>
		EncodingShift			= 0,
		EncodingMask			= 7,
		/// <summary><see cref="CodeSize"/></summary>
		OperandSizeShift		= 3,
		OperandSizeMask			= 3,
		/// <summary><see cref="CodeSize"/></summary>
		AddressSizeShift		= 5,
		AddressSizeMask			= 3,
		/// <summary><see cref="TupleType"/></summary>
		TupleTypeShift			= 7,
		TupleTypeMask			= 0x1F,

		DefaultOpSize64			= 0x00001000,
		HasRmGroupIndex			= 0x00002000,
		IntelForceOpSize64		= 0x00004000,
		Fwait					= 0x00008000,
		Bit16or32				= 0x00010000,
		Bit64					= 0x00020000,
		Lock					= 0x00040000,
		Xacquire				= 0x00080000,
		Xrelease				= 0x00100000,
		Rep						= 0x00200000,
		Repne					= 0x00400000,
		Bnd						= 0x00800000,
		HintTaken				= 0x01000000,
		Notrack					= 0x02000000,
		Broadcast				= 0x04000000,
		RoundingControl			= 0x08000000,
		SuppressAllExceptions	= 0x10000000,
		OpMaskRegister			= 0x20000000,
		ZeroingMasking			= 0x40000000,
		RequireOpMaskRegister	= 0x80000000,
	}
}
