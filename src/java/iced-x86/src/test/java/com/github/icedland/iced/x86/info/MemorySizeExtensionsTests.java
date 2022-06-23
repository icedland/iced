// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.MemorySize;
import com.github.icedland.iced.x86.MemorySizeInfo;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MemorySizeExtensionsTests {
	@ParameterizedTest
	@MethodSource("getInfo_throws_if_invalid_value_Data")
	void getInfo_throws_if_invalid_value(int memorySize) {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getInfo(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getSize(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementSize(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementType(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementTypeInfo(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isSigned(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isPacked(memorySize));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementCount(memorySize));
		MemorySize.isBroadcast(memorySize);// Does not throw
	}

	static Iterable<Arguments> getInfo_throws_if_invalid_value_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of(-1));
		result.add(Arguments.of(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
		return result;
	}

	@ParameterizedTest
	@MethodSource("verifyMemorySizeProperties_Data")
	void verifyMemorySizeProperties(int memorySize, int size, int elementSize, int elementType, int elementCount, int flags) {
		MemorySizeInfo info = MemorySize.getInfo(memorySize);
		assertEquals(memorySize, info.getMemorySize());
		assertEquals(size, info.getSize());
		assertEquals(elementSize, info.getElementSize());
		assertEquals(elementType, info.getElementType());
		assertEquals((flags & MemorySizeFlags.SIGNED) != 0, info.isSigned());
		assertEquals((flags & MemorySizeFlags.BROADCAST) != 0, info.isBroadcast());
		assertEquals((flags & MemorySizeFlags.PACKED) != 0, info.isPacked());
		assertEquals(elementCount, info.getElementCount());

		assertEquals(size, MemorySize.getSize(memorySize));
		assertEquals(elementSize, MemorySize.getElementSize(memorySize));
		assertEquals(elementType, MemorySize.getElementType(memorySize));
		assertEquals(elementType, MemorySize.getElementTypeInfo(memorySize).getMemorySize());
		assertEquals((flags & MemorySizeFlags.SIGNED) != 0, MemorySize.isSigned(memorySize));
		assertEquals((flags & MemorySizeFlags.PACKED) != 0, MemorySize.isPacked(memorySize));
		assertEquals((flags & MemorySizeFlags.BROADCAST) != 0, MemorySize.isBroadcast(memorySize));
		assertEquals(elementCount, MemorySize.getElementCount(memorySize));
	}

	public static Iterable<Arguments> verifyMemorySizeProperties_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (MemorySizeInfoTestCase tc : MemorySizeInfoTestReader.getTestCases())
			result.add(Arguments.of(tc.memorySize, tc.size, tc.elementSize, tc.elementType, tc.elementCount, tc.flags));
		return result;
	}
}
