// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Diagnostics;

namespace Iced.Intel.FormatterInternal {
	readonly struct FormatterString {
		readonly string lower;
#if GAS || INTEL || MASM || NASM
		readonly string upper;
#endif

		public int Length => lower.Length;

		public FormatterString(string lower) {
			Debug.Assert(lower.ToLowerInvariant() == lower);
			this.lower = lower;
#if GAS || INTEL || MASM || NASM
			upper = string.Intern(lower.ToUpperInvariant());
#endif
		}

		public static FormatterString[] Create(string[] strings) {
			var res = new FormatterString[strings.Length];
			for (int i = 0; i < strings.Length; i++)
				res[i] = new FormatterString(strings[i]);
			return res;
		}

#if GAS || INTEL || MASM || NASM
		public string Get(bool upper) =>
			upper ? this.upper : lower;
#endif

#if FAST_FMT
		public string Lower => lower;
#endif
	}
}
#endif
