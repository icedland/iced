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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
namespace Iced.Intel.GasFormatterInternal {
	static class MemorySizes {
		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly string bcstTo;
			public Info(MemorySize memorySize, string bcstTo) {
				this.memorySize = memorySize;
				this.bcstTo = bcstTo;
			}
		}
		public static readonly Info[] AllMemorySizes = new Info[DecoderConstants.NumberOfMemorySizes] {
			new Info(MemorySize.Unknown, null),
			new Info(MemorySize.UInt8, null),
			new Info(MemorySize.UInt16, null),
			new Info(MemorySize.UInt32, null),
			new Info(MemorySize.UInt52, null),
			new Info(MemorySize.UInt64, null),
			new Info(MemorySize.UInt128, null),
			new Info(MemorySize.UInt256, null),
			new Info(MemorySize.UInt512, null),
			new Info(MemorySize.Int8, null),
			new Info(MemorySize.Int16, null),
			new Info(MemorySize.Int32, null),
			new Info(MemorySize.Int64, null),
			new Info(MemorySize.Int128, null),
			new Info(MemorySize.Int256, null),
			new Info(MemorySize.Int512, null),
			new Info(MemorySize.SegPtr16, null),
			new Info(MemorySize.SegPtr32, null),
			new Info(MemorySize.SegPtr64, null),
			new Info(MemorySize.WordOffset, null),
			new Info(MemorySize.DwordOffset, null),
			new Info(MemorySize.QwordOffset, null),
			new Info(MemorySize.Bound16_WordWord, null),
			new Info(MemorySize.Bound32_DwordDword, null),
			new Info(MemorySize.Bnd32, null),
			new Info(MemorySize.Bnd64, null),
			new Info(MemorySize.Fword5, null),
			new Info(MemorySize.Fword6, null),
			new Info(MemorySize.Fword10, null),
			new Info(MemorySize.Float16, null),
			new Info(MemorySize.Float32, null),
			new Info(MemorySize.Float64, null),
			new Info(MemorySize.Float80, null),
			new Info(MemorySize.Float128, null),
			new Info(MemorySize.FpuEnv14, null),
			new Info(MemorySize.FpuEnv28, null),
			new Info(MemorySize.FpuState94, null),
			new Info(MemorySize.FpuState108, null),
			new Info(MemorySize.Fxsave_512Byte, null),
			new Info(MemorySize.Fxsave64_512Byte, null),
			new Info(MemorySize.Xsave, null),
			new Info(MemorySize.Xsave64, null),
			new Info(MemorySize.Bcd, null),
			new Info(MemorySize.Packed16_UInt8, null),
			new Info(MemorySize.Packed16_Int8, null),
			new Info(MemorySize.Packed32_UInt8, null),
			new Info(MemorySize.Packed32_Int8, null),
			new Info(MemorySize.Packed32_UInt16, null),
			new Info(MemorySize.Packed32_Int16, null),
			new Info(MemorySize.Packed64_UInt8, null),
			new Info(MemorySize.Packed64_Int8, null),
			new Info(MemorySize.Packed64_UInt16, null),
			new Info(MemorySize.Packed64_Int16, null),
			new Info(MemorySize.Packed64_UInt32, null),
			new Info(MemorySize.Packed64_Int32, null),
			new Info(MemorySize.Packed64_Float16, null),
			new Info(MemorySize.Packed64_Float32, null),
			new Info(MemorySize.Packed128_UInt8, null),
			new Info(MemorySize.Packed128_Int8, null),
			new Info(MemorySize.Packed128_UInt16, null),
			new Info(MemorySize.Packed128_Int16, null),
			new Info(MemorySize.Packed128_UInt32, null),
			new Info(MemorySize.Packed128_Int32, null),
			new Info(MemorySize.Packed128_UInt52, null),
			new Info(MemorySize.Packed128_UInt64, null),
			new Info(MemorySize.Packed128_Int64, null),
			new Info(MemorySize.Packed128_Float16, null),
			new Info(MemorySize.Packed128_Float32, null),
			new Info(MemorySize.Packed128_Float64, null),
			new Info(MemorySize.Packed256_UInt8, null),
			new Info(MemorySize.Packed256_Int8, null),
			new Info(MemorySize.Packed256_UInt16, null),
			new Info(MemorySize.Packed256_Int16, null),
			new Info(MemorySize.Packed256_UInt32, null),
			new Info(MemorySize.Packed256_Int32, null),
			new Info(MemorySize.Packed256_UInt52, null),
			new Info(MemorySize.Packed256_UInt64, null),
			new Info(MemorySize.Packed256_Int64, null),
			new Info(MemorySize.Packed256_UInt128, null),
			new Info(MemorySize.Packed256_Int128, null),
			new Info(MemorySize.Packed256_Float16, null),
			new Info(MemorySize.Packed256_Float32, null),
			new Info(MemorySize.Packed256_Float64, null),
			new Info(MemorySize.Packed256_Float128, null),
			new Info(MemorySize.Packed512_UInt8, null),
			new Info(MemorySize.Packed512_Int8, null),
			new Info(MemorySize.Packed512_UInt16, null),
			new Info(MemorySize.Packed512_Int16, null),
			new Info(MemorySize.Packed512_UInt32, null),
			new Info(MemorySize.Packed512_Int32, null),
			new Info(MemorySize.Packed512_UInt52, null),
			new Info(MemorySize.Packed512_UInt64, null),
			new Info(MemorySize.Packed512_Int64, null),
			new Info(MemorySize.Packed512_UInt128, null),
			new Info(MemorySize.Packed512_Float32, null),
			new Info(MemorySize.Packed512_Float64, null),
			new Info(MemorySize.Broadcast64_UInt32, "1to2"),
			new Info(MemorySize.Broadcast64_Int32, "1to2"),
			new Info(MemorySize.Broadcast64_Float32, "1to2"),
			new Info(MemorySize.Broadcast128_UInt32, "1to4"),
			new Info(MemorySize.Broadcast128_Int32, "1to4"),
			new Info(MemorySize.Broadcast128_UInt52, "1to2"),
			new Info(MemorySize.Broadcast128_UInt64, "1to2"),
			new Info(MemorySize.Broadcast128_Int64, "1to2"),
			new Info(MemorySize.Broadcast128_Float32, "1to4"),
			new Info(MemorySize.Broadcast128_Float64, "1to2"),
			new Info(MemorySize.Broadcast256_UInt32, "1to8"),
			new Info(MemorySize.Broadcast256_Int32, "1to8"),
			new Info(MemorySize.Broadcast256_UInt52, "1to4"),
			new Info(MemorySize.Broadcast256_UInt64, "1to4"),
			new Info(MemorySize.Broadcast256_Int64, "1to4"),
			new Info(MemorySize.Broadcast256_Float32, "1to8"),
			new Info(MemorySize.Broadcast256_Float64, "1to4"),
			new Info(MemorySize.Broadcast512_UInt32, "1to16"),
			new Info(MemorySize.Broadcast512_Int32, "1to16"),
			new Info(MemorySize.Broadcast512_UInt52, "1to8"),
			new Info(MemorySize.Broadcast512_UInt64, "1to8"),
			new Info(MemorySize.Broadcast512_Int64, "1to8"),
			new Info(MemorySize.Broadcast512_Float32, "1to16"),
			new Info(MemorySize.Broadcast512_Float64, "1to8"),
			new Info(MemorySize.Broadcast128_2xUInt32, "1to2"),
			new Info(MemorySize.Broadcast256_2xUInt32, "1to4"),
			new Info(MemorySize.Broadcast512_2xUInt32, "1to8"),
			new Info(MemorySize.Broadcast128_2xInt32, "1to2"),
			new Info(MemorySize.Broadcast256_2xInt32, "1to4"),
			new Info(MemorySize.Broadcast512_2xInt32, "1to8"),
		};
	}
}
#endif
