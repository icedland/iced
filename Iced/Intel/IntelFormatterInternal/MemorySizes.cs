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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.IntelFormatterInternal {
	static class MemorySizes {
		static readonly string[] byte_ptr = new string[] { "byte", "ptr" };
		static readonly string[] word_ptr = new string[] { "word", "ptr" };
		static readonly string[] dword_ptr = new string[] { "dword", "ptr" };
		static readonly string[] qword_ptr = new string[] { "qword", "ptr" };
		static readonly string[] xmmword_ptr = new string[] { "xmmword", "ptr" };
		static readonly string[] ymmword_ptr = new string[] { "ymmword", "ptr" };
		static readonly string[] zmmword_ptr = new string[] { "zmmword", "ptr" };
		static readonly string[] fword_ptr = new string[] { "fword", "ptr" };
		static readonly string[] tbyte_ptr = new string[] { "tbyte", "ptr" };

		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly string[] names;
			public readonly string bcstTo;
			public Info(MemorySize memorySize, string[] names, string bcstTo) {
				this.memorySize = memorySize;
				this.names = names;
				this.bcstTo = bcstTo;
			}
		}
		public static readonly Info[] AllMemorySizes = new Info[DecoderConstants.NumberOfMemorySizes] {
			new Info(MemorySize.Unknown, Array2.Empty<string>(), null),
			new Info(MemorySize.UInt8, byte_ptr, null),
			new Info(MemorySize.UInt16, word_ptr, null),
			new Info(MemorySize.UInt32, dword_ptr, null),
			new Info(MemorySize.UInt52, qword_ptr, null),
			new Info(MemorySize.UInt64, qword_ptr, null),
			new Info(MemorySize.UInt128, xmmword_ptr, null),
			new Info(MemorySize.UInt256, ymmword_ptr, null),
			new Info(MemorySize.UInt512, zmmword_ptr, null),
			new Info(MemorySize.Int8, byte_ptr, null),
			new Info(MemorySize.Int16, word_ptr, null),
			new Info(MemorySize.Int32, dword_ptr, null),
			new Info(MemorySize.Int64, qword_ptr, null),
			new Info(MemorySize.Int128, xmmword_ptr, null),
			new Info(MemorySize.Int256, ymmword_ptr, null),
			new Info(MemorySize.Int512, zmmword_ptr, null),
			new Info(MemorySize.SegPtr16, dword_ptr, null),
			new Info(MemorySize.SegPtr32, fword_ptr, null),
			new Info(MemorySize.SegPtr64, tbyte_ptr, null),
			new Info(MemorySize.WordOffset, word_ptr, null),
			new Info(MemorySize.DwordOffset, dword_ptr, null),
			new Info(MemorySize.QwordOffset, qword_ptr, null),
			new Info(MemorySize.Bound16_WordWord, dword_ptr, null),
			new Info(MemorySize.Bound32_DwordDword, qword_ptr, null),
			new Info(MemorySize.Bnd32, qword_ptr, null),
			new Info(MemorySize.Bnd64, xmmword_ptr, null),
			new Info(MemorySize.Fword5, fword_ptr, null),
			new Info(MemorySize.Fword6, fword_ptr, null),
			new Info(MemorySize.Fword10, fword_ptr, null),
			new Info(MemorySize.Float16, word_ptr, null),
			new Info(MemorySize.Float32, dword_ptr, null),
			new Info(MemorySize.Float64, qword_ptr, null),
			new Info(MemorySize.Float80, tbyte_ptr, null),
			new Info(MemorySize.Float128, xmmword_ptr, null),
			new Info(MemorySize.FpuEnv14, "fpuenv14 ptr".Split(' '), null),
			new Info(MemorySize.FpuEnv28, "fpuenv28 ptr".Split(' '), null),
			new Info(MemorySize.FpuState94, "fpustate94 ptr".Split(' '), null),
			new Info(MemorySize.FpuState108, "fpustate108 ptr".Split(' '), null),
			new Info(MemorySize.Fxsave_512Byte, Array2.Empty<string>(), null),
			new Info(MemorySize.Fxsave64_512Byte, Array2.Empty<string>(), null),
			new Info(MemorySize.Xsave, Array2.Empty<string>(), null),
			new Info(MemorySize.Xsave64, Array2.Empty<string>(), null),
			new Info(MemorySize.Bcd, tbyte_ptr, null),
			new Info(MemorySize.Packed16_UInt8, word_ptr, null),
			new Info(MemorySize.Packed16_Int8, word_ptr, null),
			new Info(MemorySize.Packed32_UInt8, dword_ptr, null),
			new Info(MemorySize.Packed32_Int8, dword_ptr, null),
			new Info(MemorySize.Packed32_UInt16, dword_ptr, null),
			new Info(MemorySize.Packed32_Int16, dword_ptr, null),
			new Info(MemorySize.Packed64_UInt8, qword_ptr, null),
			new Info(MemorySize.Packed64_Int8, qword_ptr, null),
			new Info(MemorySize.Packed64_UInt16, qword_ptr, null),
			new Info(MemorySize.Packed64_Int16, qword_ptr, null),
			new Info(MemorySize.Packed64_UInt32, qword_ptr, null),
			new Info(MemorySize.Packed64_Int32, qword_ptr, null),
			new Info(MemorySize.Packed64_Float16, qword_ptr, null),
			new Info(MemorySize.Packed64_Float32, qword_ptr, null),
			new Info(MemorySize.Packed128_UInt8, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Int8, xmmword_ptr, null),
			new Info(MemorySize.Packed128_UInt16, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Int16, xmmword_ptr, null),
			new Info(MemorySize.Packed128_UInt32, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Int32, xmmword_ptr, null),
			new Info(MemorySize.Packed128_UInt52, xmmword_ptr, null),
			new Info(MemorySize.Packed128_UInt64, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Int64, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Float16, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Float32, xmmword_ptr, null),
			new Info(MemorySize.Packed128_Float64, xmmword_ptr, null),
			new Info(MemorySize.Packed256_UInt8, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Int8, ymmword_ptr, null),
			new Info(MemorySize.Packed256_UInt16, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Int16, ymmword_ptr, null),
			new Info(MemorySize.Packed256_UInt32, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Int32, ymmword_ptr, null),
			new Info(MemorySize.Packed256_UInt52, ymmword_ptr, null),
			new Info(MemorySize.Packed256_UInt64, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Int64, ymmword_ptr, null),
			new Info(MemorySize.Packed256_UInt128, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Int128, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Float16, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Float32, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Float64, ymmword_ptr, null),
			new Info(MemorySize.Packed256_Float128, ymmword_ptr, null),
			new Info(MemorySize.Packed512_UInt8, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Int8, zmmword_ptr, null),
			new Info(MemorySize.Packed512_UInt16, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Int16, zmmword_ptr, null),
			new Info(MemorySize.Packed512_UInt32, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Int32, zmmword_ptr, null),
			new Info(MemorySize.Packed512_UInt52, zmmword_ptr, null),
			new Info(MemorySize.Packed512_UInt64, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Int64, zmmword_ptr, null),
			new Info(MemorySize.Packed512_UInt128, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Float32, zmmword_ptr, null),
			new Info(MemorySize.Packed512_Float64, zmmword_ptr, null),
			new Info(MemorySize.Broadcast64_UInt32, dword_ptr, "1to2"),
			new Info(MemorySize.Broadcast64_Int32, dword_ptr, "1to2"),
			new Info(MemorySize.Broadcast64_Float32, dword_ptr, "1to2"),
			new Info(MemorySize.Broadcast128_UInt32, dword_ptr, "1to4"),
			new Info(MemorySize.Broadcast128_Int32, dword_ptr, "1to4"),
			new Info(MemorySize.Broadcast128_UInt52, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast128_UInt64, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast128_Int64, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast128_Float32, dword_ptr, "1to4"),
			new Info(MemorySize.Broadcast128_Float64, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast256_UInt32, dword_ptr, "1to8"),
			new Info(MemorySize.Broadcast256_Int32, dword_ptr, "1to8"),
			new Info(MemorySize.Broadcast256_UInt52, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast256_UInt64, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast256_Int64, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast256_Float32, dword_ptr, "1to8"),
			new Info(MemorySize.Broadcast256_Float64, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast512_UInt32, dword_ptr, "1to16"),
			new Info(MemorySize.Broadcast512_Int32, dword_ptr, "1to16"),
			new Info(MemorySize.Broadcast512_UInt52, qword_ptr, "1to8"),
			new Info(MemorySize.Broadcast512_UInt64, qword_ptr, "1to8"),
			new Info(MemorySize.Broadcast512_Int64, qword_ptr, "1to8"),
			new Info(MemorySize.Broadcast512_Float32, dword_ptr, "1to16"),
			new Info(MemorySize.Broadcast512_Float64, qword_ptr, "1to8"),
			new Info(MemorySize.Broadcast128_2xUInt32, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast256_2xUInt32, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast512_2xUInt32, qword_ptr, "1to8"),
			new Info(MemorySize.Broadcast128_2xInt32, qword_ptr, "1to2"),
			new Info(MemorySize.Broadcast256_2xInt32, qword_ptr, "1to4"),
			new Info(MemorySize.Broadcast512_2xInt32, qword_ptr, "1to8"),
		};
	}
}
#endif
