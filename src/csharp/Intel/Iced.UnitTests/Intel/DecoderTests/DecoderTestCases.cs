// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
