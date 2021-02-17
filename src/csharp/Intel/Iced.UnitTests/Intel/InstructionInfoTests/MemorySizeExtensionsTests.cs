// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
