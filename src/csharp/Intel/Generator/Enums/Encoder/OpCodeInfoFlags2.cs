// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Tables;

namespace Generator.Enums.Encoder {
	[Enum("OpCodeInfoFlags2", Flags = true, NoInitialize = true)]
	enum OpCodeInfoFlags2 : uint {
		None					= 0,
		RealMode				= 0x00000001,
		ProtectedMode			= 0x00000002,
		Virtual8086Mode			= 0x00000004,
		CompatibilityMode		= 0x00000008,
		UseOutsideSmm			= 0x00000010,
		UseInSmm				= 0x00000020,
		UseOutsideEnclaveSgx	= 0x00000040,
		UseInEnclaveSgx1		= 0x00000080,
		UseInEnclaveSgx2		= 0x00000100,
		UseOutsideVmxOp			= 0x00000200,
		UseInVmxRootOp			= 0x00000400,
		UseInVmxNonRootOp		= 0x00000800,
		UseOutsideSeam			= 0x00001000,
		UseInSeam				= 0x00002000,
		TdxNonRootGenUd			= 0x00004000,
		TdxNonRootGenVe			= 0x00008000,
		TdxNonRootMayGenEx		= 0x00010000,
		IntelVmExit				= 0x00020000,
		IntelMayVmExit			= 0x00040000,
		IntelSmmVmExit			= 0x00080000,
		AmdVmExit				= 0x00100000,
		AmdMayVmExit			= 0x00200000,
		TsxAbort				= 0x00400000,
		TsxImplAbort			= 0x00800000,
		TsxMayAbort				= 0x01000000,
		IntelDecoder16or32		= 0x02000000,
		IntelDecoder64			= 0x04000000,
		AmdDecoder16or32		= 0x08000000,
		AmdDecoder64			= 0x10000000,
		/// <summary><see cref="InstrStrFmtOption"/></summary>
		InstrStrFmtOptionMask	= 7,
		InstrStrFmtOptionShift	= 29,
	}
}
