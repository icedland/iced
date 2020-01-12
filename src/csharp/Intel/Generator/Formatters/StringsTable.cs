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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Generator.Formatters {
	sealed class StringsTable {
		readonly Dictionary<string, Info> strings;
		Info[]? sortedInfos;
		bool isFrozen;

		public bool IsFrozen => isFrozen;
		public Info[] Infos => sortedInfos ?? throw new InvalidOperationException();

		[DebuggerDisplay("{Count} {String}")]
		public sealed class Info {
			public readonly string String;
			public int Count;
			public uint Index;
			public Info(string s) => String = s;
		}

		public StringsTable() =>
			strings = new Dictionary<string, Info>(StringComparer.Ordinal);

		public void Freeze() {
			if (isFrozen)
				throw new InvalidOperationException();
			isFrozen = true;

			var infos = strings.Values.ToArray();
			Array.Sort(infos, InfoSorter);
			for (int i = 0; i < infos.Length; i++)
				infos[i].Index = (uint)i;
			sortedInfos = infos;
		}

		static int InfoSorter(Info x, Info y) {
			int c = y.Count - x.Count;
			if (c != 0)
				return c;
			return StringComparer.Ordinal.Compare(x.String, y.String);
		}

		public void Add(string s, bool ignoreVPrefix) {
			if (isFrozen)
				throw new InvalidOperationException();
			if (ignoreVPrefix && s.StartsWith("v", StringComparison.Ordinal))
				s = s.Substring(1);
			if (!strings.TryGetValue(s, out var info))
				strings.Add(s, info = new Info(s));
			info.Count++;
		}

		public uint GetIndex(string s, bool ignoreVPrefix, out bool hasVPrefix) {
			if (!isFrozen)
				throw new InvalidOperationException();
			if (ignoreVPrefix && s.StartsWith("v", StringComparison.Ordinal)) {
				s = s.Substring(1);
				hasVPrefix = true;
			}
			else
				hasVPrefix = false;
			if (!strings.TryGetValue(s, out var info))
				throw new ArgumentException();
			return info.Index;
		}
	}
}
