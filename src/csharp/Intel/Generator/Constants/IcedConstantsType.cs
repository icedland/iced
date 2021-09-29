// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;

namespace Generator.Constants {
	static class IcedConstants {
		public const int MaxOpCount = 5;
		public const int MaxInstructionLength = 15;
		public const int RegisterBits = 8;

		public const string FirstBroadcastMemorySizeName = "FirstBroadcastMemorySize";

		static readonly Dictionary<TypeId, string> toEnumCountName = new();
		static bool toEnumCountNameInitd = false;
		static Dictionary<TypeId, string> GetEnumCountNameDict() {
			if (!toEnumCountNameInitd)
				throw new InvalidOperationException("Not initialized");
			return toEnumCountName;
		}

		internal static IEnumerable<KeyValuePair<TypeId, string>> GetEnumCountTypeIdsAndNames() => GetEnumCountNameDict();
		internal static void InitializeEnumCountTypes(GenTypes genTypes) {
			if (toEnumCountName.Count != 0 || toEnumCountNameInitd)
				throw new InvalidOperationException();
			foreach (var enumType in genTypes.AllEnumTypes) {
				if (!enumType.IsPublic || enumType.IsFlags)
					continue;
				if (!VerifyPublicEnumValues(enumType))
					throw new InvalidOperationException($"All values 0..max (exclusive) must be valid enum values");
				var rawName = enumType.RawName;
				string enumCountName;
				if (rawName.Contains('_', StringComparison.Ordinal))
					enumCountName = rawName + "_" + "EnumCount";
				else
					enumCountName = rawName + "EnumCount";
				toEnumCountName.Add(enumType.TypeId, enumCountName);
			}
			toEnumCountNameInitd = true;
		}

		// Lots of places (especially Rust code) assume there are no holes
		// in public non-flags enums, i.e., all values from 0 .. max are
		// assumed to be valid enum values. Verify it.
		static bool VerifyPublicEnumValues(EnumType enumType) {
			var values = enumType.Values;
			uint expectedValue = 0;
			for (int i = 0; i < values.Length; i++) {
				if (values[i].Value != expectedValue)
					return false;
				// Could be duplicate values if one is deprecated, skip them
				while (i + 1 < values.Length && values[i + 1].Value == expectedValue)
					i++;
				expectedValue++;
			}
			return true;
		}

		public static string GetEnumCountName(TypeId id) => GetEnumCountNameDict()[id];
	}

	[TypeGen(TypeGenOrders.Last)]
	sealed class IcedConstantsType : ITypeGen {
		readonly GenTypes genTypes;

		IcedConstantsType(GenTypes genTypes) => this.genTypes = genTypes;

		public void Generate(GenTypes genTypes) {
			IcedConstants.InitializeEnumCountTypes(genTypes);
			var type = new ConstantsType(TypeIds.IcedConstants, ConstantsTypeFlags.None, default, GetConstants());
			genTypes.Add(type);
		}

		Constant[] GetConstants() {
			var (mvexStart, mvexLen) = GetMvexCodeValueRange();
			var vmmFirst = Get_VMM_first().Value;
			var vmmLast = Get_VMM_last().Value;
			ConstantUtils.VerifyMask<Register>((1U << IcedConstants.RegisterBits) - 1);
			var constants = new List<Constant> {
				new Constant(ConstantKind.Index, nameof(IcedConstants.MaxOpCount), IcedConstants.MaxOpCount),
				new Constant(ConstantKind.Index, nameof(IcedConstants.MaxInstructionLength), IcedConstants.MaxInstructionLength),
				new Constant(ConstantKind.Int32, nameof(IcedConstants.RegisterBits), IcedConstants.RegisterBits),
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
				new Constant(ConstantKind.UInt32, "MvexStart", mvexStart),
				new Constant(ConstantKind.UInt32, "MvexLength", mvexLen),
			};

			foreach (var kv in IcedConstants.GetEnumCountTypeIdsAndNames().OrderBy(kv => kv.Value, StringComparer.Ordinal))
				constants.Add(new Constant(ConstantKind.Index, IcedConstants.GetEnumCountName(kv.Key), GetEnumCount(genTypes[kv.Key])));

			return constants.ToArray();
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

		(uint start, uint len) GetMvexCodeValueRange() {
			const string EncodingPrefix = "MVEX_";
			var codeValues = genTypes[TypeIds.Code].Values;
			int start = 0;
			while (start < codeValues.Length) {
				if (codeValues[start].RawName.StartsWith(EncodingPrefix, StringComparison.Ordinal))
					break;
				start++;
			}
			var end = start;
			while (end < codeValues.Length) {
				if (!codeValues[end].RawName.StartsWith(EncodingPrefix, StringComparison.Ordinal))
					break;
				end++;
			}
			for (int index = end; index < codeValues.Length; index++) {
				// All of them must be next to each other
				if (codeValues[index].RawName.StartsWith(EncodingPrefix, StringComparison.Ordinal))
					throw new InvalidOperationException();
			}
			if (end <= start)
				return (0, 0);
			return ((uint)start, (uint)(end - start));
		}
	}
}
