// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics.CodeAnalysis;

namespace Iced.Intel {
	static class ThrowHelper {
		// NOTE: NoInlining is not used because RyuJIT doesn't move the method call to the end of the caller's method

		[DoesNotReturn] internal static void ThrowArgumentException() => throw new ArgumentException();

		[DoesNotReturn] internal static void ThrowInvalidOperationException() => throw new InvalidOperationException();

		[DoesNotReturn] internal static void ThrowArgumentNullException_codeWriter() => throw new ArgumentNullException("codeWriter");
		[DoesNotReturn] internal static void ThrowArgumentNullException_data() => throw new ArgumentNullException("data");
		[DoesNotReturn] internal static void ThrowArgumentNullException_writer() => throw new ArgumentNullException("writer");
		[DoesNotReturn] internal static void ThrowArgumentNullException_options() => throw new ArgumentNullException("options");
		[DoesNotReturn] internal static void ThrowArgumentNullException_value() => throw new ArgumentNullException("value");
		[DoesNotReturn] internal static void ThrowArgumentNullException_list() => throw new ArgumentNullException("list");
		[DoesNotReturn] internal static void ThrowArgumentNullException_collection() => throw new ArgumentNullException("collection");
		[DoesNotReturn] internal static void ThrowArgumentNullException_array() => throw new ArgumentNullException("array");
		[DoesNotReturn] internal static void ThrowArgumentNullException_sb() => throw new ArgumentNullException("sb");
		[DoesNotReturn] internal static void ThrowArgumentNullException_output() => throw new ArgumentNullException("output");

		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_value() => throw new ArgumentOutOfRangeException("value");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_index() => throw new ArgumentOutOfRangeException("index");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_count() => throw new ArgumentOutOfRangeException("count");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_length() => throw new ArgumentOutOfRangeException("length");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_operand() => throw new ArgumentOutOfRangeException("operand");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_instructionOperand() => throw new ArgumentOutOfRangeException("instructionOperand");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_capacity() => throw new ArgumentOutOfRangeException("capacity");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_memorySize() => throw new ArgumentOutOfRangeException("memorySize");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_size() => throw new ArgumentOutOfRangeException("size");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_elementSize() => throw new ArgumentOutOfRangeException("elementSize");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_register() => throw new ArgumentOutOfRangeException("register");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_code() => throw new ArgumentOutOfRangeException("code");
		[DoesNotReturn] internal static void ThrowArgumentOutOfRangeException_data() => throw new ArgumentOutOfRangeException("data");
	}
}
