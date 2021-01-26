// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System;

namespace IcedFuzzer.Core {
	static class ThrowHelpers {
		public static Exception Unreachable => new InvalidOperationException("Unreachable code");
	}
}
