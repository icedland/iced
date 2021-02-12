// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums {
	[Enum("VectorLength")]
	enum VectorLength : byte {
		L128	= 0,
		L256	= 1,
		L512	= 2,
		Unknown	= 3,
	}
}
