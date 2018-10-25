/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;

namespace Iced.Intel {
	/// <summary>
	/// Size of a memory reference
	/// </summary>
	public enum MemorySize {
		/// <summary>
		/// Unknown size or the instruction doesn't reference the memory (eg. <c>lea</c>)
		/// </summary>
		Unknown,

		/// <summary>
		/// Memory location contains a <see cref="byte"/>
		/// </summary>
		UInt8,

		/// <summary>
		/// Memory location contains a <see cref="ushort"/>
		/// </summary>
		UInt16,

		/// <summary>
		/// Memory location contains a <see cref="uint"/>
		/// </summary>
		UInt32,

		/// <summary>
		/// Memory location contains a 52-bit unsigned integer
		/// </summary>
		UInt52,

		/// <summary>
		/// Memory location contains a <see cref="ulong"/>
		/// </summary>
		UInt64,

		/// <summary>
		/// Memory location contains a uint128
		/// </summary>
		UInt128,

		/// <summary>
		/// Memory location contains a uint256
		/// </summary>
		UInt256,

		/// <summary>
		/// Memory location contains a uint512
		/// </summary>
		UInt512,

		/// <summary>
		/// Memory location contains a <see cref="sbyte"/>
		/// </summary>
		Int8,

		/// <summary>
		/// Memory location contains a <see cref="short"/>
		/// </summary>
		Int16,

		/// <summary>
		/// Memory location contains a <see cref="int"/>
		/// </summary>
		Int32,

		/// <summary>
		/// Memory location contains a <see cref="long"/>
		/// </summary>
		Int64,

		/// <summary>
		/// Memory location contains a int128
		/// </summary>
		Int128,

		/// <summary>
		/// Memory location contains a int256
		/// </summary>
		Int256,

		/// <summary>
		/// Memory location contains a int512
		/// </summary>
		Int512,

		/// <summary>
		/// Memory location contains a seg:ptr pair, <see cref="ushort"/> (offset) + <see cref="ushort"/> (segment/selector)
		/// </summary>
		SegPtr16,

		/// <summary>
		/// Memory location contains a seg:ptr pair, <see cref="uint"/> (offset) + <see cref="ushort"/> (segment/selector)
		/// </summary>
		SegPtr32,

		/// <summary>
		/// Memory location contains a seg:ptr pair, <see cref="ulong"/> (offset) + <see cref="ushort"/> (segment/selector)
		/// </summary>
		SegPtr64,

		/// <summary>
		/// Memory location contains a 16-bit offset (jmp/call word ptr [mem])
		/// </summary>
		WordOffset,

		/// <summary>
		/// Memory location contains a 32-bit offset (jmp/call dword ptr [mem])
		/// </summary>
		DwordOffset,

		/// <summary>
		/// Memory location contains a 64-bit offset (jmp/call qword ptr [mem])
		/// </summary>
		QwordOffset,

		/// <summary>
		/// Memory location contains two <see cref="ushort"/>s (16-bit bound)
		/// </summary>
		Bound16_WordWord,

		/// <summary>
		/// Memory location contains two <see cref="uint"/>s (32-bit bound)
		/// </summary>
		Bound32_DwordDword,

		/// <summary>
		/// 32-bit bndmov, 2 x uint32
		/// </summary>
		Bnd32,

		/// <summary>
		/// 64-bit bndmov, 2 x uint64
		/// </summary>
		Bnd64,

		/// <summary>
		/// Memory location contains a 16-bit limit and a 24-bit address (eg. lgdtw)
		/// </summary>
		Fword5,

		/// <summary>
		/// Memory location contains a 16-bit limit and a 32-bit address (eg. lgdtd)
		/// </summary>
		Fword6,

		/// <summary>
		/// Memory location contains a 16-bit limit and a 64-bit address (eg. lgdtq)
		/// </summary>
		Fword10,

		/// <summary>
		/// Memory location contains an 16-bit floating point value
		/// </summary>
		Float16,

		/// <summary>
		/// Memory location contains a <see cref="float"/>
		/// </summary>
		Float32,

		/// <summary>
		/// Memory location contains a <see cref="double"/>
		/// </summary>
		Float64,

		/// <summary>
		/// Memory location contains an 80-bit floating point value
		/// </summary>
		Float80,

		/// <summary>
		/// Memory location contains a float128
		/// </summary>
		Float128,

		/// <summary>
		/// Memory location contains a 14-byte FPU environment (16-bit fldenv/fstenv)
		/// </summary>
		FpuEnv14,

		/// <summary>
		/// Memory location contains a 28-byte FPU environment (32/64-bit fldenv/fstenv)
		/// </summary>
		FpuEnv28,

		/// <summary>
		/// Memory location contains a 94-byte FPU environment (16-bit fsave/frstor)
		/// </summary>
		FpuState94,

		/// <summary>
		/// Memory location contains a 108-byte FPU environment (32/64-bit fsave/frstor)
		/// </summary>
		FpuState108,

		/// <summary>
		/// Memory location contains 512-bytes of fxsave/fxrstor data
		/// </summary>
		Fxsave_512Byte,

		/// <summary>
		/// Memory location contains 512-bytes of fxsave64/fxrstor64 data
		/// </summary>
		Fxsave64_512Byte,

		/// <summary>
		/// 32-bit XSAVE area
		/// </summary>
		Xsave,

		/// <summary>
		/// 64-bit XSAVE area
		/// </summary>
		Xsave64,

		/// <summary>
		/// Memory location contains a 10-byte bcd value (fbld/fbstp)
		/// </summary>
		Bcd,

		/// <summary>
		/// 16 bit location: 2 x uint8
		/// </summary>
		Packed16_UInt8,

		/// <summary>
		/// 16 bit location: 2 x int8
		/// </summary>
		Packed16_Int8,

		/// <summary>
		/// 32 bit location: 4 x uint8
		/// </summary>
		Packed32_UInt8,

		/// <summary>
		/// 32 bit location: 4 x int8
		/// </summary>
		Packed32_Int8,

		/// <summary>
		/// 32 bit location: 2 x uint16
		/// </summary>
		Packed32_UInt16,

		/// <summary>
		/// 32 bit location: 2 x int16
		/// </summary>
		Packed32_Int16,

		/// <summary>
		/// 64-bit location: 8 x uint8
		/// </summary>
		Packed64_UInt8,

		/// <summary>
		/// 64-bit location: 8 x int8
		/// </summary>
		Packed64_Int8,

		/// <summary>
		/// 64-bit location: 4 x uint16
		/// </summary>
		Packed64_UInt16,

		/// <summary>
		/// 64-bit location: 4 x int16
		/// </summary>
		Packed64_Int16,

		/// <summary>
		/// 64-bit location: 2 x uint32
		/// </summary>
		Packed64_UInt32,

		/// <summary>
		/// 64-bit location: 2 x int32
		/// </summary>
		Packed64_Int32,

		/// <summary>
		/// 64-bit location: 4 x float16
		/// </summary>
		Packed64_Float16,

		/// <summary>
		/// 64-bit location: 2 x float32
		/// </summary>
		Packed64_Float32,

		/// <summary>
		/// 128 bit location: 16 x uint8
		/// </summary>
		Packed128_UInt8,

		/// <summary>
		/// 128 bit location: 16 x int8
		/// </summary>
		Packed128_Int8,

		/// <summary>
		/// 128 bit location: 8 x uint16
		/// </summary>
		Packed128_UInt16,

		/// <summary>
		/// 128 bit location: 8 x int16
		/// </summary>
		Packed128_Int16,

		/// <summary>
		/// 128 bit location: 4 x uint32
		/// </summary>
		Packed128_UInt32,

		/// <summary>
		/// 128 bit location: 4 x int32
		/// </summary>
		Packed128_Int32,

		/// <summary>
		/// 128 bit location: 2 x uint52
		/// </summary>
		Packed128_UInt52,

		/// <summary>
		/// 128 bit location: 2 x uint64
		/// </summary>
		Packed128_UInt64,

		/// <summary>
		/// 128 bit location: 2 x int64
		/// </summary>
		Packed128_Int64,

		/// <summary>
		/// 128 bit location: 8 x float16
		/// </summary>
		Packed128_Float16,

		/// <summary>
		/// 128 bit location: 4 x float32
		/// </summary>
		Packed128_Float32,

		/// <summary>
		/// 128 bit location: 2 x float64
		/// </summary>
		Packed128_Float64,

		/// <summary>
		/// 256 bit location: 32 x uint8
		/// </summary>
		Packed256_UInt8,

		/// <summary>
		/// 256 bit location: 32 x int8
		/// </summary>
		Packed256_Int8,

		/// <summary>
		/// 256 bit location: 16 x uint16
		/// </summary>
		Packed256_UInt16,

		/// <summary>
		/// 256 bit location: 16 x int16
		/// </summary>
		Packed256_Int16,

		/// <summary>
		/// 256 bit location: 8 x uint32
		/// </summary>
		Packed256_UInt32,

		/// <summary>
		/// 256 bit location: 8 x int32
		/// </summary>
		Packed256_Int32,

		/// <summary>
		/// 256 bit location: 4 x uint52
		/// </summary>
		Packed256_UInt52,

		/// <summary>
		/// 256 bit location: 4 x uint64
		/// </summary>
		Packed256_UInt64,

		/// <summary>
		/// 256 bit location: 4 x int64
		/// </summary>
		Packed256_Int64,

		/// <summary>
		/// 256 bit location: 2 x uint128
		/// </summary>
		Packed256_UInt128,

		/// <summary>
		/// 256 bit location: 2 x int128
		/// </summary>
		Packed256_Int128,

		/// <summary>
		/// 256 bit location: 16 x float16
		/// </summary>
		Packed256_Float16,

		/// <summary>
		/// 256 bit location: 8 x float32
		/// </summary>
		Packed256_Float32,

		/// <summary>
		/// 256 bit location: 4 x float64
		/// </summary>
		Packed256_Float64,

		/// <summary>
		/// 256 bit location: 2 x float128
		/// </summary>
		Packed256_Float128,

		/// <summary>
		/// 512 bit location: 64 x uint8
		/// </summary>
		Packed512_UInt8,

		/// <summary>
		/// 512 bit location: 64 x int8
		/// </summary>
		Packed512_Int8,

		/// <summary>
		/// 512 bit location: 32 x uint16
		/// </summary>
		Packed512_UInt16,

		/// <summary>
		/// 512 bit location: 32 x int16
		/// </summary>
		Packed512_Int16,

		/// <summary>
		/// 512 bit location: 16 x uint32
		/// </summary>
		Packed512_UInt32,

		/// <summary>
		/// 512 bit location: 16 x int32
		/// </summary>
		Packed512_Int32,

		/// <summary>
		/// 512 bit location: 8 x uint52
		/// </summary>
		Packed512_UInt52,

		/// <summary>
		/// 512 bit location: 8 x uint64
		/// </summary>
		Packed512_UInt64,

		/// <summary>
		/// 512 bit location: 8 x int64
		/// </summary>
		Packed512_Int64,

		/// <summary>
		/// 256 bit location: 4 x uint128
		/// </summary>
		Packed512_UInt128,

		/// <summary>
		/// 512 bit location: 16 x float32
		/// </summary>
		Packed512_Float32,

		/// <summary>
		/// 512 bit location: 8 x float64
		/// </summary>
		Packed512_Float64,

		/// <summary>
		/// Broadcast uint32 to 64 bits
		/// </summary>
		Broadcast64_UInt32,

		/// <summary>
		/// Broadcast int32 to 64 bits
		/// </summary>
		Broadcast64_Int32,

		/// <summary>
		/// Broadcast float32 to 64 bits
		/// </summary>
		Broadcast64_Float32,

		/// <summary>
		/// Broadcast uint32 to 128 bits
		/// </summary>
		Broadcast128_UInt32,

		/// <summary>
		/// Broadcast int32 to 128 bits
		/// </summary>
		Broadcast128_Int32,

		/// <summary>
		/// Broadcast uint52 to 128 bits
		/// </summary>
		Broadcast128_UInt52,

		/// <summary>
		/// Broadcast uint64 to 128 bits
		/// </summary>
		Broadcast128_UInt64,

		/// <summary>
		/// Broadcast int64 to 128 bits
		/// </summary>
		Broadcast128_Int64,

		/// <summary>
		/// Broadcast float32 to 128 bits
		/// </summary>
		Broadcast128_Float32,

		/// <summary>
		/// Broadcast float64 to 128 bits
		/// </summary>
		Broadcast128_Float64,

		/// <summary>
		/// Broadcast uint32 to 256 bits
		/// </summary>
		Broadcast256_UInt32,

		/// <summary>
		/// Broadcast int32 to 256 bits
		/// </summary>
		Broadcast256_Int32,

		/// <summary>
		/// Broadcast uint52 to 256 bits
		/// </summary>
		Broadcast256_UInt52,

		/// <summary>
		/// Broadcast uint64 to 256 bits
		/// </summary>
		Broadcast256_UInt64,

		/// <summary>
		/// Broadcast int64 to 256 bits
		/// </summary>
		Broadcast256_Int64,

		/// <summary>
		/// Broadcast float32 to 256 bits
		/// </summary>
		Broadcast256_Float32,

		/// <summary>
		/// Broadcast float64 to 256 bits
		/// </summary>
		Broadcast256_Float64,

		/// <summary>
		/// Broadcast uint32 to 512 bits
		/// </summary>
		Broadcast512_UInt32,

		/// <summary>
		/// Broadcast int32 to 512 bits
		/// </summary>
		Broadcast512_Int32,

		/// <summary>
		/// Broadcast uint52 to 512 bits
		/// </summary>
		Broadcast512_UInt52,

		/// <summary>
		/// Broadcast uint64 to 512 bits
		/// </summary>
		Broadcast512_UInt64,

		/// <summary>
		/// Broadcast int64 to 512 bits
		/// </summary>
		Broadcast512_Int64,

		/// <summary>
		/// Broadcast float32 to 512 bits
		/// </summary>
		Broadcast512_Float32,

		/// <summary>
		/// Broadcast float64 to 512 bits
		/// </summary>
		Broadcast512_Float64,
	}

#if !NO_INSTR_INFO || !NO_ENCODER
	/// <summary>
	/// <see cref="MemorySize"/> extension methods
	/// </summary>
	public static partial class MemorySizeExtensions {
		/// <summary>
		/// Checks if <paramref name="memorySize"/> is a broadcast memory type
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static bool IsBroadcast(this MemorySize memorySize) => memorySize >= FirstBroadcastMemorySize;
		const MemorySize FirstBroadcastMemorySize = MemorySize.Broadcast64_UInt32;
	}
#endif

#if !NO_INSTR_INFO
	/// <summary>
	/// <see cref="MemorySize"/> extension methods
	/// </summary>
	public static partial class MemorySizeExtensions {
		internal static readonly MemorySizeInfo[] MemorySizeInfos = new MemorySizeInfo[DecoderConstants.NumberOfMemorySizes] {
			new MemorySizeInfo(MemorySize.Unknown, 0, 0, MemorySize.Unknown, false, false),
			new MemorySizeInfo(MemorySize.UInt8, 1, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.UInt16, 2, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.UInt32, 4, 4, MemorySize.UInt32, false, false),
			new MemorySizeInfo(MemorySize.UInt52, 8, 8, MemorySize.UInt52, false, false),
			new MemorySizeInfo(MemorySize.UInt64, 8, 8, MemorySize.UInt64, false, false),
			new MemorySizeInfo(MemorySize.UInt128, 16, 16, MemorySize.UInt128, false, false),
			new MemorySizeInfo(MemorySize.UInt256, 32, 32, MemorySize.UInt256, false, false),
			new MemorySizeInfo(MemorySize.UInt512, 64, 64, MemorySize.UInt512, false, false),
			new MemorySizeInfo(MemorySize.Int8, 1, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Int16, 2, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Int32, 4, 4, MemorySize.Int32, true, false),
			new MemorySizeInfo(MemorySize.Int64, 8, 8, MemorySize.Int64, true, false),
			new MemorySizeInfo(MemorySize.Int128, 16, 16, MemorySize.Int128, true, false),
			new MemorySizeInfo(MemorySize.Int256, 32, 32, MemorySize.Int256, true, false),
			new MemorySizeInfo(MemorySize.Int512, 64, 64, MemorySize.Int512, true, false),
			new MemorySizeInfo(MemorySize.SegPtr16, 4, 4, MemorySize.SegPtr16, false, false),
			new MemorySizeInfo(MemorySize.SegPtr32, 6, 6, MemorySize.SegPtr32, false, false),
			new MemorySizeInfo(MemorySize.SegPtr64, 10, 10, MemorySize.SegPtr64, false, false),
			new MemorySizeInfo(MemorySize.WordOffset, 2, 2, MemorySize.WordOffset, false, false),
			new MemorySizeInfo(MemorySize.DwordOffset, 4, 4, MemorySize.DwordOffset, false, false),
			new MemorySizeInfo(MemorySize.QwordOffset, 8, 8, MemorySize.QwordOffset, false, false),
			new MemorySizeInfo(MemorySize.Bound16_WordWord, 4, 4, MemorySize.Bound16_WordWord, false, false),
			new MemorySizeInfo(MemorySize.Bound32_DwordDword, 8, 8, MemorySize.Bound32_DwordDword, false, false),
			new MemorySizeInfo(MemorySize.Bnd32, 8, 8, MemorySize.Bnd32, false, false),
			new MemorySizeInfo(MemorySize.Bnd64, 16, 16, MemorySize.Bnd64, false, false),
			new MemorySizeInfo(MemorySize.Fword5, 5, 5, MemorySize.Fword5, false, false),
			new MemorySizeInfo(MemorySize.Fword6, 6, 6, MemorySize.Fword6, false, false),
			new MemorySizeInfo(MemorySize.Fword10, 10, 10, MemorySize.Fword10, false, false),
			new MemorySizeInfo(MemorySize.Float16, 2, 2, MemorySize.Float16, true, false),
			new MemorySizeInfo(MemorySize.Float32, 4, 4, MemorySize.Float32, true, false),
			new MemorySizeInfo(MemorySize.Float64, 8, 8, MemorySize.Float64, true, false),
			new MemorySizeInfo(MemorySize.Float80, 10, 10, MemorySize.Float80, true, false),
			new MemorySizeInfo(MemorySize.Float128, 16, 16, MemorySize.Float128, true, false),
			new MemorySizeInfo(MemorySize.FpuEnv14, 14, 14, MemorySize.FpuEnv14, false, false),
			new MemorySizeInfo(MemorySize.FpuEnv28, 28, 28, MemorySize.FpuEnv28, false, false),
			new MemorySizeInfo(MemorySize.FpuState94, 94, 94, MemorySize.FpuState94, false, false),
			new MemorySizeInfo(MemorySize.FpuState108, 108, 108, MemorySize.FpuState108, false, false),
			new MemorySizeInfo(MemorySize.Fxsave_512Byte, 512, 512, MemorySize.Fxsave_512Byte, false, false),
			new MemorySizeInfo(MemorySize.Fxsave64_512Byte, 512, 512, MemorySize.Fxsave64_512Byte, false, false),
			new MemorySizeInfo(MemorySize.Xsave, 0, 0, MemorySize.Xsave, false, false),
			new MemorySizeInfo(MemorySize.Xsave64, 0, 0, MemorySize.Xsave64, false, false),
			new MemorySizeInfo(MemorySize.Bcd, 10, 10, MemorySize.Bcd, true, false),
			new MemorySizeInfo(MemorySize.Packed16_UInt8, 2, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed16_Int8, 2, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed32_UInt8, 4, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed32_Int8, 4, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed32_UInt16, 4, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.Packed32_Int16, 4, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Packed64_UInt8, 8, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed64_Int8, 8, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed64_UInt16, 8, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.Packed64_Int16, 8, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Packed64_UInt32, 8, 4, MemorySize.UInt32, false, false),
			new MemorySizeInfo(MemorySize.Packed64_Int32, 8, 4, MemorySize.Int32, true, false),
			new MemorySizeInfo(MemorySize.Packed64_Float16, 8, 2, MemorySize.Float16, true, false),
			new MemorySizeInfo(MemorySize.Packed64_Float32, 8, 4, MemorySize.Float32, true, false),
			new MemorySizeInfo(MemorySize.Packed128_UInt8, 16, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed128_Int8, 16, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed128_UInt16, 16, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.Packed128_Int16, 16, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Packed128_UInt32, 16, 4, MemorySize.UInt32, false, false),
			new MemorySizeInfo(MemorySize.Packed128_Int32, 16, 4, MemorySize.Int32, true, false),
			new MemorySizeInfo(MemorySize.Packed128_UInt52, 16, 8, MemorySize.UInt52, false, false),
			new MemorySizeInfo(MemorySize.Packed128_UInt64, 16, 8, MemorySize.UInt64, false, false),
			new MemorySizeInfo(MemorySize.Packed128_Int64, 16, 8, MemorySize.Int64, true, false),
			new MemorySizeInfo(MemorySize.Packed128_Float16, 16, 2, MemorySize.Float16, true, false),
			new MemorySizeInfo(MemorySize.Packed128_Float32, 16, 4, MemorySize.Float32, true, false),
			new MemorySizeInfo(MemorySize.Packed128_Float64, 16, 8, MemorySize.Float64, true, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt8, 32, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed256_Int8, 32, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt16, 32, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.Packed256_Int16, 32, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt32, 32, 4, MemorySize.UInt32, false, false),
			new MemorySizeInfo(MemorySize.Packed256_Int32, 32, 4, MemorySize.Int32, true, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt52, 32, 8, MemorySize.UInt52, false, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt64, 32, 8, MemorySize.UInt64, false, false),
			new MemorySizeInfo(MemorySize.Packed256_Int64, 32, 8, MemorySize.Int64, true, false),
			new MemorySizeInfo(MemorySize.Packed256_UInt128, 32, 16, MemorySize.UInt128, false, false),
			new MemorySizeInfo(MemorySize.Packed256_Int128, 32, 16, MemorySize.Int128, true, false),
			new MemorySizeInfo(MemorySize.Packed256_Float16, 32, 2, MemorySize.Float16, true, false),
			new MemorySizeInfo(MemorySize.Packed256_Float32, 32, 4, MemorySize.Float32, true, false),
			new MemorySizeInfo(MemorySize.Packed256_Float64, 32, 8, MemorySize.Float64, true, false),
			new MemorySizeInfo(MemorySize.Packed256_Float128, 32, 16, MemorySize.Float128, true, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt8, 64, 1, MemorySize.UInt8, false, false),
			new MemorySizeInfo(MemorySize.Packed512_Int8, 64, 1, MemorySize.Int8, true, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt16, 64, 2, MemorySize.UInt16, false, false),
			new MemorySizeInfo(MemorySize.Packed512_Int16, 64, 2, MemorySize.Int16, true, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt32, 64, 4, MemorySize.UInt32, false, false),
			new MemorySizeInfo(MemorySize.Packed512_Int32, 64, 4, MemorySize.Int32, true, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt52, 64, 8, MemorySize.UInt52, false, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt64, 64, 8, MemorySize.UInt64, false, false),
			new MemorySizeInfo(MemorySize.Packed512_Int64, 64, 8, MemorySize.Int64, true, false),
			new MemorySizeInfo(MemorySize.Packed512_UInt128, 64, 16, MemorySize.UInt128, false, false),
			new MemorySizeInfo(MemorySize.Packed512_Float32, 64, 4, MemorySize.Float32, true, false),
			new MemorySizeInfo(MemorySize.Packed512_Float64, 64, 8, MemorySize.Float64, true, false),
			new MemorySizeInfo(MemorySize.Broadcast64_UInt32, 8, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast64_Int32, 8, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast64_Float32, 8, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt32, 16, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Int32, 16, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt52, 16, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt64, 16, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Int64, 16, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Float32, 16, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Float64, 16, 8, MemorySize.Float64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt32, 32, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Int32, 32, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt52, 32, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt64, 32, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Int64, 32, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Float32, 32, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Float64, 32, 8, MemorySize.Float64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt32, 64, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Int32, 64, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt52, 64, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt64, 64, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Int64, 64, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Float32, 64, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Float64, 64, 8, MemorySize.Float64, true, true),
		};

		/// <summary>
		/// Gets the memory size info
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static MemorySizeInfo GetInfo(this MemorySize memorySize) {
			var infos = MemorySizeInfos;
			if ((uint)memorySize >= (uint)infos.Length)
				ThrowArgumentOutOfRangeException(nameof(memorySize));
			return infos[(int)memorySize];
		}

		static void ThrowArgumentOutOfRangeException(string paramName) => throw new ArgumentOutOfRangeException(paramName);

		/// <summary>
		/// Gets the size in bytes of the memory location or 0 if it's not accessed by the instruction or unknown or variable sized
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static int GetSize(this MemorySize memorySize) => memorySize.GetInfo().Size;

		/// <summary>
		/// Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to <see cref="GetSize(MemorySize)"/>.
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static int GetElementSize(this MemorySize memorySize) => memorySize.GetInfo().ElementSize;

		/// <summary>
		/// Gets the element type if it's packed data or <paramref name="memorySize"/> if it's not packed data
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static MemorySize GetElementType(this MemorySize memorySize) => memorySize.GetInfo().ElementType;

		/// <summary>
		/// true if it's signed data (signed integer or a floating point value)
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static bool IsSigned(this MemorySize memorySize) => memorySize.GetInfo().IsSigned;

		/// <summary>
		/// true if this is a packed data type, eg. <see cref="MemorySize.Packed128_Float32"/>
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static bool IsPacked(this MemorySize memorySize) => memorySize.GetInfo().IsPacked;

		/// <summary>
		/// Gets the number of elements in the packed data type or 1 if it's not packed data (<see cref="IsPacked"/>)
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static int GetElementCount(this MemorySize memorySize) => memorySize.GetInfo().ElementCount;
	}

	/// <summary>
	/// <see cref="Intel.MemorySize"/> information
	/// </summary>
	public readonly struct MemorySizeInfo {
		// 8 bytes in size
		readonly ushort size;
		readonly ushort elementSize;
		readonly byte memorySize;
		readonly byte elementType;
		// Use flags if more booleans are needed
		readonly bool isSigned;
		readonly bool isBroadcast;

		/// <summary>
		/// Gets the <see cref="Intel.MemorySize"/> value
		/// </summary>
		public MemorySize MemorySize => (MemorySize)memorySize;

		/// <summary>
		/// Size in bytes of the memory location or 0 if it's not accessed or unknown
		/// </summary>
		public int Size => size;

		/// <summary>
		/// Size in bytes of the packed element. If it's not a packed data type, it's equal to <see cref="Size"/>.
		/// </summary>
		public int ElementSize => elementSize;

		/// <summary>
		/// Element type if it's packed data or the type itself if it's not packed data
		/// </summary>
		public MemorySize ElementType => (MemorySize)elementType;

		/// <summary>
		/// true if it's signed data (signed integer or a floating point value)
		/// </summary>
		public bool IsSigned => isSigned;

		/// <summary>
		/// true if it's a broadcast memory type
		/// </summary>
		public bool IsBroadcast => isBroadcast;

		/// <summary>
		/// true if this is a packed data type, eg. <see cref="MemorySize.Packed128_Float32"/>. See also <see cref="ElementCount"/>
		/// </summary>
		public bool IsPacked => elementSize < size;

		/// <summary>
		/// Gets the number of elements in the packed data type or 1 if it's not packed data (<see cref="IsPacked"/>)
		/// </summary>
		public int ElementCount => elementSize == size ? 1 : size / elementSize;// ElementSize can be 0 so we don't divide by it if es == s

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="memorySize">Memory size value</param>
		/// <param name="size">Size of location</param>
		/// <param name="elementSize">Size of the packed element, or <paramref name="size"/> if it's not packed data</param>
		/// <param name="elementType">Element type if it's packed data or <see cref="MemorySize.Unknown"/></param>
		/// <param name="isSigned">true if signed data</param>
		/// <param name="isBroadcast">true if broadcast</param>
		public MemorySizeInfo(MemorySize memorySize, int size, int elementSize, MemorySize elementType, bool isSigned, bool isBroadcast) {
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size));
			if (elementSize < 0)
				throw new ArgumentOutOfRangeException(nameof(elementSize));
			if (elementSize > size)
				throw new ArgumentOutOfRangeException(nameof(elementSize));
			Debug.Assert(DecoderConstants.NumberOfMemorySizes <= byte.MaxValue + 1);
			this.memorySize = (byte)memorySize;
			Debug.Assert(size <= ushort.MaxValue);
			this.size = (ushort)size;
			Debug.Assert(elementSize <= ushort.MaxValue);
			this.elementSize = (ushort)elementSize;
			Debug.Assert(DecoderConstants.NumberOfMemorySizes <= byte.MaxValue + 1);
			this.elementType = (byte)elementType;
			this.isSigned = isSigned;
			this.isBroadcast = isBroadcast;
		}
	}
#endif
}
