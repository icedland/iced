// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter.Masm {
	[Enum(nameof(SymbolTestFlags), "MasmSymbolTestFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum SymbolTestFlags {
		None					= 0,
		[Comment("Symbol is found")]
		Symbol					= 1,
		[Comment("Signed symbol")]
		Signed					= 2,
		[Comment("#(c:options.MasmSymbolDisplInBrackets)#")]
		SymbolDisplInBrackets	= 4,
		[Comment("#(c:options.MasmDisplInBrackets)#")]
		DisplInBrackets			= 8,
		[Comment("#(c:options.RipRelativeAddresses)#")]
		Rip						= 0x10,
		[Comment("#(c:options.ShowZeroDisplacements)#")]
		ShowZeroDisplacements	= 0x20,
		[Comment("#(c:!options.MasmAddDsPrefix32)#")]
		NoAddDsPrefix32			= 0x40,
	}
}
