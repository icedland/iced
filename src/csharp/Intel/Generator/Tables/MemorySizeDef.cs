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
using Generator.Enums;

namespace Generator.Tables {
	[Enum("BroadcastToKind")]
	enum BroadcastToKind {
		None,
		b1to2,
		b1to4,
		b1to8,
		b1to16,
	}

	[Enum("MemoryKeywords", "FastMemoryKeywords")]
	enum FastMemoryKeywords {
		None,
		byte_ptr,
		dword_bcst,
		dword_ptr,
		fpuenv14_ptr,
		fpuenv28_ptr,
		fpustate108_ptr,
		fpustate94_ptr,
		fword_ptr,
		oword_ptr,
		qword_bcst,
		qword_ptr,
		tbyte_ptr,
		word_ptr,
		xmmword_ptr,
		ymmword_ptr,
		zmmword_ptr,
		mem384_ptr,
	}

	[Enum("MemoryKeywords", "IntelMemoryKeywords")]
	enum IntelMemoryKeywords {
		None,
		byte_ptr,
		dword_ptr,
		fpuenv14_ptr,
		fpuenv28_ptr,
		fpustate108_ptr,
		fpustate94_ptr,
		fword_ptr,
		qword_ptr,
		tbyte_ptr,
		word_ptr,
		xmmword_ptr,
		ymmword_ptr,
		zmmword_ptr,
		mem384_ptr,
	}

	[Enum("MemoryKeywords", "MasmMemoryKeywords")]
	enum MasmMemoryKeywords {
		None,
		byte_ptr,
		dword_bcst,
		dword_ptr,
		fpuenv14_ptr,
		fpuenv28_ptr,
		fpustate108_ptr,
		fpustate94_ptr,
		fword_ptr,
		oword_ptr,
		qword_bcst,
		qword_ptr,
		tbyte_ptr,
		word_ptr,
		xmmword_ptr,
		ymmword_ptr,
		zmmword_ptr,
		mem384_ptr,
	}

	[Enum("MemoryKeywords", "NasmMemoryKeywords")]
	enum NasmMemoryKeywords {
		None,
		@byte,
		dword,
		far,
		fpuenv14,
		fpuenv28,
		fpustate108,
		fpustate94,
		oword,
		qword,
		tword,
		word,
		yword,
		zword,
		mem384,
	}

	[Flags]
	enum MemorySizeDefFlags : uint {
		None				= 0,
		Signed				= 0x00000001,
		Broadcast			= 0x00000002,
	}

	sealed class MemorySizeDef {
		public readonly EnumValue MemorySize;
		public readonly uint Size;
		public readonly EnumValue ElementType;
		public readonly uint ElementSize;
		public readonly MemorySizeDefFlags Flags;
		public bool IsSigned => (Flags & MemorySizeDefFlags.Signed) != 0;
		public bool IsBroadcast => (Flags & MemorySizeDefFlags.Broadcast) != 0;
		public readonly EnumValue BroadcastToKind;
		public readonly EnumValue Fast;
		public readonly EnumValue Intel;
		public readonly EnumValue Masm;
		public readonly EnumValue Nasm;

		public MemorySizeDef(EnumValue memorySize, uint size, EnumValue elementType, uint elementSize, MemorySizeDefFlags flags,
			EnumValue broadcastToKind, EnumValue fast, EnumValue intel, EnumValue masm, EnumValue nasm) {
			MemorySize = memorySize;
			Size = size;
			ElementType = elementType;
			ElementSize = elementSize;
			Flags = flags;
			BroadcastToKind = broadcastToKind;
			Fast = fast;
			Intel = intel;
			Masm = masm;
			Nasm = nasm;
		}
	}
}
