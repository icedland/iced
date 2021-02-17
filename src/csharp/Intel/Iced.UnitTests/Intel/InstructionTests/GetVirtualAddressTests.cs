// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	sealed class VARegisterValueProviderImpl : IVARegisterValueProvider, IVATryGetRegisterValueProvider {
		readonly (Register register, int elementIndex, int elementSize, ulong value)[] results;

		public VARegisterValueProviderImpl((Register register, int elementIndex, int elementSize, ulong value)[] results) =>
			this.results = results;

		public ulong GetRegisterValue(Register register, int elementIndex, int elementSize) {
			if (TryGetRegisterValue(register, elementIndex, elementSize, out var value))
				return value;
			throw new InvalidOperationException();
		}

		public bool TryGetRegisterValue(Register register, int elementIndex, int elementSize, out ulong value) {
			var results = this.results;
			for (int i = 0; i < results.Length; i++) {
				ref var info = ref results[i];
				if ((info.register, info.elementIndex, info.elementSize) == (register, elementIndex, elementSize)) {
					value = info.value;
					return true;
				}
			}
			value = 0;
			return false;
		}
	}

	readonly struct VirtualAddressTestCase {
		public readonly int Bitness;
		public readonly string HexBytes;
		public readonly DecoderOptions DecoderOptions;
		public readonly int Operand;
		public readonly int UsedMemIndex;
		public readonly int ElementIndex;
		public readonly ulong ExpectedValue;
		public readonly (Register register, int elementIndex, int elementSize, ulong value)[] RegisterValues;
		public VirtualAddressTestCase(int bitness, string hexBytes, DecoderOptions decoderOptions, int operand, int usedMemIndex, int elementIndex, ulong expectedValue,
									(Register register, int elementIndex, int elementSize, ulong value)[] registerValues) {
			Bitness = bitness;
			HexBytes = hexBytes;
			DecoderOptions = decoderOptions;
			Operand = operand;
			UsedMemIndex = usedMemIndex;
			ElementIndex = elementIndex;
			ExpectedValue = expectedValue;
			RegisterValues = registerValues;
		}
		public override string ToString() => $"{Bitness}, {HexBytes}, {Operand}, {UsedMemIndex}, {ElementIndex}, 0x{ExpectedValue:X}";
	}

	static class VirtualAddressTestCases {
		public static readonly VirtualAddressTestCase[] Tests = CreateTests();

		static VirtualAddressTestCase[] CreateTests() {
			var filename = PathUtils.GetTestTextFilename("VirtualAddressTests.txt", "Instruction");
			return VATestCaseReader.ReadFile(filename).ToArray();
		}
	}

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

			bool b1 = instruction.TryGetVirtualAddress(tc.Operand, tc.ElementIndex, getRegValue, out ulong value1);
			Assert.True(b1);
			Assert.Equal(tc.ExpectedValue, value1);

			bool b2 = instruction.TryGetVirtualAddress(tc.Operand, tc.ElementIndex, out ulong value2,
				(Register register, int elementIndex, int elementSize, out ulong value) =>
					getRegValue.TryGetRegisterValue(register, elementIndex, elementSize, out value));
			Assert.True(b2);
			Assert.Equal(tc.ExpectedValue, value2);

			ulong value3 = instruction.GetVirtualAddress(tc.Operand, tc.ElementIndex, getRegValue);
			Assert.Equal(tc.ExpectedValue, value3);

			ulong value4 = instruction.GetVirtualAddress(tc.Operand, tc.ElementIndex, (register, elementIndex2, elementSize) =>
				getRegValue.GetRegisterValue(register, elementIndex2, elementSize));
			Assert.Equal(tc.ExpectedValue, value4);

			Assert.False(instruction.TryGetVirtualAddress(tc.Operand, tc.ElementIndex, out ulong value5, (Register register, int elementIndex, int elementSize, out ulong value) => { value = 0; return false; }));
			Assert.Equal(0UL, value5);
			Assert.False(instruction.TryGetVirtualAddress(tc.Operand, tc.ElementIndex, getRegValueFail, out ulong value6));
			Assert.Equal(0UL, value6);
		}
		public static IEnumerable<object[]> VATests_Data {
			get {
				foreach (var tc in VirtualAddressTestCases.Tests) {
					if (tc.Operand >= 0)
						yield return new object[] { tc };
				}
			}
		}
	}
}
