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
	sealed class MemorySizeInfo {
		public EnumValue MemorySize { get; }
		public int Size { get; }
		public int ElementSize { get; }
		public EnumValue ElementType { get; }
		public bool IsSigned { get; }
		public bool IsBroadcast { get; }

		public MemorySizeInfo(EnumValue memorySize, int size, int elementSize, EnumValue elementType, bool isSigned, bool isBroadcast) {
			MemorySize = memorySize;
			Size = size;
			ElementSize = elementSize;
			ElementType = elementType;
			IsSigned = isSigned;
			IsBroadcast = isBroadcast;
		}
	}

	static class MemorySizeInfoTable {
		public static MemorySizeInfo[] Data = CreateData();

		static MemorySizeInfo[] CreateData() {
			var memSize = MemorySizeEnum.Instance;
			var result = new MemorySizeInfo[MemorySizeEnum.NumValues] {
				new MemorySizeInfo(memSize["Unknown"], 0, 0, memSize["Unknown"], false, false),
				new MemorySizeInfo(memSize["UInt8"], 1, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["UInt16"], 2, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["UInt32"], 4, 4, memSize["UInt32"], false, false),
				new MemorySizeInfo(memSize["UInt52"], 8, 8, memSize["UInt52"], false, false),
				new MemorySizeInfo(memSize["UInt64"], 8, 8, memSize["UInt64"], false, false),
				new MemorySizeInfo(memSize["UInt128"], 16, 16, memSize["UInt128"], false, false),
				new MemorySizeInfo(memSize["UInt256"], 32, 32, memSize["UInt256"], false, false),
				new MemorySizeInfo(memSize["UInt512"], 64, 64, memSize["UInt512"], false, false),
				new MemorySizeInfo(memSize["Int8"], 1, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Int16"], 2, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Int32"], 4, 4, memSize["Int32"], true, false),
				new MemorySizeInfo(memSize["Int64"], 8, 8, memSize["Int64"], true, false),
				new MemorySizeInfo(memSize["Int128"], 16, 16, memSize["Int128"], true, false),
				new MemorySizeInfo(memSize["Int256"], 32, 32, memSize["Int256"], true, false),
				new MemorySizeInfo(memSize["Int512"], 64, 64, memSize["Int512"], true, false),
				new MemorySizeInfo(memSize["SegPtr16"], 4, 4, memSize["SegPtr16"], false, false),
				new MemorySizeInfo(memSize["SegPtr32"], 6, 6, memSize["SegPtr32"], false, false),
				new MemorySizeInfo(memSize["SegPtr64"], 10, 10, memSize["SegPtr64"], false, false),
				new MemorySizeInfo(memSize["WordOffset"], 2, 2, memSize["WordOffset"], false, false),
				new MemorySizeInfo(memSize["DwordOffset"], 4, 4, memSize["DwordOffset"], false, false),
				new MemorySizeInfo(memSize["QwordOffset"], 8, 8, memSize["QwordOffset"], false, false),
				new MemorySizeInfo(memSize["Bound16_WordWord"], 4, 4, memSize["Bound16_WordWord"], false, false),
				new MemorySizeInfo(memSize["Bound32_DwordDword"], 8, 8, memSize["Bound32_DwordDword"], false, false),
				new MemorySizeInfo(memSize["Bnd32"], 8, 8, memSize["Bnd32"], false, false),
				new MemorySizeInfo(memSize["Bnd64"], 16, 16, memSize["Bnd64"], false, false),
				new MemorySizeInfo(memSize["Fword6"], 6, 6, memSize["Fword6"], false, false),
				new MemorySizeInfo(memSize["Fword10"], 10, 10, memSize["Fword10"], false, false),
				new MemorySizeInfo(memSize["Float16"], 2, 2, memSize["Float16"], true, false),
				new MemorySizeInfo(memSize["Float32"], 4, 4, memSize["Float32"], true, false),
				new MemorySizeInfo(memSize["Float64"], 8, 8, memSize["Float64"], true, false),
				new MemorySizeInfo(memSize["Float80"], 10, 10, memSize["Float80"], true, false),
				new MemorySizeInfo(memSize["Float128"], 16, 16, memSize["Float128"], true, false),
				new MemorySizeInfo(memSize["BFloat16"], 2, 2, memSize["BFloat16"], true, false),
				new MemorySizeInfo(memSize["FpuEnv14"], 14, 14, memSize["FpuEnv14"], false, false),
				new MemorySizeInfo(memSize["FpuEnv28"], 28, 28, memSize["FpuEnv28"], false, false),
				new MemorySizeInfo(memSize["FpuState94"], 94, 94, memSize["FpuState94"], false, false),
				new MemorySizeInfo(memSize["FpuState108"], 108, 108, memSize["FpuState108"], false, false),
				new MemorySizeInfo(memSize["Fxsave_512Byte"], 512, 512, memSize["Fxsave_512Byte"], false, false),
				new MemorySizeInfo(memSize["Fxsave64_512Byte"], 512, 512, memSize["Fxsave64_512Byte"], false, false),
				new MemorySizeInfo(memSize["Xsave"], 0, 0, memSize["Xsave"], false, false),
				new MemorySizeInfo(memSize["Xsave64"], 0, 0, memSize["Xsave64"], false, false),
				new MemorySizeInfo(memSize["Bcd"], 10, 10, memSize["Bcd"], true, false),
				new MemorySizeInfo(memSize["Packed16_UInt8"], 2, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed16_Int8"], 2, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed32_UInt8"], 4, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed32_Int8"], 4, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed32_UInt16"], 4, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["Packed32_Int16"], 4, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Packed32_BFloat16"], 4, 2, memSize["BFloat16"], true, false),
				new MemorySizeInfo(memSize["Packed64_UInt8"], 8, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed64_Int8"], 8, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed64_UInt16"], 8, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["Packed64_Int16"], 8, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Packed64_UInt32"], 8, 4, memSize["UInt32"], false, false),
				new MemorySizeInfo(memSize["Packed64_Int32"], 8, 4, memSize["Int32"], true, false),
				new MemorySizeInfo(memSize["Packed64_Float16"], 8, 2, memSize["Float16"], true, false),
				new MemorySizeInfo(memSize["Packed64_Float32"], 8, 4, memSize["Float32"], true, false),
				new MemorySizeInfo(memSize["Packed128_UInt8"], 16, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed128_Int8"], 16, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed128_UInt16"], 16, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["Packed128_Int16"], 16, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Packed128_UInt32"], 16, 4, memSize["UInt32"], false, false),
				new MemorySizeInfo(memSize["Packed128_Int32"], 16, 4, memSize["Int32"], true, false),
				new MemorySizeInfo(memSize["Packed128_UInt52"], 16, 8, memSize["UInt52"], false, false),
				new MemorySizeInfo(memSize["Packed128_UInt64"], 16, 8, memSize["UInt64"], false, false),
				new MemorySizeInfo(memSize["Packed128_Int64"], 16, 8, memSize["Int64"], true, false),
				new MemorySizeInfo(memSize["Packed128_Float16"], 16, 2, memSize["Float16"], true, false),
				new MemorySizeInfo(memSize["Packed128_Float32"], 16, 4, memSize["Float32"], true, false),
				new MemorySizeInfo(memSize["Packed128_Float64"], 16, 8, memSize["Float64"], true, false),
				new MemorySizeInfo(memSize["Packed128_2xBFloat16"], 16, 4, memSize["Packed32_BFloat16"], true, false),
				new MemorySizeInfo(memSize["Packed256_UInt8"], 32, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed256_Int8"], 32, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed256_UInt16"], 32, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["Packed256_Int16"], 32, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Packed256_UInt32"], 32, 4, memSize["UInt32"], false, false),
				new MemorySizeInfo(memSize["Packed256_Int32"], 32, 4, memSize["Int32"], true, false),
				new MemorySizeInfo(memSize["Packed256_UInt52"], 32, 8, memSize["UInt52"], false, false),
				new MemorySizeInfo(memSize["Packed256_UInt64"], 32, 8, memSize["UInt64"], false, false),
				new MemorySizeInfo(memSize["Packed256_Int64"], 32, 8, memSize["Int64"], true, false),
				new MemorySizeInfo(memSize["Packed256_UInt128"], 32, 16, memSize["UInt128"], false, false),
				new MemorySizeInfo(memSize["Packed256_Int128"], 32, 16, memSize["Int128"], true, false),
				new MemorySizeInfo(memSize["Packed256_Float16"], 32, 2, memSize["Float16"], true, false),
				new MemorySizeInfo(memSize["Packed256_Float32"], 32, 4, memSize["Float32"], true, false),
				new MemorySizeInfo(memSize["Packed256_Float64"], 32, 8, memSize["Float64"], true, false),
				new MemorySizeInfo(memSize["Packed256_Float128"], 32, 16, memSize["Float128"], true, false),
				new MemorySizeInfo(memSize["Packed256_2xBFloat16"], 32, 4, memSize["Packed32_BFloat16"], true, false),
				new MemorySizeInfo(memSize["Packed512_UInt8"], 64, 1, memSize["UInt8"], false, false),
				new MemorySizeInfo(memSize["Packed512_Int8"], 64, 1, memSize["Int8"], true, false),
				new MemorySizeInfo(memSize["Packed512_UInt16"], 64, 2, memSize["UInt16"], false, false),
				new MemorySizeInfo(memSize["Packed512_Int16"], 64, 2, memSize["Int16"], true, false),
				new MemorySizeInfo(memSize["Packed512_UInt32"], 64, 4, memSize["UInt32"], false, false),
				new MemorySizeInfo(memSize["Packed512_Int32"], 64, 4, memSize["Int32"], true, false),
				new MemorySizeInfo(memSize["Packed512_UInt52"], 64, 8, memSize["UInt52"], false, false),
				new MemorySizeInfo(memSize["Packed512_UInt64"], 64, 8, memSize["UInt64"], false, false),
				new MemorySizeInfo(memSize["Packed512_Int64"], 64, 8, memSize["Int64"], true, false),
				new MemorySizeInfo(memSize["Packed512_UInt128"], 64, 16, memSize["UInt128"], false, false),
				new MemorySizeInfo(memSize["Packed512_Float32"], 64, 4, memSize["Float32"], true, false),
				new MemorySizeInfo(memSize["Packed512_Float64"], 64, 8, memSize["Float64"], true, false),
				new MemorySizeInfo(memSize["Packed512_2xBFloat16"], 64, 4, memSize["Packed32_BFloat16"], true, false),
				new MemorySizeInfo(memSize["Broadcast64_UInt32"], 4, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast64_Int32"], 4, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast64_Float32"], 4, 4, memSize["Float32"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_UInt32"], 4, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast128_Int32"], 4, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_UInt52"], 8, 8, memSize["UInt52"], false, true),
				new MemorySizeInfo(memSize["Broadcast128_UInt64"], 8, 8, memSize["UInt64"], false, true),
				new MemorySizeInfo(memSize["Broadcast128_Int64"], 8, 8, memSize["Int64"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_Float32"], 4, 4, memSize["Float32"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_Float64"], 8, 8, memSize["Float64"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_UInt32"], 4, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast256_Int32"], 4, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_UInt52"], 8, 8, memSize["UInt52"], false, true),
				new MemorySizeInfo(memSize["Broadcast256_UInt64"], 8, 8, memSize["UInt64"], false, true),
				new MemorySizeInfo(memSize["Broadcast256_Int64"], 8, 8, memSize["Int64"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_Float32"], 4, 4, memSize["Float32"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_Float64"], 8, 8, memSize["Float64"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_UInt32"], 4, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast512_Int32"], 4, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_UInt52"], 8, 8, memSize["UInt52"], false, true),
				new MemorySizeInfo(memSize["Broadcast512_UInt64"], 8, 8, memSize["UInt64"], false, true),
				new MemorySizeInfo(memSize["Broadcast512_Int64"], 8, 8, memSize["Int64"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_Float32"], 4, 4, memSize["Float32"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_Float64"], 8, 8, memSize["Float64"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_2xInt16"], 4, 2, memSize["Int16"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_2xInt16"], 4, 2, memSize["Int16"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_2xInt16"], 4, 2, memSize["Int16"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_2xUInt32"], 8, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast256_2xUInt32"], 8, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast512_2xUInt32"], 8, 4, memSize["UInt32"], false, true),
				new MemorySizeInfo(memSize["Broadcast128_2xInt32"], 8, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_2xInt32"], 8, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_2xInt32"], 8, 4, memSize["Int32"], true, true),
				new MemorySizeInfo(memSize["Broadcast128_2xBFloat16"], 4, 2, memSize["BFloat16"], true, true),
				new MemorySizeInfo(memSize["Broadcast256_2xBFloat16"], 4, 2, memSize["BFloat16"], true, true),
				new MemorySizeInfo(memSize["Broadcast512_2xBFloat16"], 4, 2, memSize["BFloat16"], true, true),
			};
			if (result.Length != memSize.Values.Length)
				throw new InvalidOperationException();
			Array.Sort(result, (a, b) => a.MemorySize.Value.CompareTo(b.MemorySize.Value));
			return result;
		}
	}
}
