// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

namespace IcedFuzzer.Core {
	static class ThrowHelpers {
		public static Exception Unreachable => new InvalidOperationException("Unreachable code");
	}
}
