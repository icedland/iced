// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace IcedFuzzer.Core {
	static class ThrowHelpers {
		public static Exception Unreachable => new InvalidOperationException("Unreachable code");
	}
}
