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
				new Constant(ConstantKind.Index, IcedConstants.CodeEnumCountName, (uint)genTypes[TypeIds.Code].Values.Length),
				new Constant(ConstantKind.Index, "RegisterEnumCount", (uint)genTypes[TypeIds.Register].Values.Length),
				new Constant(ConstantKind.Index, "MemorySizeEnumCount", (uint)genTypes[TypeIds.MemorySize].Values.Length),
				new Constant(ConstantKind.Index, "EncodingKindEnumCount", (uint)genTypes[TypeIds.EncodingKind].Values.Length),
				new Constant(ConstantKind.Index, "OpKindEnumCount", (uint)genTypes[TypeIds.OpKind].Values.Length),
				new Constant(ConstantKind.Index, "CodeSizeEnumCount", (uint)genTypes[TypeIds.CodeSize].Values.Length),
				new Constant(ConstantKind.Index, "RoundingControlEnumCount", (uint)genTypes[TypeIds.RoundingControl].Values.Length),
				new Constant(ConstantKind.Index, "MemorySizeOptionsEnumCount", (uint)genTypes[TypeIds.MemorySizeOptions].Values.Length),
				new Constant(ConstantKind.Index, "CC_b_EnumCount", (uint)genTypes[TypeIds.CC_b].Values.Length),
				new Constant(ConstantKind.Index, "CC_ae_EnumCount", (uint)genTypes[TypeIds.CC_ae].Values.Length),
				new Constant(ConstantKind.Index, "CC_e_EnumCount", (uint)genTypes[TypeIds.CC_e].Values.Length),
				new Constant(ConstantKind.Index, "CC_ne_EnumCount", (uint)genTypes[TypeIds.CC_ne].Values.Length),
				new Constant(ConstantKind.Index, "CC_be_EnumCount", (uint)genTypes[TypeIds.CC_be].Values.Length),
				new Constant(ConstantKind.Index, "CC_a_EnumCount", (uint)genTypes[TypeIds.CC_a].Values.Length),
				new Constant(ConstantKind.Index, "CC_p_EnumCount", (uint)genTypes[TypeIds.CC_p].Values.Length),
				new Constant(ConstantKind.Index, "CC_np_EnumCount", (uint)genTypes[TypeIds.CC_np].Values.Length),
				new Constant(ConstantKind.Index, "CC_l_EnumCount", (uint)genTypes[TypeIds.CC_l].Values.Length),
				new Constant(ConstantKind.Index, "CC_ge_EnumCount", (uint)genTypes[TypeIds.CC_ge].Values.Length),
				new Constant(ConstantKind.Index, "CC_le_EnumCount", (uint)genTypes[TypeIds.CC_le].Values.Length),
				new Constant(ConstantKind.Index, "CC_g_EnumCount", (uint)genTypes[TypeIds.CC_g].Values.Length),
				// This is the largest vector register. If it's VEX/EVEX, the upper bits are always cleared when writing to any sub reg, eg. YMM0
				new Constant(ConstantKind.Register, "VMM_first", vmmFirst),
				new Constant(ConstantKind.Register, "VMM_last", vmmLast),
				new Constant(ConstantKind.Int32, "VMM_count", vmmLast - vmmFirst + 1),
				new Constant(ConstantKind.Register, "XMM_last", Get_VEC_last("XMM").Value),
				new Constant(ConstantKind.Register, "YMM_last", Get_VEC_last("YMM").Value),
				new Constant(ConstantKind.Register, "ZMM_last", Get_VEC_last("ZMM").Value),
				new Constant(ConstantKind.Register, "TMM_last", Get_TMM_last().Value),
				new Constant(ConstantKind.Index, "MaxCpuidFeatureInternalValues", (uint)genTypes[TypeIds.CpuidFeatureInternal].Values.Length),
				new Constant(ConstantKind.MemorySize, IcedConstants.FirstBroadcastMemorySizeName, GetFirstBroadcastMemorySize()),
			};
		}

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
