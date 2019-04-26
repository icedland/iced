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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.MasmFormatterInternal {
	static class MemorySizes {
		static readonly string[] byte_ptr = new string[] { "byte", "ptr" };
		static readonly string[] word_ptr = new string[] { "word", "ptr" };
		internal static readonly string[] dword_ptr = new string[] { "dword", "ptr" };
		internal static readonly string[] qword_ptr = new string[] { "qword", "ptr" };
		internal static readonly string[] mmword_ptr = new string[] { "mmword", "ptr" };
		internal static readonly string[] xmmword_ptr = new string[] { "xmmword", "ptr" };
		static readonly string[] ymmword_ptr = new string[] { "ymmword", "ptr" };
		static readonly string[] zmmword_ptr = new string[] { "zmmword", "ptr" };
		static readonly string[] fword_ptr = new string[] { "fword", "ptr" };
		static readonly string[] tbyte_ptr = new string[] { "tbyte", "ptr" };
		internal static readonly string[] oword_ptr = new string[] { "oword", "ptr" };
		static readonly string[] dword_bcst = new string[] { "dword", "bcst" };
		static readonly string[] qword_bcst = new string[] { "qword", "bcst" };

		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly bool isBroadcast;
			public readonly int size;
			public readonly string[] names;
			public Info(MemorySize memorySize, bool isBroadcast, int size, string[] names) {
				this.memorySize = memorySize;
				this.isBroadcast = isBroadcast;
				this.size = size;
				this.names = names;
			}
		}

		public static readonly Info[] AllMemorySizes = new Info[DecoderConstants.NumberOfMemorySizes] {
			new Info(MemorySize.Unknown, false, 0, Array2.Empty<string>()),
			new Info(MemorySize.UInt8, false, 1, byte_ptr),
			new Info(MemorySize.UInt16, false, 2, word_ptr),
			new Info(MemorySize.UInt32, false, 4, dword_ptr),
			new Info(MemorySize.UInt52, false, 8, qword_ptr),
			new Info(MemorySize.UInt64, false, 8, qword_ptr),
			new Info(MemorySize.UInt128, false, 16, xmmword_ptr),
			new Info(MemorySize.UInt256, false, 32, ymmword_ptr),
			new Info(MemorySize.UInt512, false, 64, zmmword_ptr),
			new Info(MemorySize.Int8, false, 1, byte_ptr),
			new Info(MemorySize.Int16, false, 2, word_ptr),
			new Info(MemorySize.Int32, false, 4, dword_ptr),
			new Info(MemorySize.Int64, false, 8, qword_ptr),
			new Info(MemorySize.Int128, false, 16, xmmword_ptr),
			new Info(MemorySize.Int256, false, 32, ymmword_ptr),
			new Info(MemorySize.Int512, false, 64, zmmword_ptr),
			new Info(MemorySize.SegPtr16, false, 4, dword_ptr),
			new Info(MemorySize.SegPtr32, false, 6, fword_ptr),
			new Info(MemorySize.SegPtr64, false, 10, tbyte_ptr),
			new Info(MemorySize.WordOffset, false, 2, word_ptr),
			new Info(MemorySize.DwordOffset, false, 4, dword_ptr),
			new Info(MemorySize.QwordOffset, false, 8, qword_ptr),
			new Info(MemorySize.Bound16_WordWord, false, 4, dword_ptr),
			new Info(MemorySize.Bound32_DwordDword, false, 8, qword_ptr),
			new Info(MemorySize.Bnd32, false, 8, qword_ptr),
			new Info(MemorySize.Bnd64, false, 16, oword_ptr),
			new Info(MemorySize.Fword5, false, 5, fword_ptr),
			new Info(MemorySize.Fword6, false, 6, fword_ptr),
			new Info(MemorySize.Fword10, false, 10, fword_ptr),
			new Info(MemorySize.Float16, false, 2, word_ptr),
			new Info(MemorySize.Float32, false, 4, dword_ptr),
			new Info(MemorySize.Float64, false, 8, qword_ptr),
			new Info(MemorySize.Float80, false, 10, tbyte_ptr),
			new Info(MemorySize.Float128, false, 16, xmmword_ptr),
			new Info(MemorySize.BFloat16, false, 2, word_ptr),
			new Info(MemorySize.FpuEnv14, false, 14, "fpuenv14 ptr".Split(' ')),
			new Info(MemorySize.FpuEnv28, false, 28, "fpuenv28 ptr".Split(' ')),
			new Info(MemorySize.FpuState94, false, 94, "fpustate94 ptr".Split(' ')),
			new Info(MemorySize.FpuState108, false, 108, "fpustate108 ptr".Split(' ')),
			new Info(MemorySize.Fxsave_512Byte, false, 512, Array2.Empty<string>()),
			new Info(MemorySize.Fxsave64_512Byte, false, 512, Array2.Empty<string>()),
			new Info(MemorySize.Xsave, false, 0, Array2.Empty<string>()),
			new Info(MemorySize.Xsave64, false, 0, Array2.Empty<string>()),
			new Info(MemorySize.Bcd, false, 10, tbyte_ptr),
			new Info(MemorySize.Packed16_UInt8, false, 2, word_ptr),
			new Info(MemorySize.Packed16_Int8, false, 2, word_ptr),
			new Info(MemorySize.Packed32_UInt8, false, 4, dword_ptr),
			new Info(MemorySize.Packed32_Int8, false, 4, dword_ptr),
			new Info(MemorySize.Packed32_UInt16, false, 4, dword_ptr),
			new Info(MemorySize.Packed32_Int16, false, 4, dword_ptr),
			new Info(MemorySize.Packed32_BFloat16, false, 4, dword_ptr),
			new Info(MemorySize.Packed64_UInt8, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_Int8, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_UInt16, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_Int16, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_UInt32, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_Int32, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_Float16, false, 8, qword_ptr),
			new Info(MemorySize.Packed64_Float32, false, 8, qword_ptr),
			new Info(MemorySize.Packed128_UInt8, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Int8, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_UInt16, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Int16, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_UInt32, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Int32, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_UInt52, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_UInt64, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Int64, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Float16, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Float32, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_Float64, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed128_2xBFloat16, false, 16, xmmword_ptr),
			new Info(MemorySize.Packed256_UInt8, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Int8, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_UInt16, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Int16, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_UInt32, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Int32, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_UInt52, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_UInt64, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Int64, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_UInt128, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Int128, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Float16, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Float32, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Float64, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_Float128, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed256_2xBFloat16, false, 32, ymmword_ptr),
			new Info(MemorySize.Packed512_UInt8, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Int8, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_UInt16, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Int16, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_UInt32, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Int32, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_UInt52, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_UInt64, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Int64, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_UInt128, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Float32, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_Float64, false, 64, zmmword_ptr),
			new Info(MemorySize.Packed512_2xBFloat16, false, 64, zmmword_ptr),
			new Info(MemorySize.Broadcast64_UInt32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast64_Int32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast64_Float32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast128_UInt32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast128_Int32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast128_UInt52, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_UInt64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_Int64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_Float32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast128_Float64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_UInt32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast256_Int32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast256_UInt52, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_UInt64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_Int64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_Float32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast256_Float64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_UInt32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast512_Int32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast512_UInt52, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_UInt64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_Int64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_Float32, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast512_Float64, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_2xInt16, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast256_2xInt16, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast512_2xInt16, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast128_2xUInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_2xUInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_2xUInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_2xInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast256_2xInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast512_2xInt32, true, 8, qword_bcst),
			new Info(MemorySize.Broadcast128_2xBFloat16, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast256_2xBFloat16, true, 4, dword_bcst),
			new Info(MemorySize.Broadcast512_2xBFloat16, true, 4, dword_bcst),
		};
	}
}
#endif
