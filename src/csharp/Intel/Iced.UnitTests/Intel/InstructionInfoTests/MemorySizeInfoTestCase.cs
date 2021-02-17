// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	sealed class MemorySizeInfoTestCase {
		public int LineNumber;
		public MemorySize MemorySize;
		public int Size;
		public int ElementSize;
		public MemorySize ElementType;
		public int ElementCount;
		public MemorySizeFlags Flags;
	}
}
#endif
