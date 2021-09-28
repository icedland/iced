// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("MvexConvFn", Documentation = "MVEX conversion function", Public = true)]
	enum MvexConvFn {
		[Comment("No conversion function")]
		None,
		[Comment("Sf32(xxx)")]
		Sf32,
		[Comment("Sf64(xxx)")]
		Sf64,
		[Comment("Si32(xxx)")]
		Si32,
		[Comment("Si64(xxx)")]
		Si64,
		[Comment("Uf32(xxx)")]
		Uf32,
		[Comment("Uf64(xxx)")]
		Uf64,
		[Comment("Ui32(xxx)")]
		Ui32,
		[Comment("Ui64(xxx)")]
		Ui64,
		[Comment("Df32(xxx)")]
		Df32,
		[Comment("Df64(xxx)")]
		Df64,
		[Comment("Di32(xxx)")]
		Di32,
		[Comment("Di64(xxx)")]
		Di64,
	}
}
