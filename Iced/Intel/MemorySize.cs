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
		/// Memory location contains a 16-bit limit and a 32-bit address (eg. lgdtw, lgdtd)
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
		/// Memory location contains a bfloat16
		/// </summary>
		BFloat16,

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
		/// 32 bit location: 2 x bfloat16
		/// </summary>
		Packed32_BFloat16,

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
		/// 128 bit location: 4 x (2 x bfloat16)
		/// </summary>
		Packed128_2xBFloat16,

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
		/// 256 bit location: 8 x (2 x bfloat16)
		/// </summary>
		Packed256_2xBFloat16,

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
		/// 512 bit location: 16 x (2 x bfloat16)
		/// </summary>
		Packed512_2xBFloat16,

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

		/// <summary>
		/// Broadcast 2 x int16 to 128 bits
		/// </summary>
		Broadcast128_2xInt16,

		/// <summary>
		/// Broadcast 2 x int16 to 256 bits
		/// </summary>
		Broadcast256_2xInt16,

		/// <summary>
		/// Broadcast 2 x int16 to 512 bits
		/// </summary>
		Broadcast512_2xInt16,

		/// <summary>
		/// Broadcast 2 x uint32 to 128 bits
		/// </summary>
		Broadcast128_2xUInt32,

		/// <summary>
		/// Broadcast 2 x uint32 to 256 bits
		/// </summary>
		Broadcast256_2xUInt32,

		/// <summary>
		/// Broadcast 2 x uint32 to 512 bits
		/// </summary>
		Broadcast512_2xUInt32,

		/// <summary>
		/// Broadcast 2 x int32 to 128 bits
		/// </summary>
		Broadcast128_2xInt32,

		/// <summary>
		/// Broadcast 2 x int32 to 256 bits
		/// </summary>
		Broadcast256_2xInt32,

		/// <summary>
		/// Broadcast 2 x int32 to 512 bits
		/// </summary>
		Broadcast512_2xInt32,

		/// <summary>
		/// Broadcast 2 x bfloat16 to 128 bits
		/// </summary>
		Broadcast128_2xBFloat16,

		/// <summary>
		/// Broadcast 2 x bfloat16 to 256 bits
		/// </summary>
		Broadcast256_2xBFloat16,

		/// <summary>
		/// Broadcast 2 x bfloat16 to 512 bits
		/// </summary>
		Broadcast512_2xBFloat16,
	}
}
