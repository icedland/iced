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
using System.IO;
using System.Linq;
using System.Text;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.InstructionInfo;
using Generator.Tables;

namespace Generator {
	static class CodeComments {
		public static void AddComments(GenTypes genTypes, string unitTestDir) {
			var removed = genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues).Select(a => a.RawName).ToHashSet();
			var docs = new Dictionary<string, string>(StringComparer.Ordinal);
			bool checkedIt = false;
			const char sepChar = '|';
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var toInstrInfo = defs.ToDictionary(a => a.OpCodeInfo.Code.RawName, a => a.InstrInfo, StringComparer.Ordinal);
			var toOpCodeInfo = defs.ToDictionary(a => a.OpCodeInfo.Code.RawName, a => a.OpCodeInfo, StringComparer.Ordinal);
			var sb = new StringBuilder();
			foreach (var line in File.ReadLines(Path.Combine(unitTestDir, "Encoder", "OpCodeInfos.txt"))) {
				if (line.Length == 0 || line[0] == '#')
					continue;
				var parts = line.Split(',');
				if (parts.Length != 8)
					throw new InvalidOperationException("Invalid file");
				var name = parts[0].Trim();
				if (removed.Contains(name))
					continue;
				var opCodeStr = parts[5].Trim();
				var instructionStr = parts[6].Trim();
				if (name == "Add_rm8_r8") {
					// Verify that we read the correct columns, in case someone reorders them...
					if (opCodeStr != "00 /r" || instructionStr != $"ADD r/m8{sepChar} r8")
						throw new InvalidOperationException("Wrong columns!");
					checkedIt = true;
				}
				instructionStr = instructionStr.Replace(sepChar, ',');
				var docStr = $"#(c:{instructionStr})##(p:)##(c:{opCodeStr})##(p:)##(c:{GetCpuid(toInstrInfo[name])})##(p:)##(c:{GetMode(sb, toOpCodeInfo[name])})#";
				docs.Add(name, docStr);
			}
			if (!checkedIt)
				throw new InvalidOperationException();

			foreach (var enumValue in genTypes[TypeIds.Code].Values) {
				if (!string.IsNullOrEmpty(enumValue.Documentation))
					continue;
				if (!docs.TryGetValue(enumValue.RawName, out var doc))
					throw new InvalidOperationException($"Couldn't find enum {enumValue.RawName}");
				enumValue.Documentation = doc;
			}
		}

		static string GetMode(StringBuilder sb, OpCodeInfo opCode) {
			sb.Clear();
			if ((opCode.Flags & OpCodeFlags.Mode16) != 0)
				sb.Append("16");
			if ((opCode.Flags & OpCodeFlags.Mode32) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("32");
			}
			if ((opCode.Flags & OpCodeFlags.Mode64) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("64");
			}
			if (sb.Length == 0)
				throw new InvalidOperationException();
			sb.Append("-bit");
			return sb.ToString();
		}

		static string GetCpuid(InstrInfo info) {
			if ((info.Flags & InstrInfoFlags.AVX2_Check) != 0)
				return "AVX (reg,mem) or AVX2 (reg,reg)";
			return string.Join(" and ", info.Cpuid.Select(a => GetCpuid(a)));
		}

		static string GetCpuid(EnumValue c) =>
			(CpuidFeature)c.Value switch {
				CpuidFeature.INTEL8086 => "8086+",
				CpuidFeature.INTEL8086_ONLY => "8086",
				CpuidFeature.INTEL186 => "186+",
				CpuidFeature.INTEL286 => "286+",
				CpuidFeature.INTEL286_ONLY => "286",
				CpuidFeature.INTEL386 => "386+",
				CpuidFeature.INTEL386_ONLY => "386",
				CpuidFeature.INTEL386_A0_ONLY => "386 A0",
				CpuidFeature.INTEL486 => "486+",
				CpuidFeature.INTEL486_A_ONLY => "486 A",
				CpuidFeature.INTEL386_486_ONLY => "386/486",
				CpuidFeature.IA64 => "IA-64",
				CpuidFeature.FPU => "8087+",
				CpuidFeature.FPU287 => "287+",
				CpuidFeature.FPU287XL_ONLY => "287 XL",
				CpuidFeature.FPU387 => "387+",
				CpuidFeature.FPU387SL_ONLY => "387 SL",
				CpuidFeature.GEODE => "AMD Geode LX/GX",
				CpuidFeature.HLE_or_RTM => "HLE or RTM",
				CpuidFeature.SKINIT_or_SVML => "SKINIT or SVML",
				CpuidFeature.INVEPT => "VMX and IA32_VMX_EPT_VPID_CAP[bit 20]",
				CpuidFeature.INVVPID => "VMX and IA32_VMX_EPT_VPID_CAP[bit 32]",
				CpuidFeature.MULTIBYTENOP => "CPUID.01H.EAX[Bits 11:8] = 0110B or 1111B",
				CpuidFeature.PAUSE => "Pentium 4 or later",
				CpuidFeature.RDPMC => "Pentium MMX or later, or Pentium Pro or later",
				CpuidFeature.D3NOW => "3DNOW",
				CpuidFeature.D3NOWEXT => "3DNOWEXT",
				CpuidFeature.SSE4_1 => "SSE4.1",
				CpuidFeature.SSE4_2 => "SSE4.2",
				CpuidFeature.AMX_BF16 => "AMX-BF16",
				CpuidFeature.AMX_TILE => "AMX-TILE",
				CpuidFeature.AMX_INT8 => "AMX-INT8",
				_ => c.RawName,
			};
	}
}
