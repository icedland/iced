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
using System.Linq;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Tables;

namespace Generator.Encoder {
	sealed class EncoderTypesGen {
		public (EnumValue value, uint size)[]? ImmSizes;
		public EnumType? EncFlags1;

		readonly GenTypes genTypes;

		public EncoderTypesGen(GenTypes genTypes) =>
			this.genTypes = genTypes;

		public void Generate() {
			GenerateImmSizes();
			GenerateEncFlags1();
		}

		void GenerateImmSizes() {
			var isEnum = genTypes[TypeIds.ImmSize];
			var immSizes = new (EnumValue value, uint size)[] {
				(isEnum[nameof(ImmSize.None)], 0),
				(isEnum[nameof(ImmSize.Size1)], 1),
				(isEnum[nameof(ImmSize.Size2)], 2),
				(isEnum[nameof(ImmSize.Size4)], 4),
				(isEnum[nameof(ImmSize.Size8)], 8),
				(isEnum[nameof(ImmSize.Size2_1)], 2 + 1),
				(isEnum[nameof(ImmSize.Size1_1)], 1 + 1),
				(isEnum[nameof(ImmSize.Size2_2)], 2 + 2),
				(isEnum[nameof(ImmSize.Size4_2)], 4 + 2),
				(isEnum[nameof(ImmSize.RipRelSize1_Target16)], 1),
				(isEnum[nameof(ImmSize.RipRelSize1_Target32)], 1),
				(isEnum[nameof(ImmSize.RipRelSize1_Target64)], 1),
				(isEnum[nameof(ImmSize.RipRelSize2_Target16)], 2),
				(isEnum[nameof(ImmSize.RipRelSize2_Target32)], 2),
				(isEnum[nameof(ImmSize.RipRelSize2_Target64)], 2),
				(isEnum[nameof(ImmSize.RipRelSize4_Target32)], 4),
				(isEnum[nameof(ImmSize.RipRelSize4_Target64)], 4),
				(isEnum[nameof(ImmSize.SizeIbReg)], 1),
				(isEnum[nameof(ImmSize.Size1OpCode)], 1),
			};
			Array.Sort(immSizes, (a, b) => a.value.Value.CompareTo(b.value.Value));
			if (immSizes.Length != isEnum.Values.Length)
				throw new InvalidOperationException();
			ImmSizes = immSizes;
		}

		static uint GenerateOpKindFields(List<EnumValue> fields, EnumType opKindEnumType, string fieldPrefix, int numOps) {
			uint bit = 0;

			var opKind = ConstantUtils.GetMaskBits(opKindEnumType.Values.Max(a => a.Value));
			fields.Add(new EnumValue(opKind.mask, $"{fieldPrefix}OpMask", null));
			for (int i = 0; i < numOps; i++) {
				fields.Add(new EnumValue(bit, $"{fieldPrefix}Op{i}Shift", null));
				bit += opKind.bits;
			}
			if (bit > 32)
				throw new InvalidOperationException();
			return bit;
		}

		void GenerateEncFlags1() {
			var values = new List<EnumValue>();
			values.Add(new EnumValue(0, "None", null));

			uint maxBits = 0;
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, genTypes[TypeIds.LegacyOpKind], "Legacy_", 4));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, genTypes[TypeIds.VexOpKind], "VEX_", 5));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, genTypes[TypeIds.XopOpKind], "XOP_", 4));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, genTypes[TypeIds.EvexOpKind], "EVEX_", 4));

			if (maxBits > 30)
				throw new InvalidOperationException();
			values.Add(new EnumValue(0x40000000, nameof(InstructionDefFlags3.IgnoresRoundingControl), null));
			values.Add(new EnumValue(0x80000000, nameof(InstructionDefFlags3.AmdLockRegBit), null));

			EncFlags1 = new EnumType(TypeIds.EncFlags1, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}
	}
}
