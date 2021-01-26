// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter {
	[Enum("DecoratorKind", Documentation = "Decorator", Public = true)]
	enum DecoratorKind {
		[Comment("Broadcast decorator, eg. #(c:{1to4})#")]
		Broadcast,
		[Comment("Rounding control, eg. #(c:{rd-sae})#")]
		RoundingControl,
		[Comment("Suppress all exceptions: #(c:{sae})#")]
		SuppressAllExceptions,
		[Comment("Zeroing masking: #(c:{z})#")]
		ZeroingMasking,
	}
}
