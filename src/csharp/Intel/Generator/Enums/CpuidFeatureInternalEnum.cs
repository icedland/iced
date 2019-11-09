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

using System.Linq;

namespace Generator.Enums {
	static class CpuidFeatureInternalEnum {
		const string? documentation = null;
		public static readonly EnumValue[][] AllCombinations;
		public static readonly EnumType Instance;

		static CpuidFeatureInternalEnum() {
			var cpuidFeatures = GetAllCombinations();
			AllCombinations = cpuidFeatures;
			var values = new EnumValue[cpuidFeatures.Length];
			for (int i = 0; i < values.Length; i++) {
				var name = string.Join("_and_", cpuidFeatures[i].Select(a => a.RawName));
				values[i] = new EnumValue(name);
			}

			Instance = new EnumType(TypeIds.CpuidFeatureInternal, documentation, values, EnumTypeFlags.None);
		}

		static EnumValue[][] GetAllCombinations() =>
			new EnumValue[][] {
				new[] { CpuidFeatureEnum.Instance["INTEL8086"] },
				new[] { CpuidFeatureEnum.Instance["INTEL8086_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["INTEL186"] },
				new[] { CpuidFeatureEnum.Instance["INTEL286"] },
				new[] { CpuidFeatureEnum.Instance["INTEL286_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["INTEL386"] },
				new[] { CpuidFeatureEnum.Instance["INTEL386_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["INTEL386_A0_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["INTEL486"] },
				new[] { CpuidFeatureEnum.Instance["INTEL486_A_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["INTEL386_486_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["IA64"] },
				new[] { CpuidFeatureEnum.Instance["X64"] },
				new[] { CpuidFeatureEnum.Instance["ADX"] },
				new[] { CpuidFeatureEnum.Instance["AES"] },
				new[] { CpuidFeatureEnum.Instance["AES"], CpuidFeatureEnum.Instance["AVX"] },
				new[] { CpuidFeatureEnum.Instance["AVX"] },
				new[] { CpuidFeatureEnum.Instance["AVX"], CpuidFeatureEnum.Instance["GFNI"] },
				new[] { CpuidFeatureEnum.Instance["AVX2"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_4FMAPS"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_4VNNIW"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_BITALG"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_IFMA"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_VBMI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_VBMI2"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_VNNI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512_VPOPCNTDQ"] },
				new[] { CpuidFeatureEnum.Instance["AVX512BW"] },
				new[] { CpuidFeatureEnum.Instance["AVX512CD"] },
				new[] { CpuidFeatureEnum.Instance["AVX512DQ"] },
				new[] { CpuidFeatureEnum.Instance["AVX512ER"] },
				new[] { CpuidFeatureEnum.Instance["AVX512F"] },
				new[] { CpuidFeatureEnum.Instance["AVX512F"], CpuidFeatureEnum.Instance["AVX512_VP2INTERSECT"] },
				new[] { CpuidFeatureEnum.Instance["AVX512F"], CpuidFeatureEnum.Instance["GFNI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512F"], CpuidFeatureEnum.Instance["VAES"] },
				new[] { CpuidFeatureEnum.Instance["AVX512F"], CpuidFeatureEnum.Instance["VPCLMULQDQ"] },
				new[] { CpuidFeatureEnum.Instance["AVX512PF"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_BF16"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_BITALG"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_IFMA"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_VBMI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_VBMI2"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_VNNI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_VP2INTERSECT"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512_VPOPCNTDQ"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512BW"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512CD"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512DQ"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["AVX512F"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["GFNI"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["VAES"] },
				new[] { CpuidFeatureEnum.Instance["AVX512VL"], CpuidFeatureEnum.Instance["VPCLMULQDQ"] },
				new[] { CpuidFeatureEnum.Instance["BMI1"] },
				new[] { CpuidFeatureEnum.Instance["BMI2"] },
				new[] { CpuidFeatureEnum.Instance["CET_IBT"] },
				new[] { CpuidFeatureEnum.Instance["CET_SS"] },
				new[] { CpuidFeatureEnum.Instance["CL1INVMB"] },
				new[] { CpuidFeatureEnum.Instance["CLDEMOTE"] },
				new[] { CpuidFeatureEnum.Instance["CLFLUSHOPT"] },
				new[] { CpuidFeatureEnum.Instance["CLFSH"] },
				new[] { CpuidFeatureEnum.Instance["CLWB"] },
				new[] { CpuidFeatureEnum.Instance["CLZERO"] },
				new[] { CpuidFeatureEnum.Instance["CMOV"] },
				new[] { CpuidFeatureEnum.Instance["CMPXCHG16B"] },
				new[] { CpuidFeatureEnum.Instance["CPUID"] },
				new[] { CpuidFeatureEnum.Instance["CX8"] },
				new[] { CpuidFeatureEnum.Instance["D3NOW"] },
				new[] { CpuidFeatureEnum.Instance["D3NOWEXT"] },
				new[] { CpuidFeatureEnum.Instance["ENCLV"] },
				new[] { CpuidFeatureEnum.Instance["ENQCMD"] },
				new[] { CpuidFeatureEnum.Instance["F16C"] },
				new[] { CpuidFeatureEnum.Instance["FMA"] },
				new[] { CpuidFeatureEnum.Instance["FMA4"] },
				new[] { CpuidFeatureEnum.Instance["FPU"] },
				new[] { CpuidFeatureEnum.Instance["FPU"], CpuidFeatureEnum.Instance["CMOV"] },
				new[] { CpuidFeatureEnum.Instance["FPU"], CpuidFeatureEnum.Instance["SSE3"] },
				new[] { CpuidFeatureEnum.Instance["FPU287"] },
				new[] { CpuidFeatureEnum.Instance["FPU287XL_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["FPU387"] },
				new[] { CpuidFeatureEnum.Instance["FPU387SL_ONLY"] },
				new[] { CpuidFeatureEnum.Instance["FSGSBASE"] },
				new[] { CpuidFeatureEnum.Instance["FXSR"] },
				new[] { CpuidFeatureEnum.Instance["GEODE"] },
				new[] { CpuidFeatureEnum.Instance["GFNI"] },
				new[] { CpuidFeatureEnum.Instance["HLE_or_RTM"] },
				new[] { CpuidFeatureEnum.Instance["INVEPT"] },
				new[] { CpuidFeatureEnum.Instance["INVPCID"] },
				new[] { CpuidFeatureEnum.Instance["INVVPID"] },
				new[] { CpuidFeatureEnum.Instance["LWP"] },
				new[] { CpuidFeatureEnum.Instance["LZCNT"] },
				new[] { CpuidFeatureEnum.Instance["MCOMMIT"] },
				new[] { CpuidFeatureEnum.Instance["MMX"] },
				new[] { CpuidFeatureEnum.Instance["MONITOR"] },
				new[] { CpuidFeatureEnum.Instance["MONITORX"] },
				new[] { CpuidFeatureEnum.Instance["MOVBE"] },
				new[] { CpuidFeatureEnum.Instance["MOVDIR64B"] },
				new[] { CpuidFeatureEnum.Instance["MOVDIRI"] },
				new[] { CpuidFeatureEnum.Instance["MPX"] },
				new[] { CpuidFeatureEnum.Instance["MSR"] },
				new[] { CpuidFeatureEnum.Instance["MULTIBYTENOP"] },
				new[] { CpuidFeatureEnum.Instance["PADLOCK_ACE"] },
				new[] { CpuidFeatureEnum.Instance["PADLOCK_PHE"] },
				new[] { CpuidFeatureEnum.Instance["PADLOCK_PMM"] },
				new[] { CpuidFeatureEnum.Instance["PADLOCK_RNG"] },
				new[] { CpuidFeatureEnum.Instance["PAUSE"] },
				new[] { CpuidFeatureEnum.Instance["PCLMULQDQ"] },
				new[] { CpuidFeatureEnum.Instance["PCLMULQDQ"], CpuidFeatureEnum.Instance["AVX"] },
				new[] { CpuidFeatureEnum.Instance["PCOMMIT"] },
				new[] { CpuidFeatureEnum.Instance["PCONFIG"] },
				new[] { CpuidFeatureEnum.Instance["PKU"] },
				new[] { CpuidFeatureEnum.Instance["POPCNT"] },
				new[] { CpuidFeatureEnum.Instance["PREFETCHW"] },
				new[] { CpuidFeatureEnum.Instance["PREFETCHWT1"] },
				new[] { CpuidFeatureEnum.Instance["PTWRITE"] },
				new[] { CpuidFeatureEnum.Instance["RDPID"] },
				new[] { CpuidFeatureEnum.Instance["RDPMC"] },
				new[] { CpuidFeatureEnum.Instance["RDPRU"] },
				new[] { CpuidFeatureEnum.Instance["RDRAND"] },
				new[] { CpuidFeatureEnum.Instance["RDSEED"] },
				new[] { CpuidFeatureEnum.Instance["RDTSCP"] },
				new[] { CpuidFeatureEnum.Instance["RTM"] },
				new[] { CpuidFeatureEnum.Instance["SEP"] },
				new[] { CpuidFeatureEnum.Instance["SGX1"] },
				new[] { CpuidFeatureEnum.Instance["SHA"] },
				new[] { CpuidFeatureEnum.Instance["SKINIT_or_SVML"] },
				new[] { CpuidFeatureEnum.Instance["SMAP"] },
				new[] { CpuidFeatureEnum.Instance["SMX"] },
				new[] { CpuidFeatureEnum.Instance["SSE"] },
				new[] { CpuidFeatureEnum.Instance["SSE2"] },
				new[] { CpuidFeatureEnum.Instance["SSE3"] },
				new[] { CpuidFeatureEnum.Instance["SSE4_1"] },
				new[] { CpuidFeatureEnum.Instance["SSE4_2"] },
				new[] { CpuidFeatureEnum.Instance["SSE4A"] },
				new[] { CpuidFeatureEnum.Instance["SSSE3"] },
				new[] { CpuidFeatureEnum.Instance["SVM"] },
				new[] { CpuidFeatureEnum.Instance["SYSCALL"] },
				new[] { CpuidFeatureEnum.Instance["TBM"] },
				new[] { CpuidFeatureEnum.Instance["TSC"] },
				new[] { CpuidFeatureEnum.Instance["VAES"] },
				new[] { CpuidFeatureEnum.Instance["VMX"] },
				new[] { CpuidFeatureEnum.Instance["VPCLMULQDQ"] },
				new[] { CpuidFeatureEnum.Instance["WAITPKG"] },
				new[] { CpuidFeatureEnum.Instance["WBNOINVD"] },
				new[] { CpuidFeatureEnum.Instance["XOP"] },
				new[] { CpuidFeatureEnum.Instance["XSAVE"] },
				new[] { CpuidFeatureEnum.Instance["XSAVEC"] },
				new[] { CpuidFeatureEnum.Instance["XSAVEOPT"] },
				new[] { CpuidFeatureEnum.Instance["XSAVES"] },
			};
	}
}
