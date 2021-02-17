// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.FastFormatterInternal {
	static partial class FmtData {
		public static readonly string[] Mnemonics = ParseData(out Flags);
		public static readonly FastFmtFlags[] Flags;

		static string[] ParseData(out FastFmtFlags[] outFlags) {
			var reader = new DataReader(GetSerializedData());
			var strings = FormatterStringsTable.GetStringsTable();
			var mnemonics = new string[IcedConstants.CodeEnumCount];
			var flags = new FastFmtFlags[IcedConstants.CodeEnumCount];

			var ca = new char[1];
			int prevIndex = -1;
			var prevFlags = FastFmtFlags.None;
			for (int i = 0; i < mnemonics.Length; i++) {
				var f = (FastFmtFlags)reader.ReadByte();
				int currentIndex;
				if ((f & FastFmtFlags.SameAsPrev) != 0) {
					currentIndex = reader.Index;
					reader.Index = prevIndex;
				}
				else {
					currentIndex = -1;
					prevIndex = reader.Index;
				}
				var mnemonic = strings[reader.ReadCompressedUInt32()];
				if ((prevFlags & FastFmtFlags.HasVPrefix) == (f & FastFmtFlags.HasVPrefix) &&
					(f & FastFmtFlags.SameAsPrev) != 0) {
					mnemonic = mnemonics[i - 1];
				}
				else if ((f & FastFmtFlags.HasVPrefix) != 0) {
					ca[0] = 'v';
					mnemonic = string.Intern(new string(ca) + mnemonic);
				}

				flags[i] = f;
				mnemonics[i] = mnemonic;
				prevFlags = f;
				if (currentIndex >= 0)
					reader.Index = currentIndex;
			}
			if (reader.CanRead)
				throw new InvalidOperationException();

			outFlags = flags;
			return mnemonics;
		}
	}
}
#endif
