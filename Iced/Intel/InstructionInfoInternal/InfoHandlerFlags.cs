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
namespace Iced.Intel.InstructionInfoInternal {
	enum InfoFlags1 : uint {
		// Only used by the test code
		CodeMask			= (1U << Instruction.TEST_CodeBits) - 1,

		RflagsInfoShift		= 14,
		RflagsInfoMask		= 0x3F,
		CodeInfoShift		= 20,
		CodeInfoMask		= 0x7F,
		SaveRestore			= 0x08000000,
		StackInstruction	= 0x10000000,
		ProtectedMode		= 0x20000000,
		Privileged			= 0x40000000,
		NoSegmentRead		= 0x80000000,
	}

	enum InfoFlags2 : uint {
		OpInfo0Shift		= 0,
		OpInfo0Mask			= 0xF,
		OpInfo1Shift		= 4,
		OpInfo1Mask			= 7,
		OpInfo2Shift		= 7,
		OpInfo2Mask			= 3,
		OpInfo3Shift		= 9,
		OpInfo3Mask			= 1,

		OpMaskRegReadWrite	= 0x00010000,
		EncodingShift		= 17,
		EncodingMask		= 3,
		FlowControlShift	= 21,
		FlowControlMask		= 0xF,
		CpuidFeatureShift	= 25,
		CpuidFeatureMask	= 0x7F,
	}

	enum OpInfo0 {
		None,
		Read,
		Write,
		// Don't convert Write to ReadWrite, eg. EVEX_Vblendmpd_VX_k1z_HX_WX_b since it always overwrites dest
		WriteForce,
		CondWrite,
		// CMOVcc with GPR32 dest in 64-bit mode: upper 32 bits of full 64-bit reg are always cleared.
		CondWrite32_ReadWrite64,
		ReadWrite,
		ReadCondWrite,
		NoMemAccess,
		WriteMem_ReadWriteReg,
		// If more values are added, update InfoFlags2 if needed
	}

	enum OpInfo1 {
		None,
		Read,
		ReadP3,
		Write,
		CondRead,
		ReadWrite,
		NoMemAccess,
		// If more values are added, update InfoFlags2 if needed

		Last,
	}

	enum OpInfo2 {
		None,
		Read,
		ReadWrite,
		// If more values are added, update InfoFlags2 if needed

		Last,
	}

	enum OpInfo3 {
		None,
		Read,
		// If more values are added, update InfoFlags2 if needed

		Last,
	}

	enum CodeInfo {
		None,
		Cdq,
		Cdqe,
		Cmps,
		Cmpxchg,
		Cmpxchg8b,
		Cpuid,
		Cqo,
		Cwd,
		Cwde,
		Div,
		Encls,
		Enter,
		Ins,
		Iret,
		Jrcxz,
		Lahf,
		Lds,
		Leave,
		Lods,
		Loop,
		Maskmovq,
		Monitor,
		Movs,
		Mul,
		Mulx,
		Mwait,
		Outs,
		PcmpXstrY,
		Pop_2,
		Pop_2_2,
		Pop_4,
		Pop_4_4,
		Pop_8,
		Pop_8_8,
		Pop_Ev,
		Popa,
		Push_2,
		Push_2_2,
		Push_4,
		Push_4_4,
		Push_8,
		Push_8_8,
		Pusha,
		R_AL_W_AH,
		R_AL_W_AX,
		R_CR0,
		R_EAX_ECX_EDX,
		R_EAX_EDX,
		R_ECX_W_EAX_EDX,
		R_ST0,
		R_ST0_R_ST1,
		R_ST0_RW_ST1,
		R_ST0_ST1,
		R_XMM0,
		RW_AL,
		RW_AX,
		RW_CR0,
		RW_ST0,
		RW_ST0_R_ST1,
		Salc,
		Scas,
		Shift_Ib_MASK1FMOD9,
		Shift_Ib_MASK1FMOD11,
		Shift_Ib_MASK1F,
		Shift_Ib_MASK3F,
		Stos,
		Syscall,
		Vmfunc,
		Vzeroall,
		W_EAX_ECX_EDX,
		W_EAX_EDX,
		W_ST0,
	}

	enum RflagsInfo {
		None,
		C_AC,
		C_c,
		C_d,
		C_i,
		R_a_W_ac_U_opsz,
		R_ac_W_acpsz_U_o,
		R_acopszid,
		R_acopszidAC,
		R_acpsz,
		R_c,
		R_c_W_acopsz,
		R_c_W_c,
		R_c_W_co,
		R_cz,
		R_d,
		R_d_W_acopsz,
		R_o,
		R_o_W_o,
		R_os,
		R_osz,
		R_p,
		R_s,
		R_z,
		S_AC,
		S_c,
		S_d,
		S_i,
		U_acopsz,
		W_acopsz,
		W_acopszid,
		W_acopszidAC,
		W_acpsz,
		W_aopsz,
		W_c_C_aopsz,
		W_c_U_aops,
		W_co,
		W_co_U_apsz,
		W_copsz_U_a,
		W_cosz_C_ap,
		W_cpz_C_aos,
		W_cs_C_oz_U_ap,
		W_csz_C_o_U_ap,
		W_cz_C_aops,
		W_cz_U_aops,
		W_psz_C_co_U_a,
		W_psz_U_aco,
		W_sz_C_co_U_ap,
		W_z,
		W_z_C_acops,
		W_z_C_co_U_aps,
		W_z_U_acops,

		// If a new value is added, update InfoFlags1.RflagsInfoMask if needed

		Last,
	}
}
#endif
