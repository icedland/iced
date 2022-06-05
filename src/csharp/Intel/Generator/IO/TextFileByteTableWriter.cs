// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.IO {
	sealed class TextFileByteTableWriter : ByteTableWriter {
		readonly FileWriter writer;
		public TextFileByteTableWriter(FileWriter writer) => this.writer = writer;
		public override void WriteLine() => writer.WriteLine();
		public override void WriteCommentLine(string s) => writer.WriteCommentLine(s);
		public override void WriteByte(byte value) => writer.WriteByte(value);
		public override IDisposable Indent() => writer.Indent();
	}
}
