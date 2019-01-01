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

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MemoryTest32_001 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test32_DecodeMemOps_as32_Data))]
		void Test32_DecodeMemOps_as32(string hexBytes, Code code, Register register, Register prefixSeg, Register segReg, Register baseReg, Register indexReg, int scale, uint displ, int displSize) =>
			DecodeMemOpsBase(32, hexBytes, code, register, prefixSeg, segReg, baseReg, indexReg, scale, displ, displSize);
		public static IEnumerable<object[]> Test32_DecodeMemOps_as32_Data => GetMemOpsData(nameof(MemoryTest32_001));
	}
}
