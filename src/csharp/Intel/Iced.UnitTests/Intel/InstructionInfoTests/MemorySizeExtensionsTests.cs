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
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MemorySizeExtensionsTests {
		[Fact]
		void Verify_MemorySizeInfos() {
			var infos = MemorySizeExtensions.MemorySizeInfos;
			for (int i = 0; i < infos.Length; i++)
				Assert.Equal((MemorySize)i, infos[i].MemorySize);
		}

		[Theory]
		[InlineData((MemorySize)(-1))]
		[InlineData((MemorySize)IcedConstants.NumberOfMemorySizes)]
		void GetInfo_throws_if_invalid_value(MemorySize memorySize) {
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetElementSize());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetElementType());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetElementTypeInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.IsSigned());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.IsPacked());
			Assert.Throws<ArgumentOutOfRangeException>(() => memorySize.GetElementCount());
			memorySize.IsBroadcast();// Does not throw
		}

		[Flags]
		enum MemorySizeFlags {
			None				= 0,
			Signed				= 1,
			Broadcast			= 2,
			Packed				= 4,
		}

		[Theory]
		[MemberData(nameof(VerifyMemorySizeProperties_Data))]
		void VerifyMemorySizeProperties(MemorySize memorySize, int size, int elementSize, MemorySize elementType, int elementCount, MemorySizeFlags flags) {
			var info = memorySize.GetInfo();
			Assert.Equal(memorySize, info.MemorySize);
			Assert.Equal(size, info.Size);
			Assert.Equal(elementSize, info.ElementSize);
			Assert.Equal(elementType, info.ElementType);
			Assert.Equal((flags & MemorySizeFlags.Signed) != 0, info.IsSigned);
			Assert.Equal((flags & MemorySizeFlags.Broadcast) != 0, info.IsBroadcast);
			Assert.Equal((flags & MemorySizeFlags.Packed) != 0, info.IsPacked);
			Assert.Equal(elementCount, info.ElementCount);

			Assert.Equal(size, memorySize.GetSize());
			Assert.Equal(elementSize, memorySize.GetElementSize());
			Assert.Equal(elementType, memorySize.GetElementType());
			Assert.Equal(elementType, memorySize.GetElementTypeInfo().MemorySize);
			Assert.Equal((flags & MemorySizeFlags.Signed) != 0, memorySize.IsSigned());
			Assert.Equal((flags & MemorySizeFlags.Packed) != 0, memorySize.IsPacked());
			Assert.Equal((flags & MemorySizeFlags.Broadcast) != 0, memorySize.IsBroadcast());
			Assert.Equal(elementCount, memorySize.GetElementCount());
		}
		public static IEnumerable<object[]> VerifyMemorySizeProperties_Data {
			get {
				var res = new object[IcedConstants.NumberOfMemorySizes][] {
					new object[] { MemorySize.Unknown, 0, 0, MemorySize.Unknown, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt8, 1, 1, MemorySize.UInt8, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt16, 2, 2, MemorySize.UInt16, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt32, 4, 4, MemorySize.UInt32, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt52, 8, 8, MemorySize.UInt52, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt64, 8, 8, MemorySize.UInt64, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt128, 16, 16, MemorySize.UInt128, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt256, 32, 32, MemorySize.UInt256, 1, MemorySizeFlags.None },
					new object[] { MemorySize.UInt512, 64, 64, MemorySize.UInt512, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Int8, 1, 1, MemorySize.Int8, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int16, 2, 2, MemorySize.Int16, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int32, 4, 4, MemorySize.Int32, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int64, 8, 8, MemorySize.Int64, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int128, 16, 16, MemorySize.Int128, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int256, 32, 32, MemorySize.Int256, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Int512, 64, 64, MemorySize.Int512, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.SegPtr16, 4, 4, MemorySize.SegPtr16, 1, MemorySizeFlags.None },
					new object[] { MemorySize.SegPtr32, 6, 6, MemorySize.SegPtr32, 1, MemorySizeFlags.None },
					new object[] { MemorySize.SegPtr64, 10, 10, MemorySize.SegPtr64, 1, MemorySizeFlags.None },
					new object[] { MemorySize.WordOffset, 2, 2, MemorySize.WordOffset, 1, MemorySizeFlags.None },
					new object[] { MemorySize.DwordOffset, 4, 4, MemorySize.DwordOffset, 1, MemorySizeFlags.None },
					new object[] { MemorySize.QwordOffset, 8, 8, MemorySize.QwordOffset, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Bound16_WordWord, 4, 4, MemorySize.Bound16_WordWord, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Bound32_DwordDword, 8, 8, MemorySize.Bound32_DwordDword, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Bnd32, 8, 8, MemorySize.Bnd32, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Bnd64, 16, 16, MemorySize.Bnd64, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Fword6, 6, 6, MemorySize.Fword6, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Fword10, 10, 10, MemorySize.Fword10, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Float16, 2, 2, MemorySize.Float16, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Float32, 4, 4, MemorySize.Float32, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Float64, 8, 8, MemorySize.Float64, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Float80, 10, 10, MemorySize.Float80, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Float128, 16, 16, MemorySize.Float128, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.BFloat16, 2, 2, MemorySize.BFloat16, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.FpuEnv14, 14, 14, MemorySize.FpuEnv14, 1, MemorySizeFlags.None },
					new object[] { MemorySize.FpuEnv28, 28, 28, MemorySize.FpuEnv28, 1, MemorySizeFlags.None },
					new object[] { MemorySize.FpuState94, 94, 94, MemorySize.FpuState94, 1, MemorySizeFlags.None },
					new object[] { MemorySize.FpuState108, 108, 108, MemorySize.FpuState108, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Fxsave_512Byte, 512, 512, MemorySize.Fxsave_512Byte, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Fxsave64_512Byte, 512, 512, MemorySize.Fxsave64_512Byte, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Xsave, 0, 0, MemorySize.Xsave, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Xsave64, 0, 0, MemorySize.Xsave64, 1, MemorySizeFlags.None },
					new object[] { MemorySize.Bcd, 10, 10, MemorySize.Bcd, 1, MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed16_UInt8, 2, 1, MemorySize.UInt8, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed16_Int8, 2, 1, MemorySize.Int8, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed32_UInt8, 4, 1, MemorySize.UInt8, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed32_Int8, 4, 1, MemorySize.Int8, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed32_UInt16, 4, 2, MemorySize.UInt16, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed32_Int16, 4, 2, MemorySize.Int16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed32_BFloat16, 4, 2, MemorySize.BFloat16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed64_UInt8, 8, 1, MemorySize.UInt8, 8, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed64_Int8, 8, 1, MemorySize.Int8, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed64_UInt16, 8, 2, MemorySize.UInt16, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed64_Int16, 8, 2, MemorySize.Int16, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed64_UInt32, 8, 4, MemorySize.UInt32, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed64_Int32, 8, 4, MemorySize.Int32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed64_Float16, 8, 2, MemorySize.Float16, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed64_Float32, 8, 4, MemorySize.Float32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_UInt8, 16, 1, MemorySize.UInt8, 16, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed128_Int8, 16, 1, MemorySize.Int8, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_UInt16, 16, 2, MemorySize.UInt16, 8, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed128_Int16, 16, 2, MemorySize.Int16, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_UInt32, 16, 4, MemorySize.UInt32, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed128_Int32, 16, 4, MemorySize.Int32, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_UInt52, 16, 8, MemorySize.UInt52, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed128_UInt64, 16, 8, MemorySize.UInt64, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed128_Int64, 16, 8, MemorySize.Int64, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_Float16, 16, 2, MemorySize.Float16, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_Float32, 16, 4, MemorySize.Float32, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_Float64, 16, 8, MemorySize.Float64, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed128_2xBFloat16, 16, 4, MemorySize.Packed32_BFloat16, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_UInt8, 32, 1, MemorySize.UInt8, 32, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_Int8, 32, 1, MemorySize.Int8, 32, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_UInt16, 32, 2, MemorySize.UInt16, 16, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_Int16, 32, 2, MemorySize.Int16, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_UInt32, 32, 4, MemorySize.UInt32, 8, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_Int32, 32, 4, MemorySize.Int32, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_UInt52, 32, 8, MemorySize.UInt52, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_UInt64, 32, 8, MemorySize.UInt64, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_Int64, 32, 8, MemorySize.Int64, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_UInt128, 32, 16, MemorySize.UInt128, 2, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed256_Int128, 32, 16, MemorySize.Int128, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_Float16, 32, 2, MemorySize.Float16, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_Float32, 32, 4, MemorySize.Float32, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_Float64, 32, 8, MemorySize.Float64, 4, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_Float128, 32, 16, MemorySize.Float128, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed256_2xBFloat16, 32, 4, MemorySize.Packed32_BFloat16, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_UInt8, 64, 1, MemorySize.UInt8, 64, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_Int8, 64, 1, MemorySize.Int8, 64, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_UInt16, 64, 2, MemorySize.UInt16, 32, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_Int16, 64, 2, MemorySize.Int16, 32, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_UInt32, 64, 4, MemorySize.UInt32, 16, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_Int32, 64, 4, MemorySize.Int32, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_UInt52, 64, 8, MemorySize.UInt52, 8, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_UInt64, 64, 8, MemorySize.UInt64, 8, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_Int64, 64, 8, MemorySize.Int64, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_UInt128, 64, 16, MemorySize.UInt128, 4, MemorySizeFlags.Packed },
					new object[] { MemorySize.Packed512_Float32, 64, 4, MemorySize.Float32, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_Float64, 64, 8, MemorySize.Float64, 8, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Packed512_2xBFloat16, 64, 4, MemorySize.Packed32_BFloat16, 16, MemorySizeFlags.Packed | MemorySizeFlags.Signed },
					new object[] { MemorySize.Broadcast64_UInt32, 4, 4, MemorySize.UInt32, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast64_Int32, 4, 4, MemorySize.Int32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast64_Float32, 4, 4, MemorySize.Float32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_UInt32, 4, 4, MemorySize.UInt32, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_Int32, 4, 4, MemorySize.Int32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_UInt52, 8, 8, MemorySize.UInt52, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_UInt64, 8, 8, MemorySize.UInt64, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_Int64, 8, 8, MemorySize.Int64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_Float32, 4, 4, MemorySize.Float32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_Float64, 8, 8, MemorySize.Float64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_UInt32, 4, 4, MemorySize.UInt32, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_Int32, 4, 4, MemorySize.Int32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_UInt52, 8, 8, MemorySize.UInt52, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_UInt64, 8, 8, MemorySize.UInt64, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_Int64, 8, 8, MemorySize.Int64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_Float32, 4, 4, MemorySize.Float32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_Float64, 8, 8, MemorySize.Float64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_UInt32, 4, 4, MemorySize.UInt32, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_Int32, 4, 4, MemorySize.Int32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_UInt52, 8, 8, MemorySize.UInt52, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_UInt64, 8, 8, MemorySize.UInt64, 1, MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_Int64, 8, 8, MemorySize.Int64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_Float32, 4, 4, MemorySize.Float32, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_Float64, 8, 8, MemorySize.Float64, 1, MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_2xInt16, 4, 2, MemorySize.Int16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_2xInt16, 4, 2, MemorySize.Int16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_2xInt16, 4, 2, MemorySize.Int16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_2xUInt32, 8, 4, MemorySize.UInt32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_2xUInt32, 8, 4, MemorySize.UInt32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_2xUInt32, 8, 4, MemorySize.UInt32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_2xInt32, 8, 4, MemorySize.Int32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_2xInt32, 8, 4, MemorySize.Int32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_2xInt32, 8, 4, MemorySize.Int32, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast128_2xBFloat16, 4, 2, MemorySize.BFloat16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast256_2xBFloat16, 4, 2, MemorySize.BFloat16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
					new object[] { MemorySize.Broadcast512_2xBFloat16, 4, 2, MemorySize.BFloat16, 2, MemorySizeFlags.Packed | MemorySizeFlags.Signed | MemorySizeFlags.Broadcast },
				};
				for (int i = 0; i < res.Length; i++)
					Assert.Equal((MemorySize)i, (MemorySize)res[i][0]);
				return res;
			}
		}
	}
}
#endif
