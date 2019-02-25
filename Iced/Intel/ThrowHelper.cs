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
		// NOTE: NoInlining is not used because RyuJIT doesn't move the method call to the end of the caller's method

		internal static void ThrowArgumentException() => throw new ArgumentException();

		internal static void ThrowInvalidOperationException() => throw new InvalidOperationException();

		internal static void ThrowArgumentNullException_codeWriter() => throw new ArgumentNullException("codeWriter");
		internal static void ThrowArgumentNullException_data() => throw new ArgumentNullException("data");
		internal static void ThrowArgumentNullException_writer() => throw new ArgumentNullException("writer");
		internal static void ThrowArgumentNullException_options() => throw new ArgumentNullException("options");
		internal static void ThrowArgumentNullException_value() => throw new ArgumentNullException("value");
		internal static void ThrowArgumentNullException_list() => throw new ArgumentNullException("list");
		internal static void ThrowArgumentNullException_collection() => throw new ArgumentNullException("collection");
		internal static void ThrowArgumentNullException_array() => throw new ArgumentNullException("array");
		internal static void ThrowArgumentNullException_sb() => throw new ArgumentNullException("sb");

		internal static void ThrowArgumentOutOfRangeException_value() => throw new ArgumentOutOfRangeException("value");
		internal static void ThrowArgumentOutOfRangeException_index() => throw new ArgumentOutOfRangeException("index");
		internal static void ThrowArgumentOutOfRangeException_count() => throw new ArgumentOutOfRangeException("count");
		internal static void ThrowArgumentOutOfRangeException_length() => throw new ArgumentOutOfRangeException("length");
		internal static void ThrowArgumentOutOfRangeException_operand() => throw new ArgumentOutOfRangeException("operand");
		internal static void ThrowArgumentOutOfRangeException_capacity() => throw new ArgumentOutOfRangeException("capacity");
		internal static void ThrowArgumentOutOfRangeException_memorySize() => throw new ArgumentOutOfRangeException("memorySize");
		internal static void ThrowArgumentOutOfRangeException_size() => throw new ArgumentOutOfRangeException("size");
		internal static void ThrowArgumentOutOfRangeException_elementSize() => throw new ArgumentOutOfRangeException("elementSize");
		internal static void ThrowArgumentOutOfRangeException_register() => throw new ArgumentOutOfRangeException("register");
	}
}
