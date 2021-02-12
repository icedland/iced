// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if ENCODER
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.EncoderTests {
	sealed class CodeWriterImpl : CodeWriter {
		readonly List<byte> bytes = new List<byte>();
		public override void WriteByte(byte value) => bytes.Add(value);
		public byte[] ToArray() => bytes.ToArray();
	}
}
#endif
