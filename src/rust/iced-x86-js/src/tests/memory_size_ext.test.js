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
