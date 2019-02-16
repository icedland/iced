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

using System;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	static class ThrowHelper {
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentException() => throw new ArgumentException();

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowInvalidOperationException() => throw new InvalidOperationException();

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_codeWriter() => throw new ArgumentNullException("codeWriter");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_data() => throw new ArgumentNullException("data");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_writer() => throw new ArgumentNullException("writer");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_options() => throw new ArgumentNullException("options");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_value() => throw new ArgumentNullException("value");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_list() => throw new ArgumentNullException("list");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_collection() => throw new ArgumentNullException("collection");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_array() => throw new ArgumentNullException("array");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentNullException_sb() => throw new ArgumentNullException("sb");

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_value() => throw new ArgumentOutOfRangeException("value");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_index() => throw new ArgumentOutOfRangeException("index");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_count() => throw new ArgumentOutOfRangeException("count");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_operand() => throw new ArgumentOutOfRangeException("operand");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_capacity() => throw new ArgumentOutOfRangeException("capacity");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_memorySize() => throw new ArgumentOutOfRangeException("memorySize");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_size() => throw new ArgumentOutOfRangeException("size");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_elementSize() => throw new ArgumentOutOfRangeException("elementSize");
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException_register() => throw new ArgumentOutOfRangeException("register");
	}
}
