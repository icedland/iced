// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace System {
	// Array.Empty<T>() is only available since net46 (net45 doesn't support it)
	static class Array2 {
		public static T[] Empty<T>() => EmptyClass<T>.Empty;

		static class EmptyClass<T> {
			public static readonly T[] Empty = new T[0];
		}
	}
}
