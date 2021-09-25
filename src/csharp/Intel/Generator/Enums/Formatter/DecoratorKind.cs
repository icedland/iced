// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
		[Comment("MVEX swizzle or memory up/down conversion: #(c:{dacb})# or #(c:{sint16})#")]
		SwizzleMemConv,
		[Comment("MVEX eviction hint: #(c:{eh})#")]
		EvictionHint,
	}
}
