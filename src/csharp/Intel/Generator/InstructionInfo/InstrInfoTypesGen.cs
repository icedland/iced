// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Constants.InstructionInfo;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Tables;

namespace Generator.InstructionInfo {
	[Flags]
	enum InfoFlags1 : uint {
		// OpInfo*Shift and OpInfo*Mask (* = 0..4) are added here

		// Free bits

		FirstUsedBit				= RflagsInfoShift,
		RflagsInfoShift				= 13,
		RflagsInfoMask				= 0x7F,
		ImpliedAccessShift			= 20,
		ImpliedAccessMask			= 0xFF,
		// Free bits
		IgnoresIndexVA				= 0x20000000,
		OpMaskReadWrite				= 0x40000000,
		IgnoresSegment				= 0x80000000,
	}

	[Flags]
	enum InfoFlags2 : uint {
		EncodingShift				= 0,
		EncodingMask				= 7,

		// Free bits

		SaveRestore					= 0x00020000,
		StackInstruction			= 0x00040000,
		Privileged					= 0x00080000,
		FlowControlShift			= 20,
		FlowControlMask				= 0xF,
		CpuidFeatureInternalShift	= 24,
		CpuidFeatureInternalMask	= 0xFF,
	}

	sealed class InstrInfoTypesGen {
		public EnumType? EnumRflagsInfo;
		public EnumType[]? EnumOpInfos;
		public EnumType? EnumInfoFlags1;
		public EnumType? EnumInfoFlags2;
		public EnumType? EnumCpuidFeatureInternal;
		public ConstantsType? InstrInfoConstants;
		public (EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[]? CpuidFeatures;
		public (EnumValue value, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) rflags)[]? RflagsInfos;
		readonly InstructionDef[] defs;

		readonly GenTypes genTypes;

		public InstrInfoTypesGen(GenTypes genTypes) {
			this.genTypes = genTypes;
			defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
		}

		public void Generate() {
			GenerateCpuidFeatureInternal();
			GenerateRflagsInfo();
			GenerateOpInfoX();
			GenerateInfoFlags();
			GenerateInstrInfoConstants();
		}

		sealed class EnumValueArrayComparer : IEqualityComparer<EnumValue[]> {
			public bool Equals(EnumValue[]? x, EnumValue[]? y) {
				if (x == y)
					return true;
				if (x is null || y is null)
					return false;
				if (x.Length != y.Length)
					return false;
				for (int i = 0; i < x.Length; i++) {
					if (x[i] != y[i])
						return false;
				}
				return true;
			}

			public int GetHashCode(EnumValue[]? obj) {
				if (obj is null)
					return 0;
				int hc = 0;
				foreach (var enumValue in obj)
					hc = HashCode.Combine(hc, enumValue.GetHashCode());
				return hc;
			}
		}

		static void Add(Dictionary<EnumValue[], EnumValue> cpuidToInternalDict, List<(EnumValue cpuidInternal, EnumValue[] cpuidFeatures)> cpuidFeatures, EnumValue[] cpuid) {
			if (!cpuidToInternalDict.ContainsKey(cpuid)) {
				var name = string.Join("_and_", cpuid.Select(a => a.RawName));
				var internalEnumValue = new EnumValue(0, name, default);
				cpuidFeatures.Add((internalEnumValue, cpuid));
				cpuidToInternalDict.Add(cpuid, internalEnumValue);
			}
		}

		void GenerateCpuidFeatureInternal() {
			var cpuidToInternalDict = new Dictionary<EnumValue[], EnumValue>(new EnumValueArrayComparer());
			var cpuidFeatures = new List<(EnumValue cpuidInternal, EnumValue[] cpuidFeatures)>();
			foreach (var def in defs)
				Add(cpuidToInternalDict, cpuidFeatures, def.Cpuid);

			cpuidFeatures.Sort(CompareCpuidInternalEnums);

			EnumCpuidFeatureInternal = new EnumType(TypeIds.CpuidFeatureInternal, default, cpuidFeatures.Select(a => a.cpuidInternal).ToArray(), EnumTypeFlags.None);
			CpuidFeatures = cpuidFeatures.ToArray();
			foreach (var def in defs) {
				var info = def;
				info.CpuidInternal = cpuidToInternalDict[info.Cpuid];
			}
		}

		static int CompareCpuidInternalEnums((EnumValue cpuidInternal, EnumValue[] cpuidFeatures) x, (EnumValue cpuidInternal, EnumValue[] cpuidFeatures) y) {
			int c = CompareCpuidFeatures(x.cpuidFeatures, y.cpuidFeatures);
			if (c != 0)
				return c;
			return StringComparer.Ordinal.Compare(x.cpuidInternal.RawName, y.cpuidInternal.RawName);
		}

		static int CompareCpuidFeatures(EnumValue[] ca, EnumValue[] cb) {
			ca = ca.OrderByDescending(a => a.Value).ToArray();
			cb = cb.OrderByDescending(a => a.Value).ToArray();
			int count = Math.Max(ca.Length, cb.Length);
			for (int i = 0; i < count; i++) {
				uint a = i < ca.Length ? ca[i].Value + 1 : 0;
				uint b = i < cb.Length ? cb[i].Value + 1 : 0;
				int c = a.CompareTo(b);
				if (c != 0)
					return c;
			}
			return 0;
		}

		const string RflagsInfo_None = "None";
		void GenerateRflagsInfo() {
			var rflagsHashSet = new HashSet<(RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set)> {
				// None must always be present
				default,
				// Needed by ImpliedAccess.Clear_rflags (xor)
				(RflagsBits.None, RflagsBits.AF, RflagsBits.None, RflagsBits.CF | RflagsBits.OF | RflagsBits.SF, RflagsBits.PF | RflagsBits.ZF),
				// Needed by ImpliedAccess.Clear_rflags (sub)
				(RflagsBits.None, RflagsBits.None, RflagsBits.None, RflagsBits.AF | RflagsBits.CF | RflagsBits.OF | RflagsBits.SF, RflagsBits.PF | RflagsBits.ZF),
			};
			foreach (var def in defs)
				rflagsHashSet.Add((def.RflagsRead, def.RflagsUndefined, def.RflagsWritten, def.RflagsCleared, def.RflagsSet));
			var rflags = new List<(RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set)>(rflagsHashSet);
			rflags.Sort((a, b) => Compare(a, b));
			// None must be first
			if (rflags[0] != default)
				throw new InvalidOperationException();
			var sb = new StringBuilder();
			RflagsInfos = new (EnumValue value, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) rflags)[rflags.Count];
			var toRflagsInfo = new Dictionary<(RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set), EnumValue>();
			for (int i = 0; i < RflagsInfos.Length; i++) {
				var info = rflags[i];
				var value = new EnumValue(0, GetName(sb, info), default);
				RflagsInfos[i] = (value, info);
				toRflagsInfo[info] = value;
			}
			Array.Sort(RflagsInfos, (a, b) => Compare(a.value, b.value));
			// None must be first
			if (RflagsInfos[0].rflags != default)
				throw new InvalidOperationException();
			var values = new EnumValue[rflags.Count];
			for (int i = 0; i < values.Length; i++)
				values[i] = RflagsInfos[i].value;
			if (values.Select(a => a.RawName).Distinct(StringComparer.Ordinal).Count() != values.Length)
				throw new InvalidOperationException("Dupe names");
			foreach (var def in defs)
				def.RflagsInfo = toRflagsInfo[(def.RflagsRead, def.RflagsUndefined, def.RflagsWritten, def.RflagsCleared, def.RflagsSet)];
			EnumRflagsInfo = new EnumType(TypeIds.RflagsInfo, default, values, EnumTypeFlags.None);
		}

		static int Compare(EnumValue a, EnumValue b) {
			if (a.RawName == RflagsInfo_None)
				return -1;
			if (b.RawName == RflagsInfo_None)
				return 1;
			return StringComparer.Ordinal.Compare(a.RawName, b.RawName);
		}

		static string GetName(StringBuilder sb, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) flags) {
			sb.Clear();

			Add(sb, "R", flags.read);
			Add(sb, "W", flags.written);
			Add(sb, "C", flags.cleared);
			Add(sb, "S", flags.set);
			Add(sb, "U", flags.undefined);

			if (sb.Length == 0) {
				if (flags != default)
					throw new InvalidOperationException();
				return RflagsInfo_None;
			}
			return sb.ToString();
		}

		static void Add(StringBuilder sb, string prefix, RflagsBits rflags) {
			if (rflags == RflagsBits.None)
				return;
			if (sb.Length > 0)
				sb.Append('_');
			sb.Append(prefix);
			sb.Append('_');
			if ((rflags & RflagsBits.AF) != 0) { sb.Append(RflagsBitsConstants.AF); rflags &= ~RflagsBits.AF; }
			if ((rflags & RflagsBits.CF) != 0) { sb.Append(RflagsBitsConstants.CF); rflags &= ~RflagsBits.CF; }
			if ((rflags & RflagsBits.OF) != 0) { sb.Append(RflagsBitsConstants.OF); rflags &= ~RflagsBits.OF; }
			if ((rflags & RflagsBits.PF) != 0) { sb.Append(RflagsBitsConstants.PF); rflags &= ~RflagsBits.PF; }
			if ((rflags & RflagsBits.SF) != 0) { sb.Append(RflagsBitsConstants.SF); rflags &= ~RflagsBits.SF; }
			if ((rflags & RflagsBits.ZF) != 0) { sb.Append(RflagsBitsConstants.ZF); rflags &= ~RflagsBits.ZF; }
			if ((rflags & RflagsBits.IF) != 0) { sb.Append(RflagsBitsConstants.IF); rflags &= ~RflagsBits.IF; }
			if ((rflags & RflagsBits.DF) != 0) { sb.Append(RflagsBitsConstants.DF); rflags &= ~RflagsBits.DF; }
			if ((rflags & RflagsBits.AC) != 0) { sb.Append(RflagsBitsConstants.AC); rflags &= ~RflagsBits.AC; }
			if ((rflags & RflagsBits.C0) != 0) { sb.Append(RflagsBitsConstants.C0); rflags &= ~RflagsBits.C0; }
			if ((rflags & RflagsBits.C1) != 0) { sb.Append(RflagsBitsConstants.C1); rflags &= ~RflagsBits.C1; }
			if ((rflags & RflagsBits.C2) != 0) { sb.Append(RflagsBitsConstants.C2); rflags &= ~RflagsBits.C2; }
			if ((rflags & RflagsBits.C3) != 0) { sb.Append(RflagsBitsConstants.C3); rflags &= ~RflagsBits.C3; }
			if ((rflags & RflagsBits.UIF) != 0) { sb.Append(RflagsBitsConstants.UIF); rflags &= ~RflagsBits.UIF; }
			if (rflags != RflagsBits.None)
				throw new InvalidOperationException();
		}

		static int Compare((RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) a, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) b) {
			int c;
			if ((c = Compare(a.read, b.read)) != 0) return c;
			if ((c = Compare(a.written, b.written)) != 0) return c;
			if ((c = Compare(a.cleared, b.cleared)) != 0) return c;
			if ((c = Compare(a.set, b.set)) != 0) return c;
			if ((c = Compare(a.undefined, b.undefined)) != 0) return c;
			return 0;
		}

		static int Compare(RflagsBits a, RflagsBits b) =>
			((uint)a).CompareTo((uint)b);

		void GenerateOpInfoX() {
			var opInfoHashes = new HashSet<OpInfo>[IcedConstants.MaxOpCount];
			for (int i = 0; i < opInfoHashes.Length; i++)
				opInfoHashes[i] = new HashSet<OpInfo>();
			foreach (var def in defs) {
				bool foundNone = false;
				int i;
				for (i = 0; i < def.OpInfo.Length; i++) {
					var opInfo = def.OpInfo[i];
					if (opInfo == OpInfo.None)
						foundNone = true;
					else if (foundNone)
						throw new InvalidOperationException();
					opInfoHashes[i].Add(opInfo);
				}
				for (; i < opInfoHashes.Length; i++)
					opInfoHashes[i].Add(OpInfo.None);
			}

			// Referenced by code in InstructionInfoFactory
			opInfoHashes[0].Add(OpInfo.None);
			opInfoHashes[0].Add(OpInfo.CondWrite);
			opInfoHashes[0].Add(OpInfo.CondWrite32_ReadWrite64);
			opInfoHashes[0].Add(OpInfo.NoMemAccess);
			opInfoHashes[0].Add(OpInfo.Read);
			opInfoHashes[0].Add(OpInfo.ReadCondWrite);
			opInfoHashes[0].Add(OpInfo.ReadWrite);
			opInfoHashes[0].Add(OpInfo.Write);
			opInfoHashes[0].Add(OpInfo.WriteVmm);
			opInfoHashes[0].Add(OpInfo.ReadWriteVmm);
			opInfoHashes[0].Add(OpInfo.WriteForce);
			opInfoHashes[0].Add(OpInfo.WriteMem_ReadWriteReg);
			opInfoHashes[0].Add(OpInfo.WriteForceP1);
			opInfoHashes[1].Add(OpInfo.ReadP3);

			// InstructionInfoFactory assumes these have exactly two values: None, Read.
			// It can be less than that if some instructions were filtered out.
			foreach (var i in new[] { 3, 4 }) {
				var opInfoHash = opInfoHashes[i];
				if (opInfoHash.Count != 2)
					opInfoHash.Add(OpInfo.Read);
				if (opInfoHash.Count != 2)
					throw new InvalidOperationException();
				if (!opInfoHash.Contains(OpInfo.None) || !opInfoHash.Contains(OpInfo.Read))
					throw new InvalidOperationException();
			}

			var opInfos = new OpInfo[opInfoHashes.Length][];
			for (int i = 0; i < opInfos.Length; i++) {
				var array = opInfoHashes[i].ToArray();
				opInfos[i] = array;
				Array.Sort(array, (a, b) => ((uint)a).CompareTo((uint)b));
			}
			EnumOpInfos = new EnumType[opInfos.Length];
			var typeIds = new TypeId[IcedConstants.MaxOpCount] {
				TypeIds.OpInfo0,
				TypeIds.OpInfo1,
				TypeIds.OpInfo2,
				TypeIds.OpInfo3,
				TypeIds.OpInfo4,
			};
			for (int i = 0; i < opInfos.Length; i++) {
				if (opInfos[i].Length == 0 || opInfos[i][0] != OpInfo.None)
					throw new InvalidOperationException();
				var values = opInfos[i].Select(a => new EnumValue((uint)a, a.ToString(), default)).ToArray();
				EnumOpInfos[i] = new EnumType(typeIds[i], default, values, EnumTypeFlags.None);
			}
			foreach (var def in defs) {
				var info = def;
				for (int i = 0; i < info.OpInfo.Length; i++)
					info.OpInfoEnum[i] = EnumOpInfos[i][info.OpInfo[i].ToString()];
			}
		}

		void GenerateInfoFlags() {
			var enumOpInfos = EnumOpInfos ?? throw new InvalidOperationException();
			var values1 = typeof(InfoFlags1).GetFields().Where(a => a.IsLiteral && a.Name != nameof(InfoFlags1.FirstUsedBit)).Select(a => new EnumValue((uint)(InfoFlags1)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a), DeprecatedAttribute.GetDeprecatedInfo(a))).ToList();
			var values2 = typeof(InfoFlags2).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(InfoFlags2)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a), DeprecatedAttribute.GetDeprecatedInfo(a))).ToArray();

			uint shift = 0;
			for (int i = 0; i < enumOpInfos.Length; i++) {
				var opInfos = enumOpInfos[i];
				uint maxValue = (uint)opInfos.Values.Length - 1;
				var (mask, bits) = GetMaskAndBits(maxValue);
				var shiftValue = new EnumValue(shift, $"OpInfo{i}Shift", default);
				var maskValue = new EnumValue(mask, $"OpInfo{i}Mask", default);
				values1.Insert(i * 2, shiftValue);
				values1.Insert(i * 2 + 1, maskValue);
				shift += bits;
			}
			if (shift > (uint)InfoFlags1.FirstUsedBit)
				throw new InvalidOperationException($"OpInfoX use too many bits, move some bits to {nameof(InfoFlags2)}");

			var rflagsInfos = RflagsInfos ?? throw new InvalidOperationException();
			if ((uint)rflagsInfos.Length - 1 > (uint)InfoFlags1.RflagsInfoMask)
				throw new InvalidOperationException();
			var enumImpliedAccess = genTypes[TypeIds.ImpliedAccess];
			if ((uint)enumImpliedAccess.Values.Length - 1 > (uint)InfoFlags1.ImpliedAccessMask)
				throw new InvalidOperationException();
			if ((uint)genTypes[TypeIds.EncodingKind].Values.Length - 1 > (uint)InfoFlags2.EncodingMask)
				throw new InvalidOperationException();
			if ((uint)genTypes[TypeIds.FlowControl].Values.Length - 1 > (uint)InfoFlags2.FlowControlMask)
				throw new InvalidOperationException();
			var enumCpuidFeatureInternal = EnumCpuidFeatureInternal ?? throw new InvalidOperationException();
			if ((uint)enumCpuidFeatureInternal.Values.Length - 1 > (uint)InfoFlags2.CpuidFeatureInternalMask)
				throw new InvalidOperationException();

			EnumInfoFlags1 = new EnumType(TypeIds.InfoFlags1, default, values1.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
			EnumInfoFlags2 = new EnumType(TypeIds.InfoFlags2, default, values2, EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
		}

		static (uint mask, uint bits) GetMaskAndBits(uint maxValue) {
			uint mask = 0;
			for (uint bits = 1; ; bits++) {
				mask = (mask << 1) | 1;
				if (maxValue <= mask)
					return (mask, bits);
			}
		}

		void GenerateInstrInfoConstants() {
			var constants = new List<Constant>();

			var enumOpInfos = EnumOpInfos ?? throw new InvalidOperationException();
			for (int i = 0; i < enumOpInfos.Length; i++)
				constants.Add(new Constant(ConstantKind.Index, $"OpInfo{i}_Count", (uint)enumOpInfos[i].Values.Length, ConstantsTypeFlags.None));

			var enumRflagsInfo = EnumRflagsInfo ?? throw new InvalidOperationException();
			constants.Add(new Constant(ConstantKind.Index, enumRflagsInfo.RawName + "_Count", (uint)enumRflagsInfo.Values.Length, ConstantsTypeFlags.None));

			constants.Add(new Constant(ConstantKind.Index, "DefaultUsedRegisterCollCapacity", 10, ConstantsTypeFlags.None));
			constants.Add(new Constant(ConstantKind.Index, "DefaultUsedMemoryCollCapacity", 8, ConstantsTypeFlags.None));

			InstrInfoConstants = new ConstantsType(TypeIds.InstrInfoConstants, ConstantsTypeFlags.None, default, constants.ToArray());
		}
	}
}
