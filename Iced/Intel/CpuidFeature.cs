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
		/// 8086 only
		/// </summary>
		INTEL8086_ONLY,

		/// <summary>
		/// 80186 or later
		/// </summary>
		INTEL186,

		/// <summary>
		/// 80286 or later
		/// </summary>
		INTEL286,

		/// <summary>
		/// 80286 only
		/// </summary>
		INTEL286_ONLY,

		/// <summary>
		/// 80386 or later
		/// </summary>
		INTEL386,

		/// <summary>
		/// 80386 only
		/// </summary>
		INTEL386_ONLY,

		/// <summary>
		/// 80386 A0-B0 stepping only (xbts, ibts instructions)
		/// </summary>
		INTEL386_A0_ONLY,

		/// <summary>
		/// Intel486 or later
		/// </summary>
		INTEL486,

		/// <summary>
		/// Intel486 A stepping only (cmpxchg)
		/// </summary>
		INTEL486_A_ONLY,

		/// <summary>
		/// 80386 and Intel486 only
		/// </summary>
		INTEL386_486_ONLY,

		/// <summary>
		/// IA-64
		/// </summary>
		IA64,

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
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_BITALG[bit 12]
		/// </summary>
		AVX512_BITALG,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512_IFMA[bit 21]
		/// </summary>
		AVX512_IFMA,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VBMI[bit 1]
		/// </summary>
		AVX512_VBMI,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VBMI2[bit 6] 
		/// </summary>
		AVX512_VBMI2,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VNNI[bit 11]
		/// </summary>
		AVX512_VNNI,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VPOPCNTDQ[bit 14]
		/// </summary>
		AVX512_VPOPCNTDQ,

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
		/// CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31]
		/// </summary>
		AVX512VL,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.BMI1[bit 3]
		/// </summary>
		BMI1,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.BMI2[bit 8]
		/// </summary>
		BMI2,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EDX.CET_IBT[bit 20]
		/// </summary>
		CET_IBT,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.CET_SS[bit 7]
		/// </summary>
		CET_SS,

		/// <summary>
		/// CFLSH instruction (never implemented)
		/// </summary>
		CFLSH,

		/// <summary>
		/// CL1INVMB instruction (Intel SCC = Single-Chip Computer)
		/// </summary>
		CL1INVMB,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.CLDEMOTE[bit 25]
		/// </summary>
		CLDEMOTE,

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
		/// CPUID.80000008H:EBX.CLZERO[bit 0]
		/// </summary>
		CLZERO,

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
		/// CPUID.80000001H:EDX.3DNOW[bit 31]
		/// </summary>
		D3NOW,

		/// <summary>
		/// CPUID.80000001H:EDX.3DNOWEXT[bit 30]
		/// </summary>
		D3NOWEXT,

		/// <summary>
		/// Never implemented: CPUID.01H:EDX.ECR[bit 11]
		/// </summary>
		ECR,

		/// <summary>
		/// CPUID.(EAX=12H, ECX=0H):EAX.OSS[bit 5]
		/// </summary>
		ENCLV,

		/// <summary>
		/// CPUID.01H:ECX.F16C[bit 29]
		/// </summary>
		F16C,

		/// <summary>
		/// CPUID.01H:ECX.FMA[bit 12]
		/// </summary>
		FMA,

		/// <summary>
		/// CPUID.80000001H:ECX.FMA4[bit 16]
		/// </summary>
		FMA4,

		/// <summary>
		/// 8087 or later (CPUID.01H:EDX.FPU[bit 0])
		/// </summary>
		FPU,

		/// <summary>
		/// 80287 or later
		/// </summary>
		FPU287,

		/// <summary>
		/// 80287XL only
		/// </summary>
		FPU287XL_ONLY,

		/// <summary>
		/// 80387 or later
		/// </summary>
		FPU387,

		/// <summary>
		/// 80387SL only
		/// </summary>
		FPU387SL_ONLY,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.FSGSBASE[bit 0]
		/// </summary>
		FSGSBASE,

		/// <summary>
		/// CPUID.01H:EDX.FXSR[bit 24]
		/// </summary>
		FXSR,

		/// <summary>
		/// AMD Geode LX/GX CPU
		/// </summary>
		GEODE,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.GFNI[bit 8]
		/// </summary>
		GFNI,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.HLE[bit 4]
		/// </summary>
		HLE,

		/// <summary>
		/// <see cref="HLE"/> or <see cref="RTM"/>
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
		/// CPUID.80000001H:ECX.LWP[bit 15]
		/// </summary>
		LWP,

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
		/// CPUID.80000001H:ECX.MONITORX[bit 29]
		/// </summary>
		MONITORX,

		/// <summary>
		/// CPUID.01H:ECX.MOVBE[bit 22]
		/// </summary>
		MOVBE,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.MOVDIR64B[bit 28]
		/// </summary>
		MOVDIR64B,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.MOVDIRI[bit 27]
		/// </summary>
		MOVDIRI,

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
		/// CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.ACE[Bits 7:6] = 11B ([6] = exists, [7] = enabled)
		/// </summary>
		PADLOCK_ACE,

		/// <summary>
		/// CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.PHE[Bits 11:10] = 11B ([10] = exists, [11] = enabled)
		/// </summary>
		PADLOCK_PHE,

		/// <summary>
		/// CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.PMM[Bits 13:12] = 11B ([12] = exists, [13] = enabled)
		/// </summary>
		PADLOCK_PMM,

		/// <summary>
		/// CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.RNG[Bits 3:2] = 11B ([2] = exists, [3] = enabled)
		/// </summary>
		PADLOCK_RNG,

		/// <summary>
		/// PAUSE instruction (Pentium 4 or later)
		/// </summary>
		PAUSE,

		/// <summary>
		/// CPUID.01H:ECX.PCLMULQDQ[bit 1]
		/// </summary>
		PCLMULQDQ,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EBX.PCOMMIT[bit 22]
		/// </summary>
		PCOMMIT,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):EDX.PCONFIG[bit 18]
		/// </summary>
		PCONFIG,

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
		/// CPUID.80000001H:ECX.SKINIT[bit 12]
		/// </summary>
		SKINIT,

		/// <summary>
		/// <see cref="SKINIT"/> or <see cref="SVML"/>
		/// </summary>
		SKINIT_or_SVML,

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
		/// CPUID.80000001H:ECX.SSE4A[bit 6]
		/// </summary>
		SSE4A,

		/// <summary>
		/// CPUID.01H:ECX.SSSE3[bit 9]
		/// </summary>
		SSSE3,

		/// <summary>
		/// CPUID.80000001H:ECX.SVM[bit 2]
		/// </summary>
		SVM,

		/// <summary>
		/// CPUID.8000000AH:EDX.SVML[bit 2]
		/// </summary>
		SVML,

		/// <summary>
		/// CPUID.80000001H:EDX.SYSCALL[bit 11]
		/// </summary>
		SYSCALL,

		/// <summary>
		/// CPUID.80000001H:ECX.TBM[bit 21]
		/// </summary>
		TBM,

		/// <summary>
		/// CPUID.01H:EDX.TSC[bit 4]
		/// </summary>
		TSC,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.VAES[bit 9]
		/// </summary>
		VAES,

		/// <summary>
		/// CPUID.01H:ECX.VMX[bit 5]
		/// </summary>
		VMX,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.VPCLMULQDQ[bit 10]
		/// </summary>
		VPCLMULQDQ,

		/// <summary>
		/// CPUID.(EAX=07H, ECX=0H):ECX.WAITPKG[bit 5]
		/// </summary>
		WAITPKG,

		/// <summary>
		/// CPUID.(EAX=80000008H, ECX=0H):EBX.WBNOINVD[bit 9]
		/// </summary>
		WBNOINVD,

		/// <summary>
		/// CPUID.80000001H:ECX.XOP[bit 11]
		/// </summary>
		XOP,

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

		/// <summary>
		/// Never implemented: CPUID.01H:EDX.ZALLOC[bit 16]
		/// </summary>
		ZALLOC,
	}
}
#endif
