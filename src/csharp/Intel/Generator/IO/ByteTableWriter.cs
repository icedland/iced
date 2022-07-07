// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.IO {
	abstract class ByteTableWriter {
		public abstract void WriteLine();
		public abstract void WriteCommentLine(string s);
		public abstract void WriteByte(byte value);
		public abstract IDisposable? Indent();
		public void WriteCompressedUInt32(uint value) {
			for (;;) {
				uint v = value;
				if (v < 0x80)
					WriteByte((byte)value);
				else
					WriteByte((byte)(value | 0x80));
				value >>= 7;
				if (value == 0)
					break;
			}
		}
	}
}
