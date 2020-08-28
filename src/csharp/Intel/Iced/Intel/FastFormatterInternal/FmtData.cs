/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if FAST_FMT
using System;
using Iced.Intel.FormatterInternal;
using Iced.Intel.Internal;

namespace Iced.Intel.FastFormatterInternal {
	static partial class FmtData {
		public static readonly string[] Mnemonics = ParseData(out Flags);
#pragma warning disable CS8618
		public static readonly FastFmtFlags[] Flags;
#pragma warning restore CS8618

		static string[] ParseData(out FastFmtFlags[] outFlags) {
			var reader = new DataReader(GetSerializedData());
			var strings = FormatterStringsTable.GetStringsTable();
			var mnemonics = new string[IcedConstants.NumberOfCodeValues];
			var flags = new FastFmtFlags[IcedConstants.NumberOfCodeValues];

			var ca = new char[1];
			int prevIndex = -1;
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
				if ((f & FastFmtFlags.HasVPrefix) != 0) {
					ca[0] = 'v';
					mnemonic = string.Intern(new string(ca) + mnemonic);
				}

				flags[i] = f;
				mnemonics[i] = mnemonic;
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
