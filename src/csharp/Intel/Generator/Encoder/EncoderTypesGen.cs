// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
		public EnumType? LegacyOpKind;
		public EnumType? VexOpKind;
		public EnumType? XopOpKind;
		public EnumType? EvexOpKind;
		public EnumType? MvexOpKind;

		readonly GenTypes genTypes;

		public EncoderTypesGen(GenTypes genTypes) => this.genTypes = genTypes;

		public void Generate() {
			GenerateImmSizes();
			GenerateLegacyOpKind();
			GenerateVexOpKind();
			GenerateXopOpKind();
			GenerateEvexOpKind();
			GenerateMvexOpKind();
			GenerateEncFlags1();
		}

		void GenerateLegacyOpKind() => LegacyOpKind = GenerateOpKind(TypeIds.LegacyOpKind, EncodingKind.Legacy);
		void GenerateVexOpKind() => VexOpKind = GenerateOpKind(TypeIds.VexOpKind, EncodingKind.VEX);
		void GenerateXopOpKind() => XopOpKind = GenerateOpKind(TypeIds.XopOpKind, EncodingKind.XOP);
		void GenerateEvexOpKind() => EvexOpKind = GenerateOpKind(TypeIds.EvexOpKind, EncodingKind.EVEX);
		void GenerateMvexOpKind() => MvexOpKind = GenerateOpKind(TypeIds.MvexOpKind, EncodingKind.MVEX);

		internal static OpCodeOperandKindDef[] GetDefs(GenTypes genTypes, EncodingKind encoding) {
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var opKindDefs = genTypes.GetObject<OpCodeOperandKindDefs>(TypeIds.OpCodeOperandKindDefs).Defs;
			var none = opKindDefs.Single(a => a.OperandEncoding == OperandEncoding.None);
			return defs.
				Where(a => a.Encoding == encoding).
				SelectMany(a => a.OpKindDefs).
				Append(none).
				Distinct().
				OrderBy(a => a.EnumValue.Value).
				ToArray();
		}

		EnumType GenerateOpKind(TypeId typeId, EncodingKind encoding) {
			var values = GetDefs(genTypes, encoding).Select(a => new EnumValue(0, a.EnumValue.RawName, default)).ToArray();
			return new EnumType(typeId, default, values, EnumTypeFlags.None);
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

		static uint GenerateOpKindFields(List<EnumValue> fields, EnumType? opKindEnumType, string fieldPrefix, int numOps) {
			if (opKindEnumType is null)
				throw new InvalidOperationException();
			uint bit = 0;

			var opKind = ConstantUtils.GetMaskBits(opKindEnumType.Values.Max(a => a.Value));
			fields.Add(new EnumValue(opKind.mask, $"{fieldPrefix}OpMask", default));
			for (int i = 0; i < numOps; i++) {
				fields.Add(new EnumValue(bit, $"{fieldPrefix}Op{i}Shift", default));
				bit += opKind.bits;
			}
			if (bit > 32)
				throw new InvalidOperationException();
			return bit;
		}

		void GenerateEncFlags1() {
			var values = new List<EnumValue> {
				new EnumValue(0, "None", default)
			};

			uint maxBits = 0;
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, LegacyOpKind, "Legacy_", 4));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, VexOpKind, "VEX_", 5));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, XopOpKind, "XOP_", 4));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, EvexOpKind, "EVEX_", 4));
			maxBits = Math.Max(maxBits, GenerateOpKindFields(values, MvexOpKind, "MVEX_", 4));

			if (maxBits > 30)
				throw new InvalidOperationException();
			values.Add(new EnumValue(0x40000000, nameof(InstructionDefFlags3.IgnoresRoundingControl), default));
			values.Add(new EnumValue(0x80000000, nameof(InstructionDefFlags3.AmdLockRegBit), default));

			EncFlags1 = new EnumType(TypeIds.EncFlags1, default, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}
	}
}
