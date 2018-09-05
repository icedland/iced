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

namespace Iced.Intel {
	/// <summary>
	/// x86 instruction code
	/// </summary>
	public enum Code {
#pragma warning disable 1591 // Missing XML comment for publicly visible type or member
		/// <summary>
		/// It's an invalid instruction, eg. it's a new unknown instruction, garbage or
		/// there's not enough bytes to decode the instruction etc.
		/// </summary>
		INVALID = 0,

		Add_Eb_Gb,											// 00
		Add_Ew_Gw,											// o16 01
		Add_Ed_Gd,											// o32 01
		Add_Eq_Gq,											// REX.W 01
		Add_Gb_Eb,											// 02
		Add_Gw_Ew,											// o16 03
		Add_Gd_Ed,											// o32 03
		Add_Gq_Eq,											// REX.W 03
		Add_AL_Ib,											// 04
		Add_AX_Iw,											// o16 05
		Add_EAX_Id,											// o32 05
		Add_RAX_Id64,										// REX.W 05
		Pushw_ES,											// o16 06
		Pushd_ES,											// o32 06
		Popw_ES,											// o16 07
		Popd_ES,											// o32 07

		Or_Eb_Gb,											// 08
		Or_Ew_Gw,											// o16 09
		Or_Ed_Gd,											// o32 09
		Or_Eq_Gq,											// REX.W 09
		Or_Gb_Eb,											// 0A
		Or_Gw_Ew,											// o16 0B
		Or_Gd_Ed,											// o32 0B
		Or_Gq_Eq,											// REX.W 0B
		Or_AL_Ib,											// 0C
		Or_AX_Iw,											// o16 0D
		Or_EAX_Id,											// o32 0D
		Or_RAX_Id64,										// REX.W 0D
		Pushw_CS,											// o16 0E
		Pushd_CS,											// o32 0E
		// 2-byte esc										// 0F

		Adc_Eb_Gb,											// 10
		Adc_Ew_Gw,											// o16 11
		Adc_Ed_Gd,											// o32 11
		Adc_Eq_Gq,											// REX.W 11
		Adc_Gb_Eb,											// 12
		Adc_Gw_Ew,											// o16 13
		Adc_Gd_Ed,											// o32 13
		Adc_Gq_Eq,											// REX.W 13
		Adc_AL_Ib,											// 14
		Adc_AX_Iw,											// o16 15
		Adc_EAX_Id,											// o32 15
		Adc_RAX_Id64,										// REX.W 15
		Pushw_SS,											// o16 16
		Pushd_SS,											// o32 16
		Popw_SS,											// o16 17
		Popd_SS,											// o32 17

		Sbb_Eb_Gb,											// 18
		Sbb_Ew_Gw,											// o16 19
		Sbb_Ed_Gd,											// o32 19
		Sbb_Eq_Gq,											// REX.W 19
		Sbb_Gb_Eb,											// 1A
		Sbb_Gw_Ew,											// o16 1B
		Sbb_Gd_Ed,											// o32 1B
		Sbb_Gq_Eq,											// REX.W 1B
		Sbb_AL_Ib,											// 1C
		Sbb_AX_Iw,											// o16 1D
		Sbb_EAX_Id,											// o32 1D
		Sbb_RAX_Id64,										// REX.W 1D
		Pushw_DS,											// o16 1E
		Pushd_DS,											// o32 1E
		Popw_DS,											// o16 1F
		Popd_DS,											// o32 1F

		And_Eb_Gb,											// 20
		And_Ew_Gw,											// o16 21
		And_Ed_Gd,											// o32 21
		And_Eq_Gq,											// REX.W 21
		And_Gb_Eb,											// 22
		And_Gw_Ew,											// o16 23
		And_Gd_Ed,											// o32 23
		And_Gq_Eq,											// REX.W 23
		And_AL_Ib,											// 24
		And_AX_Iw,											// o16 25
		And_EAX_Id,											// o32 25
		And_RAX_Id64,										// REX.W 25
		// ES:												// 26
		Daa,												// 27

		Sub_Eb_Gb,											// 28
		Sub_Ew_Gw,											// o16 29
		Sub_Ed_Gd,											// o32 29
		Sub_Eq_Gq,											// REX.W 29
		Sub_Gb_Eb,											// 2A
		Sub_Gw_Ew,											// o16 2B
		Sub_Gd_Ed,											// o32 2B
		Sub_Gq_Eq,											// REX.W 2B
		Sub_AL_Ib,											// 2C
		Sub_AX_Iw,											// o16 2D
		Sub_EAX_Id,											// o32 2D
		Sub_RAX_Id64,										// REX.W 2D
		// CS:												// 2E
		Das,												// 2F

		Xor_Eb_Gb,											// 30
		Xor_Ew_Gw,											// o16 31
		Xor_Ed_Gd,											// o32 31
		Xor_Eq_Gq,											// REX.W 31
		Xor_Gb_Eb,											// 32
		Xor_Gw_Ew,											// o16 33
		Xor_Gd_Ed,											// o32 33
		Xor_Gq_Eq,											// REX.W 33
		Xor_AL_Ib,											// 34
		Xor_AX_Iw,											// o16 35
		Xor_EAX_Id,											// o32 35
		Xor_RAX_Id64,										// REX.W 35
		// SS:												// 36
		Aaa,												// 37

		Cmp_Eb_Gb,											// 38
		Cmp_Ew_Gw,											// o16 39
		Cmp_Ed_Gd,											// o32 39
		Cmp_Eq_Gq,											// REX.W 39
		Cmp_Gb_Eb,											// 3A
		Cmp_Gw_Ew,											// o16 3B
		Cmp_Gd_Ed,											// o32 3B
		Cmp_Gq_Eq,											// REX.W 3B
		Cmp_AL_Ib,											// 3C
		Cmp_AX_Iw,											// o16 3D
		Cmp_EAX_Id,											// o32 3D
		Cmp_RAX_Id64,										// REX.W 3D
		// DS:												// 3E
		Aas,												// 3F

		Inc_AX,												// o16 40
		Inc_EAX,											// o32 40
		Inc_CX,												// o16 41
		Inc_ECX,											// o32 41
		Inc_DX,												// o16 42
		Inc_EDX,											// o32 42
		Inc_BX,												// o16 43
		Inc_EBX,											// o32 43
		Inc_SP,												// o16 44
		Inc_ESP,											// o32 44
		Inc_BP,												// o16 45
		Inc_EBP,											// o32 45
		Inc_SI,												// o16 46
		Inc_ESI,											// o32 46
		Inc_DI,												// o16 47
		Inc_EDI,											// o32 47

		Dec_AX,												// o16 48
		Dec_EAX,											// o32 48
		Dec_CX,												// o16 49
		Dec_ECX,											// o32 49
		Dec_DX,												// o16 4A
		Dec_EDX,											// o32 4A
		Dec_BX,												// o16 4B
		Dec_EBX,											// o32 4B
		Dec_SP,												// o16 4C
		Dec_ESP,											// o32 4C
		Dec_BP,												// o16 4D
		Dec_EBP,											// o32 4D
		Dec_SI,												// o16 4E
		Dec_ESI,											// o32 4E
		Dec_DI,												// o16 4F
		Dec_EDI,											// o32 4F

		Push_AX,											// o16 50
		Push_R8W,											// o16 REX.B 50
		Push_EAX,											// o32 50
		Push_RAX,											// 50
		Push_R8,											// REX.B 50
		Push_CX,											// o16 51
		Push_R9W,											// o16 REX.B 51
		Push_ECX,											// o32 51
		Push_RCX,											// 51
		Push_R9,											// REX.B 51
		Push_DX,											// o16 52
		Push_R10W,											// o16 REX.B 52
		Push_EDX,											// o32 52
		Push_RDX,											// 52
		Push_R10,											// REX.B 52
		Push_BX,											// o16 53
		Push_R11W,											// o16 REX.B 53
		Push_EBX,											// o32 53
		Push_RBX,											// 53
		Push_R11,											// REX.B 53
		Push_SP,											// o16 54
		Push_R12W,											// o16 REX.B 54
		Push_ESP,											// o32 54
		Push_RSP,											// 54
		Push_R12,											// REX.B 54
		Push_BP,											// o16 55
		Push_R13W,											// o16 REX.B 55
		Push_EBP,											// o32 55
		Push_RBP,											// 55
		Push_R13,											// REX.B 55
		Push_SI,											// o16 56
		Push_R14W,											// o16 REX.B 56
		Push_ESI,											// o32 56
		Push_RSI,											// 56
		Push_R14,											// REX.B 56
		Push_DI,											// o16 57
		Push_R15W,											// o16 REX.B 57
		Push_EDI,											// o32 57
		Push_RDI,											// 57
		Push_R15,											// REX.B 57

		Pop_AX,												// o16 58
		Pop_R8W,											// o16 REX.B 58
		Pop_EAX,											// o32 58
		Pop_RAX,											// 58
		Pop_R8,												// REX.B 58
		Pop_CX,												// o16 59
		Pop_R9W,											// o16 REX.B 59
		Pop_ECX,											// o32 59
		Pop_RCX,											// 59
		Pop_R9,												// REX.B 59
		Pop_DX,												// o16 5A
		Pop_R10W,											// o16 REX.B 5A
		Pop_EDX,											// o32 5A
		Pop_RDX,											// 5A
		Pop_R10,											// REX.B 5A
		Pop_BX,												// o16 5B
		Pop_R11W,											// o16 REX.B 5B
		Pop_EBX,											// o32 5B
		Pop_RBX,											// 5B
		Pop_R11,											// REX.B 5B
		Pop_SP,												// o16 5C
		Pop_R12W,											// o16 REX.B 5C
		Pop_ESP,											// o32 5C
		Pop_RSP,											// 5C
		Pop_R12,											// REX.B 5C
		Pop_BP,												// o16 5D
		Pop_R13W,											// o16 REX.B 5D
		Pop_EBP,											// o32 5D
		Pop_RBP,											// 5D
		Pop_R13,											// REX.B 5D
		Pop_SI,												// o16 5E
		Pop_R14W,											// o16 REX.B 5E
		Pop_ESI,											// o32 5E
		Pop_RSI,											// 5E
		Pop_R14,											// REX.B 5E
		Pop_DI,												// o16 5F
		Pop_R15W,											// o16 REX.B 5F
		Pop_EDI,											// o32 5F
		Pop_RDI,											// 5F
		Pop_R15,											// REX.B 5F

		Pushaw,												// o16 60
		Pushad,												// o32 60
		Popaw,												// o16 61
		Popad,												// o32 61
		Bound_Gw_Mw2,										// o16 62
		Bound_Gd_Md2,										// o32 62
		Arpl_Ew_Gw,											// 63
		Movsxd_Gw_Ew,										// o16 63
		Movsxd_Gd_Ed,										// o32 63
		Movsxd_Gq_Ed,										// REX.W 63
		// FS:												// 64
		// GS:												// 65
		// os												// 66
		// as												// 67

		Push_Iw,											// o16 68
		Push_Id,											// o32 68
		Push_Id64,											// REX.W 68
		Imul_Gw_Ew_Iw,										// o16 69
		Imul_Gd_Ed_Id,										// o32 69
		Imul_Gq_Eq_Id64,									// REX.W 69
		Push_Ib16,											// o16 6A
		Push_Ib32,											// o32 6A
		Push_Ib64,											// REX.W 6A
		Imul_Gw_Ew_Ib16,									// o16 6B
		Imul_Gd_Ed_Ib32,									// o32 6B
		Imul_Gq_Eq_Ib64,									// REX.W 6B
		Insb_Yb_DX,											// 6C
		Insw_Yw_DX,											// o16 6D
		Insd_Yd_DX,											// o32 6D
		Outsb_DX_Xb,										// 6E
		Outsw_DX_Xw,										// o16 6F
		Outsd_DX_Xd,										// o32 6F

		Jo_Jb16,											// o16 70
		Jo_Jb32,											// o32 70
		Jo_Jb64,											// 70
		Jno_Jb16,											// o16 71
		Jno_Jb32,											// o32 71
		Jno_Jb64,											// 71
		Jb_Jb16,											// o16 72
		Jb_Jb32,											// o32 72
		Jb_Jb64,											// 72
		Jae_Jb16,											// o16 73
		Jae_Jb32,											// o32 73
		Jae_Jb64,											// 73
		Je_Jb16,											// o16 74
		Je_Jb32,											// o32 74
		Je_Jb64,											// 74
		Jne_Jb16,											// o16 75
		Jne_Jb32,											// o32 75
		Jne_Jb64,											// 75
		Jbe_Jb16,											// o16 76
		Jbe_Jb32,											// o32 76
		Jbe_Jb64,											// 76
		Ja_Jb16,											// o16 77
		Ja_Jb32,											// o32 77
		Ja_Jb64,											// 77

		Js_Jb16,											// o16 78
		Js_Jb32,											// o32 78
		Js_Jb64,											// 78
		Jns_Jb16,											// o16 79
		Jns_Jb32,											// o32 79
		Jns_Jb64,											// 79
		Jp_Jb16,											// o16 7A
		Jp_Jb32,											// o32 7A
		Jp_Jb64,											// 7A
		Jnp_Jb16,											// o16 7B
		Jnp_Jb32,											// o32 7B
		Jnp_Jb64,											// 7B
		Jl_Jb16,											// o16 7C
		Jl_Jb32,											// o32 7C
		Jl_Jb64,											// 7C
		Jge_Jb16,											// o16 7D
		Jge_Jb32,											// o32 7D
		Jge_Jb64,											// 7D
		Jle_Jb16,											// o16 7E
		Jle_Jb32,											// o32 7E
		Jle_Jb64,											// 7E
		Jg_Jb16,											// o16 7F
		Jg_Jb32,											// o32 7F
		Jg_Jb64,											// 7F

		Add_Eb_Ib,											// 80 /0
		Or_Eb_Ib,											// 80 /1
		Adc_Eb_Ib,											// 80 /2
		Sbb_Eb_Ib,											// 80 /3
		And_Eb_Ib,											// 80 /4
		Sub_Eb_Ib,											// 80 /5
		Xor_Eb_Ib,											// 80 /6
		Cmp_Eb_Ib,											// 80 /7
		Add_Ew_Iw,											// o16 81 /0
		Add_Ed_Id,											// o32 81 /0
		Add_Eq_Id64,										// REX.W 81 /0
		Or_Ew_Iw,											// o16 81 /1
		Or_Ed_Id,											// o32 81 /1
		Or_Eq_Id64,											// REX.W 81 /1
		Adc_Ew_Iw,											// o16 81 /2
		Adc_Ed_Id,											// o32 81 /2
		Adc_Eq_Id64,										// REX.W 81 /2
		Sbb_Ew_Iw,											// o16 81 /3
		Sbb_Ed_Id,											// o32 81 /3
		Sbb_Eq_Id64,										// REX.W 81 /3
		And_Ew_Iw,											// o16 81 /4
		And_Ed_Id,											// o32 81 /4
		And_Eq_Id64,										// REX.W 81 /4
		Sub_Ew_Iw,											// o16 81 /5
		Sub_Ed_Id,											// o32 81 /5
		Sub_Eq_Id64,										// REX.W 81 /5
		Xor_Ew_Iw,											// o16 81 /6
		Xor_Ed_Id,											// o32 81 /6
		Xor_Eq_Id64,										// REX.W 81 /6
		Cmp_Ew_Iw,											// o16 81 /7
		Cmp_Ed_Id,											// o32 81 /7
		Cmp_Eq_Id64,										// REX.W 81 /7
		// 82 is mapped to 80 (16/32-bit mode)
		Add_Ew_Ib16,										// o16 83 /0
		Add_Ed_Ib32,										// o32 83 /0
		Add_Eq_Ib64,										// REX.W 83 /0
		Or_Ew_Ib16,											// o16 83 /1
		Or_Ed_Ib32,											// o32 83 /1
		Or_Eq_Ib64,											// REX.W 83 /1
		Adc_Ew_Ib16,										// o16 83 /2
		Adc_Ed_Ib32,										// o32 83 /2
		Adc_Eq_Ib64,										// REX.W 83 /2
		Sbb_Ew_Ib16,										// o16 83 /3
		Sbb_Ed_Ib32,										// o32 83 /3
		Sbb_Eq_Ib64,										// REX.W 83 /3
		And_Ew_Ib16,										// o16 83 /4
		And_Ed_Ib32,										// o32 83 /4
		And_Eq_Ib64,										// REX.W 83 /4
		Sub_Ew_Ib16,										// o16 83 /5
		Sub_Ed_Ib32,										// o32 83 /5
		Sub_Eq_Ib64,										// REX.W 83 /5
		Xor_Ew_Ib16,										// o16 83 /6
		Xor_Ed_Ib32,										// o32 83 /6
		Xor_Eq_Ib64,										// REX.W 83 /6
		Cmp_Ew_Ib16,										// o16 83 /7
		Cmp_Ed_Ib32,										// o32 83 /7
		Cmp_Eq_Ib64,										// REX.W 83 /7
		Test_Eb_Gb,											// 84
		Test_Ew_Gw,											// o16 85
		Test_Ed_Gd,											// o32 85
		Test_Eq_Gq,											// REX.W 85
		Xchg_Eb_Gb,											// 86
		Xchg_Ew_Gw,											// o16 87
		Xchg_Ed_Gd,											// o32 87
		Xchg_Eq_Gq,											// REX.W 87

		Mov_Eb_Gb,											// 88
		Mov_Ew_Gw,											// o16 89
		Mov_Ed_Gd,											// o32 89
		Mov_Eq_Gq,											// REX.W 89
		Mov_Gb_Eb,											// 8A
		Mov_Gw_Ew,											// o16 8B
		Mov_Gd_Ed,											// o32 8B
		Mov_Gq_Eq,											// REX.W 8B
		Mov_Ew_Sw,											// o16 8C
		Mov_Ed_Sw,											// o32 8C
		Mov_Eq_Sw,											// REX.W 8C
		Lea_Gw_M,											// o16 8D
		Lea_Gd_M,											// o32 8D
		Lea_Gq_M,											// REX.W 8D
		Mov_Sw_Ew,											// o16 8E
		Mov_Sw_Ed,											// o32 8E
		Mov_Sw_Eq,											// REX.W 8E
		Pop_Ew,												// o16 8F /0
		Pop_Ed,												// o32 8F /0
		Pop_Eq,												// REX.W 8F /0

		Nopw,												// o16 90
		Xchg_R8W_AX,										// o16 REX.B 90
		Nopd,												// o32 90
		Xchg_R8D_EAX,										// o32 REX.B 90
		Nopq,												// REX.W 90
		Xchg_R8_RAX,										// REX.W REX.B 90
		Xchg_CX_AX,											// o16 91
		Xchg_R9W_AX,										// o16 REX.B 91
		Xchg_ECX_EAX,										// o32 91
		Xchg_R9D_EAX,										// o32 REX.B 91
		Xchg_RCX_RAX,										// REX.W 91
		Xchg_R9_RAX,										// REX.W REX.B 91
		Xchg_DX_AX,											// o16 92
		Xchg_R10W_AX,										// o16 REX.B 92
		Xchg_EDX_EAX,										// o32 92
		Xchg_R10D_EAX,										// o32 REX.B 92
		Xchg_RDX_RAX,										// REX.W 92
		Xchg_R10_RAX,										// REX.W REX.B 92
		Xchg_BX_AX,											// o16 93
		Xchg_R11W_AX,										// o16 REX.B 93
		Xchg_EBX_EAX,										// o32 93
		Xchg_R11D_EAX,										// o32 REX.B 93
		Xchg_RBX_RAX,										// REX.W 93
		Xchg_R11_RAX,										// REX.W REX.B 93
		Xchg_SP_AX,											// o16 94
		Xchg_R12W_AX,										// o16 REX.B 94
		Xchg_ESP_EAX,										// o32 94
		Xchg_R12D_EAX,										// o32 REX.B 94
		Xchg_RSP_RAX,										// REX.W 94
		Xchg_R12_RAX,										// REX.W REX.B 94
		Xchg_BP_AX,											// o16 95
		Xchg_R13W_AX,										// o16 REX.B 95
		Xchg_EBP_EAX,										// o32 95
		Xchg_R13D_EAX,										// o32 REX.B 95
		Xchg_RBP_RAX,										// REX.W 95
		Xchg_R13_RAX,										// REX.W REX.B 95
		Xchg_SI_AX,											// o16 96
		Xchg_R14W_AX,										// o16 REX.B 96
		Xchg_ESI_EAX,										// o32 96
		Xchg_R14D_EAX,										// o32 REX.B 96
		Xchg_RSI_RAX,										// REX.W 96
		Xchg_R14_RAX,										// REX.W REX.B 96
		Xchg_DI_AX,											// o16 97
		Xchg_R15W_AX,										// o16 REX.B 97
		Xchg_EDI_EAX,										// o32 97
		Xchg_R15D_EAX,										// o32 REX.B 97
		Xchg_RDI_RAX,										// REX.W 97
		Xchg_R15_RAX,										// REX.W REX.B 97

		Pause,												// F3 90

		Cbw,												// o16 98
		Cwde,												// o32 98
		Cdqe,												// REX.W 98
		Cwd,												// o16 99
		Cdq,												// o32 99
		Cqo,												// REX.W 99
		Call_Aww,											// o16 9A
		Call_Adw,											// o32 9A
		Wait,												// 9B
		Pushfw,												// o16 9C
		Pushfd,												// o32 9C
		Pushfq,												// 9C
		Popfw,												// o16 9D
		Popfd,												// o32 9D
		Popfq,												// 9D
		Sahf,												// 9E
		Lahf,												// 9F

		Mov_AL_Ob,											// A0
		Mov_AX_Ow,											// o16 A1
		Mov_EAX_Od,											// o32 A1
		Mov_RAX_Oq,											// REX.W A1
		Mov_Ob_AL,											// A2
		Mov_Ow_AX,											// o16 A3
		Mov_Od_EAX,											// o32 A3
		Mov_Oq_RAX,											// REX.W A3
		Movsb_Yb_Xb,										// A4
		Movsw_Yw_Xw,										// o16 A5
		Movsd_Yd_Xd,										// o32 A5
		Movsq_Yq_Xq,										// REX.W A5
		Cmpsb_Xb_Yb,										// A6
		Cmpsw_Xw_Yw,										// o16 A7
		Cmpsd_Xd_Yd,										// o32 A7
		Cmpsq_Xq_Yq,										// REX.W A7

		Test_AL_Ib,											// A8
		Test_AX_Iw,											// o16 A9
		Test_EAX_Id,										// o32 A9
		Test_RAX_Id64,										// REX.W A9
		Stosb_Yb_AL,										// AA
		Stosw_Yw_AX,										// o16 AB
		Stosd_Yd_EAX,										// o32 AB
		Stosq_Yq_RAX,										// REX.W AB
		Lodsb_AL_Xb,										// AC
		Lodsw_AX_Xw,										// o16 AD
		Lodsd_EAX_Xd,										// o32 AD
		Lodsq_RAX_Xq,										// REX.W AD
		Scasb_AL_Yb,										// AE
		Scasw_AX_Yw,										// o16 AF
		Scasd_EAX_Yd,										// o32 AF
		Scasq_RAX_Yq,										// REX.W AF

		Mov_AL_Ib,											// B0
		Mov_R8L_Ib,											// REX.B B0
		Mov_CL_Ib,											// B1
		Mov_R9L_Ib,											// REX.B B1
		Mov_DL_Ib,											// B2
		Mov_R10L_Ib,										// REX.B B2
		Mov_BL_Ib,											// B3
		Mov_R11L_Ib,										// REX.B B3
		Mov_AH_Ib,											// B4
		Mov_SPL_Ib,											// REX B4
		Mov_R12L_Ib,										// REX.B B4
		Mov_CH_Ib,											// B5
		Mov_BPL_Ib,											// REX B5
		Mov_R13L_Ib,										// REX.B B5
		Mov_DH_Ib,											// B6
		Mov_SIL_Ib,											// REX B6
		Mov_R14L_Ib,										// REX.B B6
		Mov_BH_Ib,											// B7
		Mov_DIL_Ib,											// REX B7
		Mov_R15L_Ib,										// REX.B B7

		Mov_AX_Iw,											// o16 B8
		Mov_R8W_Iw,											// o16 REX.B B8
		Mov_EAX_Id,											// o32 B8
		Mov_R8D_Id,											// o32 REX.B B8
		Mov_RAX_Iq,											// REX.W B8
		Mov_R8_Iq,											// REX.W REX.B B8
		Mov_CX_Iw,											// o16 B9
		Mov_R9W_Iw,											// o16 REX.B B9
		Mov_ECX_Id,											// o32 B9
		Mov_R9D_Id,											// o32 REX.B B9
		Mov_RCX_Iq,											// REX.W B9
		Mov_R9_Iq,											// REX.W REX.B B9
		Mov_DX_Iw,											// o16 BA
		Mov_R10W_Iw,										// o16 REX.B BA
		Mov_EDX_Id,											// o32 BA
		Mov_R10D_Id,										// o32 REX.B BA
		Mov_RDX_Iq,											// REX.W BA
		Mov_R10_Iq,											// REX.W REX.B BA
		Mov_BX_Iw,											// o16 BB
		Mov_R11W_Iw,										// o16 REX.B BB
		Mov_EBX_Id,											// o32 BB
		Mov_R11D_Id,										// o32 REX.B BB
		Mov_RBX_Iq,											// REX.W BB
		Mov_R11_Iq,											// REX.W REX.B BB
		Mov_SP_Iw,											// o16 BC
		Mov_R12W_Iw,										// o16 REX.B BC
		Mov_ESP_Id,											// o32 BC
		Mov_R12D_Id,										// o32 REX.B BC
		Mov_RSP_Iq,											// REX.W BC
		Mov_R12_Iq,											// REX.W REX.B BC
		Mov_BP_Iw,											// o16 BD
		Mov_R13W_Iw,										// o16 REX.B BD
		Mov_EBP_Id,											// o32 BD
		Mov_R13D_Id,										// o32 REX.B BD
		Mov_RBP_Iq,											// REX.W BD
		Mov_R13_Iq,											// REX.W REX.B BD
		Mov_SI_Iw,											// o16 BE
		Mov_R14W_Iw,										// o16 REX.B BE
		Mov_ESI_Id,											// o32 BE
		Mov_R14D_Id,										// o32 REX.B BE
		Mov_RSI_Iq,											// REX.W BE
		Mov_R14_Iq,											// REX.W REX.B BE
		Mov_DI_Iw,											// o16 BF
		Mov_R15W_Iw,										// o16 REX.B BF
		Mov_EDI_Id,											// o32 BF
		Mov_R15D_Id,										// o32 REX.B BF
		Mov_RDI_Iq,											// REX.W BF
		Mov_R15_Iq,											// REX.W REX.B BF

		Rol_Eb_Ib,											// C0 /0
		Ror_Eb_Ib,											// C0 /1
		Rcl_Eb_Ib,											// C0 /2
		Rcr_Eb_Ib,											// C0 /3
		Shl_Eb_Ib,											// C0 /4
		Shr_Eb_Ib,											// C0 /5
		// Shl_Eb_Ib										// C0 /6 => mapped to C0 /4
		Sar_Eb_Ib,											// C0 /7
		Rol_Ew_Ib,											// o16 C1 /0
		Rol_Ed_Ib,											// o32 C1 /0
		Rol_Eq_Ib,											// REX.W C1 /0
		Ror_Ew_Ib,											// o16 C1 /1
		Ror_Ed_Ib,											// o32 C1 /1
		Ror_Eq_Ib,											// REX.W C1 /1
		Rcl_Ew_Ib,											// o16 C1 /2
		Rcl_Ed_Ib,											// o32 C1 /2
		Rcl_Eq_Ib,											// REX.W C1 /2
		Rcr_Ew_Ib,											// o16 C1 /3
		Rcr_Ed_Ib,											// o32 C1 /3
		Rcr_Eq_Ib,											// REX.W C1 /3
		Shl_Ew_Ib,											// o16 C1 /4
		Shl_Ed_Ib,											// o32 C1 /4
		Shl_Eq_Ib,											// REX.W C1 /4
		Shr_Ew_Ib,											// o16 C1 /5
		Shr_Ed_Ib,											// o32 C1 /5
		Shr_Eq_Ib,											// REX.W C1 /5
		// Shl_Ew_Ib										// C1 /6 => mapped to C1 /4
		// Shl_Ed_Ib										// C1 /6 => mapped to C1 /4
		Sar_Ew_Ib,											// o16 C1 /7
		Sar_Ed_Ib,											// o32 C1 /7
		Sar_Eq_Ib,											// REX.W C1 /7
		Retnw_Iw,											// o16 C2
		Retnd_Iw,											// o32 C2
		Retnq_Iw,											// C2
		Retnw,												// o16 C3
		Retnd,												// o32 C3
		Retnq,												// C3
		Les_Gw_Mp,											// o16 C4
		Les_Gd_Mp,											// o32 C4
		Lds_Gw_Mp,											// o16 C5
		Lds_Gd_Mp,											// o32 C5
		Mov_Eb_Ib,											// C6 /0
		Xabort_Ib,											// C6 F8
		Mov_Ew_Iw,											// o16 C7 /0
		Mov_Ed_Id,											// o32 C7 /0
		Mov_Eq_Id64,										// REX.W C7 /0
		Xbegin_Jw16,										// o16 C7 F8
		Xbegin_Jd32,										// o32 C7 F8
		Xbegin_Jd64,										// REX.W C7 F8

		Enterw_Iw_Ib,										// o16 C8
		Enterd_Iw_Ib,										// o32 C8
		Enterq_Iw_Ib,										// REX.W C8
		Leavew,												// o16 C9
		Leaved,												// o32 C9
		Leaveq,												// REX.W C9
		Retfw_Iw,											// o16 CA
		Retfd_Iw,											// o32 CA
		Retfq_Iw,											// REX.W CA
		Retfw,												// o16 CB
		Retfd,												// o32 CB
		Retfq,												// REX.W CB
		Int3,												// CC
		Int_Ib,												// CD
		Into,												// CE
		Iretw,												// o16 CF
		Iretd,												// o32 CF
		Iretq,												// REX.W CF

		Rol_Eb_1,											// D0 /0
		Ror_Eb_1,											// D0 /1
		Rcl_Eb_1,											// D0 /2
		Rcr_Eb_1,											// D0 /3
		Shl_Eb_1,											// D0 /4
		Shr_Eb_1,											// D0 /5
		// Shl_Eb_1											// D0 /6 => mapped to D0 /4
		Sar_Eb_1,											// D0 /7
		Rol_Ew_1,											// o16 D1 /0
		Rol_Ed_1,											// o32 D1 /0
		Rol_Eq_1,											// REX.W D1 /0
		Ror_Ew_1,											// o16 D1 /1
		Ror_Ed_1,											// o32 D1 /1
		Ror_Eq_1,											// REX.W D1 /1
		Rcl_Ew_1,											// o16 D1 /2
		Rcl_Ed_1,											// o32 D1 /2
		Rcl_Eq_1,											// REX.W D1 /2
		Rcr_Ew_1,											// o16 D1 /3
		Rcr_Ed_1,											// o32 D1 /3
		Rcr_Eq_1,											// REX.W D1 /3
		Shl_Ew_1,											// o16 D1 /4
		Shl_Ed_1,											// o32 D1 /4
		Shl_Eq_1,											// REX.W D1 /4
		Shr_Ew_1,											// o16 D1 /5
		Shr_Ed_1,											// o32 D1 /5
		Shr_Eq_1,											// REX.W D1 /5
		// Shl_Ew_1											// D1 /6 => mapped to D1 /4
		// Shl_Ed_1											// D1 /6 => mapped to D1 /4
		Sar_Ew_1,											// o16 D1 /7
		Sar_Ed_1,											// o32 D1 /7
		Sar_Eq_1,											// REX.W D1 /7
		Rol_Eb_CL,											// D2 /0
		Ror_Eb_CL,											// D2 /1
		Rcl_Eb_CL,											// D2 /2
		Rcr_Eb_CL,											// D2 /3
		Shl_Eb_CL,											// D2 /4
		Shr_Eb_CL,											// D2 /5
		// Shl_Eb_CL										// D2 /6 => mapped to D2 /4
		Sar_Eb_CL,											// D2 /7
		Rol_Ew_CL,											// o16 D3 /0
		Rol_Ed_CL,											// o32 D3 /0
		Rol_Eq_CL,											// REX.W D3 /0
		Ror_Ew_CL,											// o16 D3 /1
		Ror_Ed_CL,											// o32 D3 /1
		Ror_Eq_CL,											// REX.W D3 /1
		Rcl_Ew_CL,											// o16 D3 /2
		Rcl_Ed_CL,											// o32 D3 /2
		Rcl_Eq_CL,											// REX.W D3 /2
		Rcr_Ew_CL,											// o16 D3 /3
		Rcr_Ed_CL,											// o32 D3 /3
		Rcr_Eq_CL,											// REX.W D3 /3
		Shl_Ew_CL,											// o16 D3 /4
		Shl_Ed_CL,											// o32 D3 /4
		Shl_Eq_CL,											// REX.W D3 /4
		Shr_Ew_CL,											// o16 D3 /5
		Shr_Ed_CL,											// o32 D3 /5
		Shr_Eq_CL,											// REX.W D3 /5
		// Shl_Ew_CL										// D3 /6 => mapped to D3 /4
		// Shl_Ed_CL										// D3 /6 => mapped to D3 /4
		Sar_Ew_CL,											// o16 D3 /7
		Sar_Ed_CL,											// o32 D3 /7
		Sar_Eq_CL,											// REX.W D3 /7
		Aam_Ib,												// D4
		Aad_Ib,												// D5
		Salc,												// D6
		Xlatb,												// D7

		Fadd_Mf32,											// D8 /0
		Fmul_Mf32,											// D8 /1
		Fcom_Mf32,											// D8 /2
		Fcomp_Mf32,											// D8 /3
		Fsub_Mf32,											// D8 /4
		Fsubr_Mf32,											// D8 /5
		Fdiv_Mf32,											// D8 /6
		Fdivr_Mf32,											// D8 /7
		Fadd_ST_STi,										// D8 C0+i
		Fmul_ST_STi,										// D8 C8+i
		Fcom_ST_STi,										// D8 D0+i
		Fcomp_ST_STi,										// D8 D8+i
		Fsub_ST_STi,										// D8 E0+i
		Fsubr_ST_STi,										// D8 E8+i
		Fdiv_ST_STi,										// D8 F0+i
		Fdivr_ST_STi,										// D8 F8+i

		Fld_Mf32,											// D9 /0
		// invalid											// D9 /1
		Fst_Mf32,											// D9 /2
		Fstp_Mf32,											// D9 /3
		Fldenv_M14,											// o16 D9 /4
		Fldenv_M28,											// o32 D9 /4
		Fldcw_Mw,											// D9 /5
		Fnstenv_M14,										// o16 D9 /6
		Fnstenv_M28,										// o32 D9 /6
		Fnstcw_Mw,											// D9 /7
		Fld_ST_STi,											// D9 C0+i
		Fxch_ST_STi,										// D9 C8+i
		Fnop,												// D9 D0
		Fchs,												// D9 E0
		Fabs,												// D9 E1
		Ftst,												// D9 E4
		Fxam,												// D9 E5
		Fld1,												// D9 E8
		Fldl2t,												// D9 E9
		Fldl2e,												// D9 EA
		Fldpi,												// D9 EB
		Fldlg2,												// D9 EC
		Fldln2,												// D9 ED
		Fldz,												// D9 EE
		F2xm1,												// D9 F0
		Fyl2x,												// D9 F1
		Fptan,												// D9 F2
		Fpatan,												// D9 F3
		Fxtract,											// D9 F4
		Fprem1,												// D9 F5
		Fdecstp,											// D9 F6
		Fincstp,											// D9 F7
		Fprem,												// D9 F8
		Fyl2xp1,											// D9 F9
		Fsqrt,												// D9 FA
		Fsincos,											// D9 FB
		Frndint,											// D9 FC
		Fscale,												// D9 FD
		Fsin,												// D9 FE
		Fcos,												// D9 FF

		Fiadd_Mfi32,										// DA /0
		Fimul_Mfi32,										// DA /1
		Ficom_Mfi32,										// DA /2
		Ficomp_Mfi32,										// DA /3
		Fisub_Mfi32,										// DA /4
		Fisubr_Mfi32,										// DA /5
		Fidiv_Mfi32,										// DA /6
		Fidivr_Mfi32,										// DA /7
		Fcmovb_ST_STi,										// DA C0+i
		Fcmove_ST_STi,										// DA C8+i
		Fcmovbe_ST_STi,										// DA D0+i
		Fcmovu_ST_STi,										// DA D8+i
		Fucompp,											// DA E9

		Fild_Mfi32,											// DB /0
		Fisttp_Mfi32,										// DB /1
		Fist_Mfi32,											// DB /2
		Fistp_Mfi32,										// DB /3
		Fld_Mf80,											// DB /5
		Fstp_Mf80,											// DB /7
		Fcmovnb_ST_STi,										// DB C0+i
		Fcmovne_ST_STi,										// DB C8+i
		Fcmovnbe_ST_STi,									// DB D0+i
		Fcmovnu_ST_STi,										// DB D8+i
		Fnclex,												// DB E2
		Fninit,												// DB E3
		Fucomi_ST_STi,										// DB E8+i
		Fcomi_ST_STi,										// DB F0+i

		Fadd_Mf64,											// DC /0
		Fmul_Mf64,											// DC /1
		Fcom_Mf64,											// DC /2
		Fcomp_Mf64,											// DC /3
		Fsub_Mf64,											// DC /4
		Fsubr_Mf64,											// DC /5
		Fdiv_Mf64,											// DC /6
		Fdivr_Mf64,											// DC /7
		Fadd_STi_ST,										// DC C0+i
		Fmul_STi_ST,										// DC C8+i
		Fsubr_STi_ST,										// DC E0+i
		Fsub_STi_ST,										// DC E8+i
		Fdivr_STi_ST,										// DC F0+i
		Fdiv_STi_ST,										// DC F8+i

		Fld_Mf64,											// DD /0
		Fisttp_Mf64,										// DD /1
		Fst_Mf64,											// DD /2
		Fstp_Mf64,											// DD /3
		Frstor_M98,											// DD /4
		Frstor_M108,										// DD /4
		Fnsave_M98,											// DD /6
		Fnsave_M108,										// DD /6
		Fnstsw_Mw,											// DD /7
		Ffree_STi,											// DD C0+i
		Fst_STi,											// DD D0+i
		Fstp_STi,											// DD D8+i
		Fucom_ST_STi,										// DD E0+i
		Fucomp_ST_STi,										// DD E8+i

		Fiadd_Mfi16,										// DE /0
		Fimul_Mfi16,										// DE /1
		Ficom_Mfi16,										// DE /2
		Ficomp_Mfi16,										// DE /3
		Fisub_Mfi16,										// DE /4
		Fisubr_Mfi16,										// DE /5
		Fidiv_Mfi16,										// DE /6
		Fidivr_Mfi16,										// DE /7
		Faddp_STi_ST,										// DE C0+i
		Fmulp_STi_ST,										// DE C8+i
		Fcompp,												// DE D9
		Fsubrp_STi_ST,										// DE E0+i
		Fsubp_STi_ST,										// DE E8+i
		Fdivrp_STi_ST,										// DE F0+i
		Fdivp_STi_ST,										// DE F8+i

		Fild_Mfi16,											// DF /0
		Fisttp_Mfi16,										// DF /1
		Fist_Mfi16,											// DF /2
		Fistp_Mfi16,										// DF /3
		Fbld_Mfbcd,											// DF /4
		Fild_Mfi64,											// DF /5
		Fbstp_Mfbcd,										// DF /6
		Fistp_Mfi64,										// DF /7
		Fnstsw_AX,											// DF E0
		Fucomip_ST_STi,										// DF E8+i
		Fcomip_ST_STi,										// DF F0+i

		Loopne_Jb16_CX,										// a16 o16 E0
		Loopne_Jb32_CX,										// a16 o32 E0
		Loopne_Jb16_ECX,									// a32 o16 E0
		Loopne_Jb32_ECX,									// a32 o32 E0
		Loopne_Jb64_ECX,									// a32 E0
		Loopne_Jb64_RCX,									// E0
		Loope_Jb16_CX,										// a16 o16 E1
		Loope_Jb32_CX,										// a16 o32 E1
		Loope_Jb16_ECX,										// a32 o16 E1
		Loope_Jb32_ECX,										// a32 o32 E1
		Loope_Jb64_ECX,										// a32 E1
		Loope_Jb64_RCX,										// E1
		Loop_Jb16_CX,										// a16 o16 E2
		Loop_Jb32_CX,										// a16 o32 E2
		Loop_Jb16_ECX,										// a32 o16 E2
		Loop_Jb32_ECX,										// a32 o32 E2
		Loop_Jb64_ECX,										// a32 E2
		Loop_Jb64_RCX,										// E2
		Jcxz_Jb16,											// a16 o16 E3
		Jcxz_Jb32,											// a16 o32 E3
		Jecxz_Jb16,											// a32 o16 E3
		Jecxz_Jb32,											// a32 o32 E3
		Jecxz_Jb64,											// a32 E3
		Jrcxz_Jb64,											// E3
		In_AL_Ib,											// E4
		In_AX_Ib,											// o16 E5
		In_EAX_Ib,											// o32 E5
		Out_Ib_AL,											// E6
		Out_Ib_AX,											// o16 E7
		Out_Ib_EAX,											// o32 E7

		Call_Jw16,											// o16 E8
		Call_Jd32,											// o32 E8
		Call_Jd64,											// E8
		Jmp_Jw16,											// o16 E9
		Jmp_Jd32,											// o32 E9
		Jmp_Jd64,											// E9
		Jmp_Aww,											// o16 EA
		Jmp_Adw,											// o32 EA
		Jmp_Jb16,											// o16 EB
		Jmp_Jb32,											// o32 EB
		Jmp_Jb64,											// EB
		In_AL_DX,											// EC
		In_AX_DX,											// o16 ED
		In_EAX_DX,											// o32 ED
		Out_DX_AL,											// EE
		Out_DX_AX,											// o16 EF
		Out_DX_EAX,											// o32 EF

		// lock												// F0
		Int1,												// F1
		// repne											// F2
		// rep												// F3
		Hlt,												// F4
		Cmc,												// F5
		Test_Eb_Ib,											// F6 /0
		// Test_Eb_Ib										// F6 /1 => mapped to F6 /0
		Not_Eb,												// F6 /2
		Neg_Eb,												// F6 /3
		Mul_Eb,												// F6 /4
		Imul_Eb,											// F6 /5
		Div_Eb,												// F6 /6
		Idiv_Eb,											// F6 /7
		Test_Ew_Iw,											// o16 F7 /0
		Test_Ed_Id,											// o32 F7 /0
		Test_Eq_Id64,										// REX.W F7 /0
		// Test_Ew_Iw,										// F7 /1 => mapped to F7 /0
		// Test_Ed_Id,										// F7 /1 => mapped to F7 /0
		// Test_Eq_Id64,									// F7 /1 => mapped to F7 /0
		Not_Ew,												// o16 F7 /2
		Not_Ed,												// o32 F7 /2
		Not_Eq,												// REX.W F7 /2
		Neg_Ew,												// o16 F7 /3
		Neg_Ed,												// o32 F7 /3
		Neg_Eq,												// REX.W F7 /3
		Mul_Ew,												// o16 F7 /4
		Mul_Ed,												// o32 F7 /4
		Mul_Eq,												// REX.W F7 /4
		Imul_Ew,											// o16 F7 /5
		Imul_Ed,											// o32 F7 /5
		Imul_Eq,											// REX.W F7 /5
		Div_Ew,												// o16 F7 /6
		Div_Ed,												// o32 F7 /6
		Div_Eq,												// REX.W F7 /6
		Idiv_Ew,											// o16 F7 /7
		Idiv_Ed,											// o32 F7 /7
		Idiv_Eq,											// REX.W F7 /7

		Clc,												// F8
		Stc,												// F9
		Cli,												// FA
		Sti,												// FB
		Cld,												// FC
		Std,												// FD
		Inc_Eb,												// FE /0
		Dec_Eb,												// FE /1
		Inc_Ew,												// o16 FF /0
		Inc_Ed,												// o32 FF /0
		Inc_Eq,												// REX.W FF /0
		Dec_Ew,												// o16 FF /1
		Dec_Ed,												// o32 FF /1
		Dec_Eq,												// REX.W FF /1
		Call_Ew,											// o16 FF /2
		Call_Ed,											// o32 FF /2
		Call_Eq,											// FF /2
		Call_Eww,											// o16 FF /3
		Call_Edw,											// o32 FF /3
		Call_Eqw,											// REX.W FF /3
		Jmp_Ew,												// o16 FF /4
		Jmp_Ed,												// o32 FF /4
		Jmp_Eq,												// FF /4
		Jmp_Eww,											// o16 FF /5
		Jmp_Edw,											// o32 FF /5
		Jmp_Eqw,											// REX.W FF /5
		Push_Ew,											// o16 FF /6
		Push_Ed,											// o32 FF /6
		Push_Eq,											// REX.W FF /6

		// 0Fxx opcodes

		Sldtw_Ew,											// o16 0F00 /0
		Sldtd_Ew,											// o32 0F00 /0
		Sldtq_Ew,											// REX.W 0F00 /0
		Strw_Ew,											// o16 0F00 /1
		Strd_Ew,											// o32 0F00 /1
		Strq_Ew,											// REX.W 0F00 /1
		Lldtw_Ew,											// o16 0F00 /2
		Lldtd_Ew,											// o32 0F00 /2
		Lldtq_Ew,											// REX.W 0F00 /2
		Ltrw_Ew,											// o16 0F00 /3
		Ltrd_Ew,											// o32 0F00 /3
		Ltrq_Ew,											// REX.W 0F00 /3
		Verrw_Ew,											// o16 0F00 /4
		Verrd_Ew,											// o32 0F00 /4
		Verrq_Ew,											// REX.W 0F00 /4
		Verww_Ew,											// o16 0F00 /5
		Verwd_Ew,											// o32 0F00 /5
		Verwq_Ew,											// REX.W 0F00 /5
		Sgdtw_Ms,											// o16 0F01 /0
		Sgdtd_Ms,											// o32 0F01 /0
		Sgdtq_Ms,											// 0F01 /0
		Sidtw_Ms,											// o16 0F01 /1
		Sidtd_Ms,											// o32 0F01 /1
		Sidtq_Ms,											// 0F01 /1
		Lgdtw_Ms,											// o16 0F01 /2
		Lgdtd_Ms,											// o32 0F01 /2
		Lgdtq_Ms,											// 0F01 /2
		Lidtw_Ms,											// o16 0F01 /3
		Lidtd_Ms,											// o32 0F01 /3
		Lidtq_Ms,											// 0F01 /3
		Smsww_Ew,											// o16 0F01 /4
		Smswd_Ew,											// o32 0F01 /4
		Smswq_Ew,											// REX.W 0F01 /4
		Lmsww_Ew,											// o16 0F01 /6
		Lmswd_Ew,											// o32 0F01 /6
		Lmswq_Ew,											// REX.W 0F01 /6
		Invlpg_M,											// 0F01 /7
		Enclv,												// 0F01 C0
		Vmcall,												// 0F01 C1
		Vmlaunch,											// 0F01 C2
		Vmresume,											// 0F01 C3
		Vmxoff,												// 0F01 C4
		Monitorw,											// a16 0F01 C8
		Monitord,											// a32 0F01 C8
		Monitorq,											// 0F01 C8
		Mwait,												// 0F01 C9
		Clac,												// 0F01 CA
		Stac,												// 0F01 CB
		Encls,												// 0F01 CF
		Xgetbv,												// 0F01 D0
		Xsetbv,												// 0F01 D1
		Vmfunc,												// 0F01 D4
		Xend,												// 0F01 D5
		Xtest,												// 0F01 D6
		Enclu,												// 0F01 D7
		Rdpkru,												// 0F01 EE
		Wrpkru,												// 0F01 EF
		Swapgs,												// 0F01 F8
		Rdtscp,												// 0F01 F9
		Lar_Gw_Ew,											// o16 0F02
		Lar_Gd_Ed,											// o32 0F02
		Lar_Gq_Eq,											// REX.W 0F02
		Lsl_Gw_Ew,											// o16 0F03
		Lsl_Gd_Ed,											// o32 0F03
		Lsl_Gq_Eq,											// REX.W 0F03
		Syscall,											// 0F05
		Clts,												// 0F06
		Sysretd,											// 0F07
		Sysretq,											// REX.W 0F07

		Invd,												// 0F08
		Wbinvd,												// 0F09
		Ud2,												// 0F0B
		Prefetchw_Mb,										// 0F0D /1
		Prefetchwt1_Mb,										// 0F0D /2

		Movups_VX_WX,										// 0F10
		VEX_Vmovups_VX_WX,									// VEX.128.0F.WIG 10
		VEX_Vmovups_VY_WY,									// VEX.256.0F.WIG 10
		EVEX_Vmovups_VX_k1z_WX,								// EVEX.128.0F.W0 10
		EVEX_Vmovups_VY_k1z_WY,								// EVEX.256.0F.W0 10
		EVEX_Vmovups_VZ_k1z_WZ,								// EVEX.512.0F.W0 10

		Movupd_VX_WX,										// 66 0F10
		VEX_Vmovupd_VX_WX,									// VEX.128.66.0F.WIG 10
		VEX_Vmovupd_VY_WY,									// VEX.256.66.0F.WIG 10
		EVEX_Vmovupd_VX_k1z_WX,								// EVEX.128.66.0F.W1 10
		EVEX_Vmovupd_VY_k1z_WY,								// EVEX.256.66.0F.W1 10
		EVEX_Vmovupd_VZ_k1z_WZ,								// EVEX.512.66.0F.W1 10

		Movss_VX_WX,										// F3 0F10
		VEX_Vmovss_VX_HX_RX,								// VEX.NDS.LIG.F3.0F.WIG 10
		VEX_Vmovss_VX_M,									// VEX.LIG.F3.0F.WIG 10
		EVEX_Vmovss_VX_k1z_HX_RX,							// EVEX.NDS.LIG.F3.0F.W0 10
		EVEX_Vmovss_VX_k1z_M,								// EVEX.LIG.F3.0F.W0 10

		Movsd_VX_WX,										// F2 0F10
		VEX_Vmovsd_VX_HX_RX,								// VEX.NDS.LIG.F2.0F.WIG 10
		VEX_Vmovsd_VX_M,									// VEX.LIG.F2.0F.WIG 10
		EVEX_Vmovsd_VX_k1z_HX_RX,							// EVEX.NDS.LIG.F2.0F.W1 10
		EVEX_Vmovsd_VX_k1z_M,								// EVEX.LIG.F2.0F.W1 10

		Movups_WX_VX,										// 0F11
		VEX_Vmovups_WX_VX,									// VEX.128.0F.WIG 11
		VEX_Vmovups_WY_VY,									// VEX.256.0F.WIG 11
		EVEX_Vmovups_WX_k1z_VX,								// EVEX.128.0F.W0 11
		EVEX_Vmovups_WY_k1z_VY,								// EVEX.256.0F.W0 11
		EVEX_Vmovups_WZ_k1z_VZ,								// EVEX.512.0F.W0 11

		Movupd_WX_VX,										// 66 0F11
		VEX_Vmovupd_WX_VX,									// VEX.128.66.0F.WIG 11
		VEX_Vmovupd_WY_VY,									// VEX.256.66.0F.WIG 11
		EVEX_Vmovupd_WX_k1z_VX,								// EVEX.128.66.0F.W1 11
		EVEX_Vmovupd_WY_k1z_VY,								// EVEX.256.66.0F.W1 11
		EVEX_Vmovupd_WZ_k1z_VZ,								// EVEX.512.66.0F.W1 11

		Movss_WX_VX,										// F3 0F11
		VEX_Vmovss_RX_HX_VX,								// VEX.NDS.LIG.F3.0F.WIG 11
		VEX_Vmovss_M_VX,									// VEX.LIG.F3.0F.WIG 11
		EVEX_Vmovss_RX_k1z_HX_VX,							// EVEX.NDS.LIG.F3.0F.W0 11
		EVEX_Vmovss_M_k1_VX,								// EVEX.LIG.F3.0F.W0 11

		Movsd_WX_VX,										// F2 0F11
		VEX_Vmovsd_RX_HX_VX,								// VEX.NDS.LIG.F2.0F.WIG 11
		VEX_Vmovsd_M_VX,									// VEX.LIG.F2.0F.WIG 11
		EVEX_Vmovsd_RX_k1z_HX_VX,							// EVEX.NDS.LIG.F2.0F.W1 11
		EVEX_Vmovsd_M_k1_VX,								// EVEX.LIG.F2.0F.W1 11

		Movhlps_VX_RX,										// 0F12
		Movlps_VX_M,										// 0F12
		VEX_Vmovhlps_VX_HX_RX,								// VEX.NDS.128.0F.WIG 12
		VEX_Vmovlps_VX_HX_M,								// VEX.NDS.128.0F.WIG 12
		EVEX_Vmovhlps_VX_HX_RX,								// EVEX.NDS.128.0F.W0 12
		EVEX_Vmovlps_VX_HX_M,								// EVEX.NDS.128.0F.W0 12

		Movlpd_VX_M,										// 66 0F12
		VEX_Vmovlpd_VX_HX_M,								// VEX.NDS.128.66.0F.WIG 12
		EVEX_Vmovlpd_VX_HX_M,								// EVEX.NDS.128.66.0F.W1 12

		Movsldup_VX_WX,										// F3 0F12
		VEX_Vmovsldup_VX_WX,								// VEX.128.F3.0F.WIG 12
		VEX_Vmovsldup_VY_WY,								// VEX.256.F3.0F.WIG 12
		EVEX_Vmovsldup_VX_k1z_WX,							// EVEX.128.F3.0F.W0 12
		EVEX_Vmovsldup_VY_k1z_WY,							// EVEX.256.F3.0F.W0 12
		EVEX_Vmovsldup_VZ_k1z_WZ,							// EVEX.512.F3.0F.W0 12

		Movddup_VX_WX,										// F2 0F12
		VEX_Vmovddup_VX_WX,									// VEX.128.F2.0F.WIG 12
		VEX_Vmovddup_VY_WY,									// VEX.256.F2.0F.WIG 12
		EVEX_Vmovddup_VX_k1z_WX,							// EVEX.128.F2.0F.W1 12
		EVEX_Vmovddup_VY_k1z_WY,							// EVEX.256.F2.0F.W1 12
		EVEX_Vmovddup_VZ_k1z_WZ,							// EVEX.512.F2.0F.W1 12

		Movlps_M_VX,										// 0F13
		VEX_Vmovlps_M_VX,									// VEX.128.0F.WIG 13
		EVEX_Vmovlps_M_VX,									// EVEX.128.0F.W0 13

		Movlpd_M_VX,										// 66 0F13
		VEX_Vmovlpd_M_VX,									// VEX.128.66.0F.WIG 13
		EVEX_Vmovlpd_M_VX,									// EVEX.128.66.0F.W1 13

		Unpcklps_VX_WX,										// 0F14
		VEX_Vunpcklps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 14
		VEX_Vunpcklps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 14
		EVEX_Vunpcklps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.0F.W0 14
		EVEX_Vunpcklps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.0F.W0 14
		EVEX_Vunpcklps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.0F.W0 14

		Unpcklpd_VX_WX,										// 66 0F14
		VEX_Vunpcklpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 14
		VEX_Vunpcklpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 14
		EVEX_Vunpcklpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W1 14
		EVEX_Vunpcklpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W1 14
		EVEX_Vunpcklpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W1 14

		Unpckhps_VX_WX,										// 0F15
		VEX_Vunpckhps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 15
		VEX_Vunpckhps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 15
		EVEX_Vunpckhps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.0F.W0 15
		EVEX_Vunpckhps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.0F.W0 15
		EVEX_Vunpckhps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.0F.W0 15

		Unpckhpd_VX_WX,										// 66 0F15
		VEX_Vunpckhpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 15
		VEX_Vunpckhpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 15
		EVEX_Vunpckhpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W1 15
		EVEX_Vunpckhpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W1 15
		EVEX_Vunpckhpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W1 15

		Movlhps_VX_RX,										// 0F16
		VEX_Vmovlhps_VX_HX_RX,								// VEX.NDS.128.0F.WIG 16
		EVEX_Vmovlhps_VX_HX_RX,								// EVEX.NDS.128.0F.W0 16

		Movhps_VX_M,										// 0F16
		VEX_Vmovhps_VX_HX_M,								// VEX.NDS.128.0F.WIG 16
		EVEX_Vmovhps_VX_HX_M,								// EVEX.NDS.128.0F.W0 16

		Movhpd_VX_M,										// 66 0F16
		VEX_Vmovhpd_VX_HX_M,								// VEX.NDS.128.66.0F.WIG 16
		EVEX_Vmovhpd_VX_HX_M,								// EVEX.NDS.128.66.0F.W1 16

		Movshdup_VX_WX,										// F3 0F16
		VEX_Vmovshdup_VX_WX,								// VEX.128.F3.0F.WIG 16
		VEX_Vmovshdup_VY_WY,								// VEX.256.F3.0F.WIG 16
		EVEX_Vmovshdup_VX_k1z_WX,							// EVEX.128.F3.0F.W0 16
		EVEX_Vmovshdup_VY_k1z_WY,							// EVEX.256.F3.0F.W0 16
		EVEX_Vmovshdup_VZ_k1z_WZ,							// EVEX.512.F3.0F.W0 16

		Movhps_M_VX,										// 0F17
		VEX_Vmovhps_M_VX,									// VEX.128.0F.WIG 17
		EVEX_Vmovhps_M_VX,									// EVEX.128.0F.W0 17

		Movhpd_M_VX,										// 66 0F17
		VEX_Vmovhpd_M_VX,									// VEX.128.66.0F.WIG 17
		EVEX_Vmovhpd_M_VX,									// EVEX.128.66.0F.W1 17

		Prefetchnta_Mb,										// 0F18 /0
		Prefetcht0_Mb,										// 0F18 /1
		Prefetcht1_Mb,										// 0F18 /2
		Prefetcht2_Mb,										// 0F18 /3

		Bndldx_B_MIB,										// 0F1A
		Bndmov_B_BMq,										// 66 0F1A
		Bndmov_B_BMo,										// 66 0F1A
		Bndcl_B_Ed,											// F3 0F1A
		Bndcl_B_Eq,											// F3 0F1A
		Bndcu_B_Ed,											// F2 0F1A
		Bndcu_B_Eq,											// F2 0F1A

		Bndstx_MIB_B,										// 0F1B
		Bndmov_BMq_B,										// 66 0F1B
		Bndmov_BMo_B,										// 66 0F1B
		Bndmk_B_Md,											// F3 0F1B
		Bndmk_B_Mq,											// F3 0F1B
		Bndcn_B_Ed,											// F2 0F1B
		Bndcn_B_Eq,											// F2 0F1B

		Nop_Ew,												// o16 0F1F /0
		Nop_Ed,												// o32 0F1F /0
		Nop_Eq,												// REX.W 0F1F /0

		Mov_Rd_Cd,											// 0F20
		Mov_Rq_Cq,											// 0F20
		Mov_Rd_Dd,											// 0F21
		Mov_Rq_Dq,											// 0F21
		Mov_Cd_Rd,											// 0F22
		Mov_Cq_Rq,											// 0F22
		Mov_Dd_Rd,											// 0F23
		Mov_Dq_Rq,											// 0F23

		Movaps_VX_WX,										// 0F28
		VEX_Vmovaps_VX_WX,									// VEX.128.0F.WIG 28
		VEX_Vmovaps_VY_WY,									// VEX.256.0F.WIG 28
		EVEX_Vmovaps_VX_k1z_WX,								// EVEX.128.0F.W0 28
		EVEX_Vmovaps_VY_k1z_WY,								// EVEX.256.0F.W0 28
		EVEX_Vmovaps_VZ_k1z_WZ,								// EVEX.512.0F.W0 28

		Movapd_VX_WX,										// 66 0F28
		VEX_Vmovapd_VX_WX,									// VEX.128.66.0F.WIG 28
		VEX_Vmovapd_VY_WY,									// VEX.256.66.0F.WIG 28
		EVEX_Vmovapd_VX_k1z_WX,								// EVEX.128.66.0F.W1 28
		EVEX_Vmovapd_VY_k1z_WY,								// EVEX.256.66.0F.W1 28
		EVEX_Vmovapd_VZ_k1z_WZ,								// EVEX.512.66.0F.W1 28

		Movaps_WX_VX,										// 0F29
		VEX_Vmovaps_WX_VX,									// VEX.128.0F.WIG 29
		VEX_Vmovaps_WY_VY,									// VEX.256.0F.WIG 29
		EVEX_Vmovaps_WX_k1z_VX,								// EVEX.128.0F.W0 29
		EVEX_Vmovaps_WY_k1z_VY,								// EVEX.256.0F.W0 29
		EVEX_Vmovaps_WZ_k1z_VZ,								// EVEX.512.0F.W0 29

		Movapd_WX_VX,										// 66 0F29
		VEX_Vmovapd_WX_VX,									// VEX.128.66.0F.WIG 29
		VEX_Vmovapd_WY_VY,									// VEX.256.66.0F.WIG 29
		EVEX_Vmovapd_WX_k1z_VX,								// EVEX.128.66.0F.W1 29
		EVEX_Vmovapd_WY_k1z_VY,								// EVEX.256.66.0F.W1 29
		EVEX_Vmovapd_WZ_k1z_VZ,								// EVEX.512.66.0F.W1 29

		Cvtpi2ps_VX_Q,										// 0F2A

		Cvtpi2pd_VX_Q,										// 66 0F2A

		Cvtsi2ss_VX_Ed,										// F3 0F2A
		Cvtsi2ss_VX_Eq,										// F3 REX.W 0F2A
		VEX_Vcvtsi2ss_VX_HX_Ed,								// VEX.NDS.LIG.F3.0F.W0 2A
		VEX_Vcvtsi2ss_VX_HX_Eq,								// VEX.NDS.LIG.F3.0F.W1 2A
		EVEX_Vcvtsi2ss_VX_HX_Ed_er,							// EVEX.NDS.LIG.F3.0F.W0 2A
		EVEX_Vcvtsi2ss_VX_HX_Eq_er,							// EVEX.NDS.LIG.F3.0F.W1 2A

		Cvtsi2sd_VX_Ed,										// F2 0F2A
		Cvtsi2sd_VX_Eq,										// F2 REX.W 0F2A
		VEX_Vcvtsi2sd_VX_HX_Ed,								// VEX.NDS.LIG.F2.0F.W0 2A
		VEX_Vcvtsi2sd_VX_HX_Eq,								// VEX.NDS.LIG.F2.0F.W1 2A
		EVEX_Vcvtsi2sd_VX_HX_Ed,							// EVEX.NDS.LIG.F2.0F.W0 2A
		EVEX_Vcvtsi2sd_VX_HX_Eq_er,							// EVEX.NDS.LIG.F2.0F.W1 2A

		Movntps_M_VX,										// 0F2B
		VEX_Vmovntps_M_VX,									// VEX.128.0F.WIG 2B
		VEX_Vmovntps_M_VY,									// VEX.256.0F.WIG 2B
		EVEX_Vmovntps_M_VX,									// EVEX.128.0F.W0 2B
		EVEX_Vmovntps_M_VY,									// EVEX.256.0F.W0 2B
		EVEX_Vmovntps_M_VZ,									// EVEX.512.0F.W0 2B

		Movntpd_M_VX,										// 66 0F2B
		VEX_Vmovntpd_M_VX,									// VEX.128.66.0F.WIG 2B
		VEX_Vmovntpd_M_VY,									// VEX.256.66.0F.WIG 2B
		EVEX_Vmovntpd_M_VX,									// EVEX.128.66.0F.W1 2B
		EVEX_Vmovntpd_M_VY,									// EVEX.256.66.0F.W1 2B
		EVEX_Vmovntpd_M_VZ,									// EVEX.512.66.0F.W1 2B

		Cvttps2pi_P_WX,										// 0F2C

		Cvttpd2pi_P_WX,										// 66 0F2C

		Cvttss2si_Gd_WX,									// F3 0F2C
		Cvttss2si_Gq_WX,									// F3 REX.W 0F2C
		VEX_Vcvttss2si_Gd_WX,								// VEX.LIG.F3.0F.W0 2C
		VEX_Vcvttss2si_Gq_WX,								// VEX.LIG.F3.0F.W1 2C
		EVEX_Vcvttss2si_Gd_WX_sae,							// EVEX.LIG.F3.0F.W0 2C
		EVEX_Vcvttss2si_Gq_WX_sae,							// EVEX.LIG.F3.0F.W1 2C

		Cvttsd2si_Gd_WX,									// F2 0F2C
		Cvttsd2si_Gq_WX,									// F2 REX.W 0F2C
		VEX_Vcvttsd2si_Gd_WX,								// VEX.LIG.F2.0F.W0 2C
		VEX_Vcvttsd2si_Gq_WX,								// VEX.LIG.F2.0F.W1 2C
		EVEX_Vcvttsd2si_Gd_WX_sae,							// EVEX.LIG.F2.0F.W0 2C
		EVEX_Vcvttsd2si_Gq_WX_sae,							// EVEX.LIG.F2.0F.W1 2C

		Cvtps2pi_P_WX,										// 0F2D

		Cvtpd2pi_P_WX,										// 66 0F2D

		Cvtss2si_Gd_WX,										// F3 0F2D
		Cvtss2si_Gq_WX,										// F3 REX.W 0F2D
		VEX_Vcvtss2si_Gd_WX,								// VEX.LIG.F3.0F.W0 2D
		VEX_Vcvtss2si_Gq_WX,								// VEX.LIG.F3.0F.W1 2D
		EVEX_Vcvtss2si_Gd_WX_er,							// EVEX.LIG.F3.0F.W0 2D
		EVEX_Vcvtss2si_Gq_WX_er,							// EVEX.LIG.F3.0F.W1 2D

		Cvtsd2si_Gd_WX,										// F2 0F2D
		Cvtsd2si_Gq_WX,										// F2 REX.W 0F2D
		VEX_Vcvtsd2si_Gd_WX,								// VEX.LIG.F2.0F.W0 2D
		VEX_Vcvtsd2si_Gq_WX,								// VEX.LIG.F2.0F.W1 2D
		EVEX_Vcvtsd2si_Gd_WX_er,							// EVEX.LIG.F2.0F.W0 2D
		EVEX_Vcvtsd2si_Gq_WX_er,							// EVEX.LIG.F2.0F.W1 2D

		Ucomiss_VX_WX,										// 0F2E
		VEX_Vucomiss_VX_WX,									// VEX.LIG.0F.WIG 2E
		EVEX_Vucomiss_VX_WX_sae,							// EVEX.LIG.0F.W0 2E

		Ucomisd_VX_WX,										// 66 0F2E
		VEX_Vucomisd_VX_WX,									// VEX.LIG.66.0F.WIG 2E
		EVEX_Vucomisd_VX_WX_sae,							// EVEX.LIG.66.0F.W1 2E

		Comiss_VX_WX,										// 0F2F

		Comisd_VX_WX,										// 66 0F2F
		VEX_Vcomiss_VX_WX,									// VEX.LIG.0F.WIG 2F
		VEX_Vcomisd_VX_WX,									// VEX.LIG.66.0F.WIG 2F
		EVEX_Vcomiss_VX_WX_sae,								// EVEX.LIG.0F.W0 2F
		EVEX_Vcomisd_VX_WX_sae,								// EVEX.LIG.66.0F.W1 2F

		Wrmsr,												// 0F30
		Rdtsc,												// 0F31
		Rdmsr,												// 0F32
		Rdpmc,												// 0F33
		Sysenter,											// 0F34
		Sysexitd,											// 0F35
		Sysexitq,											// REX.W 0F35
		Getsec,												// 0F37

		Cmovo_Gw_Ew,										// o16 0F40
		Cmovo_Gd_Ed,										// o32 0F40
		Cmovo_Gq_Eq,										// REX.W 0F40
		Cmovno_Gw_Ew,										// o16 0F41
		Cmovno_Gd_Ed,										// o32 0F41
		Cmovno_Gq_Eq,										// REX.W 0F41
		Cmovb_Gw_Ew,										// o16 0F42
		Cmovb_Gd_Ed,										// o32 0F42
		Cmovb_Gq_Eq,										// REX.W 0F42
		Cmovae_Gw_Ew,										// o16 0F43
		Cmovae_Gd_Ed,										// o32 0F43
		Cmovae_Gq_Eq,										// REX.W 0F43
		Cmove_Gw_Ew,										// o16 0F44
		Cmove_Gd_Ed,										// o32 0F44
		Cmove_Gq_Eq,										// REX.W 0F44
		Cmovne_Gw_Ew,										// o16 0F45
		Cmovne_Gd_Ed,										// o32 0F45
		Cmovne_Gq_Eq,										// REX.W 0F45
		Cmovbe_Gw_Ew,										// o16 0F46
		Cmovbe_Gd_Ed,										// o32 0F46
		Cmovbe_Gq_Eq,										// REX.W 0F46
		Cmova_Gw_Ew,										// o16 0F47
		Cmova_Gd_Ed,										// o32 0F47
		Cmova_Gq_Eq,										// REX.W 0F47

		Cmovs_Gw_Ew,										// o16 0F48
		Cmovs_Gd_Ed,										// o32 0F48
		Cmovs_Gq_Eq,										// REX.W 0F48
		Cmovns_Gw_Ew,										// o16 0F49
		Cmovns_Gd_Ed,										// o32 0F49
		Cmovns_Gq_Eq,										// REX.W 0F49
		Cmovp_Gw_Ew,										// o16 0F4A
		Cmovp_Gd_Ed,										// o32 0F4A
		Cmovp_Gq_Eq,										// REX.W 0F4A
		Cmovnp_Gw_Ew,										// o16 0F4B
		Cmovnp_Gd_Ed,										// o32 0F4B
		Cmovnp_Gq_Eq,										// REX.W 0F4B
		Cmovl_Gw_Ew,										// o16 0F4C
		Cmovl_Gd_Ed,										// o32 0F4C
		Cmovl_Gq_Eq,										// REX.W 0F4C
		Cmovge_Gw_Ew,										// o16 0F4D
		Cmovge_Gd_Ed,										// o32 0F4D
		Cmovge_Gq_Eq,										// REX.W 0F4D
		Cmovle_Gw_Ew,										// o16 0F4E
		Cmovle_Gd_Ed,										// o32 0F4E
		Cmovle_Gq_Eq,										// REX.W 0F4E
		Cmovg_Gw_Ew,										// o16 0F4F
		Cmovg_Gd_Ed,										// o32 0F4F
		Cmovg_Gq_Eq,										// REX.W 0F4F

		VEX_Kandw_VK_HK_RK,									// VEX.NDS.L1.0F.W0 41
		VEX_Kandq_VK_HK_RK,									// VEX.NDS.L1.0F.W1 41

		VEX_Kandb_VK_HK_RK,									// VEX.NDS.L1.66.0F.W0 41
		VEX_Kandd_VK_HK_RK,									// VEX.NDS.L1.66.0F.W1 41

		VEX_Kandnw_VK_HK_RK,								// VEX.NDS.L1.0F.W0 42
		VEX_Kandnq_VK_HK_RK,								// VEX.NDS.L1.0F.W1 42

		VEX_Kandnb_VK_HK_RK,								// VEX.NDS.L1.66.0F.W0 42
		VEX_Kandnd_VK_HK_RK,								// VEX.NDS.L1.66.0F.W1 42

		VEX_Knotw_VK_RK,									// VEX.L0.0F.W0 44
		VEX_Knotq_VK_RK,									// VEX.L0.0F.W1 44

		VEX_Knotb_VK_RK,									// VEX.L0.66.0F.W0 44
		VEX_Knotd_VK_RK,									// VEX.L0.66.0F.W1 44

		VEX_Korw_VK_HK_RK,									// VEX.NDS.L1.0F.W0 45
		VEX_Korq_VK_HK_RK,									// VEX.NDS.L1.0F.W1 45

		VEX_Korb_VK_HK_RK,									// VEX.NDS.L1.66.0F.W0 45
		VEX_Kord_VK_HK_RK,									// VEX.NDS.L1.66.0F.W1 45

		VEX_Kxnorw_VK_HK_RK,								// VEX.NDS.L1.0F.W0 46
		VEX_Kxnorq_VK_HK_RK,								// VEX.NDS.L1.0F.W1 46

		VEX_Kxnorb_VK_HK_RK,								// VEX.NDS.L1.66.0F.W0 46
		VEX_Kxnord_VK_HK_RK,								// VEX.NDS.L1.66.0F.W1 46

		VEX_Kxorw_VK_HK_RK,									// VEX.NDS.L1.0F.W0 47
		VEX_Kxorq_VK_HK_RK,									// VEX.NDS.L1.0F.W1 47

		VEX_Kxorb_VK_HK_RK,									// VEX.NDS.L1.66.0F.W0 47
		VEX_Kxord_VK_HK_RK,									// VEX.NDS.L1.66.0F.W1 47

		VEX_Kaddw_VK_HK_RK,									// VEX.NDS.L1.0F.W0 4A
		VEX_Kaddq_VK_HK_RK,									// VEX.NDS.L1.0F.W1 4A

		VEX_Kaddb_VK_HK_RK,									// VEX.NDS.L1.66.0F.W0 4A
		VEX_Kaddd_VK_HK_RK,									// VEX.NDS.L1.66.0F.W1 4A

		VEX_Kunpckwd_VK_HK_RK,								// VEX.NDS.L1.0F.W0 4B
		VEX_Kunpckdq_VK_HK_RK,								// VEX.NDS.L1.0F.W1 4B

		VEX_Kunpckbw_VK_HK_RK,								// VEX.NDS.L1.66.0F.W0 4B

		Movmskps_Gd_RX,										// 0F50
		Movmskps_Gq_RX,										// REX.W 0F50
		VEX_Vmovmskps_Gd_RX,								// VEX.128.0F.W0 50
		VEX_Vmovmskps_Gq_RX,								// VEX.128.0F.W1 50
		VEX_Vmovmskps_Gd_RY,								// VEX.256.0F.W0 50
		VEX_Vmovmskps_Gq_RY,								// VEX.256.0F.W1 50

		Movmskpd_Gd_RX,										// 66 0F50
		Movmskpd_Gq_RX,										// 66 REX.W 0F50
		VEX_Vmovmskpd_Gd_RX,								// VEX.128.66.0F.W0 50
		VEX_Vmovmskpd_Gq_RX,								// VEX.128.66.0F.W1 50
		VEX_Vmovmskpd_Gd_RY,								// VEX.256.66.0F.W0 50
		VEX_Vmovmskpd_Gq_RY,								// VEX.256.66.0F.W1 50

		Sqrtps_VX_WX,										// 0F51
		VEX_Vsqrtps_VX_WX,									// VEX.128.0F.WIG 51
		VEX_Vsqrtps_VY_WY,									// VEX.256.0F.WIG 51
		EVEX_Vsqrtps_VX_k1z_WX_b,							// EVEX.NDS.128.0F.W0 51
		EVEX_Vsqrtps_VY_k1z_WY_b,							// EVEX.NDS.256.0F.W0 51
		EVEX_Vsqrtps_VZ_k1z_WZ_er_b,						// EVEX.NDS.512.0F.W0 51

		Sqrtpd_VX_WX,										// 66 0F51
		VEX_Vsqrtpd_VX_WX,									// VEX.128.66.0F.WIG 51
		VEX_Vsqrtpd_VY_WY,									// VEX.256.66.0F.WIG 51
		EVEX_Vsqrtpd_VX_k1z_WX_b,							// EVEX.128.66.0F.W1 51
		EVEX_Vsqrtpd_VY_k1z_WY_b,							// EVEX.256.66.0F.W1 51
		EVEX_Vsqrtpd_VZ_k1z_WZ_er_b,						// EVEX.512.66.0F.W1 51

		Sqrtss_VX_WX,										// F3 0F51
		VEX_Vsqrtss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 51
		EVEX_Vsqrtss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F3.0F.W0 51

		Sqrtsd_VX_WX,										// F2 0F51
		VEX_Vsqrtsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 51
		EVEX_Vsqrtsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 51

		Rsqrtps_VX_WX,										// 0F52
		VEX_Vrsqrtps_VX_WX,									// VEX.128.0F.WIG 52
		VEX_Vrsqrtps_VY_WY,									// VEX.256.0F.WIG 52

		Rsqrtss_VX_WX,										// F3 0F52
		VEX_Vrsqrtss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 52

		Rcpps_VX_WX,										// 0F53
		VEX_Vrcpps_VX_WX,									// VEX.128.0F.WIG 53
		VEX_Vrcpps_VY_WY,									// VEX.256.0F.WIG 53

		Rcpss_VX_WX,										// F3 0F53
		VEX_Vrcpss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 53

		Andps_VX_WX,										// 0F54
		VEX_Vandps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 54
		VEX_Vandps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 54
		EVEX_Vandps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 54
		EVEX_Vandps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 54
		EVEX_Vandps_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.0F.W0 54

		Andpd_VX_WX,										// 66 0F54
		VEX_Vandpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 54
		VEX_Vandpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 54
		EVEX_Vandpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 54
		EVEX_Vandpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 54
		EVEX_Vandpd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 54

		Andnps_VX_WX,										// 0F55
		VEX_Vandnps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 55
		VEX_Vandnps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 55
		EVEX_Vandnps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.0F.W0 55
		EVEX_Vandnps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.0F.W0 55
		EVEX_Vandnps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.0F.W0 55

		Andnpd_VX_WX,										// 66 0F55
		VEX_Vandnpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 55
		VEX_Vandnpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 55
		EVEX_Vandnpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W1 55
		EVEX_Vandnpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W1 55
		EVEX_Vandnpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W1 55

		Orps_VX_WX,											// 0F56
		VEX_Vorps_VX_HX_WX,									// VEX.NDS.128.0F.WIG 56
		VEX_Vorps_VY_HY_WY,									// VEX.NDS.256.0F.WIG 56
		EVEX_Vorps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 56
		EVEX_Vorps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 56
		EVEX_Vorps_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.0F.W0 56

		Orpd_VX_WX,											// 66 0F56
		VEX_Vorpd_VX_HX_WX,									// VEX.NDS.128.66.0F.WIG 56
		VEX_Vorpd_VY_HY_WY,									// VEX.NDS.256.66.0F.WIG 56
		EVEX_Vorpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 56
		EVEX_Vorpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 56
		EVEX_Vorpd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 56

		Xorps_VX_WX,										// 0F57
		VEX_Vxorps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 57
		VEX_Vxorps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 57
		EVEX_Vxorps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 57
		EVEX_Vxorps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 57
		EVEX_Vxorps_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.0F.W0 57

		Xorpd_VX_WX,										// 66 0F57
		VEX_Vxorpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 57
		VEX_Vxorpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 57
		EVEX_Vxorpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 57
		EVEX_Vxorpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 57
		EVEX_Vxorpd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 57

		Addps_VX_WX,										// 0F58
		VEX_Vaddps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 58
		VEX_Vaddps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 58
		EVEX_Vaddps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 58
		EVEX_Vaddps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 58
		EVEX_Vaddps_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.0F.W0 58

		Addpd_VX_WX,										// 66 0F58
		VEX_Vaddpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 58
		VEX_Vaddpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 58
		EVEX_Vaddpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 58
		EVEX_Vaddpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 58
		EVEX_Vaddpd_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.66.0F.W1 58

		Addss_VX_WX,										// F3 0F58
		VEX_Vaddss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 58
		EVEX_Vaddss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F3.0F.W0 58

		Addsd_VX_WX,										// F2 0F58
		VEX_Vaddsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 58
		EVEX_Vaddsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 58

		Mulps_VX_WX,										// 0F59
		VEX_Vmulps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 59
		VEX_Vmulps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 59
		EVEX_Vmulps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 59
		EVEX_Vmulps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 59
		EVEX_Vmulps_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.0F.W0 59

		Mulpd_VX_WX,										// 66 0F59
		VEX_Vmulpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 59
		VEX_Vmulpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 59
		EVEX_Vmulpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 59
		EVEX_Vmulpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 59
		EVEX_Vmulpd_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.66.0F.W1 59

		Mulss_VX_WX,										// F3 0F59
		VEX_Vmulss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 59
		EVEX_Vmulss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F3.0F.W0 59

		Mulsd_VX_WX,										// F2 0F59
		VEX_Vmulsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 59
		EVEX_Vmulsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 59

		Cvtps2pd_VX_WX,										// 0F5A
		VEX_Vcvtps2pd_VX_WX,								// VEX.128.0F.WIG 5A
		VEX_Vcvtps2pd_VY_WX,								// VEX.256.0F.WIG 5A
		EVEX_Vcvtps2pd_VX_k1z_WX_b,							// EVEX.128.0F.W0 5A
		EVEX_Vcvtps2pd_VY_k1z_WX_b,							// EVEX.256.0F.W0 5A
		EVEX_Vcvtps2pd_VZ_k1z_WY_sae_b,						// EVEX.512.0F.W0 5A

		Cvtpd2ps_VX_WX,										// 66 0F5A
		VEX_Vcvtpd2ps_VX_WX,								// VEX.128.66.0F.WIG 5A
		VEX_Vcvtpd2ps_VX_WY,								// VEX.256.66.0F.WIG 5A
		EVEX_Vcvtpd2ps_VX_k1z_WX_b,							// EVEX.128.66.0F.W1 5A
		EVEX_Vcvtpd2ps_VX_k1z_WY_b,							// EVEX.256.66.0F.W1 5A
		EVEX_Vcvtpd2ps_VY_k1z_WZ_er_b,						// EVEX.512.66.0F.W1 5A

		Cvtss2sd_VX_WX,										// F3 0F5A
		VEX_Vcvtss2sd_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 5A
		EVEX_Vcvtss2sd_VX_k1z_HX_WX_sae,					// EVEX.NDS.LIG.F3.0F.W0 5A

		Cvtsd2ss_VX_WX,										// F2 0F5A
		VEX_Vcvtsd2ss_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 5A
		EVEX_Vcvtsd2ss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 5A

		Cvtdq2ps_VX_WX,										// 0F5B
		VEX_Vcvtdq2ps_VX_WX,								// VEX.128.0F.WIG 5B
		VEX_Vcvtdq2ps_VY_WY,								// VEX.256.0F.WIG 5B
		EVEX_Vcvtdq2ps_VX_k1z_WX_b,							// EVEX.128.0F.W0 5B
		EVEX_Vcvtdq2ps_VY_k1z_WY_b,							// EVEX.256.0F.W0 5B
		EVEX_Vcvtdq2ps_VZ_k1z_WZ_er_b,						// EVEX.512.0F.W0 5B
		EVEX_Vcvtqq2ps_VX_k1z_WX_b,							// EVEX.128.0F.W1 5B
		EVEX_Vcvtqq2ps_VX_k1z_WY_b,							// EVEX.256.0F.W1 5B
		EVEX_Vcvtqq2ps_VY_k1z_WZ_er_b,						// EVEX.512.0F.W1 5B

		Cvtps2dq_VX_WX,										// 66 0F5B
		VEX_Vcvtps2dq_VX_WX,								// VEX.128.66.0F.WIG 5B
		VEX_Vcvtps2dq_VY_WY,								// VEX.256.66.0F.WIG 5B
		EVEX_Vcvtps2dq_VX_k1z_WX_b,							// EVEX.128.66.0F.W0 5B
		EVEX_Vcvtps2dq_VY_k1z_WY_b,							// EVEX.256.66.0F.W0 5B
		EVEX_Vcvtps2dq_VZ_k1z_WZ_er_b,						// EVEX.512.66.0F.W0 5B

		Cvttps2dq_VX_WX,									// F3 0F5B
		VEX_Vcvttps2dq_VX_WX,								// VEX.128.F3.0F.WIG 5B
		VEX_Vcvttps2dq_VY_WY,								// VEX.256.F3.0F.WIG 5B
		EVEX_Vcvttps2dq_VX_k1z_WX_b,						// EVEX.128.F3.0F.W0 5B
		EVEX_Vcvttps2dq_VY_k1z_WY_b,						// EVEX.256.F3.0F.W0 5B
		EVEX_Vcvttps2dq_VZ_k1z_WZ_sae_b,					// EVEX.512.F3.0F.W0 5B

		Subps_VX_WX,										// 0F5C
		VEX_Vsubps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 5C
		VEX_Vsubps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 5C
		EVEX_Vsubps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 5C
		EVEX_Vsubps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 5C
		EVEX_Vsubps_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.0F.W0 5C

		Subpd_VX_WX,										// 66 0F5C
		VEX_Vsubpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 5C
		VEX_Vsubpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 5C
		EVEX_Vsubpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 5C
		EVEX_Vsubpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 5C
		EVEX_Vsubpd_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.66.0F.W1 5C

		Subss_VX_WX,										// F3 0F5C
		VEX_Vsubss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 5C
		EVEX_Vsubss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F3.0F.W0 5C

		Subsd_VX_WX,										// F2 0F5C
		VEX_Vsubsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 5C
		EVEX_Vsubsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 5C

		Minps_VX_WX,										// 0F5D
		VEX_Vminps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 5D
		VEX_Vminps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 5D
		EVEX_Vminps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 5D
		EVEX_Vminps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 5D
		EVEX_Vminps_VZ_k1z_HZ_WZ_sae_b,						// EVEX.NDS.512.0F.W0 5D

		Minpd_VX_WX,										// 66 0F5D
		VEX_Vminpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 5D
		VEX_Vminpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 5D
		EVEX_Vminpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 5D
		EVEX_Vminpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 5D
		EVEX_Vminpd_VZ_k1z_HZ_WZ_sae_b,						// EVEX.NDS.512.66.0F.W1 5D

		Minss_VX_WX,										// F3 0F5D
		VEX_Vminss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 5D
		EVEX_Vminss_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.F3.0F.W0 5D

		Minsd_VX_WX,										// F2 0F5D
		VEX_Vminsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 5D
		EVEX_Vminsd_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.F2.0F.W1 5D

		Divps_VX_WX,										// 0F5E
		VEX_Vdivps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 5E
		VEX_Vdivps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 5E
		EVEX_Vdivps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 5E
		EVEX_Vdivps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 5E
		EVEX_Vdivps_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.0F.W0 5E

		Divpd_VX_WX,										// 66 0F5E
		VEX_Vdivpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 5E
		VEX_Vdivpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 5E
		EVEX_Vdivpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 5E
		EVEX_Vdivpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 5E
		EVEX_Vdivpd_VZ_k1z_HZ_WZ_er_b,						// EVEX.NDS.512.66.0F.W1 5E

		Divss_VX_WX,										// F3 0F5E
		VEX_Vdivss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 5E
		EVEX_Vdivss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F3.0F.W0 5E

		Divsd_VX_WX,										// F2 0F5E
		VEX_Vdivsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 5E
		EVEX_Vdivsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.F2.0F.W1 5E

		Maxps_VX_WX,										// 0F5F
		VEX_Vmaxps_VX_HX_WX,								// VEX.NDS.128.0F.WIG 5F
		VEX_Vmaxps_VY_HY_WY,								// VEX.NDS.256.0F.WIG 5F
		EVEX_Vmaxps_VX_k1z_HX_WX_b,							// EVEX.NDS.128.0F.W0 5F
		EVEX_Vmaxps_VY_k1z_HY_WY_b,							// EVEX.NDS.256.0F.W0 5F
		EVEX_Vmaxps_VZ_k1z_HZ_WZ_sae_b,						// EVEX.NDS.512.0F.W0 5F

		Maxpd_VX_WX,										// 66 0F5F
		VEX_Vmaxpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 5F
		VEX_Vmaxpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 5F
		EVEX_Vmaxpd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 5F
		EVEX_Vmaxpd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 5F
		EVEX_Vmaxpd_VZ_k1z_HZ_WZ_sae_b,						// EVEX.NDS.512.66.0F.W1 5F

		Maxss_VX_WX,										// F3 0F5F
		VEX_Vmaxss_VX_HX_WX,								// VEX.NDS.LIG.F3.0F.WIG 5F
		EVEX_Vmaxss_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.F3.0F.W0 5F

		Maxsd_VX_WX,										// F2 0F5F
		VEX_Vmaxsd_VX_HX_WX,								// VEX.NDS.LIG.F2.0F.WIG 5F
		EVEX_Vmaxsd_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.F2.0F.W1 5F

		Punpcklbw_P_Q,										// 0F60

		Punpcklbw_VX_WX,									// 66 0F60
		VEX_Vpunpcklbw_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 60
		VEX_Vpunpcklbw_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 60
		EVEX_Vpunpcklbw_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 60
		EVEX_Vpunpcklbw_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 60
		EVEX_Vpunpcklbw_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 60

		Punpcklwd_P_Q,										// 0F61

		Punpcklwd_VX_WX,									// 66 0F61
		VEX_Vpunpcklwd_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 61
		VEX_Vpunpcklwd_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 61
		EVEX_Vpunpcklwd_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 61
		EVEX_Vpunpcklwd_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 61
		EVEX_Vpunpcklwd_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 61

		Punpckldq_P_Q,										// 0F62

		Punpckldq_VX_WX,									// 66 0F62
		VEX_Vpunpckldq_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 62
		VEX_Vpunpckldq_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 62
		EVEX_Vpunpckldq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 62
		EVEX_Vpunpckldq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 62
		EVEX_Vpunpckldq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 62

		Packsswb_P_Q,										// 0F63

		Packsswb_VX_WX,										// 66 0F63
		VEX_Vpacksswb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 63
		VEX_Vpacksswb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 63
		EVEX_Vpacksswb_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 63
		EVEX_Vpacksswb_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 63
		EVEX_Vpacksswb_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 63

		Pcmpgtb_P_Q,										// 0F64

		Pcmpgtb_VX_WX,										// 66 0F64
		VEX_Vpcmpgtb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 64
		VEX_Vpcmpgtb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 64
		EVEX_Vpcmpgtb_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F.WIG 64
		EVEX_Vpcmpgtb_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F.WIG 64
		EVEX_Vpcmpgtb_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG 64

		Pcmpgtw_P_Q,										// 0F65

		Pcmpgtw_VX_WX,										// 66 0F65
		VEX_Vpcmpgtw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 65
		VEX_Vpcmpgtw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 65
		EVEX_Vpcmpgtw_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F.WIG 65
		EVEX_Vpcmpgtw_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F.WIG 65
		EVEX_Vpcmpgtw_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG 65

		Pcmpgtd_P_Q,										// 0F66

		Pcmpgtd_VX_WX,										// 66 0F66
		VEX_Vpcmpgtd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 66
		VEX_Vpcmpgtd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 66
		EVEX_Vpcmpgtd_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 66
		EVEX_Vpcmpgtd_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 66
		EVEX_Vpcmpgtd_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 66

		Packuswb_P_Q,										// 0F67

		Packuswb_VX_WX,										// 66 0F67
		VEX_Vpackuswb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 67
		VEX_Vpackuswb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 67
		EVEX_Vpackuswb_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 67
		EVEX_Vpackuswb_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 67
		EVEX_Vpackuswb_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 67

		Punpckhbw_P_Q,										// 0F68

		Punpckhbw_VX_WX,									// 66 0F68
		VEX_Vpunpckhbw_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 68
		VEX_Vpunpckhbw_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 68
		EVEX_Vpunpckhbw_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 68
		EVEX_Vpunpckhbw_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 68
		EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 68

		Punpckhwd_P_Q,										// 0F69

		Punpckhwd_VX_WX,									// 66 0F69
		VEX_Vpunpckhwd_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 69
		VEX_Vpunpckhwd_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 69
		EVEX_Vpunpckhwd_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F.WIG 69
		EVEX_Vpunpckhwd_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F.WIG 69
		EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F.WIG 69

		Punpckhdq_P_Q,										// 0F6A

		Punpckhdq_VX_WX,									// 66 0F6A
		VEX_Vpunpckhdq_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 6A
		VEX_Vpunpckhdq_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 6A
		EVEX_Vpunpckhdq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 6A
		EVEX_Vpunpckhdq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 6A
		EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 6A

		Packssdw_P_Q,										// 0F6B

		Packssdw_VX_WX,										// 66 0F6B
		VEX_Vpackssdw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 6B
		VEX_Vpackssdw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 6B
		EVEX_Vpackssdw_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 6B
		EVEX_Vpackssdw_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 6B
		EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 6B

		Punpcklqdq_VX_WX,									// 66 0F6C
		VEX_Vpunpcklqdq_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 6C
		VEX_Vpunpcklqdq_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 6C
		EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F.W1 6C
		EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F.W1 6C
		EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b,					// EVEX.NDS.512.66.0F.W1 6C

		Punpckhqdq_VX_WX,									// 66 0F6D
		VEX_Vpunpckhqdq_VX_HX_WX,							// VEX.NDS.128.66.0F.WIG 6D
		VEX_Vpunpckhqdq_VY_HY_WY,							// VEX.NDS.256.66.0F.WIG 6D
		EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F.W1 6D
		EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F.W1 6D
		EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b,					// EVEX.NDS.512.66.0F.W1 6D

		Movd_P_Ed,											// 0F6E
		Movq_P_Eq,											// REX.W 0F6E

		Movd_VX_Ed,											// 66 0F6E
		Movq_VX_Eq,											// 66 REX.W 0F6E
		VEX_Vmovd_VX_Ed,									// VEX.128.66.0F.W0 6E
		VEX_Vmovq_VX_Eq,									// VEX.128.66.0F.W1 6E
		EVEX_Vmovd_VX_Ed,									// EVEX.128.66.0F.W0 6E
		EVEX_Vmovq_VX_Eq,									// EVEX.128.66.0F.W1 6E

		Movq_P_Q,											// 0F6F

		Movdqa_VX_WX,										// 66 0F6F
		VEX_Vmovdqa_VX_WX,									// VEX.128.66.0F.WIG 6F
		VEX_Vmovdqa_VY_WY,									// VEX.256.66.0F.WIG 6F
		EVEX_Vmovdqa32_VX_k1z_WX,							// EVEX.128.66.0F.W0 6F
		EVEX_Vmovdqa32_VY_k1z_WY,							// EVEX.256.66.0F.W0 6F
		EVEX_Vmovdqa32_VZ_k1z_WZ,							// EVEX.512.66.0F.W0 6F
		EVEX_Vmovdqa64_VX_k1z_WX,							// EVEX.128.66.0F.W1 6F
		EVEX_Vmovdqa64_VY_k1z_WY,							// EVEX.256.66.0F.W1 6F
		EVEX_Vmovdqa64_VZ_k1z_WZ,							// EVEX.512.66.0F.W1 6F

		Movdqu_VX_WX,										// F3 0F6F
		VEX_Vmovdqu_VX_WX,									// VEX.128.F3.0F.WIG 6F
		VEX_Vmovdqu_VY_WY,									// VEX.256.F3.0F.WIG 6F
		EVEX_Vmovdqu32_VX_k1z_WX,							// EVEX.128.F3.0F.W0 6F
		EVEX_Vmovdqu32_VY_k1z_WY,							// EVEX.256.F3.0F.W0 6F
		EVEX_Vmovdqu32_VZ_k1z_WZ,							// EVEX.512.F3.0F.W0 6F
		EVEX_Vmovdqu64_VX_k1z_WX,							// EVEX.128.F3.0F.W1 6F
		EVEX_Vmovdqu64_VY_k1z_WY,							// EVEX.256.F3.0F.W1 6F
		EVEX_Vmovdqu64_VZ_k1z_WZ,							// EVEX.512.F3.0F.W1 6F

		EVEX_Vmovdqu8_VX_k1z_WX,							// EVEX.128.F2.0F.W0 6F
		EVEX_Vmovdqu8_VY_k1z_WY,							// EVEX.256.F2.0F.W0 6F
		EVEX_Vmovdqu8_VZ_k1z_WZ,							// EVEX.512.F2.0F.W0 6F
		EVEX_Vmovdqu16_VX_k1z_WX,							// EVEX.128.F2.0F.W1 6F
		EVEX_Vmovdqu16_VY_k1z_WY,							// EVEX.256.F2.0F.W1 6F
		EVEX_Vmovdqu16_VZ_k1z_WZ,							// EVEX.512.F2.0F.W1 6F

		Pshufw_P_Q_Ib,										// 0F70

		Pshufd_VX_WX_Ib,									// 66 0F70
		VEX_Vpshufd_VX_WX_Ib,								// VEX.128.66.0F.WIG 70
		VEX_Vpshufd_VY_WY_Ib,								// VEX.256.66.0F.WIG 70
		EVEX_Vpshufd_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F.W0 70
		EVEX_Vpshufd_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F.W0 70
		EVEX_Vpshufd_VZ_k1z_WZ_Ib_b,						// EVEX.512.66.0F.W0 70

		Pshufhw_VX_WX_Ib,									// F3 0F70
		VEX_Vpshufhw_VX_WX_Ib,								// VEX.128.F3.0F.WIG 70
		VEX_Vpshufhw_VY_WY_Ib,								// VEX.256.F3.0F.WIG 70
		EVEX_Vpshufhw_VX_k1z_WX_Ib,							// EVEX.128.F3.0F.WIG 70
		EVEX_Vpshufhw_VY_k1z_WY_Ib,							// EVEX.256.F3.0F.WIG 70
		EVEX_Vpshufhw_VZ_k1z_WZ_Ib,							// EVEX.512.F3.0F.WIG 70

		Pshuflw_VX_WX_Ib,									// F2 0F70
		VEX_Vpshuflw_VX_WX_Ib,								// VEX.128.F2.0F.WIG 70
		VEX_Vpshuflw_VY_WY_Ib,								// VEX.256.F2.0F.WIG 70
		EVEX_Vpshuflw_VX_k1z_WX_Ib,							// EVEX.128.F2.0F.WIG 70
		EVEX_Vpshuflw_VY_k1z_WY_Ib,							// EVEX.256.F2.0F.WIG 70
		EVEX_Vpshuflw_VZ_k1z_WZ_Ib,							// EVEX.512.F2.0F.WIG 70

		Psrlw_N_Ib,											// 0F71 /2

		Psrlw_RX_Ib,										// 66 0F71 /2
		VEX_Vpsrlw_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 71 /2
		VEX_Vpsrlw_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 71 /2
		EVEX_Vpsrlw_HX_k1z_WX_Ib,							// EVEX.NDD.128.66.0F.WIG 71 /2
		EVEX_Vpsrlw_HY_k1z_WY_Ib,							// EVEX.NDD.256.66.0F.WIG 71 /2
		EVEX_Vpsrlw_HZ_k1z_WZ_Ib,							// EVEX.NDD.512.66.0F.WIG 71 /2

		Psraw_N_Ib,											// 0F71 /4

		Psraw_RX_Ib,										// 66 0F71 /4
		VEX_Vpsraw_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 71 /4
		VEX_Vpsraw_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 71 /4
		EVEX_Vpsraw_HX_k1z_WX_Ib,							// EVEX.NDD.128.66.0F.WIG 71 /4
		EVEX_Vpsraw_HY_k1z_WY_Ib,							// EVEX.NDD.256.66.0F.WIG 71 /4
		EVEX_Vpsraw_HZ_k1z_WZ_Ib,							// EVEX.NDD.512.66.0F.WIG 71 /4

		Psllw_N_Ib,											// 0F71 /6

		Psllw_RX_Ib,										// 66 0F71 /6
		VEX_Vpsllw_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 71 /6
		VEX_Vpsllw_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 71 /6
		EVEX_Vpsllw_HX_k1z_WX_Ib,							// EVEX.NDD.128.66.0F.WIG 71 /6
		EVEX_Vpsllw_HY_k1z_WY_Ib,							// EVEX.NDD.256.66.0F.WIG 71 /6
		EVEX_Vpsllw_HZ_k1z_WZ_Ib,							// EVEX.NDD.512.66.0F.WIG 71 /6

		EVEX_Vprord_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W0 72 /0
		EVEX_Vprord_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W0 72 /0
		EVEX_Vprord_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W0 72 /0
		EVEX_Vprorq_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W1 72 /0
		EVEX_Vprorq_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W1 72 /0
		EVEX_Vprorq_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W1 72 /0

		EVEX_Vprold_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W0 72 /1
		EVEX_Vprold_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W0 72 /1
		EVEX_Vprold_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W0 72 /1
		EVEX_Vprolq_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W1 72 /1
		EVEX_Vprolq_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W1 72 /1
		EVEX_Vprolq_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W1 72 /1

		Psrld_N_Ib,											// 0F72 /2

		Psrld_RX_Ib,										// 66 0F72 /2
		VEX_Vpsrld_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 72 /2
		VEX_Vpsrld_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 72 /2
		EVEX_Vpsrld_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W0 72 /2
		EVEX_Vpsrld_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W0 72 /2
		EVEX_Vpsrld_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W0 72 /2

		Psrad_N_Ib,											// 0F72 /4

		Psrad_RX_Ib,										// 66 0F72 /4
		VEX_Vpsrad_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 72 /4
		VEX_Vpsrad_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 72 /4
		EVEX_Vpsrad_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W0 72 /4
		EVEX_Vpsrad_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W0 72 /4
		EVEX_Vpsrad_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W0 72 /4
		EVEX_Vpsraq_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W1 72 /4
		EVEX_Vpsraq_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W1 72 /4
		EVEX_Vpsraq_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W1 72 /4

		Pslld_N_Ib,											// 0F72 /6

		Pslld_RX_Ib,										// 66 0F72 /6
		VEX_Vpslld_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 72 /6
		VEX_Vpslld_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 72 /6
		EVEX_Vpslld_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W0 72 /6
		EVEX_Vpslld_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W0 72 /6
		EVEX_Vpslld_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W0 72 /6

		Psrlq_N_Ib,											// 0F73 /2

		Psrlq_RX_Ib,										// 66 0F73 /2
		VEX_Vpsrlq_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 73 /2
		VEX_Vpsrlq_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 73 /2
		EVEX_Vpsrlq_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W1 73 /2
		EVEX_Vpsrlq_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W1 73 /2
		EVEX_Vpsrlq_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W1 73 /2

		Psrldq_RX_Ib,										// 66 0F73 /3
		VEX_Vpsrldq_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 73 /3
		VEX_Vpsrldq_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 73 /3
		EVEX_Vpsrldq_HX_WX_Ib,								// EVEX.NDD.128.66.0F.WIG 73 /3
		EVEX_Vpsrldq_HY_WY_Ib,								// EVEX.NDD.256.66.0F.WIG 73 /3
		EVEX_Vpsrldq_HZ_WZ_Ib,								// EVEX.NDD.512.66.0F.WIG 73 /3

		Psllq_N_Ib,											// 0F73 /6

		Psllq_RX_Ib,										// 66 0F73 /6
		VEX_Vpsllq_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 73 /6
		VEX_Vpsllq_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 73 /6
		EVEX_Vpsllq_HX_k1z_WX_Ib_b,							// EVEX.NDD.128.66.0F.W1 73 /6
		EVEX_Vpsllq_HY_k1z_WY_Ib_b,							// EVEX.NDD.256.66.0F.W1 73 /6
		EVEX_Vpsllq_HZ_k1z_WZ_Ib_b,							// EVEX.NDD.512.66.0F.W1 73 /6

		Pslldq_RX_Ib,										// 66 0F73 /7
		VEX_Vpslldq_HX_RX_Ib,								// VEX.NDD.128.66.0F.WIG 73 /7
		VEX_Vpslldq_HY_RY_Ib,								// VEX.NDD.256.66.0F.WIG 73 /7
		EVEX_Vpslldq_HX_WX_Ib,								// EVEX.NDD.128.66.0F.WIG 73 /7
		EVEX_Vpslldq_HY_WY_Ib,								// EVEX.NDD.256.66.0F.WIG 73 /7
		EVEX_Vpslldq_HZ_WZ_Ib,								// EVEX.NDD.512.66.0F.WIG 73 /7

		Pcmpeqb_P_Q,										// 0F74

		Pcmpeqb_VX_WX,										// 66 0F74
		VEX_Vpcmpeqb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 74
		VEX_Vpcmpeqb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 74
		EVEX_Vpcmpeqb_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F.WIG 74
		EVEX_Vpcmpeqb_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F.WIG 74
		EVEX_Vpcmpeqb_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG 74

		Pcmpeqw_P_Q,										// 0F75

		Pcmpeqw_VX_WX,										// 66 0F75
		VEX_Vpcmpeqw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 75
		VEX_Vpcmpeqw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 75
		EVEX_Vpcmpeqw_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F.WIG 75
		EVEX_Vpcmpeqw_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F.WIG 75
		EVEX_Vpcmpeqw_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG 75

		Pcmpeqd_P_Q,										// 0F76

		Pcmpeqd_VX_WX,										// 66 0F76
		VEX_Vpcmpeqd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 76
		VEX_Vpcmpeqd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 76
		EVEX_Vpcmpeqd_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 76
		EVEX_Vpcmpeqd_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 76
		EVEX_Vpcmpeqd_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 76

		Emms,												// 0F77
		VEX_Vzeroupper,										// VEX.128.0F.WIG 77
		VEX_Vzeroall,										// VEX.256.0F.WIG 77

		Vmread_Ed_Gd,										// 0F78
		Vmread_Eq_Gq,										// REX.W 0F78

		Vmwrite_Gd_Ed,										// 0F79
		Vmwrite_Gq_Eq,										// REX.W 0F79

		EVEX_Vcvttps2udq_VX_k1z_WX_b,						// EVEX.128.0F.W0 78
		EVEX_Vcvttps2udq_VY_k1z_WY_b,						// EVEX.256.0F.W0 78
		EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b,					// EVEX.512.0F.W0 78
		EVEX_Vcvttpd2udq_VX_k1z_WX_b,						// EVEX.128.0F.W1 78
		EVEX_Vcvttpd2udq_VX_k1z_WY_b,						// EVEX.256.0F.W1 78
		EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b,					// EVEX.512.0F.W1 78

		EVEX_Vcvttps2uqq_VX_k1z_WX_b,						// EVEX.128.66.0F.W0 78
		EVEX_Vcvttps2uqq_VY_k1z_WX_b,						// EVEX.256.66.0F.W0 78
		EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b,					// EVEX.512.66.0F.W0 78
		EVEX_Vcvttpd2uqq_VX_k1z_WX_b,						// EVEX.128.66.0F.W1 78
		EVEX_Vcvttpd2uqq_VY_k1z_WY_b,						// EVEX.256.66.0F.W1 78
		EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b,					// EVEX.512.66.0F.W1 78

		EVEX_Vcvttss2usi_Gd_WX_sae,							// EVEX.LIG.F3.0F.W0 78
		EVEX_Vcvttss2usi_Gq_WX_sae,							// EVEX.LIG.F3.0F.W1 78

		EVEX_Vcvttsd2usi_Gd_WX_sae,							// EVEX.LIG.F2.0F.W0 78
		EVEX_Vcvttsd2usi_Gq_WX_sae,							// EVEX.LIG.F2.0F.W1 78

		EVEX_Vcvtps2udq_VX_k1z_WX_b,						// EVEX.128.0F.W0 79
		EVEX_Vcvtps2udq_VY_k1z_WY_b,						// EVEX.256.0F.W0 79
		EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b,						// EVEX.512.0F.W0 79
		EVEX_Vcvtpd2udq_VX_k1z_WX_b,						// EVEX.128.0F.W1 79
		EVEX_Vcvtpd2udq_VX_k1z_WY_b,						// EVEX.256.0F.W1 79
		EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b,						// EVEX.512.0F.W1 79

		EVEX_Vcvtps2uqq_VX_k1z_WX_b,						// EVEX.128.66.0F.W0 79
		EVEX_Vcvtps2uqq_VY_k1z_WX_b,						// EVEX.256.66.0F.W0 79
		EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b,						// EVEX.512.66.0F.W0 79
		EVEX_Vcvtpd2uqq_VX_k1z_WX_b,						// EVEX.128.66.0F.W1 79
		EVEX_Vcvtpd2uqq_VY_k1z_WY_b,						// EVEX.256.66.0F.W1 79
		EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b,						// EVEX.512.66.0F.W1 79

		EVEX_Vcvtss2usi_Gd_WX_er,							// EVEX.LIG.F3.0F.W0 79
		EVEX_Vcvtss2usi_Gq_WX_er,							// EVEX.LIG.F3.0F.W1 79

		EVEX_Vcvtsd2usi_Gd_WX_er,							// EVEX.LIG.F2.0F.W0 79
		EVEX_Vcvtsd2usi_Gq_WX_er,							// EVEX.LIG.F2.0F.W1 79

		EVEX_Vcvttps2qq_VX_k1z_WX_b,						// EVEX.128.66.0F.W0 7A
		EVEX_Vcvttps2qq_VY_k1z_WX_b,						// EVEX.256.66.0F.W0 7A
		EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b,					// EVEX.512.66.0F.W0 7A
		EVEX_Vcvttpd2qq_VX_k1z_WX_b,						// EVEX.128.66.0F.W1 7A
		EVEX_Vcvttpd2qq_VY_k1z_WY_b,						// EVEX.256.66.0F.W1 7A
		EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b,					// EVEX.512.66.0F.W1 7A

		EVEX_Vcvtudq2pd_VX_k1z_WX_b,						// EVEX.128.F3.0F.W0 7A
		EVEX_Vcvtudq2pd_VY_k1z_WX_b,						// EVEX.256.F3.0F.W0 7A
		EVEX_Vcvtudq2pd_VZ_k1z_WY_b,						// EVEX.512.F3.0F.W0 7A
		EVEX_Vcvtuqq2pd_VX_k1z_WX_b,						// EVEX.128.F3.0F.W1 7A
		EVEX_Vcvtuqq2pd_VY_k1z_WY_b,						// EVEX.256.F3.0F.W1 7A
		EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b,						// EVEX.512.F3.0F.W1 7A

		EVEX_Vcvtudq2ps_VX_k1z_WX_b,						// EVEX.128.F2.0F.W0 7A
		EVEX_Vcvtudq2ps_VY_k1z_WY_b,						// EVEX.256.F2.0F.W0 7A
		EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b,						// EVEX.512.F2.0F.W0 7A
		EVEX_Vcvtuqq2ps_VX_k1z_WX_b,						// EVEX.128.F2.0F.W1 7A
		EVEX_Vcvtuqq2ps_VX_k1z_WY_b,						// EVEX.256.F2.0F.W1 7A
		EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b,						// EVEX.512.F2.0F.W1 7A

		EVEX_Vcvtps2qq_VX_k1z_WX_b,							// EVEX.128.66.0F.W0 7B
		EVEX_Vcvtps2qq_VY_k1z_WX_b,							// EVEX.256.66.0F.W0 7B
		EVEX_Vcvtps2qq_VZ_k1z_WY_er_b,						// EVEX.512.66.0F.W0 7B
		EVEX_Vcvtpd2qq_VX_k1z_WX_b,							// EVEX.128.66.0F.W1 7B
		EVEX_Vcvtpd2qq_VY_k1z_WY_b,							// EVEX.256.66.0F.W1 7B
		EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b,						// EVEX.512.66.0F.W1 7B

		EVEX_Vcvtusi2ss_VX_HX_Ed_er,						// EVEX.NDS.LIG.F3.0F.W0 7B
		EVEX_Vcvtusi2ss_VX_HX_Eq_er,						// EVEX.NDS.LIG.F3.0F.W1 7B

		EVEX_Vcvtusi2sd_VX_HX_Ed,							// EVEX.NDS.LIG.F2.0F.W0 7B
		EVEX_Vcvtusi2sd_VX_HX_Eq_er,						// EVEX.NDS.LIG.F2.0F.W1 7B

		Haddpd_VX_WX,										// 66 0F7C
		VEX_Vhaddpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 7C
		VEX_Vhaddpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 7C

		Haddps_VX_WX,										// F2 0F7C
		VEX_Vhaddps_VX_HX_WX,								// VEX.NDS.128.F2.0F.WIG 7C
		VEX_Vhaddps_VY_HY_WY,								// VEX.NDS.256.F2.0F.WIG 7C

		Hsubpd_VX_WX,										// 66 0F7D
		VEX_Vhsubpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG 7D
		VEX_Vhsubpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG 7D

		Hsubps_VX_WX,										// F2 0F7D
		VEX_Vhsubps_VX_HX_WX,								// VEX.NDS.128.F2.0F.WIG 7D
		VEX_Vhsubps_VY_HY_WY,								// VEX.NDS.256.F2.0F.WIG 7D

		Movd_Ed_P,											// 0F7E
		Movq_Eq_P,											// REX.W 0F7E

		Movd_Ed_VX,											// 66 0F7E
		Movq_Eq_VX,											// 66 REX.W 0F7E
		VEX_Vmovd_Ed_VX,									// VEX.128.66.0F.W0 7E
		VEX_Vmovq_Eq_VX,									// VEX.128.66.0F.W1 7E
		EVEX_Vmovd_Ed_VX,									// EVEX.128.66.0F.W0 7E
		EVEX_Vmovq_Eq_VX,									// EVEX.128.66.0F.W1 7E

		Movq_VX_WX,											// F3 0F7E
		VEX_Vmovq_VX_WX,									// VEX.128.F3.0F.WIG 7E
		EVEX_Vmovq_VX_WX,									// EVEX.128.F3.0F.W1 7E

		Movq_Q_P,											// 0F7F

		Movdqa_WX_VX,										// 66 0F7F
		VEX_Vmovdqa_WX_VX,									// VEX.128.66.0F.WIG 7F
		VEX_Vmovdqa_WY_VY,									// VEX.256.66.0F.WIG 7F
		EVEX_Vmovdqa32_WX_k1z_VX,							// EVEX.128.66.0F.W0 7F
		EVEX_Vmovdqa32_WY_k1z_VY,							// EVEX.256.66.0F.W0 7F
		EVEX_Vmovdqa32_WZ_k1z_VZ,							// EVEX.512.66.0F.W0 7F
		EVEX_Vmovdqa64_WX_k1z_VX,							// EVEX.128.66.0F.W1 7F
		EVEX_Vmovdqa64_WY_k1z_VY,							// EVEX.256.66.0F.W1 7F
		EVEX_Vmovdqa64_WZ_k1z_VZ,							// EVEX.512.66.0F.W1 7F

		Movdqu_WX_VX,										// F3 0F7F
		VEX_Vmovdqu_WX_VX,									// VEX.128.F3.0F.WIG 7F
		VEX_Vmovdqu_WY_VY,									// VEX.256.F3.0F.WIG 7F
		EVEX_Vmovdqu32_WX_k1z_VX,							// EVEX.128.F3.0F.W0 7F
		EVEX_Vmovdqu32_WY_k1z_VY,							// EVEX.256.F3.0F.W0 7F
		EVEX_Vmovdqu32_WZ_k1z_VZ,							// EVEX.512.F3.0F.W0 7F
		EVEX_Vmovdqu64_WX_k1z_VX,							// EVEX.128.F3.0F.W1 7F
		EVEX_Vmovdqu64_WY_k1z_VY,							// EVEX.256.F3.0F.W1 7F
		EVEX_Vmovdqu64_WZ_k1z_VZ,							// EVEX.512.F3.0F.W1 7F

		EVEX_Vmovdqu8_WX_k1z_VX,							// EVEX.128.F2.0F.W0 7F
		EVEX_Vmovdqu8_WY_k1z_VY,							// EVEX.256.F2.0F.W0 7F
		EVEX_Vmovdqu8_WZ_k1z_VZ,							// EVEX.512.F2.0F.W0 7F
		EVEX_Vmovdqu16_WX_k1z_VX,							// EVEX.128.F2.0F.W1 7F
		EVEX_Vmovdqu16_WY_k1z_VY,							// EVEX.256.F2.0F.W1 7F
		EVEX_Vmovdqu16_WZ_k1z_VZ,							// EVEX.512.F2.0F.W1 7F

		Jo_Jw16,											// o16 0F80
		Jo_Jd32,											// o32 0F80
		Jo_Jd64,											// 0F80
		Jno_Jw16,											// o16 0F81
		Jno_Jd32,											// o32 0F81
		Jno_Jd64,											// 0F81
		Jb_Jw16,											// o16 0F82
		Jb_Jd32,											// o32 0F82
		Jb_Jd64,											// 0F82
		Jae_Jw16,											// o16 0F83
		Jae_Jd32,											// o32 0F83
		Jae_Jd64,											// 0F83
		Je_Jw16,											// o16 0F84
		Je_Jd32,											// o32 0F84
		Je_Jd64,											// 0F84
		Jne_Jw16,											// o16 0F85
		Jne_Jd32,											// o32 0F85
		Jne_Jd64,											// 0F85
		Jbe_Jw16,											// o16 0F86
		Jbe_Jd32,											// o32 0F86
		Jbe_Jd64,											// 0F86
		Ja_Jw16,											// o16 0F87
		Ja_Jd32,											// o32 0F87
		Ja_Jd64,											// 0F87

		Js_Jw16,											// o16 0F88
		Js_Jd32,											// o32 0F88
		Js_Jd64,											// 0F88
		Jns_Jw16,											// o16 0F89
		Jns_Jd32,											// o32 0F89
		Jns_Jd64,											// 0F89
		Jp_Jw16,											// o16 0F8A
		Jp_Jd32,											// o32 0F8A
		Jp_Jd64,											// 0F8A
		Jnp_Jw16,											// o16 0F8B
		Jnp_Jd32,											// o32 0F8B
		Jnp_Jd64,											// 0F8B
		Jl_Jw16,											// o16 0F8C
		Jl_Jd32,											// o32 0F8C
		Jl_Jd64,											// 0F8C
		Jge_Jw16,											// o16 0F8D
		Jge_Jd32,											// o32 0F8D
		Jge_Jd64,											// 0F8D
		Jle_Jw16,											// o16 0F8E
		Jle_Jd32,											// o32 0F8E
		Jle_Jd64,											// 0F8E
		Jg_Jw16,											// o16 0F8F
		Jg_Jd32,											// o32 0F8F
		Jg_Jd64,											// 0F8F

		Seto_Eb,											// 0F90
		Setno_Eb,											// 0F91
		Setb_Eb,											// 0F92
		Setae_Eb,											// 0F93
		Sete_Eb,											// 0F94
		Setne_Eb,											// 0F95
		Setbe_Eb,											// 0F96
		Seta_Eb,											// 0F97

		Sets_Eb,											// 0F98
		Setns_Eb,											// 0F99
		Setp_Eb,											// 0F9A
		Setnp_Eb,											// 0F9B
		Setl_Eb,											// 0F9C
		Setge_Eb,											// 0F9D
		Setle_Eb,											// 0F9E
		Setg_Eb,											// 0F9F

		VEX_Kmovw_VK_WK,									// VEX.L0.0F.W0 90
		VEX_Kmovq_VK_WK,									// VEX.L0.0F.W1 90

		VEX_Kmovb_VK_WK,									// VEX.L0.66.0F.W0 90
		VEX_Kmovd_VK_WK,									// VEX.L0.66.0F.W1 90

		VEX_Kmovw_MK_VK,									// VEX.L0.0F.W0 91
		VEX_Kmovq_MK_VK,									// VEX.L0.0F.W1 91

		VEX_Kmovb_MK_VK,									// VEX.L0.66.0F.W0 91
		VEX_Kmovd_MK_VK,									// VEX.L0.66.0F.W1 91

		VEX_Kmovw_VK_Rd,									// VEX.L0.0F.W0 92

		VEX_Kmovb_VK_Rd,									// VEX.L0.66.0F.W0 92

		VEX_Kmovq_VK_Rq,									// VEX.L0.F2.0F.W1 92
		VEX_Kmovd_VK_Rd,									// VEX.L0.F2.0F.W0 92

		VEX_Kmovw_Gd_RK,									// VEX.L0.0F.W0 93

		VEX_Kmovb_Gd_RK,									// VEX.L0.66.0F.W0 93

		VEX_Kmovq_Gq_RK,									// VEX.L0.F2.0F.W1 93
		VEX_Kmovd_Gd_RK,									// VEX.L0.F2.0F.W0 93

		VEX_Kortestw_VK_RK,									// VEX.L0.0F.W0 98
		VEX_Kortestq_VK_RK,									// VEX.L0.0F.W1 98

		VEX_Kortestb_VK_RK,									// VEX.L0.66.0F.W0 98
		VEX_Kortestd_VK_RK,									// VEX.L0.66.0F.W1 98

		VEX_Ktestw_VK_RK,									// VEX.L0.0F.W0 99
		VEX_Ktestq_VK_RK,									// VEX.L0.0F.W1 99

		VEX_Ktestb_VK_RK,									// VEX.L0.66.0F.W0 99
		VEX_Ktestd_VK_RK,									// VEX.L0.66.0F.W1 99

		Pushw_FS,											// o16 0FA0
		Pushd_FS,											// o32 0FA0
		Pushq_FS,											// 0FA0
		Popw_FS,											// o16 0FA1
		Popd_FS,											// o32 0FA1
		Popq_FS,											// 0FA1
		Cpuid,												// 0FA2
		Bt_Ew_Gw,											// o16 0FA3
		Bt_Ed_Gd,											// o32 0FA3
		Bt_Eq_Gq,											// REX.W 0FA3
		Shld_Ew_Gw_Ib,										// o16 0FA4
		Shld_Ed_Gd_Ib,										// o32 0FA4
		Shld_Eq_Gq_Ib,										// REX.W 0FA4
		Shld_Ew_Gw_CL,										// o16 0FA5
		Shld_Ed_Gd_CL,										// o32 0FA5
		Shld_Eq_Gq_CL,										// REX.W 0FA5

		Pushw_GS,											// o16 0FA8
		Pushd_GS,											// o32 0FA8
		Pushq_GS,											// 0FA8
		Popw_GS,											// o16 0FA9
		Popd_GS,											// o32 0FA9
		Popq_GS,											// 0FA9
		Rsm,												// 0FAA
		Bts_Ew_Gw,											// o16 0FAB
		Bts_Ed_Gd,											// o32 0FAB
		Bts_Eq_Gq,											// REX.W 0FAB
		Shrd_Ew_Gw_Ib,										// o16 0FAC
		Shrd_Ed_Gd_Ib,										// o32 0FAC
		Shrd_Eq_Gq_Ib,										// REX.W 0FAC
		Shrd_Ew_Gw_CL,										// o16 0FAD
		Shrd_Ed_Gd_CL,										// o32 0FAD
		Shrd_Eq_Gq_CL,										// REX.W 0FAD

		Fxsave_M,											// 0FAE /0
		Fxsave64_M,											// REX.W 0FAE /0
		Rdfsbase_Rd,										// F3 0FAE /0
		Rdfsbase_Rq,										// F3 REX.W 0FAE /0
		Fxrstor_M,											// 0FAE /1
		Fxrstor64_M,										// REX.W 0FAE /1
		Rdgsbase_Rd,										// F3 0FAE /1
		Rdgsbase_Rq,										// F3 REX.W 0FAE /1
		Ldmxcsr_Md,											// 0FAE /2
		Wrfsbase_Rd,										// F3 0FAE /2
		Wrfsbase_Rq,										// F3 REX.W 0FAE /2
		VEX_Vldmxcsr_Md,									// VEX.L0.0F.WIG AE /2
		Stmxcsr_Md,											// 0FAE /3
		Wrgsbase_Rd,										// F3 0FAE /3
		Wrgsbase_Rq,										// F3 REX.W 0FAE /3
		VEX_Vstmxcsr_Md,									// VEX.L0.0F.WIG AE /3
		Xsave_M,											// 0FAE /4
		Xsave64_M,											// REX.W 0FAE /4
		Ptwrite_Ed,											// F3 0FAE /4
		Ptwrite_Eq,											// F3 REX.W 0FAE /4
		Xrstor_M,											// 0FAE /5
		Xrstor64_M,											// REX.W 0FAE /5
		Xsaveopt_M,											// 0FAE /6
		Xsaveopt64_M,										// REX.W 0FAE /6
		Clwb_Mb,											// 66 0FAE /6
		Clflush_Mb,											// 0FAE /7
		Clflushopt_Mb,										// 66 0FAE /7
		Lfence,												// 0FAE E8-EF
		Mfence,												// 0FAE F0-F7
		Sfence,												// 0FAE F8-FF
		Imul_Gw_Ew,											// o16 0FAF
		Imul_Gd_Ed,											// o32 0FAF
		Imul_Gq_Eq,											// REX.W 0FAF

		Cmpxchg_Eb_Gb,										// 0FB0
		Cmpxchg_Ew_Gw,										// o16 0FB1
		Cmpxchg_Ed_Gd,										// o32 0FB1
		Cmpxchg_Eq_Gq,										// REX.W 0FB1
		Lss_Gw_Mp,											// o16 0FB2
		Lss_Gd_Mp,											// o32 0FB2
		Lss_Gq_Mp,											// REX.W 0FB2
		Btr_Ew_Gw,											// o16 0FB3
		Btr_Ed_Gd,											// o32 0FB3
		Btr_Eq_Gq,											// REX.W 0FB3
		Lfs_Gw_Mp,											// o16 0FB4
		Lfs_Gd_Mp,											// o32 0FB4
		Lfs_Gq_Mp,											// REX.W 0FB4
		Lgs_Gw_Mp,											// o16 0FB5
		Lgs_Gd_Mp,											// o32 0FB5
		Lgs_Gq_Mp,											// REX.W 0FB5
		Movzx_Gw_Eb,										// o16 0FB6
		Movzx_Gd_Eb,										// o32 0FB6
		Movzx_Gq_Eb,										// REX.W 0FB6
		Movzx_Gw_Ew,										// o16 0FB7
		Movzx_Gd_Ew,										// o32 0FB7
		Movzx_Gq_Ew,										// REX.W 0FB7

		Popcnt_Gw_Ew,										// o16 F3 0FB8
		Popcnt_Gd_Ed,										// o32 F3 0FB8
		Popcnt_Gq_Eq,										// F3 REX.W 0FB8

		Ud1_Gw_Ew,											// o16 0FB9
		Ud1_Gd_Ed,											// o32 0FB9
		Ud1_Gq_Eq,											// REX.W 0FB9
		Bt_Ew_Ib,											// o16 0FBA /4
		Bt_Ed_Ib,											// o32 0FBA /4
		Bt_Eq_Ib,											// REX.W 0FBA /4
		Bts_Ew_Ib,											// o16 0FBA /5
		Bts_Ed_Ib,											// o32 0FBA /5
		Bts_Eq_Ib,											// REX.W 0FBA /5
		Btr_Ew_Ib,											// o16 0FBA /6
		Btr_Ed_Ib,											// o32 0FBA /6
		Btr_Eq_Ib,											// REX.W 0FBA /6
		Btc_Ew_Ib,											// o16 0FBA /7
		Btc_Ed_Ib,											// o32 0FBA /7
		Btc_Eq_Ib,											// REX.W 0FBA /7
		Btc_Ew_Gw,											// o16 0FBB
		Btc_Ed_Gd,											// o32 0FBB
		Btc_Eq_Gq,											// REX.W 0FBB
		Bsf_Gw_Ew,											// o16 0FBC
		Bsf_Gd_Ed,											// o32 0FBC
		Bsf_Gq_Eq,											// REX.W 0FBC
		Bsr_Gw_Ew,											// o16 0FBD
		Bsr_Gd_Ed,											// o32 0FBD
		Bsr_Gq_Eq,											// REX.W 0FBD
		Movsx_Gw_Eb,										// o16 0FBE
		Movsx_Gd_Eb,										// o32 0FBE
		Movsx_Gq_Eb,										// REX.W 0FBE
		Movsx_Gw_Ew,										// o16 0FBF
		Movsx_Gd_Ew,										// o32 0FBF
		Movsx_Gq_Ew,										// REX.W 0FBF

		Tzcnt_Gw_Ew,										// o16 F3 0FBC
		Tzcnt_Gd_Ed,										// o32 F3 0FBC
		Tzcnt_Gq_Eq,										// F3 REX.W 0FBC

		Lzcnt_Gw_Ew,										// o16 F3 0FBD
		Lzcnt_Gd_Ed,										// o32 F3 0FBD
		Lzcnt_Gq_Eq,										// F3 REX.W 0FBD

		Xadd_Eb_Gb,											// 0FC0
		Xadd_Ew_Gw,											// o16 0FC1
		Xadd_Ed_Gd,											// o32 0FC1
		Xadd_Eq_Gq,											// REX.W 0FC1

		Cmpps_VX_WX_Ib,										// 0FC2
		VEX_Vcmpps_VX_HX_WX_Ib,								// VEX.NDS.128.0F.WIG C2
		VEX_Vcmpps_VY_HY_WY_Ib,								// VEX.NDS.256.0F.WIG C2
		EVEX_Vcmpps_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.0F.W0 C2
		EVEX_Vcmpps_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.0F.W0 C2
		EVEX_Vcmpps_VK_k1_HZ_WZ_Ib_sae_b,					// EVEX.NDS.512.0F.W0 C2

		Cmppd_VX_WX_Ib,										// 66 0FC2
		VEX_Vcmppd_VX_HX_WX_Ib,								// VEX.NDS.128.66.0F.WIG C2
		VEX_Vcmppd_VY_HY_WY_Ib,								// VEX.NDS.256.66.0F.WIG C2
		EVEX_Vcmppd_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F.W1 C2
		EVEX_Vcmppd_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F.W1 C2
		EVEX_Vcmppd_VK_k1_HZ_WZ_Ib_sae_b,					// EVEX.NDS.512.66.0F.W1 C2

		Cmpss_VX_WX_Ib,										// F3 0FC2
		VEX_Vcmpss_VX_HX_WX_Ib,								// VEX.NDS.LIG.F3.0F.WIG C2
		EVEX_Vcmpss_VK_k1_HX_WX_Ib_sae,						// EVEX.NDS.LIG.F3.0F.W0 C2

		Cmpsd_VX_WX_Ib,										// F2 0FC2
		VEX_Vcmpsd_VX_HX_WX_Ib,								// VEX.NDS.LIG.F2.0F.WIG C2
		EVEX_Vcmpsd_VK_k1_HX_WX_Ib_sae,						// EVEX.NDS.LIG.F2.0F.W1 C2

		Movnti_Md_Gd,										// 0FC3
		Movnti_Mq_Gq,										// REX.W 0FC3

		Pinsrw_P_RdMw_Ib,									// 0FC4
		Pinsrw_P_RqMw_Ib,									// REX.W 0FC4

		Pinsrw_VX_RdMw_Ib,									// 66 0FC4
		Pinsrw_VX_RqMw_Ib,									// 66 REX.W 0FC4
		VEX_Vpinsrw_VX_HX_RdMw_Ib,							// VEX.NDS.128.66.0F.W0 C4
		VEX_Vpinsrw_VX_HX_RqMw_Ib,							// VEX.NDS.128.66.0F.W1 C4
		EVEX_Vpinsrw_VX_HX_RdMw_Ib,							// EVEX.NDS.128.66.0F.W0 C4
		EVEX_Vpinsrw_VX_HX_RqMw_Ib,							// EVEX.NDS.128.66.0F.W1 C4

		Pextrw_Gd_N_Ib,										// 0FC5
		Pextrw_Gq_N_Ib,										// REX.W 0FC5

		Pextrw_Gd_RX_Ib,									// 66 0FC5
		Pextrw_Gq_RX_Ib,									// 66 REX.W 0FC5
		VEX_Vpextrw_Gd_RX_Ib,								// VEX.128.66.0F.W0 C5
		VEX_Vpextrw_Gq_RX_Ib,								// VEX.128.66.0F.W1 C5
		EVEX_Vpextrw_Gd_RX_Ib,								// EVEX.128.66.0F.W0 C5
		EVEX_Vpextrw_Gq_RX_Ib,								// EVEX.128.66.0F.W1 C5

		Shufps_VX_WX_Ib,									// 0FC6
		VEX_Vshufps_VX_HX_WX_Ib,							// VEX.NDS.128.0F.WIG C6
		VEX_Vshufps_VY_HY_WY_Ib,							// VEX.NDS.256.0F.WIG C6
		EVEX_Vshufps_VX_k1z_HX_WX_Ib_b,						// EVEX.NDS.128.0F.W0 C6
		EVEX_Vshufps_VY_k1z_HY_WY_Ib_b,						// EVEX.NDS.256.0F.W0 C6
		EVEX_Vshufps_VZ_k1z_HZ_WZ_Ib_b,						// EVEX.NDS.512.0F.W0 C6

		Shufpd_VX_WX_Ib,									// 66 0FC6
		VEX_Vshufpd_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F.WIG C6
		VEX_Vshufpd_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F.WIG C6
		EVEX_Vshufpd_VX_k1z_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F.W1 C6
		EVEX_Vshufpd_VY_k1z_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F.W1 C6
		EVEX_Vshufpd_VZ_k1z_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F.W1 C6

		Cmpxchg8b_Mq,										// 0FC7 /1
		Cmpxchg16b_Mo,										// REX.W 0FC7 /1
		Xrstors_M,											// 0FC7 /3
		Xrstors64_M,										// REX.W 0FC7 /3
		Xsavec_M,											// 0FC7 /4
		Xsavec64_M,											// REX.W 0FC7 /4
		Xsaves_M,											// 0FC7 /5
		Xsaves64_M,											// REX.W 0FC7 /5
		Vmptrld_M,											// 0FC7 /6
		Vmclear_M,											// 66 0FC7 /6
		Vmxon_M,											// F3 0FC7 /6
		Rdrand_Rw,											// o16 0FC7 /6
		Rdrand_Rd,											// o32 0FC7 /6
		Rdrand_Rq,											// REX.W 0FC7 /6
		Vmptrst_M,											// 0FC7 /7
		Rdseed_Rw,											// o16 0FC7 /7
		Rdseed_Rd,											// o32 0FC7 /7
		Rdseed_Rq,											// REX.W 0FC7 /7
		Rdpid_Rd,											// F3 0FC7 /7
		Rdpid_Rq,											// F3 0FC7 /7

		Bswap_AX,											// o16 0FC8
		Bswap_R8W,											// o16 REX.B 0FC8
		Bswap_EAX,											// o32 0FC8
		Bswap_R8D,											// o32 REX.B 0FC8
		Bswap_RAX,											// REX.W 0FC8
		Bswap_R8,											// REX.W REX.B 0FC8
		Bswap_CX,											// o16 0FC9
		Bswap_R9W,											// o16 REX.B 0FC9
		Bswap_ECX,											// o32 0FC9
		Bswap_R9D,											// o32 REX.B 0FC9
		Bswap_RCX,											// REX.W 0FC9
		Bswap_R9,											// REX.W REX.B 0FC9
		Bswap_DX,											// o16 0FCA
		Bswap_R10W,											// o16 REX.B 0FCA
		Bswap_EDX,											// o32 0FCA
		Bswap_R10D,											// o32 REX.B 0FCA
		Bswap_RDX,											// REX.W 0FCA
		Bswap_R10,											// REX.W REX.B 0FCA
		Bswap_BX,											// o16 0FCB
		Bswap_R11W,											// o16 REX.B 0FCB
		Bswap_EBX,											// o32 0FCB
		Bswap_R11D,											// o32 REX.B 0FCB
		Bswap_RBX,											// REX.W 0FCB
		Bswap_R11,											// REX.W REX.B 0FCB
		Bswap_SP,											// o16 0FCC
		Bswap_R12W,											// o16 REX.B 0FCC
		Bswap_ESP,											// o32 0FCC
		Bswap_R12D,											// o32 REX.B 0FCC
		Bswap_RSP,											// REX.W 0FCC
		Bswap_R12,											// REX.W REX.B 0FCC
		Bswap_BP,											// o16 0FCD
		Bswap_R13W,											// o16 REX.B 0FCD
		Bswap_EBP,											// o32 0FCD
		Bswap_R13D,											// o32 REX.B 0FCD
		Bswap_RBP,											// REX.W 0FCD
		Bswap_R13,											// REX.W REX.B 0FCD
		Bswap_SI,											// o16 0FCE
		Bswap_R14W,											// o16 REX.B 0FCE
		Bswap_ESI,											// o32 0FCE
		Bswap_R14D,											// o32 REX.B 0FCE
		Bswap_RSI,											// REX.W 0FCE
		Bswap_R14,											// REX.W REX.B 0FCE
		Bswap_DI,											// o16 0FCF
		Bswap_R15W,											// o16 REX.B 0FCF
		Bswap_EDI,											// o32 0FCF
		Bswap_R15D,											// o32 REX.B 0FCF
		Bswap_RDI,											// REX.W 0FCF
		Bswap_R15,											// REX.W REX.B 0FCF

		Addsubpd_VX_WX,										// 66 0FD0
		VEX_Vaddsubpd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D0
		VEX_Vaddsubpd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG D0

		Addsubps_VX_WX,										// F2 0FD0
		VEX_Vaddsubps_VX_HX_WX,								// VEX.NDS.128.F2.0F.WIG D0
		VEX_Vaddsubps_VY_HY_WY,								// VEX.NDS.256.F2.0F.WIG D0

		Psrlw_P_Q,											// 0FD1

		Psrlw_VX_WX,										// 66 0FD1
		VEX_Vpsrlw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D1
		VEX_Vpsrlw_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG D1
		EVEX_Vpsrlw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG D1
		EVEX_Vpsrlw_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.WIG D1
		EVEX_Vpsrlw_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.WIG D1

		Psrld_P_Q,											// 0FD2

		Psrld_VX_WX,										// 66 0FD2
		VEX_Vpsrld_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D2
		VEX_Vpsrld_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG D2
		EVEX_Vpsrld_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W0 D2
		EVEX_Vpsrld_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W0 D2
		EVEX_Vpsrld_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W0 D2

		Psrlq_P_Q,											// 0FD3

		Psrlq_VX_WX,										// 66 0FD3
		VEX_Vpsrlq_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D3
		VEX_Vpsrlq_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG D3
		EVEX_Vpsrlq_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W1 D3
		EVEX_Vpsrlq_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W1 D3
		EVEX_Vpsrlq_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W1 D3

		Paddq_P_Q,											// 0FD4

		Paddq_VX_WX,										// 66 0FD4
		VEX_Vpaddq_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D4
		VEX_Vpaddq_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG D4
		EVEX_Vpaddq_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 D4
		EVEX_Vpaddq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 D4
		EVEX_Vpaddq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 D4

		Pmullw_P_Q,											// 0FD5

		Pmullw_VX_WX,										// 66 0FD5
		VEX_Vpmullw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D5
		VEX_Vpmullw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG D5
		EVEX_Vpmullw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG D5
		EVEX_Vpmullw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG D5
		EVEX_Vpmullw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG D5

		Movq_WX_VX,											// 66 0FD6
		VEX_Vmovq_WX_VX,									// VEX.128.66.0F.WIG D6
		EVEX_Vmovq_WX_VX,									// EVEX.128.66.0F.W1 D6

		Movq2dq_VX_N,										// F3 0FD6

		Movdq2q_P_RX,										// F2 0FD6

		Pmovmskb_Gd_N,										// 0FD7
		Pmovmskb_Gq_N,										// REX.W 0FD7

		Pmovmskb_Gd_RX,										// 66 0FD7
		Pmovmskb_Gq_RX,										// 66 REX.W 0FD7
		VEX_Vpmovmskb_Gd_RX,								// VEX.128.66.0F.W0 D7
		VEX_Vpmovmskb_Gq_RX,								// VEX.128.66.0F.W1 D7
		VEX_Vpmovmskb_Gd_RY,								// VEX.256.66.0F.W0 D7
		VEX_Vpmovmskb_Gq_RY,								// VEX.256.66.0F.W1 D7

		Psubusb_P_Q,										// 0FD8

		Psubusb_VX_WX,										// 66 0FD8
		VEX_Vpsubusb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D8
		VEX_Vpsubusb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG D8
		EVEX_Vpsubusb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG D8
		EVEX_Vpsubusb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG D8
		EVEX_Vpsubusb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG D8

		Psubusw_P_Q,										// 0FD9

		Psubusw_VX_WX,										// 66 0FD9
		VEX_Vpsubusw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG D9
		VEX_Vpsubusw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG D9
		EVEX_Vpsubusw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG D9
		EVEX_Vpsubusw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG D9
		EVEX_Vpsubusw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG D9

		Pminub_P_Q,											// 0FDA

		Pminub_VX_WX,										// 66 0FDA
		VEX_Vpminub_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG DA
		VEX_Vpminub_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG DA
		EVEX_Vpminub_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG DA
		EVEX_Vpminub_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG DA
		EVEX_Vpminub_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG DA

		Pand_P_Q,											// 0FDB

		Pand_VX_WX,											// 66 0FDB
		VEX_Vpand_VX_HX_WX,									// VEX.NDS.128.66.0F.WIG DB
		VEX_Vpand_VY_HY_WY,									// VEX.NDS.256.66.0F.WIG DB
		EVEX_Vpandd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W0 DB
		EVEX_Vpandd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W0 DB
		EVEX_Vpandd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W0 DB
		EVEX_Vpandq_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 DB
		EVEX_Vpandq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 DB
		EVEX_Vpandq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 DB

		Paddusb_P_Q,										// 0FDC

		Paddusb_VX_WX,										// 66 0FDC
		VEX_Vpaddusb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG DC
		VEX_Vpaddusb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG DC
		EVEX_Vpaddusb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG DC
		EVEX_Vpaddusb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG DC
		EVEX_Vpaddusb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG DC

		Paddusw_P_Q,										// 0FDD

		Paddusw_VX_WX,										// 66 0FDD
		VEX_Vpaddusw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG DD
		VEX_Vpaddusw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG DD
		EVEX_Vpaddusw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG DD
		EVEX_Vpaddusw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG DD
		EVEX_Vpaddusw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG DD

		Pmaxub_P_Q,											// 0FDE

		Pmaxub_VX_WX,										// 66 0FDE
		VEX_Vpmaxub_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG DE
		VEX_Vpmaxub_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG DE
		EVEX_Vpmaxub_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG DE
		EVEX_Vpmaxub_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG DE
		EVEX_Vpmaxub_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG DE

		Pandn_P_Q,											// 0FDF

		Pandn_VX_WX,										// 66 0FDF
		VEX_Vpandn_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG DF
		VEX_Vpandn_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG DF
		EVEX_Vpandnd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W0 DF
		EVEX_Vpandnd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W0 DF
		EVEX_Vpandnd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W0 DF
		EVEX_Vpandnq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W1 DF
		EVEX_Vpandnq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W1 DF
		EVEX_Vpandnq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W1 DF

		Pavgb_P_Q,											// 0FE0

		Pavgb_VX_WX,										// 66 0FE0
		VEX_Vpavgb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E0
		VEX_Vpavgb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E0
		EVEX_Vpavgb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E0
		EVEX_Vpavgb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E0
		EVEX_Vpavgb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E0

		Psraw_P_Q,											// 0FE1

		Psraw_VX_WX,										// 66 0FE1
		VEX_Vpsraw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E1
		VEX_Vpsraw_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG E1
		EVEX_Vpsraw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E1
		EVEX_Vpsraw_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.WIG E1
		EVEX_Vpsraw_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.WIG E1

		Psrad_P_Q,											// 0FE2

		Psrad_VX_WX,										// 66 0FE2
		VEX_Vpsrad_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E2
		VEX_Vpsrad_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG E2
		EVEX_Vpsrad_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W0 E2
		EVEX_Vpsrad_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W0 E2
		EVEX_Vpsrad_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W0 E2
		EVEX_Vpsraq_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W1 E2
		EVEX_Vpsraq_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W1 E2
		EVEX_Vpsraq_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W1 E2

		Pavgw_P_Q,											// 0FE3

		Pavgw_VX_WX,										// 66 0FE3
		VEX_Vpavgw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E3
		VEX_Vpavgw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E3
		EVEX_Vpavgw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E3
		EVEX_Vpavgw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E3
		EVEX_Vpavgw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E3

		Pmulhuw_P_Q,										// 0FE4

		Pmulhuw_VX_WX,										// 66 0FE4
		VEX_Vpmulhuw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E4
		VEX_Vpmulhuw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E4
		EVEX_Vpmulhuw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E4
		EVEX_Vpmulhuw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E4
		EVEX_Vpmulhuw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E4

		Pmulhw_P_Q,											// 0FE5

		Pmulhw_VX_WX,										// 66 0FE5
		VEX_Vpmulhw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E5
		VEX_Vpmulhw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E5
		EVEX_Vpmulhw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E5
		EVEX_Vpmulhw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E5
		EVEX_Vpmulhw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E5

		Cvttpd2dq_VX_WX,									// 66 0FE6
		VEX_Vcvttpd2dq_VX_WX,								// VEX.128.66.0F.WIG E6
		VEX_Vcvttpd2dq_VX_WY,								// VEX.256.66.0F.WIG E6
		EVEX_Vcvttpd2dq_VX_k1z_WX_b,						// EVEX.128.66.0F.W1 E6
		EVEX_Vcvttpd2dq_VX_k1z_WY_b,						// EVEX.256.66.0F.W1 E6
		EVEX_Vcvttpd2dq_VY_k1z_WZ_sae_b,					// EVEX.512.66.0F.W1 E6

		Cvtdq2pd_VX_WX,										// F3 0FE6
		VEX_Vcvtdq2pd_VX_WX,								// VEX.128.F3.0F.WIG E6
		VEX_Vcvtdq2pd_VY_WX,								// VEX.256.F3.0F.WIG E6
		EVEX_Vcvtdq2pd_VX_k1z_WX_b,							// EVEX.128.F3.0F.W0 E6
		EVEX_Vcvtdq2pd_VY_k1z_WX_b,							// EVEX.256.F3.0F.W0 E6
		EVEX_Vcvtdq2pd_VZ_k1z_WY_b,							// EVEX.512.F3.0F.W0 E6
		EVEX_Vcvtqq2pd_VX_k1z_WX_b,							// EVEX.128.F3.0F.W1 E6
		EVEX_Vcvtqq2pd_VY_k1z_WY_b,							// EVEX.256.F3.0F.W1 E6
		EVEX_Vcvtqq2pd_VZ_k1z_WZ_er_b,						// EVEX.512.F3.0F.W1 E6

		Cvtpd2dq_VX_WX,										// F2 0FE6
		VEX_Vcvtpd2dq_VX_WX,								// VEX.128.F2.0F.WIG E6
		VEX_Vcvtpd2dq_VX_WY,								// VEX.256.F2.0F.WIG E6
		EVEX_Vcvtpd2dq_VX_k1z_WX_b,							// EVEX.128.F2.0F.W1 E6
		EVEX_Vcvtpd2dq_VX_k1z_WY_b,							// EVEX.256.F2.0F.W1 E6
		EVEX_Vcvtpd2dq_VY_k1z_WZ_er_b,						// EVEX.512.F2.0F.W1 E6

		Movntq_M_P,											// 0FE7

		Movntdq_M_VX,										// 66 0FE7
		VEX_Vmovntdq_M_VX,									// VEX.128.66.0F.WIG E7
		VEX_Vmovntdq_M_VY,									// VEX.256.66.0F.WIG E7
		EVEX_Vmovntdq_M_VX,									// EVEX.128.66.0F.W0 E7
		EVEX_Vmovntdq_M_VY,									// EVEX.256.66.0F.W0 E7
		EVEX_Vmovntdq_M_VZ,									// EVEX.512.66.0F.W0 E7

		Psubsb_P_Q,											// 0FE8

		Psubsb_VX_WX,										// 66 0FE8
		VEX_Vpsubsb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E8
		VEX_Vpsubsb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E8
		EVEX_Vpsubsb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E8
		EVEX_Vpsubsb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E8
		EVEX_Vpsubsb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E8

		Psubsw_P_Q,											// 0FE9

		Psubsw_VX_WX,										// 66 0FE9
		VEX_Vpsubsw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG E9
		VEX_Vpsubsw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG E9
		EVEX_Vpsubsw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG E9
		EVEX_Vpsubsw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG E9
		EVEX_Vpsubsw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG E9

		Pminsw_P_Q,											// 0FEA

		Pminsw_VX_WX,										// 66 0FEA
		VEX_Vpminsw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG EA
		VEX_Vpminsw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG EA
		EVEX_Vpminsw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG EA
		EVEX_Vpminsw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG EA
		EVEX_Vpminsw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG EA

		Por_P_Q,											// 0FEB

		Por_VX_WX,											// 66 0FEB
		VEX_Vpor_VX_HX_WX,									// VEX.NDS.128.66.0F.WIG EB
		VEX_Vpor_VY_HY_WY,									// VEX.NDS.256.66.0F.WIG EB
		EVEX_Vpord_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W0 EB
		EVEX_Vpord_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W0 EB
		EVEX_Vpord_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W0 EB
		EVEX_Vporq_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 EB
		EVEX_Vporq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 EB
		EVEX_Vporq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 EB

		Paddsb_P_Q,											// 0FEC

		Paddsb_VX_WX,										// 66 0FEC
		VEX_Vpaddsb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG EC
		VEX_Vpaddsb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG EC
		EVEX_Vpaddsb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG EC
		EVEX_Vpaddsb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG EC
		EVEX_Vpaddsb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG EC

		Paddsw_P_Q,											// 0FED

		Paddsw_VX_WX,										// 66 0FED
		VEX_Vpaddsw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG ED
		VEX_Vpaddsw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG ED
		EVEX_Vpaddsw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG ED
		EVEX_Vpaddsw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG ED
		EVEX_Vpaddsw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG ED

		Pmaxsw_P_Q,											// 0FEE

		Pmaxsw_VX_WX,										// 66 0FEE
		VEX_Vpmaxsw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG EE
		VEX_Vpmaxsw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG EE
		EVEX_Vpmaxsw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG EE
		EVEX_Vpmaxsw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG EE
		EVEX_Vpmaxsw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG EE

		Pxor_P_Q,											// 0FEF

		Pxor_VX_WX,											// 66 0FEF
		VEX_Vpxor_VX_HX_WX,									// VEX.NDS.128.66.0F.WIG EF
		VEX_Vpxor_VY_HY_WY,									// VEX.NDS.256.66.0F.WIG EF
		EVEX_Vpxord_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W0 EF
		EVEX_Vpxord_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W0 EF
		EVEX_Vpxord_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W0 EF
		EVEX_Vpxorq_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 EF
		EVEX_Vpxorq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 EF
		EVEX_Vpxorq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 EF

		Lddqu_VX_M,											// F2 0FF0
		VEX_Vlddqu_VX_M,									// VEX.128.F2.0F.WIG F0
		VEX_Vlddqu_VY_M,									// VEX.256.F2.0F.WIG F0

		Psllw_P_Q,											// 0FF1

		Psllw_VX_WX,										// 66 0FF1
		VEX_Vpsllw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F1
		VEX_Vpsllw_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG F1
		EVEX_Vpsllw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG F1
		EVEX_Vpsllw_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.WIG F1
		EVEX_Vpsllw_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.WIG F1

		Pslld_P_Q,											// 0FF2

		Pslld_VX_WX,										// 66 0FF2
		VEX_Vpslld_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F2
		VEX_Vpslld_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG F2
		EVEX_Vpslld_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W0 F2
		EVEX_Vpslld_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W0 F2
		EVEX_Vpslld_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W0 F2

		Psllq_P_Q,											// 0FF3

		Psllq_VX_WX,										// 66 0FF3
		VEX_Vpsllq_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F3
		VEX_Vpsllq_VY_HY_WX,								// VEX.NDS.256.66.0F.WIG F3
		EVEX_Vpsllq_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.W1 F3
		EVEX_Vpsllq_VY_k1z_HY_WX,							// EVEX.NDS.256.66.0F.W1 F3
		EVEX_Vpsllq_VZ_k1z_HZ_WX,							// EVEX.NDS.512.66.0F.W1 F3

		Pmuludq_P_Q,										// 0FF4

		Pmuludq_VX_WX,										// 66 0FF4
		VEX_Vpmuludq_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F4
		VEX_Vpmuludq_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG F4
		EVEX_Vpmuludq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F.W1 F4
		EVEX_Vpmuludq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F.W1 F4
		EVEX_Vpmuludq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F.W1 F4

		Pmaddwd_P_Q,										// 0FF5

		Pmaddwd_VX_WX,										// 66 0FF5
		VEX_Vpmaddwd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F5
		VEX_Vpmaddwd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG F5
		EVEX_Vpmaddwd_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG F5
		EVEX_Vpmaddwd_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG F5
		EVEX_Vpmaddwd_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG F5

		Psadbw_P_Q,											// 0FF6

		Psadbw_VX_WX,										// 66 0FF6
		VEX_Vpsadbw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F6
		VEX_Vpsadbw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG F6
		EVEX_Vpsadbw_VX_HX_WX,								// EVEX.NDS.128.66.0F.WIG F6
		EVEX_Vpsadbw_VY_HY_WY,								// EVEX.NDS.256.66.0F.WIG F6
		EVEX_Vpsadbw_VZ_HZ_WZ,								// EVEX.NDS.512.66.0F.WIG F6

		Maskmovq_rDI_P_N,									// 0FF7

		Maskmovdqu_rDI_VX_RX,								// 66 0FF7
		VEX_Vmaskmovdqu_rDI_VX_RX,							// VEX.128.66.0F.WIG F7

		Psubb_P_Q,											// 0FF8

		Psubb_VX_WX,										// 66 0FF8
		VEX_Vpsubb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F8
		VEX_Vpsubb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG F8
		EVEX_Vpsubb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG F8
		EVEX_Vpsubb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG F8
		EVEX_Vpsubb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG F8

		Psubw_P_Q,											// 0FF9

		Psubw_VX_WX,										// 66 0FF9
		VEX_Vpsubw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG F9
		VEX_Vpsubw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG F9
		EVEX_Vpsubw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG F9
		EVEX_Vpsubw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG F9
		EVEX_Vpsubw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG F9

		Psubd_P_Q,											// 0FFA

		Psubd_VX_WX,										// 66 0FFA
		VEX_Vpsubd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG FA
		VEX_Vpsubd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG FA
		EVEX_Vpsubd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W0 FA
		EVEX_Vpsubd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W0 FA
		EVEX_Vpsubd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W0 FA

		Psubq_P_Q,											// 0FFB

		Psubq_VX_WX,										// 66 0FFB
		VEX_Vpsubq_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG FB
		VEX_Vpsubq_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG FB
		EVEX_Vpsubq_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W1 FB
		EVEX_Vpsubq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W1 FB
		EVEX_Vpsubq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W1 FB

		Paddb_P_Q,											// 0FFC

		Paddb_VX_WX,										// 66 0FFC
		VEX_Vpaddb_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG FC
		VEX_Vpaddb_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG FC
		EVEX_Vpaddb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG FC
		EVEX_Vpaddb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG FC
		EVEX_Vpaddb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG FC

		Paddw_P_Q,											// 0FFD

		Paddw_VX_WX,										// 66 0FFD
		VEX_Vpaddw_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG FD
		VEX_Vpaddw_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG FD
		EVEX_Vpaddw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F.WIG FD
		EVEX_Vpaddw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F.WIG FD
		EVEX_Vpaddw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F.WIG FD

		Paddd_P_Q,											// 0FFE

		Paddd_VX_WX,										// 66 0FFE
		VEX_Vpaddd_VX_HX_WX,								// VEX.NDS.128.66.0F.WIG FE
		VEX_Vpaddd_VY_HY_WY,								// VEX.NDS.256.66.0F.WIG FE
		EVEX_Vpaddd_VX_k1z_HX_WX_b,							// EVEX.NDS.128.66.0F.W0 FE
		EVEX_Vpaddd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F.W0 FE
		EVEX_Vpaddd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F.W0 FE

		Ud0_Gw_Ew,											// o16 0FFF
		Ud0_Gd_Ed,											// o32 0FFF
		Ud0_Gq_Eq,											// REX.W 0FFF

		// 0F38xx opcodes

		Pshufb_P_Q,											// 0F3800

		Pshufb_VX_WX,										// 66 0F3800
		VEX_Vpshufb_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 00
		VEX_Vpshufb_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 00
		EVEX_Vpshufb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.WIG 00
		EVEX_Vpshufb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.WIG 00
		EVEX_Vpshufb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.WIG 00

		Phaddw_P_Q,											// 0F3801

		Phaddw_VX_WX,										// 66 0F3801
		VEX_Vphaddw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 01
		VEX_Vphaddw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 01

		Phaddd_P_Q,											// 0F3802

		Phaddd_VX_WX,										// 66 0F3802
		VEX_Vphaddd_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 02
		VEX_Vphaddd_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 02

		Phaddsw_P_Q,										// 0F3803

		Phaddsw_VX_WX,										// 66 0F3803
		VEX_Vphaddsw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 03
		VEX_Vphaddsw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 03

		Pmaddubsw_P_Q,										// 0F3804

		Pmaddubsw_VX_WX,									// 66 0F3804
		VEX_Vpmaddubsw_VX_HX_WX,							// VEX.NDS.128.66.0F38.WIG 04
		VEX_Vpmaddubsw_VY_HY_WY,							// VEX.NDS.256.66.0F38.WIG 04
		EVEX_Vpmaddubsw_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F38.WIG 04
		EVEX_Vpmaddubsw_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F38.WIG 04
		EVEX_Vpmaddubsw_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F38.WIG 04

		Phsubw_P_Q,											// 0F3805

		Phsubw_VX_WX,										// 66 0F3805
		VEX_Vphsubw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 05
		VEX_Vphsubw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 05

		Phsubd_P_Q,											// 0F3806

		Phsubd_VX_WX,										// 66 0F3806
		VEX_Vphsubd_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 06
		VEX_Vphsubd_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 06

		Phsubsw_P_Q,										// 0F3807

		Phsubsw_VX_WX,										// 66 0F3807
		VEX_Vphsubsw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 07
		VEX_Vphsubsw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 07

		Psignb_P_Q,											// 0F3808

		Psignb_VX_WX,										// 66 0F3808
		VEX_Vpsignb_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 08
		VEX_Vpsignb_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 08

		Psignw_P_Q,											// 0F3809

		Psignw_VX_WX,										// 66 0F3809
		VEX_Vpsignw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 09
		VEX_Vpsignw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 09

		Psignd_P_Q,											// 0F380A

		Psignd_VX_WX,										// 66 0F380A
		VEX_Vpsignd_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 0A
		VEX_Vpsignd_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 0A

		Pmulhrsw_P_Q,										// 0F380B

		Pmulhrsw_VX_WX,										// 66 0F380B
		VEX_Vpmulhrsw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 0B
		VEX_Vpmulhrsw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F38.WIG 0B

		VEX_Vpermilps_VX_HX_WX,								// VEX.NDS.128.66.0F38.W0 0C
		VEX_Vpermilps_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 0C
		EVEX_Vpermilps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 0C
		EVEX_Vpermilps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 0C
		EVEX_Vpermilps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 0C

		VEX_Vpermilpd_VX_HX_WX,								// VEX.NDS.128.66.0F38.W0 0D
		VEX_Vpermilpd_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 0D
		EVEX_Vpermilpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 0D
		EVEX_Vpermilpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 0D
		EVEX_Vpermilpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 0D

		VEX_Vtestps_VX_WX,									// VEX.128.66.0F38.W0 0E
		VEX_Vtestps_VY_WY,									// VEX.256.66.0F38.W0 0E

		VEX_Vtestpd_VX_WX,									// VEX.128.66.0F38.W0 0F
		VEX_Vtestpd_VY_WY,									// VEX.256.66.0F38.W0 0F

		Pblendvb_VX_WX,										// 66 0F3810

		EVEX_Vpsrlvw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.W1 10
		EVEX_Vpsrlvw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.W1 10
		EVEX_Vpsrlvw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.W1 10

		EVEX_Vpmovuswb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 10
		EVEX_Vpmovuswb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 10
		EVEX_Vpmovuswb_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 10

		EVEX_Vpsravw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.W1 11
		EVEX_Vpsravw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.W1 11
		EVEX_Vpsravw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.W1 11

		EVEX_Vpmovusdb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 11
		EVEX_Vpmovusdb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 11
		EVEX_Vpmovusdb_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 11

		EVEX_Vpsllvw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.W1 12
		EVEX_Vpsllvw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.W1 12
		EVEX_Vpsllvw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.W1 12

		EVEX_Vpmovusqb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 12
		EVEX_Vpmovusqb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 12
		EVEX_Vpmovusqb_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 12

		VEX_Vcvtph2ps_VX_WX,								// VEX.128.66.0F38.W0 13
		VEX_Vcvtph2ps_VY_WX,								// VEX.256.66.0F38.W0 13
		EVEX_Vcvtph2ps_VX_k1z_WX,							// EVEX.128.66.0F38.W0 13
		EVEX_Vcvtph2ps_VY_k1z_WX,							// EVEX.256.66.0F38.W0 13
		EVEX_Vcvtph2ps_VZ_k1z_WY_sae,						// EVEX.512.66.0F38.W0 13

		EVEX_Vpmovusdw_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 13
		EVEX_Vpmovusdw_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 13
		EVEX_Vpmovusdw_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 13

		Blendvps_VX_WX,										// 66 0F3814
		EVEX_Vprorvd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 14
		EVEX_Vprorvd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 14
		EVEX_Vprorvd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 14
		EVEX_Vprorvq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 14
		EVEX_Vprorvq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 14
		EVEX_Vprorvq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 14

		EVEX_Vpmovusqw_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 14
		EVEX_Vpmovusqw_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 14
		EVEX_Vpmovusqw_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 14

		Blendvpd_VX_WX,										// 66 0F3815
		EVEX_Vprolvd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 15
		EVEX_Vprolvd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 15
		EVEX_Vprolvd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 15
		EVEX_Vprolvq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 15
		EVEX_Vprolvq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 15
		EVEX_Vprolvq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 15

		EVEX_Vpmovusqd_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 15
		EVEX_Vpmovusqd_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 15
		EVEX_Vpmovusqd_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 15

		VEX_Vpermps_VY_HY_WY,								// VEX.256.66.0F38.W0 16
		EVEX_Vpermps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 16
		EVEX_Vpermps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 16
		EVEX_Vpermpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 16
		EVEX_Vpermpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 16

		Ptest_VX_WX,										// 66 0F3817
		VEX_Vptest_VX_WX,									// VEX.128.66.0F38.WIG 17
		VEX_Vptest_VY_WY,									// VEX.256.66.0F38.WIG 17

		VEX_Vbroadcastss_VX_WX,								// VEX.128.66.0F38.W0 18
		VEX_Vbroadcastss_VY_WX,								// VEX.256.66.0F38.W0 18
		EVEX_Vbroadcastss_VX_k1z_WX,						// EVEX.128.66.0F38.W0 18
		EVEX_Vbroadcastss_VY_k1z_WX,						// EVEX.256.66.0F38.W0 18
		EVEX_Vbroadcastss_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 18

		VEX_Vbroadcastsd_VY_WX,								// VEX.256.66.0F38.W0 19
		EVEX_Vbroadcastf32x2_VY_k1z_WX,						// EVEX.256.66.0F38.W0 19
		EVEX_Vbroadcastf32x2_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 19
		EVEX_Vbroadcastsd_VY_k1z_WX,						// EVEX.256.66.0F38.W1 19
		EVEX_Vbroadcastsd_VZ_k1z_WX,						// EVEX.512.66.0F38.W1 19

		VEX_Vbroadcastf128_VY_M,							// VEX.256.66.0F38.W0 1A
		EVEX_Vbroadcastf32x4_VY_k1z_M,						// EVEX.256.66.0F38.W0 1A
		EVEX_Vbroadcastf32x4_VZ_k1z_M,						// EVEX.512.66.0F38.W0 1A
		EVEX_Vbroadcastf64x2_VY_k1z_M,						// EVEX.256.66.0F38.W1 1A
		EVEX_Vbroadcastf64x2_VZ_k1z_M,						// EVEX.512.66.0F38.W1 1A

		EVEX_Vbroadcastf32x8_VZ_k1z_M,						// EVEX.512.66.0F38.W0 1B
		EVEX_Vbroadcastf64x4_VZ_k1z_M,						// EVEX.512.66.0F38.W1 1B

		Pabsb_P_Q,											// 0F381C

		Pabsb_VX_WX,										// 66 0F381C
		VEX_Vpabsb_VX_WX,									// VEX.128.66.0F38.WIG 1C
		VEX_Vpabsb_VY_WY,									// VEX.256.66.0F38.WIG 1C
		EVEX_Vpabsb_VX_k1z_WX,								// EVEX.128.66.0F38.WIG 1C
		EVEX_Vpabsb_VY_k1z_WY,								// EVEX.256.66.0F38.WIG 1C
		EVEX_Vpabsb_VZ_k1z_WZ,								// EVEX.512.66.0F38.WIG 1C

		Pabsw_P_Q,											// 0F381D

		Pabsw_VX_WX,										// 66 0F381D
		VEX_Vpabsw_VX_WX,									// VEX.128.66.0F38.WIG 1D
		VEX_Vpabsw_VY_WY,									// VEX.256.66.0F38.WIG 1D
		EVEX_Vpabsw_VX_k1z_WX,								// EVEX.128.66.0F38.WIG 1D
		EVEX_Vpabsw_VY_k1z_WY,								// EVEX.256.66.0F38.WIG 1D
		EVEX_Vpabsw_VZ_k1z_WZ,								// EVEX.512.66.0F38.WIG 1D

		Pabsd_P_Q,											// 0F381E

		Pabsd_VX_WX,										// 66 0F381E
		VEX_Vpabsd_VX_WX,									// VEX.128.66.0F38.WIG 1E
		VEX_Vpabsd_VY_WY,									// VEX.256.66.0F38.WIG 1E
		EVEX_Vpabsd_VX_k1z_WX_b,							// EVEX.128.66.0F38.W0 1E
		EVEX_Vpabsd_VY_k1z_WY_b,							// EVEX.256.66.0F38.W0 1E
		EVEX_Vpabsd_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W0 1E

		EVEX_Vpabsq_VX_k1z_WX_b,							// EVEX.128.66.0F38.W1 1F
		EVEX_Vpabsq_VY_k1z_WY_b,							// EVEX.256.66.0F38.W1 1F
		EVEX_Vpabsq_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W1 1F

		Pmovsxbw_VX_WX,										// 66 0F3820
		VEX_Vpmovsxbw_VX_WX,								// VEX.128.66.0F38.WIG 20
		VEX_Vpmovsxbw_VY_WX,								// VEX.256.66.0F38.WIG 20
		EVEX_Vpmovsxbw_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 20
		EVEX_Vpmovsxbw_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 20
		EVEX_Vpmovsxbw_VZ_k1z_WY,							// EVEX.512.66.0F38.WIG 20

		EVEX_Vpmovswb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 20
		EVEX_Vpmovswb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 20
		EVEX_Vpmovswb_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 20

		Pmovsxbd_VX_WX,										// 66 0F3821
		VEX_Vpmovsxbd_VX_WX,								// VEX.128.66.0F38.WIG 21
		VEX_Vpmovsxbd_VY_WX,								// VEX.256.66.0F38.WIG 21
		EVEX_Vpmovsxbd_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 21
		EVEX_Vpmovsxbd_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 21
		EVEX_Vpmovsxbd_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 21

		EVEX_Vpmovsdb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 21
		EVEX_Vpmovsdb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 21
		EVEX_Vpmovsdb_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 21

		Pmovsxbq_VX_WX,										// 66 0F3822
		VEX_Vpmovsxbq_VX_WX,								// VEX.128.66.0F38.WIG 22
		VEX_Vpmovsxbq_VY_WX,								// VEX.256.66.0F38.WIG 22
		EVEX_Vpmovsxbq_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 22
		EVEX_Vpmovsxbq_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 22
		EVEX_Vpmovsxbq_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 22

		EVEX_Vpmovsqb_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 22
		EVEX_Vpmovsqb_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 22
		EVEX_Vpmovsqb_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 22

		Pmovsxwd_VX_WX,										// 66 0F3823
		VEX_Vpmovsxwd_VX_WX,								// VEX.128.66.0F38.WIG 23
		VEX_Vpmovsxwd_VY_WX,								// VEX.256.66.0F38.WIG 23
		EVEX_Vpmovsxwd_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 23
		EVEX_Vpmovsxwd_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 23
		EVEX_Vpmovsxwd_VZ_k1z_WY,							// EVEX.512.66.0F38.WIG 23

		EVEX_Vpmovsdw_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 23
		EVEX_Vpmovsdw_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 23
		EVEX_Vpmovsdw_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 23

		Pmovsxwq_VX_WX,										// 66 0F3824
		VEX_Vpmovsxwq_VX_WX,								// VEX.128.66.0F38.WIG 24
		VEX_Vpmovsxwq_VY_WX,								// VEX.256.66.0F38.WIG 24
		EVEX_Vpmovsxwq_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 24
		EVEX_Vpmovsxwq_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 24
		EVEX_Vpmovsxwq_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 24

		EVEX_Vpmovsqw_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 24
		EVEX_Vpmovsqw_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 24
		EVEX_Vpmovsqw_WX_k1z_VZ,							// EVEX.512.F3.0F38.W0 24

		Pmovsxdq_VX_WX,										// 66 0F3825
		VEX_Vpmovsxdq_VX_WX,								// VEX.128.66.0F38.WIG 25
		VEX_Vpmovsxdq_VY_WX,								// VEX.256.66.0F38.WIG 25
		EVEX_Vpmovsxdq_VX_k1z_WX,							// EVEX.128.66.0F38.W0 25
		EVEX_Vpmovsxdq_VY_k1z_WX,							// EVEX.256.66.0F38.W0 25
		EVEX_Vpmovsxdq_VZ_k1z_WY,							// EVEX.512.66.0F38.W0 25

		EVEX_Vpmovsqd_WX_k1z_VX,							// EVEX.128.F3.0F38.W0 25
		EVEX_Vpmovsqd_WX_k1z_VY,							// EVEX.256.F3.0F38.W0 25
		EVEX_Vpmovsqd_WY_k1z_VZ,							// EVEX.512.F3.0F38.W0 25

		EVEX_Vptestmb_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F38.W0 26
		EVEX_Vptestmb_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F38.W0 26
		EVEX_Vptestmb_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F38.W0 26
		EVEX_Vptestmw_VK_k1_HX_WX,							// EVEX.NDS.128.66.0F38.W1 26
		EVEX_Vptestmw_VK_k1_HY_WY,							// EVEX.NDS.256.66.0F38.W1 26
		EVEX_Vptestmw_VK_k1_HZ_WZ,							// EVEX.NDS.512.66.0F38.W1 26

		EVEX_Vptestnmb_VK_k1_HX_WX,							// EVEX.NDS.128.F3.0F38.W0 26
		EVEX_Vptestnmb_VK_k1_HY_WY,							// EVEX.NDS.256.F3.0F38.W0 26
		EVEX_Vptestnmb_VK_k1_HZ_WZ,							// EVEX.NDS.512.F3.0F38.W0 26
		EVEX_Vptestnmw_VK_k1_HX_WX,							// EVEX.NDS.128.F3.0F38.W1 26
		EVEX_Vptestnmw_VK_k1_HY_WY,							// EVEX.NDS.256.F3.0F38.W1 26
		EVEX_Vptestnmw_VK_k1_HZ_WZ,							// EVEX.NDS.512.F3.0F38.W1 26

		EVEX_Vptestmd_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 27
		EVEX_Vptestmd_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 27
		EVEX_Vptestmd_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 27
		EVEX_Vptestmq_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 27
		EVEX_Vptestmq_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 27
		EVEX_Vptestmq_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 27

		EVEX_Vptestnmd_VK_k1_HX_WX_b,						// EVEX.NDS.128.F3.0F38.W0 27
		EVEX_Vptestnmd_VK_k1_HY_WY_b,						// EVEX.NDS.256.F3.0F38.W0 27
		EVEX_Vptestnmd_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.F3.0F38.W0 27
		EVEX_Vptestnmq_VK_k1_HX_WX_b,						// EVEX.NDS.128.F3.0F38.W1 27
		EVEX_Vptestnmq_VK_k1_HY_WY_b,						// EVEX.NDS.256.F3.0F38.W1 27
		EVEX_Vptestnmq_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.F3.0F38.W1 27

		Pmuldq_VX_WX,										// 66 0F3828
		VEX_Vpmuldq_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 28
		VEX_Vpmuldq_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 28
		EVEX_Vpmuldq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 28
		EVEX_Vpmuldq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 28
		EVEX_Vpmuldq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 28

		EVEX_Vpmovm2b_VX_RK,								// EVEX.128.F3.0F38.W0 28
		EVEX_Vpmovm2b_VY_RK,								// EVEX.256.F3.0F38.W0 28
		EVEX_Vpmovm2b_VZ_RK,								// EVEX.512.F3.0F38.W0 28
		EVEX_Vpmovm2w_VX_RK,								// EVEX.128.F3.0F38.W1 28
		EVEX_Vpmovm2w_VY_RK,								// EVEX.256.F3.0F38.W1 28
		EVEX_Vpmovm2w_VZ_RK,								// EVEX.512.F3.0F38.W1 28

		Pcmpeqq_VX_WX,										// 66 0F3829
		VEX_Vpcmpeqq_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 29
		VEX_Vpcmpeqq_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 29
		EVEX_Vpcmpeqq_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 29
		EVEX_Vpcmpeqq_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 29
		EVEX_Vpcmpeqq_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 29

		EVEX_Vpmovb2m_VK_RX,								// EVEX.128.F3.0F38.W0 29
		EVEX_Vpmovb2m_VK_RY,								// EVEX.256.F3.0F38.W0 29
		EVEX_Vpmovb2m_VK_RZ,								// EVEX.512.F3.0F38.W0 29
		EVEX_Vpmovw2m_VK_RX,								// EVEX.128.F3.0F38.W1 29
		EVEX_Vpmovw2m_VK_RY,								// EVEX.256.F3.0F38.W1 29
		EVEX_Vpmovw2m_VK_RZ,								// EVEX.512.F3.0F38.W1 29

		Movntdqa_VX_M,										// 66 0F382A
		VEX_Vmovntdqa_VX_M,									// VEX.128.66.0F38.WIG 2A
		VEX_Vmovntdqa_VY_M,									// VEX.256.66.0F38.WIG 2A
		EVEX_Vmovntdqa_VX_M,								// EVEX.128.66.0F38.W0 2A
		EVEX_Vmovntdqa_VY_M,								// EVEX.256.66.0F38.W0 2A
		EVEX_Vmovntdqa_VZ_M,								// EVEX.512.66.0F38.W0 2A

		EVEX_Vpbroadcastmb2q_VX_RK,							// EVEX.128.F3.0F38.W1 2A
		EVEX_Vpbroadcastmb2q_VY_RK,							// EVEX.256.F3.0F38.W1 2A
		EVEX_Vpbroadcastmb2q_VZ_RK,							// EVEX.512.F3.0F38.W1 2A

		Packusdw_VX_WX,										// 66 0F382B
		VEX_Vpackusdw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 2B
		VEX_Vpackusdw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 2B
		EVEX_Vpackusdw_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 2B
		EVEX_Vpackusdw_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 2B
		EVEX_Vpackusdw_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 2B

		VEX_Vmaskmovps_VX_HX_M,								// VEX.NDS.128.66.0F38.W0 2C
		VEX_Vmaskmovps_VY_HY_M,								// VEX.NDS.256.66.0F38.W0 2C
		EVEX_Vscalefps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 2C
		EVEX_Vscalefps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 2C
		EVEX_Vscalefps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 2C
		EVEX_Vscalefpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 2C
		EVEX_Vscalefpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 2C
		EVEX_Vscalefpd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 2C

		VEX_Vmaskmovpd_VX_HX_M,								// VEX.NDS.128.66.0F38.W0 2D
		VEX_Vmaskmovpd_VY_HY_M,								// VEX.NDS.256.66.0F38.W0 2D
		EVEX_Vscalefss_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.66.0F38.W0 2D
		EVEX_Vscalefsd_VX_k1z_HX_WX_er,						// EVEX.NDS.LIG.66.0F38.W1 2D

		VEX_Vmaskmovps_M_HX_VX,								// VEX.NDS.128.66.0F38.W0 2E
		VEX_Vmaskmovps_M_HY_VY,								// VEX.NDS.256.66.0F38.W0 2E

		VEX_Vmaskmovpd_M_HX_VX,								// VEX.NDS.128.66.0F38.W0 2F
		VEX_Vmaskmovpd_M_HY_VY,								// VEX.NDS.256.66.0F38.W0 2F

		Pmovzxbw_VX_WX,										// 66 0F3830
		VEX_Vpmovzxbw_VX_WX,								// VEX.128.66.0F38.WIG 30
		VEX_Vpmovzxbw_VY_WX,								// VEX.256.66.0F38.WIG 30
		EVEX_Vpmovzxbw_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 30
		EVEX_Vpmovzxbw_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 30
		EVEX_Vpmovzxbw_VZ_k1z_WY,							// EVEX.512.66.0F38.WIG 30

		EVEX_Vpmovwb_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 30
		EVEX_Vpmovwb_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 30
		EVEX_Vpmovwb_WY_k1z_VZ,								// EVEX.512.F3.0F38.W0 30

		Pmovzxbd_VX_WX,										// 66 0F3831
		VEX_Vpmovzxbd_VX_WX,								// VEX.128.66.0F38.WIG 31
		VEX_Vpmovzxbd_VY_WX,								// VEX.256.66.0F38.WIG 31
		EVEX_Vpmovzxbd_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 31
		EVEX_Vpmovzxbd_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 31
		EVEX_Vpmovzxbd_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 31

		EVEX_Vpmovdb_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 31
		EVEX_Vpmovdb_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 31
		EVEX_Vpmovdb_WX_k1z_VZ,								// EVEX.512.F3.0F38.W0 31

		Pmovzxbq_VX_WX,										// 66 0F3832
		VEX_Vpmovzxbq_VX_WX,								// VEX.128.66.0F38.WIG 32
		VEX_Vpmovzxbq_VY_WX,								// VEX.256.66.0F38.WIG 32
		EVEX_Vpmovzxbq_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 32
		EVEX_Vpmovzxbq_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 32
		EVEX_Vpmovzxbq_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 32

		EVEX_Vpmovqb_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 32
		EVEX_Vpmovqb_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 32
		EVEX_Vpmovqb_WX_k1z_VZ,								// EVEX.512.F3.0F38.W0 32

		Pmovzxwd_VX_WX,										// 66 0F3833
		VEX_Vpmovzxwd_VX_WX,								// VEX.128.66.0F38.WIG 33
		VEX_Vpmovzxwd_VY_WX,								// VEX.256.66.0F38.WIG 33
		EVEX_Vpmovzxwd_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 33
		EVEX_Vpmovzxwd_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 33
		EVEX_Vpmovzxwd_VZ_k1z_WY,							// EVEX.512.66.0F38.WIG 33

		EVEX_Vpmovdw_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 33
		EVEX_Vpmovdw_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 33
		EVEX_Vpmovdw_WY_k1z_VZ,								// EVEX.512.F3.0F38.W0 33

		Pmovzxwq_VX_WX,										// 66 0F3834
		VEX_Vpmovzxwq_VX_WX,								// VEX.128.66.0F38.WIG 34
		VEX_Vpmovzxwq_VY_WX,								// VEX.256.66.0F38.WIG 34
		EVEX_Vpmovzxwq_VX_k1z_WX,							// EVEX.128.66.0F38.WIG 34
		EVEX_Vpmovzxwq_VY_k1z_WX,							// EVEX.256.66.0F38.WIG 34
		EVEX_Vpmovzxwq_VZ_k1z_WX,							// EVEX.512.66.0F38.WIG 34

		EVEX_Vpmovqw_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 34
		EVEX_Vpmovqw_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 34
		EVEX_Vpmovqw_WX_k1z_VZ,								// EVEX.512.F3.0F38.W0 34

		Pmovzxdq_VX_WX,										// 66 0F3835
		VEX_Vpmovzxdq_VX_WX,								// VEX.128.66.0F38.WIG 35
		VEX_Vpmovzxdq_VY_WX,								// VEX.256.66.0F38.WIG 35
		EVEX_Vpmovzxdq_VX_k1z_WX,							// EVEX.128.66.0F38.W0 35
		EVEX_Vpmovzxdq_VY_k1z_WX,							// EVEX.256.66.0F38.W0 35
		EVEX_Vpmovzxdq_VZ_k1z_WY,							// EVEX.512.66.0F38.W0 35

		EVEX_Vpmovqd_WX_k1z_VX,								// EVEX.128.F3.0F38.W0 35
		EVEX_Vpmovqd_WX_k1z_VY,								// EVEX.256.F3.0F38.W0 35
		EVEX_Vpmovqd_WY_k1z_VZ,								// EVEX.512.F3.0F38.W0 35

		VEX_Vpermd_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 36
		EVEX_Vpermd_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F38.W0 36
		EVEX_Vpermd_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F38.W0 36
		EVEX_Vpermq_VY_k1z_HY_WY_b,							// EVEX.NDS.256.66.0F38.W1 36
		EVEX_Vpermq_VZ_k1z_HZ_WZ_b,							// EVEX.NDS.512.66.0F38.W1 36

		Pcmpgtq_VX_WX,										// 66 0F3837
		VEX_Vpcmpgtq_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 37
		VEX_Vpcmpgtq_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 37
		EVEX_Vpcmpgtq_VK_k1_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 37
		EVEX_Vpcmpgtq_VK_k1_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 37
		EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 37

		Pminsb_VX_WX,										// 66 0F3838
		VEX_Vpminsb_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 38
		VEX_Vpminsb_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 38
		EVEX_Vpminsb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.WIG 38
		EVEX_Vpminsb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.WIG 38
		EVEX_Vpminsb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.WIG 38

		EVEX_Vpmovm2d_VX_RK,								// EVEX.128.F3.0F38.W0 38
		EVEX_Vpmovm2d_VY_RK,								// EVEX.256.F3.0F38.W0 38
		EVEX_Vpmovm2d_VZ_RK,								// EVEX.512.F3.0F38.W0 38
		EVEX_Vpmovm2q_VX_RK,								// EVEX.128.F3.0F38.W1 38
		EVEX_Vpmovm2q_VY_RK,								// EVEX.256.F3.0F38.W1 38
		EVEX_Vpmovm2q_VZ_RK,								// EVEX.512.F3.0F38.W1 38

		Pminsd_VX_WX,										// 66 0F3839
		VEX_Vpminsd_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 39
		VEX_Vpminsd_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 39
		EVEX_Vpminsd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 39
		EVEX_Vpminsd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 39
		EVEX_Vpminsd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 39
		EVEX_Vpminsq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 39
		EVEX_Vpminsq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 39
		EVEX_Vpminsq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 39

		EVEX_Vpmovd2m_VK_RX,								// EVEX.128.F3.0F38.W0 39
		EVEX_Vpmovd2m_VK_RY,								// EVEX.256.F3.0F38.W0 39
		EVEX_Vpmovd2m_VK_RZ,								// EVEX.512.F3.0F38.W0 39
		EVEX_Vpmovq2m_VK_RX,								// EVEX.128.F3.0F38.W1 39
		EVEX_Vpmovq2m_VK_RY,								// EVEX.256.F3.0F38.W1 39
		EVEX_Vpmovq2m_VK_RZ,								// EVEX.512.F3.0F38.W1 39

		Pminuw_VX_WX,										// 66 0F383A
		VEX_Vpminuw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3A
		VEX_Vpminuw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3A
		EVEX_Vpminuw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.WIG 3A
		EVEX_Vpminuw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.WIG 3A
		EVEX_Vpminuw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.WIG 3A

		EVEX_Vpbroadcastmw2d_VX_RK,							// EVEX.128.F3.0F38.W0 3A
		EVEX_Vpbroadcastmw2d_VY_RK,							// EVEX.256.F3.0F38.W0 3A
		EVEX_Vpbroadcastmw2d_VZ_RK,							// EVEX.512.F3.0F38.W0 3A

		Pminud_VX_WX,										// 66 0F383B
		VEX_Vpminud_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3B
		VEX_Vpminud_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3B
		EVEX_Vpminud_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 3B
		EVEX_Vpminud_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 3B
		EVEX_Vpminud_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 3B
		EVEX_Vpminuq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 3B
		EVEX_Vpminuq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 3B
		EVEX_Vpminuq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 3B

		Pmaxsb_VX_WX,										// 66 0F383C
		VEX_Vpmaxsb_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3C
		VEX_Vpmaxsb_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3C
		EVEX_Vpmaxsb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.WIG 3C
		EVEX_Vpmaxsb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.WIG 3C
		EVEX_Vpmaxsb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.WIG 3C

		Pmaxsd_VX_WX,										// 66 0F383D
		VEX_Vpmaxsd_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3D
		VEX_Vpmaxsd_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3D
		EVEX_Vpmaxsd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 3D
		EVEX_Vpmaxsd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 3D
		EVEX_Vpmaxsd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 3D
		EVEX_Vpmaxsq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 3D
		EVEX_Vpmaxsq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 3D
		EVEX_Vpmaxsq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 3D

		Pmaxuw_VX_WX,										// 66 0F383E
		VEX_Vpmaxuw_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3E
		VEX_Vpmaxuw_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3E
		EVEX_Vpmaxuw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.WIG 3E
		EVEX_Vpmaxuw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.WIG 3E
		EVEX_Vpmaxuw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.WIG 3E

		Pmaxud_VX_WX,										// 66 0F383F
		VEX_Vpmaxud_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 3F
		VEX_Vpmaxud_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 3F
		EVEX_Vpmaxud_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 3F
		EVEX_Vpmaxud_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 3F
		EVEX_Vpmaxud_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 3F
		EVEX_Vpmaxuq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 3F
		EVEX_Vpmaxuq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 3F
		EVEX_Vpmaxuq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 3F

		Pmulld_VX_WX,										// 66 0F3840
		VEX_Vpmulld_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG 40
		VEX_Vpmulld_VY_HY_WY,								// VEX.NDS.256.66.0F38.WIG 40
		EVEX_Vpmulld_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 40
		EVEX_Vpmulld_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 40
		EVEX_Vpmulld_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 40
		EVEX_Vpmullq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 40
		EVEX_Vpmullq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 40
		EVEX_Vpmullq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 40

		Phminposuw_VX_WX,									// 66 0F3841
		VEX_Vphminposuw_VX_WX,								// VEX.128.66.0F38.WIG 41

		EVEX_Vgetexpps_VX_k1z_WX_b,							// EVEX.128.66.0F38.W0 42
		EVEX_Vgetexpps_VY_k1z_WY_b,							// EVEX.256.66.0F38.W0 42
		EVEX_Vgetexpps_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W0 42
		EVEX_Vgetexppd_VX_k1z_WX_b,							// EVEX.128.66.0F38.W1 42
		EVEX_Vgetexppd_VY_k1z_WY_b,							// EVEX.256.66.0F38.W1 42
		EVEX_Vgetexppd_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W1 42

		EVEX_Vgetexpss_VX_k1z_HX_WX_sae,					// EVEX.NDS.LIG.66.0F38.W0 43
		EVEX_Vgetexpsd_VX_k1z_HX_WX_sae,					// EVEX.NDS.LIG.66.0F38.W1 43

		EVEX_Vplzcntd_VX_k1z_WX_b,							// EVEX.128.66.0F38.W0 44
		EVEX_Vplzcntd_VY_k1z_WY_b,							// EVEX.256.66.0F38.W0 44
		EVEX_Vplzcntd_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W0 44
		EVEX_Vplzcntq_VX_k1z_WX_b,							// EVEX.128.66.0F38.W1 44
		EVEX_Vplzcntq_VY_k1z_WY_b,							// EVEX.256.66.0F38.W1 44
		EVEX_Vplzcntq_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W1 44

		VEX_Vpsrlvd_VX_HX_WX,								// VEX.NDS.128.66.0F38.W0 45
		VEX_Vpsrlvd_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 45
		VEX_Vpsrlvq_VX_HX_WX,								// VEX.NDS.128.66.0F38.W1 45
		VEX_Vpsrlvq_VY_HY_WY,								// VEX.NDS.256.66.0F38.W1 45
		EVEX_Vpsrlvd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 45
		EVEX_Vpsrlvd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 45
		EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 45
		EVEX_Vpsrlvq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 45
		EVEX_Vpsrlvq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 45
		EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 45

		VEX_Vpsravd_VX_HX_WX,								// VEX.NDS.128.66.0F38.W0 46
		VEX_Vpsravd_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 46
		EVEX_Vpsravd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 46
		EVEX_Vpsravd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 46
		EVEX_Vpsravd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 46
		EVEX_Vpsravq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 46
		EVEX_Vpsravq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 46
		EVEX_Vpsravq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 46

		VEX_Vpsllvd_VX_HX_WX,								// VEX.NDS.128.66.0F38.W0 47
		VEX_Vpsllvd_VY_HY_WY,								// VEX.NDS.256.66.0F38.W0 47
		VEX_Vpsllvq_VX_HX_WX,								// VEX.NDS.128.66.0F38.W1 47
		VEX_Vpsllvq_VY_HY_WY,								// VEX.NDS.256.66.0F38.W1 47
		EVEX_Vpsllvd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 47
		EVEX_Vpsllvd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 47
		EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 47
		EVEX_Vpsllvq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 47
		EVEX_Vpsllvq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 47
		EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 47

		EVEX_Vrcp14ps_VX_k1z_WX_b,							// EVEX.128.66.0F38.W0 4C
		EVEX_Vrcp14ps_VY_k1z_WY_b,							// EVEX.256.66.0F38.W0 4C
		EVEX_Vrcp14ps_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W0 4C
		EVEX_Vrcp14pd_VX_k1z_WX_b,							// EVEX.128.66.0F38.W1 4C
		EVEX_Vrcp14pd_VY_k1z_WY_b,							// EVEX.256.66.0F38.W1 4C
		EVEX_Vrcp14pd_VZ_k1z_WZ_b,							// EVEX.512.66.0F38.W1 4C

		EVEX_Vrcp14ss_VX_k1z_HX_WX,							// EVEX.NDS.LIG.66.0F38.W0 4D
		EVEX_Vrcp14sd_VX_k1z_HX_WX,							// EVEX.NDS.LIG.66.0F38.W1 4D

		EVEX_Vrsqrt14ps_VX_k1z_WX_b,						// EVEX.128.66.0F38.W0 4E
		EVEX_Vrsqrt14ps_VY_k1z_WY_b,						// EVEX.256.66.0F38.W0 4E
		EVEX_Vrsqrt14ps_VZ_k1z_WZ_b,						// EVEX.512.66.0F38.W0 4E
		EVEX_Vrsqrt14pd_VX_k1z_WX_b,						// EVEX.128.66.0F38.W1 4E
		EVEX_Vrsqrt14pd_VY_k1z_WY_b,						// EVEX.256.66.0F38.W1 4E
		EVEX_Vrsqrt14pd_VZ_k1z_WZ_b,						// EVEX.512.66.0F38.W1 4E

		EVEX_Vrsqrt14ss_VX_k1z_HX_WX,						// EVEX.NDS.LIG.66.0F38.W0 4F
		EVEX_Vrsqrt14sd_VX_k1z_HX_WX,						// EVEX.NDS.LIG.66.0F38.W1 4F

		EVEX_Vp4dpwssd_VZ_k1z_HZP3_M,						// EVEX.DDS.512.F2.0F38.W0 52

		EVEX_Vp4dpwssds_VZ_k1z_HZP3_M,						// EVEX.DDS.512.F2.0F38.W0 53

		VEX_Vpbroadcastd_VX_WX,								// VEX.128.66.0F38.W0 58
		VEX_Vpbroadcastd_VY_WX,								// VEX.256.66.0F38.W0 58
		EVEX_Vpbroadcastd_VX_k1z_WX,						// EVEX.128.66.0F38.W0 58
		EVEX_Vpbroadcastd_VY_k1z_WX,						// EVEX.256.66.0F38.W0 58
		EVEX_Vpbroadcastd_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 58

		VEX_Vpbroadcastq_VX_WX,								// VEX.128.66.0F38.W0 59
		VEX_Vpbroadcastq_VY_WX,								// VEX.256.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_VX_k1z_WX,						// EVEX.128.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_VY_k1z_WX,						// EVEX.256.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 59
		EVEX_Vpbroadcastq_VX_k1z_WX,						// EVEX.128.66.0F38.W1 59
		EVEX_Vpbroadcastq_VY_k1z_WX,						// EVEX.256.66.0F38.W1 59
		EVEX_Vpbroadcastq_VZ_k1z_WX,						// EVEX.512.66.0F38.W1 59

		VEX_Vbroadcasti128_VY_M,							// VEX.256.66.0F38.W0 5A
		EVEX_Vbroadcasti32x4_VY_k1z_M,						// EVEX.256.66.0F38.W0 5A
		EVEX_Vbroadcasti32x4_VZ_k1z_M,						// EVEX.512.66.0F38.W0 5A
		EVEX_Vbroadcasti64x2_VY_k1z_M,						// EVEX.256.66.0F38.W1 5A
		EVEX_Vbroadcasti64x2_VZ_k1z_M,						// EVEX.512.66.0F38.W1 5A

		EVEX_Vbroadcasti32x8_VZ_k1z_M,						// EVEX.512.66.0F38.W0 5B
		EVEX_Vbroadcasti64x4_VZ_k1z_M,						// EVEX.512.66.0F38.W1 5B

		EVEX_Vpblendmd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 64
		EVEX_Vpblendmd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 64
		EVEX_Vpblendmd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 64
		EVEX_Vpblendmq_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 64
		EVEX_Vpblendmq_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 64
		EVEX_Vpblendmq_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 64

		EVEX_Vblendmps_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W0 65
		EVEX_Vblendmps_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W0 65
		EVEX_Vblendmps_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W0 65
		EVEX_Vblendmpd_VX_k1z_HX_WX_b,						// EVEX.NDS.128.66.0F38.W1 65
		EVEX_Vblendmpd_VY_k1z_HY_WY_b,						// EVEX.NDS.256.66.0F38.W1 65
		EVEX_Vblendmpd_VZ_k1z_HZ_WZ_b,						// EVEX.NDS.512.66.0F38.W1 65

		EVEX_Vpblendmb_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F38.W0 66
		EVEX_Vpblendmb_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F38.W0 66
		EVEX_Vpblendmb_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F38.W0 66
		EVEX_Vpblendmw_VX_k1z_HX_WX,						// EVEX.NDS.128.66.0F38.W1 66
		EVEX_Vpblendmw_VY_k1z_HY_WY,						// EVEX.NDS.256.66.0F38.W1 66
		EVEX_Vpblendmw_VZ_k1z_HZ_WZ,						// EVEX.NDS.512.66.0F38.W1 66

		EVEX_Vpermi2b_VX_k1z_HX_WX,							// EVEX.DDS.128.66.0F38.W0 75
		EVEX_Vpermi2b_VY_k1z_HY_WY,							// EVEX.DDS.256.66.0F38.W0 75
		EVEX_Vpermi2b_VZ_k1z_HZ_WZ,							// EVEX.DDS.512.66.0F38.W0 75
		EVEX_Vpermi2w_VX_k1z_HX_WX,							// EVEX.DDS.128.66.0F38.W1 75
		EVEX_Vpermi2w_VY_k1z_HY_WY,							// EVEX.DDS.256.66.0F38.W1 75
		EVEX_Vpermi2w_VZ_k1z_HZ_WZ,							// EVEX.DDS.512.66.0F38.W1 75

		EVEX_Vpermi2d_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W0 76
		EVEX_Vpermi2d_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W0 76
		EVEX_Vpermi2d_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W0 76
		EVEX_Vpermi2q_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W1 76
		EVEX_Vpermi2q_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W1 76
		EVEX_Vpermi2q_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W1 76

		EVEX_Vpermi2ps_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W0 77
		EVEX_Vpermi2ps_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W0 77
		EVEX_Vpermi2ps_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W0 77
		EVEX_Vpermi2pd_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W1 77
		EVEX_Vpermi2pd_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W1 77
		EVEX_Vpermi2pd_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W1 77

		VEX_Vpbroadcastb_VX_WX,								// VEX.128.66.0F38.W0 78
		VEX_Vpbroadcastb_VY_WX,								// VEX.256.66.0F38.W0 78
		EVEX_Vpbroadcastb_VX_k1z_WX,						// EVEX.128.66.0F38.W0 78
		EVEX_Vpbroadcastb_VY_k1z_WX,						// EVEX.256.66.0F38.W0 78
		EVEX_Vpbroadcastb_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 78

		VEX_Vpbroadcastw_VX_WX,								// VEX.128.66.0F38.W0 79
		VEX_Vpbroadcastw_VY_WX,								// VEX.256.66.0F38.W0 79
		EVEX_Vpbroadcastw_VX_k1z_WX,						// EVEX.128.66.0F38.W0 79
		EVEX_Vpbroadcastw_VY_k1z_WX,						// EVEX.256.66.0F38.W0 79
		EVEX_Vpbroadcastw_VZ_k1z_WX,						// EVEX.512.66.0F38.W0 79

		EVEX_Vpbroadcastb_VX_k1z_Rd,						// EVEX.128.66.0F38.W0 7A
		EVEX_Vpbroadcastb_VY_k1z_Rd,						// EVEX.256.66.0F38.W0 7A
		EVEX_Vpbroadcastb_VZ_k1z_Rd,						// EVEX.512.66.0F38.W0 7A

		EVEX_Vpbroadcastw_VX_k1z_Rd,						// EVEX.128.66.0F38.W0 7B
		EVEX_Vpbroadcastw_VY_k1z_Rd,						// EVEX.256.66.0F38.W0 7B
		EVEX_Vpbroadcastw_VZ_k1z_Rd,						// EVEX.512.66.0F38.W0 7B

		EVEX_Vpbroadcastd_VX_k1z_Rd,						// EVEX.128.66.0F38.W0 7C
		EVEX_Vpbroadcastd_VY_k1z_Rd,						// EVEX.256.66.0F38.W0 7C
		EVEX_Vpbroadcastd_VZ_k1z_Rd,						// EVEX.512.66.0F38.W0 7C
		EVEX_Vpbroadcastq_VX_k1z_Rq,						// EVEX.128.66.0F38.W1 7C
		EVEX_Vpbroadcastq_VY_k1z_Rq,						// EVEX.256.66.0F38.W1 7C
		EVEX_Vpbroadcastq_VZ_k1z_Rq,						// EVEX.512.66.0F38.W1 7C

		EVEX_Vpermt2b_VX_k1z_HX_WX,							// EVEX.DDS.128.66.0F38.W0 7D
		EVEX_Vpermt2b_VY_k1z_HY_WY,							// EVEX.DDS.256.66.0F38.W0 7D
		EVEX_Vpermt2b_VZ_k1z_HZ_WZ,							// EVEX.DDS.512.66.0F38.W0 7D
		EVEX_Vpermt2w_VX_k1z_HX_WX,							// EVEX.DDS.128.66.0F38.W1 7D
		EVEX_Vpermt2w_VY_k1z_HY_WY,							// EVEX.DDS.256.66.0F38.W1 7D
		EVEX_Vpermt2w_VZ_k1z_HZ_WZ,							// EVEX.DDS.512.66.0F38.W1 7D

		EVEX_Vpermt2d_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W0 7E
		EVEX_Vpermt2d_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W0 7E
		EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W0 7E
		EVEX_Vpermt2q_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W1 7E
		EVEX_Vpermt2q_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W1 7E
		EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W1 7E

		EVEX_Vpermt2ps_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W0 7F
		EVEX_Vpermt2ps_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W0 7F
		EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W0 7F
		EVEX_Vpermt2pd_VX_k1z_HX_WX_b,						// EVEX.DDS.128.66.0F38.W1 7F
		EVEX_Vpermt2pd_VY_k1z_HY_WY_b,						// EVEX.DDS.256.66.0F38.W1 7F
		EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b,						// EVEX.DDS.512.66.0F38.W1 7F

		Invept_Gd_M,										// 66 0F3880
		Invept_Gq_M,										// 66 0F3880

		Invvpid_Gd_M,										// 66 0F3881
		Invvpid_Gq_M,										// 66 0F3881

		Invpcid_Gd_M,										// 66 0F3882
		Invpcid_Gq_M,										// 66 0F3882

		EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 83
		EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 83
		EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b,					// EVEX.NDS.512.66.0F38.W1 83

		EVEX_Vexpandps_VX_k1z_WX,							// EVEX.128.66.0F38.W0 88
		EVEX_Vexpandps_VY_k1z_WY,							// EVEX.256.66.0F38.W0 88
		EVEX_Vexpandps_VZ_k1z_WZ,							// EVEX.512.66.0F38.W0 88
		EVEX_Vexpandpd_VX_k1z_WX,							// EVEX.128.66.0F38.W1 88
		EVEX_Vexpandpd_VY_k1z_WY,							// EVEX.256.66.0F38.W1 88
		EVEX_Vexpandpd_VZ_k1z_WZ,							// EVEX.512.66.0F38.W1 88

		EVEX_Vpexpandd_VX_k1z_WX,							// EVEX.128.66.0F38.W0 89
		EVEX_Vpexpandd_VY_k1z_WY,							// EVEX.256.66.0F38.W0 89
		EVEX_Vpexpandd_VZ_k1z_WZ,							// EVEX.512.66.0F38.W0 89
		EVEX_Vpexpandq_VX_k1z_WX,							// EVEX.128.66.0F38.W1 89
		EVEX_Vpexpandq_VY_k1z_WY,							// EVEX.256.66.0F38.W1 89
		EVEX_Vpexpandq_VZ_k1z_WZ,							// EVEX.512.66.0F38.W1 89

		EVEX_Vcompressps_WX_k1z_VX,							// EVEX.128.66.0F38.W0 8A
		EVEX_Vcompressps_WY_k1z_VY,							// EVEX.256.66.0F38.W0 8A
		EVEX_Vcompressps_WZ_k1z_VZ,							// EVEX.512.66.0F38.W0 8A
		EVEX_Vcompresspd_WX_k1z_VX,							// EVEX.128.66.0F38.W1 8A
		EVEX_Vcompresspd_WY_k1z_VY,							// EVEX.256.66.0F38.W1 8A
		EVEX_Vcompresspd_WZ_k1z_VZ,							// EVEX.512.66.0F38.W1 8A

		EVEX_Vpcompressd_WX_k1z_VX,							// EVEX.128.66.0F38.W0 8B
		EVEX_Vpcompressd_WY_k1z_VY,							// EVEX.256.66.0F38.W0 8B
		EVEX_Vpcompressd_WZ_k1z_VZ,							// EVEX.512.66.0F38.W0 8B
		EVEX_Vpcompressq_WX_k1z_VX,							// EVEX.128.66.0F38.W1 8B
		EVEX_Vpcompressq_WY_k1z_VY,							// EVEX.256.66.0F38.W1 8B
		EVEX_Vpcompressq_WZ_k1z_VZ,							// EVEX.512.66.0F38.W1 8B

		VEX_Vpmaskmovd_VX_HX_M,								// VEX.NDS.128.66.0F38.W0 8C
		VEX_Vpmaskmovd_VY_HY_M,								// VEX.NDS.256.66.0F38.W0 8C
		VEX_Vpmaskmovq_VX_HX_M,								// VEX.NDS.128.66.0F38.W1 8C
		VEX_Vpmaskmovq_VY_HY_M,								// VEX.NDS.256.66.0F38.W1 8C

		EVEX_Vpermb_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.W0 8D
		EVEX_Vpermb_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.W0 8D
		EVEX_Vpermb_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.W0 8D
		EVEX_Vpermw_VX_k1z_HX_WX,							// EVEX.NDS.128.66.0F38.W1 8D
		EVEX_Vpermw_VY_k1z_HY_WY,							// EVEX.NDS.256.66.0F38.W1 8D
		EVEX_Vpermw_VZ_k1z_HZ_WZ,							// EVEX.NDS.512.66.0F38.W1 8D

		VEX_Vpmaskmovd_M_HX_VX,								// VEX.NDS.128.66.0F38.W0 8E
		VEX_Vpmaskmovd_M_HY_VY,								// VEX.NDS.256.66.0F38.W0 8E
		VEX_Vpmaskmovq_M_HX_VX,								// VEX.NDS.128.66.0F38.W1 8E
		VEX_Vpmaskmovq_M_HY_VY,								// VEX.NDS.256.66.0F38.W1 8E

		VEX_Vpgatherdd_VX_VM32X_HX,							// VEX.DDS.128.66.0F38.W0 90
		VEX_Vpgatherdd_VY_VM32Y_HY,							// VEX.DDS.256.66.0F38.W0 90
		VEX_Vpgatherdq_VX_VM32X_HX,							// VEX.DDS.128.66.0F38.W1 90
		VEX_Vpgatherdq_VY_VM32X_HY,							// VEX.DDS.256.66.0F38.W1 90
		EVEX_Vpgatherdd_VX_k1_VM32X,						// EVEX.128.66.0F38.W0 90
		EVEX_Vpgatherdd_VY_k1_VM32Y,						// EVEX.256.66.0F38.W0 90
		EVEX_Vpgatherdd_VZ_k1_VM32Z,						// EVEX.512.66.0F38.W0 90
		EVEX_Vpgatherdq_VX_k1_VM32X,						// EVEX.128.66.0F38.W1 90
		EVEX_Vpgatherdq_VY_k1_VM32X,						// EVEX.256.66.0F38.W1 90
		EVEX_Vpgatherdq_VZ_k1_VM32Y,						// EVEX.512.66.0F38.W1 90

		VEX_Vpgatherqd_VX_VM64X_HX,							// VEX.DDS.128.66.0F38.W0 91
		VEX_Vpgatherqd_VX_VM64Y_HX,							// VEX.DDS.256.66.0F38.W0 91
		VEX_Vpgatherqq_VX_VM64X_HX,							// VEX.DDS.128.66.0F38.W1 91
		VEX_Vpgatherqq_VY_VM64Y_HY,							// VEX.DDS.256.66.0F38.W1 91
		EVEX_Vpgatherqd_VX_k1_VM64X,						// EVEX.128.66.0F38.W0 91
		EVEX_Vpgatherqd_VX_k1_VM64Y,						// EVEX.256.66.0F38.W0 91
		EVEX_Vpgatherqd_VY_k1_VM64Z,						// EVEX.512.66.0F38.W0 91
		EVEX_Vpgatherqq_VX_k1_VM64X,						// EVEX.128.66.0F38.W1 91
		EVEX_Vpgatherqq_VY_k1_VM64Y,						// EVEX.256.66.0F38.W1 91
		EVEX_Vpgatherqq_VZ_k1_VM64Z,						// EVEX.512.66.0F38.W1 91

		VEX_Vgatherdps_VX_VM32X_HX,							// VEX.DDS.128.66.0F38.W0 92
		VEX_Vgatherdps_VY_VM32Y_HY,							// VEX.DDS.256.66.0F38.W0 92
		VEX_Vgatherdpd_VX_VM32X_HX,							// VEX.DDS.128.66.0F38.W1 92
		VEX_Vgatherdpd_VY_VM32X_HY,							// VEX.DDS.256.66.0F38.W1 92
		EVEX_Vgatherdps_VX_k1_VM32X,						// EVEX.128.66.0F38.W0 92
		EVEX_Vgatherdps_VY_k1_VM32Y,						// EVEX.256.66.0F38.W0 92
		EVEX_Vgatherdps_VZ_k1_VM32Z,						// EVEX.512.66.0F38.W0 92
		EVEX_Vgatherdpd_VX_k1_VM32X,						// EVEX.128.66.0F38.W1 92
		EVEX_Vgatherdpd_VY_k1_VM32X,						// EVEX.256.66.0F38.W1 92
		EVEX_Vgatherdpd_VZ_k1_VM32Y,						// EVEX.512.66.0F38.W1 92

		VEX_Vgatherqps_VX_VM64X_HX,							// VEX.DDS.128.66.0F38.W0 93
		VEX_Vgatherqps_VX_VM64Y_HX,							// VEX.DDS.256.66.0F38.W0 93
		VEX_Vgatherqpd_VX_VM64X_HX,							// VEX.DDS.128.66.0F38.W1 93
		VEX_Vgatherqpd_VY_VM64Y_HY,							// VEX.DDS.256.66.0F38.W1 93
		EVEX_Vgatherqps_VX_k1_VM64X,						// EVEX.128.66.0F38.W0 93
		EVEX_Vgatherqps_VX_k1_VM64Y,						// EVEX.256.66.0F38.W0 93
		EVEX_Vgatherqps_VY_k1_VM64Z,						// EVEX.512.66.0F38.W0 93
		EVEX_Vgatherqpd_VX_k1_VM64X,						// EVEX.128.66.0F38.W1 93
		EVEX_Vgatherqpd_VY_k1_VM64Y,						// EVEX.256.66.0F38.W1 93
		EVEX_Vgatherqpd_VZ_k1_VM64Z,						// EVEX.512.66.0F38.W1 93

		VEX_Vfmaddsub132ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 96
		VEX_Vfmaddsub132ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 96
		VEX_Vfmaddsub132pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 96
		VEX_Vfmaddsub132pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 96
		EVEX_Vfmaddsub132ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 96
		EVEX_Vfmaddsub132ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 96
		EVEX_Vfmaddsub132ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 96
		EVEX_Vfmaddsub132pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 96
		EVEX_Vfmaddsub132pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 96
		EVEX_Vfmaddsub132pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 96

		VEX_Vfmsubadd132ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 97
		VEX_Vfmsubadd132ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 97
		VEX_Vfmsubadd132pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 97
		VEX_Vfmsubadd132pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 97
		EVEX_Vfmsubadd132ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 97
		EVEX_Vfmsubadd132ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 97
		EVEX_Vfmsubadd132ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 97
		EVEX_Vfmsubadd132pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 97
		EVEX_Vfmsubadd132pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 97
		EVEX_Vfmsubadd132pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 97

		VEX_Vfmadd132ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 98
		VEX_Vfmadd132ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 98
		VEX_Vfmadd132pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 98
		VEX_Vfmadd132pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 98
		EVEX_Vfmadd132ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 98
		EVEX_Vfmadd132ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 98
		EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 98
		EVEX_Vfmadd132pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 98
		EVEX_Vfmadd132pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 98
		EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 98

		VEX_Vfmadd132ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 99
		VEX_Vfmadd132sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 99
		EVEX_Vfmadd132ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 99
		EVEX_Vfmadd132sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 99

		VEX_Vfmsub132ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 9A
		VEX_Vfmsub132ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 9A
		VEX_Vfmsub132pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 9A
		VEX_Vfmsub132pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 9A
		EVEX_Vfmsub132ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 9A
		EVEX_Vfmsub132ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 9A
		EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 9A
		EVEX_Vfmsub132pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 9A
		EVEX_Vfmsub132pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 9A
		EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 9A

		EVEX_V4fmaddps_VZ_k1z_HZP3_M,						// EVEX.DDS.512.F2.0F38.W0 9A

		VEX_Vfmsub132ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 9B
		VEX_Vfmsub132sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 9B
		EVEX_Vfmsub132ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 9B
		EVEX_Vfmsub132sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 9B

		EVEX_V4fmaddss_VX_k1z_HXP3_M,						// EVEX.DDS.LIG.F2.0F38.W0 9B

		VEX_Vfnmadd132ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 9C
		VEX_Vfnmadd132ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 9C
		VEX_Vfnmadd132pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 9C
		VEX_Vfnmadd132pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 9C
		EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 9C
		EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 9C
		EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 9C
		EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 9C
		EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 9C
		EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 9C

		VEX_Vfnmadd132ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 9D
		VEX_Vfnmadd132sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 9D
		EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 9D
		EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 9D

		VEX_Vfnmsub132ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 9E
		VEX_Vfnmsub132ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 9E
		VEX_Vfnmsub132pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 9E
		VEX_Vfnmsub132pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 9E
		EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 9E
		EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 9E
		EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 9E
		EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 9E
		EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 9E
		EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 9E

		VEX_Vfnmsub132ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 9F
		VEX_Vfnmsub132sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 9F
		EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 9F
		EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 9F

		EVEX_Vpscatterdd_VM32X_k1_VX,						// EVEX.128.66.0F38.W0 A0
		EVEX_Vpscatterdd_VM32Y_k1_VY,						// EVEX.256.66.0F38.W0 A0
		EVEX_Vpscatterdd_VM32Z_k1_VZ,						// EVEX.512.66.0F38.W0 A0
		EVEX_Vpscatterdq_VM32X_k1_VX,						// EVEX.128.66.0F38.W1 A0
		EVEX_Vpscatterdq_VM32X_k1_VY,						// EVEX.256.66.0F38.W1 A0
		EVEX_Vpscatterdq_VM32Y_k1_VZ,						// EVEX.512.66.0F38.W1 A0

		EVEX_Vpscatterqd_VM64X_k1_VX,						// EVEX.128.66.0F38.W0 A1
		EVEX_Vpscatterqd_VM64Y_k1_VX,						// EVEX.256.66.0F38.W0 A1
		EVEX_Vpscatterqd_VM64Z_k1_VY,						// EVEX.512.66.0F38.W0 A1
		EVEX_Vpscatterqq_VM64X_k1_VX,						// EVEX.128.66.0F38.W1 A1
		EVEX_Vpscatterqq_VM64Y_k1_VY,						// EVEX.256.66.0F38.W1 A1
		EVEX_Vpscatterqq_VM64Z_k1_VZ,						// EVEX.512.66.0F38.W1 A1

		EVEX_Vscatterdps_VM32X_k1_VX,						// EVEX.128.66.0F38.W0 A2
		EVEX_Vscatterdps_VM32Y_k1_VY,						// EVEX.256.66.0F38.W0 A2
		EVEX_Vscatterdps_VM32Z_k1_VZ,						// EVEX.512.66.0F38.W0 A2
		EVEX_Vscatterdpd_VM32X_k1_VX,						// EVEX.128.66.0F38.W1 A2
		EVEX_Vscatterdpd_VM32X_k1_VY,						// EVEX.256.66.0F38.W1 A2
		EVEX_Vscatterdpd_VM32Y_k1_VZ,						// EVEX.512.66.0F38.W1 A2

		EVEX_Vscatterqps_VM64X_k1_VX,						// EVEX.128.66.0F38.W0 A3
		EVEX_Vscatterqps_VM64Y_k1_VX,						// EVEX.256.66.0F38.W0 A3
		EVEX_Vscatterqps_VM64Z_k1_VY,						// EVEX.512.66.0F38.W0 A3
		EVEX_Vscatterqpd_VM64X_k1_VX,						// EVEX.128.66.0F38.W1 A3
		EVEX_Vscatterqpd_VM64Y_k1_VY,						// EVEX.256.66.0F38.W1 A3
		EVEX_Vscatterqpd_VM64Z_k1_VZ,						// EVEX.512.66.0F38.W1 A3

		VEX_Vfmaddsub213ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 A6
		VEX_Vfmaddsub213ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 A6
		VEX_Vfmaddsub213pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 A6
		VEX_Vfmaddsub213pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 A6
		EVEX_Vfmaddsub213ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 A6
		EVEX_Vfmaddsub213ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 A6
		EVEX_Vfmaddsub213ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 A6
		EVEX_Vfmaddsub213pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 A6
		EVEX_Vfmaddsub213pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 A6
		EVEX_Vfmaddsub213pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 A6

		VEX_Vfmsubadd213ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 A7
		VEX_Vfmsubadd213ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 A7
		VEX_Vfmsubadd213pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 A7
		VEX_Vfmsubadd213pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 A7
		EVEX_Vfmsubadd213ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 A7
		EVEX_Vfmsubadd213ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 A7
		EVEX_Vfmsubadd213ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 A7
		EVEX_Vfmsubadd213pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 A7
		EVEX_Vfmsubadd213pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 A7
		EVEX_Vfmsubadd213pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 A7

		VEX_Vfmadd213ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 A8
		VEX_Vfmadd213ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 A8
		VEX_Vfmadd213pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 A8
		VEX_Vfmadd213pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 A8
		EVEX_Vfmadd213ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 A8
		EVEX_Vfmadd213ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 A8
		EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 A8
		EVEX_Vfmadd213pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 A8
		EVEX_Vfmadd213pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 A8
		EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 A8

		VEX_Vfmadd213ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 A9
		VEX_Vfmadd213sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 A9
		EVEX_Vfmadd213ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 A9
		EVEX_Vfmadd213sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 A9

		VEX_Vfmsub213ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 AA
		VEX_Vfmsub213ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 AA
		VEX_Vfmsub213pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 AA
		VEX_Vfmsub213pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 AA
		EVEX_Vfmsub213ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 AA
		EVEX_Vfmsub213ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 AA
		EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 AA
		EVEX_Vfmsub213pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 AA
		EVEX_Vfmsub213pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 AA
		EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 AA

		EVEX_V4fnmaddps_VZ_k1z_HZP3_M,						// EVEX.DDS.512.F2.0F38.W0 AA

		VEX_Vfmsub213ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 AB
		VEX_Vfmsub213sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 AB
		EVEX_Vfmsub213ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 AB
		EVEX_Vfmsub213sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 AB

		EVEX_V4fnmaddss_VX_k1z_HXP3_M,						// EVEX.DDS.LIG.F2.0F38.W0 AB

		VEX_Vfnmadd213ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 AC
		VEX_Vfnmadd213ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 AC
		VEX_Vfnmadd213pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 AC
		VEX_Vfnmadd213pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 AC
		EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 AC
		EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 AC
		EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 AC
		EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 AC
		EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 AC
		EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 AC

		VEX_Vfnmadd213ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 AD
		VEX_Vfnmadd213sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 AD
		EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 AD
		EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 AD

		VEX_Vfnmsub213ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 AE
		VEX_Vfnmsub213ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 AE
		VEX_Vfnmsub213pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 AE
		VEX_Vfnmsub213pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 AE
		EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 AE
		EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 AE
		EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 AE
		EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 AE
		EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 AE
		EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 AE

		VEX_Vfnmsub213ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 AF
		VEX_Vfnmsub213sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 AF
		EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 AF
		EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 AF

		EVEX_Vpmadd52luq_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 B4
		EVEX_Vpmadd52luq_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 B4
		EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b,					// EVEX.DDS.512.66.0F38.W1 B4

		EVEX_Vpmadd52huq_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 B5
		EVEX_Vpmadd52huq_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 B5
		EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b,					// EVEX.DDS.512.66.0F38.W1 B5

		VEX_Vfmaddsub231ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 B6
		VEX_Vfmaddsub231ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 B6
		VEX_Vfmaddsub231pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 B6
		VEX_Vfmaddsub231pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 B6
		EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 B6
		EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 B6
		EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 B6
		EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 B6
		EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 B6
		EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 B6

		VEX_Vfmsubadd231ps_VX_HX_WX,						// VEX.DDS.128.66.0F38.W0 B7
		VEX_Vfmsubadd231ps_VY_HY_WY,						// VEX.DDS.256.66.0F38.W0 B7
		VEX_Vfmsubadd231pd_VX_HX_WX,						// VEX.DDS.128.66.0F38.W1 B7
		VEX_Vfmsubadd231pd_VY_HY_WY,						// VEX.DDS.256.66.0F38.W1 B7
		EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W0 B7
		EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W0 B7
		EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W0 B7
		EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b,					// EVEX.DDS.128.66.0F38.W1 B7
		EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b,					// EVEX.DDS.256.66.0F38.W1 B7
		EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.DDS.512.66.0F38.W1 B7

		VEX_Vfmadd231ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 B8
		VEX_Vfmadd231ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 B8
		VEX_Vfmadd231pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 B8
		VEX_Vfmadd231pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 B8
		EVEX_Vfmadd231ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 B8
		EVEX_Vfmadd231ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 B8
		EVEX_Vfmadd231ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 B8
		EVEX_Vfmadd231pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 B8
		EVEX_Vfmadd231pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 B8
		EVEX_Vfmadd231pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 B8

		VEX_Vfmadd231ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 B9
		VEX_Vfmadd231sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 B9
		EVEX_Vfmadd231ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 B9
		EVEX_Vfmadd231sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 B9

		VEX_Vfmsub231ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 BA
		VEX_Vfmsub231ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 BA
		VEX_Vfmsub231pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 BA
		VEX_Vfmsub231pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 BA
		EVEX_Vfmsub231ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 BA
		EVEX_Vfmsub231ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 BA
		EVEX_Vfmsub231ps_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W0 BA
		EVEX_Vfmsub231pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 BA
		EVEX_Vfmsub231pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 BA
		EVEX_Vfmsub231pd_VZ_k1z_HZ_WZ_er_b,					// EVEX.NDS.512.66.0F38.W1 BA

		VEX_Vfmsub231ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 BB
		VEX_Vfmsub231sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 BB
		EVEX_Vfmsub231ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 BB
		EVEX_Vfmsub231sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 BB

		VEX_Vfnmadd231ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 BC
		VEX_Vfnmadd231ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 BC
		VEX_Vfnmadd231pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 BC
		VEX_Vfnmadd231pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 BC
		EVEX_Vfnmadd231ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 BC
		EVEX_Vfnmadd231ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 BC
		EVEX_Vfnmadd231ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 BC
		EVEX_Vfnmadd231pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 BC
		EVEX_Vfnmadd231pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 BC
		EVEX_Vfnmadd231pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 BC

		VEX_Vfnmadd231ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 BD
		VEX_Vfnmadd231sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 BD
		EVEX_Vfnmadd231ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 BD
		EVEX_Vfnmadd231sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 BD

		VEX_Vfnmsub231ps_VX_HX_WX,							// VEX.NDS.128.66.0F38.W0 BE
		VEX_Vfnmsub231ps_VY_HY_WY,							// VEX.NDS.256.66.0F38.W0 BE
		VEX_Vfnmsub231pd_VX_HX_WX,							// VEX.NDS.128.66.0F38.W1 BE
		VEX_Vfnmsub231pd_VY_HY_WY,							// VEX.NDS.256.66.0F38.W1 BE
		EVEX_Vfnmsub231ps_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W0 BE
		EVEX_Vfnmsub231ps_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W0 BE
		EVEX_Vfnmsub231ps_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W0 BE
		EVEX_Vfnmsub231pd_VX_k1z_HX_WX_b,					// EVEX.NDS.128.66.0F38.W1 BE
		EVEX_Vfnmsub231pd_VY_k1z_HY_WY_b,					// EVEX.NDS.256.66.0F38.W1 BE
		EVEX_Vfnmsub231pd_VZ_k1z_HZ_WZ_er_b,				// EVEX.NDS.512.66.0F38.W1 BE

		VEX_Vfnmsub231ss_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W0 BF
		VEX_Vfnmsub231sd_VX_HX_WX,							// VEX.DDS.LIG.66.0F38.W1 BF
		EVEX_Vfnmsub231ss_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W0 BF
		EVEX_Vfnmsub231sd_VX_k1z_HX_WX_er,					// EVEX.DDS.LIG.66.0F38.W1 BF

		EVEX_Vpconflictd_VX_k1z_WX_b,						// EVEX.128.66.0F38.W0 C4
		EVEX_Vpconflictd_VY_k1z_WY_b,						// EVEX.256.66.0F38.W0 C4
		EVEX_Vpconflictd_VZ_k1z_WZ_b,						// EVEX.512.66.0F38.W0 C4
		EVEX_Vpconflictq_VX_k1z_WX_b,						// EVEX.128.66.0F38.W1 C4
		EVEX_Vpconflictq_VY_k1z_WY_b,						// EVEX.256.66.0F38.W1 C4
		EVEX_Vpconflictq_VZ_k1z_WZ_b,						// EVEX.512.66.0F38.W1 C4

		Sha1nexte_VX_WX,									// 0F38C8
		Sha1msg1_VX_WX,										// 0F38C9
		Sha1msg2_VX_WX,										// 0F38CA
		Sha256rnds2_VX_WX,									// 0F38CB
		Sha256msg1_VX_WX,									// 0F38CC
		Sha256msg2_VX_WX,									// 0F38CD

		EVEX_Vgatherpf0dps_VM32Z_k1,						// EVEX.512.66.0F38.W0 C6 /1
		EVEX_Vgatherpf0dpd_VM32Y_k1,						// EVEX.512.66.0F38.W1 C6 /1
		EVEX_Vgatherpf1dps_VM32Z_k1,						// EVEX.512.66.0F38.W0 C6 /2
		EVEX_Vgatherpf1dpd_VM32Y_k1,						// EVEX.512.66.0F38.W1 C6 /2
		EVEX_Vscatterpf0dps_VM32Z_k1,						// EVEX.512.66.0F38.W0 C6 /5
		EVEX_Vscatterpf0dpd_VM32Y_k1,						// EVEX.512.66.0F38.W1 C6 /5
		EVEX_Vscatterpf1dps_VM32Z_k1,						// EVEX.512.66.0F38.W0 C6 /6
		EVEX_Vscatterpf1dpd_VM32Y_k1,						// EVEX.512.66.0F38.W1 C6 /6

		EVEX_Vgatherpf0qps_VM64Z_k1,						// EVEX.512.66.0F38.W0 C7 /1
		EVEX_Vgatherpf0qpd_VM64Z_k1,						// EVEX.512.66.0F38.W1 C7 /1
		EVEX_Vgatherpf1qps_VM64Z_k1,						// EVEX.512.66.0F38.W0 C7 /2
		EVEX_Vgatherpf1qpd_VM64Z_k1,						// EVEX.512.66.0F38.W1 C7 /2
		EVEX_Vscatterpf0qps_VM64Z_k1,						// EVEX.512.66.0F38.W0 C7 /5
		EVEX_Vscatterpf0qpd_VM64Z_k1,						// EVEX.512.66.0F38.W1 C7 /5
		EVEX_Vscatterpf1qps_VM64Z_k1,						// EVEX.512.66.0F38.W0 C7 /6
		EVEX_Vscatterpf1qpd_VM64Z_k1,						// EVEX.512.66.0F38.W1 C7 /6

		EVEX_Vexp2ps_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W0 C8
		EVEX_Vexp2pd_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W1 C8

		EVEX_Vrcp28ps_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W0 CA
		EVEX_Vrcp28pd_VZ_k1z_WZ_sae_b,						// EVEX.512.66.0F38.W1 CA

		EVEX_Vrcp28ss_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.66.0F38.W0 CB
		EVEX_Vrcp28sd_VX_k1z_HX_WX_sae,						// EVEX.NDS.LIG.66.0F38.W1 CB

		EVEX_Vrsqrt28ps_VZ_k1z_WZ_sae_b,					// EVEX.512.66.0F38.W0 CC
		EVEX_Vrsqrt28pd_VZ_k1z_WZ_sae_b,					// EVEX.512.66.0F38.W1 CC

		EVEX_Vrsqrt28ss_VX_k1z_HX_WX_sae,					// EVEX.NDS.LIG.66.0F38.W0 CD
		EVEX_Vrsqrt28sd_VX_k1z_HX_WX_sae,					// EVEX.NDS.LIG.66.0F38.W1 CD

		Aesimc_VX_WX,										// 66 0F38DB
		VEX_Vaesimc_VX_WX,									// VEX.128.66.0F38.WIG DB

		Aesenc_VX_WX,										// 66 0F38DC
		VEX_Vaesenc_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG DC

		Aesenclast_VX_WX,									// 66 0F38DD
		VEX_Vaesenclast_VX_HX_WX,							// VEX.NDS.128.66.0F38.WIG DD

		Aesdec_VX_WX,										// 66 0F38DE
		VEX_Vaesdec_VX_HX_WX,								// VEX.NDS.128.66.0F38.WIG DE

		Aesdeclast_VX_WX,									// 66 0F38DF
		VEX_Vaesdeclast_VX_HX_WX,							// VEX.NDS.128.66.0F38.WIG DF

		Movbe_Gw_Mw,										// o16 0F38F0
		Movbe_Gd_Md,										// o32 0F38F0
		Movbe_Gq_Mq,										// REX.W 0F38F0

		Movbe_Mw_Gw,										// o16 0F38F1
		Movbe_Md_Gd,										// o32 0F38F1
		Movbe_Mq_Gq,										// REX.W 0F38F1

		Crc32_Gd_Eb,										// F2 0F38F0
		Crc32_Gq_Eb,										// F2 REX.W 0F38F0

		Crc32_Gd_Ed,										// F2 0F38F1
		Crc32_Gq_Eq,										// F2 REX.W 0F38F1

		VEX_Andn_Gd_Hd_Ed,									// VEX.NDS.L0.0F38.W0 F2
		VEX_Andn_Gq_Hq_Eq,									// VEX.NDS.L0.0F38.W1 F2

		VEX_Blsr_Hd_Ed,										// VEX.NDD.L0.0F38.W0 F3 /1
		VEX_Blsr_Hq_Eq,										// VEX.NDD.L0.0F38.W1 F3 /1
		VEX_Blsmsk_Hd_Ed,									// VEX.NDD.L0.0F38.W0 F3 /2
		VEX_Blsmsk_Hq_Eq,									// VEX.NDD.L0.0F38.W1 F3 /2
		VEX_Blsi_Hd_Ed,										// VEX.NDD.L0.0F38.W0 F3 /3
		VEX_Blsi_Hq_Eq,										// VEX.NDD.L0.0F38.W1 F3 /3

		VEX_Bzhi_Gd_Ed_Hd,									// VEX.NDS.L0.0F38.W0 F5
		VEX_Bzhi_Gq_Eq_Hq,									// VEX.NDS.L0.0F38.W1 F5

		VEX_Pext_Gd_Hd_Ed,									// VEX.NDS.L0.F3.0F38.W0 F5
		VEX_Pext_Gq_Hq_Eq,									// VEX.NDS.L0.F3.0F38.W1 F5

		VEX_Pdep_Gd_Hd_Ed,									// VEX.NDS.L0.F2.0F38.W0 F5
		VEX_Pdep_Gq_Hq_Eq,									// VEX.NDS.L0.F2.0F38.W1 F5

		Adcx_Gd_Ed,											// 66 0F38F6
		Adcx_Gq_Eq,											// 66 REX.W 0F38F6

		Adox_Gd_Ed,											// F3 0F38F6
		Adox_Gq_Eq,											// F3 REX.W 0F38F6

		VEX_Mulx_Gd_Hd_Ed,									// VEX.NDD.L0.F2.0F38.W0 F6
		VEX_Mulx_Gq_Hq_Eq,									// VEX.NDD.L0.F2.0F38.W1 F6

		VEX_Bextr_Gd_Ed_Hd,									// VEX.NDS.L0.0F38.W0 F7
		VEX_Bextr_Gq_Eq_Hq,									// VEX.NDS.L0.0F38.W1 F7

		VEX_Shlx_Gd_Ed_Hd,									// VEX.NDS.L0.66.0F38.W0 F7
		VEX_Shlx_Gq_Eq_Hq,									// VEX.NDS.L0.66.0F38.W1 F7

		VEX_Sarx_Gd_Ed_Hd,									// VEX.NDS.L0.F3.0F38.W0 F7
		VEX_Sarx_Gq_Eq_Hq,									// VEX.NDS.L0.F3.0F38.W1 F7

		VEX_Shrx_Gd_Ed_Hd,									// VEX.NDS.L0.F2.0F38.W0 F7
		VEX_Shrx_Gq_Eq_Hq,									// VEX.NDS.L0.F2.0F38.W1 F7

		// 0F3Axx opcodes

		VEX_Vpermq_VY_WY_Ib,								// VEX.256.66.0F3A.W1 00
		EVEX_Vpermq_VY_k1z_WY_Ib_b,							// EVEX.256.66.0F3A.W1 00
		EVEX_Vpermq_VZ_k1z_WZ_Ib_b,							// EVEX.512.66.0F3A.W1 00

		VEX_Vpermpd_VY_WY_Ib,								// VEX.256.66.0F3A.W1 01
		EVEX_Vpermpd_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W1 01
		EVEX_Vpermpd_VZ_k1z_WZ_Ib_b,						// EVEX.512.66.0F3A.W1 01

		VEX_Vpblendd_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.W0 02
		VEX_Vpblendd_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.W0 02

		EVEX_Valignd_VX_k1z_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W0 03
		EVEX_Valignd_VY_k1z_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W0 03
		EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W0 03
		EVEX_Valignq_VX_k1z_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W1 03
		EVEX_Valignq_VY_k1z_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W1 03
		EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W1 03

		VEX_Vpermilps_VX_WX_Ib,								// VEX.128.66.0F3A.W0 04
		VEX_Vpermilps_VY_WY_Ib,								// VEX.256.66.0F3A.W0 04
		EVEX_Vpermilps_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W0 04
		EVEX_Vpermilps_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W0 04
		EVEX_Vpermilps_VZ_k1z_WZ_Ib_b,						// EVEX.512.66.0F3A.W0 04

		VEX_Vpermilpd_VX_WX_Ib,								// VEX.128.66.0F3A.W0 05
		VEX_Vpermilpd_VY_WY_Ib,								// VEX.256.66.0F3A.W0 05
		EVEX_Vpermilpd_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W1 05
		EVEX_Vpermilpd_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W1 05
		EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b,						// EVEX.512.66.0F3A.W1 05

		VEX_Vperm2f128_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.W0 06

		Roundps_VX_WX_Ib,									// 66 0F3A08
		VEX_Vroundps_VX_WX_Ib,								// VEX.128.66.0F3A.WIG 08
		VEX_Vroundps_VY_WY_Ib,								// VEX.256.66.0F3A.WIG 08
		EVEX_Vrndscaleps_VX_k1z_WX_Ib_b,					// EVEX.128.66.0F3A.W0 08
		EVEX_Vrndscaleps_VY_k1z_WY_Ib_b,					// EVEX.256.66.0F3A.W0 08
		EVEX_Vrndscaleps_VZ_k1z_WZ_Ib_sae_b,				// EVEX.512.66.0F3A.W0 08

		Roundpd_VX_WX_Ib,									// 66 0F3A09
		VEX_Vroundpd_VX_WX_Ib,								// VEX.128.66.0F3A.WIG 09
		VEX_Vroundpd_VY_WY_Ib,								// VEX.256.66.0F3A.WIG 09
		EVEX_Vrndscalepd_VX_k1z_WX_Ib_b,					// EVEX.128.66.0F3A.W1 09
		EVEX_Vrndscalepd_VY_k1z_WY_Ib_b,					// EVEX.256.66.0F3A.W1 09
		EVEX_Vrndscalepd_VZ_k1z_WZ_Ib_sae_b,				// EVEX.512.66.0F3A.W1 09

		Roundss_VX_WX_Ib,									// 66 0F3A0A
		VEX_Vroundss_VX_HX_WX_Ib,							// VEX.NDS.LIG.66.0F3A.WIG 0A
		EVEX_Vrndscaless_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W0 0A

		Roundsd_VX_WX_Ib,									// 66 0F3A0B
		VEX_Vroundsd_VX_HX_WX_Ib,							// VEX.NDS.LIG.66.0F3A.WIG 0B
		EVEX_Vrndscalesd_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W1 0B

		Blendps_VX_WX_Ib,									// 66 0F3A0C
		VEX_Vblendps_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 0C
		VEX_Vblendps_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.WIG 0C

		Blendpd_VX_WX_Ib,									// 66 0F3A0D
		VEX_Vblendpd_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 0D
		VEX_Vblendpd_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.WIG 0D

		Pblendw_VX_WX_Ib,									// 66 0F3A0E
		VEX_Vpblendw_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 0E
		VEX_Vpblendw_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.WIG 0E

		Palignr_P_Q_Ib,										// 0F3A0F

		Palignr_VX_WX_Ib,									// 66 0F3A0F
		VEX_Vpalignr_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 0F
		VEX_Vpalignr_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.WIG 0F
		EVEX_Vpalignr_VX_k1z_HX_WX_Ib,						// EVEX.NDS.128.66.0F3A.WIG 0F
		EVEX_Vpalignr_VY_k1z_HY_WY_Ib,						// EVEX.NDS.256.66.0F3A.WIG 0F
		EVEX_Vpalignr_VZ_k1z_HZ_WZ_Ib,						// EVEX.NDS.512.66.0F3A.WIG 0F

		Pextrb_RdMb_VX_Ib,									// 66 0F3A14
		Pextrb_RqMb_VX_Ib,									// 66 REX.W 0F3A14
		VEX_Vpextrb_RdMb_VX_Ib,								// VEX.128.66.0F3A.W0 14
		VEX_Vpextrb_RqMb_VX_Ib,								// VEX.128.66.0F3A.W1 14
		EVEX_Vpextrb_RdMb_VX_Ib,							// EVEX.128.66.0F3A.W0 14
		EVEX_Vpextrb_RqMb_VX_Ib,							// EVEX.128.66.0F3A.W1 14

		Pextrw_RdMw_VX_Ib,									// 66 0F3A15
		Pextrw_RqMw_VX_Ib,									// 66 REX.W 0F3A15
		VEX_Vpextrw_RdMw_VX_Ib,								// VEX.128.66.0F3A.W0 15
		VEX_Vpextrw_RqMw_VX_Ib,								// VEX.128.66.0F3A.W1 15
		EVEX_Vpextrw_RdMw_VX_Ib,							// EVEX.128.66.0F3A.W0 15
		EVEX_Vpextrw_RqMw_VX_Ib,							// EVEX.128.66.0F3A.W1 15

		Pextrd_Ed_VX_Ib,									// 66 0F3A16
		Pextrq_Eq_VX_Ib,									// 66 REX.W 0F3A16
		VEX_Vpextrd_Ed_VX_Ib,								// VEX.128.66.0F3A.W0 16
		VEX_Vpextrq_Eq_VX_Ib,								// VEX.128.66.0F3A.W1 16
		EVEX_Vpextrd_Ed_VX_Ib,								// EVEX.128.66.0F3A.W0 16
		EVEX_Vpextrq_Eq_VX_Ib,								// EVEX.128.66.0F3A.W1 16

		Extractps_Ed_VX_Ib,									// 66 0F3A17
		Extractps_Eq_VX_Ib,									// 66 REX.W 0F3A17
		VEX_Vextractps_Ed_VX_Ib,							// VEX.128.66.0F3A.W0 17
		VEX_Vextractps_Eq_VX_Ib,							// VEX.128.66.0F3A.W1 17
		EVEX_Vextractps_Ed_VX_Ib,							// EVEX.128.66.0F3A.W0 17
		EVEX_Vextractps_Eq_VX_Ib,							// EVEX.128.66.0F3A.W1 17

		VEX_Vinsertf128_VY_HY_WX_Ib,						// VEX.NDS.256.66.0F3A.W0 18
		EVEX_Vinsertf32x4_VY_k1z_HY_WX_Ib,					// EVEX.NDS.256.66.0F3A.W0 18
		EVEX_Vinsertf32x4_VZ_k1z_HZ_WX_Ib,					// EVEX.NDS.512.66.0F3A.W0 18
		EVEX_Vinsertf64x2_VY_k1z_HY_WX_Ib,					// EVEX.NDS.256.66.0F3A.W1 18
		EVEX_Vinsertf64x2_VZ_k1z_HZ_WX_Ib,					// EVEX.NDS.512.66.0F3A.W1 18

		VEX_Vextractf128_WX_VY_Ib,							// VEX.256.66.0F3A.W0 19
		EVEX_Vextractf32x4_WX_k1z_VY_Ib,					// EVEX.256.66.0F3A.W0 19
		EVEX_Vextractf32x4_WX_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W0 19
		EVEX_Vextractf64x2_WX_k1z_VY_Ib,					// EVEX.256.66.0F3A.W1 19
		EVEX_Vextractf64x2_WX_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W1 19

		EVEX_Vinsertf32x8_VZ_k1z_HZ_WY_Ib,					// EVEX.NDS.512.66.0F3A.W0 1A
		EVEX_Vinsertf64x4_VZ_k1z_HZ_WY_Ib,					// EVEX.NDS.512.66.0F3A.W1 1A

		EVEX_Vextractf32x8_WY_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W0 1B
		EVEX_Vextractf64x4_WY_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W1 1B

		VEX_Vcvtps2ph_WX_VX_Ib,								// VEX.128.66.0F3A.W0 1D
		VEX_Vcvtps2ph_WX_VY_Ib,								// VEX.256.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_WX_k1z_VX_Ib,						// EVEX.128.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_WX_k1z_VY_Ib,						// EVEX.256.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_WY_k1z_VZ_Ib_sae,					// EVEX.512.66.0F3A.W0 1D

		EVEX_Vpcmpud_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W0 1E
		EVEX_Vpcmpud_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W0 1E
		EVEX_Vpcmpud_VK_k1_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W0 1E
		EVEX_Vpcmpuq_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W1 1E
		EVEX_Vpcmpuq_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W1 1E
		EVEX_Vpcmpuq_VK_k1_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W1 1E

		EVEX_Vpcmpd_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W0 1F
		EVEX_Vpcmpd_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W0 1F
		EVEX_Vpcmpd_VK_k1_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W0 1F
		EVEX_Vpcmpq_VK_k1_HX_WX_Ib_b,						// EVEX.NDS.128.66.0F3A.W1 1F
		EVEX_Vpcmpq_VK_k1_HY_WY_Ib_b,						// EVEX.NDS.256.66.0F3A.W1 1F
		EVEX_Vpcmpq_VK_k1_HZ_WZ_Ib_b,						// EVEX.NDS.512.66.0F3A.W1 1F

		Pinsrb_VX_RdMb_Ib,									// 66 0F3A20
		Pinsrb_VX_RqMb_Ib,									// 66 REX.W 0F3A20
		VEX_Vpinsrb_VX_HX_RdMb_Ib,							// VEX.NDS.128.66.0F3A.W0 20
		VEX_Vpinsrb_VX_HX_RqMb_Ib,							// VEX.NDS.128.66.0F3A.W1 20
		EVEX_Vpinsrb_VX_HX_RdMb_Ib,							// EVEX.NDS.128.66.0F3A.W0 20
		EVEX_Vpinsrb_VX_HX_RqMb_Ib,							// EVEX.NDS.128.66.0F3A.W1 20

		Insertps_VX_WX_Ib,									// 66 0F3A21
		VEX_Vinsertps_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 21
		EVEX_Vinsertps_VX_HX_WX_Ib,							// EVEX.NDS.128.66.0F3A.W0 21

		Pinsrd_VX_Ed_Ib,									// 66 0F3A22
		Pinsrq_VX_Eq_Ib,									// REX.W 66 0F3A22
		VEX_Vpinsrd_VX_HX_Ed_Ib,							// VEX.NDS.128.66.0F3A.W0 22
		VEX_Vpinsrq_VX_HX_Eq_Ib,							// VEX.NDS.128.66.0F3A.W1 22
		EVEX_Vpinsrd_VX_HX_Ed_Ib,							// EVEX.NDS.128.66.0F3A.W0 22
		EVEX_Vpinsrq_VX_HX_Eq_Ib,							// EVEX.NDS.128.66.0F3A.W1 22

		EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W0 23
		EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.NDS.512.66.0F3A.W0 23
		EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W1 23
		EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.NDS.512.66.0F3A.W1 23

		EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b,					// EVEX.DDS.128.66.0F3A.W0 25
		EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b,					// EVEX.DDS.256.66.0F3A.W0 25
		EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.DDS.512.66.0F3A.W0 25
		EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b,					// EVEX.DDS.128.66.0F3A.W1 25
		EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b,					// EVEX.DDS.256.66.0F3A.W1 25
		EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.DDS.512.66.0F3A.W1 25

		EVEX_Vgetmantps_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W0 26
		EVEX_Vgetmantps_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W0 26
		EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b,					// EVEX.512.66.0F3A.W0 26
		EVEX_Vgetmantpd_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W1 26
		EVEX_Vgetmantpd_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W1 26
		EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b,					// EVEX.512.66.0F3A.W1 26

		EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W0 27
		EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W1 27

		VEX_Kshiftrw_VK_RK_Ib,								// VEX.L0.66.0F3A.W1 30
		VEX_Kshiftrb_VK_RK_Ib,								// VEX.L0.66.0F3A.W0 30
		VEX_Kshiftrq_VK_RK_Ib,								// VEX.L0.66.0F3A.W1 31
		VEX_Kshiftrd_VK_RK_Ib,								// VEX.L0.66.0F3A.W0 31

		VEX_Kshiftlw_VK_RK_Ib,								// VEX.L0.66.0F3A.W1 32
		VEX_Kshiftlb_VK_RK_Ib,								// VEX.L0.66.0F3A.W0 32
		VEX_Kshiftlq_VK_RK_Ib,								// VEX.L0.66.0F3A.W1 33
		VEX_Kshiftld_VK_RK_Ib,								// VEX.L0.66.0F3A.W0 33

		VEX_Vinserti128_VY_HY_WX_Ib,						// VEX.NDS.256.66.0F3A.W0 38
		EVEX_Vinserti32x4_VY_k1z_HY_WX_Ib,					// EVEX.NDS.256.66.0F3A.W0 38
		EVEX_Vinserti32x4_VZ_k1z_HZ_WX_Ib,					// EVEX.NDS.512.66.0F3A.W0 38
		EVEX_Vinserti64x2_VY_k1z_HY_WX_Ib,					// EVEX.NDS.256.66.0F3A.W1 38
		EVEX_Vinserti64x2_VZ_k1z_HZ_WX_Ib,					// EVEX.NDS.512.66.0F3A.W1 38

		VEX_Vextracti128_WX_VY_Ib,							// VEX.256.66.0F3A.W0 39
		EVEX_Vextracti32x4_WX_k1z_VY_Ib,					// EVEX.256.66.0F3A.W0 39
		EVEX_Vextracti32x4_WX_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W0 39
		EVEX_Vextracti64x2_WX_k1z_VY_Ib,					// EVEX.256.66.0F3A.W1 39
		EVEX_Vextracti64x2_WX_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W1 39

		EVEX_Vinserti32x8_VZ_k1z_HZ_WY_Ib,					// EVEX.NDS.512.66.0F3A.W0 3A
		EVEX_Vinserti64x4_VZ_k1z_HZ_WY_Ib,					// EVEX.NDS.512.66.0F3A.W1 3A

		EVEX_Vextracti32x8_WY_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W0 3B
		EVEX_Vextracti64x4_WY_k1z_VZ_Ib,					// EVEX.512.66.0F3A.W1 3B

		EVEX_Vpcmpub_VK_k1_HX_WX_Ib,						// EVEX.NDS.128.66.0F3A.W0 3E
		EVEX_Vpcmpub_VK_k1_HY_WY_Ib,						// EVEX.NDS.256.66.0F3A.W0 3E
		EVEX_Vpcmpub_VK_k1_HZ_WZ_Ib,						// EVEX.NDS.512.66.0F3A.W0 3E
		EVEX_Vpcmpuw_VK_k1_HX_WX_Ib,						// EVEX.NDS.128.66.0F3A.W1 3E
		EVEX_Vpcmpuw_VK_k1_HY_WY_Ib,						// EVEX.NDS.256.66.0F3A.W1 3E
		EVEX_Vpcmpuw_VK_k1_HZ_WZ_Ib,						// EVEX.NDS.512.66.0F3A.W1 3E

		EVEX_Vpcmpb_VK_k1_HX_WX_Ib,							// EVEX.NDS.128.66.0F3A.W0 3F
		EVEX_Vpcmpb_VK_k1_HY_WY_Ib,							// EVEX.NDS.256.66.0F3A.W0 3F
		EVEX_Vpcmpb_VK_k1_HZ_WZ_Ib,							// EVEX.NDS.512.66.0F3A.W0 3F
		EVEX_Vpcmpw_VK_k1_HX_WX_Ib,							// EVEX.NDS.128.66.0F3A.W1 3F
		EVEX_Vpcmpw_VK_k1_HY_WY_Ib,							// EVEX.NDS.256.66.0F3A.W1 3F
		EVEX_Vpcmpw_VK_k1_HZ_WZ_Ib,							// EVEX.NDS.512.66.0F3A.W1 3F

		Dpps_VX_WX_Ib,										// 66 0F3A40
		VEX_Vdpps_VX_HX_WX_Ib,								// VEX.NDS.128.66.0F3A.WIG 40
		VEX_Vdpps_VY_HY_WY_Ib,								// VEX.NDS.256.66.0F3A.WIG 40

		Dppd_VX_WX_Ib,										// 66 0F3A41
		VEX_Vdppd_VX_HX_WX_Ib,								// VEX.NDS.128.66.0F3A.WIG 41

		Mpsadbw_VX_WX_Ib,									// 66 0F3A42
		VEX_Vmpsadbw_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 42
		VEX_Vmpsadbw_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.WIG 42
		EVEX_Vdbpsadbw_VX_k1z_HX_WX_Ib,						// EVEX.NDS.128.66.0F3A.W0 42
		EVEX_Vdbpsadbw_VY_k1z_HY_WY_Ib,						// EVEX.NDS.256.66.0F3A.W0 42
		EVEX_Vdbpsadbw_VZ_k1z_HZ_WZ_Ib,						// EVEX.NDS.512.66.0F3A.W0 42

		EVEX_Vshufi32x4_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W0 43
		EVEX_Vshufi32x4_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.NDS.512.66.0F3A.W0 43
		EVEX_Vshufi64x2_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W1 43
		EVEX_Vshufi64x2_VZ_k1z_HZ_WZ_Ib_b,					// EVEX.NDS.512.66.0F3A.W1 43

		Pclmulqdq_VX_WX_Ib,									// 66 0F3A44
		VEX_Vpclmulqdq_VX_HX_WX_Ib,							// VEX.NDS.128.66.0F3A.WIG 44

		VEX_Vperm2i128_VY_HY_WY_Ib,							// VEX.NDS.256.66.0F3A.W0 46

		VEX_Vblendvps_VX_HX_WX_Is4X,						// VEX.NDS.128.66.0F3A.W0 4A
		VEX_Vblendvps_VY_HY_WY_Is4Y,						// VEX.NDS.256.66.0F3A.W0 4A

		VEX_Vblendvpd_VX_HX_WX_Is4X,						// VEX.NDS.128.66.0F3A.W0 4B
		VEX_Vblendvpd_VY_HY_WY_Is4Y,						// VEX.NDS.256.66.0F3A.W0 4B

		VEX_Vpblendvb_VX_HX_WX_Is4X,						// VEX.NDS.128.66.0F3A.W0 4C
		VEX_Vpblendvb_VY_HY_WY_Is4Y,						// VEX.NDS.256.66.0F3A.W0 4C

		EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b,					// EVEX.NDS.128.66.0F3A.W0 50
		EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W0 50
		EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b,				// EVEX.NDS.512.66.0F3A.W0 50
		EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b,					// EVEX.NDS.128.66.0F3A.W1 50
		EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W1 50
		EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b,				// EVEX.NDS.512.66.0F3A.W1 50

		EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae,					// EVEX.NDS.LIG.66.0F3A.W0 51
		EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae,					// EVEX.NDS.LIG.66.0F3A.W1 51

		EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b,					// EVEX.NDS.128.66.0F3A.W0 54
		EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W0 54
		EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b,				// EVEX.NDS.512.66.0F3A.W0 54
		EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b,					// EVEX.NDS.128.66.0F3A.W1 54
		EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b,					// EVEX.NDS.256.66.0F3A.W1 54
		EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b,				// EVEX.NDS.512.66.0F3A.W1 54

		EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W0 55
		EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae,				// EVEX.NDS.LIG.66.0F3A.W1 55

		EVEX_Vreduceps_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W0 56
		EVEX_Vreduceps_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W0 56
		EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b,					// EVEX.512.66.0F3A.W0 56
		EVEX_Vreducepd_VX_k1z_WX_Ib_b,						// EVEX.128.66.0F3A.W1 56
		EVEX_Vreducepd_VY_k1z_WY_Ib_b,						// EVEX.256.66.0F3A.W1 56
		EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b,					// EVEX.512.66.0F3A.W1 56

		EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae,					// EVEX.NDS.LIG.66.0F3A.W0 57
		EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae,					// EVEX.NDS.LIG.66.0F3A.W1 57

		Pcmpestrm_VX_WX_Ib,									// 66 0F3A60
		VEX_Vpcmpestrm_VX_WX_Ib,							// VEX.128.66.0F3A.WIG 60

		Pcmpestri_VX_WX_Ib,									// 66 0F3A61
		VEX_Vpcmpestri_VX_WX_Ib,							// VEX.128.66.0F3A.WIG 61

		Pcmpistrm_VX_WX_Ib,									// 66 0F3A62
		VEX_Vpcmpistrm_VX_WX_Ib,							// VEX.128.66.0F3A.WIG 62

		Pcmpistri_VX_WX_Ib,									// 66 0F3A63
		VEX_Vpcmpistri_VX_WX_Ib,							// VEX.128.66.0F3A.WIG 63

		EVEX_Vfpclassps_VK_k1_WX_Ib_b,						// EVEX.128.66.0F3A.W0 66
		EVEX_Vfpclassps_VK_k1_WY_Ib_b,						// EVEX.256.66.0F3A.W0 66
		EVEX_Vfpclassps_VK_k1_WZ_Ib_b,						// EVEX.512.66.0F3A.W0 66
		EVEX_Vfpclasspd_VK_k1_WX_Ib_b,						// EVEX.128.66.0F3A.W1 66
		EVEX_Vfpclasspd_VK_k1_WY_Ib_b,						// EVEX.256.66.0F3A.W1 66
		EVEX_Vfpclasspd_VK_k1_WZ_Ib_b,						// EVEX.512.66.0F3A.W1 66

		EVEX_Vfpclassss_VK_k1_WX_Ib,						// EVEX.LIG.66.0F3A.W0 67
		EVEX_Vfpclasssd_VK_k1_WX_Ib,						// EVEX.LIG.66.0F3A.W1 67

		Sha1rnds4_VX_WX_Ib,									// 0F3ACC

		Aeskeygenassist_VX_WX_Ib,							// 66 0F3ADF
		VEX_Vaeskeygenassist_VX_WX_Ib,						// VEX.128.66.0F3A.WIG DF

		VEX_Rorx_Gd_Ed_Ib,									// VEX.L0.F2.0F3A.W0 F0
		VEX_Rorx_Gq_Eq_Ib,									// VEX.L0.F2.0F3A.W1 F0
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
	}
}
