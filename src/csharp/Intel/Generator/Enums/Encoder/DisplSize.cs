// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums.Encoder {
	[Enum("DisplSize")]
	enum DisplSize : byte {
		None,
		Size1,
		Size2,
		Size4,
		Size8,
		RipRelSize4_Target32,
		RipRelSize4_Target64,
	}
}
