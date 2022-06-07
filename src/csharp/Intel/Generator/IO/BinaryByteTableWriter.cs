// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;

namespace Generator.IO {
	sealed class BinaryByteTableWriter : ByteTableWriter, IDisposable {
		readonly BinaryWriter writer;
		public BinaryByteTableWriter(string filename) {
			Directory.CreateDirectory(Path.GetDirectoryName(filename) ?? throw new InvalidOperationException());
			this.writer = new BinaryWriter(File.Create(filename));
		}
		public BinaryByteTableWriter(BinaryWriter writer) => this.writer = writer;
		public override void WriteLine() {}
		public override void WriteCommentLine(string s) {}
		public override IDisposable? Indent() => null;
		public override void WriteByte(byte value) => writer.Write(value);
		public void Dispose() => writer.Dispose();
		public void WriteUInt16(ushort value) {
			WriteByte((byte)value);
			WriteByte((byte)(value >> 8));
		}
		public void WriteUInt32(uint value) {
			WriteByte((byte)value);
			WriteByte((byte)(value >> 8));
			WriteByte((byte)(value >> 16));
			WriteByte((byte)(value >> 24));
		}
	}
}
