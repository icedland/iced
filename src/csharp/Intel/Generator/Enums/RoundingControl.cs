// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums {
	[Enum("RoundingControl", Documentation = "Rounding control", Public = true)]
	enum RoundingControl {
		[Comment("No rounding mode")]
		None,
		[Comment("Round to nearest (even)")]
		RoundToNearest,
		[Comment("Round down (toward -inf)")]
		RoundDown,
		[Comment("Round up (toward +inf)")]
		RoundUp,
		[Comment("Round toward zero (truncate)")]
		RoundTowardZero,
	}
}
