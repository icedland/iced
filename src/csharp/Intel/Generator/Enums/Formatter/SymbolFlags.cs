// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter {
	[Enum("SymbolFlags", Documentation = "Symbol flags", Public = true, Flags = true, NoInitialize = true)]
	[Flags]
	enum SymbolFlags : uint {
		[Comment("No bit is set")]
		None				= 0,

		[Comment("It's a symbol relative to a register, eg. a struct offset #(c:[ebx+some_struct.field1])#. If this is cleared, it's the address of a symbol.")]
		Relative			= 0x00000001,

		[Comment("It's a signed symbol and it should be displayed as #(c:-symbol)# or #(c:reg-symbol)# instead of #(c:symbol)# or #(c:reg+symbol)#")]
		Signed				= 0x00000002,

		[Comment("Set if #(f:SymbolResult.SymbolSize)# is valid")]
		HasSymbolSize		= 0x00000004,
	}
}
