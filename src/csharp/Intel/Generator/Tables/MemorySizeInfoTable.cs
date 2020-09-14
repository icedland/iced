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
using System.Collections.Generic;
using System.Linq;
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

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class MemorySizeInfoTable {
		public readonly MemorySizeInfo[] Data;

		MemorySizeInfoTable(GenTypes genTypes) {
			Data = CreateData(genTypes);
			genTypes.AddObject(TypeIds.MemorySizeInfoTable, this);
		}

		static MemorySizeInfo[] CreateData(GenTypes genTypes) {
			var memSize = genTypes[TypeIds.MemorySize];
			var result = new List<MemorySizeInfo> {
				new MemorySizeInfo(memSize[nameof(MemorySize.Unknown)], 0, 0, memSize[nameof(MemorySize.Unknown)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt8)], 1, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt16)], 2, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt32)], 4, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt52)], 8, 8, memSize[nameof(MemorySize.UInt52)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt64)], 8, 8, memSize[nameof(MemorySize.UInt64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt128)], 16, 16, memSize[nameof(MemorySize.UInt128)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt256)], 32, 32, memSize[nameof(MemorySize.UInt256)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.UInt512)], 64, 64, memSize[nameof(MemorySize.UInt512)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int8)], 1, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int16)], 2, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int32)], 4, 4, memSize[nameof(MemorySize.Int32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int64)], 8, 8, memSize[nameof(MemorySize.Int64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int128)], 16, 16, memSize[nameof(MemorySize.Int128)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int256)], 32, 32, memSize[nameof(MemorySize.Int256)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Int512)], 64, 64, memSize[nameof(MemorySize.Int512)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.SegPtr16)], 4, 4, memSize[nameof(MemorySize.SegPtr16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.SegPtr32)], 6, 6, memSize[nameof(MemorySize.SegPtr32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.SegPtr64)], 10, 10, memSize[nameof(MemorySize.SegPtr64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.WordOffset)], 2, 2, memSize[nameof(MemorySize.WordOffset)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.DwordOffset)], 4, 4, memSize[nameof(MemorySize.DwordOffset)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.QwordOffset)], 8, 8, memSize[nameof(MemorySize.QwordOffset)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Bound16_WordWord)], 4, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Bound32_DwordDword)], 8, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Bnd32)], 8, 8, memSize[nameof(MemorySize.Bnd32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Bnd64)], 16, 16, memSize[nameof(MemorySize.Bnd64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Fword6)], 6, 6, memSize[nameof(MemorySize.Fword6)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Fword10)], 10, 10, memSize[nameof(MemorySize.Fword10)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Float16)], 2, 2, memSize[nameof(MemorySize.Float16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Float32)], 4, 4, memSize[nameof(MemorySize.Float32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Float64)], 8, 8, memSize[nameof(MemorySize.Float64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Float80)], 10, 10, memSize[nameof(MemorySize.Float80)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Float128)], 16, 16, memSize[nameof(MemorySize.Float128)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.BFloat16)], 2, 2, memSize[nameof(MemorySize.BFloat16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.FpuEnv14)], 14, 14, memSize[nameof(MemorySize.FpuEnv14)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.FpuEnv28)], 28, 28, memSize[nameof(MemorySize.FpuEnv28)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.FpuState94)], 94, 94, memSize[nameof(MemorySize.FpuState94)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.FpuState108)], 108, 108, memSize[nameof(MemorySize.FpuState108)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Fxsave_512Byte)], 512, 512, memSize[nameof(MemorySize.Fxsave_512Byte)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Fxsave64_512Byte)], 512, 512, memSize[nameof(MemorySize.Fxsave64_512Byte)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Xsave)], 0, 0, memSize[nameof(MemorySize.Xsave)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Xsave64)], 0, 0, memSize[nameof(MemorySize.Xsave64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Bcd)], 10, 10, memSize[nameof(MemorySize.Bcd)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Tilecfg)], 64, 64, memSize[nameof(MemorySize.Tilecfg)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Tile)], 0, 0, memSize[nameof(MemorySize.Tile)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.SegmentDescSelector)], 10, 10, memSize[nameof(MemorySize.SegmentDescSelector)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed16_UInt8)], 2, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed16_Int8)], 2, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed32_UInt8)], 4, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed32_Int8)], 4, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed32_UInt16)], 4, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed32_Int16)], 4, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed32_BFloat16)], 4, 2, memSize[nameof(MemorySize.BFloat16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_UInt8)], 8, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_Int8)], 8, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_UInt16)], 8, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_Int16)], 8, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_UInt32)], 8, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_Int32)], 8, 4, memSize[nameof(MemorySize.Int32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_Float16)], 8, 2, memSize[nameof(MemorySize.Float16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed64_Float32)], 8, 4, memSize[nameof(MemorySize.Float32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_UInt8)], 16, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Int8)], 16, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_UInt16)], 16, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Int16)], 16, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_UInt32)], 16, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Int32)], 16, 4, memSize[nameof(MemorySize.Int32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_UInt52)], 16, 8, memSize[nameof(MemorySize.UInt52)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_UInt64)], 16, 8, memSize[nameof(MemorySize.UInt64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Int64)], 16, 8, memSize[nameof(MemorySize.Int64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Float16)], 16, 2, memSize[nameof(MemorySize.Float16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Float32)], 16, 4, memSize[nameof(MemorySize.Float32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_Float64)], 16, 8, memSize[nameof(MemorySize.Float64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed128_2xBFloat16)], 16, 4, memSize[nameof(MemorySize.Packed32_BFloat16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt8)], 32, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Int8)], 32, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt16)], 32, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Int16)], 32, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt32)], 32, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Int32)], 32, 4, memSize[nameof(MemorySize.Int32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt52)], 32, 8, memSize[nameof(MemorySize.UInt52)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt64)], 32, 8, memSize[nameof(MemorySize.UInt64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Int64)], 32, 8, memSize[nameof(MemorySize.Int64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_UInt128)], 32, 16, memSize[nameof(MemorySize.UInt128)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Int128)], 32, 16, memSize[nameof(MemorySize.Int128)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Float16)], 32, 2, memSize[nameof(MemorySize.Float16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Float32)], 32, 4, memSize[nameof(MemorySize.Float32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Float64)], 32, 8, memSize[nameof(MemorySize.Float64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_Float128)], 32, 16, memSize[nameof(MemorySize.Float128)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed256_2xBFloat16)], 32, 4, memSize[nameof(MemorySize.Packed32_BFloat16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt8)], 64, 1, memSize[nameof(MemorySize.UInt8)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Int8)], 64, 1, memSize[nameof(MemorySize.Int8)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt16)], 64, 2, memSize[nameof(MemorySize.UInt16)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Int16)], 64, 2, memSize[nameof(MemorySize.Int16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt32)], 64, 4, memSize[nameof(MemorySize.UInt32)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Int32)], 64, 4, memSize[nameof(MemorySize.Int32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt52)], 64, 8, memSize[nameof(MemorySize.UInt52)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt64)], 64, 8, memSize[nameof(MemorySize.UInt64)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Int64)], 64, 8, memSize[nameof(MemorySize.Int64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_UInt128)], 64, 16, memSize[nameof(MemorySize.UInt128)], false, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Float32)], 64, 4, memSize[nameof(MemorySize.Float32)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_Float64)], 64, 8, memSize[nameof(MemorySize.Float64)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Packed512_2xBFloat16)], 64, 4, memSize[nameof(MemorySize.Packed32_BFloat16)], true, false),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast64_UInt32)], 4, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast64_Int32)], 4, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast64_Float32)], 4, 4, memSize[nameof(MemorySize.Float32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_UInt32)], 4, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_Int32)], 4, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_UInt52)], 8, 8, memSize[nameof(MemorySize.UInt52)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_UInt64)], 8, 8, memSize[nameof(MemorySize.UInt64)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_Int64)], 8, 8, memSize[nameof(MemorySize.Int64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_Float32)], 4, 4, memSize[nameof(MemorySize.Float32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_Float64)], 8, 8, memSize[nameof(MemorySize.Float64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_UInt32)], 4, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_Int32)], 4, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_UInt52)], 8, 8, memSize[nameof(MemorySize.UInt52)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_UInt64)], 8, 8, memSize[nameof(MemorySize.UInt64)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_Int64)], 8, 8, memSize[nameof(MemorySize.Int64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_Float32)], 4, 4, memSize[nameof(MemorySize.Float32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_Float64)], 8, 8, memSize[nameof(MemorySize.Float64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_UInt32)], 4, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_Int32)], 4, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_UInt52)], 8, 8, memSize[nameof(MemorySize.UInt52)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_UInt64)], 8, 8, memSize[nameof(MemorySize.UInt64)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_Int64)], 8, 8, memSize[nameof(MemorySize.Int64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_Float32)], 4, 4, memSize[nameof(MemorySize.Float32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_Float64)], 8, 8, memSize[nameof(MemorySize.Float64)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_2xInt16)], 4, 2, memSize[nameof(MemorySize.Int16)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_2xInt16)], 4, 2, memSize[nameof(MemorySize.Int16)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_2xInt16)], 4, 2, memSize[nameof(MemorySize.Int16)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_2xUInt32)], 8, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_2xUInt32)], 8, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_2xUInt32)], 8, 4, memSize[nameof(MemorySize.UInt32)], false, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_2xInt32)], 8, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_2xInt32)], 8, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_2xInt32)], 8, 4, memSize[nameof(MemorySize.Int32)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast128_2xBFloat16)], 4, 2, memSize[nameof(MemorySize.BFloat16)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast256_2xBFloat16)], 4, 2, memSize[nameof(MemorySize.BFloat16)], true, true),
				new MemorySizeInfo(memSize[nameof(MemorySize.Broadcast512_2xBFloat16)], 4, 2, memSize[nameof(MemorySize.BFloat16)], true, true),
			}.ToArray();
			if (result.Length != memSize.Values.Length)
				throw new InvalidOperationException();
			if (result.Select(a => a.MemorySize).ToHashSet().Count != memSize.Values.Length)
				throw new InvalidOperationException();
			Array.Sort(result, (a, b) => a.MemorySize.Value.CompareTo(b.MemorySize.Value));
			return result;
		}
	}
}
