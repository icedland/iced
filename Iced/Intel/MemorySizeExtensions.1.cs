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

#if !NO_INSTR_INFO
using System.Diagnostics;

namespace Iced.Intel {
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
			new MemorySizeInfo(MemorySize.BFloat16, 2, 2, MemorySize.BFloat16, true, false),
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
			new MemorySizeInfo(MemorySize.Packed32_BFloat16, 4, 2, MemorySize.BFloat16, true, false),
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
			new MemorySizeInfo(MemorySize.Packed128_2xBFloat16, 16, 4, MemorySize.Packed32_BFloat16, true, false),
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
			new MemorySizeInfo(MemorySize.Packed256_2xBFloat16, 32, 4, MemorySize.Packed32_BFloat16, true, false),
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
			new MemorySizeInfo(MemorySize.Packed512_2xBFloat16, 64, 4, MemorySize.Packed32_BFloat16, true, false),
			new MemorySizeInfo(MemorySize.Broadcast64_UInt32, 4, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast64_Int32, 4, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast64_Float32, 4, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt32, 4, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Int32, 4, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt52, 8, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_UInt64, 8, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Int64, 8, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Float32, 4, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_Float64, 8, 8, MemorySize.Float64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt32, 4, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Int32, 4, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt52, 8, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_UInt64, 8, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Int64, 8, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Float32, 4, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_Float64, 8, 8, MemorySize.Float64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt32, 4, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Int32, 4, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt52, 8, 8, MemorySize.UInt52, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_UInt64, 8, 8, MemorySize.UInt64, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Int64, 8, 8, MemorySize.Int64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Float32, 4, 4, MemorySize.Float32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_Float64, 8, 8, MemorySize.Float64, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_2xInt16, 4, 2, MemorySize.Int16, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_2xInt16, 4, 2, MemorySize.Int16, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_2xInt16, 4, 2, MemorySize.Int16, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_2xUInt32, 8, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast256_2xUInt32, 8, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast512_2xUInt32, 8, 4, MemorySize.UInt32, false, true),
			new MemorySizeInfo(MemorySize.Broadcast128_2xInt32, 8, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_2xInt32, 8, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_2xInt32, 8, 4, MemorySize.Int32, true, true),
			new MemorySizeInfo(MemorySize.Broadcast128_2xBFloat16, 4, 2, MemorySize.BFloat16, true, true),
			new MemorySizeInfo(MemorySize.Broadcast256_2xBFloat16, 4, 2, MemorySize.BFloat16, true, true),
			new MemorySizeInfo(MemorySize.Broadcast512_2xBFloat16, 4, 2, MemorySize.BFloat16, true, true),
		};

		/// <summary>
		/// Gets the memory size info
		/// </summary>
		/// <param name="memorySize">Memory size</param>
		/// <returns></returns>
		public static MemorySizeInfo GetInfo(this MemorySize memorySize) {
			var infos = MemorySizeInfos;
			if ((uint)memorySize >= (uint)infos.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_memorySize();
			return infos[(int)memorySize];
		}

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
		/// <param name="elementType">Element type if it's packed data or <paramref name="memorySize"/> if it's not packed data</param>
		/// <param name="isSigned">true if signed data</param>
		/// <param name="isBroadcast">true if broadcast</param>
		public MemorySizeInfo(MemorySize memorySize, int size, int elementSize, MemorySize elementType, bool isSigned, bool isBroadcast) {
			if (size < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_size();
			if (elementSize < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_elementSize();
			if (elementSize > size)
				ThrowHelper.ThrowArgumentOutOfRangeException_elementSize();
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
}
#endif
