// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;
using Iced.UnitTests.Intel.InstructionTests;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class GetVirtualAddressTests {
		[Theory]
		[MemberData(nameof(VATests_Data))]
		void VATests(VirtualAddressTestCase tc) {
			var decoder = Decoder.Create(tc.Bitness, new ByteArrayCodeReader(tc.HexBytes), tc.DecoderOptions);
			decoder.IP = tc.Bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};
			var instruction = decoder.Decode();
			var getRegValue = new VARegisterValueProviderImpl(tc.RegisterValues);
			var getRegValueFail = new VARegisterValueProviderImpl(Array.Empty<(Register register, int elementIndex, int elementSize, ulong value)>());

			var factory = new InstructionInfoFactory();
			var info = factory.GetInfo(instruction);
			var usedMem = info.GetUsedMemory().Skip(tc.UsedMemIndex).First();

			bool b1 = usedMem.TryGetVirtualAddress(tc.ElementIndex, getRegValue, out ulong value1);
			Assert.True(b1);
			Assert.Equal(tc.ExpectedValue, value1);

			bool b2 = usedMem.TryGetVirtualAddress(tc.ElementIndex, out ulong value2,
				(Register register, int elementIndex, int elementSize, out ulong value) =>
					getRegValue.TryGetRegisterValue(register, elementIndex, elementSize, out value));
			Assert.True(b2);
			Assert.Equal(tc.ExpectedValue, value2);

			ulong value3 = usedMem.GetVirtualAddress(tc.ElementIndex, getRegValue);
			Assert.Equal(tc.ExpectedValue, value3);

			ulong value4 = usedMem.GetVirtualAddress(tc.ElementIndex, (register, elementIndex2, elementSize) =>
				getRegValue.GetRegisterValue(register, elementIndex2, elementSize));
			Assert.Equal(tc.ExpectedValue, value4);

			Assert.False(usedMem.TryGetVirtualAddress(tc.ElementIndex, out ulong value5, (Register register, int elementIndex, int elementSize, out ulong value) => { value = 0; return false; }));
			Assert.Equal(0UL, value5);
			Assert.False(usedMem.TryGetVirtualAddress(tc.ElementIndex, getRegValueFail, out ulong value6));
			Assert.Equal(0UL, value6);
		}
		public static IEnumerable<object[]> VATests_Data {
			get {
				foreach (var tc in VirtualAddressTestCases.Tests) {
					if (tc.UsedMemIndex >= 0)
						yield return new object[] { tc };
				}
			}
		}
	}
}
#endif
