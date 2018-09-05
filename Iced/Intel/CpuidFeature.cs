/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// CPUID feature flags
	/// </summary>
	public enum CpuidFeature {
		/// <summary>
		/// 8086 or later
		/// </summary>
		INTEL8086,

		/// <summary>
		/// 80186 or later
		/// </summary>
		INTEL186,

		/// <summary>
		/// 80286 or later
		/// </summary>
		INTEL286,

		/// <summary>
		/// 80386 or later
		/// </summary>
		INTEL386,

		/// <summary>
		/// Intel486 or later
		/// </summary>
		INTEL486,

		/// <summary>
		/// CPUID.80000001H:EDX.LM[bit 29]
		/// </summary>
		X64,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.ADX[bit 19]
		/// </summary>
		ADX,

		/// <summary>
		/// CPUID.01H:ECX.AES[bit 25]
		/// </summary>
		AES,

		/// <summary>
		/// <see cref="AES"/> and <see cref="AVX"/>
		/// </summary>
		AES_and_AVX,

		/// <summary>
		/// CPUID.01H:ECX.AVX[bit 28]
		/// </summary>
		AVX,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX2[bit 5]
		/// </summary>
		AVX2,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EDX.AVX512_4FMAPS[bit 3]
		/// </summary>
		AVX512_4FMAPS,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EDX.AVX512_4VNNIW[bit 2]
		/// </summary>
		AVX512_4VNNIW,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512_IFMA[bit 21]
		/// </summary>
		AVX512_IFMA,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VBMI[bit 1]
		/// </summary>
		AVX512_VBMI,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512BW[bit 30]
		/// </summary>
		AVX512BW,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512CD[bit 28]
		/// </summary>
		AVX512CD,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512DQ[bit 17]
		/// </summary>
		AVX512DQ,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512ER[bit 27]
		/// </summary>
		AVX512ER,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512F[bit 16]
		/// </summary>
		AVX512F,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512PF[bit 26]
		/// </summary>
		AVX512PF,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512_IFMA"/>
		/// </summary>
		AVX512VL_or_AVX512_IFMA,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512_VBMI"/>
		/// </summary>
		AVX512VL_or_AVX512_VBMI,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512BW"/>
		/// </summary>
		AVX512VL_or_AVX512BW,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512CD"/>
		/// </summary>
		AVX512VL_or_AVX512CD,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512DQ"/>
		/// </summary>
		AVX512VL_or_AVX512DQ,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31] or <see cref="AVX512F"/>
		/// </summary>
		AVX512VL_or_AVX512F,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.BMI1[bit 3]
		/// </summary>
		BMI1,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.BMI2[bit 8]
		/// </summary>
		BMI2,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.CLFLUSHOPT[bit 23]
		/// </summary>
		CLFLUSHOPT,

		/// <summary>
		/// CPUID.01H:EDX.CLFSH[bit 19]
		/// </summary>
		CLFSH,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.CLWB[bit 24]
		/// </summary>
		CLWB,

		/// <summary>
		/// CPUID.01H:EDX.CMOV[bit 15]
		/// </summary>
		CMOV,

		/// <summary>
		/// CPUID.01H:ECX.CMPXCHG16B[bit 13]
		/// </summary>
		CMPXCHG16B,

		/// <summary>
		/// RFLAGS.ID can be toggled
		/// </summary>
		CPUID,

		/// <summary>
		/// CPUID.01H:EDX.CX8[bit 8]
		/// </summary>
		CX8,

		/// <summary>
		/// CPUID.(EAX=12H, ECX=0H):EAX.OSS[bit 5]
		/// </summary>
		ENCLV,

		/// <summary>
		/// CPUID.01H:ECX.F16C[bit 29]
		/// </summary>
		F16C,

		/// <summary>
		/// FCOMI/FCOMIP/FUCOMI/FUCOMIP instructions (<see cref="FPU"/> and Pentium Pro or later)
		/// </summary>
		FCOMI,

		/// <summary>
		/// CPUID.01H:ECX.FMA[bit 12]
		/// </summary>
		FMA,

		/// <summary>
		/// 8087 or later (CPUID.01H:EDX.FPU[bit 0])
		/// </summary>
		FPU,

		/// <summary>
		/// <see cref="FPU"/> and <see cref="CMOV"/>
		/// </summary>
		FPU_and_CMOV,

		/// <summary>
		/// <see cref="FPU"/> and <see cref="SSE3"/>
		/// </summary>
		FPU_and_SSE3,

		/// <summary>
		/// 80287 or later
		/// </summary>
		FPU287,

		/// <summary>
		/// 80387 or later
		/// </summary>
		FPU387,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.FSGSBASE[bit 0]
		/// </summary>
		FSGSBASE,

		/// <summary>
		/// CPUID.01H:EDX.FXSR[bit 24]
		/// </summary>
		FXSR,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.HLE[bit 4] or <see cref="RTM"/>
		/// </summary>
		HLE_or_RTM,

		/// <summary>
		/// <see cref="VMX"/> and IA32_VMX_EPT_VPID_CAP[bit 20]
		/// </summary>
		INVEPT,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.INVPCID[bit 10]
		/// </summary>
		INVPCID,

		/// <summary>
		/// <see cref="VMX"/> and IA32_VMX_EPT_VPID_CAP[bit 32]
		/// </summary>
		INVVPID,

		/// <summary>
		/// CPUID.80000001H:ECX.LZCNT[bit 5]
		/// </summary>
		LZCNT,

		/// <summary>
		/// CPUID.01H:EDX.MMX[bit 23]
		/// </summary>
		MMX,

		/// <summary>
		/// CPUID.01H:ECX.MONITOR[bit 3]
		/// </summary>
		MONITOR,

		/// <summary>
		/// CPUID.01H:ECX.MOVBE[bit 22]
		/// </summary>
		MOVBE,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.MPX[bit 14]
		/// </summary>
		MPX,

		/// <summary>
		/// CPUID.01H:EDX.MSR[bit 5]
		/// </summary>
		MSR,

		/// <summary>
		/// Multi-byte nops (0F1F /0): CPUID.01H.EAX[Bits 11:8] = 0110B or 1111B
		/// </summary>
		MULTIBYTENOP,

		/// <summary>
		/// PAUSE instruction (Pentium 4 or later)
		/// </summary>
		PAUSE,

		/// <summary>
		/// CPUID.01H:ECX.PCLMULQDQ[bit 1]
		/// </summary>
		PCLMULQDQ,

		/// <summary>
		/// <see cref="PCLMULQDQ"/> and <see cref="AVX"/>
		/// </summary>
		PCLMULQDQ_and_AVX,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.PKU[bit 3]
		/// </summary>
		PKU,

		/// <summary>
		/// CPUID.01H:ECX.POPCNT[bit 23]
		/// </summary>
		POPCNT,

		/// <summary>
		/// CPUID.80000001H:ECX.PREFETCHW[bit 8]
		/// </summary>
		PREFETCHW,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.PREFETCHWT1[bit 0]
		/// </summary>
		PREFETCHWT1,

		/// <summary>
		/// CPUID.(EAX=14H, ECX=0H):EBX.PTWRITE[bit 4]
		/// </summary>
		PTWRITE,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.RDPID[bit 22]
		/// </summary>
		RDPID,

		/// <summary>
		/// RDPMC instruction (Pentium MMX or later, or Pentium Pro or later)
		/// </summary>
		RDPMC,

		/// <summary>
		/// CPUID.01H:ECX.RDRAND[bit 30]
		/// </summary>
		RDRAND,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.RDSEED[bit 18]
		/// </summary>
		RDSEED,

		/// <summary>
		/// CPUID.80000001H:EDX.RDTSCP[bit 27]
		/// </summary>
		RDTSCP,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.RTM[bit 11]
		/// </summary>
		RTM,

		/// <summary>
		/// CPUID.01H:EDX.SEP[bit 11]
		/// </summary>
		SEP,

		/// <summary>
		/// CPUID.(EAX=12H, ECX=0H):EAX.SGX1[bit 0]
		/// </summary>
		SGX1,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.SHA[bit 29]
		/// </summary>
		SHA,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.SMAP[bit 20]
		/// </summary>
		SMAP,

		/// <summary>
		/// CPUID.01H:ECX.SMX[bit 6]
		/// </summary>
		SMX,

		/// <summary>
		/// CPUID.01H:EDX.SSE[bit 25]
		/// </summary>
		SSE,

		/// <summary>
		/// CPUID.01H:EDX.SSE2[bit 26]
		/// </summary>
		SSE2,

		/// <summary>
		/// CPUID.01H:ECX.SSE3[bit 0]
		/// </summary>
		SSE3,

		/// <summary>
		/// CPUID.01H:ECX.SSE4_1[bit 19]
		/// </summary>
		SSE4_1,

		/// <summary>
		/// CPUID.01H:ECX.SSE4_2[bit 20]
		/// </summary>
		SSE4_2,

		/// <summary>
		/// CPUID.01H:ECX.SSSE3[bit 9]
		/// </summary>
		SSSE3,

		/// <summary>
		/// CPUID.80000001H:EDX.SYSCALL[bit 11]
		/// </summary>
		SYSCALL,

		/// <summary>
		/// CPUID.01H:EDX.TSC[bit 4]
		/// </summary>
		TSC,

		/// <summary>
		/// CPUID.01H:ECX.VMX[bit 5]
		/// </summary>
		VMX,

		/// <summary>
		/// CPUID.01H:ECX.XSAVE[bit 26]
		/// </summary>
		XSAVE,

		/// <summary>
		/// CPUID.(EAX=0DH, ECX=1H):EAX.XSAVEC[bit 1]
		/// </summary>
		XSAVEC,

		/// <summary>
		/// CPUID.(EAX=0DH, ECX=1H):EAX.XSAVEOPT[bit 0]
		/// </summary>
		XSAVEOPT,

		/// <summary>
		/// CPUID.(EAX=0DH, ECX=1H):EAX.XSAVES[bit 3]
		/// </summary>
		XSAVES,

		// If a new value is added, update InfoFlags2.CpuidFeatureMask if needed
	}
}
#endif
