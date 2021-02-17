// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("VectorLength")]
	enum VectorLength : byte {
		L128	= 0,
		L256	= 1,
		L512	= 2,
		Unknown	= 3,
	}
}
