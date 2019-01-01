/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class DecoderTestCases {
		public static readonly DecoderTestCase[] TestCases16 = ReadTestCases(16);
		public static readonly DecoderTestCase[] TestCases32 = ReadTestCases(32);
		public static readonly DecoderTestCase[] TestCases64 = ReadTestCases(64);

		public static DecoderTestCase[] GetTestCases(int bitness) {
			switch (bitness) {
			case 16: return TestCases16;
			case 32: return TestCases32;
			case 64: return TestCases64;
			default: throw new ArgumentOutOfRangeException(nameof(bitness));
			}
		}

		static DecoderTestCase[] ReadTestCases(int bitness) {
			var filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Intel", "DecoderTests", $"DecoderTest{bitness}.txt");
			return DecoderTestParser.ReadFile(bitness, filename).ToArray();
		}
	}
}
