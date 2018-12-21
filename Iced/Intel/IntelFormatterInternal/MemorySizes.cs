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

		public static readonly (MemorySize memorySize, string[] names, string bcstTo)[] AllMemorySizes = new(MemorySize memorySize, string[] names, string bcstTo)[DecoderConstants.NumberOfMemorySizes] {
			(MemorySize.Unknown, Array.Empty<string>(), null),
			(MemorySize.UInt8, byte_ptr, null),
			(MemorySize.UInt16, word_ptr, null),
			(MemorySize.UInt32, dword_ptr, null),
			(MemorySize.UInt52, qword_ptr, null),
			(MemorySize.UInt64, qword_ptr, null),
			(MemorySize.UInt128, xmmword_ptr, null),
			(MemorySize.UInt256, ymmword_ptr, null),
			(MemorySize.UInt512, zmmword_ptr, null),
			(MemorySize.Int8, byte_ptr, null),
			(MemorySize.Int16, word_ptr, null),
			(MemorySize.Int32, dword_ptr, null),
			(MemorySize.Int64, qword_ptr, null),
			(MemorySize.Int128, xmmword_ptr, null),
			(MemorySize.Int256, ymmword_ptr, null),
			(MemorySize.Int512, zmmword_ptr, null),
			(MemorySize.SegPtr16, dword_ptr, null),
			(MemorySize.SegPtr32, fword_ptr, null),
			(MemorySize.SegPtr64, tbyte_ptr, null),
			(MemorySize.WordOffset, word_ptr, null),
			(MemorySize.DwordOffset, dword_ptr, null),
			(MemorySize.QwordOffset, qword_ptr, null),
			(MemorySize.Bound16_WordWord, dword_ptr, null),
			(MemorySize.Bound32_DwordDword, qword_ptr, null),
			(MemorySize.Bnd32, qword_ptr, null),
			(MemorySize.Bnd64, xmmword_ptr, null),
			(MemorySize.Fword5, fword_ptr, null),
			(MemorySize.Fword6, fword_ptr, null),
			(MemorySize.Fword10, fword_ptr, null),
			(MemorySize.Float16, word_ptr, null),
			(MemorySize.Float32, dword_ptr, null),
			(MemorySize.Float64, qword_ptr, null),
			(MemorySize.Float80, tbyte_ptr, null),
			(MemorySize.Float128, xmmword_ptr, null),
			(MemorySize.FpuEnv14, "fpuenv14 ptr".Split(' '), null),
			(MemorySize.FpuEnv28, "fpuenv28 ptr".Split(' '), null),
			(MemorySize.FpuState94, "fpustate94 ptr".Split(' '), null),
			(MemorySize.FpuState108, "fpustate108 ptr".Split(' '), null),
			(MemorySize.Fxsave_512Byte, Array.Empty<string>(), null),
			(MemorySize.Fxsave64_512Byte, Array.Empty<string>(), null),
			(MemorySize.Xsave, Array.Empty<string>(), null),
			(MemorySize.Xsave64, Array.Empty<string>(), null),
			(MemorySize.Bcd, tbyte_ptr, null),
			(MemorySize.Packed16_UInt8, word_ptr, null),
			(MemorySize.Packed16_Int8, word_ptr, null),
			(MemorySize.Packed32_UInt8, dword_ptr, null),
			(MemorySize.Packed32_Int8, dword_ptr, null),
			(MemorySize.Packed32_UInt16, dword_ptr, null),
			(MemorySize.Packed32_Int16, dword_ptr, null),
			(MemorySize.Packed64_UInt8, qword_ptr, null),
			(MemorySize.Packed64_Int8, qword_ptr, null),
			(MemorySize.Packed64_UInt16, qword_ptr, null),
			(MemorySize.Packed64_Int16, qword_ptr, null),
			(MemorySize.Packed64_UInt32, qword_ptr, null),
			(MemorySize.Packed64_Int32, qword_ptr, null),
			(MemorySize.Packed64_Float16, qword_ptr, null),
			(MemorySize.Packed64_Float32, qword_ptr, null),
			(MemorySize.Packed128_UInt8, xmmword_ptr, null),
			(MemorySize.Packed128_Int8, xmmword_ptr, null),
			(MemorySize.Packed128_UInt16, xmmword_ptr, null),
			(MemorySize.Packed128_Int16, xmmword_ptr, null),
			(MemorySize.Packed128_UInt32, xmmword_ptr, null),
			(MemorySize.Packed128_Int32, xmmword_ptr, null),
			(MemorySize.Packed128_UInt52, xmmword_ptr, null),
			(MemorySize.Packed128_UInt64, xmmword_ptr, null),
			(MemorySize.Packed128_Int64, xmmword_ptr, null),
			(MemorySize.Packed128_Float16, xmmword_ptr, null),
			(MemorySize.Packed128_Float32, xmmword_ptr, null),
			(MemorySize.Packed128_Float64, xmmword_ptr, null),
			(MemorySize.Packed256_UInt8, ymmword_ptr, null),
			(MemorySize.Packed256_Int8, ymmword_ptr, null),
			(MemorySize.Packed256_UInt16, ymmword_ptr, null),
			(MemorySize.Packed256_Int16, ymmword_ptr, null),
			(MemorySize.Packed256_UInt32, ymmword_ptr, null),
			(MemorySize.Packed256_Int32, ymmword_ptr, null),
			(MemorySize.Packed256_UInt52, ymmword_ptr, null),
			(MemorySize.Packed256_UInt64, ymmword_ptr, null),
			(MemorySize.Packed256_Int64, ymmword_ptr, null),
			(MemorySize.Packed256_UInt128, ymmword_ptr, null),
			(MemorySize.Packed256_Int128, ymmword_ptr, null),
			(MemorySize.Packed256_Float16, ymmword_ptr, null),
			(MemorySize.Packed256_Float32, ymmword_ptr, null),
			(MemorySize.Packed256_Float64, ymmword_ptr, null),
			(MemorySize.Packed256_Float128, ymmword_ptr, null),
			(MemorySize.Packed512_UInt8, zmmword_ptr, null),
			(MemorySize.Packed512_Int8, zmmword_ptr, null),
			(MemorySize.Packed512_UInt16, zmmword_ptr, null),
			(MemorySize.Packed512_Int16, zmmword_ptr, null),
			(MemorySize.Packed512_UInt32, zmmword_ptr, null),
			(MemorySize.Packed512_Int32, zmmword_ptr, null),
			(MemorySize.Packed512_UInt52, zmmword_ptr, null),
			(MemorySize.Packed512_UInt64, zmmword_ptr, null),
			(MemorySize.Packed512_Int64, zmmword_ptr, null),
			(MemorySize.Packed512_UInt128, zmmword_ptr, null),
			(MemorySize.Packed512_Float32, zmmword_ptr, null),
			(MemorySize.Packed512_Float64, zmmword_ptr, null),
			(MemorySize.Broadcast64_UInt32, dword_ptr, "1to2"),
			(MemorySize.Broadcast64_Int32, dword_ptr, "1to2"),
			(MemorySize.Broadcast64_Float32, dword_ptr, "1to2"),
			(MemorySize.Broadcast128_UInt32, dword_ptr, "1to4"),
			(MemorySize.Broadcast128_Int32, dword_ptr, "1to4"),
			(MemorySize.Broadcast128_UInt52, qword_ptr, "1to2"),
			(MemorySize.Broadcast128_UInt64, qword_ptr, "1to2"),
			(MemorySize.Broadcast128_Int64, qword_ptr, "1to2"),
			(MemorySize.Broadcast128_Float32, dword_ptr, "1to4"),
			(MemorySize.Broadcast128_Float64, qword_ptr, "1to2"),
			(MemorySize.Broadcast256_UInt32, dword_ptr, "1to8"),
			(MemorySize.Broadcast256_Int32, dword_ptr, "1to8"),
			(MemorySize.Broadcast256_UInt52, qword_ptr, "1to4"),
			(MemorySize.Broadcast256_UInt64, qword_ptr, "1to4"),
			(MemorySize.Broadcast256_Int64, qword_ptr, "1to4"),
			(MemorySize.Broadcast256_Float32, dword_ptr, "1to8"),
			(MemorySize.Broadcast256_Float64, qword_ptr, "1to4"),
			(MemorySize.Broadcast512_UInt32, dword_ptr, "1to16"),
			(MemorySize.Broadcast512_Int32, dword_ptr, "1to16"),
			(MemorySize.Broadcast512_UInt52, qword_ptr, "1to8"),
			(MemorySize.Broadcast512_UInt64, qword_ptr, "1to8"),
			(MemorySize.Broadcast512_Int64, qword_ptr, "1to8"),
			(MemorySize.Broadcast512_Float32, dword_ptr, "1to16"),
			(MemorySize.Broadcast512_Float64, qword_ptr, "1to8"),
			(MemorySize.Broadcast128_2xUInt32, qword_ptr, "1to2"),
			(MemorySize.Broadcast256_2xUInt32, qword_ptr, "1to4"),
			(MemorySize.Broadcast512_2xUInt32, qword_ptr, "1to8"),
			(MemorySize.Broadcast128_2xInt32, qword_ptr, "1to2"),
			(MemorySize.Broadcast256_2xInt32, qword_ptr, "1to4"),
			(MemorySize.Broadcast512_2xInt32, qword_ptr, "1to8"),
		};
	}
}
#endif
