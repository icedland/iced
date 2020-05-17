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
using System.Text;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;

namespace Generator.Encoder {
	sealed class EncoderTypesGen {
		public (EnumValue value, uint size)[]? ImmSizes;
		public EnumType? EncFlags1;
		public EnumType? LegacyFlags3;
		public EnumType? VexFlags3;
		public EnumType? XopFlags3;
		public EnumType? EvexFlags3;
		public EnumType? AllowedPrefixes;
		public Dictionary<OpCodeFlags, EnumValue>? AllowedPrefixesMap;
		public EnumType? LegacyFlags;
		public EnumType? VexFlags;
		public EnumType? XopFlags;
		public EnumType? EvexFlags;
		public EnumType? D3nowFlags;

		readonly GenTypes genTypes;

		public EncoderTypesGen(GenTypes genTypes) =>
			this.genTypes = genTypes;

		public void Generate() {
			GenerateImmSizes();
			GenerateEncFlags1();
			GenerateLegacyFlags3();
			GenerateVexFlags3();
			GenerateXopFlags3();
			GenerateEvexFlags3();
			GenerateAllowedPrefixes();
			GenerateLegacyFlags();
			GenerateVexFlags();
			GenerateXopFlags();
			GenerateEvexFlags();
			GenerateD3nowFlags();
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

		void GenerateEncFlags1() {
			var values = new List<EnumValue>();
			uint bit = 0;

			var encoding = ConstantUtils.GetMaskBits<EncodingKind>();
			values.Add(new EnumValue(bit, "EncodingShift", null));
			bit += encoding.bits;
			values.Add(new EnumValue(encoding.mask, "EncodingMask", null));

			const uint opCodeBit = 16;
			if (bit > opCodeBit)
				throw new InvalidOperationException();
			values.Add(new EnumValue(opCodeBit, "OpCodeShift", null));

			EncFlags1 = new EnumType(TypeIds.EncFlags1, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		static EnumType GenerateFlags3(EnumType opKindEnumType, TypeId flags3TypeId, int numOps) {
			var values = new List<EnumValue>();
			uint bit = 0;

			var opKind = ConstantUtils.GetMaskBits(opKindEnumType.Values.Max(a => a.Value));
			values.Add(new EnumValue(opKind.mask, "OpMask", null));
			for (int i = 0; i < numOps; i++) {
				values.Add(new EnumValue(bit, $"Op{i}Shift", null));
				bit += opKind.bits;
			}
			if (bit > 32)
				throw new InvalidOperationException();

			return new EnumType(flags3TypeId, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		void GenerateLegacyFlags3() => LegacyFlags3 = GenerateFlags3(genTypes[TypeIds.LegacyOpKind], TypeIds.LegacyFlags3, 4);
		void GenerateVexFlags3() => VexFlags3 = GenerateFlags3(genTypes[TypeIds.VexOpKind], TypeIds.VexFlags3, 5);
		void GenerateXopFlags3() => XopFlags3 = GenerateFlags3(genTypes[TypeIds.XopOpKind], TypeIds.XopFlags3, 4);
		void GenerateEvexFlags3() => EvexFlags3 = GenerateFlags3(genTypes[TypeIds.EvexOpKind], TypeIds.EvexFlags3, 4);

		internal const OpCodeFlags PrefixesMask =
			OpCodeFlags.LockPrefix | OpCodeFlags.XacquirePrefix | OpCodeFlags.XreleasePrefix | OpCodeFlags.RepPrefix |
			OpCodeFlags.RepnePrefix | OpCodeFlags.BndPrefix | OpCodeFlags.HintTakenPrefix | OpCodeFlags.NotrackPrefix;
		void GenerateAllowedPrefixes() {
			var maskHash = new HashSet<OpCodeFlags>();
			maskHash.Add(OpCodeFlags.None);
			foreach (var info in genTypes.GetObject<OpCodeInfoTable>(TypeIds.OpCodeInfoTable).Data) {
				maskHash.Add(info.Flags & PrefixesMask);
			}

			AllowedPrefixesMap = new Dictionary<OpCodeFlags, EnumValue>();
			var values = new EnumValue[maskHash.Count];
			var sb = new StringBuilder();
			int i = 0;
			foreach (var flags in maskHash) {
				var value = new EnumValue(0, GetName(sb, flags), null);
				values[i++] = value;
				AllowedPrefixesMap.Add(flags, value);
			}
			Array.Sort(values, (a, b) => {
				if (a == b)
					return 0;
				if (a.RawName == "None")
					return -1;
				if (b.RawName == "None")
					return 1;
				return StringComparer.Ordinal.Compare(a.RawName, b.RawName);
			});

			AllowedPrefixes = new EnumType(TypeIds.AllowedPrefixes, null, values, EnumTypeFlags.None);

			static string GetName(StringBuilder sb, OpCodeFlags flags) {
				if (flags == OpCodeFlags.None)
					return "None";
				sb.Clear();
				if ((flags & OpCodeFlags.RepPrefix) != 0) {
					flags &= ~OpCodeFlags.RepPrefix;
					sb.Append("Rep");
				}
				if ((flags & OpCodeFlags.RepnePrefix) != 0) {
					flags &= ~OpCodeFlags.RepnePrefix;
					sb.Append("Repne");
				}
				if ((flags & OpCodeFlags.HintTakenPrefix) != 0) {
					flags &= ~OpCodeFlags.HintTakenPrefix;
					sb.Append("HintTaken");
				}
				if ((flags & OpCodeFlags.BndPrefix) != 0) {
					flags &= ~OpCodeFlags.BndPrefix;
					sb.Append("Bnd");
				}
				if ((flags & OpCodeFlags.NotrackPrefix) != 0) {
					flags &= ~OpCodeFlags.NotrackPrefix;
					sb.Append("Notrack");
				}
				if ((flags & OpCodeFlags.XacquirePrefix) != 0) {
					flags &= ~OpCodeFlags.XacquirePrefix;
					sb.Append("Xacquire");
				}
				if ((flags & OpCodeFlags.XreleasePrefix) != 0) {
					flags &= ~OpCodeFlags.XreleasePrefix;
					sb.Append("Xrelease");
				}
				if ((flags & OpCodeFlags.LockPrefix) != 0) {
					flags &= ~OpCodeFlags.LockPrefix;
					sb.Append("Lock");
				}
				if (flags != OpCodeFlags.None)
					throw new InvalidOperationException();
				return sb.ToString();
			}
		}

		static void VerifyBit(uint bit) {
			if (bit > 32)
				throw new InvalidOperationException();
		}

		static void AddMaskShift<T>(List<EnumValue> values, ref uint bit, string maskName, string shiftName) where T : Enum {
			var info = ConstantUtils.GetMaskBits<T>();
			AddMaskShift(values, ref bit, maskName, shiftName, info.mask, info.bits);
		}

		static void AddMaskShift(List<EnumValue> values, ref uint bit, string maskName, string shiftName, uint mask, uint bits) {
			values.Add(new EnumValue(mask, maskName, null));
			values.Add(new EnumValue(bit, shiftName, null));
			bit += bits;
			VerifyBit(bit);
		}

		static void AddMandatoryPrefix(List<EnumValue> values, ref uint bit) =>
			AddMaskShift<MandatoryPrefixByte>(values, ref bit, "MandatoryPrefixByteMask", "MandatoryPrefixByteShift");

		static void AddEncodable(List<EnumValue> values, ref uint bit) =>
			AddMaskShift<Encodable>(values, ref bit, "EncodableMask", "EncodableShift");

		void AddAllowedPrefixes(List<EnumValue> values, ref uint bit) {
			var allowedPrefixes = AllowedPrefixes ?? throw new InvalidOperationException();
			var info = ConstantUtils.GetMaskBits(allowedPrefixes.Values.Max(a => a.Value));
			AddMaskShift(values, ref bit, "AllowedPrefixesMask", "AllowedPrefixesShift", info.mask, info.bits);
		}

		static void AddFlag(List<EnumValue> values, ref uint bit, string name) {
			values.Add(new EnumValue(1U << (int)bit, name, null));
			bit++;
			VerifyBit(bit);
		}

		static void AddMaskShift(List<EnumValue> values, ref uint bit, string name, uint bits) {
			values.Add(new EnumValue(bit, name, null));
			bit += bits;
			VerifyBit(bit);
		}

		void GenerateLegacyFlags() {
			var values = new List<EnumValue>();
			uint bit = 0;

			AddMandatoryPrefix(values, ref bit);
			AddMaskShift<LegacyOpCodeTable>(values, ref bit, "LegacyOpCodeTableMask", "LegacyOpCodeTableShift");
			AddEncodable(values, ref bit);
			AddFlag(values, ref bit, "HasGroupIndex");
			AddMaskShift(values, ref bit, "GroupShift", 3);// group index: 0-7
			AddAllowedPrefixes(values, ref bit);
			AddFlag(values, ref bit, "Fwait");
			AddFlag(values, ref bit, "HasMandatoryPrefix");
			AddMaskShift<OperandSize>(values, ref bit, "OperandSizeMask", "OperandSizeShift");
			AddMaskShift<AddressSize>(values, ref bit, "AddressSizeMask", "AddressSizeShift");

			VerifyBit(bit);
			LegacyFlags = new EnumType(TypeIds.LegacyFlags, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		void GenerateVexFlags() {
			var values = new List<EnumValue>();
			uint bit = 0;

			AddMandatoryPrefix(values, ref bit);
			AddMaskShift<VexOpCodeTable>(values, ref bit, "VexOpCodeTableMask", "VexOpCodeTableShift");
			AddEncodable(values, ref bit);
			AddFlag(values, ref bit, "HasGroupIndex");
			AddMaskShift(values, ref bit, "GroupShift", 3);// group index: 0-7
			AddMaskShift<VexVectorLength>(values, ref bit, "VexVectorLengthMask", "VexVectorLengthShift");
			AddMaskShift<WBit>(values, ref bit, "WBitMask", "WBitShift");

			VerifyBit(bit);
			VexFlags = new EnumType(TypeIds.VexFlags, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		void GenerateXopFlags() {
			var values = new List<EnumValue>();
			uint bit = 0;

			AddMandatoryPrefix(values, ref bit);
			AddMaskShift<XopOpCodeTable>(values, ref bit, "XopOpCodeTableMask", "XopOpCodeTableShift");
			AddEncodable(values, ref bit);
			AddFlag(values, ref bit, "HasGroupIndex");
			AddMaskShift(values, ref bit, "GroupShift", 3);// group index: 0-7
			AddMaskShift<XopVectorLength>(values, ref bit, "XopVectorLengthMask", "XopVectorLengthShift");
			AddMaskShift<WBit>(values, ref bit, "WBitMask", "WBitShift");

			VerifyBit(bit);
			XopFlags = new EnumType(TypeIds.XopFlags, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		void GenerateEvexFlags() {
			var values = new List<EnumValue>();
			uint bit = 0;

			AddMandatoryPrefix(values, ref bit);
			AddMaskShift<EvexOpCodeTable>(values, ref bit, "EvexOpCodeTableMask", "EvexOpCodeTableShift");
			AddEncodable(values, ref bit);
			AddFlag(values, ref bit, "HasGroupIndex");
			AddMaskShift(values, ref bit, "GroupShift", 3);// group index: 0-7
			AddMaskShift<EvexVectorLength>(values, ref bit, "EvexVectorLengthMask", "EvexVectorLengthShift");
			AddMaskShift<WBit>(values, ref bit, "WBitMask", "WBitShift");
			AddMaskShift<TupleType>(values, ref bit, "TupleTypeMask", "TupleTypeShift");
			AddFlag(values, ref bit, "LIG");
			AddFlag(values, ref bit, "b");
			AddFlag(values, ref bit, "er");
			AddFlag(values, ref bit, "sae");
			AddFlag(values, ref bit, "k1");
			AddFlag(values, ref bit, "z");
			AddFlag(values, ref bit, "NonZeroOpMaskRegister");

			VerifyBit(bit);
			EvexFlags = new EnumType(TypeIds.EvexFlags, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		void GenerateD3nowFlags() {
			var values = new List<EnumValue>();
			uint bit = 0;

			AddEncodable(values, ref bit);

			VerifyBit(bit);
			D3nowFlags = new EnumType(TypeIds.D3nowFlags, null, values.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}
	}
}
