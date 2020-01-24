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
using System.Linq;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class DecoderTestCases {
		public static readonly DecoderTestCase[] TestCases16 = ReadTestCases(16);
		public static readonly DecoderTestCase[] TestCases32 = ReadTestCases(32);
		public static readonly DecoderTestCase[] TestCases64 = ReadTestCases(64);
		public static readonly DecoderTestCase[] TestCasesMisc16 = ReadMiscTestCases(16);
		public static readonly DecoderTestCase[] TestCasesMisc32 = ReadMiscTestCases(32);
		public static readonly DecoderTestCase[] TestCasesMisc64 = ReadMiscTestCases(64);
		public static readonly DecoderMemoryTestCase[] TestCasesMemory16 = ReadMemoryTestCases(16);
		public static readonly DecoderMemoryTestCase[] TestCasesMemory32 = ReadMemoryTestCases(32);
		public static readonly DecoderMemoryTestCase[] TestCasesMemory64 = ReadMemoryTestCases(64);

		public static DecoderTestCase[] GetTestCases(int bitness) =>
			bitness switch {
				16 => TestCases16,
				32 => TestCases32,
				64 => TestCases64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};

		public static DecoderTestCase[] GetMiscTestCases(int bitness) =>
			bitness switch {
				16 => TestCasesMisc16,
				32 => TestCasesMisc32,
				64 => TestCasesMisc64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};

		public static DecoderMemoryTestCase[] GetMemoryTestCases(int bitness) =>
			bitness switch {
				16 => TestCasesMemory16,
				32 => TestCasesMemory32,
				64 => TestCasesMemory64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};

		static DecoderTestCase[] ReadTestCases(int bitness) {
			var filename = PathUtils.GetTestTextFilename($"DecoderTest{bitness}.txt", "Decoder");
			return DecoderTestParser.ReadFile(bitness, filename).ToArray();
		}

		static DecoderTestCase[] ReadMiscTestCases(int bitness) {
			var filename = PathUtils.GetTestTextFilename($"DecoderTestMisc{bitness}.txt", "Decoder");
			return DecoderTestParser.ReadFile(bitness, filename).ToArray();
		}

		static DecoderMemoryTestCase[] ReadMemoryTestCases(int bitness) {
			var filename = PathUtils.GetTestTextFilename($"MemoryTest{bitness}.txt", "Decoder");
			return MemoryDecoderTestParser.ReadFile(bitness, filename).ToArray();
		}
	}
}
