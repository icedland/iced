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
