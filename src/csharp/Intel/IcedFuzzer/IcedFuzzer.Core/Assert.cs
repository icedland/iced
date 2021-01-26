// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System;
using System.Diagnostics.CodeAnalysis;

namespace IcedFuzzer.Core {
	static class Assert {
		public static void True([DoesNotReturnIf(false)] bool b) {
			if (!b)
				throw new InvalidOperationException("Assert.True() failed");
		}

		public static void True([DoesNotReturnIf(false)] bool b, string message) {
			if (!b)
				throw new InvalidOperationException(message);
		}

		public static void False([DoesNotReturnIf(true)] bool b) {
			if (b)
				throw new InvalidOperationException("Assert.False() failed");
		}

		public static void False([DoesNotReturnIf(true)] bool b, string message) {
			if (b)
				throw new InvalidOperationException(message);
		}

		[DoesNotReturn]
		public static void Fail(string message) => throw new InvalidOperationException(message);
	}
}
