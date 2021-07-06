// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Tables;

namespace Generator {
	static class CodeComments {
		public static void AddComments(GenTypes genTypes) {
			var cpuid = genTypes[TypeIds.CpuidFeature];
			var toCpuidName = cpuid.Values.ToDictionary(a => a, a => a.RawName);
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL8086)]] = "8086+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL8086_ONLY)]] = "8086";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL186)]] = "186+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL286)]] = "286+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL286_ONLY)]] = "286";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386)]] = "386+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386_ONLY)]] = "386";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386_A0_ONLY)]] = "386 A0";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL486)]] = "486+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL486_A_ONLY)]] = "486 A";
			toCpuidName[cpuid[nameof(CpuidFeature.SMM)]] = "386+";
			toCpuidName[cpuid[nameof(CpuidFeature.UMOV)]] = "386/486";
			toCpuidName[cpuid[nameof(CpuidFeature.MOV_TR)]] = "386/486/Cyrix/Geode";
			toCpuidName[cpuid[nameof(CpuidFeature.IA64)]] = "IA-64";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU)]] = "8087+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU287)]] = "287+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU287XL_ONLY)]] = "287 XL";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU387)]] = "387+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU387SL_ONLY)]] = "387 SL";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_D3NOW)]] = "AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.HLE_or_RTM)]] = "HLE or RTM";
			toCpuidName[cpuid[nameof(CpuidFeature.SEV_ES)]] = "SEV-ES";
			toCpuidName[cpuid[nameof(CpuidFeature.SEV_SNP)]] = "SEV-SNP";
			toCpuidName[cpuid[nameof(CpuidFeature.SKINIT_or_SVM)]] = "SKINIT or SVM";
			toCpuidName[cpuid[nameof(CpuidFeature.INVEPT)]] = "IA32_VMX_EPT_VPID_CAP[bit 20]";
			toCpuidName[cpuid[nameof(CpuidFeature.INVVPID)]] = "IA32_VMX_EPT_VPID_CAP[bit 32]";
			toCpuidName[cpuid[nameof(CpuidFeature.MULTIBYTENOP)]] = "CPUID.01H.EAX[Bits 11:8] = 0110B or 1111B";
			toCpuidName[cpuid[nameof(CpuidFeature.PAUSE)]] = "Pentium 4 or later";
			toCpuidName[cpuid[nameof(CpuidFeature.RDPMC)]] = "Pentium MMX or later, or Pentium Pro or later";
			toCpuidName[cpuid[nameof(CpuidFeature.D3NOW)]] = "3DNOW";
			toCpuidName[cpuid[nameof(CpuidFeature.D3NOWEXT)]] = "3DNOWEXT";
			toCpuidName[cpuid[nameof(CpuidFeature.SSE4_1)]] = "SSE4.1";
			toCpuidName[cpuid[nameof(CpuidFeature.SSE4_2)]] = "SSE4.2";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_BF16)]] = "AMX-BF16";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_TILE)]] = "AMX-TILE";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_INT8)]] = "AMX-INT8";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_FPU)]] = "Cyrix, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMM)]] = "Cyrix, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMINT)]] = "Cyrix 6x86MX+, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMINT_0F7E)]] = "Cyrix 6x86 or earlier";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SHR)]] = "Cyrix 6x86MX, M II, III";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_DDI)]] = "Cyrix MediaGX, GXm, GXLV, GX1";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_DMI)]] = "AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CENTAUR_AIS)]] = "Centaur AIS";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_VNNI)]] = "AVX-VNNI";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX512_FP16)]] = "AVX512-FP16";

			var sb = new StringBuilder();
			foreach (var def in genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs) {
				var docStr = $"#(c:{def.InstructionString})##(p:)##(c:{def.OpCodeString})##(p:)##(c:{GetCpuid(toCpuidName, def)})##(p:)##(c:{GetMode(sb, def)})#";
				if (string.IsNullOrEmpty(def.Code.Documentation))
					def.Code.Documentation = docStr;
			}
		}

		static string GetMode(StringBuilder sb, InstructionDef def) {
			sb.Clear();
			if ((def.Flags1 & InstructionDefFlags1.Bit16) != 0)
				sb.Append("16");
			if ((def.Flags1 & InstructionDefFlags1.Bit32) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("32");
			}
			if ((def.Flags1 & InstructionDefFlags1.Bit64) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("64");
			}
			if (sb.Length == 0)
				throw new InvalidOperationException();
			sb.Append("-bit");
			return sb.ToString();
		}

		static string GetCpuid(Dictionary<EnumValue, string> toCpuidName, InstructionDef def) =>
			string.Join(" and ", def.Cpuid.Select(a => toCpuidName[a]));
	}
}
