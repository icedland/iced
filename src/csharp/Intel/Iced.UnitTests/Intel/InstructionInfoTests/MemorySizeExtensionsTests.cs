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

#if INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MemorySizeExtensionsTests {
		[Theory]
		[InlineData((MemorySize)(-1))]
		[InlineData((MemorySize)IcedConstants.MemorySizeEnumCount)]
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
				foreach (var tc in MemorySizeInfoTestReader.GetTestCases())
					yield return new object[] { tc.MemorySize, tc.Size, tc.ElementSize, tc.ElementType, tc.ElementCount, tc.Flags };
			}
		}
	}
}
#endif
