// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Generator.Formatters {
	sealed class StringsTable {
		readonly Dictionary<string, Info> strings;
		readonly List<(uint code, string s, bool optimize)> addedStrings;
		Info[]? sortedInfos;
		bool isFrozen;

		public bool IsFrozen => isFrozen;
		public Info[] Infos => sortedInfos ?? throw new InvalidOperationException();

		[DebuggerDisplay("{" + nameof(Count) + "} {" + nameof(String) + "}")]
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

		public StringsTable() {
			strings = new Dictionary<string, Info>(StringComparer.Ordinal);
			addedStrings = new List<(uint code, string s, bool optimize)>();
		}

		public void Freeze() {
			if (isFrozen)
				throw new InvalidOperationException();
			isFrozen = true;

			// If the string can be optimized (i.e., it's the first string (== mnemonic string) and not a possible 2nd string),
			// and if it starts with 'v' and there already exists a string without the 'v', then we can set the 'v' flag and
			// just store the non-v string.
			void AddString(uint code, string s) {
				if (!strings.TryGetValue(s, out var info))
					strings.Add(s, info = new Info(s, code));
				info.Count++;
			}
			bool CanOptimize(string s, bool optimize) => optimize && s.StartsWith("v", StringComparison.Ordinal);
			foreach (var (code, s, optimize) in addedStrings) {
				if (CanOptimize(s, optimize))
					continue;
				AddString(code, s);
			}
			foreach (var (code, tmp, optimize) in addedStrings) {
				var s = tmp;
				if (!CanOptimize(s, optimize))
					continue;
				Debug.Assert(s.StartsWith("v", StringComparison.Ordinal));
				var optimizedString = s[1..];
				// If the smaller string doesn't exist, there's no need to store it. It will just lead to an extra allocation at runtime.
				if (strings.ContainsKey(optimizedString))
					AddString(code, optimizedString);
				else
					AddString(code, s);
			}

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
			public static readonly InfoSorterOptimizeSize Instance = new();
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
			public static readonly InfoSorterMinimizeDiff Instance = new();
			InfoSorterMinimizeDiff() { }
			public int Compare([AllowNull] Info x, [AllowNull] Info y) {
				int c = x!.ApproxCode.CompareTo(y!.ApproxCode);
				if (c != 0)
					return c;
				return StringComparer.Ordinal.Compare(x.String, y.String);
			}
		}

		public void Add(uint code, string s, bool optimize) {
			if (isFrozen)
				throw new InvalidOperationException();
			addedStrings.Add((code, s, optimize));
		}

		public uint GetIndex(string s, bool optimize, out bool hasVPrefix) {
			if (!isFrozen)
				throw new InvalidOperationException();

			if (strings.TryGetValue(s, out var info)) {
				hasVPrefix = false;
				return info.Index;
			}

			if (optimize) {
				if (s.StartsWith("v", StringComparison.Ordinal)) {
					var optimizedString = s[1..];
					if (strings.TryGetValue(optimizedString, out info)) {
						hasVPrefix = true;
						return info.Index;
					}
				}
			}

			throw new ArgumentException();
		}
	}
}
