// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Formatters {
	static class StringsTableSerializerUtils {
		public static int GetByteCount(StringsTable.Info[] sortedInfos) {
			int count = 0;
			foreach (var info in sortedInfos)
				count += 1 + info.String.Length;
			return count;
		}

		public static void SerializeTable(ByteTableWriter writer, StringsTable.Info[] sortedInfos, int extraPadding = -1, string fastStrMsg = "") {
			foreach (var info in sortedInfos) {
				var s = info.String;
				if (s.Length > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)s.Length);
				foreach (var c in s) {
					if ((ushort)c > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)c);
				}
				writer.WriteCommentLine(s);
			}

			// Include comment even if padding is 0
			if (extraPadding >= 0) {
				writer.WriteCommentLine(fastStrMsg);
				if (extraPadding > 0) {
					for (int i = 0; i < extraPadding; i++)
						writer.WriteByte(0);
					writer.WriteLine();
				}
				else
					writer.WriteCommentLine("No padding needed");
			}
		}
	}
}
