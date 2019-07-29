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

#if !NO_INSTR_INFO
using System;
using System.IO;
using Generator.IO;
using Iced.Intel;
using Iced.Intel.InstructionInfoInternal;

namespace Generator.InstructionInfo {
	sealed class CpuidFeatureTableGenerator {
		readonly string icedProjectDir;

		public CpuidFeatureTableGenerator(string icedProjectDir) => this.icedProjectDir = icedProjectDir;

		public void Generate() {
			var cpuidFeatures = new CpuidFeature[CpuidFeatureInternalConstants.MaxCpuidFeatureInternalValues][] {
				new[] { CpuidFeature.INTEL8086 },
				new[] { CpuidFeature.INTEL8086_ONLY },
				new[] { CpuidFeature.INTEL186 },
				new[] { CpuidFeature.INTEL286 },
				new[] { CpuidFeature.INTEL286_ONLY },
				new[] { CpuidFeature.INTEL386 },
				new[] { CpuidFeature.INTEL386_ONLY },
				new[] { CpuidFeature.INTEL386_A0_ONLY },
				new[] { CpuidFeature.INTEL486 },
				new[] { CpuidFeature.INTEL486_A_ONLY },
				new[] { CpuidFeature.INTEL386_486_ONLY },
				new[] { CpuidFeature.IA64 },
				new[] { CpuidFeature.X64 },
				new[] { CpuidFeature.ADX },
				new[] { CpuidFeature.AES },
				new[] { CpuidFeature.AES, CpuidFeature.AVX },
				new[] { CpuidFeature.AVX },
				new[] { CpuidFeature.AVX, CpuidFeature.GFNI },
				new[] { CpuidFeature.AVX2 },
				new[] { CpuidFeature.AVX512_4FMAPS },
				new[] { CpuidFeature.AVX512_4VNNIW },
				new[] { CpuidFeature.AVX512_BITALG },
				new[] { CpuidFeature.AVX512_IFMA },
				new[] { CpuidFeature.AVX512_VBMI },
				new[] { CpuidFeature.AVX512_VBMI2 },
				new[] { CpuidFeature.AVX512_VNNI },
				new[] { CpuidFeature.AVX512_VPOPCNTDQ },
				new[] { CpuidFeature.AVX512BW },
				new[] { CpuidFeature.AVX512CD },
				new[] { CpuidFeature.AVX512DQ },
				new[] { CpuidFeature.AVX512ER },
				new[] { CpuidFeature.AVX512F },
				new[] { CpuidFeature.AVX512F, CpuidFeature.AVX512_VP2INTERSECT },
				new[] { CpuidFeature.AVX512F, CpuidFeature.GFNI },
				new[] { CpuidFeature.AVX512F, CpuidFeature.VAES },
				new[] { CpuidFeature.AVX512F, CpuidFeature.VPCLMULQDQ },
				new[] { CpuidFeature.AVX512PF },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_BF16 },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_BITALG },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_IFMA },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_VBMI },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_VBMI2 },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_VNNI },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_VP2INTERSECT },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512_VPOPCNTDQ },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512BW },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512CD },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512DQ },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.AVX512F },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.GFNI },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.VAES },
				new[] { CpuidFeature.AVX512VL, CpuidFeature.VPCLMULQDQ },
				new[] { CpuidFeature.BMI1 },
				new[] { CpuidFeature.BMI2 },
				new[] { CpuidFeature.CET_IBT },
				new[] { CpuidFeature.CET_SS },
				new[] { CpuidFeature.CFLSH },
				new[] { CpuidFeature.CL1INVMB },
				new[] { CpuidFeature.CLDEMOTE },
				new[] { CpuidFeature.CLFLUSHOPT },
				new[] { CpuidFeature.CLFSH },
				new[] { CpuidFeature.CLWB },
				new[] { CpuidFeature.CLZERO },
				new[] { CpuidFeature.CMOV },
				new[] { CpuidFeature.CMPXCHG16B },
				new[] { CpuidFeature.CPUID },
				new[] { CpuidFeature.CX8 },
				new[] { CpuidFeature.D3NOW },
				new[] { CpuidFeature.D3NOWEXT },
				new[] { CpuidFeature.ECR },
				new[] { CpuidFeature.ENCLV },
				new[] { CpuidFeature.ENQCMD },
				new[] { CpuidFeature.F16C },
				new[] { CpuidFeature.FMA },
				new[] { CpuidFeature.FMA4 },
				new[] { CpuidFeature.FPU },
				new[] { CpuidFeature.FPU, CpuidFeature.CMOV },
				new[] { CpuidFeature.FPU, CpuidFeature.SSE3 },
				new[] { CpuidFeature.FPU287 },
				new[] { CpuidFeature.FPU287XL_ONLY },
				new[] { CpuidFeature.FPU387 },
				new[] { CpuidFeature.FPU387SL_ONLY },
				new[] { CpuidFeature.FSGSBASE },
				new[] { CpuidFeature.FXSR },
				new[] { CpuidFeature.GEODE },
				new[] { CpuidFeature.GFNI },
				new[] { CpuidFeature.HLE_or_RTM },
				new[] { CpuidFeature.INVEPT },
				new[] { CpuidFeature.INVPCID },
				new[] { CpuidFeature.INVVPID },
				new[] { CpuidFeature.LWP },
				new[] { CpuidFeature.LZCNT },
				new[] { CpuidFeature.MMX },
				new[] { CpuidFeature.MONITOR },
				new[] { CpuidFeature.MONITORX },
				new[] { CpuidFeature.MOVBE },
				new[] { CpuidFeature.MOVDIR64B },
				new[] { CpuidFeature.MOVDIRI },
				new[] { CpuidFeature.MPX },
				new[] { CpuidFeature.MSR },
				new[] { CpuidFeature.MULTIBYTENOP },
				new[] { CpuidFeature.PADLOCK_ACE },
				new[] { CpuidFeature.PADLOCK_PHE },
				new[] { CpuidFeature.PADLOCK_PMM },
				new[] { CpuidFeature.PADLOCK_RNG },
				new[] { CpuidFeature.PAUSE },
				new[] { CpuidFeature.PCLMULQDQ },
				new[] { CpuidFeature.PCLMULQDQ, CpuidFeature.AVX },
				new[] { CpuidFeature.PCOMMIT },
				new[] { CpuidFeature.PCONFIG },
				new[] { CpuidFeature.PKU },
				new[] { CpuidFeature.POPCNT },
				new[] { CpuidFeature.PREFETCHW },
				new[] { CpuidFeature.PREFETCHWT1 },
				new[] { CpuidFeature.PTWRITE },
				new[] { CpuidFeature.RDPID },
				new[] { CpuidFeature.RDPMC },
				new[] { CpuidFeature.RDRAND },
				new[] { CpuidFeature.RDSEED },
				new[] { CpuidFeature.RDTSCP },
				new[] { CpuidFeature.RTM },
				new[] { CpuidFeature.SEP },
				new[] { CpuidFeature.SGX1 },
				new[] { CpuidFeature.SHA },
				new[] { CpuidFeature.SKINIT_or_SVML },
				new[] { CpuidFeature.SMAP },
				new[] { CpuidFeature.SMX },
				new[] { CpuidFeature.SSE },
				new[] { CpuidFeature.SSE2 },
				new[] { CpuidFeature.SSE3 },
				new[] { CpuidFeature.SSE4_1 },
				new[] { CpuidFeature.SSE4_2 },
				new[] { CpuidFeature.SSE4A },
				new[] { CpuidFeature.SSSE3 },
				new[] { CpuidFeature.SVM },
				new[] { CpuidFeature.SYSCALL },
				new[] { CpuidFeature.TBM },
				new[] { CpuidFeature.TSC },
				new[] { CpuidFeature.VAES },
				new[] { CpuidFeature.VMX },
				new[] { CpuidFeature.VPCLMULQDQ },
				new[] { CpuidFeature.WAITPKG },
				new[] { CpuidFeature.WBNOINVD },
				new[] { CpuidFeature.XOP },
				new[] { CpuidFeature.XSAVE },
				new[] { CpuidFeature.XSAVEC },
				new[] { CpuidFeature.XSAVEOPT },
				new[] { CpuidFeature.XSAVES },
				new[] { CpuidFeature.ZALLOC },
			};

			var header = new byte[(CpuidFeatureInternalConstants.MaxCpuidFeatureInternalValues + 7) / 8];
			for (int i = 0; i < cpuidFeatures.Length; i++) {
				int len = cpuidFeatures[i].Length;
				if (len < 1 || len > 2)
					throw new InvalidOperationException();
				header[i / 8] |= (byte)((len - 1) << (i % 8));
			}

			using (var writer = new FileWriter(FileUtils.OpenWrite(Path.Combine(icedProjectDir, "Intel", "InstructionInfoInternal", "CpuidFeatureInternalData.g.cs")))) {
				writer.WriteHeader();
				writer.WriteLine("#if !NO_INSTR_INFO");
				writer.WriteLine("namespace Iced.Intel.InstructionInfoInternal {");
				writer.Indent();
				writer.WriteLine("static partial class CpuidFeatureInternalData {");
				writer.Indent();
				writer.WriteLine("static byte[] GetGetCpuidFeaturesData() =>");
				writer.Indent();
				writer.WriteLine("new byte[] {");
				writer.Indent();

				writer.WriteComment("Header");
				writer.WriteLine();
				foreach (var b in header) {
					writer.WriteByte(b);
					writer.WriteLine();
				}
				writer.WriteLine();
				foreach (var info in cpuidFeatures) {
					foreach (var f in info) {
						if ((uint)f > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteByte((byte)f);
					}
					writer.WriteComment(string.Join(", ", info));
					writer.WriteLine();
				}

				writer.Unindent();
				writer.WriteLine("};");
				writer.Unindent();
				writer.Unindent();
				writer.WriteLine("}");
				writer.Unindent();
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}
	}
}
#endif
