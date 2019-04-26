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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
namespace Iced.Intel.NasmFormatterInternal {
	static class MemorySizes {
		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly int size;
			public readonly string name;
			public readonly string bcstTo;
			public Info(MemorySize memorySize, int size, string name, string bcstTo) {
				this.memorySize = memorySize;
				this.size = size;
				this.name = name;
				this.bcstTo = bcstTo;
			}
		}
		public static readonly Info[] AllMemorySizes = new Info[DecoderConstants.NumberOfMemorySizes] {
			new Info(MemorySize.Unknown, 0, null, null),
			new Info(MemorySize.UInt8, 1, "byte", null),
			new Info(MemorySize.UInt16, 2, "word", null),
			new Info(MemorySize.UInt32, 4, "dword", null),
			new Info(MemorySize.UInt52, 8, "qword", null),
			new Info(MemorySize.UInt64, 8, "qword", null),
			new Info(MemorySize.UInt128, 16, "oword", null),
			new Info(MemorySize.UInt256, 32, "yword", null),
			new Info(MemorySize.UInt512, 64, "zword", null),
			new Info(MemorySize.Int8, 1, "byte", null),
			new Info(MemorySize.Int16, 2, "word", null),
			new Info(MemorySize.Int32, 4, "dword", null),
			new Info(MemorySize.Int64, 8, "qword", null),
			new Info(MemorySize.Int128, 16, "oword", null),
			new Info(MemorySize.Int256, 32, "yword", null),
			new Info(MemorySize.Int512, 64, "zword", null),
			new Info(MemorySize.SegPtr16, 4, "far", null),
			new Info(MemorySize.SegPtr32, 6, "far", null),
			new Info(MemorySize.SegPtr64, 10, "far", null),
			new Info(MemorySize.WordOffset, 2, "word", null),
			new Info(MemorySize.DwordOffset, 4, "dword", null),
			new Info(MemorySize.QwordOffset, 8, "qword", null),
			new Info(MemorySize.Bound16_WordWord, 4, null, null),
			new Info(MemorySize.Bound32_DwordDword, 8, null, null),
			new Info(MemorySize.Bnd32, 8, "qword", null),
			new Info(MemorySize.Bnd64, 16, "oword", null),
			new Info(MemorySize.Fword5, 5, null, null),
			new Info(MemorySize.Fword6, 6, null, null),
			new Info(MemorySize.Fword10, 10, null, null),
			new Info(MemorySize.Float16, 2, "word", null),
			new Info(MemorySize.Float32, 4, "dword", null),
			new Info(MemorySize.Float64, 8, "qword", null),
			new Info(MemorySize.Float80, 10, "tword", null),
			new Info(MemorySize.Float128, 16, "oword", null),
			new Info(MemorySize.BFloat16, 2, "word", null),
			new Info(MemorySize.FpuEnv14, 14, "fpuenv14", null),
			new Info(MemorySize.FpuEnv28, 28, "fpuenv28", null),
			new Info(MemorySize.FpuState94, 94, "fpustate94", null),
			new Info(MemorySize.FpuState108, 108, "fpustate108", null),
			new Info(MemorySize.Fxsave_512Byte, 512, null, null),
			new Info(MemorySize.Fxsave64_512Byte, 512, null, null),
			new Info(MemorySize.Xsave, 0, null, null),
			new Info(MemorySize.Xsave64, 0, null, null),
			new Info(MemorySize.Bcd, 10, "tword", null),
			new Info(MemorySize.Packed16_UInt8, 2, "word", null),
			new Info(MemorySize.Packed16_Int8, 2, "word", null),
			new Info(MemorySize.Packed32_UInt8, 4, "dword", null),
			new Info(MemorySize.Packed32_Int8, 4, "dword", null),
			new Info(MemorySize.Packed32_UInt16, 4, "dword", null),
			new Info(MemorySize.Packed32_Int16, 4, "dword", null),
			new Info(MemorySize.Packed32_BFloat16, 4, "dword", null),
			new Info(MemorySize.Packed64_UInt8, 8, "qword", null),
			new Info(MemorySize.Packed64_Int8, 8, "qword", null),
			new Info(MemorySize.Packed64_UInt16, 8, "qword", null),
			new Info(MemorySize.Packed64_Int16, 8, "qword", null),
			new Info(MemorySize.Packed64_UInt32, 8, "qword", null),
			new Info(MemorySize.Packed64_Int32, 8, "qword", null),
			new Info(MemorySize.Packed64_Float16, 8, "qword", null),
			new Info(MemorySize.Packed64_Float32, 8, "qword", null),
			new Info(MemorySize.Packed128_UInt8, 16, "oword", null),
			new Info(MemorySize.Packed128_Int8, 16, "oword", null),
			new Info(MemorySize.Packed128_UInt16, 16, "oword", null),
			new Info(MemorySize.Packed128_Int16, 16, "oword", null),
			new Info(MemorySize.Packed128_UInt32, 16, "oword", null),
			new Info(MemorySize.Packed128_Int32, 16, "oword", null),
			new Info(MemorySize.Packed128_UInt52, 16, "oword", null),
			new Info(MemorySize.Packed128_UInt64, 16, "oword", null),
			new Info(MemorySize.Packed128_Int64, 16, "oword", null),
			new Info(MemorySize.Packed128_Float16, 16, "oword", null),
			new Info(MemorySize.Packed128_Float32, 16, "oword", null),
			new Info(MemorySize.Packed128_Float64, 16, "oword", null),
			new Info(MemorySize.Packed128_2xBFloat16, 16, "oword", null),
			new Info(MemorySize.Packed256_UInt8, 32, "yword", null),
			new Info(MemorySize.Packed256_Int8, 32, "yword", null),
			new Info(MemorySize.Packed256_UInt16, 32, "yword", null),
			new Info(MemorySize.Packed256_Int16, 32, "yword", null),
			new Info(MemorySize.Packed256_UInt32, 32, "yword", null),
			new Info(MemorySize.Packed256_Int32, 32, "yword", null),
			new Info(MemorySize.Packed256_UInt52, 32, "yword", null),
			new Info(MemorySize.Packed256_UInt64, 32, "yword", null),
			new Info(MemorySize.Packed256_Int64, 32, "yword", null),
			new Info(MemorySize.Packed256_UInt128, 32, "yword", null),
			new Info(MemorySize.Packed256_Int128, 32, "yword", null),
			new Info(MemorySize.Packed256_Float16, 32, "yword", null),
			new Info(MemorySize.Packed256_Float32, 32, "yword", null),
			new Info(MemorySize.Packed256_Float64, 32, "yword", null),
			new Info(MemorySize.Packed256_Float128, 32, "yword", null),
			new Info(MemorySize.Packed256_2xBFloat16, 32, "yword", null),
			new Info(MemorySize.Packed512_UInt8, 64, "zword", null),
			new Info(MemorySize.Packed512_Int8, 64, "zword", null),
			new Info(MemorySize.Packed512_UInt16, 64, "zword", null),
			new Info(MemorySize.Packed512_Int16, 64, "zword", null),
			new Info(MemorySize.Packed512_UInt32, 64, "zword", null),
			new Info(MemorySize.Packed512_Int32, 64, "zword", null),
			new Info(MemorySize.Packed512_UInt52, 64, "zword", null),
			new Info(MemorySize.Packed512_UInt64, 64, "zword", null),
			new Info(MemorySize.Packed512_Int64, 64, "zword", null),
			new Info(MemorySize.Packed512_UInt128, 64, "zword", null),
			new Info(MemorySize.Packed512_Float32, 64, "zword", null),
			new Info(MemorySize.Packed512_Float64, 64, "zword", null),
			new Info(MemorySize.Packed512_2xBFloat16, 64, "zword", null),
			new Info(MemorySize.Broadcast64_UInt32, 4, "dword", "1to2"),
			new Info(MemorySize.Broadcast64_Int32, 4, "dword", "1to2"),
			new Info(MemorySize.Broadcast64_Float32, 4, "dword", "1to2"),
			new Info(MemorySize.Broadcast128_UInt32, 4, "dword", "1to4"),
			new Info(MemorySize.Broadcast128_Int32, 4, "dword", "1to4"),
			new Info(MemorySize.Broadcast128_UInt52, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast128_UInt64, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast128_Int64, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast128_Float32, 4, "dword", "1to4"),
			new Info(MemorySize.Broadcast128_Float64, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast256_UInt32, 4, "dword", "1to8"),
			new Info(MemorySize.Broadcast256_Int32, 4, "dword", "1to8"),
			new Info(MemorySize.Broadcast256_UInt52, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast256_UInt64, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast256_Int64, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast256_Float32, 4, "dword", "1to8"),
			new Info(MemorySize.Broadcast256_Float64, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast512_UInt32, 4, "dword", "1to16"),
			new Info(MemorySize.Broadcast512_Int32, 4, "dword", "1to16"),
			new Info(MemorySize.Broadcast512_UInt52, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast512_UInt64, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast512_Int64, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast512_Float32, 4, "dword", "1to16"),
			new Info(MemorySize.Broadcast512_Float64, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast128_2xInt16, 4, "dword", "1to4"),
			new Info(MemorySize.Broadcast256_2xInt16, 4, "dword", "1to8"),
			new Info(MemorySize.Broadcast512_2xInt16, 4, "dword", "1to16"),
			new Info(MemorySize.Broadcast128_2xUInt32, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast256_2xUInt32, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast512_2xUInt32, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast128_2xInt32, 8, "qword", "1to2"),
			new Info(MemorySize.Broadcast256_2xInt32, 8, "qword", "1to4"),
			new Info(MemorySize.Broadcast512_2xInt32, 8, "qword", "1to8"),
			new Info(MemorySize.Broadcast128_2xBFloat16, 4, "dword", "1to4"),
			new Info(MemorySize.Broadcast256_2xBFloat16, 4, "dword", "1to8"),
			new Info(MemorySize.Broadcast512_2xBFloat16, 4, "dword", "1to16"),
		};
	}
}
#endif
