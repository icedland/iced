// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// This is needed because net45/netstandard2.0 reference assemblies don't
// have any nullable attributes

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System {
	static class string2 {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullOrEmpty([NotNullWhen(false)] string? value) => string.IsNullOrEmpty(value);
	}
}

namespace System.Diagnostics {
	static class Debug2 {
		[Conditional("DEBUG")]
		public static void Assert([DoesNotReturnIf(false)] bool condition) => Debug.Assert(condition);
	}
}
