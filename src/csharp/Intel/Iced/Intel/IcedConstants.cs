// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Runtime.CompilerServices;

namespace Iced.Intel {
	static partial class IcedConstants {
#if MVEX
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsMvex(Code code) => ((uint)code - MvexStart) < MvexLength;
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsMvex(Code code) => false;
#endif
	}
}
