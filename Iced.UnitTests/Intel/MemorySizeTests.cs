/*
    Copyright (C) 2018 de4dot@gmail.com

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
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel {
	public sealed class MemorySizeTests {
		[Fact]
		void Verify_MemorySizeInfos() {
			var infos = MemorySizeExtensions.MemorySizeInfos;
			for (int i = 0; i < infos.Length; i++)
				Assert.Equal((MemorySize)i, infos[i].MemorySize);
		}

		[Fact]
		void GetInfo_throws_if_invalid_value() {
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(-1)).GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => ((MemorySize)(Iced.Intel.DecoderConstants.NumberOfMemorySizes)).GetInfo());
		}

		[Theory]
		[MemberData(nameof(Verify_FirstBroadcastMemorySize_value_Data))]
		void Verify_FirstBroadcastMemorySize_value(MemorySize memorySize) {
			Assert.Equal(memorySize.IsBroadcast(), memorySize.GetInfo().IsBroadcast);
		}
		public static IEnumerable<object[]> Verify_FirstBroadcastMemorySize_value_Data {
			get {
				for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfMemorySizes; i++) {
					var memorySize = (MemorySize)i;
					yield return new object[] { (MemorySize)i };
				}
			}
		}
	}
}
