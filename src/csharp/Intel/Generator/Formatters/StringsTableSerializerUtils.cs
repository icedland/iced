// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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

		public static void SerializeTable(FileWriter writer, StringsTable.Info[] sortedInfos) {
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
		}
	}
}
