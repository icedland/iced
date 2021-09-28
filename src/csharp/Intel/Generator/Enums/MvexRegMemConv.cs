// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("MvexRegMemConv", Documentation = "MVEX register/memory operand conversion", Public = true)]
	enum MvexRegMemConv {
		[Comment("No operand conversion")]
		None,

		[Comment("Register swizzle: #(c:zmm0)# or #(c:zmm0 {dcba})#")]
		RegSwizzleNone,
		[Comment("Register swizzle: #(c:zmm0 {cdab})#")]
		RegSwizzleCdab,
		[Comment("Register swizzle: #(c:zmm0 {badc})#")]
		RegSwizzleBadc,
		[Comment("Register swizzle: #(c:zmm0 {dacb})#")]
		RegSwizzleDacb,
		[Comment("Register swizzle: #(c:zmm0 {aaaa})#")]
		RegSwizzleAaaa,
		[Comment("Register swizzle: #(c:zmm0 {bbbb})#")]
		RegSwizzleBbbb,
		[Comment("Register swizzle: #(c:zmm0 {cccc})#")]
		RegSwizzleCccc,
		[Comment("Register swizzle: #(c:zmm0 {dddd})#")]
		RegSwizzleDddd,

		[Comment("Memory Up/DownConv: #(c:[rax])# / #(c:zmm0)#")]
		MemConvNone,
		[Comment("Memory UpConv: #(c:[rax] {1to16})# or #(c:[rax] {1to8})#")]
		MemConvBroadcast1,
		[Comment("Memory UpConv: #(c:[rax] {4to16})# or #(c:[rax] {4to8})#")]
		MemConvBroadcast4,
		[Comment("Memory Up/DownConv: #(c:[rax] {float16})# / #(c:zmm0 {float16})#")]
		MemConvFloat16,
		[Comment("Memory Up/DownConv: #(c:[rax] {uint8})# / #(c:zmm0 {uint8})#")]
		MemConvUint8,
		[Comment("Memory Up/DownConv: #(c:[rax] {sint8})# / #(c:zmm0 {sint8})#")]
		MemConvSint8,
		[Comment("Memory Up/DownConv: #(c:[rax] {uint16})# / #(c:zmm0 {uint16})#")]
		MemConvUint16,
		[Comment("Memory Up/DownConv: #(c:[rax] {sint16})# / #(c:zmm0 {sint16})#")]
		MemConvSint16,
	}
}
