// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using System.Linq;
using Generator.Enums;

namespace Generator.Constants {
	static class IcedConstants {
		public const int MaxOpCount = 5;
		public const int MaxInstructionLength = 15;
		public const int RegisterBits = 8;

		public const string CodeEnumCountName = "CodeEnumCount";
		public const string FirstBroadcastMemorySizeName = "FirstBroadcastMemorySize";
	}

	[TypeGen(TypeGenOrders.Last)]
	sealed class IcedConstantsType : ITypeGen {
		readonly GenTypes genTypes;

		IcedConstantsType(GenTypes genTypes) => this.genTypes = genTypes;

		public void Generate(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.IcedConstants, ConstantsTypeFlags.None, null, GetConstants());
			genTypes.Add(type);
		}

		Constant[] GetConstants() {
			var vmmFirst = Get_VMM_first().Value;
			var vmmLast = Get_VMM_last().Value;
			ConstantUtils.VerifyMask<Register>((1U << IcedConstants.RegisterBits) - 1);
			return new Constant[] {
				new Constant(ConstantKind.Index, nameof(IcedConstants.MaxOpCount), IcedConstants.MaxOpCount),
				new Constant(ConstantKind.Index, nameof(IcedConstants.MaxInstructionLength), IcedConstants.MaxInstructionLength),
				new Constant(ConstantKind.Int32, nameof(IcedConstants.RegisterBits), IcedConstants.RegisterBits),
				new Constant(ConstantKind.Index, IcedConstants.CodeEnumCountName, GetEnumCount(genTypes[TypeIds.Code])),
				new Constant(ConstantKind.Index, "RegisterEnumCount", GetEnumCount(genTypes[TypeIds.Register])),
				new Constant(ConstantKind.Index, "MemorySizeEnumCount", GetEnumCount(genTypes[TypeIds.MemorySize])),
				new Constant(ConstantKind.Index, "EncodingKindEnumCount", GetEnumCount(genTypes[TypeIds.EncodingKind])),
				new Constant(ConstantKind.Index, "OpKindEnumCount", GetEnumCount(genTypes[TypeIds.OpKind])),
				new Constant(ConstantKind.Index, "CodeSizeEnumCount", GetEnumCount(genTypes[TypeIds.CodeSize])),
				new Constant(ConstantKind.Index, "RoundingControlEnumCount", GetEnumCount(genTypes[TypeIds.RoundingControl])),
				new Constant(ConstantKind.Index, "MemorySizeOptionsEnumCount", GetEnumCount(genTypes[TypeIds.MemorySizeOptions])),
				new Constant(ConstantKind.Index, "CC_b_EnumCount", GetEnumCount(genTypes[TypeIds.CC_b])),
				new Constant(ConstantKind.Index, "CC_ae_EnumCount", GetEnumCount(genTypes[TypeIds.CC_ae])),
				new Constant(ConstantKind.Index, "CC_e_EnumCount", GetEnumCount(genTypes[TypeIds.CC_e])),
				new Constant(ConstantKind.Index, "CC_ne_EnumCount", GetEnumCount(genTypes[TypeIds.CC_ne])),
				new Constant(ConstantKind.Index, "CC_be_EnumCount", GetEnumCount(genTypes[TypeIds.CC_be])),
				new Constant(ConstantKind.Index, "CC_a_EnumCount", GetEnumCount(genTypes[TypeIds.CC_a])),
				new Constant(ConstantKind.Index, "CC_p_EnumCount", GetEnumCount(genTypes[TypeIds.CC_p])),
				new Constant(ConstantKind.Index, "CC_np_EnumCount", GetEnumCount(genTypes[TypeIds.CC_np])),
				new Constant(ConstantKind.Index, "CC_l_EnumCount", GetEnumCount(genTypes[TypeIds.CC_l])),
				new Constant(ConstantKind.Index, "CC_ge_EnumCount", GetEnumCount(genTypes[TypeIds.CC_ge])),
				new Constant(ConstantKind.Index, "CC_le_EnumCount", GetEnumCount(genTypes[TypeIds.CC_le])),
				new Constant(ConstantKind.Index, "CC_g_EnumCount", GetEnumCount(genTypes[TypeIds.CC_g])),
				new Constant(ConstantKind.Index, "RepPrefixKindEnumCount", GetEnumCount(genTypes[TypeIds.RepPrefixKind])),
				// This is the largest vector register. If it's VEX/EVEX, the upper bits are always cleared when writing to any sub reg, eg. YMM0
				new Constant(ConstantKind.Register, "VMM_first", vmmFirst),
				new Constant(ConstantKind.Register, "VMM_last", vmmLast),
				new Constant(ConstantKind.Int32, "VMM_count", vmmLast - vmmFirst + 1),
				new Constant(ConstantKind.Register, "XMM_last", Get_VEC_last("XMM").Value),
				new Constant(ConstantKind.Register, "YMM_last", Get_VEC_last("YMM").Value),
				new Constant(ConstantKind.Register, "ZMM_last", Get_VEC_last("ZMM").Value),
				new Constant(ConstantKind.Register, "TMM_last", Get_TMM_last().Value),
				new Constant(ConstantKind.Index, "MaxCpuidFeatureInternalValues", GetEnumCount(genTypes[TypeIds.CpuidFeatureInternal])),
				new Constant(ConstantKind.MemorySize, IcedConstants.FirstBroadcastMemorySizeName, GetFirstBroadcastMemorySize()),
			};
		}

		static uint GetEnumCount(EnumType enumType) =>
			(uint)enumType.Values.Where(a => !a.DeprecatedInfo.IsDeprecatedAndRenamed).Count();

		EnumValue Get_VMM_first() => Get_VMM_first(genTypes);
		EnumValue Get_VMM_last() => Get_VMM_last(genTypes);
		EnumValue Get_VEC_last(string prefix) => Get_VEC_last(genTypes, prefix);
		EnumValue Get_TMM_last() => Get_TMM_last(genTypes);

		const string VMM_prefix = "ZMM";
		const int vmmLastNum = 31;
		public static EnumValue Get_VMM_first(GenTypes genTypes) => genTypes[TypeIds.Register][VMM_prefix + "0"];
		public static EnumValue Get_VMM_last(GenTypes genTypes) => Get_VEC_last(genTypes, VMM_prefix);
		static EnumValue Get_VEC_last(GenTypes genTypes, string prefix) {
			var vmmLastStr = prefix + vmmLastNum.ToString();
			var vmmShouldNotExistStr = prefix + (vmmLastNum + 1).ToString();
			var vmmLast = genTypes[TypeIds.Register][vmmLastStr];
			if (genTypes[TypeIds.Register].Values.Any(a => a.RawName == vmmShouldNotExistStr))
				throw new InvalidOperationException($"Register {vmmShouldNotExistStr} exists so {vmmLast.RawName} isn't the last one");
			return vmmLast;
		}

		public static EnumValue Get_TMM_last(GenTypes genTypes) {
			int lastIndex = -1;
			EnumValue? tmm = null;
			var regs = genTypes[TypeIds.Register];
			const string TMMPrefix = "TMM";
			foreach (var reg in regs.Values) {
				if (!reg.RawName.StartsWith(TMMPrefix, StringComparison.Ordinal))
					continue;
				int index = int.Parse(reg.RawName.AsSpan()[TMMPrefix.Length..]);
				if (index > lastIndex) {
					lastIndex = index;
					tmm = reg;
				}
			}
			return tmm ?? throw new InvalidOperationException();
		}

		uint GetFirstBroadcastMemorySize() {
			var values = genTypes[TypeIds.MemorySize].Values;
			uint? firstBroadcastValue = null;
			for (int i = 0; i < values.Length; i++) {
				var name = values[i].RawName;
				bool isBroadcast = name.StartsWith("Broadcast", StringComparison.Ordinal);
				if (firstBroadcastValue is not null) {
					if (!isBroadcast)
						throw new InvalidOperationException("Must be sorted so that all broadcast memory types are at the end of the enum");
				}
				else {
					if (isBroadcast)
						firstBroadcastValue = (uint)i;
				}
			}
			return firstBroadcastValue ?? throw new InvalidOperationException("Couldn't find a broadcast memory type");
		}
	}
}
