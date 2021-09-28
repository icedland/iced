// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("TupleType", Documentation = "Tuple type (EVEX/MVEX) which can be used to get the disp8 scale factor #(c:N)#", Public = true)]
	enum TupleType {
		[Comment("#(c:N = 1)#")]
		N1,
		[Comment("#(c:N = 2)#")]
		N2,
		[Comment("#(c:N = 4)#")]
		N4,
		[Comment("#(c:N = 8)#")]
		N8,
		[Comment("#(c:N = 16)#")]
		N16,
		[Comment("#(c:N = 32)#")]
		N32,
		[Comment("#(c:N = 64)#")]
		N64,
		[Comment("#(c:N = b ? 4 : 8)#")]
		N8b4,
		[Comment("#(c:N = b ? 4 : 16)#")]
		N16b4,
		[Comment("#(c:N = b ? 4 : 32)#")]
		N32b4,
		[Comment("#(c:N = b ? 4 : 64)#")]
		N64b4,
		[Comment("#(c:N = b ? 8 : 16)#")]
		N16b8,
		[Comment("#(c:N = b ? 8 : 32)#")]
		N32b8,
		[Comment("#(c:N = b ? 8 : 64)#")]
		N64b8,
		[Comment("#(c:N = b ? 2 : 4)#")]
		N4b2,
		[Comment("#(c:N = b ? 2 : 8)#")]
		N8b2,
		[Comment("#(c:N = b ? 2 : 16)#")]
		N16b2,
		[Comment("#(c:N = b ? 2 : 32)#")]
		N32b2,
		[Comment("#(c:N = b ? 2 : 64)#")]
		N64b2,
	}
}
