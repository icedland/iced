// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;

namespace Generator.Constants {
	static class EnumUtils {
		public static EnumType GetEnumType(GenTypes genTypes, ConstantKind kind) =>
			kind switch {
				ConstantKind.Register => genTypes[TypeIds.Register],
				ConstantKind.MemorySize => genTypes[TypeIds.MemorySize],
				_ => throw new InvalidOperationException(),
			};
	}
}
