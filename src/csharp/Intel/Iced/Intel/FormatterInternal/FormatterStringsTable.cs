// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using Iced.Intel.Internal;

namespace Iced.Intel.FormatterInternal {
	static partial class FormatterStringsTable {
		// The returned array isn't cached since only one formatter is normally used
		public static string[] GetStringsTable() {
			var reader = new DataReader(GetSerializedStrings(), MaxStringLength);
			var strings = new string[StringsCount];
			for (int i = 0; i < strings.Length; i++)
				strings[i] = reader.ReadAsciiString();
			if (reader.CanRead)
				throw new InvalidOperationException();
			return strings;
		}
	}
}
#endif
