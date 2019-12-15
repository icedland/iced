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
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	[Flags]
	enum InfoFlags1 : uint {
		// OpInfo*Shift and OpInfo*Mask (* = 0..4) are added here

		// Free bits

		FirstUsedBit				= RflagsInfoShift,
		RflagsInfoShift				= 14,
		RflagsInfoMask				= 0x3F,
		CodeInfoShift				= 20,
		CodeInfoMask				= 0x7F,
		SaveRestore					= 0x08000000,
		StackInstruction			= 0x10000000,
		ProtectedMode				= 0x20000000,
		Privileged					= 0x40000000,
		NoSegmentRead				= 0x80000000,
	}

	[Flags]
	enum InfoFlags2 : uint {
		EncodingShift				= 0,
		EncodingMask				= 7,

		// Free bits

		AVX2_Check					= 0x00040000,
		OpMaskRegReadWrite			= 0x00080000,
		FlowControlShift			= 20,
		FlowControlMask				= 0xF,
		CpuidFeatureInternalShift	= 24,
		CpuidFeatureInternalMask	= 0xFF,
	}

	sealed class InstrInfoTypesGen {
		public EnumType? EnumCodeInfo;
		public EnumType? EnumRflagsInfo;
		public EnumType[]? EnumOpInfos;
		public EnumType? EnumInfoFlags1;
		public EnumType? EnumInfoFlags2;
		public EnumType? EnumCpuidFeatureInternal;
		public ConstantsType? InstrInfoConstants;
		public (EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[]? CpuidFeatures;
		public (EnumValue value, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) rflags)[]? RflagsInfos;
		public readonly InstrInfo[] InstrInfos;

		public InstrInfoTypesGen() => InstrInfos = InstrInfoTable.Data;

		public void Generate() {
			GenerateCodeInfo();
			GenerateCpuidFeatureInternal();
			GenerateRflagsInfo();
			GenerateOpInfoX();
			GenerateInfoFlags();
			GenerateInstrInfoConstants();
		}

		void GenerateCodeInfo() {
			var values = typeof(CodeInfo).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CodeInfo)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();
			EnumCodeInfo = new EnumType(TypeIds.CodeInfo, null, values, EnumTypeFlags.NoInitialize);
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

		void GenerateCpuidFeatureInternal() {
			var cpuidToInternalDict = new Dictionary<EnumValue[], EnumValue>(new EnumValueArrayComparer());
			var cpuidFeatures = new List<(EnumValue cpuidInternal, EnumValue[] cpuidFeatures)>();
			foreach (var info in InstrInfos) {
				if (!cpuidToInternalDict.ContainsKey(info.Cpuid)) {
					var name = string.Join("_and_", info.Cpuid.Select(a => a.RawName));
					var internalEnumValue = new EnumValue(0, name, null);
					cpuidFeatures.Add((internalEnumValue, info.Cpuid));
					cpuidToInternalDict.Add(info.Cpuid, internalEnumValue);
				}
			}
			cpuidFeatures.Sort((a, b) => StringComparer.Ordinal.Compare(a.cpuidInternal.RawName, b.cpuidInternal.RawName));

			EnumCpuidFeatureInternal = new EnumType(TypeIds.CpuidFeatureInternal, null, cpuidFeatures.Select(a => a.cpuidInternal).ToArray(), EnumTypeFlags.None);
			this.CpuidFeatures = cpuidFeatures.ToArray();
			foreach (var info in InstrInfos)
				info.CpuidInternal = cpuidToInternalDict[info.Cpuid];
		}

		const string RflagsInfo_None = "None";
		void GenerateRflagsInfo() {
			var rflagsHashSet = new HashSet<(RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set)> {
				// None must always be present
				default,
				// Needed by CodeInfo.Clear_rflags
				(RflagsBits.None, RflagsBits.AF, RflagsBits.None, RflagsBits.CF | RflagsBits.OF | RflagsBits.SF, RflagsBits.PF | RflagsBits.ZF),
			};
			foreach (var info in InstrInfos)
				rflagsHashSet.Add((info.RflagsRead, info.RflagsUndefined, info.RflagsWritten, info.RflagsCleared, info.RflagsSet));
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
				var value = new EnumValue(0, GetName(sb, info), null);
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
			foreach (var info in InstrInfos)
				info.RflagsInfo = toRflagsInfo[(info.RflagsRead, info.RflagsUndefined, info.RflagsWritten, info.RflagsCleared, info.RflagsSet)];
			EnumRflagsInfo = new EnumType(TypeIds.RflagsInfo, null, values, EnumTypeFlags.None);
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
			if ((rflags & RflagsBits.AF) != 0) { sb.Append('a'); rflags &= ~RflagsBits.AF; }
			if ((rflags & RflagsBits.CF) != 0) { sb.Append('c'); rflags &= ~RflagsBits.CF; }
			if ((rflags & RflagsBits.OF) != 0) { sb.Append('o'); rflags &= ~RflagsBits.OF; }
			if ((rflags & RflagsBits.PF) != 0) { sb.Append('p'); rflags &= ~RflagsBits.PF; }
			if ((rflags & RflagsBits.SF) != 0) { sb.Append('s'); rflags &= ~RflagsBits.SF; }
			if ((rflags & RflagsBits.ZF) != 0) { sb.Append('z'); rflags &= ~RflagsBits.ZF; }
			if ((rflags & RflagsBits.IF) != 0) { sb.Append('i'); rflags &= ~RflagsBits.IF; }
			if ((rflags & RflagsBits.DF) != 0) { sb.Append('d'); rflags &= ~RflagsBits.DF; }
			if ((rflags & RflagsBits.AC) != 0) { sb.Append("AC"); rflags &= ~RflagsBits.AC; }
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
			foreach (var info in InstrInfos) {
				bool foundNone = false;
				for (int i = 0; i < info.OpInfo.Length; i++) {
					var opInfo = info.OpInfo[i];
					if (opInfo == OpInfo.None)
						foundNone = true;
					else if (foundNone)
						throw new InvalidOperationException();
					opInfoHashes[i].Add(opInfo);
				}
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
				var values = opInfos[i].Select(a => new EnumValue((uint)a, a.ToString(), null)).ToArray();
				EnumOpInfos[i] = new EnumType(typeIds[i], null, values, EnumTypeFlags.None);
			}
			foreach (var info in InstrInfos) {
				for (int i = 0; i < info.OpInfo.Length; i++)
					info.OpInfoEnum[i] = EnumOpInfos[i][info.OpInfo[i].ToString()];
			}
		}

		void GenerateInfoFlags() {
			var enumOpInfos = this.EnumOpInfos ?? throw new InvalidOperationException();
			var values1 = typeof(InfoFlags1).GetFields().Where(a => a.IsLiteral && a.Name != nameof(InfoFlags1.FirstUsedBit)).Select(a => new EnumValue((uint)(InfoFlags1)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToList();
			var values2 = typeof(InfoFlags2).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(InfoFlags2)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

			uint shift = 0;
			for (int i = 0; i < enumOpInfos.Length; i++) {
				var opInfos = enumOpInfos[i];
				uint maxValue = (uint)opInfos.Values.Length - 1;
				var (mask, bits) = GetMaskAndBits(maxValue);
				var shiftValue = new EnumValue(shift, $"OpInfo{i}Shift", null);
				var maskValue = new EnumValue(mask, $"OpInfo{i}Mask", null);
				values1.Insert(i * 2, shiftValue);
				values1.Insert(i * 2 + 1, maskValue);
				shift += bits;
			}
			if (shift > (uint)InfoFlags1.FirstUsedBit)
				throw new InvalidOperationException($"OpInfoX use too many bits, move some bits to {nameof(InfoFlags2)}");

			var rflagsInfos = this.RflagsInfos ?? throw new InvalidOperationException();
			if ((uint)rflagsInfos.Length - 1 > (uint)InfoFlags1.RflagsInfoMask)
				throw new InvalidOperationException();
			var enumCodeInfo = this.EnumCodeInfo ?? throw new InvalidOperationException();
			if ((uint)enumCodeInfo.Values.Length - 1 > (uint)InfoFlags1.CodeInfoMask)
				throw new InvalidOperationException();
			if ((uint)EncodingKindEnum.Instance.Values.Length - 1 > (uint)InfoFlags2.EncodingMask)
				throw new InvalidOperationException();
			if ((uint)FlowControlEnum.Instance.Values.Length - 1 > (uint)InfoFlags2.FlowControlMask)
				throw new InvalidOperationException();
			var enumCpuidFeatureInternal = this.EnumCpuidFeatureInternal ?? throw new InvalidOperationException();
			if ((uint)enumCpuidFeatureInternal.Values.Length - 1 > (uint)InfoFlags2.CpuidFeatureInternalMask)
				throw new InvalidOperationException();

			EnumInfoFlags1 = new EnumType(TypeIds.InfoFlags1, null, values1.ToArray(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
			EnumInfoFlags2 = new EnumType(TypeIds.InfoFlags2, null, values2, EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
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

			var enumOpInfos = this.EnumOpInfos ?? throw new InvalidOperationException();
			for (int i = 0; i < enumOpInfos.Length; i++)
				constants.Add(new Constant(ConstantKind.Int32, $"OpInfo{i}_Count", (uint)enumOpInfos[i].Values.Length, ConstantsTypeFlags.None));

			var enumRflagsInfo = this.EnumRflagsInfo ?? throw new InvalidOperationException();
			constants.Add(new Constant(ConstantKind.Int32, enumRflagsInfo.RawName + "_Count", (uint)enumRflagsInfo.Values.Length, ConstantsTypeFlags.None));

			constants.Add(new Constant(ConstantKind.Int32, "DefaultUsedRegisterCollCapacity", 10, ConstantsTypeFlags.None));
			constants.Add(new Constant(ConstantKind.Int32, "DefaultUsedMemoryCollCapacity", 8, ConstantsTypeFlags.None));

			InstrInfoConstants = new ConstantsType(TypeIds.InstrInfoConstants, ConstantsTypeFlags.None, null, constants.ToArray());
		}
	}
}
