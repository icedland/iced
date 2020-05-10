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
using System.Diagnostics.CodeAnalysis;
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
			// Since the first 'v' char isn't stored in the strings table, multiple Code values can share the same Info instance
			public readonly uint ApproxCode;
			public int Count;
			public uint Index;
			public Info(string s, uint approxCode) {
				String = s;
				ApproxCode = approxCode;
			}
		}

		public StringsTable() =>
			strings = new Dictionary<string, Info>(StringComparer.Ordinal);

		public void Freeze() {
			if (isFrozen)
				throw new InvalidOperationException();
			isFrozen = true;

			var infos = strings.Values.ToArray();
			const int N = 1;
			const int MIN = 1 << (7 * N);
			const int MAX = 1 << (7 * (N + 1));
			if (infos.Length < MIN || infos.Length >= MAX)
				throw new InvalidOperationException("Update constants");

			// Optimize for size. The most commonly used strings are sorted first so they can be referenced
			// by a byte index. See FileWriter.WriteCompressedUInt32(). 0-127 = 1 byte, 128-16383 = 2 bytes.
			Array.Sort(infos, 0, infos.Length, InfoSorterOptimizeSize.Instance);

			// Sort again. This time the latest added mnemonics should be added at the end of the table
			// so we get a minimal diff.
			Array.Sort(infos, MIN, infos.Length - MIN, InfoSorterMinimizeDiff.Instance);

			for (int i = 0; i < infos.Length; i++)
				infos[i].Index = (uint)i;
			sortedInfos = infos;
		}

		sealed class InfoSorterOptimizeSize : IComparer<Info> {
			public static readonly InfoSorterOptimizeSize Instance = new InfoSorterOptimizeSize();
			InfoSorterOptimizeSize() { }
			public int Compare([AllowNull] Info x, [AllowNull] Info y) {
				int c = y!.Count - x!.Count;
				if (c != 0)
					return c;
				c = x.ApproxCode.CompareTo(y.ApproxCode);
				if (c != 0)
					return c;
				return StringComparer.Ordinal.Compare(x.String, y.String);
			}
		}

		sealed class InfoSorterMinimizeDiff : IComparer<Info> {
			public static readonly InfoSorterMinimizeDiff Instance = new InfoSorterMinimizeDiff();
			InfoSorterMinimizeDiff() { }
			public int Compare([AllowNull] Info x, [AllowNull] Info y) {
				int c = x!.ApproxCode.CompareTo(y!.ApproxCode);
				if (c != 0)
					return c;
				return StringComparer.Ordinal.Compare(x.String, y.String);
			}
		}

		public void Add(uint code, string s, bool ignoreVPrefix) {
			if (isFrozen)
				throw new InvalidOperationException();
			if (ignoreVPrefix && s.StartsWith("v", StringComparison.Ordinal))
				s = s.Substring(1);
			if (!strings.TryGetValue(s, out var info))
				strings.Add(s, info = new Info(s, code));
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
