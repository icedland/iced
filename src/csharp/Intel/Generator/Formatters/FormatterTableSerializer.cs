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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters {
	abstract class FormatterTableSerializer {
		public abstract void Initialize(StringsTable stringsTable);
		public abstract string GetFilename(ProjectDirs projectDirs);
		public abstract void Serialize(FileWriter writer, StringsTable stringsTable);

		protected void Initialize(StringsTable stringsTable, object[][] infos) {
			var expectedLength = Constants.IcedConstantsType.Instance["NumberOfCodeValues"].Value;
			if ((uint)infos.Length != expectedLength)
				throw new InvalidOperationException($"Found {infos.Length} elements, expected {expectedLength}");
			foreach (var info in infos) {
				bool ignoreVPrefix = true;
				foreach (var o in info) {
					if (o is string s) {
						stringsTable.Add(s, ignoreVPrefix);
						ignoreVPrefix = false;
					}
				}
			}
		}

		protected uint GetFirstStringIndex(StringsTable stringsTable, object[] info, out bool hasVPrefix) {
			if (!(info[2] is string s))
				throw new InvalidOperationException();
			return stringsTable.GetIndex(s, ignoreVPrefix: true, out hasVPrefix);
		}

		protected static bool IsSame(object[] a, object[] b) {
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++) {
				if (i == 1) {
					if (!(a[i] is EnumValue eva && eva.DeclaringType.EnumKind == EnumKind.Code) ||
						!(b[i] is EnumValue evb && evb.DeclaringType.EnumKind == EnumKind.Code)) {
						throw new InvalidOperationException();
					}
					continue;
				}
				bool same;
				if (a[i] is string sa && b[i] is string sb)
					same = StringComparer.Ordinal.Equals(sa, sb);
				else
					same = a[i].Equals(b[i]);
				if (!same)
					return false;
			}
			return true;
		}
	}
}
#endif
