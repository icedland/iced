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
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	sealed class VARegisterValueProviderImpl : IVARegisterValueProvider {
		readonly (Register register, int elementIndex, int elementSize, ulong value)[] results;

		public VARegisterValueProviderImpl((Register register, int elementIndex, int elementSize, ulong value)[] results) =>
			this.results = results;

		public ulong GetRegisterValue(Register register, int elementIndex, int elementSize) {
			var results = this.results;
			for (int i = 0; i < results.Length; i++) {
				ref var info = ref results[i];
				if ((info.register, info.elementIndex, info.elementSize) == (register, elementIndex, elementSize))
					return info.value;
			}
			Assert.True(false);
			throw new InvalidOperationException();
		}
	}

	readonly struct VirtualAddressTestCase {
		public readonly int Bitness;
		public readonly string HexBytes;
		public readonly int Operand;
		public readonly int ElementIndex;
		public readonly ulong ExpectedValue;
		public readonly (Register register, int elementIndex, int elementSize, ulong value)[] RegisterValues;
		public VirtualAddressTestCase(int bitness, string hexBytes, int operand, int elementIndex, ulong expectedValue,
									(Register register, int elementIndex, int elementSize, ulong value)[] registerValues) {
			Bitness = bitness;
			HexBytes = hexBytes;
			Operand = operand;
			ElementIndex = elementIndex;
			ExpectedValue = expectedValue;
			RegisterValues = registerValues;
		}
	}

	public sealed class GetVirtualAddressTests {
		[Theory]
		[MemberData(nameof(VATests_Data))]
		void VATests(VirtualAddressTestCase tc) {
			var decoder = Decoder.Create(tc.Bitness, new ByteArrayCodeReader(tc.HexBytes));
			decoder.IP = tc.Bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};
			var instruction = decoder.Decode();
			var getRegValue = new VARegisterValueProviderImpl(tc.RegisterValues);
			ulong value1 = instruction.GetVirtualAddress(tc.Operand, tc.ElementIndex, getRegValue);
			Assert.Equal(tc.ExpectedValue, value1);
			ulong value2 = instruction.GetVirtualAddress(tc.Operand, tc.ElementIndex, (register, elementIndex2, elementSize) =>
				getRegValue.GetRegisterValue(register, elementIndex2, elementSize));
			Assert.Equal(tc.ExpectedValue, value2);
		}
		public static IEnumerable<object[]> VATests_Data {
			get {
				var filename = PathUtils.GetTestTextFilename("VirtualAddressTests.txt", "Instruction");
				foreach (var tc in VATestCaseReader.ReadFile(filename))
					yield return new object[] { tc };
			}
		}
	}
}
