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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class MemorySizeExtensionsTests {
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
#endif
