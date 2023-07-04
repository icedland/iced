// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.InstructionInfo {
	[Enum("CpuidFeature", Documentation = "#(c:CPUID)# feature flags", Public = true)]
	enum CpuidFeature {
		[Comment("8086 or later")]
		INTEL8086,
		[Comment("8086 only")]
		INTEL8086_ONLY,
		[Comment("80186 or later")]
		INTEL186,
		[Comment("80286 or later")]
		INTEL286,
		[Comment("80286 only")]
		INTEL286_ONLY,
		[Comment("80386 or later")]
		INTEL386,
		[Comment("80386 only")]
		INTEL386_ONLY,
		[Comment("80386 A0-B0 stepping only (#(c:XBTS)#, #(c:IBTS)# instructions)")]
		INTEL386_A0_ONLY,
		[Comment("Intel486 or later")]
		INTEL486,
		[Comment("Intel486 A stepping only (#(c:CMPXCHG)#)")]
		INTEL486_A_ONLY,
		[Comment("UMOV (80386 and Intel486)")]
		UMOV,
		[Comment("IA-64")]
		IA64,
		[Comment("CPUID.80000001H:EDX.LM[bit 29]")]
		X64,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.ADX[bit 19]")]
		ADX,
		[Comment("CPUID.01H:ECX.AES[bit 25]")]
		AES,
		[Comment("CPUID.01H:ECX.AVX[bit 28]")]
		AVX,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX2[bit 5]")]
		AVX2,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AVX512_4FMAPS[bit 3]")]
		AVX512_4FMAPS,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AVX512_4VNNIW[bit 2]")]
		AVX512_4VNNIW,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.AVX512_BF16[bit 5]")]
		AVX512_BF16,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.AVX512_BITALG[bit 12]")]
		AVX512_BITALG,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512_IFMA[bit 21]")]
		AVX512_IFMA,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VBMI[bit 1]")]
		AVX512_VBMI,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VBMI2[bit 6]")]
		AVX512_VBMI2,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VNNI[bit 11]")]
		AVX512_VNNI,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AVX512_VP2INTERSECT[bit 08]")]
		AVX512_VP2INTERSECT,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.AVX512_VPOPCNTDQ[bit 14]")]
		AVX512_VPOPCNTDQ,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512BW[bit 30]")]
		AVX512BW,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512CD[bit 28]")]
		AVX512CD,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512DQ[bit 17]")]
		AVX512DQ,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512ER[bit 27]")]
		AVX512ER,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512F[bit 16]")]
		AVX512F,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512PF[bit 26]")]
		AVX512PF,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.AVX512VL[bit 31]")]
		AVX512VL,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.BMI1[bit 3]")]
		BMI1,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.BMI2[bit 8]")]
		BMI2,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.CET_IBT[bit 20]")]
		CET_IBT,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.CET_SS[bit 7]")]
		CET_SS,
		[Comment("#(c:CL1INVMB)# instruction (Intel SCC = Single-Chip Computer)")]
		CL1INVMB,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.CLDEMOTE[bit 25]")]
		CLDEMOTE,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.CLFLUSHOPT[bit 23]")]
		CLFLUSHOPT,
		[Comment("CPUID.01H:EDX.CLFSH[bit 19]")]
		CLFSH,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.CLWB[bit 24]")]
		CLWB,
		[Comment("CPUID.80000008H:EBX.CLZERO[bit 0]")]
		CLZERO,
		[Comment("CPUID.01H:EDX.CMOV[bit 15]")]
		CMOV,
		[Comment("CPUID.01H:ECX.CMPXCHG16B[bit 13]")]
		CMPXCHG16B,
		[Comment("#(c:RFLAGS.ID)# can be toggled")]
		CPUID,
		[Comment("CPUID.01H:EDX.CX8[bit 8]")]
		CX8,
		[Comment("CPUID.80000001H:EDX.3DNOW[bit 31]")]
		D3NOW,
		[Comment("CPUID.80000001H:EDX.3DNOWEXT[bit 30]")]
		D3NOWEXT,
		[Comment("CPUID.(EAX=12H, ECX=0H):EAX.OSS[bit 5]")]
		OSS,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.ENQCMD[bit 29]")]
		ENQCMD,
		[Comment("CPUID.01H:ECX.F16C[bit 29]")]
		F16C,
		[Comment("CPUID.01H:ECX.FMA[bit 12]")]
		FMA,
		[Comment("CPUID.80000001H:ECX.FMA4[bit 16]")]
		FMA4,
		[Comment("8087 or later (CPUID.01H:EDX.FPU[bit 0])")]
		FPU,
		[Comment("80287 or later")]
		FPU287,
		[Comment("80287XL only")]
		FPU287XL_ONLY,
		[Comment("80387 or later")]
		FPU387,
		[Comment("80387SL only")]
		FPU387SL_ONLY,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.FSGSBASE[bit 0]")]
		FSGSBASE,
		[Comment("CPUID.01H:EDX.FXSR[bit 24]")]
		FXSR,
		[Comment("Cyrix (AMD Geode GX/LX) 3DNow! instructions")]
		CYRIX_D3NOW,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.GFNI[bit 8]")]
		GFNI,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.HLE[bit 4]")]
		HLE,
		[Comment("#(e:HLE)# or #(e:RTM)#")]
		HLE_or_RTM,
		[Comment("IA32_VMX_EPT_VPID_CAP[bit 20]")]
		INVEPT,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.INVPCID[bit 10]")]
		INVPCID,
		[Comment("IA32_VMX_EPT_VPID_CAP[bit 32]")]
		INVVPID,
		[Comment("CPUID.80000001H:ECX.LWP[bit 15]")]
		LWP,
		[Comment("CPUID.80000001H:ECX.LZCNT[bit 5]")]
		LZCNT,
		[Comment("CPUID.80000008H:EBX.MCOMMIT[bit 8]")]
		MCOMMIT,
		[Comment("CPUID.01H:EDX.MMX[bit 23]")]
		MMX,
		[Comment("CPUID.01H:ECX.MONITOR[bit 3]")]
		MONITOR,
		[Comment("CPUID.80000001H:ECX.MONITORX[bit 29]")]
		MONITORX,
		[Comment("CPUID.01H:ECX.MOVBE[bit 22]")]
		MOVBE,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.MOVDIR64B[bit 28]")]
		MOVDIR64B,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.MOVDIRI[bit 27]")]
		MOVDIRI,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.MPX[bit 14]")]
		MPX,
		[Comment("CPUID.01H:EDX.MSR[bit 5]")]
		MSR,
		[Comment("Multi-byte nops (#(c:0F1F /0)#): CPUID.01H.EAX[Bits 11:8] = 0110B or 1111B")]
		MULTIBYTENOP,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.ACE[Bits 7:6] = 11B ([6] = exists, [7] = enabled)")]
		PADLOCK_ACE,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.PHE[Bits 11:10] = 11B ([10] = exists, [11] = enabled)")]
		PADLOCK_PHE,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.PMM[Bits 13:12] = 11B ([12] = exists, [13] = enabled)")]
		PADLOCK_PMM,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.RNG[Bits 3:2] = 11B ([2] = exists, [3] = enabled)")]
		PADLOCK_RNG,
		[Comment("#(c:PAUSE)# instruction (Pentium 4 or later)")]
		PAUSE,
		[Comment("CPUID.01H:ECX.PCLMULQDQ[bit 1]")]
		PCLMULQDQ,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.PCOMMIT[bit 22]")]
		PCOMMIT,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.PCONFIG[bit 18]")]
		PCONFIG,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.PKU[bit 3]")]
		PKU,
		[Comment("CPUID.01H:ECX.POPCNT[bit 23]")]
		POPCNT,
		[Comment("CPUID.80000001H:ECX.PREFETCHW[bit 8]")]
		PREFETCHW,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.PREFETCHWT1[bit 0]")]
		PREFETCHWT1,
		[Comment("CPUID.(EAX=14H, ECX=0H):EBX.PTWRITE[bit 4]")]
		PTWRITE,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.RDPID[bit 22]")]
		RDPID,
		[Comment("#(c:RDPMC)# instruction (Pentium MMX or later, or Pentium Pro or later)")]
		RDPMC,
		[Comment("CPUID.80000008H:EBX.RDPRU[bit 4]")]
		RDPRU,
		[Comment("CPUID.01H:ECX.RDRAND[bit 30]")]
		RDRAND,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.RDSEED[bit 18]")]
		RDSEED,
		[Comment("CPUID.80000001H:EDX.RDTSCP[bit 27]")]
		RDTSCP,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.RTM[bit 11]")]
		RTM,
		[Comment("CPUID.01H:EDX.SEP[bit 11]")]
		SEP,
		[Comment("CPUID.(EAX=12H, ECX=0H):EAX.SGX1[bit 0]")]
		SGX1,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.SHA[bit 29]")]
		SHA,
		[Comment("CPUID.80000001H:ECX.SKINIT[bit 12]")]
		SKINIT,
		[Comment("#(e:SKINIT)# or #(e:SVM)#")]
		SKINIT_or_SVM,
		[Comment("CPUID.(EAX=07H, ECX=0H):EBX.SMAP[bit 20]")]
		SMAP,
		[Comment("CPUID.01H:ECX.SMX[bit 6]")]
		SMX,
		[Comment("CPUID.01H:EDX.SSE[bit 25]")]
		SSE,
		[Comment("CPUID.01H:EDX.SSE2[bit 26]")]
		SSE2,
		[Comment("CPUID.01H:ECX.SSE3[bit 0]")]
		SSE3,
		[Comment("CPUID.01H:ECX.SSE4_1[bit 19]")]
		SSE4_1,
		[Comment("CPUID.01H:ECX.SSE4_2[bit 20]")]
		SSE4_2,
		[Comment("CPUID.80000001H:ECX.SSE4A[bit 6]")]
		SSE4A,
		[Comment("CPUID.01H:ECX.SSSE3[bit 9]")]
		SSSE3,
		[Comment("CPUID.80000001H:ECX.SVM[bit 2]")]
		SVM,
		[Comment("CPUID.8000001FH:EAX.SEV-ES[bit 3]")]
		SEV_ES,
		[Comment("CPUID.80000001H:EDX.SYSCALL[bit 11]")]
		SYSCALL,
		[Comment("CPUID.80000001H:ECX.TBM[bit 21]")]
		TBM,
		[Comment("CPUID.01H:EDX.TSC[bit 4]")]
		TSC,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.VAES[bit 9]")]
		VAES,
		[Comment("CPUID.01H:ECX.VMX[bit 5]")]
		VMX,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.VPCLMULQDQ[bit 10]")]
		VPCLMULQDQ,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.WAITPKG[bit 5]")]
		WAITPKG,
		[Comment("CPUID.(EAX=80000008H, ECX=0H):EBX.WBNOINVD[bit 9]")]
		WBNOINVD,
		[Comment("CPUID.80000001H:ECX.XOP[bit 11]")]
		XOP,
		[Comment("CPUID.01H:ECX.XSAVE[bit 26]")]
		XSAVE,
		[Comment("CPUID.(EAX=0DH, ECX=1H):EAX.XSAVEC[bit 1]")]
		XSAVEC,
		[Comment("CPUID.(EAX=0DH, ECX=1H):EAX.XSAVEOPT[bit 0]")]
		XSAVEOPT,
		[Comment("CPUID.(EAX=0DH, ECX=1H):EAX.XSAVES[bit 3]")]
		XSAVES,
		[Comment("CPUID.8000001FH:EAX.SEV-SNP[bit 4]")]
		SEV_SNP,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.SERIALIZE[bit 14]")]
		SERIALIZE,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.TSXLDTRK[bit 16]")]
		TSXLDTRK,
		[Comment("CPUID.80000008H:EBX.INVLPGB[bit 3]")]
		INVLPGB,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AMX-BF16[bit 22]")]
		AMX_BF16,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AMX-TILE[bit 24]")]
		AMX_TILE,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AMX-INT8[bit 25]")]
		AMX_INT8,
		[Comment("Cyrix FPU instructions (Cyrix, AMD Geode GX/LX)")]
		CYRIX_FPU,
		[Comment("Cyrix SMM instructions: #(c:SVDC)#, #(c:RSDC)#, #(c:SVLDT)#, #(c:RSLDT)#, #(c:SVTS)#, #(c:RSTS)# (Cyrix, AMD Geode GX/LX)")]
		CYRIX_SMM,
		[Comment("Cyrix #(c:SMINT 0F38)# (6x86MX and later, AMD Geode GX/LX)")]
		CYRIX_SMINT,
		[Comment("Cyrix #(c:SMINT 0F7E)# (6x86 or earlier)")]
		CYRIX_SMINT_0F7E,
		[Comment("Cyrix SMM instructions: #(c:RDSHR)#, #(c:WRSHR)# (6x86MX, M II, Cyrix III)")]
		CYRIX_SHR,
		[Comment("Cyrix DDI instructions: #(c:BB0_Reset)#, #(c:BB1_Reset)#, #(c:CPU_READ)#, #(c:CPU_WRITE)# (MediaGX, GXm, GXLV, GX1)")]
		CYRIX_DDI,
		[Comment("Cyrix AND CPUID.80000001H:EDX.EMMI[bit 24]")]
		CYRIX_EMMI,
		[Comment("Cyrix DMI instructions: #(c:DMINT)#, #(c:RDM)# (AMD Geode GX/LX)")]
		CYRIX_DMI,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.AIS[Bits 1:0] = 11B ([0] = exists, [1] = enabled)")]
		CENTAUR_AIS,
		[Comment("MOV to/from TR (80386, Intel486, Cyrix, Geode)")]
		MOV_TR,
		[Comment("#(c:RSM)# instruction (some 386s, some 486s, Pentium and later)")]
		SMM,
		[Comment("CPUID.(EAX=??H, ECX=?H):???.????[bit ??]")]
		TDX,
		[Comment("CPUID.(EAX=07H, ECX=0H):ECX.KL[bit 23]")]
		KL,
		[Comment("CPUID.19H:EBX.AESKLE[bit 0]")]
		AESKLE,
		[Comment("CPUID.19H:EBX.WIDE_KL[bit 2]")]
		WIDE_KL,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.UINTR[bit 5]")]
		UINTR,
		[Comment("CPUID.(EAX=07H, ECX=01H):EAX.HRESET[bit 22]")]
		HRESET,
		[Comment("CPUID.(EAX=07H, ECX=01H):EAX.AVX-VNNI[bit 4]")]
		AVX_VNNI,
		[Comment("CPUID.0C0000000H:EAX >= 0C0000001H AND CPUID.0C0000001H:EDX.GMI[Bits 5:4] = 11B ([4] = exists, [5] = enabled)")]
		PADLOCK_GMI,
		[Comment("CPUID.(EAX=07H, ECX=01H):EAX.FRED[bit 17]")]
		FRED,
		[Comment("CPUID.(EAX=07H, ECX=01H):EAX.LKGS[bit 18]")]
		LKGS,
		[Comment("CPUID.(EAX=07H, ECX=0H):EDX.AVX512-FP16[bit 23]")]
		AVX512_FP16,
		[Comment("Undocumented Intel #(c:RDUDBG)# and #(c:WRUDBG)# instructions")]
		UDBG,
		[Comment("Intel Knights Corner")]
		KNC,
		[Comment("Undocumented instruction")]
		PADLOCK_UNDOC,
		[Comment("CPUID.8000001FH:EAX.RMPQUERY[bit 6]")]
		RMPQUERY,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.RAO-INT[bit 3]")]
		RAO_INT,
		[Comment("CPUID.(EAX=07H, ECX=1H):EDX.PREFETCHITI[bit 14]")]
		PREFETCHITI,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.AMX-FP16[bit 21]")]
		AMX_FP16,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.CMPCCXADD[bit 7]")]
		CMPCCXADD,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.AVX-IFMA[bit 23]")]
		AVX_IFMA,
		[Comment("CPUID.(EAX=07H, ECX=1H):EDX.AVX-NE-CONVERT[bit 5]")]
		AVX_NE_CONVERT,
		[Comment("CPUID.(EAX=07H, ECX=1H):EDX.AVX-VNNI-INT8[bit 4]")]
		AVX_VNNI_INT8,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.MSRLIST[bit 27]")]
		MSRLIST,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.WRMSRNS[bit 19]")]
		WRMSRNS,
		[Comment("CPUID.(EAX=07H, ECX=1H):EDX.AMX-COMPLEX[bit 8]")]
		AMX_COMPLEX,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.SHA512[bit 0]")]
		SHA512,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.SM3[bit 1]")]
		SM3,
		[Comment("CPUID.(EAX=07H, ECX=1H):EAX.SM4[bit 2]")]
		SM4,
		[Comment("CPUID.(EAX=07H, ECX=1H):EBX.TSE[bit 1]")]
		TSE,
		[Comment("CPUID.(EAX=07H, ECX=1H):EDX.AVX-VNNI-INT16[bit 10]")]
		AVX_VNNI_INT16,
	}
}
