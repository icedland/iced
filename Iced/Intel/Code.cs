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

		Add_rm8_r8,													// 00
		Add_rm16_r16,												// o16 01
		Add_rm32_r32,												// o32 01
		Add_rm64_r64,												// REX.W 01
		Add_r8_rm8,													// 02
		Add_r16_rm16,												// o16 03
		Add_r32_rm32,												// o32 03
		Add_r64_rm64,												// REX.W 03
		Add_AL_imm8,												// 04
		Add_AX_imm16,												// o16 05
		Add_EAX_imm32,												// o32 05
		Add_RAX_imm32,												// REX.W 05
		Pushw_ES,													// o16 06
		Pushd_ES,													// o32 06
		Popw_ES,													// o16 07
		Popd_ES,													// o32 07

		Or_rm8_r8,													// 08
		Or_rm16_r16,												// o16 09
		Or_rm32_r32,												// o32 09
		Or_rm64_r64,												// REX.W 09
		Or_r8_rm8,													// 0A
		Or_r16_rm16,												// o16 0B
		Or_r32_rm32,												// o32 0B
		Or_r64_rm64,												// REX.W 0B
		Or_AL_imm8,													// 0C
		Or_AX_imm16,												// o16 0D
		Or_EAX_imm32,												// o32 0D
		Or_RAX_imm32,												// REX.W 0D
		Pushw_CS,													// o16 0E
		Pushd_CS,													// o32 0E
		Popw_CS,													// 0F

		Adc_rm8_r8,													// 10
		Adc_rm16_r16,												// o16 11
		Adc_rm32_r32,												// o32 11
		Adc_rm64_r64,												// REX.W 11
		Adc_r8_rm8,													// 12
		Adc_r16_rm16,												// o16 13
		Adc_r32_rm32,												// o32 13
		Adc_r64_rm64,												// REX.W 13
		Adc_AL_imm8,												// 14
		Adc_AX_imm16,												// o16 15
		Adc_EAX_imm32,												// o32 15
		Adc_RAX_imm32,												// REX.W 15
		Pushw_SS,													// o16 16
		Pushd_SS,													// o32 16
		Popw_SS,													// o16 17
		Popd_SS,													// o32 17

		Sbb_rm8_r8,													// 18
		Sbb_rm16_r16,												// o16 19
		Sbb_rm32_r32,												// o32 19
		Sbb_rm64_r64,												// REX.W 19
		Sbb_r8_rm8,													// 1A
		Sbb_r16_rm16,												// o16 1B
		Sbb_r32_rm32,												// o32 1B
		Sbb_r64_rm64,												// REX.W 1B
		Sbb_AL_imm8,												// 1C
		Sbb_AX_imm16,												// o16 1D
		Sbb_EAX_imm32,												// o32 1D
		Sbb_RAX_imm32,												// REX.W 1D
		Pushw_DS,													// o16 1E
		Pushd_DS,													// o32 1E
		Popw_DS,													// o16 1F
		Popd_DS,													// o32 1F

		And_rm8_r8,													// 20
		And_rm16_r16,												// o16 21
		And_rm32_r32,												// o32 21
		And_rm64_r64,												// REX.W 21
		And_r8_rm8,													// 22
		And_r16_rm16,												// o16 23
		And_r32_rm32,												// o32 23
		And_r64_rm64,												// REX.W 23
		And_AL_imm8,												// 24
		And_AX_imm16,												// o16 25
		And_EAX_imm32,												// o32 25
		And_RAX_imm32,												// REX.W 25
		Daa,														// 27

		Sub_rm8_r8,													// 28
		Sub_rm16_r16,												// o16 29
		Sub_rm32_r32,												// o32 29
		Sub_rm64_r64,												// REX.W 29
		Sub_r8_rm8,													// 2A
		Sub_r16_rm16,												// o16 2B
		Sub_r32_rm32,												// o32 2B
		Sub_r64_rm64,												// REX.W 2B
		Sub_AL_imm8,												// 2C
		Sub_AX_imm16,												// o16 2D
		Sub_EAX_imm32,												// o32 2D
		Sub_RAX_imm32,												// REX.W 2D
		Das,														// 2F

		Xor_rm8_r8,													// 30
		Xor_rm16_r16,												// o16 31
		Xor_rm32_r32,												// o32 31
		Xor_rm64_r64,												// REX.W 31
		Xor_r8_rm8,													// 32
		Xor_r16_rm16,												// o16 33
		Xor_r32_rm32,												// o32 33
		Xor_r64_rm64,												// REX.W 33
		Xor_AL_imm8,												// 34
		Xor_AX_imm16,												// o16 35
		Xor_EAX_imm32,												// o32 35
		Xor_RAX_imm32,												// REX.W 35
		Aaa,														// 37

		Cmp_rm8_r8,													// 38
		Cmp_rm16_r16,												// o16 39
		Cmp_rm32_r32,												// o32 39
		Cmp_rm64_r64,												// REX.W 39
		Cmp_r8_rm8,													// 3A
		Cmp_r16_rm16,												// o16 3B
		Cmp_r32_rm32,												// o32 3B
		Cmp_r64_rm64,												// REX.W 3B
		Cmp_AL_imm8,												// 3C
		Cmp_AX_imm16,												// o16 3D
		Cmp_EAX_imm32,												// o32 3D
		Cmp_RAX_imm32,												// REX.W 3D
		Aas,														// 3F

		Inc_r16,													// o16 40
		Inc_r32,													// o32 40

		Dec_r16,													// o16 48
		Dec_r32,													// o32 48

		Push_r16,													// o16 50
		Push_r32,													// o32 50
		Push_r64,													// 50

		Pop_r16,													// o16 58
		Pop_r32,													// o32 58
		Pop_r64,													// 58

		Pushaw,														// o16 60
		Pushad,														// o32 60
		Popaw,														// o16 61
		Popad,														// o32 61
		Bound_r16_m1616,											// o16 62
		Bound_r32_m3232,											// o32 62
		Arpl_rm16_r16,												// o16 63
		Arpl_r32m16_r32,											// o32 63
		Movsxd_r16_rm16,											// o16 63
		Movsxd_r32_rm32,											// o32 63
		Movsxd_r64_rm32,											// REX.W 63

		Push_imm16,													// o16 68
		Pushd_imm32,												// o32 68
		Pushq_imm32,												// REX.W 68
		Imul_r16_rm16_imm16,										// o16 69
		Imul_r32_rm32_imm32,										// o32 69
		Imul_r64_rm64_imm32,										// REX.W 69
		Pushw_imm8,													// o16 6A
		Pushd_imm8,													// o32 6A
		Pushq_imm8,													// REX.W 6A
		Imul_r16_rm16_imm8,											// o16 6B
		Imul_r32_rm32_imm8,											// o32 6B
		Imul_r64_rm64_imm8,											// REX.W 6B
		Insb_m8_DX,													// 6C
		Insw_m16_DX,												// o16 6D
		Insd_m32_DX,												// o32 6D
		Outsb_DX_m8,												// 6E
		Outsw_DX_m16,												// o16 6F
		Outsd_DX_m32,												// o32 6F

		Jo_rel8_16,													// o16 70
		Jo_rel8_32,													// o32 70
		Jo_rel8_64,													// 70
		Jno_rel8_16,												// o16 71
		Jno_rel8_32,												// o32 71
		Jno_rel8_64,												// 71
		Jb_rel8_16,													// o16 72
		Jb_rel8_32,													// o32 72
		Jb_rel8_64,													// 72
		Jae_rel8_16,												// o16 73
		Jae_rel8_32,												// o32 73
		Jae_rel8_64,												// 73
		Je_rel8_16,													// o16 74
		Je_rel8_32,													// o32 74
		Je_rel8_64,													// 74
		Jne_rel8_16,												// o16 75
		Jne_rel8_32,												// o32 75
		Jne_rel8_64,												// 75
		Jbe_rel8_16,												// o16 76
		Jbe_rel8_32,												// o32 76
		Jbe_rel8_64,												// 76
		Ja_rel8_16,													// o16 77
		Ja_rel8_32,													// o32 77
		Ja_rel8_64,													// 77

		Js_rel8_16,													// o16 78
		Js_rel8_32,													// o32 78
		Js_rel8_64,													// 78
		Jns_rel8_16,												// o16 79
		Jns_rel8_32,												// o32 79
		Jns_rel8_64,												// 79
		Jp_rel8_16,													// o16 7A
		Jp_rel8_32,													// o32 7A
		Jp_rel8_64,													// 7A
		Jnp_rel8_16,												// o16 7B
		Jnp_rel8_32,												// o32 7B
		Jnp_rel8_64,												// 7B
		Jl_rel8_16,													// o16 7C
		Jl_rel8_32,													// o32 7C
		Jl_rel8_64,													// 7C
		Jge_rel8_16,												// o16 7D
		Jge_rel8_32,												// o32 7D
		Jge_rel8_64,												// 7D
		Jle_rel8_16,												// o16 7E
		Jle_rel8_32,												// o32 7E
		Jle_rel8_64,												// 7E
		Jg_rel8_16,													// o16 7F
		Jg_rel8_32,													// o32 7F
		Jg_rel8_64,													// 7F

		Add_rm8_imm8,												// 80 /0
		Or_rm8_imm8,												// 80 /1
		Adc_rm8_imm8,												// 80 /2
		Sbb_rm8_imm8,												// 80 /3
		And_rm8_imm8,												// 80 /4
		Sub_rm8_imm8,												// 80 /5
		Xor_rm8_imm8,												// 80 /6
		Cmp_rm8_imm8,												// 80 /7
		Add_rm16_imm16,												// o16 81 /0
		Add_rm32_imm32,												// o32 81 /0
		Add_rm64_imm32,												// REX.W 81 /0
		Or_rm16_imm16,												// o16 81 /1
		Or_rm32_imm32,												// o32 81 /1
		Or_rm64_imm32,												// REX.W 81 /1
		Adc_rm16_imm16,												// o16 81 /2
		Adc_rm32_imm32,												// o32 81 /2
		Adc_rm64_imm32,												// REX.W 81 /2
		Sbb_rm16_imm16,												// o16 81 /3
		Sbb_rm32_imm32,												// o32 81 /3
		Sbb_rm64_imm32,												// REX.W 81 /3
		And_rm16_imm16,												// o16 81 /4
		And_rm32_imm32,												// o32 81 /4
		And_rm64_imm32,												// REX.W 81 /4
		Sub_rm16_imm16,												// o16 81 /5
		Sub_rm32_imm32,												// o32 81 /5
		Sub_rm64_imm32,												// REX.W 81 /5
		Xor_rm16_imm16,												// o16 81 /6
		Xor_rm32_imm32,												// o32 81 /6
		Xor_rm64_imm32,												// REX.W 81 /6
		Cmp_rm16_imm16,												// o16 81 /7
		Cmp_rm32_imm32,												// o32 81 /7
		Cmp_rm64_imm32,												// REX.W 81 /7
		Add_rm8_imm8_82,											// 82 /0
		Or_rm8_imm8_82,												// 82 /1
		Adc_rm8_imm8_82,											// 82 /2
		Sbb_rm8_imm8_82,											// 82 /3
		And_rm8_imm8_82,											// 82 /4
		Sub_rm8_imm8_82,											// 82 /5
		Xor_rm8_imm8_82,											// 82 /6
		Cmp_rm8_imm8_82,											// 82 /7
		Add_rm16_imm8,												// o16 83 /0
		Add_rm32_imm8,												// o32 83 /0
		Add_rm64_imm8,												// REX.W 83 /0
		Or_rm16_imm8,												// o16 83 /1
		Or_rm32_imm8,												// o32 83 /1
		Or_rm64_imm8,												// REX.W 83 /1
		Adc_rm16_imm8,												// o16 83 /2
		Adc_rm32_imm8,												// o32 83 /2
		Adc_rm64_imm8,												// REX.W 83 /2
		Sbb_rm16_imm8,												// o16 83 /3
		Sbb_rm32_imm8,												// o32 83 /3
		Sbb_rm64_imm8,												// REX.W 83 /3
		And_rm16_imm8,												// o16 83 /4
		And_rm32_imm8,												// o32 83 /4
		And_rm64_imm8,												// REX.W 83 /4
		Sub_rm16_imm8,												// o16 83 /5
		Sub_rm32_imm8,												// o32 83 /5
		Sub_rm64_imm8,												// REX.W 83 /5
		Xor_rm16_imm8,												// o16 83 /6
		Xor_rm32_imm8,												// o32 83 /6
		Xor_rm64_imm8,												// REX.W 83 /6
		Cmp_rm16_imm8,												// o16 83 /7
		Cmp_rm32_imm8,												// o32 83 /7
		Cmp_rm64_imm8,												// REX.W 83 /7
		Test_rm8_r8,												// 84
		Test_rm16_r16,												// o16 85
		Test_rm32_r32,												// o32 85
		Test_rm64_r64,												// REX.W 85
		Xchg_rm8_r8,												// 86
		Xchg_rm16_r16,												// o16 87
		Xchg_rm32_r32,												// o32 87
		Xchg_rm64_r64,												// REX.W 87

		Mov_rm8_r8,													// 88
		Mov_rm16_r16,												// o16 89
		Mov_rm32_r32,												// o32 89
		Mov_rm64_r64,												// REX.W 89
		Mov_r8_rm8,													// 8A
		Mov_r16_rm16,												// o16 8B
		Mov_r32_rm32,												// o32 8B
		Mov_r64_rm64,												// REX.W 8B
		Mov_rm16_Sreg,												// o16 8C
		Mov_rm32_Sreg,												// o32 8C
		Mov_rm64_Sreg,												// REX.W 8C
		Lea_r16_m,													// o16 8D
		Lea_r32_m,													// o32 8D
		Lea_r64_m,													// REX.W 8D
		Mov_Sreg_rm16,												// o16 8E
		Mov_Sreg_rm32,												// o32 8E
		Mov_Sreg_rm64,												// REX.W 8E
		Pop_rm16,													// o16 8F /0
		Pop_rm32,													// o32 8F /0
		Pop_rm64,													// REX.W 8F /0

		Nopw,														// o16 90
		Nopd,														// o32 90
		Nopq,														// REX.W 90
		Xchg_r16_AX,												// o16 REX.B 90
		Xchg_r32_EAX,												// o32 REX.B 90
		Xchg_r64_RAX,												// REX.W REX.B 90

		Pause,														// F3 90

		Cbw,														// o16 98
		Cwde,														// o32 98
		Cdqe,														// REX.W 98
		Cwd,														// o16 99
		Cdq,														// o32 99
		Cqo,														// REX.W 99
		Call_ptr1616,												// o16 9A
		Call_ptr3216,												// o32 9A
		Wait,														// 9B
		Pushfw,														// o16 9C
		Pushfd,														// o32 9C
		Pushfq,														// 9C
		Popfw,														// o16 9D
		Popfd,														// o32 9D
		Popfq,														// 9D
		Sahf,														// 9E
		Lahf,														// 9F

		Mov_AL_moffs8,												// A0
		Mov_AX_moffs16,												// o16 A1
		Mov_EAX_moffs32,											// o32 A1
		Mov_RAX_moffs64,											// REX.W A1
		Mov_moffs8_AL,												// A2
		Mov_moffs16_AX,												// o16 A3
		Mov_moffs32_EAX,											// o32 A3
		Mov_moffs64_RAX,											// REX.W A3
		Movsb_m8_m8,												// A4
		Movsw_m16_m16,												// o16 A5
		Movsd_m32_m32,												// o32 A5
		Movsq_m64_m64,												// REX.W A5
		Cmpsb_m8_m8,												// A6
		Cmpsw_m16_m16,												// o16 A7
		Cmpsd_m32_m32,												// o32 A7
		Cmpsq_m64_m64,												// REX.W A7

		Test_AL_imm8,												// A8
		Test_AX_imm16,												// o16 A9
		Test_EAX_imm32,												// o32 A9
		Test_RAX_imm32,												// REX.W A9
		Stosb_m8_AL,												// AA
		Stosw_m16_AX,												// o16 AB
		Stosd_m32_EAX,												// o32 AB
		Stosq_m64_RAX,												// REX.W AB
		Lodsb_AL_m8,												// AC
		Lodsw_AX_m16,												// o16 AD
		Lodsd_EAX_m32,												// o32 AD
		Lodsq_RAX_m64,												// REX.W AD
		Scasb_AL_m8,												// AE
		Scasw_AX_m16,												// o16 AF
		Scasd_EAX_m32,												// o32 AF
		Scasq_RAX_m64,												// REX.W AF

		Mov_r8_imm8,												// B0

		Mov_r16_imm16,												// o16 B8
		Mov_r32_imm32,												// o32 B8
		Mov_r64_imm64,												// REX.W B8

		Rol_rm8_imm8,												// C0 /0
		Ror_rm8_imm8,												// C0 /1
		Rcl_rm8_imm8,												// C0 /2
		Rcr_rm8_imm8,												// C0 /3
		Shl_rm8_imm8,												// C0 /4
		Shr_rm8_imm8,												// C0 /5
		Sal_rm8_imm8,												// C0 /6
		Sar_rm8_imm8,												// C0 /7
		Rol_rm16_imm8,												// o16 C1 /0
		Rol_rm32_imm8,												// o32 C1 /0
		Rol_rm64_imm8,												// REX.W C1 /0
		Ror_rm16_imm8,												// o16 C1 /1
		Ror_rm32_imm8,												// o32 C1 /1
		Ror_rm64_imm8,												// REX.W C1 /1
		Rcl_rm16_imm8,												// o16 C1 /2
		Rcl_rm32_imm8,												// o32 C1 /2
		Rcl_rm64_imm8,												// REX.W C1 /2
		Rcr_rm16_imm8,												// o16 C1 /3
		Rcr_rm32_imm8,												// o32 C1 /3
		Rcr_rm64_imm8,												// REX.W C1 /3
		Shl_rm16_imm8,												// o16 C1 /4
		Shl_rm32_imm8,												// o32 C1 /4
		Shl_rm64_imm8,												// REX.W C1 /4
		Shr_rm16_imm8,												// o16 C1 /5
		Shr_rm32_imm8,												// o32 C1 /5
		Shr_rm64_imm8,												// REX.W C1 /5
		Sal_rm16_imm8,												// o16 C1 /6
		Sal_rm32_imm8,												// o32 C1 /6
		Sal_rm64_imm8,												// REX.W C1 /6
		Sar_rm16_imm8,												// o16 C1 /7
		Sar_rm32_imm8,												// o32 C1 /7
		Sar_rm64_imm8,												// REX.W C1 /7
		Retnw_imm16,												// o16 C2
		Retnd_imm16,												// o32 C2
		Retnq_imm16,												// C2
		Retnw,														// o16 C3
		Retnd,														// o32 C3
		Retnq,														// C3
		Les_r16_m32,												// o16 C4
		Les_r32_m48,												// o32 C4
		Lds_r16_m32,												// o16 C5
		Lds_r32_m48,												// o32 C5
		Mov_rm8_imm8,												// C6 /0
		Xabort_imm8,												// C6 F8
		Mov_rm16_imm16,												// o16 C7 /0
		Mov_rm32_imm32,												// o32 C7 /0
		Mov_rm64_imm32,												// REX.W C7 /0
		Xbegin_rel16,												// o16 C7 F8
		Xbegin_rel32,												// o32 C7 F8

		Enterw_imm16_imm8,											// o16 C8
		Enterd_imm16_imm8,											// o32 C8
		Enterq_imm16_imm8,											// REX.W C8
		Leavew,														// o16 C9
		Leaved,														// o32 C9
		Leaveq,														// REX.W C9
		Retfw_imm16,												// o16 CA
		Retfd_imm16,												// o32 CA
		Retfq_imm16,												// REX.W CA
		Retfw,														// o16 CB
		Retfd,														// o32 CB
		Retfq,														// REX.W CB
		Int3,														// CC
		Int_imm8,													// CD
		Into,														// CE
		Iretw,														// o16 CF
		Iretd,														// o32 CF
		Iretq,														// REX.W CF

		Rol_rm8_1,													// D0 /0
		Ror_rm8_1,													// D0 /1
		Rcl_rm8_1,													// D0 /2
		Rcr_rm8_1,													// D0 /3
		Shl_rm8_1,													// D0 /4
		Shr_rm8_1,													// D0 /5
		Sal_rm8_1,													// D0 /6
		Sar_rm8_1,													// D0 /7
		Rol_rm16_1,													// o16 D1 /0
		Rol_rm32_1,													// o32 D1 /0
		Rol_rm64_1,													// REX.W D1 /0
		Ror_rm16_1,													// o16 D1 /1
		Ror_rm32_1,													// o32 D1 /1
		Ror_rm64_1,													// REX.W D1 /1
		Rcl_rm16_1,													// o16 D1 /2
		Rcl_rm32_1,													// o32 D1 /2
		Rcl_rm64_1,													// REX.W D1 /2
		Rcr_rm16_1,													// o16 D1 /3
		Rcr_rm32_1,													// o32 D1 /3
		Rcr_rm64_1,													// REX.W D1 /3
		Shl_rm16_1,													// o16 D1 /4
		Shl_rm32_1,													// o32 D1 /4
		Shl_rm64_1,													// REX.W D1 /4
		Shr_rm16_1,													// o16 D1 /5
		Shr_rm32_1,													// o32 D1 /5
		Shr_rm64_1,													// REX.W D1 /5
		Sal_rm16_1,													// o16 D1 /6
		Sal_rm32_1,													// o32 D1 /6
		Sal_rm64_1,													// REX.W D1 /6
		Sar_rm16_1,													// o16 D1 /7
		Sar_rm32_1,													// o32 D1 /7
		Sar_rm64_1,													// REX.W D1 /7
		Rol_rm8_CL,													// D2 /0
		Ror_rm8_CL,													// D2 /1
		Rcl_rm8_CL,													// D2 /2
		Rcr_rm8_CL,													// D2 /3
		Shl_rm8_CL,													// D2 /4
		Shr_rm8_CL,													// D2 /5
		Sal_rm8_CL,													// D2 /6
		Sar_rm8_CL,													// D2 /7
		Rol_rm16_CL,												// o16 D3 /0
		Rol_rm32_CL,												// o32 D3 /0
		Rol_rm64_CL,												// REX.W D3 /0
		Ror_rm16_CL,												// o16 D3 /1
		Ror_rm32_CL,												// o32 D3 /1
		Ror_rm64_CL,												// REX.W D3 /1
		Rcl_rm16_CL,												// o16 D3 /2
		Rcl_rm32_CL,												// o32 D3 /2
		Rcl_rm64_CL,												// REX.W D3 /2
		Rcr_rm16_CL,												// o16 D3 /3
		Rcr_rm32_CL,												// o32 D3 /3
		Rcr_rm64_CL,												// REX.W D3 /3
		Shl_rm16_CL,												// o16 D3 /4
		Shl_rm32_CL,												// o32 D3 /4
		Shl_rm64_CL,												// REX.W D3 /4
		Shr_rm16_CL,												// o16 D3 /5
		Shr_rm32_CL,												// o32 D3 /5
		Shr_rm64_CL,												// REX.W D3 /5
		Sal_rm16_CL,												// o16 D3 /6
		Sal_rm32_CL,												// o32 D3 /6
		Sal_rm64_CL,												// REX.W D3 /6
		Sar_rm16_CL,												// o16 D3 /7
		Sar_rm32_CL,												// o32 D3 /7
		Sar_rm64_CL,												// REX.W D3 /7
		Aam_imm8,													// D4
		Aad_imm8,													// D5
		Salc,														// D6
		Xlatb,														// D7

		Fadd_m32fp,													// D8 /0
		Fmul_m32fp,													// D8 /1
		Fcom_m32fp,													// D8 /2
		Fcomp_m32fp,												// D8 /3
		Fsub_m32fp,													// D8 /4
		Fsubr_m32fp,												// D8 /5
		Fdiv_m32fp,													// D8 /6
		Fdivr_m32fp,												// D8 /7
		Fadd_st0_sti,												// D8 C0+i
		Fmul_st0_sti,												// D8 C8+i
		Fcom_st0_sti,												// D8 D0+i
		Fcomp_st0_sti,												// D8 D8+i
		Fsub_st0_sti,												// D8 E0+i
		Fsubr_st0_sti,												// D8 E8+i
		Fdiv_st0_sti,												// D8 F0+i
		Fdivr_st0_sti,												// D8 F8+i

		Fld_m32fp,													// D9 /0
		Fst_m32fp,													// D9 /2
		Fstp_m32fp,													// D9 /3
		Fldenv_m14byte,												// o16 D9 /4
		Fldenv_m28byte,												// o32 D9 /4
		Fldcw_m16,													// D9 /5
		Fnstenv_m14byte,											// o16 D9 /6
		Fstenv_m14byte,												// 9B o16 D9 /6
		Fnstenv_m28byte,											// o32 D9 /6
		Fstenv_m28byte,												// 9B o32 D9 /6
		Fnstcw_m16,													// D9 /7
		Fstcw_m16,													// 9B D9 /7
		Fld_st0_sti,												// D9 C0+i
		Fxch_st0_sti,												// D9 C8+i
		Fnop,														// D9 D0
		Fstpnce_sti,												// D9 D8+i
		Fchs,														// D9 E0
		Fabs,														// D9 E1
		Ftst,														// D9 E4
		Fxam,														// D9 E5
		Fld1,														// D9 E8
		Fldl2t,														// D9 E9
		Fldl2e,														// D9 EA
		Fldpi,														// D9 EB
		Fldlg2,														// D9 EC
		Fldln2,														// D9 ED
		Fldz,														// D9 EE
		F2xm1,														// D9 F0
		Fyl2x,														// D9 F1
		Fptan,														// D9 F2
		Fpatan,														// D9 F3
		Fxtract,													// D9 F4
		Fprem1,														// D9 F5
		Fdecstp,													// D9 F6
		Fincstp,													// D9 F7
		Fprem,														// D9 F8
		Fyl2xp1,													// D9 F9
		Fsqrt,														// D9 FA
		Fsincos,													// D9 FB
		Frndint,													// D9 FC
		Fscale,														// D9 FD
		Fsin,														// D9 FE
		Fcos,														// D9 FF

		Fiadd_m32int,												// DA /0
		Fimul_m32int,												// DA /1
		Ficom_m32int,												// DA /2
		Ficomp_m32int,												// DA /3
		Fisub_m32int,												// DA /4
		Fisubr_m32int,												// DA /5
		Fidiv_m32int,												// DA /6
		Fidivr_m32int,												// DA /7
		Fcmovb_st0_sti,												// DA C0+i
		Fcmove_st0_sti,												// DA C8+i
		Fcmovbe_st0_sti,											// DA D0+i
		Fcmovu_st0_sti,												// DA D8+i
		Fucompp,													// DA E9

		Fild_m32int,												// DB /0
		Fisttp_m32int,												// DB /1
		Fist_m32int,												// DB /2
		Fistp_m32int,												// DB /3
		Fld_m80fp,													// DB /5
		Fstp_m80fp,													// DB /7
		Fcmovnb_st0_sti,											// DB C0+i
		Fcmovne_st0_sti,											// DB C8+i
		Fcmovnbe_st0_sti,											// DB D0+i
		Fcmovnu_st0_sti,											// DB D8+i
		Fneni,														// DB E0
		Feni,														// 9B DB E0
		Fndisi,														// DB E1
		Fdisi,														// 9B DB E1
		Fnclex,														// DB E2
		Fclex,														// 9B DB E2
		Fninit,														// DB E3
		Finit,														// 9B DB E3
		Fnsetpm,													// DB E4
		Fsetpm,														// 9B DB E4
		Frstpm,														// DB E5
		Fucomi_st0_sti,												// DB E8+i
		Fcomi_st0_sti,												// DB F0+i

		Fadd_m64fp,													// DC /0
		Fmul_m64fp,													// DC /1
		Fcom_m64fp,													// DC /2
		Fcomp_m64fp,												// DC /3
		Fsub_m64fp,													// DC /4
		Fsubr_m64fp,												// DC /5
		Fdiv_m64fp,													// DC /6
		Fdivr_m64fp,												// DC /7
		Fadd_sti_st0,												// DC C0+i
		Fmul_sti_st0,												// DC C8+i
		Fcom_st0_sti_DCD0,											// DC D0+i
		Fcomp_st0_sti_DCD8,											// DC D8+i
		Fsubr_sti_st0,												// DC E0+i
		Fsub_sti_st0,												// DC E8+i
		Fdivr_sti_st0,												// DC F0+i
		Fdiv_sti_st0,												// DC F8+i

		Fld_m64fp,													// DD /0
		Fisttp_m64fp,												// DD /1
		Fst_m64fp,													// DD /2
		Fstp_m64fp,													// DD /3
		Frstor_m94byte,												// o16 DD /4
		Frstor_m108byte,											// o32 DD /4
		Fnsave_m94byte,												// o16 DD /6
		Fsave_m94byte,												// 9B o16 DD /6
		Fnsave_m108byte,											// o32 DD /6
		Fsave_m108byte,												// 9B o32 DD /6
		Fnstsw_m16,													// DD /7
		Fstsw_m16,													// 9B DD /7
		Ffree_sti,													// DD C0+i
		Fxch_st0_sti_DDC8,											// DD C8+i
		Fst_sti,													// DD D0+i
		Fstp_sti,													// DD D8+i
		Fucom_st0_sti,												// DD E0+i
		Fucomp_st0_sti,												// DD E8+i

		Fiadd_m16int,												// DE /0
		Fimul_m16int,												// DE /1
		Ficom_m16int,												// DE /2
		Ficomp_m16int,												// DE /3
		Fisub_m16int,												// DE /4
		Fisubr_m16int,												// DE /5
		Fidiv_m16int,												// DE /6
		Fidivr_m16int,												// DE /7
		Faddp_sti_st0,												// DE C0+i
		Fmulp_sti_st0,												// DE C8+i
		Fcomp_st0_sti_DED0,											// DE D0+i
		Fcompp,														// DE D9
		Fsubrp_sti_st0,												// DE E0+i
		Fsubp_sti_st0,												// DE E8+i
		Fdivrp_sti_st0,												// DE F0+i
		Fdivp_sti_st0,												// DE F8+i

		Fild_m16int,												// DF /0
		Fisttp_m16int,												// DF /1
		Fist_m16int,												// DF /2
		Fistp_m16int,												// DF /3
		Fbld_m80bcd,												// DF /4
		Fild_m64int,												// DF /5
		Fbstp_m80bcd,												// DF /6
		Fistp_m64int,												// DF /7
		Ffreep_sti,													// DF C0+i
		Fxch_st0_sti_DFC8,											// DF C8+i
		Fstp_sti_DFD0,												// DF D0+i
		Fstp_sti_DFD8,												// DF D8+i
		Fnstsw_AX,													// DF E0
		Fstsw_AX,													// 9B DF E0
		Fstdw_AX,													// DF E1
		Fstsg_AX,													// DF E2
		Fucomip_st0_sti,											// DF E8+i
		Fcomip_st0_sti,												// DF F0+i

		Loopne_rel8_16_CX,											// a16 o16 E0
		Loopne_rel8_32_CX,											// a16 o32 E0
		Loopne_rel8_16_ECX,											// a32 o16 E0
		Loopne_rel8_32_ECX,											// a32 o32 E0
		Loopne_rel8_64_ECX,											// a32 E0
		Loopne_rel8_16_RCX,											// a64 o16 E0
		Loopne_rel8_64_RCX,											// a64 E0
		Loope_rel8_16_CX,											// a16 o16 E1
		Loope_rel8_32_CX,											// a16 o32 E1
		Loope_rel8_16_ECX,											// a32 o16 E1
		Loope_rel8_32_ECX,											// a32 o32 E1
		Loope_rel8_64_ECX,											// a32 E1
		Loope_rel8_16_RCX,											// a64 o16 E1
		Loope_rel8_64_RCX,											// a64 E1
		Loop_rel8_16_CX,											// a16 o16 E2
		Loop_rel8_32_CX,											// a16 o32 E2
		Loop_rel8_16_ECX,											// a32 o16 E2
		Loop_rel8_32_ECX,											// a32 o32 E2
		Loop_rel8_64_ECX,											// a32 E2
		Loop_rel8_16_RCX,											// a64 o16 E2
		Loop_rel8_64_RCX,											// a64 E2
		Jcxz_rel8_16,												// a16 o16 E3
		Jcxz_rel8_32,												// a16 o32 E3
		Jecxz_rel8_16,												// a32 o16 E3
		Jecxz_rel8_32,												// a32 o32 E3
		Jecxz_rel8_64,												// a32 E3
		Jrcxz_rel8_16,												// a64 o16 E3
		Jrcxz_rel8_64,												// a64 E3
		In_AL_imm8,													// E4
		In_AX_imm8,													// o16 E5
		In_EAX_imm8,												// o32 E5
		Out_imm8_AL,												// E6
		Out_imm8_AX,												// o16 E7
		Out_imm8_EAX,												// o32 E7

		Call_rel16,													// o16 E8
		Call_rel32_32,												// o32 E8
		Call_rel32_64,												// E8
		Jmp_rel16,													// o16 E9
		Jmp_rel32_32,												// o32 E9
		Jmp_rel32_64,												// E9
		Jmp_ptr1616,												// o16 EA
		Jmp_ptr3216,												// o32 EA
		Jmp_rel8_16,												// o16 EB
		Jmp_rel8_32,												// o32 EB
		Jmp_rel8_64,												// EB
		In_AL_DX,													// EC
		In_AX_DX,													// o16 ED
		In_EAX_DX,													// o32 ED
		Out_DX_AL,													// EE
		Out_DX_AX,													// o16 EF
		Out_DX_EAX,													// o32 EF

		Int1,														// F1
		Hlt,														// F4
		Cmc,														// F5
		Test_rm8_imm8,												// F6 /0
		Test_rm8_imm8_F6r1,											// F6 /1
		Not_rm8,													// F6 /2
		Neg_rm8,													// F6 /3
		Mul_rm8,													// F6 /4
		Imul_rm8,													// F6 /5
		Div_rm8,													// F6 /6
		Idiv_rm8,													// F6 /7
		Test_rm16_imm16,											// o16 F7 /0
		Test_rm32_imm32,											// o32 F7 /0
		Test_rm64_imm32,											// REX.W F7 /0
		Test_rm16_imm16_F7r1,										// o16 F7 /1
		Test_rm32_imm32_F7r1,										// o32 F7 /1
		Test_rm64_imm32_F7r1,										// REX.W F7 /1
		Not_rm16,													// o16 F7 /2
		Not_rm32,													// o32 F7 /2
		Not_rm64,													// REX.W F7 /2
		Neg_rm16,													// o16 F7 /3
		Neg_rm32,													// o32 F7 /3
		Neg_rm64,													// REX.W F7 /3
		Mul_rm16,													// o16 F7 /4
		Mul_rm32,													// o32 F7 /4
		Mul_rm64,													// REX.W F7 /4
		Imul_rm16,													// o16 F7 /5
		Imul_rm32,													// o32 F7 /5
		Imul_rm64,													// REX.W F7 /5
		Div_rm16,													// o16 F7 /6
		Div_rm32,													// o32 F7 /6
		Div_rm64,													// REX.W F7 /6
		Idiv_rm16,													// o16 F7 /7
		Idiv_rm32,													// o32 F7 /7
		Idiv_rm64,													// REX.W F7 /7

		Clc,														// F8
		Stc,														// F9
		Cli,														// FA
		Sti,														// FB
		Cld,														// FC
		Std,														// FD
		Inc_rm8,													// FE /0
		Dec_rm8,													// FE /1
		Inc_rm16,													// o16 FF /0
		Inc_rm32,													// o32 FF /0
		Inc_rm64,													// REX.W FF /0
		Dec_rm16,													// o16 FF /1
		Dec_rm32,													// o32 FF /1
		Dec_rm64,													// REX.W FF /1
		Call_rm16,													// o16 FF /2
		Call_rm32,													// o32 FF /2
		Call_rm64,													// FF /2
		Call_m1616,													// o16 FF /3
		Call_m3216,													// o32 FF /3
		Call_m6416,													// REX.W FF /3
		Jmp_rm16,													// o16 FF /4
		Jmp_rm32,													// o32 FF /4
		Jmp_rm64,													// FF /4
		Jmp_m1616,													// o16 FF /5
		Jmp_m3216,													// o32 FF /5
		Jmp_m6416,													// REX.W FF /5
		Push_rm16,													// o16 FF /6
		Push_rm32,													// o32 FF /6
		Push_rm64,													// REX.W FF /6

		// 0Fxx opcodes

		Sldt_rm16,													// o16 0F00 /0
		Sldt_r32m16,												// o32 0F00 /0
		Sldt_r64m16,												// REX.W 0F00 /0
		Str_rm16,													// o16 0F00 /1
		Str_r32m16,													// o32 0F00 /1
		Str_r64m16,													// REX.W 0F00 /1
		Lldt_rm16,													// o16 0F00 /2
		Lldt_r32m16,												// o32 0F00 /2
		Lldt_r64m16,												// REX.W 0F00 /2
		Ltr_rm16,													// o16 0F00 /3
		Ltr_r32m16,													// o32 0F00 /3
		Ltr_r64m16,													// REX.W 0F00 /3
		Verr_rm16,													// o16 0F00 /4
		Verr_r32m16,												// o32 0F00 /4
		Verr_r64m16,												// REX.W 0F00 /4
		Verw_rm16,													// o16 0F00 /5
		Verw_r32m16,												// o32 0F00 /5
		Verw_r64m16,												// REX.W 0F00 /5
		Jmpe_rm16,													// o16 0F00 /6
		Jmpe_rm32,													// o32 0F00 /6
		Sgdt_m40,													// o16 0F01 /0
		Sgdt_m48,													// o32 0F01 /0
		Sgdt_m80,													// 0F01 /0
		Sidt_m40,													// o16 0F01 /1
		Sidt_m48,													// o32 0F01 /1
		Sidt_m80,													// 0F01 /1
		Lgdt_m40,													// o16 0F01 /2
		Lgdt_m48,													// o32 0F01 /2
		Lgdt_m80,													// 0F01 /2
		Lidt_m40,													// o16 0F01 /3
		Lidt_m48,													// o32 0F01 /3
		Lidt_m80,													// 0F01 /3
		Smsw_rm16,													// o16 0F01 /4
		Smsw_r32m16,												// o32 0F01 /4
		Smsw_r64m16,												// REX.W 0F01 /4
		Rstorssp_m64,												// F3 0F01 /5
		Lmsw_rm16,													// o16 0F01 /6
		Lmsw_r32m16,												// o32 0F01 /6
		Lmsw_r64m16,												// REX.W 0F01 /6
		Invlpg_m,													// 0F01 /7
		Enclv,														// 0F01 C0
		Vmcall,														// 0F01 C1
		Vmlaunch,													// 0F01 C2
		Vmresume,													// 0F01 C3
		Vmxoff,														// 0F01 C4
		Pconfig,													// 0F01 C5
		Monitorw,													// a16 0F01 C8
		Monitord,													// a32 0F01 C8
		Monitorq,													// a64 0F01 C8
		Mwait,														// 0F01 C9
		Clac,														// 0F01 CA
		Stac,														// 0F01 CB
		Encls,														// 0F01 CF
		Xgetbv,														// 0F01 D0
		Xsetbv,														// 0F01 D1
		Vmfunc,														// 0F01 D4
		Xend,														// 0F01 D5
		Xtest,														// 0F01 D6
		Enclu,														// 0F01 D7
		Vmrunw,														// a16 0F01 D8
		Vmrund,														// a32 0F01 D8
		Vmrunq,														// a64 0F01 D8
		Vmmcall,													// 0F01 D9
		Vmloadw,													// a16 0F01 DA
		Vmloadd,													// a32 0F01 DA
		Vmloadq,													// a64 0F01 DA
		Vmsavew,													// a16 0F01 DB
		Vmsaved,													// a32 0F01 DB
		Vmsaveq,													// a64 0F01 DB
		Stgi,														// 0F01 DC
		Clgi,														// 0F01 DD
		Skinit,														// 0F01 DE
		Invlpgaw,													// a16 0F01 DF
		Invlpgad,													// a32 0F01 DF
		Invlpgaq,													// a64 0F01 DF
		Setssbsy,													// F3 0F01 E8
		Saveprevssp,												// F3 0F01 EA
		Rdpkru,														// 0F01 EE
		Wrpkru,														// 0F01 EF
		Swapgs,														// 0F01 F8
		Rdtscp,														// 0F01 F9
		Monitorxw,													// a16 0F01 FA
		Monitorxd,													// a32 0F01 FA
		Monitorxq,													// a64 0F01 FA
		Mwaitx,														// 0F01 FB
		Clzerow,													// a16 0F01 FC
		Clzerod,													// a32 0F01 FC
		Clzeroq,													// a64 0F01 FC
		Lar_r16_rm16,												// o16 0F02
		Lar_r32_rm32,												// o32 0F02
		Lar_r64_rm64,												// REX.W 0F02
		Lsl_r16_rm16,												// o16 0F03
		Lsl_r32_rm32,												// o32 0F03
		Lsl_r64_rm64,												// REX.W 0F03
		Loadallreset286,											// 0F04
		Loadall286,													// 0F05
		Syscall,													// 0F05
		Clts,														// 0F06
		Loadall386,													// 0F07
		Sysretd,													// 0F07
		Sysretq,													// REX.W 0F07

		Invd,														// 0F08
		Wbinvd,														// 0F09
		Wbnoinvd,													// F3 0F09
		Cflsh,														// 0F0A
		Cl1invmb,													// 0F0A
		Ud2,														// 0F0B
		ReservedNop_rm16_r16_0F0D,									// o16 0F0D
		ReservedNop_rm32_r32_0F0D,									// o16 0F0D
		ReservedNop_rm64_r64_0F0D,									// REX.W 0F0D
		Prefetch_m8,												// 0F0D /0
		Prefetchw_m8,												// 0F0D /1
		Prefetchwt1_m8,												// 0F0D /2
		Prefetch_m8_r3,												// 0F0D /3
		Prefetch_m8_r4,												// 0F0D /4
		Prefetch_m8_r5,												// 0F0D /5
		Prefetch_m8_r6,												// 0F0D /6
		Prefetch_m8_r7,												// 0F0D /7
		Femms,														// 0F0E

		Umov_rm8_r8,												// 0F10
		Umov_rm16_r16,												// o16 0F11
		Umov_rm32_r32,												// o32 0F11
		Umov_r8_rm8,												// 0F12
		Umov_r16_rm16,												// o16 0F13
		Umov_r32_rm32,												// o32 0F13

		Movups_xmm_xmmm128,											// 0F10
		VEX_Vmovups_xmm_xmmm128,									// VEX.128.0F.WIG 10
		VEX_Vmovups_ymm_ymmm256,									// VEX.256.0F.WIG 10
		EVEX_Vmovups_xmm_k1z_xmmm128,								// EVEX.128.0F.W0 10
		EVEX_Vmovups_ymm_k1z_ymmm256,								// EVEX.256.0F.W0 10
		EVEX_Vmovups_zmm_k1z_zmmm512,								// EVEX.512.0F.W0 10

		Movupd_xmm_xmmm128,											// 66 0F10
		VEX_Vmovupd_xmm_xmmm128,									// VEX.128.66.0F.WIG 10
		VEX_Vmovupd_ymm_ymmm256,									// VEX.256.66.0F.WIG 10
		EVEX_Vmovupd_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 10
		EVEX_Vmovupd_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 10
		EVEX_Vmovupd_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 10

		Movss_xmm_xmmm32,											// F3 0F10
		VEX_Vmovss_xmm_xmm_xmm,										// VEX.LIG.F3.0F.WIG 10
		VEX_Vmovss_xmm_m32,											// VEX.LIG.F3.0F.WIG 10
		EVEX_Vmovss_xmm_k1z_xmm_xmm,								// EVEX.LIG.F3.0F.W0 10
		EVEX_Vmovss_xmm_k1z_m32,									// EVEX.LIG.F3.0F.W0 10

		Movsd_xmm_xmmm64,											// F2 0F10
		VEX_Vmovsd_xmm_xmm_xmm,										// VEX.LIG.F2.0F.WIG 10
		VEX_Vmovsd_xmm_m64,											// VEX.LIG.F2.0F.WIG 10
		EVEX_Vmovsd_xmm_k1z_xmm_xmm,								// EVEX.LIG.F2.0F.W1 10
		EVEX_Vmovsd_xmm_k1z_m64,									// EVEX.LIG.F2.0F.W1 10

		Movups_xmmm128_xmm,											// 0F11
		VEX_Vmovups_xmmm128_xmm,									// VEX.128.0F.WIG 11
		VEX_Vmovups_ymmm256_ymm,									// VEX.256.0F.WIG 11
		EVEX_Vmovups_xmmm128_k1z_xmm,								// EVEX.128.0F.W0 11
		EVEX_Vmovups_ymmm256_k1z_ymm,								// EVEX.256.0F.W0 11
		EVEX_Vmovups_zmmm512_k1z_zmm,								// EVEX.512.0F.W0 11

		Movupd_xmmm128_xmm,											// 66 0F11
		VEX_Vmovupd_xmmm128_xmm,									// VEX.128.66.0F.WIG 11
		VEX_Vmovupd_ymmm256_ymm,									// VEX.256.66.0F.WIG 11
		EVEX_Vmovupd_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 11
		EVEX_Vmovupd_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 11
		EVEX_Vmovupd_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 11

		Movss_xmmm32_xmm,											// F3 0F11
		VEX_Vmovss_xmm_xmm_xmm_0F11,								// VEX.LIG.F3.0F.WIG 11
		VEX_Vmovss_m32_xmm,											// VEX.LIG.F3.0F.WIG 11
		EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11,							// EVEX.LIG.F3.0F.W0 11
		EVEX_Vmovss_m32_k1_xmm,										// EVEX.LIG.F3.0F.W0 11

		Movsd_xmmm64_xmm,											// F2 0F11
		VEX_Vmovsd_xmm_xmm_xmm_0F11,								// VEX.LIG.F2.0F.WIG 11
		VEX_Vmovsd_m64_xmm,											// VEX.LIG.F2.0F.WIG 11
		EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11,							// EVEX.LIG.F2.0F.W1 11
		EVEX_Vmovsd_m64_k1_xmm,										// EVEX.LIG.F2.0F.W1 11

		Movhlps_xmm_xmm,											// 0F12
		Movlps_xmm_m64,												// 0F12
		VEX_Vmovhlps_xmm_xmm_xmm,									// VEX.128.0F.WIG 12
		VEX_Vmovlps_xmm_xmm_m64,									// VEX.128.0F.WIG 12
		EVEX_Vmovhlps_xmm_xmm_xmm,									// EVEX.128.0F.W0 12
		EVEX_Vmovlps_xmm_xmm_m64,									// EVEX.128.0F.W0 12

		Movlpd_xmm_m64,												// 66 0F12
		VEX_Vmovlpd_xmm_xmm_m64,									// VEX.128.66.0F.WIG 12
		EVEX_Vmovlpd_xmm_xmm_m64,									// EVEX.128.66.0F.W1 12

		Movsldup_xmm_xmmm128,										// F3 0F12
		VEX_Vmovsldup_xmm_xmmm128,									// VEX.128.F3.0F.WIG 12
		VEX_Vmovsldup_ymm_ymmm256,									// VEX.256.F3.0F.WIG 12
		EVEX_Vmovsldup_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 12
		EVEX_Vmovsldup_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 12
		EVEX_Vmovsldup_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 12

		Movddup_xmm_xmmm64,											// F2 0F12
		VEX_Vmovddup_xmm_xmmm64,									// VEX.128.F2.0F.WIG 12
		VEX_Vmovddup_ymm_ymmm256,									// VEX.256.F2.0F.WIG 12
		EVEX_Vmovddup_xmm_k1z_xmmm64,								// EVEX.128.F2.0F.W1 12
		EVEX_Vmovddup_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W1 12
		EVEX_Vmovddup_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W1 12

		Movlps_m64_xmm,												// 0F13
		VEX_Vmovlps_m64_xmm,										// VEX.128.0F.WIG 13
		EVEX_Vmovlps_m64_xmm,										// EVEX.128.0F.W0 13

		Movlpd_m64_xmm,												// 66 0F13
		VEX_Vmovlpd_m64_xmm,										// VEX.128.66.0F.WIG 13
		EVEX_Vmovlpd_m64_xmm,										// EVEX.128.66.0F.W1 13

		Unpcklps_xmm_xmmm128,										// 0F14
		VEX_Vunpcklps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 14
		VEX_Vunpcklps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 14
		EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 14
		EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 14
		EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 14

		Unpcklpd_xmm_xmmm128,										// 66 0F14
		VEX_Vunpcklpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 14
		VEX_Vunpcklpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 14
		EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 14
		EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 14
		EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 14

		Unpckhps_xmm_xmmm128,										// 0F15
		VEX_Vunpckhps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 15
		VEX_Vunpckhps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 15
		EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 15
		EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 15
		EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 15

		Unpckhpd_xmm_xmmm128,										// 66 0F15
		VEX_Vunpckhpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 15
		VEX_Vunpckhpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 15
		EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 15
		EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 15
		EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 15

		Movlhps_xmm_xmm,											// 0F16
		VEX_Vmovlhps_xmm_xmm_xmm,									// VEX.128.0F.WIG 16
		EVEX_Vmovlhps_xmm_xmm_xmm,									// EVEX.128.0F.W0 16

		Movhps_xmm_m64,												// 0F16
		VEX_Vmovhps_xmm_xmm_m64,									// VEX.128.0F.WIG 16
		EVEX_Vmovhps_xmm_xmm_m64,									// EVEX.128.0F.W0 16

		Movhpd_xmm_m64,												// 66 0F16
		VEX_Vmovhpd_xmm_xmm_m64,									// VEX.128.66.0F.WIG 16
		EVEX_Vmovhpd_xmm_xmm_m64,									// EVEX.128.66.0F.W1 16

		Movshdup_xmm_xmmm128,										// F3 0F16
		VEX_Vmovshdup_xmm_xmmm128,									// VEX.128.F3.0F.WIG 16
		VEX_Vmovshdup_ymm_ymmm256,									// VEX.256.F3.0F.WIG 16
		EVEX_Vmovshdup_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 16
		EVEX_Vmovshdup_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 16
		EVEX_Vmovshdup_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 16

		Movhps_m64_xmm,												// 0F17
		VEX_Vmovhps_m64_xmm,										// VEX.128.0F.WIG 17
		EVEX_Vmovhps_m64_xmm,										// EVEX.128.0F.W0 17

		Movhpd_m64_xmm,												// 66 0F17
		VEX_Vmovhpd_m64_xmm,										// VEX.128.66.0F.WIG 17
		EVEX_Vmovhpd_m64_xmm,										// EVEX.128.66.0F.W1 17

		ReservedNop_rm16_r16_0F18,									// o16 0F18
		ReservedNop_rm32_r32_0F18,									// o32 0F18
		ReservedNop_rm64_r64_0F18,									// REX.W 0F18
		ReservedNop_rm16_r16_0F19,									// o16 0F19
		ReservedNop_rm32_r32_0F19,									// o32 0F19
		ReservedNop_rm64_r64_0F19,									// REX.W 0F19
		ReservedNop_rm16_r16_0F1A,									// o16 0F1A
		ReservedNop_rm32_r32_0F1A,									// o32 0F1A
		ReservedNop_rm64_r64_0F1A,									// REX.W 0F1A
		ReservedNop_rm16_r16_0F1B,									// o16 0F1B
		ReservedNop_rm32_r32_0F1B,									// o32 0F1B
		ReservedNop_rm64_r64_0F1B,									// REX.W 0F1B
		ReservedNop_rm16_r16_0F1C,									// o16 0F1C
		ReservedNop_rm32_r32_0F1C,									// o32 0F1C
		ReservedNop_rm64_r64_0F1C,									// REX.W 0F1C
		ReservedNop_rm16_r16_0F1D,									// o16 0F1D
		ReservedNop_rm32_r32_0F1D,									// o32 0F1D
		ReservedNop_rm64_r64_0F1D,									// REX.W 0F1D
		ReservedNop_rm16_r16_0F1E,									// o16 0F1E
		ReservedNop_rm32_r32_0F1E,									// o32 0F1E
		ReservedNop_rm64_r64_0F1E,									// REX.W 0F1E
		ReservedNop_rm16_r16_0F1F,									// o16 0F1F
		ReservedNop_rm32_r32_0F1F,									// o32 0F1F
		ReservedNop_rm64_r64_0F1F,									// REX.W 0F1F

		Prefetchnta_m8,												// 0F18 /0
		Prefetcht0_m8,												// 0F18 /1
		Prefetcht1_m8,												// 0F18 /2
		Prefetcht2_m8,												// 0F18 /3

		Bndldx_bnd_mib,												// 0F1A
		Bndmov_bnd_bndm64,											// 66 0F1A
		Bndmov_bnd_bndm128,											// 66 0F1A
		Bndcl_bnd_rm32,												// F3 0F1A
		Bndcl_bnd_rm64,												// F3 0F1A
		Bndcu_bnd_rm32,												// F2 0F1A
		Bndcu_bnd_rm64,												// F2 0F1A

		Bndstx_mib_bnd,												// 0F1B
		Bndmov_bndm64_bnd,											// 66 0F1B
		Bndmov_bndm128_bnd,											// 66 0F1B
		Bndmk_bnd_m32,												// F3 0F1B
		Bndmk_bnd_m64,												// F3 0F1B
		Bndcn_bnd_rm32,												// F2 0F1B
		Bndcn_bnd_rm64,												// F2 0F1B

		Cldemote_m8,												// 0F1C /0

		Rdsspd_r32,													// F3 0F1E /1
		Rdsspq_r64,													// F3 REX.W 0F1E /1
		Endbr64,													// F3 0F1E FA
		Endbr32,													// F3 0F1E FB

		Nop_rm16,													// o16 0F1F /0
		Nop_rm32,													// o32 0F1F /0
		Nop_rm64,													// REX.W 0F1F /0

		Mov_r32_cr,													// 0F20
		Mov_r64_cr,													// 0F20
		Mov_r32_dr,													// 0F21
		Mov_r64_dr,													// 0F21
		Mov_cr_r32,													// 0F22
		Mov_cr_r64,													// 0F22
		Mov_dr_r32,													// 0F23
		Mov_dr_r64,													// 0F23
		Mov_r32_tr,													// 0F24
		Mov_tr_r32,													// 0F26

		Movaps_xmm_xmmm128,											// 0F28
		VEX_Vmovaps_xmm_xmmm128,									// VEX.128.0F.WIG 28
		VEX_Vmovaps_ymm_ymmm256,									// VEX.256.0F.WIG 28
		EVEX_Vmovaps_xmm_k1z_xmmm128,								// EVEX.128.0F.W0 28
		EVEX_Vmovaps_ymm_k1z_ymmm256,								// EVEX.256.0F.W0 28
		EVEX_Vmovaps_zmm_k1z_zmmm512,								// EVEX.512.0F.W0 28

		Movapd_xmm_xmmm128,											// 66 0F28
		VEX_Vmovapd_xmm_xmmm128,									// VEX.128.66.0F.WIG 28
		VEX_Vmovapd_ymm_ymmm256,									// VEX.256.66.0F.WIG 28
		EVEX_Vmovapd_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 28
		EVEX_Vmovapd_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 28
		EVEX_Vmovapd_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 28

		Movaps_xmmm128_xmm,											// 0F29
		VEX_Vmovaps_xmmm128_xmm,									// VEX.128.0F.WIG 29
		VEX_Vmovaps_ymmm256_ymm,									// VEX.256.0F.WIG 29
		EVEX_Vmovaps_xmmm128_k1z_xmm,								// EVEX.128.0F.W0 29
		EVEX_Vmovaps_ymmm256_k1z_ymm,								// EVEX.256.0F.W0 29
		EVEX_Vmovaps_zmmm512_k1z_zmm,								// EVEX.512.0F.W0 29

		Movapd_xmmm128_xmm,											// 66 0F29
		VEX_Vmovapd_xmmm128_xmm,									// VEX.128.66.0F.WIG 29
		VEX_Vmovapd_ymmm256_ymm,									// VEX.256.66.0F.WIG 29
		EVEX_Vmovapd_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 29
		EVEX_Vmovapd_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 29
		EVEX_Vmovapd_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 29

		Cvtpi2ps_xmm_mmm64,											// 0F2A

		Cvtpi2pd_xmm_mmm64,											// 66 0F2A

		Cvtsi2ss_xmm_rm32,											// F3 0F2A
		Cvtsi2ss_xmm_rm64,											// F3 REX.W 0F2A
		VEX_Vcvtsi2ss_xmm_xmm_rm32,									// VEX.LIG.F3.0F.W0 2A
		VEX_Vcvtsi2ss_xmm_xmm_rm64,									// VEX.LIG.F3.0F.W1 2A
		EVEX_Vcvtsi2ss_xmm_xmm_rm32_er,								// EVEX.LIG.F3.0F.W0 2A
		EVEX_Vcvtsi2ss_xmm_xmm_rm64_er,								// EVEX.LIG.F3.0F.W1 2A

		Cvtsi2sd_xmm_rm32,											// F2 0F2A
		Cvtsi2sd_xmm_rm64,											// F2 REX.W 0F2A
		VEX_Vcvtsi2sd_xmm_xmm_rm32,									// VEX.LIG.F2.0F.W0 2A
		VEX_Vcvtsi2sd_xmm_xmm_rm64,									// VEX.LIG.F2.0F.W1 2A
		EVEX_Vcvtsi2sd_xmm_xmm_rm32,								// EVEX.LIG.F2.0F.W0 2A
		EVEX_Vcvtsi2sd_xmm_xmm_rm64_er,								// EVEX.LIG.F2.0F.W1 2A

		Movntps_m128_xmm,											// 0F2B
		VEX_Vmovntps_m128_xmm,										// VEX.128.0F.WIG 2B
		VEX_Vmovntps_m256_ymm,										// VEX.256.0F.WIG 2B
		EVEX_Vmovntps_m128_xmm,										// EVEX.128.0F.W0 2B
		EVEX_Vmovntps_m256_ymm,										// EVEX.256.0F.W0 2B
		EVEX_Vmovntps_m512_zmm,										// EVEX.512.0F.W0 2B

		Movntpd_m128_xmm,											// 66 0F2B
		VEX_Vmovntpd_m128_xmm,										// VEX.128.66.0F.WIG 2B
		VEX_Vmovntpd_m256_ymm,										// VEX.256.66.0F.WIG 2B
		EVEX_Vmovntpd_m128_xmm,										// EVEX.128.66.0F.W1 2B
		EVEX_Vmovntpd_m256_ymm,										// EVEX.256.66.0F.W1 2B
		EVEX_Vmovntpd_m512_zmm,										// EVEX.512.66.0F.W1 2B

		Movntss_m32_xmm,											// F3 0F2B

		Movntsd_m64_xmm,											// F2 0F2B

		Cvttps2pi_mm_xmmm64,										// 0F2C

		Cvttpd2pi_mm_xmmm128,										// 66 0F2C

		Cvttss2si_r32_xmmm32,										// F3 0F2C
		Cvttss2si_r64_xmmm32,										// F3 REX.W 0F2C
		VEX_Vcvttss2si_r32_xmmm32,									// VEX.LIG.F3.0F.W0 2C
		VEX_Vcvttss2si_r64_xmmm32,									// VEX.LIG.F3.0F.W1 2C
		EVEX_Vcvttss2si_r32_xmmm32_sae,								// EVEX.LIG.F3.0F.W0 2C
		EVEX_Vcvttss2si_r64_xmmm32_sae,								// EVEX.LIG.F3.0F.W1 2C

		Cvttsd2si_r32_xmmm64,										// F2 0F2C
		Cvttsd2si_r64_xmmm64,										// F2 REX.W 0F2C
		VEX_Vcvttsd2si_r32_xmmm64,									// VEX.LIG.F2.0F.W0 2C
		VEX_Vcvttsd2si_r64_xmmm64,									// VEX.LIG.F2.0F.W1 2C
		EVEX_Vcvttsd2si_r32_xmmm64_sae,								// EVEX.LIG.F2.0F.W0 2C
		EVEX_Vcvttsd2si_r64_xmmm64_sae,								// EVEX.LIG.F2.0F.W1 2C

		Cvtps2pi_mm_xmmm64,											// 0F2D

		Cvtpd2pi_mm_xmmm128,										// 66 0F2D

		Cvtss2si_r32_xmmm32,										// F3 0F2D
		Cvtss2si_r64_xmmm32,										// F3 REX.W 0F2D
		VEX_Vcvtss2si_r32_xmmm32,									// VEX.LIG.F3.0F.W0 2D
		VEX_Vcvtss2si_r64_xmmm32,									// VEX.LIG.F3.0F.W1 2D
		EVEX_Vcvtss2si_r32_xmmm32_er,								// EVEX.LIG.F3.0F.W0 2D
		EVEX_Vcvtss2si_r64_xmmm32_er,								// EVEX.LIG.F3.0F.W1 2D

		Cvtsd2si_r32_xmmm64,										// F2 0F2D
		Cvtsd2si_r64_xmmm64,										// F2 REX.W 0F2D
		VEX_Vcvtsd2si_r32_xmmm64,									// VEX.LIG.F2.0F.W0 2D
		VEX_Vcvtsd2si_r64_xmmm64,									// VEX.LIG.F2.0F.W1 2D
		EVEX_Vcvtsd2si_r32_xmmm64_er,								// EVEX.LIG.F2.0F.W0 2D
		EVEX_Vcvtsd2si_r64_xmmm64_er,								// EVEX.LIG.F2.0F.W1 2D

		Ucomiss_xmm_xmmm32,											// 0F2E
		VEX_Vucomiss_xmm_xmmm32,									// VEX.LIG.0F.WIG 2E
		EVEX_Vucomiss_xmm_xmmm32_sae,								// EVEX.LIG.0F.W0 2E

		Ucomisd_xmm_xmmm64,											// 66 0F2E
		VEX_Vucomisd_xmm_xmmm64,									// VEX.LIG.66.0F.WIG 2E
		EVEX_Vucomisd_xmm_xmmm64_sae,								// EVEX.LIG.66.0F.W1 2E

		Comiss_xmm_xmmm32,											// 0F2F

		Comisd_xmm_xmmm64,											// 66 0F2F
		VEX_Vcomiss_xmm_xmmm32,										// VEX.LIG.0F.WIG 2F
		VEX_Vcomisd_xmm_xmmm64,										// VEX.LIG.66.0F.WIG 2F
		EVEX_Vcomiss_xmm_xmmm32_sae,								// EVEX.LIG.0F.W0 2F
		EVEX_Vcomisd_xmm_xmmm64_sae,								// EVEX.LIG.66.0F.W1 2F

		Wrmsr,														// 0F30
		Rdtsc,														// 0F31
		Rdmsr,														// 0F32
		Rdpmc,														// 0F33
		Wrecr,														// 0F34
		Sysenter,													// 0F34
		Sysexitd,													// 0F35
		Sysexitq,													// REX.W 0F35
		Rdecr,														// 0F36
		Getsec,														// 0F37

		Cmovo_r16_rm16,												// o16 0F40
		Cmovo_r32_rm32,												// o32 0F40
		Cmovo_r64_rm64,												// REX.W 0F40
		Cmovno_r16_rm16,											// o16 0F41
		Cmovno_r32_rm32,											// o32 0F41
		Cmovno_r64_rm64,											// REX.W 0F41
		Cmovb_r16_rm16,												// o16 0F42
		Cmovb_r32_rm32,												// o32 0F42
		Cmovb_r64_rm64,												// REX.W 0F42
		Cmovae_r16_rm16,											// o16 0F43
		Cmovae_r32_rm32,											// o32 0F43
		Cmovae_r64_rm64,											// REX.W 0F43
		Cmove_r16_rm16,												// o16 0F44
		Cmove_r32_rm32,												// o32 0F44
		Cmove_r64_rm64,												// REX.W 0F44
		Cmovne_r16_rm16,											// o16 0F45
		Cmovne_r32_rm32,											// o32 0F45
		Cmovne_r64_rm64,											// REX.W 0F45
		Cmovbe_r16_rm16,											// o16 0F46
		Cmovbe_r32_rm32,											// o32 0F46
		Cmovbe_r64_rm64,											// REX.W 0F46
		Cmova_r16_rm16,												// o16 0F47
		Cmova_r32_rm32,												// o32 0F47
		Cmova_r64_rm64,												// REX.W 0F47

		Cmovs_r16_rm16,												// o16 0F48
		Cmovs_r32_rm32,												// o32 0F48
		Cmovs_r64_rm64,												// REX.W 0F48
		Cmovns_r16_rm16,											// o16 0F49
		Cmovns_r32_rm32,											// o32 0F49
		Cmovns_r64_rm64,											// REX.W 0F49
		Cmovp_r16_rm16,												// o16 0F4A
		Cmovp_r32_rm32,												// o32 0F4A
		Cmovp_r64_rm64,												// REX.W 0F4A
		Cmovnp_r16_rm16,											// o16 0F4B
		Cmovnp_r32_rm32,											// o32 0F4B
		Cmovnp_r64_rm64,											// REX.W 0F4B
		Cmovl_r16_rm16,												// o16 0F4C
		Cmovl_r32_rm32,												// o32 0F4C
		Cmovl_r64_rm64,												// REX.W 0F4C
		Cmovge_r16_rm16,											// o16 0F4D
		Cmovge_r32_rm32,											// o32 0F4D
		Cmovge_r64_rm64,											// REX.W 0F4D
		Cmovle_r16_rm16,											// o16 0F4E
		Cmovle_r32_rm32,											// o32 0F4E
		Cmovle_r64_rm64,											// REX.W 0F4E
		Cmovg_r16_rm16,												// o16 0F4F
		Cmovg_r32_rm32,												// o32 0F4F
		Cmovg_r64_rm64,												// REX.W 0F4F

		VEX_Kandw_k_k_k,											// VEX.L1.0F.W0 41
		VEX_Kandq_k_k_k,											// VEX.L1.0F.W1 41

		VEX_Kandb_k_k_k,											// VEX.L1.66.0F.W0 41
		VEX_Kandd_k_k_k,											// VEX.L1.66.0F.W1 41

		VEX_Kandnw_k_k_k,											// VEX.L1.0F.W0 42
		VEX_Kandnq_k_k_k,											// VEX.L1.0F.W1 42

		VEX_Kandnb_k_k_k,											// VEX.L1.66.0F.W0 42
		VEX_Kandnd_k_k_k,											// VEX.L1.66.0F.W1 42

		VEX_Knotw_k_k,												// VEX.L0.0F.W0 44
		VEX_Knotq_k_k,												// VEX.L0.0F.W1 44

		VEX_Knotb_k_k,												// VEX.L0.66.0F.W0 44
		VEX_Knotd_k_k,												// VEX.L0.66.0F.W1 44

		VEX_Korw_k_k_k,												// VEX.L1.0F.W0 45
		VEX_Korq_k_k_k,												// VEX.L1.0F.W1 45

		VEX_Korb_k_k_k,												// VEX.L1.66.0F.W0 45
		VEX_Kord_k_k_k,												// VEX.L1.66.0F.W1 45

		VEX_Kxnorw_k_k_k,											// VEX.L1.0F.W0 46
		VEX_Kxnorq_k_k_k,											// VEX.L1.0F.W1 46

		VEX_Kxnorb_k_k_k,											// VEX.L1.66.0F.W0 46
		VEX_Kxnord_k_k_k,											// VEX.L1.66.0F.W1 46

		VEX_Kxorw_k_k_k,											// VEX.L1.0F.W0 47
		VEX_Kxorq_k_k_k,											// VEX.L1.0F.W1 47

		VEX_Kxorb_k_k_k,											// VEX.L1.66.0F.W0 47
		VEX_Kxord_k_k_k,											// VEX.L1.66.0F.W1 47

		VEX_Kaddw_k_k_k,											// VEX.L1.0F.W0 4A
		VEX_Kaddq_k_k_k,											// VEX.L1.0F.W1 4A

		VEX_Kaddb_k_k_k,											// VEX.L1.66.0F.W0 4A
		VEX_Kaddd_k_k_k,											// VEX.L1.66.0F.W1 4A

		VEX_Kunpckwd_k_k_k,											// VEX.L1.0F.W0 4B
		VEX_Kunpckdq_k_k_k,											// VEX.L1.0F.W1 4B

		VEX_Kunpckbw_k_k_k,											// VEX.L1.66.0F.W0 4B

		Movmskps_r32_xmm,											// 0F50
		Movmskps_r64_xmm,											// REX.W 0F50
		VEX_Vmovmskps_r32_xmm,										// VEX.128.0F.W0 50
		VEX_Vmovmskps_r64_xmm,										// VEX.128.0F.W1 50
		VEX_Vmovmskps_r32_ymm,										// VEX.256.0F.W0 50
		VEX_Vmovmskps_r64_ymm,										// VEX.256.0F.W1 50

		Movmskpd_r32_xmm,											// 66 0F50
		Movmskpd_r64_xmm,											// 66 REX.W 0F50
		VEX_Vmovmskpd_r32_xmm,										// VEX.128.66.0F.W0 50
		VEX_Vmovmskpd_r64_xmm,										// VEX.128.66.0F.W1 50
		VEX_Vmovmskpd_r32_ymm,										// VEX.256.66.0F.W0 50
		VEX_Vmovmskpd_r64_ymm,										// VEX.256.66.0F.W1 50

		Sqrtps_xmm_xmmm128,											// 0F51
		VEX_Vsqrtps_xmm_xmmm128,									// VEX.128.0F.WIG 51
		VEX_Vsqrtps_ymm_ymmm256,									// VEX.256.0F.WIG 51
		EVEX_Vsqrtps_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 51
		EVEX_Vsqrtps_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 51
		EVEX_Vsqrtps_zmm_k1z_zmmm512b32_er,							// EVEX.512.0F.W0 51

		Sqrtpd_xmm_xmmm128,											// 66 0F51
		VEX_Vsqrtpd_xmm_xmmm128,									// VEX.128.66.0F.WIG 51
		VEX_Vsqrtpd_ymm_ymmm256,									// VEX.256.66.0F.WIG 51
		EVEX_Vsqrtpd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 51
		EVEX_Vsqrtpd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 51
		EVEX_Vsqrtpd_zmm_k1z_zmmm512b64_er,							// EVEX.512.66.0F.W1 51

		Sqrtss_xmm_xmmm32,											// F3 0F51
		VEX_Vsqrtss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 51
		EVEX_Vsqrtss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 51

		Sqrtsd_xmm_xmmm64,											// F2 0F51
		VEX_Vsqrtsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 51
		EVEX_Vsqrtsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 51

		Rsqrtps_xmm_xmmm128,										// 0F52
		VEX_Vrsqrtps_xmm_xmmm128,									// VEX.128.0F.WIG 52
		VEX_Vrsqrtps_ymm_ymmm256,									// VEX.256.0F.WIG 52

		Rsqrtss_xmm_xmmm32,											// F3 0F52
		VEX_Vrsqrtss_xmm_xmm_xmmm32,								// VEX.LIG.F3.0F.WIG 52

		Rcpps_xmm_xmmm128,											// 0F53
		VEX_Vrcpps_xmm_xmmm128,										// VEX.128.0F.WIG 53
		VEX_Vrcpps_ymm_ymmm256,										// VEX.256.0F.WIG 53

		Rcpss_xmm_xmmm32,											// F3 0F53
		VEX_Vrcpss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 53

		Andps_xmm_xmmm128,											// 0F54
		VEX_Vandps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 54
		VEX_Vandps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 54
		EVEX_Vandps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 54
		EVEX_Vandps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 54
		EVEX_Vandps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 54

		Andpd_xmm_xmmm128,											// 66 0F54
		VEX_Vandpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 54
		VEX_Vandpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 54
		EVEX_Vandpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 54
		EVEX_Vandpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 54
		EVEX_Vandpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 54

		Andnps_xmm_xmmm128,											// 0F55
		VEX_Vandnps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 55
		VEX_Vandnps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 55
		EVEX_Vandnps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 55
		EVEX_Vandnps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 55
		EVEX_Vandnps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 55

		Andnpd_xmm_xmmm128,											// 66 0F55
		VEX_Vandnpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 55
		VEX_Vandnpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 55
		EVEX_Vandnpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 55
		EVEX_Vandnpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 55
		EVEX_Vandnpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 55

		Orps_xmm_xmmm128,											// 0F56
		VEX_Vorps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 56
		VEX_Vorps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 56
		EVEX_Vorps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 56
		EVEX_Vorps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 56
		EVEX_Vorps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 56

		Orpd_xmm_xmmm128,											// 66 0F56
		VEX_Vorpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 56
		VEX_Vorpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 56
		EVEX_Vorpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 56
		EVEX_Vorpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 56
		EVEX_Vorpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 56

		Xorps_xmm_xmmm128,											// 0F57
		VEX_Vxorps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 57
		VEX_Vxorps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 57
		EVEX_Vxorps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 57
		EVEX_Vxorps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 57
		EVEX_Vxorps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 57

		Xorpd_xmm_xmmm128,											// 66 0F57
		VEX_Vxorpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 57
		VEX_Vxorpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 57
		EVEX_Vxorpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 57
		EVEX_Vxorpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 57
		EVEX_Vxorpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 57

		Addps_xmm_xmmm128,											// 0F58
		VEX_Vaddps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 58
		VEX_Vaddps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 58
		EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 58
		EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 58
		EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 58

		Addpd_xmm_xmmm128,											// 66 0F58
		VEX_Vaddpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 58
		VEX_Vaddpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 58
		EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 58
		EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 58
		EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 58

		Addss_xmm_xmmm32,											// F3 0F58
		VEX_Vaddss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 58
		EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 58

		Addsd_xmm_xmmm64,											// F2 0F58
		VEX_Vaddsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 58
		EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 58

		Mulps_xmm_xmmm128,											// 0F59
		VEX_Vmulps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 59
		VEX_Vmulps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 59
		EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 59
		EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 59
		EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 59

		Mulpd_xmm_xmmm128,											// 66 0F59
		VEX_Vmulpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 59
		VEX_Vmulpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 59
		EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 59
		EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 59
		EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 59

		Mulss_xmm_xmmm32,											// F3 0F59
		VEX_Vmulss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 59
		EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 59

		Mulsd_xmm_xmmm64,											// F2 0F59
		VEX_Vmulsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 59
		EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 59

		Cvtps2pd_xmm_xmmm64,										// 0F5A
		VEX_Vcvtps2pd_xmm_xmmm64,									// VEX.128.0F.WIG 5A
		VEX_Vcvtps2pd_ymm_xmmm128,									// VEX.256.0F.WIG 5A
		EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32,							// EVEX.128.0F.W0 5A
		EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32,							// EVEX.256.0F.W0 5A
		EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae,						// EVEX.512.0F.W0 5A

		Cvtpd2ps_xmm_xmmm128,										// 66 0F5A
		VEX_Vcvtpd2ps_xmm_xmmm128,									// VEX.128.66.0F.WIG 5A
		VEX_Vcvtpd2ps_xmm_ymmm256,									// VEX.256.66.0F.WIG 5A
		EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 5A
		EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 5A
		EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 5A

		Cvtss2sd_xmm_xmmm32,										// F3 0F5A
		VEX_Vcvtss2sd_xmm_xmm_xmmm32,								// VEX.LIG.F3.0F.WIG 5A
		EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.F3.0F.W0 5A

		Cvtsd2ss_xmm_xmmm64,										// F2 0F5A
		VEX_Vcvtsd2ss_xmm_xmm_xmmm64,								// VEX.LIG.F2.0F.WIG 5A
		EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.F2.0F.W1 5A

		Cvtdq2ps_xmm_xmmm128,										// 0F5B
		VEX_Vcvtdq2ps_xmm_xmmm128,									// VEX.128.0F.WIG 5B
		VEX_Vcvtdq2ps_ymm_ymmm256,									// VEX.256.0F.WIG 5B
		EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 5B
		EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 5B
		EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er,						// EVEX.512.0F.W0 5B
		EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64,							// EVEX.128.0F.W1 5B
		EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64,							// EVEX.256.0F.W1 5B
		EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.0F.W1 5B

		Cvtps2dq_xmm_xmmm128,										// 66 0F5B
		VEX_Vcvtps2dq_xmm_xmmm128,									// VEX.128.66.0F.WIG 5B
		VEX_Vcvtps2dq_ymm_ymmm256,									// VEX.256.66.0F.WIG 5B
		EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F.W0 5B
		EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F.W0 5B
		EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er,						// EVEX.512.66.0F.W0 5B

		Cvttps2dq_xmm_xmmm128,										// F3 0F5B
		VEX_Vcvttps2dq_xmm_xmmm128,									// VEX.128.F3.0F.WIG 5B
		VEX_Vcvttps2dq_ymm_ymmm256,									// VEX.256.F3.0F.WIG 5B
		EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32,							// EVEX.128.F3.0F.W0 5B
		EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32,							// EVEX.256.F3.0F.W0 5B
		EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae,						// EVEX.512.F3.0F.W0 5B

		Subps_xmm_xmmm128,											// 0F5C
		VEX_Vsubps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5C
		VEX_Vsubps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5C
		EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5C
		EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5C
		EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 5C

		Subpd_xmm_xmmm128,											// 66 0F5C
		VEX_Vsubpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5C
		VEX_Vsubpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5C
		EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5C
		EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5C
		EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 5C

		Subss_xmm_xmmm32,											// F3 0F5C
		VEX_Vsubss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5C
		EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 5C

		Subsd_xmm_xmmm64,											// F2 0F5C
		VEX_Vsubsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5C
		EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 5C

		Minps_xmm_xmmm128,											// 0F5D
		VEX_Vminps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5D
		VEX_Vminps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5D
		EVEX_Vminps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5D
		EVEX_Vminps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5D
		EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae,						// EVEX.512.0F.W0 5D

		Minpd_xmm_xmmm128,											// 66 0F5D
		VEX_Vminpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5D
		VEX_Vminpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5D
		EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5D
		EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5D
		EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae,						// EVEX.512.66.0F.W1 5D

		Minss_xmm_xmmm32,											// F3 0F5D
		VEX_Vminss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5D
		EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 5D

		Minsd_xmm_xmmm64,											// F2 0F5D
		VEX_Vminsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5D
		EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 5D

		Divps_xmm_xmmm128,											// 0F5E
		VEX_Vdivps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5E
		VEX_Vdivps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5E
		EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5E
		EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5E
		EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 5E

		Divpd_xmm_xmmm128,											// 66 0F5E
		VEX_Vdivpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5E
		VEX_Vdivpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5E
		EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5E
		EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5E
		EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 5E

		Divss_xmm_xmmm32,											// F3 0F5E
		VEX_Vdivss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5E
		EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 5E

		Divsd_xmm_xmmm64,											// F2 0F5E
		VEX_Vdivsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5E
		EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 5E

		Maxps_xmm_xmmm128,											// 0F5F
		VEX_Vmaxps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5F
		VEX_Vmaxps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5F
		EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5F
		EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5F
		EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae,						// EVEX.512.0F.W0 5F

		Maxpd_xmm_xmmm128,											// 66 0F5F
		VEX_Vmaxpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5F
		VEX_Vmaxpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5F
		EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5F
		EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5F
		EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae,						// EVEX.512.66.0F.W1 5F

		Maxss_xmm_xmmm32,											// F3 0F5F
		VEX_Vmaxss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5F
		EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 5F

		Maxsd_xmm_xmmm64,											// F2 0F5F
		VEX_Vmaxsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5F
		EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 5F

		Punpcklbw_mm_mmm32,											// 0F60

		Punpcklbw_xmm_xmmm128,										// 66 0F60
		VEX_Vpunpcklbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 60
		VEX_Vpunpcklbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 60
		EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 60
		EVEX_Vpunpcklbw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 60
		EVEX_Vpunpcklbw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 60

		Punpcklwd_mm_mmm32,											// 0F61

		Punpcklwd_xmm_xmmm128,										// 66 0F61
		VEX_Vpunpcklwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 61
		VEX_Vpunpcklwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 61
		EVEX_Vpunpcklwd_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 61
		EVEX_Vpunpcklwd_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 61
		EVEX_Vpunpcklwd_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 61

		Punpckldq_mm_mmm32,											// 0F62

		Punpckldq_xmm_xmmm128,										// 66 0F62
		VEX_Vpunpckldq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 62
		VEX_Vpunpckldq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 62
		EVEX_Vpunpckldq_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 62
		EVEX_Vpunpckldq_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 62
		EVEX_Vpunpckldq_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 62

		Packsswb_mm_mmm64,											// 0F63

		Packsswb_xmm_xmmm128,										// 66 0F63
		VEX_Vpacksswb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 63
		VEX_Vpacksswb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 63
		EVEX_Vpacksswb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG 63
		EVEX_Vpacksswb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG 63
		EVEX_Vpacksswb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG 63

		Pcmpgtb_mm_mmm64,											// 0F64

		Pcmpgtb_xmm_xmmm128,										// 66 0F64
		VEX_Vpcmpgtb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 64
		VEX_Vpcmpgtb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 64
		EVEX_Vpcmpgtb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 64
		EVEX_Vpcmpgtb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 64
		EVEX_Vpcmpgtb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 64

		Pcmpgtw_mm_mmm64,											// 0F65

		Pcmpgtw_xmm_xmmm128,										// 66 0F65
		VEX_Vpcmpgtw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 65
		VEX_Vpcmpgtw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 65
		EVEX_Vpcmpgtw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 65
		EVEX_Vpcmpgtw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 65
		EVEX_Vpcmpgtw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 65

		Pcmpgtd_mm_mmm64,											// 0F66

		Pcmpgtd_xmm_xmmm128,										// 66 0F66
		VEX_Vpcmpgtd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 66
		VEX_Vpcmpgtd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 66
		EVEX_Vpcmpgtd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 66
		EVEX_Vpcmpgtd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 66
		EVEX_Vpcmpgtd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 66

		Packuswb_mm_mmm64,											// 0F67

		Packuswb_xmm_xmmm128,										// 66 0F67
		VEX_Vpackuswb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 67
		VEX_Vpackuswb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 67
		EVEX_Vpackuswb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG 67
		EVEX_Vpackuswb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG 67
		EVEX_Vpackuswb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG 67

		Punpckhbw_mm_mmm64,											// 0F68

		Punpckhbw_xmm_xmmm128,										// 66 0F68
		VEX_Vpunpckhbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 68
		VEX_Vpunpckhbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 68
		EVEX_Vpunpckhbw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 68
		EVEX_Vpunpckhbw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 68
		EVEX_Vpunpckhbw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 68

		Punpckhwd_mm_mmm64,											// 0F69

		Punpckhwd_xmm_xmmm128,										// 66 0F69
		VEX_Vpunpckhwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 69
		VEX_Vpunpckhwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 69
		EVEX_Vpunpckhwd_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 69
		EVEX_Vpunpckhwd_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 69
		EVEX_Vpunpckhwd_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 69

		Punpckhdq_mm_mmm64,											// 0F6A

		Punpckhdq_xmm_xmmm128,										// 66 0F6A
		VEX_Vpunpckhdq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 6A
		VEX_Vpunpckhdq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 6A
		EVEX_Vpunpckhdq_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 6A
		EVEX_Vpunpckhdq_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 6A
		EVEX_Vpunpckhdq_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 6A

		Packssdw_mm_mmm64,											// 0F6B

		Packssdw_xmm_xmmm128,										// 66 0F6B
		VEX_Vpackssdw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 6B
		VEX_Vpackssdw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 6B
		EVEX_Vpackssdw_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 6B
		EVEX_Vpackssdw_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 6B
		EVEX_Vpackssdw_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 6B

		Punpcklqdq_xmm_xmmm128,										// 66 0F6C
		VEX_Vpunpcklqdq_xmm_xmm_xmmm128,							// VEX.128.66.0F.WIG 6C
		VEX_Vpunpcklqdq_ymm_ymm_ymmm256,							// VEX.256.66.0F.WIG 6C
		EVEX_Vpunpcklqdq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F.W1 6C
		EVEX_Vpunpcklqdq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F.W1 6C
		EVEX_Vpunpcklqdq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F.W1 6C

		Punpckhqdq_xmm_xmmm128,										// 66 0F6D
		VEX_Vpunpckhqdq_xmm_xmm_xmmm128,							// VEX.128.66.0F.WIG 6D
		VEX_Vpunpckhqdq_ymm_ymm_ymmm256,							// VEX.256.66.0F.WIG 6D
		EVEX_Vpunpckhqdq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F.W1 6D
		EVEX_Vpunpckhqdq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F.W1 6D
		EVEX_Vpunpckhqdq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F.W1 6D

		Movd_mm_rm32,												// 0F6E
		Movq_mm_rm64,												// REX.W 0F6E

		Movd_xmm_rm32,												// 66 0F6E
		Movq_xmm_rm64,												// 66 REX.W 0F6E
		VEX_Vmovd_xmm_rm32,											// VEX.128.66.0F.W0 6E
		VEX_Vmovq_xmm_rm64,											// VEX.128.66.0F.W1 6E
		EVEX_Vmovd_xmm_rm32,										// EVEX.128.66.0F.W0 6E
		EVEX_Vmovq_xmm_rm64,										// EVEX.128.66.0F.W1 6E

		Movq_mm_mmm64,												// 0F6F

		Movdqa_xmm_xmmm128,											// 66 0F6F
		VEX_Vmovdqa_xmm_xmmm128,									// VEX.128.66.0F.WIG 6F
		VEX_Vmovdqa_ymm_ymmm256,									// VEX.256.66.0F.WIG 6F
		EVEX_Vmovdqa32_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W0 6F
		EVEX_Vmovdqa32_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W0 6F
		EVEX_Vmovdqa32_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W0 6F
		EVEX_Vmovdqa64_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 6F
		EVEX_Vmovdqa64_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 6F
		EVEX_Vmovdqa64_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 6F

		Movdqu_xmm_xmmm128,											// F3 0F6F
		VEX_Vmovdqu_xmm_xmmm128,									// VEX.128.F3.0F.WIG 6F
		VEX_Vmovdqu_ymm_ymmm256,									// VEX.256.F3.0F.WIG 6F
		EVEX_Vmovdqu32_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 6F
		EVEX_Vmovdqu32_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 6F
		EVEX_Vmovdqu32_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 6F
		EVEX_Vmovdqu64_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W1 6F
		EVEX_Vmovdqu64_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W1 6F
		EVEX_Vmovdqu64_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W1 6F

		EVEX_Vmovdqu8_xmm_k1z_xmmm128,								// EVEX.128.F2.0F.W0 6F
		EVEX_Vmovdqu8_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W0 6F
		EVEX_Vmovdqu8_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W0 6F
		EVEX_Vmovdqu16_xmm_k1z_xmmm128,								// EVEX.128.F2.0F.W1 6F
		EVEX_Vmovdqu16_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W1 6F
		EVEX_Vmovdqu16_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W1 6F

		Pshufw_mm_mmm64_imm8,										// 0F70

		Pshufd_xmm_xmmm128_imm8,									// 66 0F70
		VEX_Vpshufd_xmm_xmmm128_imm8,								// VEX.128.66.0F.WIG 70
		VEX_Vpshufd_ymm_ymmm256_imm8,								// VEX.256.66.0F.WIG 70
		EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 70
		EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 70
		EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 70

		Pshufhw_xmm_xmmm128_imm8,									// F3 0F70
		VEX_Vpshufhw_xmm_xmmm128_imm8,								// VEX.128.F3.0F.WIG 70
		VEX_Vpshufhw_ymm_ymmm256_imm8,								// VEX.256.F3.0F.WIG 70
		EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8,							// EVEX.128.F3.0F.WIG 70
		EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8,							// EVEX.256.F3.0F.WIG 70
		EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8,							// EVEX.512.F3.0F.WIG 70

		Pshuflw_xmm_xmmm128_imm8,									// F2 0F70
		VEX_Vpshuflw_xmm_xmmm128_imm8,								// VEX.128.F2.0F.WIG 70
		VEX_Vpshuflw_ymm_ymmm256_imm8,								// VEX.256.F2.0F.WIG 70
		EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8,							// EVEX.128.F2.0F.WIG 70
		EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8,							// EVEX.256.F2.0F.WIG 70
		EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8,							// EVEX.512.F2.0F.WIG 70

		Psrlw_mm_imm8,												// 0F71 /2

		Psrlw_xmm_imm8,												// 66 0F71 /2
		VEX_Vpsrlw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /2
		VEX_Vpsrlw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /2
		EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /2
		EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /2
		EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /2

		Psraw_mm_imm8,												// 0F71 /4

		Psraw_xmm_imm8,												// 66 0F71 /4
		VEX_Vpsraw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /4
		VEX_Vpsraw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /4
		EVEX_Vpsraw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /4
		EVEX_Vpsraw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /4
		EVEX_Vpsraw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /4

		Psllw_mm_imm8,												// 0F71 /6

		Psllw_xmm_imm8,												// 66 0F71 /6
		VEX_Vpsllw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /6
		VEX_Vpsllw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /6
		EVEX_Vpsllw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /6
		EVEX_Vpsllw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /6
		EVEX_Vpsllw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /6

		EVEX_Vprord_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /0
		EVEX_Vprord_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /0
		EVEX_Vprord_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /0
		EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /0
		EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /0
		EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /0

		EVEX_Vprold_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /1
		EVEX_Vprold_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /1
		EVEX_Vprold_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /1
		EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /1
		EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /1
		EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /1

		Psrld_mm_imm8,												// 0F72 /2

		Psrld_xmm_imm8,												// 66 0F72 /2
		VEX_Vpsrld_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /2
		VEX_Vpsrld_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /2
		EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /2
		EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /2
		EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /2

		Psrad_mm_imm8,												// 0F72 /4

		Psrad_xmm_imm8,												// 66 0F72 /4
		VEX_Vpsrad_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /4
		VEX_Vpsrad_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /4
		EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /4
		EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /4
		EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /4
		EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /4
		EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /4
		EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /4

		Pslld_mm_imm8,												// 0F72 /6

		Pslld_xmm_imm8,												// 66 0F72 /6
		VEX_Vpslld_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /6
		VEX_Vpslld_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /6
		EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /6
		EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /6
		EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /6

		Psrlq_mm_imm8,												// 0F73 /2

		Psrlq_xmm_imm8,												// 66 0F73 /2
		VEX_Vpsrlq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /2
		VEX_Vpsrlq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /2
		EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 73 /2
		EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 73 /2
		EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 73 /2

		Psrldq_xmm_imm8,											// 66 0F73 /3
		VEX_Vpsrldq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /3
		VEX_Vpsrldq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /3
		EVEX_Vpsrldq_xmm_xmmm128_imm8,								// EVEX.128.66.0F.WIG 73 /3
		EVEX_Vpsrldq_ymm_ymmm256_imm8,								// EVEX.256.66.0F.WIG 73 /3
		EVEX_Vpsrldq_zmm_zmmm512_imm8,								// EVEX.512.66.0F.WIG 73 /3

		Psllq_mm_imm8,												// 0F73 /6

		Psllq_xmm_imm8,												// 66 0F73 /6
		VEX_Vpsllq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /6
		VEX_Vpsllq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /6
		EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 73 /6
		EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 73 /6
		EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 73 /6

		Pslldq_xmm_imm8,											// 66 0F73 /7
		VEX_Vpslldq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /7
		VEX_Vpslldq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /7
		EVEX_Vpslldq_xmm_xmmm128_imm8,								// EVEX.128.66.0F.WIG 73 /7
		EVEX_Vpslldq_ymm_ymmm256_imm8,								// EVEX.256.66.0F.WIG 73 /7
		EVEX_Vpslldq_zmm_zmmm512_imm8,								// EVEX.512.66.0F.WIG 73 /7

		Pcmpeqb_mm_mmm64,											// 0F74

		Pcmpeqb_xmm_xmmm128,										// 66 0F74
		VEX_Vpcmpeqb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 74
		VEX_Vpcmpeqb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 74
		EVEX_Vpcmpeqb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 74
		EVEX_Vpcmpeqb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 74
		EVEX_Vpcmpeqb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 74

		Pcmpeqw_mm_mmm64,											// 0F75

		Pcmpeqw_xmm_xmmm128,										// 66 0F75
		VEX_Vpcmpeqw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 75
		VEX_Vpcmpeqw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 75
		EVEX_Vpcmpeqw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 75
		EVEX_Vpcmpeqw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 75
		EVEX_Vpcmpeqw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 75

		Pcmpeqd_mm_mmm64,											// 0F76

		Pcmpeqd_xmm_xmmm128,										// 66 0F76
		VEX_Vpcmpeqd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 76
		VEX_Vpcmpeqd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 76
		EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 76
		EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 76
		EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 76

		Emms,														// 0F77
		VEX_Vzeroupper,												// VEX.128.0F.WIG 77
		VEX_Vzeroall,												// VEX.256.0F.WIG 77

		Vmread_rm32_r32,											// 0F78
		Vmread_rm64_r64,											// REX.W 0F78
		EVEX_Vcvttps2udq_xmm_k1z_xmmm128b32,						// EVEX.128.0F.W0 78
		EVEX_Vcvttps2udq_ymm_k1z_ymmm256b32,						// EVEX.256.0F.W0 78
		EVEX_Vcvttps2udq_zmm_k1z_zmmm512b32_sae,					// EVEX.512.0F.W0 78
		EVEX_Vcvttpd2udq_xmm_k1z_xmmm128b64,						// EVEX.128.0F.W1 78
		EVEX_Vcvttpd2udq_xmm_k1z_ymmm256b64,						// EVEX.256.0F.W1 78
		EVEX_Vcvttpd2udq_ymm_k1z_zmmm512b64_sae,					// EVEX.512.0F.W1 78

		Extrq_xmm_imm8_imm8,										// 66 0F78 /0
		EVEX_Vcvttps2uqq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 78
		EVEX_Vcvttps2uqq_ymm_k1z_xmmm128b32,						// EVEX.256.66.0F.W0 78
		EVEX_Vcvttps2uqq_zmm_k1z_ymmm256b32_sae,					// EVEX.512.66.0F.W0 78
		EVEX_Vcvttpd2uqq_xmm_k1z_xmmm128b64,						// EVEX.128.66.0F.W1 78
		EVEX_Vcvttpd2uqq_ymm_k1z_ymmm256b64,						// EVEX.256.66.0F.W1 78
		EVEX_Vcvttpd2uqq_zmm_k1z_zmmm512b64_sae,					// EVEX.512.66.0F.W1 78

		EVEX_Vcvttss2usi_r32_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 78
		EVEX_Vcvttss2usi_r64_xmmm32_sae,							// EVEX.LIG.F3.0F.W1 78

		Insertq_xmm_xmm_imm8_imm8,									// F2 0F78
		EVEX_Vcvttsd2usi_r32_xmmm64_sae,							// EVEX.LIG.F2.0F.W0 78
		EVEX_Vcvttsd2usi_r64_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 78

		Vmwrite_r32_rm32,											// 0F79
		Vmwrite_r64_rm64,											// REX.W 0F79
		EVEX_Vcvtps2udq_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 79
		EVEX_Vcvtps2udq_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 79
		EVEX_Vcvtps2udq_zmm_k1z_zmmm512b32_er,						// EVEX.512.0F.W0 79
		EVEX_Vcvtpd2udq_xmm_k1z_xmmm128b64,							// EVEX.128.0F.W1 79
		EVEX_Vcvtpd2udq_xmm_k1z_ymmm256b64,							// EVEX.256.0F.W1 79
		EVEX_Vcvtpd2udq_ymm_k1z_zmmm512b64_er,						// EVEX.512.0F.W1 79

		Extrq_xmm_xmm,												// 66 0F79
		EVEX_Vcvtps2uqq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 79
		EVEX_Vcvtps2uqq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 79
		EVEX_Vcvtps2uqq_zmm_k1z_ymmm256b32_er,						// EVEX.512.66.0F.W0 79
		EVEX_Vcvtpd2uqq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 79
		EVEX_Vcvtpd2uqq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 79
		EVEX_Vcvtpd2uqq_zmm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 79

		EVEX_Vcvtss2usi_r32_xmmm32_er,								// EVEX.LIG.F3.0F.W0 79
		EVEX_Vcvtss2usi_r64_xmmm32_er,								// EVEX.LIG.F3.0F.W1 79

		Insertq_xmm_xmm,											// F2 0F79
		EVEX_Vcvtsd2usi_r32_xmmm64_er,								// EVEX.LIG.F2.0F.W0 79
		EVEX_Vcvtsd2usi_r64_xmmm64_er,								// EVEX.LIG.F2.0F.W1 79

		EVEX_Vcvttps2qq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 7A
		EVEX_Vcvttps2qq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 7A
		EVEX_Vcvttps2qq_zmm_k1z_ymmm256b32_sae,						// EVEX.512.66.0F.W0 7A
		EVEX_Vcvttpd2qq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 7A
		EVEX_Vcvttpd2qq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 7A
		EVEX_Vcvttpd2qq_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F.W1 7A

		EVEX_Vcvtudq2pd_xmm_k1z_xmmm64b32,							// EVEX.128.F3.0F.W0 7A
		EVEX_Vcvtudq2pd_ymm_k1z_xmmm128b32,							// EVEX.256.F3.0F.W0 7A
		EVEX_Vcvtudq2pd_zmm_k1z_ymmm256b32,							// EVEX.512.F3.0F.W0 7A
		EVEX_Vcvtuqq2pd_xmm_k1z_xmmm128b64,							// EVEX.128.F3.0F.W1 7A
		EVEX_Vcvtuqq2pd_ymm_k1z_ymmm256b64,							// EVEX.256.F3.0F.W1 7A
		EVEX_Vcvtuqq2pd_zmm_k1z_zmmm512b64_er,						// EVEX.512.F3.0F.W1 7A

		EVEX_Vcvtudq2ps_xmm_k1z_xmmm128b32,							// EVEX.128.F2.0F.W0 7A
		EVEX_Vcvtudq2ps_ymm_k1z_ymmm256b32,							// EVEX.256.F2.0F.W0 7A
		EVEX_Vcvtudq2ps_zmm_k1z_zmmm512b32_er,						// EVEX.512.F2.0F.W0 7A
		EVEX_Vcvtuqq2ps_xmm_k1z_xmmm128b64,							// EVEX.128.F2.0F.W1 7A
		EVEX_Vcvtuqq2ps_xmm_k1z_ymmm256b64,							// EVEX.256.F2.0F.W1 7A
		EVEX_Vcvtuqq2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.F2.0F.W1 7A

		EVEX_Vcvtps2qq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 7B
		EVEX_Vcvtps2qq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 7B
		EVEX_Vcvtps2qq_zmm_k1z_ymmm256b32_er,						// EVEX.512.66.0F.W0 7B
		EVEX_Vcvtpd2qq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 7B
		EVEX_Vcvtpd2qq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 7B
		EVEX_Vcvtpd2qq_zmm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 7B

		EVEX_Vcvtusi2ss_xmm_xmm_rm32_er,							// EVEX.LIG.F3.0F.W0 7B
		EVEX_Vcvtusi2ss_xmm_xmm_rm64_er,							// EVEX.LIG.F3.0F.W1 7B

		EVEX_Vcvtusi2sd_xmm_xmm_rm32,								// EVEX.LIG.F2.0F.W0 7B
		EVEX_Vcvtusi2sd_xmm_xmm_rm64_er,							// EVEX.LIG.F2.0F.W1 7B

		Haddpd_xmm_xmmm128,											// 66 0F7C
		VEX_Vhaddpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 7C
		VEX_Vhaddpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 7C

		Haddps_xmm_xmmm128,											// F2 0F7C
		VEX_Vhaddps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG 7C
		VEX_Vhaddps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG 7C

		Hsubpd_xmm_xmmm128,											// 66 0F7D
		VEX_Vhsubpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 7D
		VEX_Vhsubpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 7D

		Hsubps_xmm_xmmm128,											// F2 0F7D
		VEX_Vhsubps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG 7D
		VEX_Vhsubps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG 7D

		Movd_rm32_mm,												// 0F7E
		Movq_rm64_mm,												// REX.W 0F7E

		Movd_rm32_xmm,												// 66 0F7E
		Movq_rm64_xmm,												// 66 REX.W 0F7E
		VEX_Vmovd_rm32_xmm,											// VEX.128.66.0F.W0 7E
		VEX_Vmovq_rm64_xmm,											// VEX.128.66.0F.W1 7E
		EVEX_Vmovd_rm32_xmm,										// EVEX.128.66.0F.W0 7E
		EVEX_Vmovq_rm64_xmm,										// EVEX.128.66.0F.W1 7E

		Movq_xmm_xmmm64,											// F3 0F7E
		VEX_Vmovq_xmm_xmmm64,										// VEX.128.F3.0F.WIG 7E
		EVEX_Vmovq_xmm_xmmm64,										// EVEX.128.F3.0F.W1 7E

		Movq_mmm64_mm,												// 0F7F

		Movdqa_xmmm128_xmm,											// 66 0F7F
		VEX_Vmovdqa_xmmm128_xmm,									// VEX.128.66.0F.WIG 7F
		VEX_Vmovdqa_ymmm256_ymm,									// VEX.256.66.0F.WIG 7F
		EVEX_Vmovdqa32_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W0 7F
		EVEX_Vmovdqa32_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W0 7F
		EVEX_Vmovdqa32_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W0 7F
		EVEX_Vmovdqa64_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 7F
		EVEX_Vmovdqa64_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 7F
		EVEX_Vmovdqa64_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 7F

		Movdqu_xmmm128_xmm,											// F3 0F7F
		VEX_Vmovdqu_xmmm128_xmm,									// VEX.128.F3.0F.WIG 7F
		VEX_Vmovdqu_ymmm256_ymm,									// VEX.256.F3.0F.WIG 7F
		EVEX_Vmovdqu32_xmmm128_k1z_xmm,								// EVEX.128.F3.0F.W0 7F
		EVEX_Vmovdqu32_ymmm256_k1z_ymm,								// EVEX.256.F3.0F.W0 7F
		EVEX_Vmovdqu32_zmmm512_k1z_zmm,								// EVEX.512.F3.0F.W0 7F
		EVEX_Vmovdqu64_xmmm128_k1z_xmm,								// EVEX.128.F3.0F.W1 7F
		EVEX_Vmovdqu64_ymmm256_k1z_ymm,								// EVEX.256.F3.0F.W1 7F
		EVEX_Vmovdqu64_zmmm512_k1z_zmm,								// EVEX.512.F3.0F.W1 7F

		EVEX_Vmovdqu8_xmmm128_k1z_xmm,								// EVEX.128.F2.0F.W0 7F
		EVEX_Vmovdqu8_ymmm256_k1z_ymm,								// EVEX.256.F2.0F.W0 7F
		EVEX_Vmovdqu8_zmmm512_k1z_zmm,								// EVEX.512.F2.0F.W0 7F
		EVEX_Vmovdqu16_xmmm128_k1z_xmm,								// EVEX.128.F2.0F.W1 7F
		EVEX_Vmovdqu16_ymmm256_k1z_ymm,								// EVEX.256.F2.0F.W1 7F
		EVEX_Vmovdqu16_zmmm512_k1z_zmm,								// EVEX.512.F2.0F.W1 7F

		Jo_rel16,													// o16 0F80
		Jo_rel32_32,												// o32 0F80
		Jo_rel32_64,												// 0F80
		Jno_rel16,													// o16 0F81
		Jno_rel32_32,												// o32 0F81
		Jno_rel32_64,												// 0F81
		Jb_rel16,													// o16 0F82
		Jb_rel32_32,												// o32 0F82
		Jb_rel32_64,												// 0F82
		Jae_rel16,													// o16 0F83
		Jae_rel32_32,												// o32 0F83
		Jae_rel32_64,												// 0F83
		Je_rel16,													// o16 0F84
		Je_rel32_32,												// o32 0F84
		Je_rel32_64,												// 0F84
		Jne_rel16,													// o16 0F85
		Jne_rel32_32,												// o32 0F85
		Jne_rel32_64,												// 0F85
		Jbe_rel16,													// o16 0F86
		Jbe_rel32_32,												// o32 0F86
		Jbe_rel32_64,												// 0F86
		Ja_rel16,													// o16 0F87
		Ja_rel32_32,												// o32 0F87
		Ja_rel32_64,												// 0F87

		Js_rel16,													// o16 0F88
		Js_rel32_32,												// o32 0F88
		Js_rel32_64,												// 0F88
		Jns_rel16,													// o16 0F89
		Jns_rel32_32,												// o32 0F89
		Jns_rel32_64,												// 0F89
		Jp_rel16,													// o16 0F8A
		Jp_rel32_32,												// o32 0F8A
		Jp_rel32_64,												// 0F8A
		Jnp_rel16,													// o16 0F8B
		Jnp_rel32_32,												// o32 0F8B
		Jnp_rel32_64,												// 0F8B
		Jl_rel16,													// o16 0F8C
		Jl_rel32_32,												// o32 0F8C
		Jl_rel32_64,												// 0F8C
		Jge_rel16,													// o16 0F8D
		Jge_rel32_32,												// o32 0F8D
		Jge_rel32_64,												// 0F8D
		Jle_rel16,													// o16 0F8E
		Jle_rel32_32,												// o32 0F8E
		Jle_rel32_64,												// 0F8E
		Jg_rel16,													// o16 0F8F
		Jg_rel32_32,												// o32 0F8F
		Jg_rel32_64,												// 0F8F

		Seto_rm8,													// 0F90
		Setno_rm8,													// 0F91
		Setb_rm8,													// 0F92
		Setae_rm8,													// 0F93
		Sete_rm8,													// 0F94
		Setne_rm8,													// 0F95
		Setbe_rm8,													// 0F96
		Seta_rm8,													// 0F97

		Sets_rm8,													// 0F98
		Setns_rm8,													// 0F99
		Setp_rm8,													// 0F9A
		Setnp_rm8,													// 0F9B
		Setl_rm8,													// 0F9C
		Setge_rm8,													// 0F9D
		Setle_rm8,													// 0F9E
		Setg_rm8,													// 0F9F

		VEX_Kmovw_k_km16,											// VEX.L0.0F.W0 90
		VEX_Kmovq_k_km64,											// VEX.L0.0F.W1 90

		VEX_Kmovb_k_km8,											// VEX.L0.66.0F.W0 90
		VEX_Kmovd_k_km32,											// VEX.L0.66.0F.W1 90

		VEX_Kmovw_m16_k,											// VEX.L0.0F.W0 91
		VEX_Kmovq_m64_k,											// VEX.L0.0F.W1 91

		VEX_Kmovb_m8_k,												// VEX.L0.66.0F.W0 91
		VEX_Kmovd_m32_k,											// VEX.L0.66.0F.W1 91

		VEX_Kmovw_k_r32,											// VEX.L0.0F.W0 92

		VEX_Kmovb_k_r32,											// VEX.L0.66.0F.W0 92

		VEX_Kmovq_k_r64,											// VEX.L0.F2.0F.W1 92
		VEX_Kmovd_k_r32,											// VEX.L0.F2.0F.W0 92

		VEX_Kmovw_r32_k,											// VEX.L0.0F.W0 93

		VEX_Kmovb_r32_k,											// VEX.L0.66.0F.W0 93

		VEX_Kmovq_r64_k,											// VEX.L0.F2.0F.W1 93
		VEX_Kmovd_r32_k,											// VEX.L0.F2.0F.W0 93

		VEX_Kortestw_k_k,											// VEX.L0.0F.W0 98
		VEX_Kortestq_k_k,											// VEX.L0.0F.W1 98

		VEX_Kortestb_k_k,											// VEX.L0.66.0F.W0 98
		VEX_Kortestd_k_k,											// VEX.L0.66.0F.W1 98

		VEX_Ktestw_k_k,												// VEX.L0.0F.W0 99
		VEX_Ktestq_k_k,												// VEX.L0.0F.W1 99

		VEX_Ktestb_k_k,												// VEX.L0.66.0F.W0 99
		VEX_Ktestd_k_k,												// VEX.L0.66.0F.W1 99

		Pushw_FS,													// o16 0FA0
		Pushd_FS,													// o32 0FA0
		Pushq_FS,													// 0FA0
		Popw_FS,													// o16 0FA1
		Popd_FS,													// o32 0FA1
		Popq_FS,													// 0FA1
		Cpuid,														// 0FA2
		Bt_rm16_r16,												// o16 0FA3
		Bt_rm32_r32,												// o32 0FA3
		Bt_rm64_r64,												// REX.W 0FA3
		Shld_rm16_r16_imm8,											// o16 0FA4
		Shld_rm32_r32_imm8,											// o32 0FA4
		Shld_rm64_r64_imm8,											// REX.W 0FA4
		Shld_rm16_r16_CL,											// o16 0FA5
		Shld_rm32_r32_CL,											// o32 0FA5
		Shld_rm64_r64_CL,											// REX.W 0FA5
		Montmul_16,													// a16 0FA6 C0
		Montmul_32,													// a32 0FA6 C0
		Montmul_64,													// a64 0FA6 C0
		Xsha1_16,													// a16 0FA6 C8
		Xsha1_32,													// a32 0FA6 C8
		Xsha1_64,													// a64 0FA6 C8
		Xsha256_16,													// a16 0FA6 D0
		Xsha256_32,													// a32 0FA6 D0
		Xsha256_64,													// a64 0FA6 D0
		Xbts_r16_rm16,												// o16 0FA6
		Xbts_r32_rm32,												// o32 0FA6
		Xstore_16,													// a16 0FA7 C0
		Xstore_32,													// a32 0FA7 C0
		Xstore_64,													// a64 0FA7 C0
		XcryptEcb_16,												// a16 0FA7 C8
		XcryptEcb_32,												// a32 0FA7 C8
		XcryptEcb_64,												// a64 0FA7 C8
		XcryptCbc_16,												// a16 0FA7 D0
		XcryptCbc_32,												// a32 0FA7 D0
		XcryptCbc_64,												// a64 0FA7 D0
		XcryptCtr_16,												// a16 0FA7 D8
		XcryptCtr_32,												// a32 0FA7 D8
		XcryptCtr_64,												// a64 0FA7 D8
		XcryptCfb_16,												// a16 0FA7 E0
		XcryptCfb_32,												// a32 0FA7 E0
		XcryptCfb_64,												// a64 0FA7 E0
		XcryptOfb_16,												// a16 0FA7 E8
		XcryptOfb_32,												// a32 0FA7 E8
		XcryptOfb_64,												// a64 0FA7 E8
		Ibts_rm16_r16,												// o16 0FA7
		Ibts_rm32_r32,												// o32 0FA7
		Cmpxchg486_rm8_r8,											// 0FA6
		Cmpxchg486_rm16_r16,										// o16 0FA7
		Cmpxchg486_rm32_r32,										// o32 0FA7

		Pushw_GS,													// o16 0FA8
		Pushd_GS,													// o32 0FA8
		Pushq_GS,													// 0FA8
		Popw_GS,													// o16 0FA9
		Popd_GS,													// o32 0FA9
		Popq_GS,													// 0FA9
		Rsm,														// 0FAA
		Bts_rm16_r16,												// o16 0FAB
		Bts_rm32_r32,												// o32 0FAB
		Bts_rm64_r64,												// REX.W 0FAB
		Shrd_rm16_r16_imm8,											// o16 0FAC
		Shrd_rm32_r32_imm8,											// o32 0FAC
		Shrd_rm64_r64_imm8,											// REX.W 0FAC
		Shrd_rm16_r16_CL,											// o16 0FAD
		Shrd_rm32_r32_CL,											// o32 0FAD
		Shrd_rm64_r64_CL,											// REX.W 0FAD

		Zalloc_m256,												// 0FAE /0
		Fxsave_m512byte,											// 0FAE /0
		Fxsave64_m512byte,											// REX.W 0FAE /0
		Rdfsbase_r32,												// F3 0FAE /0
		Rdfsbase_r64,												// F3 REX.W 0FAE /0
		Fxrstor_m512byte,											// 0FAE /1
		Fxrstor64_m512byte,											// REX.W 0FAE /1
		Rdgsbase_r32,												// F3 0FAE /1
		Rdgsbase_r64,												// F3 REX.W 0FAE /1
		Ldmxcsr_m32,												// 0FAE /2
		Wrfsbase_r32,												// F3 0FAE /2
		Wrfsbase_r64,												// F3 REX.W 0FAE /2
		VEX_Vldmxcsr_m32,											// VEX.L0.0F.WIG AE /2
		Stmxcsr_m32,												// 0FAE /3
		Wrgsbase_r32,												// F3 0FAE /3
		Wrgsbase_r64,												// F3 REX.W 0FAE /3
		VEX_Vstmxcsr_m32,											// VEX.L0.0F.WIG AE /3
		Xsave_m,													// 0FAE /4
		Xsave64_m,													// REX.W 0FAE /4
		Ptwrite_rm32,												// F3 0FAE /4
		Ptwrite_rm64,												// F3 REX.W 0FAE /4
		Xrstor_m,													// 0FAE /5
		Xrstor64_m,													// REX.W 0FAE /5
		Incsspd_r32,												// F3 0FAE /5
		Incsspq_r64,												// F3 REX.W 0FAE /5
		Xsaveopt_m,													// 0FAE /6
		Xsaveopt64_m,												// REX.W 0FAE /6
		Clwb_m8,													// 66 0FAE /6
		Tpause_r32,													// 66 0FAE /6
		Tpause_r64,													// 66 REX.W 0FAE /6
		Clrssbsy_m64,												// F3 0FAE /6
		Umonitor_r16,												// a16 F3 0FAE /6
		Umonitor_r32,												// a32 F3 0FAE /6
		Umonitor_r64,												// a64 F3 0FAE /6
		Umwait_r32,													// F2 0FAE /6
		Umwait_r64,													// F2 REX.W 0FAE /6
		Clflush_m8,													// 0FAE /7
		Clflushopt_m8,												// 66 0FAE /7
		Lfence,														// 0FAE E8-EF
		Mfence,														// 0FAE F0-F7
		Sfence,														// 0FAE F8-FF
		Pcommit,													// 66 0FAE F8
		Imul_r16_rm16,												// o16 0FAF
		Imul_r32_rm32,												// o32 0FAF
		Imul_r64_rm64,												// REX.W 0FAF

		Cmpxchg_rm8_r8,												// 0FB0
		Cmpxchg_rm16_r16,											// o16 0FB1
		Cmpxchg_rm32_r32,											// o32 0FB1
		Cmpxchg_rm64_r64,											// REX.W 0FB1
		Lss_r16_m32,												// o16 0FB2
		Lss_r32_m48,												// o32 0FB2
		Lss_r64_m80,												// REX.W 0FB2
		Btr_rm16_r16,												// o16 0FB3
		Btr_rm32_r32,												// o32 0FB3
		Btr_rm64_r64,												// REX.W 0FB3
		Lfs_r16_m32,												// o16 0FB4
		Lfs_r32_m48,												// o32 0FB4
		Lfs_r64_m80,												// REX.W 0FB4
		Lgs_r16_m32,												// o16 0FB5
		Lgs_r32_m48,												// o32 0FB5
		Lgs_r64_m80,												// REX.W 0FB5
		Movzx_r16_rm8,												// o16 0FB6
		Movzx_r32_rm8,												// o32 0FB6
		Movzx_r64_rm8,												// REX.W 0FB6
		Movzx_r16_rm16,												// o16 0FB7
		Movzx_r32_rm16,												// o32 0FB7
		Movzx_r64_rm16,												// REX.W 0FB7

		Jmpe_disp16,												// o16 0FB8
		Jmpe_disp32,												// o32 0FB8

		Popcnt_r16_rm16,											// o16 F3 0FB8
		Popcnt_r32_rm32,											// o32 F3 0FB8
		Popcnt_r64_rm64,											// F3 REX.W 0FB8

		Ud1_r16_rm16,												// o16 0FB9
		Ud1_r32_rm32,												// o32 0FB9
		Ud1_r64_rm64,												// REX.W 0FB9
		Bt_rm16_imm8,												// o16 0FBA /4
		Bt_rm32_imm8,												// o32 0FBA /4
		Bt_rm64_imm8,												// REX.W 0FBA /4
		Bts_rm16_imm8,												// o16 0FBA /5
		Bts_rm32_imm8,												// o32 0FBA /5
		Bts_rm64_imm8,												// REX.W 0FBA /5
		Btr_rm16_imm8,												// o16 0FBA /6
		Btr_rm32_imm8,												// o32 0FBA /6
		Btr_rm64_imm8,												// REX.W 0FBA /6
		Btc_rm16_imm8,												// o16 0FBA /7
		Btc_rm32_imm8,												// o32 0FBA /7
		Btc_rm64_imm8,												// REX.W 0FBA /7
		Btc_rm16_r16,												// o16 0FBB
		Btc_rm32_r32,												// o32 0FBB
		Btc_rm64_r64,												// REX.W 0FBB
		Bsf_r16_rm16,												// o16 0FBC
		Bsf_r32_rm32,												// o32 0FBC
		Bsf_r64_rm64,												// REX.W 0FBC
		Tzcnt_r16_rm16,												// o16 F3 0FBC
		Tzcnt_r32_rm32,												// o32 F3 0FBC
		Tzcnt_r64_rm64,												// F3 REX.W 0FBC
		Bsr_r16_rm16,												// o16 0FBD
		Bsr_r32_rm32,												// o32 0FBD
		Bsr_r64_rm64,												// REX.W 0FBD
		Lzcnt_r16_rm16,												// o16 F3 0FBD
		Lzcnt_r32_rm32,												// o32 F3 0FBD
		Lzcnt_r64_rm64,												// F3 REX.W 0FBD
		Movsx_r16_rm8,												// o16 0FBE
		Movsx_r32_rm8,												// o32 0FBE
		Movsx_r64_rm8,												// REX.W 0FBE
		Movsx_r16_rm16,												// o16 0FBF
		Movsx_r32_rm16,												// o32 0FBF
		Movsx_r64_rm16,												// REX.W 0FBF

		Xadd_rm8_r8,												// 0FC0
		Xadd_rm16_r16,												// o16 0FC1
		Xadd_rm32_r32,												// o32 0FC1
		Xadd_rm64_r64,												// REX.W 0FC1

		Cmpps_xmm_xmmm128_imm8,										// 0FC2
		VEX_Vcmpps_xmm_xmm_xmmm128_imm8,							// VEX.128.0F.WIG C2
		VEX_Vcmpps_ymm_ymm_ymmm256_imm8,							// VEX.256.0F.WIG C2
		EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.0F.W0 C2
		EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.0F.W0 C2
		EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae,					// EVEX.512.0F.W0 C2

		Cmppd_xmm_xmmm128_imm8,										// 66 0FC2
		VEX_Vcmppd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F.WIG C2
		VEX_Vcmppd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F.WIG C2
		EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 C2
		EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 C2
		EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae,					// EVEX.512.66.0F.W1 C2

		Cmpss_xmm_xmmm32_imm8,										// F3 0FC2
		VEX_Vcmpss_xmm_xmm_xmmm32_imm8,								// VEX.LIG.F3.0F.WIG C2
		EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae,						// EVEX.LIG.F3.0F.W0 C2

		Cmpsd_xmm_xmmm64_imm8,										// F2 0FC2
		VEX_Vcmpsd_xmm_xmm_xmmm64_imm8,								// VEX.LIG.F2.0F.WIG C2
		EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae,						// EVEX.LIG.F2.0F.W1 C2

		Movnti_m32_r32,												// 0FC3
		Movnti_m64_r64,												// REX.W 0FC3

		Pinsrw_mm_r32m16_imm8,										// 0FC4
		Pinsrw_mm_r64m16_imm8,										// REX.W 0FC4

		Pinsrw_xmm_r32m16_imm8,										// 66 0FC4
		Pinsrw_xmm_r64m16_imm8,										// 66 REX.W 0FC4
		VEX_Vpinsrw_xmm_xmm_r32m16_imm8,							// VEX.128.66.0F.W0 C4
		VEX_Vpinsrw_xmm_xmm_r64m16_imm8,							// VEX.128.66.0F.W1 C4
		EVEX_Vpinsrw_xmm_xmm_r32m16_imm8,							// EVEX.128.66.0F.W0 C4
		EVEX_Vpinsrw_xmm_xmm_r64m16_imm8,							// EVEX.128.66.0F.W1 C4

		Pextrw_r32_mm_imm8,											// 0FC5
		Pextrw_r64_mm_imm8,											// REX.W 0FC5

		Pextrw_r32_xmm_imm8,										// 66 0FC5
		Pextrw_r64_xmm_imm8,										// 66 REX.W 0FC5
		VEX_Vpextrw_r32_xmm_imm8,									// VEX.128.66.0F.W0 C5
		VEX_Vpextrw_r64_xmm_imm8,									// VEX.128.66.0F.W1 C5
		EVEX_Vpextrw_r32_xmm_imm8,									// EVEX.128.66.0F.W0 C5
		EVEX_Vpextrw_r64_xmm_imm8,									// EVEX.128.66.0F.W1 C5

		Shufps_xmm_xmmm128_imm8,									// 0FC6
		VEX_Vshufps_xmm_xmm_xmmm128_imm8,							// VEX.128.0F.WIG C6
		VEX_Vshufps_ymm_ymm_ymmm256_imm8,							// VEX.256.0F.WIG C6
		EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.0F.W0 C6
		EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.0F.W0 C6
		EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.0F.W0 C6

		Shufpd_xmm_xmmm128_imm8,									// 66 0FC6
		VEX_Vshufpd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F.WIG C6
		VEX_Vshufpd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F.WIG C6
		EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F.W1 C6
		EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F.W1 C6
		EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F.W1 C6

		Cmpxchg8b_m64,												// 0FC7 /1
		Cmpxchg16b_m128,											// REX.W 0FC7 /1
		Xrstors_m,													// 0FC7 /3
		Xrstors64_m,												// REX.W 0FC7 /3
		Xsavec_m,													// 0FC7 /4
		Xsavec64_m,													// REX.W 0FC7 /4
		Xsaves_m,													// 0FC7 /5
		Xsaves64_m,													// REX.W 0FC7 /5
		Vmptrld_m64,												// 0FC7 /6
		Vmclear_m64,												// 66 0FC7 /6
		Vmxon_m64,													// F3 0FC7 /6
		Rdrand_r16,													// o16 0FC7 /6
		Rdrand_r32,													// o32 0FC7 /6
		Rdrand_r64,													// REX.W 0FC7 /6
		Vmptrst_m64,												// 0FC7 /7
		Rdseed_r16,													// o16 0FC7 /7
		Rdseed_r32,													// o32 0FC7 /7
		Rdseed_r64,													// REX.W 0FC7 /7
		Rdpid_r32,													// F3 0FC7 /7
		Rdpid_r64,													// F3 0FC7 /7

		Bswap_r16,													// o16 0FC8
		Bswap_r32,													// o32 0FC8
		Bswap_r64,													// REX.W 0FC8

		Addsubpd_xmm_xmmm128,										// 66 0FD0
		VEX_Vaddsubpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D0
		VEX_Vaddsubpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D0

		Addsubps_xmm_xmmm128,										// F2 0FD0
		VEX_Vaddsubps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG D0
		VEX_Vaddsubps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG D0

		Psrlw_mm_mmm64,												// 0FD1

		Psrlw_xmm_xmmm128,											// 66 0FD1
		VEX_Vpsrlw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D1
		VEX_Vpsrlw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D1
		EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D1
		EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG D1
		EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG D1

		Psrld_mm_mmm64,												// 0FD2

		Psrld_xmm_xmmm128,											// 66 0FD2
		VEX_Vpsrld_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D2
		VEX_Vpsrld_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D2
		EVEX_Vpsrld_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 D2
		EVEX_Vpsrld_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 D2
		EVEX_Vpsrld_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 D2

		Psrlq_mm_mmm64,												// 0FD3

		Psrlq_xmm_xmmm128,											// 66 0FD3
		VEX_Vpsrlq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D3
		VEX_Vpsrlq_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D3
		EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 D3
		EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 D3
		EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 D3

		Paddq_mm_mmm64,												// 0FD4

		Paddq_xmm_xmmm128,											// 66 0FD4
		VEX_Vpaddq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D4
		VEX_Vpaddq_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG D4
		EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 D4
		EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 D4
		EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 D4

		Pmullw_mm_mmm64,											// 0FD5

		Pmullw_xmm_xmmm128,											// 66 0FD5
		VEX_Vpmullw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D5
		VEX_Vpmullw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D5
		EVEX_Vpmullw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D5
		EVEX_Vpmullw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D5
		EVEX_Vpmullw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D5

		Movq_xmmm64_xmm,											// 66 0FD6
		VEX_Vmovq_xmmm64_xmm,										// VEX.128.66.0F.WIG D6
		EVEX_Vmovq_xmmm64_xmm,										// EVEX.128.66.0F.W1 D6

		Movq2dq_xmm_mm,												// F3 0FD6

		Movdq2q_mm_xmm,												// F2 0FD6

		Pmovmskb_r32_mm,											// 0FD7
		Pmovmskb_r64_mm,											// REX.W 0FD7

		Pmovmskb_r32_xmm,											// 66 0FD7
		Pmovmskb_r64_xmm,											// 66 REX.W 0FD7
		VEX_Vpmovmskb_r32_xmm,										// VEX.128.66.0F.W0 D7
		VEX_Vpmovmskb_r64_xmm,										// VEX.128.66.0F.W1 D7
		VEX_Vpmovmskb_r32_ymm,										// VEX.256.66.0F.W0 D7
		VEX_Vpmovmskb_r64_ymm,										// VEX.256.66.0F.W1 D7

		Psubusb_mm_mmm64,											// 0FD8

		Psubusb_xmm_xmmm128,										// 66 0FD8
		VEX_Vpsubusb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D8
		VEX_Vpsubusb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D8
		EVEX_Vpsubusb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D8
		EVEX_Vpsubusb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D8
		EVEX_Vpsubusb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D8

		Psubusw_mm_mmm64,											// 0FD9

		Psubusw_xmm_xmmm128,										// 66 0FD9
		VEX_Vpsubusw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D9
		VEX_Vpsubusw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D9
		EVEX_Vpsubusw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D9
		EVEX_Vpsubusw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D9
		EVEX_Vpsubusw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D9

		Pminub_mm_mmm64,											// 0FDA

		Pminub_xmm_xmmm128,											// 66 0FDA
		VEX_Vpminub_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DA
		VEX_Vpminub_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DA
		EVEX_Vpminub_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DA
		EVEX_Vpminub_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DA
		EVEX_Vpminub_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DA

		Pand_mm_mmm64,												// 0FDB

		Pand_xmm_xmmm128,											// 66 0FDB
		VEX_Vpand_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG DB
		VEX_Vpand_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG DB
		EVEX_Vpandd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 DB
		EVEX_Vpandd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 DB
		EVEX_Vpandd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 DB
		EVEX_Vpandq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 DB
		EVEX_Vpandq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 DB
		EVEX_Vpandq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 DB

		Paddusb_mm_mmm64,											// 0FDC

		Paddusb_xmm_xmmm128,										// 66 0FDC
		VEX_Vpaddusb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DC
		VEX_Vpaddusb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DC
		EVEX_Vpaddusb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DC
		EVEX_Vpaddusb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DC
		EVEX_Vpaddusb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DC

		Paddusw_mm_mmm64,											// 0FDD

		Paddusw_xmm_xmmm128,										// 66 0FDD
		VEX_Vpaddusw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DD
		VEX_Vpaddusw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DD
		EVEX_Vpaddusw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DD
		EVEX_Vpaddusw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DD
		EVEX_Vpaddusw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DD

		Pmaxub_mm_mmm64,											// 0FDE

		Pmaxub_xmm_xmmm128,											// 66 0FDE
		VEX_Vpmaxub_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DE
		VEX_Vpmaxub_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DE
		EVEX_Vpmaxub_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DE
		EVEX_Vpmaxub_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DE
		EVEX_Vpmaxub_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DE

		Pandn_mm_mmm64,												// 0FDF

		Pandn_xmm_xmmm128,											// 66 0FDF
		VEX_Vpandn_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG DF
		VEX_Vpandn_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG DF
		EVEX_Vpandnd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 DF
		EVEX_Vpandnd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 DF
		EVEX_Vpandnd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 DF
		EVEX_Vpandnq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 DF
		EVEX_Vpandnq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 DF
		EVEX_Vpandnq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 DF

		Pavgb_mm_mmm64,												// 0FE0

		Pavgb_xmm_xmmm128,											// 66 0FE0
		VEX_Vpavgb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E0
		VEX_Vpavgb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG E0
		EVEX_Vpavgb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E0
		EVEX_Vpavgb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E0
		EVEX_Vpavgb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E0

		Psraw_mm_mmm64,												// 0FE1

		Psraw_xmm_xmmm128,											// 66 0FE1
		VEX_Vpsraw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E1
		VEX_Vpsraw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG E1
		EVEX_Vpsraw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E1
		EVEX_Vpsraw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG E1
		EVEX_Vpsraw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG E1

		Psrad_mm_mmm64,												// 0FE2

		Psrad_xmm_xmmm128,											// 66 0FE2
		VEX_Vpsrad_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E2
		VEX_Vpsrad_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG E2
		EVEX_Vpsrad_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 E2
		EVEX_Vpsrad_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 E2
		EVEX_Vpsrad_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 E2
		EVEX_Vpsraq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 E2
		EVEX_Vpsraq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 E2
		EVEX_Vpsraq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 E2

		Pavgw_mm_mmm64,												// 0FE3

		Pavgw_xmm_xmmm128,											// 66 0FE3
		VEX_Vpavgw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E3
		VEX_Vpavgw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG E3
		EVEX_Vpavgw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E3
		EVEX_Vpavgw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E3
		EVEX_Vpavgw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E3

		Pmulhuw_mm_mmm64,											// 0FE4

		Pmulhuw_xmm_xmmm128,										// 66 0FE4
		VEX_Vpmulhuw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E4
		VEX_Vpmulhuw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E4
		EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E4
		EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E4
		EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E4

		Pmulhw_mm_mmm64,											// 0FE5

		Pmulhw_xmm_xmmm128,											// 66 0FE5
		VEX_Vpmulhw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E5
		VEX_Vpmulhw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E5
		EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E5
		EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E5
		EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E5

		Cvttpd2dq_xmm_xmmm128,										// 66 0FE6
		VEX_Vcvttpd2dq_xmm_xmmm128,									// VEX.128.66.0F.WIG E6
		VEX_Vcvttpd2dq_xmm_ymmm256,									// VEX.256.66.0F.WIG E6
		EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 E6
		EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 E6
		EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F.W1 E6

		Cvtdq2pd_xmm_xmmm64,										// F3 0FE6
		VEX_Vcvtdq2pd_xmm_xmmm64,									// VEX.128.F3.0F.WIG E6
		VEX_Vcvtdq2pd_ymm_xmmm128,									// VEX.256.F3.0F.WIG E6
		EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32,							// EVEX.128.F3.0F.W0 E6
		EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32,							// EVEX.256.F3.0F.W0 E6
		EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32,							// EVEX.512.F3.0F.W0 E6
		EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64,							// EVEX.128.F3.0F.W1 E6
		EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64,							// EVEX.256.F3.0F.W1 E6
		EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er,						// EVEX.512.F3.0F.W1 E6

		Cvtpd2dq_xmm_xmmm128,										// F2 0FE6
		VEX_Vcvtpd2dq_xmm_xmmm128,									// VEX.128.F2.0F.WIG E6
		VEX_Vcvtpd2dq_xmm_ymmm256,									// VEX.256.F2.0F.WIG E6
		EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64,							// EVEX.128.F2.0F.W1 E6
		EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64,							// EVEX.256.F2.0F.W1 E6
		EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er,						// EVEX.512.F2.0F.W1 E6

		Movntq_m64_mm,												// 0FE7

		Movntdq_m128_xmm,											// 66 0FE7
		VEX_Vmovntdq_m128_xmm,										// VEX.128.66.0F.WIG E7
		VEX_Vmovntdq_m256_ymm,										// VEX.256.66.0F.WIG E7
		EVEX_Vmovntdq_m128_xmm,										// EVEX.128.66.0F.W0 E7
		EVEX_Vmovntdq_m256_ymm,										// EVEX.256.66.0F.W0 E7
		EVEX_Vmovntdq_m512_zmm,										// EVEX.512.66.0F.W0 E7

		Psubsb_mm_mmm64,											// 0FE8

		Psubsb_xmm_xmmm128,											// 66 0FE8
		VEX_Vpsubsb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E8
		VEX_Vpsubsb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E8
		EVEX_Vpsubsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E8
		EVEX_Vpsubsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E8
		EVEX_Vpsubsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E8

		Psubsw_mm_mmm64,											// 0FE9

		Psubsw_xmm_xmmm128,											// 66 0FE9
		VEX_Vpsubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E9
		VEX_Vpsubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E9
		EVEX_Vpsubsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E9
		EVEX_Vpsubsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E9
		EVEX_Vpsubsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E9

		Pminsw_mm_mmm64,											// 0FEA

		Pminsw_xmm_xmmm128,											// 66 0FEA
		VEX_Vpminsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EA
		VEX_Vpminsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EA
		EVEX_Vpminsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EA
		EVEX_Vpminsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EA
		EVEX_Vpminsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EA

		Por_mm_mmm64,												// 0FEB

		Por_xmm_xmmm128,											// 66 0FEB
		VEX_Vpor_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG EB
		VEX_Vpor_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG EB
		EVEX_Vpord_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 EB
		EVEX_Vpord_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 EB
		EVEX_Vpord_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 EB
		EVEX_Vporq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 EB
		EVEX_Vporq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 EB
		EVEX_Vporq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 EB

		Paddsb_mm_mmm64,											// 0FEC

		Paddsb_xmm_xmmm128,											// 66 0FEC
		VEX_Vpaddsb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EC
		VEX_Vpaddsb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EC
		EVEX_Vpaddsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EC
		EVEX_Vpaddsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EC
		EVEX_Vpaddsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EC

		Paddsw_mm_mmm64,											// 0FED

		Paddsw_xmm_xmmm128,											// 66 0FED
		VEX_Vpaddsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG ED
		VEX_Vpaddsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG ED
		EVEX_Vpaddsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG ED
		EVEX_Vpaddsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG ED
		EVEX_Vpaddsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG ED

		Pmaxsw_mm_mmm64,											// 0FEE

		Pmaxsw_xmm_xmmm128,											// 66 0FEE
		VEX_Vpmaxsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EE
		VEX_Vpmaxsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EE
		EVEX_Vpmaxsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EE
		EVEX_Vpmaxsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EE
		EVEX_Vpmaxsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EE

		Pxor_mm_mmm64,												// 0FEF

		Pxor_xmm_xmmm128,											// 66 0FEF
		VEX_Vpxor_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG EF
		VEX_Vpxor_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG EF
		EVEX_Vpxord_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 EF
		EVEX_Vpxord_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 EF
		EVEX_Vpxord_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 EF
		EVEX_Vpxorq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 EF
		EVEX_Vpxorq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 EF
		EVEX_Vpxorq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 EF

		Lddqu_xmm_m128,												// F2 0FF0
		VEX_Vlddqu_xmm_m128,										// VEX.128.F2.0F.WIG F0
		VEX_Vlddqu_ymm_m256,										// VEX.256.F2.0F.WIG F0

		Psllw_mm_mmm64,												// 0FF1

		Psllw_xmm_xmmm128,											// 66 0FF1
		VEX_Vpsllw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F1
		VEX_Vpsllw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F1
		EVEX_Vpsllw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F1
		EVEX_Vpsllw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG F1
		EVEX_Vpsllw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG F1

		Pslld_mm_mmm64,												// 0FF2

		Pslld_xmm_xmmm128,											// 66 0FF2
		VEX_Vpslld_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F2
		VEX_Vpslld_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F2
		EVEX_Vpslld_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 F2
		EVEX_Vpslld_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 F2
		EVEX_Vpslld_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 F2

		Psllq_mm_mmm64,												// 0FF3

		Psllq_xmm_xmmm128,											// 66 0FF3
		VEX_Vpsllq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F3
		VEX_Vpsllq_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F3
		EVEX_Vpsllq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 F3
		EVEX_Vpsllq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 F3
		EVEX_Vpsllq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 F3

		Pmuludq_mm_mmm64,											// 0FF4

		Pmuludq_xmm_xmmm128,										// 66 0FF4
		VEX_Vpmuludq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F4
		VEX_Vpmuludq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F4
		EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 F4
		EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 F4
		EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 F4

		Pmaddwd_mm_mmm64,											// 0FF5

		Pmaddwd_xmm_xmmm128,										// 66 0FF5
		VEX_Vpmaddwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F5
		VEX_Vpmaddwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F5
		EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F5
		EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F5
		EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F5

		Psadbw_mm_mmm64,											// 0FF6

		Psadbw_xmm_xmmm128,											// 66 0FF6
		VEX_Vpsadbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F6
		VEX_Vpsadbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F6
		EVEX_Vpsadbw_xmm_xmm_xmmm128,								// EVEX.128.66.0F.WIG F6
		EVEX_Vpsadbw_ymm_ymm_ymmm256,								// EVEX.256.66.0F.WIG F6
		EVEX_Vpsadbw_zmm_zmm_zmmm512,								// EVEX.512.66.0F.WIG F6

		Maskmovq_rDI_mm_mm,											// 0FF7

		Maskmovdqu_rDI_xmm_xmm,										// 66 0FF7
		VEX_Vmaskmovdqu_rDI_xmm_xmm,								// VEX.128.66.0F.WIG F7

		Psubb_mm_mmm64,												// 0FF8

		Psubb_xmm_xmmm128,											// 66 0FF8
		VEX_Vpsubb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F8
		VEX_Vpsubb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG F8
		EVEX_Vpsubb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F8
		EVEX_Vpsubb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F8
		EVEX_Vpsubb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F8

		Psubw_mm_mmm64,												// 0FF9

		Psubw_xmm_xmmm128,											// 66 0FF9
		VEX_Vpsubw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F9
		VEX_Vpsubw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG F9
		EVEX_Vpsubw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F9
		EVEX_Vpsubw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F9
		EVEX_Vpsubw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F9

		Psubd_mm_mmm64,												// 0FFA

		Psubd_xmm_xmmm128,											// 66 0FFA
		VEX_Vpsubd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FA
		VEX_Vpsubd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FA
		EVEX_Vpsubd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 FA
		EVEX_Vpsubd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 FA
		EVEX_Vpsubd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 FA

		Psubq_mm_mmm64,												// 0FFB

		Psubq_xmm_xmmm128,											// 66 0FFB
		VEX_Vpsubq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FB
		VEX_Vpsubq_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FB
		EVEX_Vpsubq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 FB
		EVEX_Vpsubq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 FB
		EVEX_Vpsubq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 FB

		Paddb_mm_mmm64,												// 0FFC

		Paddb_xmm_xmmm128,											// 66 0FFC
		VEX_Vpaddb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FC
		VEX_Vpaddb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FC
		EVEX_Vpaddb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG FC
		EVEX_Vpaddb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG FC
		EVEX_Vpaddb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG FC

		Paddw_mm_mmm64,												// 0FFD

		Paddw_xmm_xmmm128,											// 66 0FFD
		VEX_Vpaddw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FD
		VEX_Vpaddw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FD
		EVEX_Vpaddw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG FD
		EVEX_Vpaddw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG FD
		EVEX_Vpaddw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG FD

		Paddd_mm_mmm64,												// 0FFE

		Paddd_xmm_xmmm128,											// 66 0FFE
		VEX_Vpaddd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FE
		VEX_Vpaddd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FE
		EVEX_Vpaddd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 FE
		EVEX_Vpaddd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 FE
		EVEX_Vpaddd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 FE

		Ud0_r16_rm16,												// o16 0FFF
		Ud0_r32_rm32,												// o32 0FFF
		Ud0_r64_rm64,												// REX.W 0FFF

		// 0F38xx opcodes

		Pshufb_mm_mmm64,											// 0F3800

		Pshufb_xmm_xmmm128,											// 66 0F3800
		VEX_Vpshufb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 00
		VEX_Vpshufb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 00
		EVEX_Vpshufb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 00
		EVEX_Vpshufb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 00
		EVEX_Vpshufb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 00

		Phaddw_mm_mmm64,											// 0F3801

		Phaddw_xmm_xmmm128,											// 66 0F3801
		VEX_Vphaddw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 01
		VEX_Vphaddw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 01

		Phaddd_mm_mmm64,											// 0F3802

		Phaddd_xmm_xmmm128,											// 66 0F3802
		VEX_Vphaddd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 02
		VEX_Vphaddd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 02

		Phaddsw_mm_mmm64,											// 0F3803

		Phaddsw_xmm_xmmm128,										// 66 0F3803
		VEX_Vphaddsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 03
		VEX_Vphaddsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 03

		Pmaddubsw_mm_mmm64,											// 0F3804

		Pmaddubsw_xmm_xmmm128,										// 66 0F3804
		VEX_Vpmaddubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 04
		VEX_Vpmaddubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 04
		EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F38.WIG 04
		EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F38.WIG 04
		EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F38.WIG 04

		Phsubw_mm_mmm64,											// 0F3805

		Phsubw_xmm_xmmm128,											// 66 0F3805
		VEX_Vphsubw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 05
		VEX_Vphsubw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 05

		Phsubd_mm_mmm64,											// 0F3806

		Phsubd_xmm_xmmm128,											// 66 0F3806
		VEX_Vphsubd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 06
		VEX_Vphsubd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 06

		Phsubsw_mm_mmm64,											// 0F3807

		Phsubsw_xmm_xmmm128,										// 66 0F3807
		VEX_Vphsubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 07
		VEX_Vphsubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 07

		Psignb_mm_mmm64,											// 0F3808

		Psignb_xmm_xmmm128,											// 66 0F3808
		VEX_Vpsignb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 08
		VEX_Vpsignb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 08

		Psignw_mm_mmm64,											// 0F3809

		Psignw_xmm_xmmm128,											// 66 0F3809
		VEX_Vpsignw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 09
		VEX_Vpsignw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 09

		Psignd_mm_mmm64,											// 0F380A

		Psignd_xmm_xmmm128,											// 66 0F380A
		VEX_Vpsignd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 0A
		VEX_Vpsignd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 0A

		Pmulhrsw_mm_mmm64,											// 0F380B

		Pmulhrsw_xmm_xmmm128,										// 66 0F380B
		VEX_Vpmulhrsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 0B
		VEX_Vpmulhrsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 0B
		EVEX_Vpmulhrsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 0B

		VEX_Vpermilps_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 0C
		VEX_Vpermilps_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 0C
		EVEX_Vpermilps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 0C
		EVEX_Vpermilps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 0C
		EVEX_Vpermilps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 0C

		VEX_Vpermilpd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 0D
		VEX_Vpermilpd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 0D
		EVEX_Vpermilpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 0D
		EVEX_Vpermilpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 0D
		EVEX_Vpermilpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 0D

		VEX_Vtestps_xmm_xmmm128,									// VEX.128.66.0F38.W0 0E
		VEX_Vtestps_ymm_ymmm256,									// VEX.256.66.0F38.W0 0E

		VEX_Vtestpd_xmm_xmmm128,									// VEX.128.66.0F38.W0 0F
		VEX_Vtestpd_ymm_ymmm256,									// VEX.256.66.0F38.W0 0F

		Pblendvb_xmm_xmmm128,										// 66 0F3810

		EVEX_Vpsrlvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 10
		EVEX_Vpsrlvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 10
		EVEX_Vpsrlvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 10

		EVEX_Vpmovuswb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 10
		EVEX_Vpmovuswb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 10
		EVEX_Vpmovuswb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 10

		EVEX_Vpsravw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 11
		EVEX_Vpsravw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 11
		EVEX_Vpsravw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 11

		EVEX_Vpmovusdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 11
		EVEX_Vpmovusdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 11
		EVEX_Vpmovusdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 11

		EVEX_Vpsllvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 12
		EVEX_Vpsllvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 12
		EVEX_Vpsllvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 12

		EVEX_Vpmovusqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 12
		EVEX_Vpmovusqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 12
		EVEX_Vpmovusqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 12

		VEX_Vcvtph2ps_xmm_xmmm64,									// VEX.128.66.0F38.W0 13
		VEX_Vcvtph2ps_ymm_xmmm128,									// VEX.256.66.0F38.W0 13
		EVEX_Vcvtph2ps_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 13
		EVEX_Vcvtph2ps_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 13
		EVEX_Vcvtph2ps_zmm_k1z_ymmm256_sae,							// EVEX.512.66.0F38.W0 13

		EVEX_Vpmovusdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 13
		EVEX_Vpmovusdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 13
		EVEX_Vpmovusdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 13

		Blendvps_xmm_xmmm128,										// 66 0F3814
		EVEX_Vprorvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 14
		EVEX_Vprorvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 14
		EVEX_Vprorvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 14
		EVEX_Vprorvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 14
		EVEX_Vprorvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 14
		EVEX_Vprorvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 14

		EVEX_Vpmovusqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 14
		EVEX_Vpmovusqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 14
		EVEX_Vpmovusqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 14

		Blendvpd_xmm_xmmm128,										// 66 0F3815
		EVEX_Vprolvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 15
		EVEX_Vprolvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 15
		EVEX_Vprolvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 15
		EVEX_Vprolvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 15
		EVEX_Vprolvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 15
		EVEX_Vprolvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 15

		EVEX_Vpmovusqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 15
		EVEX_Vpmovusqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 15
		EVEX_Vpmovusqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 15

		VEX_Vpermps_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 16
		EVEX_Vpermps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 16
		EVEX_Vpermps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 16
		EVEX_Vpermpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 16
		EVEX_Vpermpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 16

		Ptest_xmm_xmmm128,											// 66 0F3817
		VEX_Vptest_xmm_xmmm128,										// VEX.128.66.0F38.WIG 17
		VEX_Vptest_ymm_ymmm256,										// VEX.256.66.0F38.WIG 17

		VEX_Vbroadcastss_xmm_xmmm32,								// VEX.128.66.0F38.W0 18
		VEX_Vbroadcastss_ymm_xmmm32,								// VEX.256.66.0F38.W0 18
		EVEX_Vbroadcastss_xmm_k1z_xmmm32,							// EVEX.128.66.0F38.W0 18
		EVEX_Vbroadcastss_ymm_k1z_xmmm32,							// EVEX.256.66.0F38.W0 18
		EVEX_Vbroadcastss_zmm_k1z_xmmm32,							// EVEX.512.66.0F38.W0 18

		VEX_Vbroadcastsd_ymm_xmmm64,								// VEX.256.66.0F38.W0 19
		EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64,						// EVEX.256.66.0F38.W0 19
		EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64,						// EVEX.512.66.0F38.W0 19
		EVEX_Vbroadcastsd_ymm_k1z_xmmm64,							// EVEX.256.66.0F38.W1 19
		EVEX_Vbroadcastsd_zmm_k1z_xmmm64,							// EVEX.512.66.0F38.W1 19

		VEX_Vbroadcastf128_ymm_m128,								// VEX.256.66.0F38.W0 1A
		EVEX_Vbroadcastf32x4_ymm_k1z_m128,							// EVEX.256.66.0F38.W0 1A
		EVEX_Vbroadcastf32x4_zmm_k1z_m128,							// EVEX.512.66.0F38.W0 1A
		EVEX_Vbroadcastf64x2_ymm_k1z_m128,							// EVEX.256.66.0F38.W1 1A
		EVEX_Vbroadcastf64x2_zmm_k1z_m128,							// EVEX.512.66.0F38.W1 1A

		EVEX_Vbroadcastf32x8_zmm_k1z_m256,							// EVEX.512.66.0F38.W0 1B
		EVEX_Vbroadcastf64x4_zmm_k1z_m256,							// EVEX.512.66.0F38.W1 1B

		Pabsb_mm_mmm64,												// 0F381C

		Pabsb_xmm_xmmm128,											// 66 0F381C
		VEX_Vpabsb_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1C
		VEX_Vpabsb_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1C
		EVEX_Vpabsb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.WIG 1C
		EVEX_Vpabsb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.WIG 1C
		EVEX_Vpabsb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.WIG 1C

		Pabsw_mm_mmm64,												// 0F381D

		Pabsw_xmm_xmmm128,											// 66 0F381D
		VEX_Vpabsw_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1D
		VEX_Vpabsw_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1D
		EVEX_Vpabsw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.WIG 1D
		EVEX_Vpabsw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.WIG 1D
		EVEX_Vpabsw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.WIG 1D

		Pabsd_mm_mmm64,												// 0F381E

		Pabsd_xmm_xmmm128,											// 66 0F381E
		VEX_Vpabsd_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1E
		VEX_Vpabsd_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1E
		EVEX_Vpabsd_xmm_k1z_xmmm128b32,								// EVEX.128.66.0F38.W0 1E
		EVEX_Vpabsd_ymm_k1z_ymmm256b32,								// EVEX.256.66.0F38.W0 1E
		EVEX_Vpabsd_zmm_k1z_zmmm512b32,								// EVEX.512.66.0F38.W0 1E

		EVEX_Vpabsq_xmm_k1z_xmmm128b64,								// EVEX.128.66.0F38.W1 1F
		EVEX_Vpabsq_ymm_k1z_ymmm256b64,								// EVEX.256.66.0F38.W1 1F
		EVEX_Vpabsq_zmm_k1z_zmmm512b64,								// EVEX.512.66.0F38.W1 1F

		Pmovsxbw_xmm_xmmm64,										// 66 0F3820
		VEX_Vpmovsxbw_xmm_xmmm64,									// VEX.128.66.0F38.WIG 20
		VEX_Vpmovsxbw_ymm_xmmm128,									// VEX.256.66.0F38.WIG 20
		EVEX_Vpmovsxbw_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 20
		EVEX_Vpmovsxbw_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 20
		EVEX_Vpmovsxbw_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 20

		EVEX_Vpmovswb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 20
		EVEX_Vpmovswb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 20
		EVEX_Vpmovswb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 20

		Pmovsxbd_xmm_xmmm32,										// 66 0F3821
		VEX_Vpmovsxbd_xmm_xmmm32,									// VEX.128.66.0F38.WIG 21
		VEX_Vpmovsxbd_ymm_xmmm64,									// VEX.256.66.0F38.WIG 21
		EVEX_Vpmovsxbd_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 21
		EVEX_Vpmovsxbd_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 21
		EVEX_Vpmovsxbd_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 21

		EVEX_Vpmovsdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 21
		EVEX_Vpmovsdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 21
		EVEX_Vpmovsdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 21

		Pmovsxbq_xmm_xmmm16,										// 66 0F3822
		VEX_Vpmovsxbq_xmm_xmmm16,									// VEX.128.66.0F38.WIG 22
		VEX_Vpmovsxbq_ymm_xmmm32,									// VEX.256.66.0F38.WIG 22
		EVEX_Vpmovsxbq_xmm_k1z_xmmm16,								// EVEX.128.66.0F38.WIG 22
		EVEX_Vpmovsxbq_ymm_k1z_xmmm32,								// EVEX.256.66.0F38.WIG 22
		EVEX_Vpmovsxbq_zmm_k1z_xmmm64,								// EVEX.512.66.0F38.WIG 22

		EVEX_Vpmovsqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 22
		EVEX_Vpmovsqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 22
		EVEX_Vpmovsqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 22

		Pmovsxwd_xmm_xmmm64,										// 66 0F3823
		VEX_Vpmovsxwd_xmm_xmmm64,									// VEX.128.66.0F38.WIG 23
		VEX_Vpmovsxwd_ymm_xmmm128,									// VEX.256.66.0F38.WIG 23
		EVEX_Vpmovsxwd_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 23
		EVEX_Vpmovsxwd_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 23
		EVEX_Vpmovsxwd_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 23

		EVEX_Vpmovsdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 23
		EVEX_Vpmovsdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 23
		EVEX_Vpmovsdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 23

		Pmovsxwq_xmm_xmmm32,										// 66 0F3824
		VEX_Vpmovsxwq_xmm_xmmm32,									// VEX.128.66.0F38.WIG 24
		VEX_Vpmovsxwq_ymm_xmmm64,									// VEX.256.66.0F38.WIG 24
		EVEX_Vpmovsxwq_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 24
		EVEX_Vpmovsxwq_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 24
		EVEX_Vpmovsxwq_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 24

		EVEX_Vpmovsqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 24
		EVEX_Vpmovsqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 24
		EVEX_Vpmovsqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 24

		Pmovsxdq_xmm_xmmm64,										// 66 0F3825
		VEX_Vpmovsxdq_xmm_xmmm64,									// VEX.128.66.0F38.WIG 25
		VEX_Vpmovsxdq_ymm_xmmm128,									// VEX.256.66.0F38.WIG 25
		EVEX_Vpmovsxdq_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 25
		EVEX_Vpmovsxdq_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 25
		EVEX_Vpmovsxdq_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.W0 25

		EVEX_Vpmovsqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 25
		EVEX_Vpmovsqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 25
		EVEX_Vpmovsqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 25

		EVEX_Vptestmb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F38.W0 26
		EVEX_Vptestmb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F38.W0 26
		EVEX_Vptestmb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F38.W0 26
		EVEX_Vptestmw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F38.W1 26
		EVEX_Vptestmw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F38.W1 26
		EVEX_Vptestmw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F38.W1 26

		EVEX_Vptestnmb_k_k1_xmm_xmmm128,							// EVEX.128.F3.0F38.W0 26
		EVEX_Vptestnmb_k_k1_ymm_ymmm256,							// EVEX.256.F3.0F38.W0 26
		EVEX_Vptestnmb_k_k1_zmm_zmmm512,							// EVEX.512.F3.0F38.W0 26
		EVEX_Vptestnmw_k_k1_xmm_xmmm128,							// EVEX.128.F3.0F38.W1 26
		EVEX_Vptestnmw_k_k1_ymm_ymmm256,							// EVEX.256.F3.0F38.W1 26
		EVEX_Vptestnmw_k_k1_zmm_zmmm512,							// EVEX.512.F3.0F38.W1 26

		EVEX_Vptestmd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F38.W0 27
		EVEX_Vptestmd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F38.W0 27
		EVEX_Vptestmd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F38.W0 27
		EVEX_Vptestmq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 27
		EVEX_Vptestmq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 27
		EVEX_Vptestmq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 27

		EVEX_Vptestnmd_k_k1_xmm_xmmm128b32,							// EVEX.128.F3.0F38.W0 27
		EVEX_Vptestnmd_k_k1_ymm_ymmm256b32,							// EVEX.256.F3.0F38.W0 27
		EVEX_Vptestnmd_k_k1_zmm_zmmm512b32,							// EVEX.512.F3.0F38.W0 27
		EVEX_Vptestnmq_k_k1_xmm_xmmm128b64,							// EVEX.128.F3.0F38.W1 27
		EVEX_Vptestnmq_k_k1_ymm_ymmm256b64,							// EVEX.256.F3.0F38.W1 27
		EVEX_Vptestnmq_k_k1_zmm_zmmm512b64,							// EVEX.512.F3.0F38.W1 27

		Pmuldq_xmm_xmmm128,											// 66 0F3828
		VEX_Vpmuldq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 28
		VEX_Vpmuldq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 28
		EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 28
		EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 28
		EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 28

		EVEX_Vpmovm2b_xmm_k,										// EVEX.128.F3.0F38.W0 28
		EVEX_Vpmovm2b_ymm_k,										// EVEX.256.F3.0F38.W0 28
		EVEX_Vpmovm2b_zmm_k,										// EVEX.512.F3.0F38.W0 28
		EVEX_Vpmovm2w_xmm_k,										// EVEX.128.F3.0F38.W1 28
		EVEX_Vpmovm2w_ymm_k,										// EVEX.256.F3.0F38.W1 28
		EVEX_Vpmovm2w_zmm_k,										// EVEX.512.F3.0F38.W1 28

		Pcmpeqq_xmm_xmmm128,										// 66 0F3829
		VEX_Vpcmpeqq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 29
		VEX_Vpcmpeqq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 29
		EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 29
		EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 29
		EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 29

		EVEX_Vpmovb2m_k_xmm,										// EVEX.128.F3.0F38.W0 29
		EVEX_Vpmovb2m_k_ymm,										// EVEX.256.F3.0F38.W0 29
		EVEX_Vpmovb2m_k_zmm,										// EVEX.512.F3.0F38.W0 29
		EVEX_Vpmovw2m_k_xmm,										// EVEX.128.F3.0F38.W1 29
		EVEX_Vpmovw2m_k_ymm,										// EVEX.256.F3.0F38.W1 29
		EVEX_Vpmovw2m_k_zmm,										// EVEX.512.F3.0F38.W1 29

		Movntdqa_xmm_m128,											// 66 0F382A
		VEX_Vmovntdqa_xmm_m128,										// VEX.128.66.0F38.WIG 2A
		VEX_Vmovntdqa_ymm_m256,										// VEX.256.66.0F38.WIG 2A
		EVEX_Vmovntdqa_xmm_m128,									// EVEX.128.66.0F38.W0 2A
		EVEX_Vmovntdqa_ymm_m256,									// EVEX.256.66.0F38.W0 2A
		EVEX_Vmovntdqa_zmm_m512,									// EVEX.512.66.0F38.W0 2A

		EVEX_Vpbroadcastmb2q_xmm_k,									// EVEX.128.F3.0F38.W1 2A
		EVEX_Vpbroadcastmb2q_ymm_k,									// EVEX.256.F3.0F38.W1 2A
		EVEX_Vpbroadcastmb2q_zmm_k,									// EVEX.512.F3.0F38.W1 2A

		Packusdw_xmm_xmmm128,										// 66 0F382B
		VEX_Vpackusdw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 2B
		VEX_Vpackusdw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 2B
		EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 2B
		EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 2B
		EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 2B

		VEX_Vmaskmovps_xmm_xmm_m128,								// VEX.128.66.0F38.W0 2C
		VEX_Vmaskmovps_ymm_ymm_m256,								// VEX.256.66.0F38.W0 2C
		EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 2C
		EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 2C
		EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 2C
		EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 2C
		EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 2C
		EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 2C

		VEX_Vmaskmovpd_xmm_xmm_m128,								// VEX.128.66.0F38.W0 2D
		VEX_Vmaskmovpd_ymm_ymm_m256,								// VEX.256.66.0F38.W0 2D
		EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 2D
		EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 2D

		VEX_Vmaskmovps_m128_xmm_xmm,								// VEX.128.66.0F38.W0 2E
		VEX_Vmaskmovps_m256_ymm_ymm,								// VEX.256.66.0F38.W0 2E

		VEX_Vmaskmovpd_m128_xmm_xmm,								// VEX.128.66.0F38.W0 2F
		VEX_Vmaskmovpd_m256_ymm_ymm,								// VEX.256.66.0F38.W0 2F

		Pmovzxbw_xmm_xmmm64,										// 66 0F3830
		VEX_Vpmovzxbw_xmm_xmmm64,									// VEX.128.66.0F38.WIG 30
		VEX_Vpmovzxbw_ymm_xmmm128,									// VEX.256.66.0F38.WIG 30
		EVEX_Vpmovzxbw_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 30
		EVEX_Vpmovzxbw_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 30
		EVEX_Vpmovzxbw_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 30

		EVEX_Vpmovwb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 30
		EVEX_Vpmovwb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 30
		EVEX_Vpmovwb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 30

		Pmovzxbd_xmm_xmmm32,										// 66 0F3831
		VEX_Vpmovzxbd_xmm_xmmm32,									// VEX.128.66.0F38.WIG 31
		VEX_Vpmovzxbd_ymm_xmmm64,									// VEX.256.66.0F38.WIG 31
		EVEX_Vpmovzxbd_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 31
		EVEX_Vpmovzxbd_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 31
		EVEX_Vpmovzxbd_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 31

		EVEX_Vpmovdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 31
		EVEX_Vpmovdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 31
		EVEX_Vpmovdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 31

		Pmovzxbq_xmm_xmmm16,										// 66 0F3832
		VEX_Vpmovzxbq_xmm_xmmm16,									// VEX.128.66.0F38.WIG 32
		VEX_Vpmovzxbq_ymm_xmmm32,									// VEX.256.66.0F38.WIG 32
		EVEX_Vpmovzxbq_xmm_k1z_xmmm16,								// EVEX.128.66.0F38.WIG 32
		EVEX_Vpmovzxbq_ymm_k1z_xmmm32,								// EVEX.256.66.0F38.WIG 32
		EVEX_Vpmovzxbq_zmm_k1z_xmmm64,								// EVEX.512.66.0F38.WIG 32

		EVEX_Vpmovqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 32
		EVEX_Vpmovqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 32
		EVEX_Vpmovqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 32

		Pmovzxwd_xmm_xmmm64,										// 66 0F3833
		VEX_Vpmovzxwd_xmm_xmmm64,									// VEX.128.66.0F38.WIG 33
		VEX_Vpmovzxwd_ymm_xmmm128,									// VEX.256.66.0F38.WIG 33
		EVEX_Vpmovzxwd_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 33
		EVEX_Vpmovzxwd_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 33
		EVEX_Vpmovzxwd_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 33

		EVEX_Vpmovdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 33
		EVEX_Vpmovdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 33
		EVEX_Vpmovdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 33

		Pmovzxwq_xmm_xmmm32,										// 66 0F3834
		VEX_Vpmovzxwq_xmm_xmmm32,									// VEX.128.66.0F38.WIG 34
		VEX_Vpmovzxwq_ymm_xmmm64,									// VEX.256.66.0F38.WIG 34
		EVEX_Vpmovzxwq_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 34
		EVEX_Vpmovzxwq_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 34
		EVEX_Vpmovzxwq_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 34

		EVEX_Vpmovqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 34
		EVEX_Vpmovqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 34
		EVEX_Vpmovqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 34

		Pmovzxdq_xmm_xmmm64,										// 66 0F3835
		VEX_Vpmovzxdq_xmm_xmmm64,									// VEX.128.66.0F38.WIG 35
		VEX_Vpmovzxdq_ymm_xmmm128,									// VEX.256.66.0F38.WIG 35
		EVEX_Vpmovzxdq_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 35
		EVEX_Vpmovzxdq_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 35
		EVEX_Vpmovzxdq_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.W0 35

		EVEX_Vpmovqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 35
		EVEX_Vpmovqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 35
		EVEX_Vpmovqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 35

		VEX_Vpermd_ymm_ymm_ymmm256,									// VEX.256.66.0F38.W0 36
		EVEX_Vpermd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F38.W0 36
		EVEX_Vpermd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F38.W0 36
		EVEX_Vpermq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 36
		EVEX_Vpermq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 36

		Pcmpgtq_xmm_xmmm128,										// 66 0F3837
		VEX_Vpcmpgtq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 37
		VEX_Vpcmpgtq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 37
		EVEX_Vpcmpgtq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 37
		EVEX_Vpcmpgtq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 37
		EVEX_Vpcmpgtq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 37

		Pminsb_xmm_xmmm128,											// 66 0F3838
		VEX_Vpminsb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 38
		VEX_Vpminsb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 38
		EVEX_Vpminsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 38
		EVEX_Vpminsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 38
		EVEX_Vpminsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 38

		EVEX_Vpmovm2d_xmm_k,										// EVEX.128.F3.0F38.W0 38
		EVEX_Vpmovm2d_ymm_k,										// EVEX.256.F3.0F38.W0 38
		EVEX_Vpmovm2d_zmm_k,										// EVEX.512.F3.0F38.W0 38
		EVEX_Vpmovm2q_xmm_k,										// EVEX.128.F3.0F38.W1 38
		EVEX_Vpmovm2q_ymm_k,										// EVEX.256.F3.0F38.W1 38
		EVEX_Vpmovm2q_zmm_k,										// EVEX.512.F3.0F38.W1 38

		Pminsd_xmm_xmmm128,											// 66 0F3839
		VEX_Vpminsd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 39
		VEX_Vpminsd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 39
		EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 39
		EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 39
		EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 39
		EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 39
		EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 39
		EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 39

		EVEX_Vpmovd2m_k_xmm,										// EVEX.128.F3.0F38.W0 39
		EVEX_Vpmovd2m_k_ymm,										// EVEX.256.F3.0F38.W0 39
		EVEX_Vpmovd2m_k_zmm,										// EVEX.512.F3.0F38.W0 39
		EVEX_Vpmovq2m_k_xmm,										// EVEX.128.F3.0F38.W1 39
		EVEX_Vpmovq2m_k_ymm,										// EVEX.256.F3.0F38.W1 39
		EVEX_Vpmovq2m_k_zmm,										// EVEX.512.F3.0F38.W1 39

		Pminuw_xmm_xmmm128,											// 66 0F383A
		VEX_Vpminuw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3A
		VEX_Vpminuw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3A
		EVEX_Vpminuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3A
		EVEX_Vpminuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3A
		EVEX_Vpminuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3A

		EVEX_Vpbroadcastmw2d_xmm_k,									// EVEX.128.F3.0F38.W0 3A
		EVEX_Vpbroadcastmw2d_ymm_k,									// EVEX.256.F3.0F38.W0 3A
		EVEX_Vpbroadcastmw2d_zmm_k,									// EVEX.512.F3.0F38.W0 3A

		Pminud_xmm_xmmm128,											// 66 0F383B
		VEX_Vpminud_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3B
		VEX_Vpminud_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3B
		EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3B
		EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3B
		EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3B
		EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3B
		EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3B
		EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3B

		Pmaxsb_xmm_xmmm128,											// 66 0F383C
		VEX_Vpmaxsb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3C
		VEX_Vpmaxsb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3C
		EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3C
		EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3C
		EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3C

		Pmaxsd_xmm_xmmm128,											// 66 0F383D
		VEX_Vpmaxsd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3D
		VEX_Vpmaxsd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3D
		EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3D
		EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3D
		EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3D
		EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3D
		EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3D
		EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3D

		Pmaxuw_xmm_xmmm128,											// 66 0F383E
		VEX_Vpmaxuw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3E
		VEX_Vpmaxuw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3E
		EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3E
		EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3E
		EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3E

		Pmaxud_xmm_xmmm128,											// 66 0F383F
		VEX_Vpmaxud_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3F
		VEX_Vpmaxud_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3F
		EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3F
		EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3F
		EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3F
		EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3F
		EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3F
		EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3F

		Pmulld_xmm_xmmm128,											// 66 0F3840
		VEX_Vpmulld_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 40
		VEX_Vpmulld_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 40
		EVEX_Vpmulld_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 40
		EVEX_Vpmulld_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 40
		EVEX_Vpmulld_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 40
		EVEX_Vpmullq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 40
		EVEX_Vpmullq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 40
		EVEX_Vpmullq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 40

		Phminposuw_xmm_xmmm128,										// 66 0F3841
		VEX_Vphminposuw_xmm_xmmm128,								// VEX.128.66.0F38.WIG 41

		EVEX_Vgetexpps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 42
		EVEX_Vgetexpps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 42
		EVEX_Vgetexpps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 42
		EVEX_Vgetexppd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 42
		EVEX_Vgetexppd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 42
		EVEX_Vgetexppd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 42

		EVEX_Vgetexpss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 43
		EVEX_Vgetexpsd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 43

		EVEX_Vplzcntd_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 44
		EVEX_Vplzcntd_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 44
		EVEX_Vplzcntd_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 44
		EVEX_Vplzcntq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 44
		EVEX_Vplzcntq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 44
		EVEX_Vplzcntq_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 44

		VEX_Vpsrlvd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 45
		VEX_Vpsrlvd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 45
		VEX_Vpsrlvq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W1 45
		VEX_Vpsrlvq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W1 45
		EVEX_Vpsrlvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 45
		EVEX_Vpsrlvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 45
		EVEX_Vpsrlvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 45
		EVEX_Vpsrlvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 45
		EVEX_Vpsrlvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 45
		EVEX_Vpsrlvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 45

		VEX_Vpsravd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 46
		VEX_Vpsravd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 46
		EVEX_Vpsravd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 46
		EVEX_Vpsravd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 46
		EVEX_Vpsravd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 46
		EVEX_Vpsravq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 46
		EVEX_Vpsravq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 46
		EVEX_Vpsravq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 46

		VEX_Vpsllvd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 47
		VEX_Vpsllvd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 47
		VEX_Vpsllvq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W1 47
		VEX_Vpsllvq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W1 47
		EVEX_Vpsllvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 47
		EVEX_Vpsllvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 47
		EVEX_Vpsllvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 47
		EVEX_Vpsllvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 47
		EVEX_Vpsllvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 47
		EVEX_Vpsllvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 47

		EVEX_Vrcp14ps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 4C
		EVEX_Vrcp14ps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 4C
		EVEX_Vrcp14ps_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 4C
		EVEX_Vrcp14pd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 4C
		EVEX_Vrcp14pd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 4C
		EVEX_Vrcp14pd_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 4C

		EVEX_Vrcp14ss_xmm_k1z_xmm_xmmm32,							// EVEX.LIG.66.0F38.W0 4D
		EVEX_Vrcp14sd_xmm_k1z_xmm_xmmm64,							// EVEX.LIG.66.0F38.W1 4D

		EVEX_Vrsqrt14ps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 4E
		EVEX_Vrsqrt14ps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 4E
		EVEX_Vrsqrt14ps_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 4E
		EVEX_Vrsqrt14pd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 4E
		EVEX_Vrsqrt14pd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 4E
		EVEX_Vrsqrt14pd_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 4E

		EVEX_Vrsqrt14ss_xmm_k1z_xmm_xmmm32,							// EVEX.LIG.66.0F38.W0 4F
		EVEX_Vrsqrt14sd_xmm_k1z_xmm_xmmm64,							// EVEX.LIG.66.0F38.W1 4F

		EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 50
		EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 50
		EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 50

		EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 51
		EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 51
		EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 51

		EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 52
		EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 52
		EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 52

		EVEX_Vdpbf16ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.F3.0F38.W0 52
		EVEX_Vdpbf16ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.F3.0F38.W0 52
		EVEX_Vdpbf16ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.F3.0F38.W0 52

		EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 52

		EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 53
		EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 53
		EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 53

		EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 53

		EVEX_Vpopcntb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 54
		EVEX_Vpopcntb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 54
		EVEX_Vpopcntb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 54
		EVEX_Vpopcntw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 54
		EVEX_Vpopcntw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 54
		EVEX_Vpopcntw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 54

		EVEX_Vpopcntd_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 55
		EVEX_Vpopcntd_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 55
		EVEX_Vpopcntd_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 55
		EVEX_Vpopcntq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 55
		EVEX_Vpopcntq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 55
		EVEX_Vpopcntq_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 55

		VEX_Vpbroadcastd_xmm_xmmm32,								// VEX.128.66.0F38.W0 58
		VEX_Vpbroadcastd_ymm_xmmm32,								// VEX.256.66.0F38.W0 58
		EVEX_Vpbroadcastd_xmm_k1z_xmmm32,							// EVEX.128.66.0F38.W0 58
		EVEX_Vpbroadcastd_ymm_k1z_xmmm32,							// EVEX.256.66.0F38.W0 58
		EVEX_Vpbroadcastd_zmm_k1z_xmmm32,							// EVEX.512.66.0F38.W0 58

		VEX_Vpbroadcastq_xmm_xmmm64,								// VEX.128.66.0F38.W0 59
		VEX_Vpbroadcastq_ymm_xmmm64,								// VEX.256.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_xmm_k1z_xmmm64,						// EVEX.128.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_ymm_k1z_xmmm64,						// EVEX.256.66.0F38.W0 59
		EVEX_Vbroadcasti32x2_zmm_k1z_xmmm64,						// EVEX.512.66.0F38.W0 59
		EVEX_Vpbroadcastq_xmm_k1z_xmmm64,							// EVEX.128.66.0F38.W1 59
		EVEX_Vpbroadcastq_ymm_k1z_xmmm64,							// EVEX.256.66.0F38.W1 59
		EVEX_Vpbroadcastq_zmm_k1z_xmmm64,							// EVEX.512.66.0F38.W1 59

		VEX_Vbroadcasti128_ymm_m128,								// VEX.256.66.0F38.W0 5A
		EVEX_Vbroadcasti32x4_ymm_k1z_m128,							// EVEX.256.66.0F38.W0 5A
		EVEX_Vbroadcasti32x4_zmm_k1z_m128,							// EVEX.512.66.0F38.W0 5A
		EVEX_Vbroadcasti64x2_ymm_k1z_m128,							// EVEX.256.66.0F38.W1 5A
		EVEX_Vbroadcasti64x2_zmm_k1z_m128,							// EVEX.512.66.0F38.W1 5A

		EVEX_Vbroadcasti32x8_zmm_k1z_m256,							// EVEX.512.66.0F38.W0 5B
		EVEX_Vbroadcasti64x4_zmm_k1z_m256,							// EVEX.512.66.0F38.W1 5B

		EVEX_Vpexpandb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 62
		EVEX_Vpexpandb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 62
		EVEX_Vpexpandb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 62
		EVEX_Vpexpandw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 62
		EVEX_Vpexpandw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 62
		EVEX_Vpexpandw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 62

		EVEX_Vpcompressb_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 63
		EVEX_Vpcompressb_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 63
		EVEX_Vpcompressb_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 63
		EVEX_Vpcompressw_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 63
		EVEX_Vpcompressw_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 63
		EVEX_Vpcompressw_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 63

		EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 64
		EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 64
		EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 64
		EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 64
		EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 64
		EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 64

		EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 65
		EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 65
		EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 65
		EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 65
		EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 65
		EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 65

		EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 66
		EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 66
		EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 66
		EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 66
		EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 66
		EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 66

		EVEX_Vpshldvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 70
		EVEX_Vpshldvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 70
		EVEX_Vpshldvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 70

		EVEX_Vpshldvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 71
		EVEX_Vpshldvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 71
		EVEX_Vpshldvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 71
		EVEX_Vpshldvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 71
		EVEX_Vpshldvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 71
		EVEX_Vpshldvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 71

		EVEX_Vpshrdvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 72
		EVEX_Vpshrdvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 72
		EVEX_Vpshrdvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 72

		EVEX_Vcvtneps2bf16_xmm_k1z_xmmm128b32,						// EVEX.128.F3.0F38.W0 72
		EVEX_Vcvtneps2bf16_xmm_k1z_ymmm256b32,						// EVEX.256.F3.0F38.W0 72
		EVEX_Vcvtneps2bf16_ymm_k1z_zmmm512b32,						// EVEX.512.F3.0F38.W0 72

		EVEX_Vcvtne2ps2bf16_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.F2.0F38.W0 72
		EVEX_Vcvtne2ps2bf16_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.F2.0F38.W0 72
		EVEX_Vcvtne2ps2bf16_zmm_k1z_zmm_zmmm512b32,					// EVEX.512.F2.0F38.W0 72

		EVEX_Vpshrdvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 73
		EVEX_Vpshrdvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 73
		EVEX_Vpshrdvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 73
		EVEX_Vpshrdvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 73
		EVEX_Vpshrdvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 73
		EVEX_Vpshrdvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 73

		EVEX_Vpermi2b_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 75
		EVEX_Vpermi2b_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 75
		EVEX_Vpermi2b_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 75
		EVEX_Vpermi2w_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 75
		EVEX_Vpermi2w_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 75
		EVEX_Vpermi2w_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 75

		EVEX_Vpermi2d_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 76
		EVEX_Vpermi2d_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 76
		EVEX_Vpermi2d_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 76
		EVEX_Vpermi2q_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 76
		EVEX_Vpermi2q_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 76
		EVEX_Vpermi2q_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 76

		EVEX_Vpermi2ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 77
		EVEX_Vpermi2ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 77
		EVEX_Vpermi2ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 77
		EVEX_Vpermi2pd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 77
		EVEX_Vpermi2pd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 77
		EVEX_Vpermi2pd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 77

		VEX_Vpbroadcastb_xmm_xmmm8,									// VEX.128.66.0F38.W0 78
		VEX_Vpbroadcastb_ymm_xmmm8,									// VEX.256.66.0F38.W0 78
		EVEX_Vpbroadcastb_xmm_k1z_xmmm8,							// EVEX.128.66.0F38.W0 78
		EVEX_Vpbroadcastb_ymm_k1z_xmmm8,							// EVEX.256.66.0F38.W0 78
		EVEX_Vpbroadcastb_zmm_k1z_xmmm8,							// EVEX.512.66.0F38.W0 78

		VEX_Vpbroadcastw_xmm_xmmm16,								// VEX.128.66.0F38.W0 79
		VEX_Vpbroadcastw_ymm_xmmm16,								// VEX.256.66.0F38.W0 79
		EVEX_Vpbroadcastw_xmm_k1z_xmmm16,							// EVEX.128.66.0F38.W0 79
		EVEX_Vpbroadcastw_ymm_k1z_xmmm16,							// EVEX.256.66.0F38.W0 79
		EVEX_Vpbroadcastw_zmm_k1z_xmmm16,							// EVEX.512.66.0F38.W0 79

		EVEX_Vpbroadcastb_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7A
		EVEX_Vpbroadcastb_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7A
		EVEX_Vpbroadcastb_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7A

		EVEX_Vpbroadcastw_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7B
		EVEX_Vpbroadcastw_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7B
		EVEX_Vpbroadcastw_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7B

		EVEX_Vpbroadcastd_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7C
		EVEX_Vpbroadcastd_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7C
		EVEX_Vpbroadcastd_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7C
		EVEX_Vpbroadcastq_xmm_k1z_r64,								// EVEX.128.66.0F38.W1 7C
		EVEX_Vpbroadcastq_ymm_k1z_r64,								// EVEX.256.66.0F38.W1 7C
		EVEX_Vpbroadcastq_zmm_k1z_r64,								// EVEX.512.66.0F38.W1 7C

		EVEX_Vpermt2b_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 7D
		EVEX_Vpermt2b_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 7D
		EVEX_Vpermt2b_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 7D
		EVEX_Vpermt2w_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 7D
		EVEX_Vpermt2w_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 7D
		EVEX_Vpermt2w_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 7D

		EVEX_Vpermt2d_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 7E
		EVEX_Vpermt2d_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 7E
		EVEX_Vpermt2d_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 7E
		EVEX_Vpermt2q_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 7E
		EVEX_Vpermt2q_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 7E
		EVEX_Vpermt2q_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 7E

		EVEX_Vpermt2ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 7F
		EVEX_Vpermt2ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 7F
		EVEX_Vpermt2ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 7F
		EVEX_Vpermt2pd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 7F
		EVEX_Vpermt2pd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 7F
		EVEX_Vpermt2pd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 7F

		Invept_r32_m128,											// 66 0F3880
		Invept_r64_m128,											// 66 0F3880

		Invvpid_r32_m128,											// 66 0F3881
		Invvpid_r64_m128,											// 66 0F3881

		Invpcid_r32_m128,											// 66 0F3882
		Invpcid_r64_m128,											// 66 0F3882

		EVEX_Vpmultishiftqb_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 83
		EVEX_Vpmultishiftqb_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 83
		EVEX_Vpmultishiftqb_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 83

		EVEX_Vexpandps_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 88
		EVEX_Vexpandps_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 88
		EVEX_Vexpandps_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 88
		EVEX_Vexpandpd_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 88
		EVEX_Vexpandpd_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 88
		EVEX_Vexpandpd_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 88

		EVEX_Vpexpandd_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 89
		EVEX_Vpexpandd_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 89
		EVEX_Vpexpandd_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 89
		EVEX_Vpexpandq_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 89
		EVEX_Vpexpandq_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 89
		EVEX_Vpexpandq_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 89

		EVEX_Vcompressps_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 8A
		EVEX_Vcompressps_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 8A
		EVEX_Vcompressps_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 8A
		EVEX_Vcompresspd_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 8A
		EVEX_Vcompresspd_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 8A
		EVEX_Vcompresspd_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 8A

		EVEX_Vpcompressd_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 8B
		EVEX_Vpcompressd_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 8B
		EVEX_Vpcompressd_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 8B
		EVEX_Vpcompressq_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 8B
		EVEX_Vpcompressq_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 8B
		EVEX_Vpcompressq_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 8B

		VEX_Vpmaskmovd_xmm_xmm_m128,								// VEX.128.66.0F38.W0 8C
		VEX_Vpmaskmovd_ymm_ymm_m256,								// VEX.256.66.0F38.W0 8C
		VEX_Vpmaskmovq_xmm_xmm_m128,								// VEX.128.66.0F38.W1 8C
		VEX_Vpmaskmovq_ymm_ymm_m256,								// VEX.256.66.0F38.W1 8C

		EVEX_Vpermb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 8D
		EVEX_Vpermb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 8D
		EVEX_Vpermb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 8D
		EVEX_Vpermw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 8D
		EVEX_Vpermw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 8D
		EVEX_Vpermw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 8D

		VEX_Vpmaskmovd_m128_xmm_xmm,								// VEX.128.66.0F38.W0 8E
		VEX_Vpmaskmovd_m256_ymm_ymm,								// VEX.256.66.0F38.W0 8E
		VEX_Vpmaskmovq_m128_xmm_xmm,								// VEX.128.66.0F38.W1 8E
		VEX_Vpmaskmovq_m256_ymm_ymm,								// VEX.256.66.0F38.W1 8E

		EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128,							// EVEX.128.66.0F38.W0 8F
		EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256,							// EVEX.256.66.0F38.W0 8F
		EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512,							// EVEX.512.66.0F38.W0 8F

		VEX_Vpgatherdd_xmm_vm32x_xmm,								// VEX.128.66.0F38.W0 90
		VEX_Vpgatherdd_ymm_vm32y_ymm,								// VEX.256.66.0F38.W0 90
		VEX_Vpgatherdq_xmm_vm32x_xmm,								// VEX.128.66.0F38.W1 90
		VEX_Vpgatherdq_ymm_vm32x_ymm,								// VEX.256.66.0F38.W1 90
		EVEX_Vpgatherdd_xmm_k1_vm32x,								// EVEX.128.66.0F38.W0 90
		EVEX_Vpgatherdd_ymm_k1_vm32y,								// EVEX.256.66.0F38.W0 90
		EVEX_Vpgatherdd_zmm_k1_vm32z,								// EVEX.512.66.0F38.W0 90
		EVEX_Vpgatherdq_xmm_k1_vm32x,								// EVEX.128.66.0F38.W1 90
		EVEX_Vpgatherdq_ymm_k1_vm32x,								// EVEX.256.66.0F38.W1 90
		EVEX_Vpgatherdq_zmm_k1_vm32y,								// EVEX.512.66.0F38.W1 90

		VEX_Vpgatherqd_xmm_vm64x_xmm,								// VEX.128.66.0F38.W0 91
		VEX_Vpgatherqd_xmm_vm64y_xmm,								// VEX.256.66.0F38.W0 91
		VEX_Vpgatherqq_xmm_vm64x_xmm,								// VEX.128.66.0F38.W1 91
		VEX_Vpgatherqq_ymm_vm64y_ymm,								// VEX.256.66.0F38.W1 91
		EVEX_Vpgatherqd_xmm_k1_vm64x,								// EVEX.128.66.0F38.W0 91
		EVEX_Vpgatherqd_xmm_k1_vm64y,								// EVEX.256.66.0F38.W0 91
		EVEX_Vpgatherqd_ymm_k1_vm64z,								// EVEX.512.66.0F38.W0 91
		EVEX_Vpgatherqq_xmm_k1_vm64x,								// EVEX.128.66.0F38.W1 91
		EVEX_Vpgatherqq_ymm_k1_vm64y,								// EVEX.256.66.0F38.W1 91
		EVEX_Vpgatherqq_zmm_k1_vm64z,								// EVEX.512.66.0F38.W1 91

		VEX_Vgatherdps_xmm_vm32x_xmm,								// VEX.128.66.0F38.W0 92
		VEX_Vgatherdps_ymm_vm32y_ymm,								// VEX.256.66.0F38.W0 92
		VEX_Vgatherdpd_xmm_vm32x_xmm,								// VEX.128.66.0F38.W1 92
		VEX_Vgatherdpd_ymm_vm32x_ymm,								// VEX.256.66.0F38.W1 92
		EVEX_Vgatherdps_xmm_k1_vm32x,								// EVEX.128.66.0F38.W0 92
		EVEX_Vgatherdps_ymm_k1_vm32y,								// EVEX.256.66.0F38.W0 92
		EVEX_Vgatherdps_zmm_k1_vm32z,								// EVEX.512.66.0F38.W0 92
		EVEX_Vgatherdpd_xmm_k1_vm32x,								// EVEX.128.66.0F38.W1 92
		EVEX_Vgatherdpd_ymm_k1_vm32x,								// EVEX.256.66.0F38.W1 92
		EVEX_Vgatherdpd_zmm_k1_vm32y,								// EVEX.512.66.0F38.W1 92

		VEX_Vgatherqps_xmm_vm64x_xmm,								// VEX.128.66.0F38.W0 93
		VEX_Vgatherqps_xmm_vm64y_xmm,								// VEX.256.66.0F38.W0 93
		VEX_Vgatherqpd_xmm_vm64x_xmm,								// VEX.128.66.0F38.W1 93
		VEX_Vgatherqpd_ymm_vm64y_ymm,								// VEX.256.66.0F38.W1 93
		EVEX_Vgatherqps_xmm_k1_vm64x,								// EVEX.128.66.0F38.W0 93
		EVEX_Vgatherqps_xmm_k1_vm64y,								// EVEX.256.66.0F38.W0 93
		EVEX_Vgatherqps_ymm_k1_vm64z,								// EVEX.512.66.0F38.W0 93
		EVEX_Vgatherqpd_xmm_k1_vm64x,								// EVEX.128.66.0F38.W1 93
		EVEX_Vgatherqpd_ymm_k1_vm64y,								// EVEX.256.66.0F38.W1 93
		EVEX_Vgatherqpd_zmm_k1_vm64z,								// EVEX.512.66.0F38.W1 93

		VEX_Vfmaddsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 96
		VEX_Vfmaddsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 96
		VEX_Vfmaddsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 96
		VEX_Vfmaddsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 96
		EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 96
		EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 96
		EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 96
		EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 96
		EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 96
		EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 96

		VEX_Vfmsubadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 97
		VEX_Vfmsubadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 97
		VEX_Vfmsubadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 97
		VEX_Vfmsubadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 97
		EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 97
		EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 97
		EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 97
		EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 97
		EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 97
		EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 97

		VEX_Vfmadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 98
		VEX_Vfmadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 98
		VEX_Vfmadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 98
		VEX_Vfmadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 98
		EVEX_Vfmadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 98
		EVEX_Vfmadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 98
		EVEX_Vfmadd132ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 98
		EVEX_Vfmadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 98
		EVEX_Vfmadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 98
		EVEX_Vfmadd132pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 98

		VEX_Vfmadd132ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 99
		VEX_Vfmadd132sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 99
		EVEX_Vfmadd132ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 99
		EVEX_Vfmadd132sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 99

		VEX_Vfmsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9A
		VEX_Vfmsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9A
		VEX_Vfmsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9A
		VEX_Vfmsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9A
		EVEX_Vfmsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9A
		EVEX_Vfmsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9A
		EVEX_Vfmsub132ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 9A
		EVEX_Vfmsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9A
		EVEX_Vfmsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9A
		EVEX_Vfmsub132pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 9A

		EVEX_V4fmaddps_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 9A

		VEX_Vfmsub132ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 9B
		VEX_Vfmsub132sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 9B
		EVEX_Vfmsub132ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 9B
		EVEX_Vfmsub132sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 9B

		EVEX_V4fmaddss_xmm_k1z_xmmp3_m128,							// EVEX.LIG.F2.0F38.W0 9B

		VEX_Vfnmadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9C
		VEX_Vfnmadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9C
		VEX_Vfnmadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9C
		VEX_Vfnmadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9C
		EVEX_Vfnmadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9C
		EVEX_Vfnmadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9C
		EVEX_Vfnmadd132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 9C
		EVEX_Vfnmadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9C
		EVEX_Vfnmadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9C
		EVEX_Vfnmadd132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 9C

		VEX_Vfnmadd132ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 9D
		VEX_Vfnmadd132sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 9D
		EVEX_Vfnmadd132ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 9D
		EVEX_Vfnmadd132sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 9D

		VEX_Vfnmsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9E
		VEX_Vfnmsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9E
		VEX_Vfnmsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9E
		VEX_Vfnmsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9E
		EVEX_Vfnmsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9E
		EVEX_Vfnmsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9E
		EVEX_Vfnmsub132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 9E
		EVEX_Vfnmsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9E
		EVEX_Vfnmsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9E
		EVEX_Vfnmsub132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 9E

		VEX_Vfnmsub132ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 9F
		VEX_Vfnmsub132sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 9F
		EVEX_Vfnmsub132ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 9F
		EVEX_Vfnmsub132sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 9F

		EVEX_Vpscatterdd_vm32x_k1_xmm,								// EVEX.128.66.0F38.W0 A0
		EVEX_Vpscatterdd_vm32y_k1_ymm,								// EVEX.256.66.0F38.W0 A0
		EVEX_Vpscatterdd_vm32z_k1_zmm,								// EVEX.512.66.0F38.W0 A0
		EVEX_Vpscatterdq_vm32x_k1_xmm,								// EVEX.128.66.0F38.W1 A0
		EVEX_Vpscatterdq_vm32x_k1_ymm,								// EVEX.256.66.0F38.W1 A0
		EVEX_Vpscatterdq_vm32y_k1_zmm,								// EVEX.512.66.0F38.W1 A0

		EVEX_Vpscatterqd_vm64x_k1_xmm,								// EVEX.128.66.0F38.W0 A1
		EVEX_Vpscatterqd_vm64y_k1_xmm,								// EVEX.256.66.0F38.W0 A1
		EVEX_Vpscatterqd_vm64z_k1_ymm,								// EVEX.512.66.0F38.W0 A1
		EVEX_Vpscatterqq_vm64x_k1_xmm,								// EVEX.128.66.0F38.W1 A1
		EVEX_Vpscatterqq_vm64y_k1_ymm,								// EVEX.256.66.0F38.W1 A1
		EVEX_Vpscatterqq_vm64z_k1_zmm,								// EVEX.512.66.0F38.W1 A1

		EVEX_Vscatterdps_vm32x_k1_xmm,								// EVEX.128.66.0F38.W0 A2
		EVEX_Vscatterdps_vm32y_k1_ymm,								// EVEX.256.66.0F38.W0 A2
		EVEX_Vscatterdps_vm32z_k1_zmm,								// EVEX.512.66.0F38.W0 A2
		EVEX_Vscatterdpd_vm32x_k1_xmm,								// EVEX.128.66.0F38.W1 A2
		EVEX_Vscatterdpd_vm32x_k1_ymm,								// EVEX.256.66.0F38.W1 A2
		EVEX_Vscatterdpd_vm32y_k1_zmm,								// EVEX.512.66.0F38.W1 A2

		EVEX_Vscatterqps_vm64x_k1_xmm,								// EVEX.128.66.0F38.W0 A3
		EVEX_Vscatterqps_vm64y_k1_xmm,								// EVEX.256.66.0F38.W0 A3
		EVEX_Vscatterqps_vm64z_k1_ymm,								// EVEX.512.66.0F38.W0 A3
		EVEX_Vscatterqpd_vm64x_k1_xmm,								// EVEX.128.66.0F38.W1 A3
		EVEX_Vscatterqpd_vm64y_k1_ymm,								// EVEX.256.66.0F38.W1 A3
		EVEX_Vscatterqpd_vm64z_k1_zmm,								// EVEX.512.66.0F38.W1 A3

		VEX_Vfmaddsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A6
		VEX_Vfmaddsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A6
		VEX_Vfmaddsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A6
		VEX_Vfmaddsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A6
		EVEX_Vfmaddsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A6
		EVEX_Vfmaddsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A6
		EVEX_Vfmaddsub213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 A6
		EVEX_Vfmaddsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A6
		EVEX_Vfmaddsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A6
		EVEX_Vfmaddsub213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 A6

		VEX_Vfmsubadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A7
		VEX_Vfmsubadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A7
		VEX_Vfmsubadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A7
		VEX_Vfmsubadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A7
		EVEX_Vfmsubadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A7
		EVEX_Vfmsubadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A7
		EVEX_Vfmsubadd213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 A7
		EVEX_Vfmsubadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A7
		EVEX_Vfmsubadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A7
		EVEX_Vfmsubadd213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 A7

		VEX_Vfmadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A8
		VEX_Vfmadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A8
		VEX_Vfmadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A8
		VEX_Vfmadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A8
		EVEX_Vfmadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A8
		EVEX_Vfmadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A8
		EVEX_Vfmadd213ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 A8
		EVEX_Vfmadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A8
		EVEX_Vfmadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A8
		EVEX_Vfmadd213pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 A8

		VEX_Vfmadd213ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 A9
		VEX_Vfmadd213sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 A9
		EVEX_Vfmadd213ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 A9
		EVEX_Vfmadd213sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 A9

		VEX_Vfmsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AA
		VEX_Vfmsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AA
		VEX_Vfmsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AA
		VEX_Vfmsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AA
		EVEX_Vfmsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AA
		EVEX_Vfmsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AA
		EVEX_Vfmsub213ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 AA
		EVEX_Vfmsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AA
		EVEX_Vfmsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AA
		EVEX_Vfmsub213pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 AA

		EVEX_V4fnmaddps_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 AA

		VEX_Vfmsub213ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 AB
		VEX_Vfmsub213sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 AB
		EVEX_Vfmsub213ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 AB
		EVEX_Vfmsub213sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 AB

		EVEX_V4fnmaddss_xmm_k1z_xmmp3_m128,							// EVEX.LIG.F2.0F38.W0 AB

		VEX_Vfnmadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AC
		VEX_Vfnmadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AC
		VEX_Vfnmadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AC
		VEX_Vfnmadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AC
		EVEX_Vfnmadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AC
		EVEX_Vfnmadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AC
		EVEX_Vfnmadd213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 AC
		EVEX_Vfnmadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AC
		EVEX_Vfnmadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AC
		EVEX_Vfnmadd213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 AC

		VEX_Vfnmadd213ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 AD
		VEX_Vfnmadd213sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 AD
		EVEX_Vfnmadd213ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 AD
		EVEX_Vfnmadd213sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 AD

		VEX_Vfnmsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AE
		VEX_Vfnmsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AE
		VEX_Vfnmsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AE
		VEX_Vfnmsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AE
		EVEX_Vfnmsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AE
		EVEX_Vfnmsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AE
		EVEX_Vfnmsub213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 AE
		EVEX_Vfnmsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AE
		EVEX_Vfnmsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AE
		EVEX_Vfnmsub213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 AE

		VEX_Vfnmsub213ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 AF
		VEX_Vfnmsub213sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 AF
		EVEX_Vfnmsub213ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 AF
		EVEX_Vfnmsub213sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 AF

		EVEX_Vpmadd52luq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B4
		EVEX_Vpmadd52luq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B4
		EVEX_Vpmadd52luq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 B4

		EVEX_Vpmadd52huq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B5
		EVEX_Vpmadd52huq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B5
		EVEX_Vpmadd52huq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 B5

		VEX_Vfmaddsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B6
		VEX_Vfmaddsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B6
		VEX_Vfmaddsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B6
		VEX_Vfmaddsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B6
		EVEX_Vfmaddsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B6
		EVEX_Vfmaddsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B6
		EVEX_Vfmaddsub231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 B6
		EVEX_Vfmaddsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B6
		EVEX_Vfmaddsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B6
		EVEX_Vfmaddsub231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 B6

		VEX_Vfmsubadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B7
		VEX_Vfmsubadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B7
		VEX_Vfmsubadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B7
		VEX_Vfmsubadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B7
		EVEX_Vfmsubadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B7
		EVEX_Vfmsubadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B7
		EVEX_Vfmsubadd231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 B7
		EVEX_Vfmsubadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B7
		EVEX_Vfmsubadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B7
		EVEX_Vfmsubadd231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 B7

		VEX_Vfmadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B8
		VEX_Vfmadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B8
		VEX_Vfmadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B8
		VEX_Vfmadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B8
		EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B8
		EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B8
		EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 B8
		EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B8
		EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B8
		EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 B8

		VEX_Vfmadd231ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 B9
		VEX_Vfmadd231sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 B9
		EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 B9
		EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 B9

		VEX_Vfmsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BA
		VEX_Vfmsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BA
		VEX_Vfmsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BA
		VEX_Vfmsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BA
		EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BA
		EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BA
		EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 BA
		EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BA
		EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BA
		EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 BA

		VEX_Vfmsub231ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 BB
		VEX_Vfmsub231sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 BB
		EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 BB
		EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 BB

		VEX_Vfnmadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BC
		VEX_Vfnmadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BC
		VEX_Vfnmadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BC
		VEX_Vfnmadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BC
		EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BC
		EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BC
		EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 BC
		EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BC
		EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BC
		EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 BC

		VEX_Vfnmadd231ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 BD
		VEX_Vfnmadd231sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 BD
		EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 BD
		EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 BD

		VEX_Vfnmsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BE
		VEX_Vfnmsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BE
		VEX_Vfnmsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BE
		VEX_Vfnmsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BE
		EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BE
		EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BE
		EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 BE
		EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BE
		EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BE
		EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 BE

		VEX_Vfnmsub231ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 BF
		VEX_Vfnmsub231sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 BF
		EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 BF
		EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 BF

		EVEX_Vpconflictd_xmm_k1z_xmmm128b32,						// EVEX.128.66.0F38.W0 C4
		EVEX_Vpconflictd_ymm_k1z_ymmm256b32,						// EVEX.256.66.0F38.W0 C4
		EVEX_Vpconflictd_zmm_k1z_zmmm512b32,						// EVEX.512.66.0F38.W0 C4
		EVEX_Vpconflictq_xmm_k1z_xmmm128b64,						// EVEX.128.66.0F38.W1 C4
		EVEX_Vpconflictq_ymm_k1z_ymmm256b64,						// EVEX.256.66.0F38.W1 C4
		EVEX_Vpconflictq_zmm_k1z_zmmm512b64,						// EVEX.512.66.0F38.W1 C4

		EVEX_Vgatherpf0dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /1
		EVEX_Vgatherpf0dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /1
		EVEX_Vgatherpf1dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /2
		EVEX_Vgatherpf1dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /2
		EVEX_Vscatterpf0dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /5
		EVEX_Vscatterpf0dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /5
		EVEX_Vscatterpf1dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /6
		EVEX_Vscatterpf1dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /6

		EVEX_Vgatherpf0qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /1
		EVEX_Vgatherpf0qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /1
		EVEX_Vgatherpf1qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /2
		EVEX_Vgatherpf1qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /2
		EVEX_Vscatterpf0qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /5
		EVEX_Vscatterpf0qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /5
		EVEX_Vscatterpf1qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /6
		EVEX_Vscatterpf1qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /6

		Sha1nexte_xmm_xmmm128,										// 0F38C8

		EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 C8
		EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 C8

		Sha1msg1_xmm_xmmm128,										// 0F38C9

		Sha1msg2_xmm_xmmm128,										// 0F38CA

		EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 CA
		EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 CA

		Sha256rnds2_xmm_xmmm128,									// 0F38CB

		EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 CB
		EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 CB

		Sha256msg1_xmm_xmmm128,										// 0F38CC

		EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 CC
		EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 CC

		Sha256msg2_xmm_xmmm128,										// 0F38CD

		EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 CD
		EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 CD

		Gf2p8mulb_xmm_xmmm128,										// 66 0F38CF
		VEX_Vgf2p8mulb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 CF
		VEX_Vgf2p8mulb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 CF
		EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F38.W0 CF
		EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F38.W0 CF
		EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F38.W0 CF

		Aesimc_xmm_xmmm128,											// 66 0F38DB
		VEX_Vaesimc_xmm_xmmm128,									// VEX.128.66.0F38.WIG DB

		Aesenc_xmm_xmmm128,											// 66 0F38DC
		VEX_Vaesenc_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG DC
		VEX_Vaesenc_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG DC
		EVEX_Vaesenc_xmm_xmm_xmmm128,								// EVEX.128.66.0F38.WIG DC
		EVEX_Vaesenc_ymm_ymm_ymmm256,								// EVEX.256.66.0F38.WIG DC
		EVEX_Vaesenc_zmm_zmm_zmmm512,								// EVEX.512.66.0F38.WIG DC

		Aesenclast_xmm_xmmm128,										// 66 0F38DD
		VEX_Vaesenclast_xmm_xmm_xmmm128,							// VEX.128.66.0F38.WIG DD
		VEX_Vaesenclast_ymm_ymm_ymmm256,							// VEX.256.66.0F38.WIG DD
		EVEX_Vaesenclast_xmm_xmm_xmmm128,							// EVEX.128.66.0F38.WIG DD
		EVEX_Vaesenclast_ymm_ymm_ymmm256,							// EVEX.256.66.0F38.WIG DD
		EVEX_Vaesenclast_zmm_zmm_zmmm512,							// EVEX.512.66.0F38.WIG DD

		Aesdec_xmm_xmmm128,											// 66 0F38DE
		VEX_Vaesdec_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG DE
		VEX_Vaesdec_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG DE
		EVEX_Vaesdec_xmm_xmm_xmmm128,								// EVEX.128.66.0F38.WIG DE
		EVEX_Vaesdec_ymm_ymm_ymmm256,								// EVEX.256.66.0F38.WIG DE
		EVEX_Vaesdec_zmm_zmm_zmmm512,								// EVEX.512.66.0F38.WIG DE

		Aesdeclast_xmm_xmmm128,										// 66 0F38DF
		VEX_Vaesdeclast_xmm_xmm_xmmm128,							// VEX.128.66.0F38.WIG DF
		VEX_Vaesdeclast_ymm_ymm_ymmm256,							// VEX.256.66.0F38.WIG DF
		EVEX_Vaesdeclast_xmm_xmm_xmmm128,							// EVEX.128.66.0F38.WIG DF
		EVEX_Vaesdeclast_ymm_ymm_ymmm256,							// EVEX.256.66.0F38.WIG DF
		EVEX_Vaesdeclast_zmm_zmm_zmmm512,							// EVEX.512.66.0F38.WIG DF

		Movbe_r16_m16,												// o16 0F38F0
		Movbe_r32_m32,												// o32 0F38F0
		Movbe_r64_m64,												// REX.W 0F38F0

		Movbe_m16_r16,												// o16 0F38F1
		Movbe_m32_r32,												// o32 0F38F1
		Movbe_m64_r64,												// REX.W 0F38F1

		Crc32_r32_rm8,												// F2 0F38F0
		Crc32_r64_rm8,												// F2 REX.W 0F38F0

		Crc32_r32_rm16,												// o16 F2 0F38F1
		Crc32_r32_rm32,												// o32 F2 0F38F1
		Crc32_r64_rm64,												// F2 REX.W 0F38F1

		VEX_Andn_r32_r32_rm32,										// VEX.L0.0F38.W0 F2
		VEX_Andn_r64_r64_rm64,										// VEX.L0.0F38.W1 F2

		VEX_Blsr_r32_rm32,											// VEX.L0.0F38.W0 F3 /1
		VEX_Blsr_r64_rm64,											// VEX.L0.0F38.W1 F3 /1
		VEX_Blsmsk_r32_rm32,										// VEX.L0.0F38.W0 F3 /2
		VEX_Blsmsk_r64_rm64,										// VEX.L0.0F38.W1 F3 /2
		VEX_Blsi_r32_rm32,											// VEX.L0.0F38.W0 F3 /3
		VEX_Blsi_r64_rm64,											// VEX.L0.0F38.W1 F3 /3

		VEX_Bzhi_r32_rm32_r32,										// VEX.L0.0F38.W0 F5
		VEX_Bzhi_r64_rm64_r64,										// VEX.L0.0F38.W1 F5

		Wrussd_m32_r32,												// 66 0F38F5
		Wrussq_m64_r64,												// 66 REX.W 0F38F5

		VEX_Pext_r32_r32_rm32,										// VEX.L0.F3.0F38.W0 F5
		VEX_Pext_r64_r64_rm64,										// VEX.L0.F3.0F38.W1 F5

		VEX_Pdep_r32_r32_rm32,										// VEX.L0.F2.0F38.W0 F5
		VEX_Pdep_r64_r64_rm64,										// VEX.L0.F2.0F38.W1 F5

		Wrssd_m32_r32,												// 0F38F6
		Wrssq_m64_r64,												// REX.W 0F38F6

		Adcx_r32_rm32,												// 66 0F38F6
		Adcx_r64_rm64,												// 66 REX.W 0F38F6

		Adox_r32_rm32,												// F3 0F38F6
		Adox_r64_rm64,												// F3 REX.W 0F38F6

		VEX_Mulx_r32_r32_rm32,										// VEX.L0.F2.0F38.W0 F6
		VEX_Mulx_r64_r64_rm64,										// VEX.L0.F2.0F38.W1 F6

		VEX_Bextr_r32_rm32_r32,										// VEX.L0.0F38.W0 F7
		VEX_Bextr_r64_rm64_r64,										// VEX.L0.0F38.W1 F7

		VEX_Shlx_r32_rm32_r32,										// VEX.L0.66.0F38.W0 F7
		VEX_Shlx_r64_rm64_r64,										// VEX.L0.66.0F38.W1 F7

		VEX_Sarx_r32_rm32_r32,										// VEX.L0.F3.0F38.W0 F7
		VEX_Sarx_r64_rm64_r64,										// VEX.L0.F3.0F38.W1 F7

		VEX_Shrx_r32_rm32_r32,										// VEX.L0.F2.0F38.W0 F7
		VEX_Shrx_r64_rm64_r64,										// VEX.L0.F2.0F38.W1 F7

		Movdir64b_r16_m512,											// a16 66 0F38F8
		Movdir64b_r32_m512,											// a32 66 0F38F8
		Movdir64b_r64_m512,											// a64 66 0F38F8

		Movdiri_m32_r32,											// 0F38F9
		Movdiri_m64_r64,											// REX.W 0F38F9

		// 0F3Axx opcodes

		VEX_Vpermq_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W1 00
		EVEX_Vpermq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 00
		EVEX_Vpermq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 00

		VEX_Vpermpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W1 01
		EVEX_Vpermpd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 01
		EVEX_Vpermpd_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 01

		VEX_Vpblendd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 02
		VEX_Vpblendd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.W0 02

		EVEX_Valignd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 03
		EVEX_Valignd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 03
		EVEX_Valignd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 03
		EVEX_Valignq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 03
		EVEX_Valignq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 03
		EVEX_Valignq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 03

		VEX_Vpermilps_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.W0 04
		VEX_Vpermilps_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W0 04
		EVEX_Vpermilps_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 04
		EVEX_Vpermilps_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 04
		EVEX_Vpermilps_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 04

		VEX_Vpermilpd_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.W0 05
		VEX_Vpermilpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W0 05
		EVEX_Vpermilpd_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 05
		EVEX_Vpermilpd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 05
		EVEX_Vpermilpd_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 05

		VEX_Vperm2f128_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.W0 06

		Roundps_xmm_xmmm128_imm8,									// 66 0F3A08
		VEX_Vroundps_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 08
		VEX_Vroundps_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 08
		EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 08
		EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 08
		EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 08

		Roundpd_xmm_xmmm128_imm8,									// 66 0F3A09
		VEX_Vroundpd_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 09
		VEX_Vroundpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 09
		EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 09
		EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 09
		EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 09

		Roundss_xmm_xmmm32_imm8,									// 66 0F3A0A
		VEX_Vroundss_xmm_xmm_xmmm32_imm8,							// VEX.LIG.66.0F3A.WIG 0A
		EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 0A

		Roundsd_xmm_xmmm64_imm8,									// 66 0F3A0B
		VEX_Vroundsd_xmm_xmm_xmmm64_imm8,							// VEX.LIG.66.0F3A.WIG 0B
		EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 0B

		Blendps_xmm_xmmm128_imm8,									// 66 0F3A0C
		VEX_Vblendps_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0C
		VEX_Vblendps_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0C

		Blendpd_xmm_xmmm128_imm8,									// 66 0F3A0D
		VEX_Vblendpd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0D
		VEX_Vblendpd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0D

		Pblendw_xmm_xmmm128_imm8,									// 66 0F3A0E
		VEX_Vpblendw_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0E
		VEX_Vpblendw_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0E

		Palignr_mm_mmm64_imm8,										// 0F3A0F

		Palignr_xmm_xmmm128_imm8,									// 66 0F3A0F
		VEX_Vpalignr_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0F
		VEX_Vpalignr_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0F
		EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.WIG 0F
		EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.WIG 0F
		EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.WIG 0F

		Pextrb_r32m8_xmm_imm8,										// 66 0F3A14
		Pextrb_r64m8_xmm_imm8,										// 66 REX.W 0F3A14
		VEX_Vpextrb_r32m8_xmm_imm8,									// VEX.128.66.0F3A.W0 14
		VEX_Vpextrb_r64m8_xmm_imm8,									// VEX.128.66.0F3A.W1 14
		EVEX_Vpextrb_r32m8_xmm_imm8,								// EVEX.128.66.0F3A.W0 14
		EVEX_Vpextrb_r64m8_xmm_imm8,								// EVEX.128.66.0F3A.W1 14

		Pextrw_r32m16_xmm_imm8,										// 66 0F3A15
		Pextrw_r64m16_xmm_imm8,										// 66 REX.W 0F3A15
		VEX_Vpextrw_r32m16_xmm_imm8,								// VEX.128.66.0F3A.W0 15
		VEX_Vpextrw_r64m16_xmm_imm8,								// VEX.128.66.0F3A.W1 15
		EVEX_Vpextrw_r32m16_xmm_imm8,								// EVEX.128.66.0F3A.W0 15
		EVEX_Vpextrw_r64m16_xmm_imm8,								// EVEX.128.66.0F3A.W1 15

		Pextrd_rm32_xmm_imm8,										// 66 0F3A16
		Pextrq_rm64_xmm_imm8,										// 66 REX.W 0F3A16
		VEX_Vpextrd_rm32_xmm_imm8,									// VEX.128.66.0F3A.W0 16
		VEX_Vpextrq_rm64_xmm_imm8,									// VEX.128.66.0F3A.W1 16
		EVEX_Vpextrd_rm32_xmm_imm8,									// EVEX.128.66.0F3A.W0 16
		EVEX_Vpextrq_rm64_xmm_imm8,									// EVEX.128.66.0F3A.W1 16

		Extractps_rm32_xmm_imm8,									// 66 0F3A17
		Extractps_rm64_xmm_imm8,									// 66 REX.W 0F3A17
		VEX_Vextractps_rm32_xmm_imm8,								// VEX.128.66.0F3A.W0 17
		VEX_Vextractps_rm64_xmm_imm8,								// VEX.128.66.0F3A.W1 17
		EVEX_Vextractps_rm32_xmm_imm8,								// EVEX.128.66.0F3A.W0 17
		EVEX_Vextractps_rm64_xmm_imm8,								// EVEX.128.66.0F3A.W1 17

		VEX_Vinsertf128_ymm_ymm_xmmm128_imm8,						// VEX.256.66.0F3A.W0 18
		EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W0 18
		EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W0 18
		EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W1 18
		EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W1 18

		VEX_Vextractf128_xmmm128_ymm_imm8,							// VEX.256.66.0F3A.W0 19
		EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W0 19
		EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 19
		EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W1 19
		EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 19

		EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W0 1A
		EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W1 1A

		EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 1B
		EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 1B

		VEX_Vcvtps2ph_xmmm64_xmm_imm8,								// VEX.128.66.0F3A.W0 1D
		VEX_Vcvtps2ph_xmmm128_ymm_imm8,								// VEX.256.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8,							// EVEX.128.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8,						// EVEX.256.66.0F3A.W0 1D
		EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae,					// EVEX.512.66.0F3A.W0 1D

		EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 1E
		EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 1E
		EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 1E
		EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 1E
		EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 1E
		EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 1E

		EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 1F
		EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 1F
		EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 1F
		EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 1F
		EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 1F
		EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 1F

		Pinsrb_xmm_r32m8_imm8,										// 66 0F3A20
		Pinsrb_xmm_r64m8_imm8,										// 66 REX.W 0F3A20
		VEX_Vpinsrb_xmm_xmm_r32m8_imm8,								// VEX.128.66.0F3A.W0 20
		VEX_Vpinsrb_xmm_xmm_r64m8_imm8,								// VEX.128.66.0F3A.W1 20
		EVEX_Vpinsrb_xmm_xmm_r32m8_imm8,							// EVEX.128.66.0F3A.W0 20
		EVEX_Vpinsrb_xmm_xmm_r64m8_imm8,							// EVEX.128.66.0F3A.W1 20

		Insertps_xmm_xmmm32_imm8,									// 66 0F3A21
		VEX_Vinsertps_xmm_xmm_xmmm32_imm8,							// VEX.128.66.0F3A.WIG 21
		EVEX_Vinsertps_xmm_xmm_xmmm32_imm8,							// EVEX.128.66.0F3A.W0 21

		Pinsrd_xmm_rm32_imm8,										// 66 0F3A22
		Pinsrq_xmm_rm64_imm8,										// REX.W 66 0F3A22
		VEX_Vpinsrd_xmm_xmm_rm32_imm8,								// VEX.128.66.0F3A.W0 22
		VEX_Vpinsrq_xmm_xmm_rm64_imm8,								// VEX.128.66.0F3A.W1 22
		EVEX_Vpinsrd_xmm_xmm_rm32_imm8,								// EVEX.128.66.0F3A.W0 22
		EVEX_Vpinsrq_xmm_xmm_rm64_imm8,								// EVEX.128.66.0F3A.W1 22

		EVEX_Vshuff32x4_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 23
		EVEX_Vshuff32x4_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 23
		EVEX_Vshuff64x2_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 23
		EVEX_Vshuff64x2_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 23

		EVEX_Vpternlogd_xmm_k1z_xmm_xmmm128b32_imm8,				// EVEX.128.66.0F3A.W0 25
		EVEX_Vpternlogd_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 25
		EVEX_Vpternlogd_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 25
		EVEX_Vpternlogq_xmm_k1z_xmm_xmmm128b64_imm8,				// EVEX.128.66.0F3A.W1 25
		EVEX_Vpternlogq_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 25
		EVEX_Vpternlogq_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 25

		EVEX_Vgetmantps_xmm_k1z_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 26
		EVEX_Vgetmantps_ymm_k1z_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 26
		EVEX_Vgetmantps_zmm_k1z_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 26
		EVEX_Vgetmantpd_xmm_k1z_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 26
		EVEX_Vgetmantpd_ymm_k1z_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 26
		EVEX_Vgetmantpd_zmm_k1z_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 26

		EVEX_Vgetmantss_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 27
		EVEX_Vgetmantsd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 27

		VEX_Kshiftrw_k_k_imm8,										// VEX.L0.66.0F3A.W1 30
		VEX_Kshiftrb_k_k_imm8,										// VEX.L0.66.0F3A.W0 30
		VEX_Kshiftrq_k_k_imm8,										// VEX.L0.66.0F3A.W1 31
		VEX_Kshiftrd_k_k_imm8,										// VEX.L0.66.0F3A.W0 31

		VEX_Kshiftlw_k_k_imm8,										// VEX.L0.66.0F3A.W1 32
		VEX_Kshiftlb_k_k_imm8,										// VEX.L0.66.0F3A.W0 32
		VEX_Kshiftlq_k_k_imm8,										// VEX.L0.66.0F3A.W1 33
		VEX_Kshiftld_k_k_imm8,										// VEX.L0.66.0F3A.W0 33

		VEX_Vinserti128_ymm_ymm_xmmm128_imm8,						// VEX.256.66.0F3A.W0 38
		EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W0 38
		EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W0 38
		EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W1 38
		EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W1 38

		VEX_Vextracti128_xmmm128_ymm_imm8,							// VEX.256.66.0F3A.W0 39
		EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W0 39
		EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 39
		EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W1 39
		EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 39

		EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W0 3A
		EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W1 3A

		EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 3B
		EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 3B

		EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W0 3E
		EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W0 3E
		EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W0 3E
		EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W1 3E
		EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W1 3E
		EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W1 3E

		EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W0 3F
		EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W0 3F
		EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W0 3F
		EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W1 3F
		EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W1 3F
		EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W1 3F

		Dpps_xmm_xmmm128_imm8,										// 66 0F3A40
		VEX_Vdpps_xmm_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 40
		VEX_Vdpps_ymm_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 40

		Dppd_xmm_xmmm128_imm8,										// 66 0F3A41
		VEX_Vdppd_xmm_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 41

		Mpsadbw_xmm_xmmm128_imm8,									// 66 0F3A42
		VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 42
		VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 42
		EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8,					// EVEX.128.66.0F3A.W0 42
		EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8,					// EVEX.256.66.0F3A.W0 42
		EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8,					// EVEX.512.66.0F3A.W0 42

		EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 43
		EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 43
		EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 43
		EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 43

		Pclmulqdq_xmm_xmmm128_imm8,									// 66 0F3A44
		VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8,						// VEX.128.66.0F3A.WIG 44
		VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.WIG 44
		EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.WIG 44
		EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.WIG 44
		EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.WIG 44

		VEX_Vperm2i128_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.W0 46

		VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8,					// VEX.128.66.0F3A.W0 48
		VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8,					// VEX.256.66.0F3A.W0 48
		VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 48
		VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 48

		VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8,					// VEX.128.66.0F3A.W0 49
		VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8,					// VEX.256.66.0F3A.W0 49
		VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 49
		VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 49

		VEX_Vblendvps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4A
		VEX_Vblendvps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4A

		VEX_Vblendvpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4B
		VEX_Vblendvpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4B

		VEX_Vpblendvb_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4C
		VEX_Vpblendvb_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4C

		EVEX_Vrangeps_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 50
		EVEX_Vrangeps_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 50
		EVEX_Vrangeps_zmm_k1z_zmm_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 50
		EVEX_Vrangepd_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 50
		EVEX_Vrangepd_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 50
		EVEX_Vrangepd_zmm_k1z_zmm_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 50

		EVEX_Vrangess_xmm_k1z_xmm_xmmm32_imm8_sae,					// EVEX.LIG.66.0F3A.W0 51
		EVEX_Vrangesd_xmm_k1z_xmm_xmmm64_imm8_sae,					// EVEX.LIG.66.0F3A.W1 51

		EVEX_Vfixupimmps_xmm_k1z_xmm_xmmm128b32_imm8,				// EVEX.128.66.0F3A.W0 54
		EVEX_Vfixupimmps_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 54
		EVEX_Vfixupimmps_zmm_k1z_zmm_zmmm512b32_imm8_sae,			// EVEX.512.66.0F3A.W0 54
		EVEX_Vfixupimmpd_xmm_k1z_xmm_xmmm128b64_imm8,				// EVEX.128.66.0F3A.W1 54
		EVEX_Vfixupimmpd_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 54
		EVEX_Vfixupimmpd_zmm_k1z_zmm_zmmm512b64_imm8_sae,			// EVEX.512.66.0F3A.W1 54

		EVEX_Vfixupimmss_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 55
		EVEX_Vfixupimmsd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 55

		EVEX_Vreduceps_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 56
		EVEX_Vreduceps_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 56
		EVEX_Vreduceps_zmm_k1z_zmmm512b32_imm8_sae,					// EVEX.512.66.0F3A.W0 56
		EVEX_Vreducepd_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 56
		EVEX_Vreducepd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 56
		EVEX_Vreducepd_zmm_k1z_zmmm512b64_imm8_sae,					// EVEX.512.66.0F3A.W1 56

		EVEX_Vreducess_xmm_k1z_xmm_xmmm32_imm8_sae,					// EVEX.LIG.66.0F3A.W0 57
		EVEX_Vreducesd_xmm_k1z_xmm_xmmm64_imm8_sae,					// EVEX.LIG.66.0F3A.W1 57

		VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5C
		VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5C
		VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5C
		VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5C

		VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5D
		VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5D
		VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5D
		VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5D

		VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5E
		VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5E
		VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5E
		VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5E

		VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5F
		VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5F
		VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5F
		VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5F

		Pcmpestrm_xmm_xmmm128_imm8,									// 66 0F3A60
		Pcmpestrm64_xmm_xmmm128_imm8,								// REX.W 66 0F3A60
		VEX_Vpcmpestrm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 60
		VEX_Vpcmpestrm64_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W1 60

		Pcmpestri_xmm_xmmm128_imm8,									// 66 0F3A61
		Pcmpestri64_xmm_xmmm128_imm8,								// REX.W 66 0F3A61
		VEX_Vpcmpestri_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 61
		VEX_Vpcmpestri64_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W1 61

		Pcmpistrm_xmm_xmmm128_imm8,									// 66 0F3A62
		VEX_Vpcmpistrm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 62

		Pcmpistri_xmm_xmmm128_imm8,									// 66 0F3A63
		VEX_Vpcmpistri_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 63

		EVEX_Vfpclassps_k_k1_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 66
		EVEX_Vfpclassps_k_k1_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 66
		EVEX_Vfpclassps_k_k1_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 66
		EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 66
		EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 66
		EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 66

		EVEX_Vfpclassss_k_k1_xmmm32_imm8,							// EVEX.LIG.66.0F3A.W0 67
		EVEX_Vfpclasssd_k_k1_xmmm64_imm8,							// EVEX.LIG.66.0F3A.W1 67

		VEX_Vfmaddps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 68
		VEX_Vfmaddps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 68
		VEX_Vfmaddps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 68
		VEX_Vfmaddps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 68

		VEX_Vfmaddpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 69
		VEX_Vfmaddpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 69
		VEX_Vfmaddpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 69
		VEX_Vfmaddpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 69

		VEX_Vfmaddss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 6A
		VEX_Vfmaddss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 6A

		VEX_Vfmaddsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 6B
		VEX_Vfmaddsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 6B

		VEX_Vfmsubps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 6C
		VEX_Vfmsubps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 6C
		VEX_Vfmsubps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 6C
		VEX_Vfmsubps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 6C

		VEX_Vfmsubpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 6D
		VEX_Vfmsubpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 6D
		VEX_Vfmsubpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 6D
		VEX_Vfmsubpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 6D

		VEX_Vfmsubss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 6E
		VEX_Vfmsubss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 6E

		VEX_Vfmsubsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 6F
		VEX_Vfmsubsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 6F

		EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.W1 70
		EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.W1 70
		EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.W1 70

		EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 71
		EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 71
		EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 71
		EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 71
		EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 71
		EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 71

		EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.W1 72
		EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.W1 72
		EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.W1 72

		EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 73
		EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 73
		EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 73
		EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 73
		EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 73
		EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 73

		VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 78
		VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 78
		VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 78
		VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 78

		VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 79
		VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 79
		VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 79
		VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 79

		VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 7A
		VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 7A

		VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 7B
		VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 7B

		VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 7C
		VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 7C
		VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 7C
		VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 7C

		VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 7D
		VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 7D
		VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 7D
		VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 7D

		VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 7E
		VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 7E

		VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 7F
		VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 7F

		Sha1rnds4_xmm_xmmm128_imm8,									// 0F3ACC

		Gf2p8affineqb_xmm_xmmm128_imm8,								// 66 0F3ACE
		VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 CE
		VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 CE
		EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8,			// EVEX.128.66.0F3A.W1 CE
		EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8,			// EVEX.256.66.0F3A.W1 CE
		EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8,			// EVEX.512.66.0F3A.W1 CE

		Gf2p8affineinvqb_xmm_xmmm128_imm8,							// 66 0F3ACF
		VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 CF
		VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 CF
		EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8,			// EVEX.128.66.0F3A.W1 CF
		EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8,			// EVEX.256.66.0F3A.W1 CF
		EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8,			// EVEX.512.66.0F3A.W1 CF

		Aeskeygenassist_xmm_xmmm128_imm8,							// 66 0F3ADF
		VEX_Vaeskeygenassist_xmm_xmmm128_imm8,						// VEX.128.66.0F3A.WIG DF

		VEX_Rorx_r32_rm32_imm8,										// VEX.L0.F2.0F3A.W0 F0
		VEX_Rorx_r64_rm64_imm8,										// VEX.L0.F2.0F3A.W1 F0

		// XOP8 opcodes

		XOP_Vpmacssww_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 85

		XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 86

		XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 87

		XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 8E

		XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 8F

		XOP_Vpmacsww_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 95

		XOP_Vpmacswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 96

		XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 97

		XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 9E

		XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 9F

		XOP_Vpcmov_xmm_xmm_xmmm128_xmm,								// XOP.128.X8.W0 A2
		XOP_Vpcmov_ymm_ymm_ymmm256_ymm,								// XOP.256.X8.W0 A2
		XOP_Vpcmov_xmm_xmm_xmm_xmmm128,								// XOP.128.X8.W1 A2
		XOP_Vpcmov_ymm_ymm_ymm_ymmm256,								// XOP.256.X8.W1 A2

		XOP_Vpperm_xmm_xmm_xmmm128_xmm,								// XOP.128.X8.W0 A3
		XOP_Vpperm_xmm_xmm_xmm_xmmm128,								// XOP.128.X8.W1 A3

		XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 A6

		XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 B6

		XOP_Vprotb_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C0

		XOP_Vprotw_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C1

		XOP_Vprotd_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C2

		XOP_Vprotq_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C3

		XOP_Vpcomb_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CC

		XOP_Vpcomw_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CD

		XOP_Vpcomd_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CE

		XOP_Vpcomq_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CF

		XOP_Vpcomub_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EC

		XOP_Vpcomuw_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 ED

		XOP_Vpcomud_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EE

		XOP_Vpcomuq_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EF

		// XOP9 opcodes

		XOP_Blcfill_r32_rm32,										// XOP.L0.X9.W0 01 /1
		XOP_Blcfill_r64_rm64,										// XOP.L0.X9.W1 01 /1
		XOP_Blsfill_r32_rm32,										// XOP.L0.X9.W0 01 /2
		XOP_Blsfill_r64_rm64,										// XOP.L0.X9.W1 01 /2
		XOP_Blcs_r32_rm32,											// XOP.L0.X9.W0 01 /3
		XOP_Blcs_r64_rm64,											// XOP.L0.X9.W1 01 /3
		XOP_Tzmsk_r32_rm32,											// XOP.L0.X9.W0 01 /4
		XOP_Tzmsk_r64_rm64,											// XOP.L0.X9.W1 01 /4
		XOP_Blcic_r32_rm32,											// XOP.L0.X9.W0 01 /5
		XOP_Blcic_r64_rm64,											// XOP.L0.X9.W1 01 /5
		XOP_Blsic_r32_rm32,											// XOP.L0.X9.W0 01 /6
		XOP_Blsic_r64_rm64,											// XOP.L0.X9.W1 01 /6
		XOP_T1mskc_r32_rm32,										// XOP.L0.X9.W0 01 /7
		XOP_T1mskc_r64_rm64,										// XOP.L0.X9.W1 01 /7

		XOP_Blcmsk_r32_rm32,										// XOP.L0.X9.W0 02 /1
		XOP_Blcmsk_r64_rm64,										// XOP.L0.X9.W1 02 /1
		XOP_Blci_r32_rm32,											// XOP.L0.X9.W0 02 /6
		XOP_Blci_r64_rm64,											// XOP.L0.X9.W1 02 /6

		XOP_Llwpcb_r32,												// XOP.L0.X9.W0 12 /0
		XOP_Llwpcb_r64,												// XOP.L0.X9.W1 12 /0
		XOP_Slwpcb_r32,												// XOP.L0.X9.W0 12 /1
		XOP_Slwpcb_r64,												// XOP.L0.X9.W1 12 /1

		XOP_Vfrczps_xmm_xmmm128,									// XOP.128.X9.W0 80
		XOP_Vfrczps_ymm_ymmm256,									// XOP.256.X9.W0 80

		XOP_Vfrczpd_xmm_xmmm128,									// XOP.128.X9.W0 81
		XOP_Vfrczpd_ymm_ymmm256,									// XOP.256.X9.W0 81

		XOP_Vfrczss_xmm_xmmm32,										// XOP.128.X9.W0 82

		XOP_Vfrczsd_xmm_xmmm64,										// XOP.128.X9.W0 83

		XOP_Vprotb_xmm_xmmm128_xmm,									// XOP.128.X9.W0 90
		XOP_Vprotb_xmm_xmm_xmmm128,									// XOP.128.X9.W1 90

		XOP_Vprotw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 91
		XOP_Vprotw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 91

		XOP_Vprotd_xmm_xmmm128_xmm,									// XOP.128.X9.W0 92
		XOP_Vprotd_xmm_xmm_xmmm128,									// XOP.128.X9.W1 92

		XOP_Vprotq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 93
		XOP_Vprotq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 93

		XOP_Vpshlb_xmm_xmmm128_xmm,									// XOP.128.X9.W0 94
		XOP_Vpshlb_xmm_xmm_xmmm128,									// XOP.128.X9.W1 94

		XOP_Vpshlw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 95
		XOP_Vpshlw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 95

		XOP_Vpshld_xmm_xmmm128_xmm,									// XOP.128.X9.W0 96
		XOP_Vpshld_xmm_xmm_xmmm128,									// XOP.128.X9.W1 96

		XOP_Vpshlq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 97
		XOP_Vpshlq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 97

		XOP_Vpshab_xmm_xmmm128_xmm,									// XOP.128.X9.W0 98
		XOP_Vpshab_xmm_xmm_xmmm128,									// XOP.128.X9.W1 98

		XOP_Vpshaw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 99
		XOP_Vpshaw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 99

		XOP_Vpshad_xmm_xmmm128_xmm,									// XOP.128.X9.W0 9A
		XOP_Vpshad_xmm_xmm_xmmm128,									// XOP.128.X9.W1 9A

		XOP_Vpshaq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 9B
		XOP_Vpshaq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 9B

		XOP_Vphaddbw_xmm_xmmm128,									// XOP.128.X9.W0 C1

		XOP_Vphaddbd_xmm_xmmm128,									// XOP.128.X9.W0 C2

		XOP_Vphaddbq_xmm_xmmm128,									// XOP.128.X9.W0 C3

		XOP_Vphaddwd_xmm_xmmm128,									// XOP.128.X9.W0 C6

		XOP_Vphaddwq_xmm_xmmm128,									// XOP.128.X9.W0 C7

		XOP_Vphadddq_xmm_xmmm128,									// XOP.128.X9.W0 CB

		XOP_Vphaddubw_xmm_xmmm128,									// XOP.128.X9.W0 D1

		XOP_Vphaddubd_xmm_xmmm128,									// XOP.128.X9.W0 D2

		XOP_Vphaddubq_xmm_xmmm128,									// XOP.128.X9.W0 D3

		XOP_Vphadduwd_xmm_xmmm128,									// XOP.128.X9.W0 D6

		XOP_Vphadduwq_xmm_xmmm128,									// XOP.128.X9.W0 D7

		XOP_Vphaddudq_xmm_xmmm128,									// XOP.128.X9.W0 DB

		XOP_Vphsubbw_xmm_xmmm128,									// XOP.128.X9.W0 E1

		XOP_Vphsubwd_xmm_xmmm128,									// XOP.128.X9.W0 E2

		XOP_Vphsubdq_xmm_xmmm128,									// XOP.128.X9.W0 E3

		// XOPA opcodes

		XOP_Bextr_r32_rm32_imm32,									// XOP.L0.XA.W0 10
		XOP_Bextr_r64_rm64_imm32,									// XOP.L0.XA.W1 10

		XOP_Lwpins_r32_rm32_imm32,									// XOP.L0.XA.W0 12 /0
		XOP_Lwpins_r64_rm32_imm32,									// XOP.L0.XA.W1 12 /0
		XOP_Lwpval_r32_rm32_imm32,									// XOP.L0.XA.W0 12 /1
		XOP_Lwpval_r64_rm32_imm32,									// XOP.L0.XA.W1 12 /1

		// 3DNow! opcodes

		D3NOW_Pi2fw_mm_mmm64,										// 0F0F 0C
		D3NOW_Pi2fd_mm_mmm64,										// 0F0F 0D

		D3NOW_Pf2iw_mm_mmm64,										// 0F0F 1C
		D3NOW_Pf2id_mm_mmm64,										// 0F0F 1D

		D3NOW_Pfrcpv_mm_mmm64,										// 0F0F 86
		D3NOW_Pfrsqrtv_mm_mmm64,									// 0F0F 87

		D3NOW_Pfnacc_mm_mmm64,										// 0F0F 8A
		D3NOW_Pfpnacc_mm_mmm64,										// 0F0F 8E

		D3NOW_Pfcmpge_mm_mmm64,										// 0F0F 90
		D3NOW_Pfmin_mm_mmm64,										// 0F0F 94
		D3NOW_Pfrcp_mm_mmm64,										// 0F0F 96
		D3NOW_Pfrsqrt_mm_mmm64,										// 0F0F 97

		D3NOW_Pfsub_mm_mmm64,										// 0F0F 9A
		D3NOW_Pfadd_mm_mmm64,										// 0F0F 9E

		D3NOW_Pfcmpgt_mm_mmm64,										// 0F0F A0
		D3NOW_Pfmax_mm_mmm64,										// 0F0F A4
		D3NOW_Pfrcpit1_mm_mmm64,									// 0F0F A6
		D3NOW_Pfrsqit1_mm_mmm64,									// 0F0F A7

		D3NOW_Pfsubr_mm_mmm64,										// 0F0F AA
		D3NOW_Pfacc_mm_mmm64,										// 0F0F AE

		D3NOW_Pfcmpeq_mm_mmm64,										// 0F0F B0
		D3NOW_Pfmul_mm_mmm64,										// 0F0F B4
		D3NOW_Pfrcpit2_mm_mmm64,									// 0F0F B6
		D3NOW_Pmulhrw_mm_mmm64,										// 0F0F B7

		D3NOW_Pswapd_mm_mmm64,										// 0F0F BB
		D3NOW_Pavgusb_mm_mmm64,										// 0F0F BF

		// Misc

		/// <summary>
		/// A 'db' asm directive that can store 1-16 bytes
		/// </summary>
		DeclareByte,
		/// <summary>
		/// A 'dw' asm directive that can store 1-8 words
		/// </summary>
		DeclareWord,
		/// <summary>
		/// A 'dd' asm directive that can store 1-4 dwords
		/// </summary>
		DeclareDword,
		/// <summary>
		/// A 'dq' asm directive that can store 1-2 qwords
		/// </summary>
		DeclareQword,
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
	}
}
