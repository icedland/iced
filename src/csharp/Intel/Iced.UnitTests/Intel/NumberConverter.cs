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
using SG = System.Globalization;

namespace Iced.UnitTests.Intel {
	static class NumberConverter {
		public static ulong ToUInt64(string value) {
			if (value.StartsWith("0x")) {
				value = value.Substring(2);
				if (ulong.TryParse(value, SG.NumberStyles.HexNumber, null, out var number))
					return number;
			}
			else if (ulong.TryParse(value, out var number))
				return number;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static long ToInt64(string value) {
			if (value.StartsWith("0x")) {
				value = value.Substring(2);
				if (long.TryParse(value, SG.NumberStyles.HexNumber, null, out var number))
					return number;
			}
			else if (long.TryParse(value, out var number))
				return number;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static uint ToUInt32(string value) {
			ulong v = ToUInt64(value);
			if (v <= uint.MaxValue)
				return (uint)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static int ToInt32(string value) {
			long v = ToInt64(value);
			if (int.MinValue <= v && v <= int.MaxValue)
				return (int)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static ushort ToUInt16(string value) {
			ulong v = ToUInt64(value);
			if (v <= ushort.MaxValue)
				return (ushort)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static byte ToUInt8(string value) {
			ulong v = ToUInt64(value);
			if (v <= byte.MaxValue)
				return (byte)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static bool ToBoolean(string value) =>
			value switch {
				"true" => true,
				"false" => false,
				_ => throw new InvalidOperationException($"Invalid boolean: '{value}'"),
			};
	}
}
