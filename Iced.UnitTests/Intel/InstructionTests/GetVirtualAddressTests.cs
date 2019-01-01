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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	sealed class VARegisterValueProviderImpl : IVARegisterValueProvider {
		readonly (Register register, int elementIndex, int elementSize, ulong value)[] results;

		public VARegisterValueProviderImpl(params (Register register, ulong value)[] results) =>
			this.results = results.Select(a => (a.register, 0, 0, a.value)).ToArray();

		public VARegisterValueProviderImpl(params (Register register, int elementIndex, int elementSize, ulong value)[] results) =>
			this.results = results;

		public ulong GetRegisterValue(Register register, int elementIndex, int elementSize) {
			var results = this.results;
			for (int i = 0; i < results.Length; i++) {
				ref var info = ref results[i];
				if (info.register != register || info.elementIndex != elementIndex || info.elementSize != elementSize)
					continue;
				return info.value;
			}
			Assert.True(false);
			throw new InvalidOperationException();
		}
	}

	public abstract class GetVirtualAddressTests {
		private protected void TestBase(int bitness, string hexBytes, int operand, ulong expectedValue, VARegisterValueProviderImpl getRegValue) =>
			TestBase(bitness, hexBytes, operand, 0, expectedValue, getRegValue);

		private protected void TestBase(int bitness, string hexBytes, int operand, int elementIndex, ulong expectedValue, VARegisterValueProviderImpl getRegValue) {
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(hexBytes));
			switch (bitness) {
			case 16:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP16;
				break;
			case 32:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP32;
				break;
			case 64:
				decoder.InstructionPointer = DecoderConstants.DEFAULT_IP64;
				break;
			default:
				throw new InvalidOperationException();
			}
			var instr = decoder.Decode();
			ulong value1 = instr.GetVirtualAddress(operand, elementIndex, getRegValue);
			Assert.Equal(expectedValue, value1);
			ulong value2 = instr.GetVirtualAddress(operand, elementIndex, (register, elementIndex2, elementSize) =>
				getRegValue.GetRegisterValue(register, elementIndex2, elementSize));
			Assert.Equal(expectedValue, value2);
		}
	}
}
