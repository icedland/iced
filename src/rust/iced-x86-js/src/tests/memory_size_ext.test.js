// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const { MemorySize, MemorySizeExt } = require("iced-x86");

test("MemorySizeExt funcs", () => {
	expect(MemorySizeExt.size(MemorySize.UInt128)).toBe(16);
	expect(MemorySizeExt.elementSize(MemorySize.UInt128)).toBe(16);
	expect(MemorySizeExt.elementType(MemorySize.UInt128)).toBe(MemorySize.UInt128);
	expect(MemorySizeExt.isSigned(MemorySize.UInt128)).toBe(false);
	expect(MemorySizeExt.isSigned(MemorySize.Packed256_Int16)).toBe(true);
	expect(MemorySizeExt.elementCount(MemorySize.UInt128)).toBe(1);
	expect(MemorySizeExt.isBroadcast(MemorySize.UInt128)).toBe(false);

	expect(MemorySizeExt.size(MemorySize.Packed256_Int16)).toBe(32);
	expect(MemorySizeExt.elementSize(MemorySize.Packed256_Int16)).toBe(2);
	expect(MemorySizeExt.elementType(MemorySize.Packed256_Int16)).toBe(MemorySize.Int16);
	expect(MemorySizeExt.isSigned(MemorySize.Packed256_Int16)).toBe(true);
	expect(MemorySizeExt.isPacked(MemorySize.Packed256_Int16)).toBe(true);
	expect(MemorySizeExt.elementCount(MemorySize.Packed256_Int16)).toBe(16);
	expect(MemorySizeExt.isBroadcast(MemorySize.Packed256_Int16)).toBe(false);

	expect(MemorySizeExt.isBroadcast(MemorySize.Broadcast128_2xInt32)).toBe(true);
});
