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
		INVALID,													// <invalid>

		Add_rm8_r8,													// 00 /r
		Add_rm16_r16,												// o16 01 /r
		Add_rm32_r32,												// o32 01 /r
		Add_rm64_r64,												// REX.W 01 /r
		Add_r8_rm8,													// 02 /r
		Add_r16_rm16,												// o16 03 /r
		Add_r32_rm32,												// o32 03 /r
		Add_r64_rm64,												// REX.W 03 /r
		Add_AL_imm8,												// 04 ib
		Add_AX_imm16,												// o16 05 iw
		Add_EAX_imm32,												// o32 05 id
		Add_RAX_imm32,												// REX.W 05 id
		Pushw_ES,													// o16 06
		Pushd_ES,													// o32 06
		Popw_ES,													// o16 07
		Popd_ES,													// o32 07

		Or_rm8_r8,													// 08 /r
		Or_rm16_r16,												// o16 09 /r
		Or_rm32_r32,												// o32 09 /r
		Or_rm64_r64,												// REX.W 09 /r
		Or_r8_rm8,													// 0A /r
		Or_r16_rm16,												// o16 0B /r
		Or_r32_rm32,												// o32 0B /r
		Or_r64_rm64,												// REX.W 0B /r
		Or_AL_imm8,													// 0C ib
		Or_AX_imm16,												// o16 0D iw
		Or_EAX_imm32,												// o32 0D id
		Or_RAX_imm32,												// REX.W 0D id
		Pushw_CS,													// o16 0E
		Pushd_CS,													// o32 0E
		Popw_CS,													// o16 0F

		Adc_rm8_r8,													// 10 /r
		Adc_rm16_r16,												// o16 11 /r
		Adc_rm32_r32,												// o32 11 /r
		Adc_rm64_r64,												// REX.W 11 /r
		Adc_r8_rm8,													// 12 /r
		Adc_r16_rm16,												// o16 13 /r
		Adc_r32_rm32,												// o32 13 /r
		Adc_r64_rm64,												// REX.W 13 /r
		Adc_AL_imm8,												// 14 ib
		Adc_AX_imm16,												// o16 15 iw
		Adc_EAX_imm32,												// o32 15 id
		Adc_RAX_imm32,												// REX.W 15 id
		Pushw_SS,													// o16 16
		Pushd_SS,													// o32 16
		Popw_SS,													// o16 17
		Popd_SS,													// o32 17

		Sbb_rm8_r8,													// 18 /r
		Sbb_rm16_r16,												// o16 19 /r
		Sbb_rm32_r32,												// o32 19 /r
		Sbb_rm64_r64,												// REX.W 19 /r
		Sbb_r8_rm8,													// 1A /r
		Sbb_r16_rm16,												// o16 1B /r
		Sbb_r32_rm32,												// o32 1B /r
		Sbb_r64_rm64,												// REX.W 1B /r
		Sbb_AL_imm8,												// 1C ib
		Sbb_AX_imm16,												// o16 1D iw
		Sbb_EAX_imm32,												// o32 1D id
		Sbb_RAX_imm32,												// REX.W 1D id
		Pushw_DS,													// o16 1E
		Pushd_DS,													// o32 1E
		Popw_DS,													// o16 1F
		Popd_DS,													// o32 1F

		And_rm8_r8,													// 20 /r
		And_rm16_r16,												// o16 21 /r
		And_rm32_r32,												// o32 21 /r
		And_rm64_r64,												// REX.W 21 /r
		And_r8_rm8,													// 22 /r
		And_r16_rm16,												// o16 23 /r
		And_r32_rm32,												// o32 23 /r
		And_r64_rm64,												// REX.W 23 /r
		And_AL_imm8,												// 24 ib
		And_AX_imm16,												// o16 25 iw
		And_EAX_imm32,												// o32 25 id
		And_RAX_imm32,												// REX.W 25 id
		Daa,														// 27

		Sub_rm8_r8,													// 28 /r
		Sub_rm16_r16,												// o16 29 /r
		Sub_rm32_r32,												// o32 29 /r
		Sub_rm64_r64,												// REX.W 29 /r
		Sub_r8_rm8,													// 2A /r
		Sub_r16_rm16,												// o16 2B /r
		Sub_r32_rm32,												// o32 2B /r
		Sub_r64_rm64,												// REX.W 2B /r
		Sub_AL_imm8,												// 2C ib
		Sub_AX_imm16,												// o16 2D iw
		Sub_EAX_imm32,												// o32 2D id
		Sub_RAX_imm32,												// REX.W 2D id
		Das,														// 2F

		Xor_rm8_r8,													// 30 /r
		Xor_rm16_r16,												// o16 31 /r
		Xor_rm32_r32,												// o32 31 /r
		Xor_rm64_r64,												// REX.W 31 /r
		Xor_r8_rm8,													// 32 /r
		Xor_r16_rm16,												// o16 33 /r
		Xor_r32_rm32,												// o32 33 /r
		Xor_r64_rm64,												// REX.W 33 /r
		Xor_AL_imm8,												// 34 ib
		Xor_AX_imm16,												// o16 35 iw
		Xor_EAX_imm32,												// o32 35 id
		Xor_RAX_imm32,												// REX.W 35 id
		Aaa,														// 37

		Cmp_rm8_r8,													// 38 /r
		Cmp_rm16_r16,												// o16 39 /r
		Cmp_rm32_r32,												// o32 39 /r
		Cmp_rm64_r64,												// REX.W 39 /r
		Cmp_r8_rm8,													// 3A /r
		Cmp_r16_rm16,												// o16 3B /r
		Cmp_r32_rm32,												// o32 3B /r
		Cmp_r64_rm64,												// REX.W 3B /r
		Cmp_AL_imm8,												// 3C ib
		Cmp_AX_imm16,												// o16 3D iw
		Cmp_EAX_imm32,												// o32 3D id
		Cmp_RAX_imm32,												// REX.W 3D id
		Aas,														// 3F

		Inc_r16,													// o16 40+rw
		Inc_r32,													// o32 40+rd

		Dec_r16,													// o16 48+rw
		Dec_r32,													// o32 48+rd

		Push_r16,													// o16 50+rw
		Push_r32,													// o32 50+rd
		Push_r64,													// 50+ro

		Pop_r16,													// o16 58+rw
		Pop_r32,													// o32 58+rd
		Pop_r64,													// 58+ro

		Pushaw,														// o16 60
		Pushad,														// o32 60
		Popaw,														// o16 61
		Popad,														// o32 61
		Bound_r16_m1616,											// o16 62 /r
		Bound_r32_m3232,											// o32 62 /r
		Arpl_rm16_r16,												// o16 63 /r
		Arpl_r32m16_r32,											// o32 63 /r
		Movsxd_r16_rm16,											// o16 63 /r
		Movsxd_r32_rm32,											// o32 63 /r
		Movsxd_r64_rm32,											// REX.W 63 /r

		Push_imm16,													// o16 68 iw
		Pushd_imm32,												// o32 68 id
		Pushq_imm32,												// 68 id
		Imul_r16_rm16_imm16,										// o16 69 /r iw
		Imul_r32_rm32_imm32,										// o32 69 /r id
		Imul_r64_rm64_imm32,										// REX.W 69 /r id
		Pushw_imm8,													// o16 6A ib
		Pushd_imm8,													// o32 6A ib
		Pushq_imm8,													// 6A ib
		Imul_r16_rm16_imm8,											// o16 6B /r ib
		Imul_r32_rm32_imm8,											// o32 6B /r ib
		Imul_r64_rm64_imm8,											// REX.W 6B /r ib
		Insb_m8_DX,													// 6C
		Insw_m16_DX,												// o16 6D
		Insd_m32_DX,												// o32 6D
		Outsb_DX_m8,												// 6E
		Outsw_DX_m16,												// o16 6F
		Outsd_DX_m32,												// o32 6F

		Jo_rel8_16,													// o16 70 cb
		Jo_rel8_32,													// o32 70 cb
		Jo_rel8_64,													// 70 cb
		Jno_rel8_16,												// o16 71 cb
		Jno_rel8_32,												// o32 71 cb
		Jno_rel8_64,												// 71 cb
		Jb_rel8_16,													// o16 72 cb
		Jb_rel8_32,													// o32 72 cb
		Jb_rel8_64,													// 72 cb
		Jae_rel8_16,												// o16 73 cb
		Jae_rel8_32,												// o32 73 cb
		Jae_rel8_64,												// 73 cb
		Je_rel8_16,													// o16 74 cb
		Je_rel8_32,													// o32 74 cb
		Je_rel8_64,													// 74 cb
		Jne_rel8_16,												// o16 75 cb
		Jne_rel8_32,												// o32 75 cb
		Jne_rel8_64,												// 75 cb
		Jbe_rel8_16,												// o16 76 cb
		Jbe_rel8_32,												// o32 76 cb
		Jbe_rel8_64,												// 76 cb
		Ja_rel8_16,													// o16 77 cb
		Ja_rel8_32,													// o32 77 cb
		Ja_rel8_64,													// 77 cb

		Js_rel8_16,													// o16 78 cb
		Js_rel8_32,													// o32 78 cb
		Js_rel8_64,													// 78 cb
		Jns_rel8_16,												// o16 79 cb
		Jns_rel8_32,												// o32 79 cb
		Jns_rel8_64,												// 79 cb
		Jp_rel8_16,													// o16 7A cb
		Jp_rel8_32,													// o32 7A cb
		Jp_rel8_64,													// 7A cb
		Jnp_rel8_16,												// o16 7B cb
		Jnp_rel8_32,												// o32 7B cb
		Jnp_rel8_64,												// 7B cb
		Jl_rel8_16,													// o16 7C cb
		Jl_rel8_32,													// o32 7C cb
		Jl_rel8_64,													// 7C cb
		Jge_rel8_16,												// o16 7D cb
		Jge_rel8_32,												// o32 7D cb
		Jge_rel8_64,												// 7D cb
		Jle_rel8_16,												// o16 7E cb
		Jle_rel8_32,												// o32 7E cb
		Jle_rel8_64,												// 7E cb
		Jg_rel8_16,													// o16 7F cb
		Jg_rel8_32,													// o32 7F cb
		Jg_rel8_64,													// 7F cb

		Add_rm8_imm8,												// 80 /0 ib
		Or_rm8_imm8,												// 80 /1 ib
		Adc_rm8_imm8,												// 80 /2 ib
		Sbb_rm8_imm8,												// 80 /3 ib
		And_rm8_imm8,												// 80 /4 ib
		Sub_rm8_imm8,												// 80 /5 ib
		Xor_rm8_imm8,												// 80 /6 ib
		Cmp_rm8_imm8,												// 80 /7 ib
		Add_rm16_imm16,												// o16 81 /0 iw
		Add_rm32_imm32,												// o32 81 /0 id
		Add_rm64_imm32,												// REX.W 81 /0 id
		Or_rm16_imm16,												// o16 81 /1 iw
		Or_rm32_imm32,												// o32 81 /1 id
		Or_rm64_imm32,												// REX.W 81 /1 id
		Adc_rm16_imm16,												// o16 81 /2 iw
		Adc_rm32_imm32,												// o32 81 /2 id
		Adc_rm64_imm32,												// REX.W 81 /2 id
		Sbb_rm16_imm16,												// o16 81 /3 iw
		Sbb_rm32_imm32,												// o32 81 /3 id
		Sbb_rm64_imm32,												// REX.W 81 /3 id
		And_rm16_imm16,												// o16 81 /4 iw
		And_rm32_imm32,												// o32 81 /4 id
		And_rm64_imm32,												// REX.W 81 /4 id
		Sub_rm16_imm16,												// o16 81 /5 iw
		Sub_rm32_imm32,												// o32 81 /5 id
		Sub_rm64_imm32,												// REX.W 81 /5 id
		Xor_rm16_imm16,												// o16 81 /6 iw
		Xor_rm32_imm32,												// o32 81 /6 id
		Xor_rm64_imm32,												// REX.W 81 /6 id
		Cmp_rm16_imm16,												// o16 81 /7 iw
		Cmp_rm32_imm32,												// o32 81 /7 id
		Cmp_rm64_imm32,												// REX.W 81 /7 id
		Add_rm8_imm8_82,											// 82 /0 ib
		Or_rm8_imm8_82,												// 82 /1 ib
		Adc_rm8_imm8_82,											// 82 /2 ib
		Sbb_rm8_imm8_82,											// 82 /3 ib
		And_rm8_imm8_82,											// 82 /4 ib
		Sub_rm8_imm8_82,											// 82 /5 ib
		Xor_rm8_imm8_82,											// 82 /6 ib
		Cmp_rm8_imm8_82,											// 82 /7 ib
		Add_rm16_imm8,												// o16 83 /0 ib
		Add_rm32_imm8,												// o32 83 /0 ib
		Add_rm64_imm8,												// REX.W 83 /0 ib
		Or_rm16_imm8,												// o16 83 /1 ib
		Or_rm32_imm8,												// o32 83 /1 ib
		Or_rm64_imm8,												// REX.W 83 /1 ib
		Adc_rm16_imm8,												// o16 83 /2 ib
		Adc_rm32_imm8,												// o32 83 /2 ib
		Adc_rm64_imm8,												// REX.W 83 /2 ib
		Sbb_rm16_imm8,												// o16 83 /3 ib
		Sbb_rm32_imm8,												// o32 83 /3 ib
		Sbb_rm64_imm8,												// REX.W 83 /3 ib
		And_rm16_imm8,												// o16 83 /4 ib
		And_rm32_imm8,												// o32 83 /4 ib
		And_rm64_imm8,												// REX.W 83 /4 ib
		Sub_rm16_imm8,												// o16 83 /5 ib
		Sub_rm32_imm8,												// o32 83 /5 ib
		Sub_rm64_imm8,												// REX.W 83 /5 ib
		Xor_rm16_imm8,												// o16 83 /6 ib
		Xor_rm32_imm8,												// o32 83 /6 ib
		Xor_rm64_imm8,												// REX.W 83 /6 ib
		Cmp_rm16_imm8,												// o16 83 /7 ib
		Cmp_rm32_imm8,												// o32 83 /7 ib
		Cmp_rm64_imm8,												// REX.W 83 /7 ib
		Test_rm8_r8,												// 84 /r
		Test_rm16_r16,												// o16 85 /r
		Test_rm32_r32,												// o32 85 /r
		Test_rm64_r64,												// REX.W 85 /r
		Xchg_rm8_r8,												// 86 /r
		Xchg_rm16_r16,												// o16 87 /r
		Xchg_rm32_r32,												// o32 87 /r
		Xchg_rm64_r64,												// REX.W 87 /r

		Mov_rm8_r8,													// 88 /r
		Mov_rm16_r16,												// o16 89 /r
		Mov_rm32_r32,												// o32 89 /r
		Mov_rm64_r64,												// REX.W 89 /r
		Mov_r8_rm8,													// 8A /r
		Mov_r16_rm16,												// o16 8B /r
		Mov_r32_rm32,												// o32 8B /r
		Mov_r64_rm64,												// REX.W 8B /r
		Mov_rm16_Sreg,												// o16 8C /r
		Mov_r32m16_Sreg,											// o32 8C /r
		Mov_r64m16_Sreg,											// REX.W 8C /r
		Lea_r16_m,													// o16 8D /r
		Lea_r32_m,													// o32 8D /r
		Lea_r64_m,													// REX.W 8D /r
		Mov_Sreg_rm16,												// o16 8E /r
		Mov_Sreg_r32m16,											// o32 8E /r
		Mov_Sreg_r64m16,											// REX.W 8E /r
		Pop_rm16,													// o16 8F /0
		Pop_rm32,													// o32 8F /0
		Pop_rm64,													// 8F /0

		Nopw,														// o16 90
		Nopd,														// o32 90
		Nopq,														// REX.W 90
		Xchg_r16_AX,												// o16 90+rw
		Xchg_r32_EAX,												// o32 90+rd
		Xchg_r64_RAX,												// REX.W 90+ro

		Pause,														// F3 90

		Cbw,														// o16 98
		Cwde,														// o32 98
		Cdqe,														// REX.W 98
		Cwd,														// o16 99
		Cdq,														// o32 99
		Cqo,														// REX.W 99
		Call_ptr1616,												// o16 9A cd
		Call_ptr1632,												// o32 9A cp
		Wait,														// 9B
		Pushfw,														// o16 9C
		Pushfd,														// o32 9C
		Pushfq,														// 9C
		Popfw,														// o16 9D
		Popfd,														// o32 9D
		Popfq,														// 9D
		Sahf,														// 9E
		Lahf,														// 9F

		Mov_AL_moffs8,												// A0 mo
		Mov_AX_moffs16,												// o16 A1 mo
		Mov_EAX_moffs32,											// o32 A1 mo
		Mov_RAX_moffs64,											// REX.W A1 mo
		Mov_moffs8_AL,												// A2 mo
		Mov_moffs16_AX,												// o16 A3 mo
		Mov_moffs32_EAX,											// o32 A3 mo
		Mov_moffs64_RAX,											// REX.W A3 mo
		Movsb_m8_m8,												// A4
		Movsw_m16_m16,												// o16 A5
		Movsd_m32_m32,												// o32 A5
		Movsq_m64_m64,												// REX.W A5
		Cmpsb_m8_m8,												// A6
		Cmpsw_m16_m16,												// o16 A7
		Cmpsd_m32_m32,												// o32 A7
		Cmpsq_m64_m64,												// REX.W A7

		Test_AL_imm8,												// A8 ib
		Test_AX_imm16,												// o16 A9 iw
		Test_EAX_imm32,												// o32 A9 id
		Test_RAX_imm32,												// REX.W A9 id
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

		Mov_r8_imm8,												// B0+rb ib

		Mov_r16_imm16,												// o16 B8+rw iw
		Mov_r32_imm32,												// o32 B8+rd id
		Mov_r64_imm64,												// REX.W B8+ro io

		Rol_rm8_imm8,												// C0 /0 ib
		Ror_rm8_imm8,												// C0 /1 ib
		Rcl_rm8_imm8,												// C0 /2 ib
		Rcr_rm8_imm8,												// C0 /3 ib
		Shl_rm8_imm8,												// C0 /4 ib
		Shr_rm8_imm8,												// C0 /5 ib
		Sal_rm8_imm8,												// C0 /6 ib
		Sar_rm8_imm8,												// C0 /7 ib
		Rol_rm16_imm8,												// o16 C1 /0 ib
		Rol_rm32_imm8,												// o32 C1 /0 ib
		Rol_rm64_imm8,												// REX.W C1 /0 ib
		Ror_rm16_imm8,												// o16 C1 /1 ib
		Ror_rm32_imm8,												// o32 C1 /1 ib
		Ror_rm64_imm8,												// REX.W C1 /1 ib
		Rcl_rm16_imm8,												// o16 C1 /2 ib
		Rcl_rm32_imm8,												// o32 C1 /2 ib
		Rcl_rm64_imm8,												// REX.W C1 /2 ib
		Rcr_rm16_imm8,												// o16 C1 /3 ib
		Rcr_rm32_imm8,												// o32 C1 /3 ib
		Rcr_rm64_imm8,												// REX.W C1 /3 ib
		Shl_rm16_imm8,												// o16 C1 /4 ib
		Shl_rm32_imm8,												// o32 C1 /4 ib
		Shl_rm64_imm8,												// REX.W C1 /4 ib
		Shr_rm16_imm8,												// o16 C1 /5 ib
		Shr_rm32_imm8,												// o32 C1 /5 ib
		Shr_rm64_imm8,												// REX.W C1 /5 ib
		Sal_rm16_imm8,												// o16 C1 /6 ib
		Sal_rm32_imm8,												// o32 C1 /6 ib
		Sal_rm64_imm8,												// REX.W C1 /6 ib
		Sar_rm16_imm8,												// o16 C1 /7 ib
		Sar_rm32_imm8,												// o32 C1 /7 ib
		Sar_rm64_imm8,												// REX.W C1 /7 ib
		Retnw_imm16,												// o16 C2 iw
		Retnd_imm16,												// o32 C2 iw
		Retnq_imm16,												// C2 iw
		Retnw,														// o16 C3
		Retnd,														// o32 C3
		Retnq,														// C3
		Les_r16_m1616,												// o16 C4 /r
		Les_r32_m1632,												// o32 C4 /r
		Lds_r16_m1616,												// o16 C5 /r
		Lds_r32_m1632,												// o32 C5 /r
		Mov_rm8_imm8,												// C6 /0 ib
		Xabort_imm8,												// C6 F8 ib
		Mov_rm16_imm16,												// o16 C7 /0 iw
		Mov_rm32_imm32,												// o32 C7 /0 id
		Mov_rm64_imm32,												// REX.W C7 /0 id
		Xbegin_rel16,												// o16 C7 F8 cw
		Xbegin_rel32,												// o32 C7 F8 cd

		Enterw_imm16_imm8,											// o16 C8 iw ib
		Enterd_imm16_imm8,											// o32 C8 iw ib
		Enterq_imm16_imm8,											// C8 iw ib
		Leavew,														// o16 C9
		Leaved,														// o32 C9
		Leaveq,														// C9
		Retfw_imm16,												// o16 CA iw
		Retfd_imm16,												// o32 CA iw
		Retfq_imm16,												// REX.W CA iw
		Retfw,														// o16 CB
		Retfd,														// o32 CB
		Retfq,														// REX.W CB
		Int3,														// CC
		Int_imm8,													// CD ib
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
		Aam_imm8,													// D4 ib
		Aad_imm8,													// D5 ib
		Salc,														// D6
		Xlat_m8,													// D7

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
		Fldcw_m2byte,												// D9 /5
		Fnstenv_m14byte,											// o16 D9 /6
		Fstenv_m14byte,												// 9B o16 D9 /6
		Fnstenv_m28byte,											// o32 D9 /6
		Fstenv_m28byte,												// 9B o32 D9 /6
		Fnstcw_m2byte,												// D9 /7
		Fstcw_m2byte,												// 9B D9 /7
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
		Fisttp_m64int,												// DD /1
		Fst_m64fp,													// DD /2
		Fstp_m64fp,													// DD /3
		Frstor_m94byte,												// o16 DD /4
		Frstor_m108byte,											// o32 DD /4
		Fnsave_m94byte,												// o16 DD /6
		Fsave_m94byte,												// 9B o16 DD /6
		Fnsave_m108byte,											// o32 DD /6
		Fsave_m108byte,												// 9B o32 DD /6
		Fnstsw_m2byte,												// DD /7
		Fstsw_m2byte,												// 9B DD /7
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

		Loopne_rel8_16_CX,											// a16 o16 E0 cb
		Loopne_rel8_32_CX,											// a16 o32 E0 cb
		Loopne_rel8_16_ECX,											// a32 o16 E0 cb
		Loopne_rel8_32_ECX,											// a32 o32 E0 cb
		Loopne_rel8_64_ECX,											// a32 E0 cb
		Loopne_rel8_16_RCX,											// o16 E0 cb
		Loopne_rel8_64_RCX,											// E0 cb
		Loope_rel8_16_CX,											// a16 o16 E1 cb
		Loope_rel8_32_CX,											// a16 o32 E1 cb
		Loope_rel8_16_ECX,											// a32 o16 E1 cb
		Loope_rel8_32_ECX,											// a32 o32 E1 cb
		Loope_rel8_64_ECX,											// a32 E1 cb
		Loope_rel8_16_RCX,											// o16 E1 cb
		Loope_rel8_64_RCX,											// E1 cb
		Loop_rel8_16_CX,											// a16 o16 E2 cb
		Loop_rel8_32_CX,											// a16 o32 E2 cb
		Loop_rel8_16_ECX,											// a32 o16 E2 cb
		Loop_rel8_32_ECX,											// a32 o32 E2 cb
		Loop_rel8_64_ECX,											// a32 E2 cb
		Loop_rel8_16_RCX,											// o16 E2 cb
		Loop_rel8_64_RCX,											// E2 cb
		Jcxz_rel8_16,												// a16 o16 E3 cb
		Jcxz_rel8_32,												// a16 o32 E3 cb
		Jecxz_rel8_16,												// a32 o16 E3 cb
		Jecxz_rel8_32,												// a32 o32 E3 cb
		Jecxz_rel8_64,												// a32 E3 cb
		Jrcxz_rel8_16,												// o16 E3 cb
		Jrcxz_rel8_64,												// E3 cb
		In_AL_imm8,													// E4 ib
		In_AX_imm8,													// o16 E5 ib
		In_EAX_imm8,												// o32 E5 ib
		Out_imm8_AL,												// E6 ib
		Out_imm8_AX,												// o16 E7 ib
		Out_imm8_EAX,												// o32 E7 ib

		Call_rel16,													// o16 E8 cw
		Call_rel32_32,												// o32 E8 cd
		Call_rel32_64,												// E8 cd
		Jmp_rel16,													// o16 E9 cw
		Jmp_rel32_32,												// o32 E9 cd
		Jmp_rel32_64,												// E9 cd
		Jmp_ptr1616,												// o16 EA cd
		Jmp_ptr1632,												// o32 EA cp
		Jmp_rel8_16,												// o16 EB cb
		Jmp_rel8_32,												// o32 EB cb
		Jmp_rel8_64,												// EB cb
		In_AL_DX,													// EC
		In_AX_DX,													// o16 ED
		In_EAX_DX,													// o32 ED
		Out_DX_AL,													// EE
		Out_DX_AX,													// o16 EF
		Out_DX_EAX,													// o32 EF

		Int1,														// F1
		Hlt,														// F4
		Cmc,														// F5
		Test_rm8_imm8,												// F6 /0 ib
		Test_rm8_imm8_F6r1,											// F6 /1 ib
		Not_rm8,													// F6 /2
		Neg_rm8,													// F6 /3
		Mul_rm8,													// F6 /4
		Imul_rm8,													// F6 /5
		Div_rm8,													// F6 /6
		Idiv_rm8,													// F6 /7
		Test_rm16_imm16,											// o16 F7 /0 iw
		Test_rm32_imm32,											// o32 F7 /0 id
		Test_rm64_imm32,											// REX.W F7 /0 id
		Test_rm16_imm16_F7r1,										// o16 F7 /1 iw
		Test_rm32_imm32_F7r1,										// o32 F7 /1 id
		Test_rm64_imm32_F7r1,										// REX.W F7 /1 id
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
		Call_m1632,													// o32 FF /3
		Call_m1664,													// REX.W FF /3
		Jmp_rm16,													// o16 FF /4
		Jmp_rm32,													// o32 FF /4
		Jmp_rm64,													// FF /4
		Jmp_m1616,													// o16 FF /5
		Jmp_m1632,													// o32 FF /5
		Jmp_m1664,													// REX.W FF /5
		Push_rm16,													// o16 FF /6
		Push_rm32,													// o32 FF /6
		Push_rm64,													// FF /6

		// 0F xx opcodes

		Sldt_rm16,													// o16 0F 00 /0
		Sldt_r32m16,												// o32 0F 00 /0
		Sldt_r64m16,												// REX.W 0F 00 /0
		Str_rm16,													// o16 0F 00 /1
		Str_r32m16,													// o32 0F 00 /1
		Str_r64m16,													// REX.W 0F 00 /1
		Lldt_rm16,													// o16 0F 00 /2
		Lldt_r32m16,												// o32 0F 00 /2
		Lldt_r64m16,												// REX.W 0F 00 /2
		Ltr_rm16,													// o16 0F 00 /3
		Ltr_r32m16,													// o32 0F 00 /3
		Ltr_r64m16,													// REX.W 0F 00 /3
		Verr_rm16,													// o16 0F 00 /4
		Verr_r32m16,												// o32 0F 00 /4
		Verr_r64m16,												// REX.W 0F 00 /4
		Verw_rm16,													// o16 0F 00 /5
		Verw_r32m16,												// o32 0F 00 /5
		Verw_r64m16,												// REX.W 0F 00 /5
		Jmpe_rm16,													// o16 0F 00 /6
		Jmpe_rm32,													// o32 0F 00 /6
		Sgdt_m1632_16,												// o16 0F 01 /0
		Sgdt_m1632,													// o32 0F 01 /0
		Sgdt_m1664,													// 0F 01 /0
		Sidt_m1632_16,												// o16 0F 01 /1
		Sidt_m1632,													// o32 0F 01 /1
		Sidt_m1664,													// 0F 01 /1
		Lgdt_m1632_16,												// o16 0F 01 /2
		Lgdt_m1632,													// o32 0F 01 /2
		Lgdt_m1664,													// 0F 01 /2
		Lidt_m1632_16,												// o16 0F 01 /3
		Lidt_m1632,													// o32 0F 01 /3
		Lidt_m1664,													// 0F 01 /3
		Smsw_rm16,													// o16 0F 01 /4
		Smsw_r32m16,												// o32 0F 01 /4
		Smsw_r64m16,												// REX.W 0F 01 /4
		Rstorssp_m64,												// F3 0F 01 /5
		Lmsw_rm16,													// o16 0F 01 /6
		Lmsw_r32m16,												// o32 0F 01 /6
		Lmsw_r64m16,												// REX.W 0F 01 /6
		Invlpg_m,													// 0F 01 /7
		Enclv,														// NP 0F 01 C0
		Vmcall,														// NP 0F 01 C1
		Vmlaunch,													// NP 0F 01 C2
		Vmresume,													// NP 0F 01 C3
		Vmxoff,														// NP 0F 01 C4
		Pconfig,													// NP 0F 01 C5
		Monitorw,													// a16 NP 0F 01 C8
		Monitord,													// a32 NP 0F 01 C8
		Monitorq,													// NP 0F 01 C8
		Mwait,														// NP 0F 01 C9
		Clac,														// NP 0F 01 CA
		Stac,														// NP 0F 01 CB
		Encls,														// NP 0F 01 CF
		Xgetbv,														// NP 0F 01 D0
		Xsetbv,														// NP 0F 01 D1
		Vmfunc,														// NP 0F 01 D4
		Xend,														// NP 0F 01 D5
		Xtest,														// NP 0F 01 D6
		Enclu,														// NP 0F 01 D7
		Vmrunw,														// a16 0F 01 D8
		Vmrund,														// a32 0F 01 D8
		Vmrunq,														// 0F 01 D8
		Vmmcall,													// 0F 01 D9
		Vmloadw,													// a16 0F 01 DA
		Vmloadd,													// a32 0F 01 DA
		Vmloadq,													// 0F 01 DA
		Vmsavew,													// a16 0F 01 DB
		Vmsaved,													// a32 0F 01 DB
		Vmsaveq,													// 0F 01 DB
		Stgi,														// 0F 01 DC
		Clgi,														// 0F 01 DD
		Skinit,														// 0F 01 DE
		Invlpgaw,													// a16 0F 01 DF
		Invlpgad,													// a32 0F 01 DF
		Invlpgaq,													// 0F 01 DF
		Setssbsy,													// F3 0F 01 E8
		Saveprevssp,												// F3 0F 01 EA
		Rdpkru,														// NP 0F 01 EE
		Wrpkru,														// NP 0F 01 EF
		Swapgs,														// 0F 01 F8
		Rdtscp,														// 0F 01 F9
		Monitorxw,													// a16 NP 0F 01 FA
		Monitorxd,													// a32 NP 0F 01 FA
		Monitorxq,													// NP 0F 01 FA
		Mcommit,													// F3 0F 01 FA
		Mwaitx,														// 0F 01 FB
		Clzerow,													// a16 0F 01 FC
		Clzerod,													// a32 0F 01 FC
		Clzeroq,													// 0F 01 FC
		Rdpru,														// 0F 01 FD
		Lar_r16_rm16,												// o16 0F 02 /r
		Lar_r32_r32m16,												// o32 0F 02 /r
		Lar_r64_r64m16,												// REX.W 0F 02 /r
		Lsl_r16_rm16,												// o16 0F 03 /r
		Lsl_r32_r32m16,												// o32 0F 03 /r
		Lsl_r64_r64m16,												// REX.W 0F 03 /r
		Loadallreset286,											// 0F 04
		Loadall286,													// 0F 05
		Syscall,													// 0F 05
		Clts,														// 0F 06
		Loadall386,													// 0F 07
		Sysretd,													// 0F 07
		Sysretq,													// REX.W 0F 07

		Invd,														// 0F 08
		Wbinvd,														// 0F 09
		Wbnoinvd,													// F3 0F 09
		Cl1invmb,													// 0F 0A
		Ud2,														// 0F 0B
		ReservedNop_rm16_r16_0F0D,									// o16 0F 0D /r
		ReservedNop_rm32_r32_0F0D,									// o32 0F 0D /r
		ReservedNop_rm64_r64_0F0D,									// REX.W 0F 0D /r
		Prefetch_m8,												// 0F 0D /0
		Prefetchw_m8,												// 0F 0D /1
		Prefetchwt1_m8,												// 0F 0D /2
		Femms,														// 0F 0E

		Umov_rm8_r8,												// 0F 10 /r
		Umov_rm16_r16,												// o16 0F 11 /r
		Umov_rm32_r32,												// o32 0F 11 /r
		Umov_r8_rm8,												// 0F 12 /r
		Umov_r16_rm16,												// o16 0F 13 /r
		Umov_r32_rm32,												// o32 0F 13 /r

		Movups_xmm_xmmm128,											// NP 0F 10 /r
		VEX_Vmovups_xmm_xmmm128,									// VEX.128.0F.WIG 10 /r
		VEX_Vmovups_ymm_ymmm256,									// VEX.256.0F.WIG 10 /r
		EVEX_Vmovups_xmm_k1z_xmmm128,								// EVEX.128.0F.W0 10 /r
		EVEX_Vmovups_ymm_k1z_ymmm256,								// EVEX.256.0F.W0 10 /r
		EVEX_Vmovups_zmm_k1z_zmmm512,								// EVEX.512.0F.W0 10 /r

		Movupd_xmm_xmmm128,											// 66 0F 10 /r
		VEX_Vmovupd_xmm_xmmm128,									// VEX.128.66.0F.WIG 10 /r
		VEX_Vmovupd_ymm_ymmm256,									// VEX.256.66.0F.WIG 10 /r
		EVEX_Vmovupd_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 10 /r
		EVEX_Vmovupd_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 10 /r
		EVEX_Vmovupd_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 10 /r

		Movss_xmm_xmmm32,											// F3 0F 10 /r
		VEX_Vmovss_xmm_xmm_xmm,										// VEX.LIG.F3.0F.WIG 10 /r
		VEX_Vmovss_xmm_m32,											// VEX.LIG.F3.0F.WIG 10 /r
		EVEX_Vmovss_xmm_k1z_xmm_xmm,								// EVEX.LIG.F3.0F.W0 10 /r
		EVEX_Vmovss_xmm_k1z_m32,									// EVEX.LIG.F3.0F.W0 10 /r

		Movsd_xmm_xmmm64,											// F2 0F 10 /r
		VEX_Vmovsd_xmm_xmm_xmm,										// VEX.LIG.F2.0F.WIG 10 /r
		VEX_Vmovsd_xmm_m64,											// VEX.LIG.F2.0F.WIG 10 /r
		EVEX_Vmovsd_xmm_k1z_xmm_xmm,								// EVEX.LIG.F2.0F.W1 10 /r
		EVEX_Vmovsd_xmm_k1z_m64,									// EVEX.LIG.F2.0F.W1 10 /r

		Movups_xmmm128_xmm,											// NP 0F 11 /r
		VEX_Vmovups_xmmm128_xmm,									// VEX.128.0F.WIG 11 /r
		VEX_Vmovups_ymmm256_ymm,									// VEX.256.0F.WIG 11 /r
		EVEX_Vmovups_xmmm128_k1z_xmm,								// EVEX.128.0F.W0 11 /r
		EVEX_Vmovups_ymmm256_k1z_ymm,								// EVEX.256.0F.W0 11 /r
		EVEX_Vmovups_zmmm512_k1z_zmm,								// EVEX.512.0F.W0 11 /r

		Movupd_xmmm128_xmm,											// 66 0F 11 /r
		VEX_Vmovupd_xmmm128_xmm,									// VEX.128.66.0F.WIG 11 /r
		VEX_Vmovupd_ymmm256_ymm,									// VEX.256.66.0F.WIG 11 /r
		EVEX_Vmovupd_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 11 /r
		EVEX_Vmovupd_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 11 /r
		EVEX_Vmovupd_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 11 /r

		Movss_xmmm32_xmm,											// F3 0F 11 /r
		VEX_Vmovss_xmm_xmm_xmm_0F11,								// VEX.LIG.F3.0F.WIG 11 /r
		VEX_Vmovss_m32_xmm,											// VEX.LIG.F3.0F.WIG 11 /r
		EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11,							// EVEX.LIG.F3.0F.W0 11 /r
		EVEX_Vmovss_m32_k1_xmm,										// EVEX.LIG.F3.0F.W0 11 /r

		Movsd_xmmm64_xmm,											// F2 0F 11 /r
		VEX_Vmovsd_xmm_xmm_xmm_0F11,								// VEX.LIG.F2.0F.WIG 11 /r
		VEX_Vmovsd_m64_xmm,											// VEX.LIG.F2.0F.WIG 11 /r
		EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11,							// EVEX.LIG.F2.0F.W1 11 /r
		EVEX_Vmovsd_m64_k1_xmm,										// EVEX.LIG.F2.0F.W1 11 /r

		Movhlps_xmm_xmm,											// NP 0F 12 /r
		Movlps_xmm_m64,												// NP 0F 12 /r
		VEX_Vmovhlps_xmm_xmm_xmm,									// VEX.128.0F.WIG 12 /r
		VEX_Vmovlps_xmm_xmm_m64,									// VEX.128.0F.WIG 12 /r
		EVEX_Vmovhlps_xmm_xmm_xmm,									// EVEX.128.0F.W0 12 /r
		EVEX_Vmovlps_xmm_xmm_m64,									// EVEX.128.0F.W0 12 /r

		Movlpd_xmm_m64,												// 66 0F 12 /r
		VEX_Vmovlpd_xmm_xmm_m64,									// VEX.128.66.0F.WIG 12 /r
		EVEX_Vmovlpd_xmm_xmm_m64,									// EVEX.128.66.0F.W1 12 /r

		Movsldup_xmm_xmmm128,										// F3 0F 12 /r
		VEX_Vmovsldup_xmm_xmmm128,									// VEX.128.F3.0F.WIG 12 /r
		VEX_Vmovsldup_ymm_ymmm256,									// VEX.256.F3.0F.WIG 12 /r
		EVEX_Vmovsldup_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 12 /r
		EVEX_Vmovsldup_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 12 /r
		EVEX_Vmovsldup_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 12 /r

		Movddup_xmm_xmmm64,											// F2 0F 12 /r
		VEX_Vmovddup_xmm_xmmm64,									// VEX.128.F2.0F.WIG 12 /r
		VEX_Vmovddup_ymm_ymmm256,									// VEX.256.F2.0F.WIG 12 /r
		EVEX_Vmovddup_xmm_k1z_xmmm64,								// EVEX.128.F2.0F.W1 12 /r
		EVEX_Vmovddup_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W1 12 /r
		EVEX_Vmovddup_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W1 12 /r

		Movlps_m64_xmm,												// NP 0F 13 /r
		VEX_Vmovlps_m64_xmm,										// VEX.128.0F.WIG 13 /r
		EVEX_Vmovlps_m64_xmm,										// EVEX.128.0F.W0 13 /r

		Movlpd_m64_xmm,												// 66 0F 13 /r
		VEX_Vmovlpd_m64_xmm,										// VEX.128.66.0F.WIG 13 /r
		EVEX_Vmovlpd_m64_xmm,										// EVEX.128.66.0F.W1 13 /r

		Unpcklps_xmm_xmmm128,										// NP 0F 14 /r
		VEX_Vunpcklps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 14 /r
		VEX_Vunpcklps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 14 /r
		EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 14 /r
		EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 14 /r
		EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 14 /r

		Unpcklpd_xmm_xmmm128,										// 66 0F 14 /r
		VEX_Vunpcklpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 14 /r
		VEX_Vunpcklpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 14 /r
		EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 14 /r
		EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 14 /r
		EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 14 /r

		Unpckhps_xmm_xmmm128,										// NP 0F 15 /r
		VEX_Vunpckhps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 15 /r
		VEX_Vunpckhps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 15 /r
		EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 15 /r
		EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 15 /r
		EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 15 /r

		Unpckhpd_xmm_xmmm128,										// 66 0F 15 /r
		VEX_Vunpckhpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 15 /r
		VEX_Vunpckhpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 15 /r
		EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 15 /r
		EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 15 /r
		EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 15 /r

		Movlhps_xmm_xmm,											// NP 0F 16 /r
		VEX_Vmovlhps_xmm_xmm_xmm,									// VEX.128.0F.WIG 16 /r
		EVEX_Vmovlhps_xmm_xmm_xmm,									// EVEX.128.0F.W0 16 /r

		Movhps_xmm_m64,												// NP 0F 16 /r
		VEX_Vmovhps_xmm_xmm_m64,									// VEX.128.0F.WIG 16 /r
		EVEX_Vmovhps_xmm_xmm_m64,									// EVEX.128.0F.W0 16 /r

		Movhpd_xmm_m64,												// 66 0F 16 /r
		VEX_Vmovhpd_xmm_xmm_m64,									// VEX.128.66.0F.WIG 16 /r
		EVEX_Vmovhpd_xmm_xmm_m64,									// EVEX.128.66.0F.W1 16 /r

		Movshdup_xmm_xmmm128,										// F3 0F 16 /r
		VEX_Vmovshdup_xmm_xmmm128,									// VEX.128.F3.0F.WIG 16 /r
		VEX_Vmovshdup_ymm_ymmm256,									// VEX.256.F3.0F.WIG 16 /r
		EVEX_Vmovshdup_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 16 /r
		EVEX_Vmovshdup_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 16 /r
		EVEX_Vmovshdup_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 16 /r

		Movhps_m64_xmm,												// NP 0F 17 /r
		VEX_Vmovhps_m64_xmm,										// VEX.128.0F.WIG 17 /r
		EVEX_Vmovhps_m64_xmm,										// EVEX.128.0F.W0 17 /r

		Movhpd_m64_xmm,												// 66 0F 17 /r
		VEX_Vmovhpd_m64_xmm,										// VEX.128.66.0F.WIG 17 /r
		EVEX_Vmovhpd_m64_xmm,										// EVEX.128.66.0F.W1 17 /r

		ReservedNop_rm16_r16_0F18,									// o16 0F 18 /r
		ReservedNop_rm32_r32_0F18,									// o32 0F 18 /r
		ReservedNop_rm64_r64_0F18,									// REX.W 0F 18 /r
		ReservedNop_rm16_r16_0F19,									// o16 0F 19 /r
		ReservedNop_rm32_r32_0F19,									// o32 0F 19 /r
		ReservedNop_rm64_r64_0F19,									// REX.W 0F 19 /r
		ReservedNop_rm16_r16_0F1A,									// o16 0F 1A /r
		ReservedNop_rm32_r32_0F1A,									// o32 0F 1A /r
		ReservedNop_rm64_r64_0F1A,									// REX.W 0F 1A /r
		ReservedNop_rm16_r16_0F1B,									// o16 0F 1B /r
		ReservedNop_rm32_r32_0F1B,									// o32 0F 1B /r
		ReservedNop_rm64_r64_0F1B,									// REX.W 0F 1B /r
		ReservedNop_rm16_r16_0F1C,									// o16 0F 1C /r
		ReservedNop_rm32_r32_0F1C,									// o32 0F 1C /r
		ReservedNop_rm64_r64_0F1C,									// REX.W 0F 1C /r
		ReservedNop_rm16_r16_0F1D,									// o16 0F 1D /r
		ReservedNop_rm32_r32_0F1D,									// o32 0F 1D /r
		ReservedNop_rm64_r64_0F1D,									// REX.W 0F 1D /r
		ReservedNop_rm16_r16_0F1E,									// o16 0F 1E /r
		ReservedNop_rm32_r32_0F1E,									// o32 0F 1E /r
		ReservedNop_rm64_r64_0F1E,									// REX.W 0F 1E /r
		ReservedNop_rm16_r16_0F1F,									// o16 0F 1F /r
		ReservedNop_rm32_r32_0F1F,									// o32 0F 1F /r
		ReservedNop_rm64_r64_0F1F,									// REX.W 0F 1F /r

		Prefetchnta_m8,												// 0F 18 /0
		Prefetcht0_m8,												// 0F 18 /1
		Prefetcht1_m8,												// 0F 18 /2
		Prefetcht2_m8,												// 0F 18 /3

		Bndldx_bnd_mib,												// NP 0F 1A /r

		Bndmov_bnd_bndm64,											// 66 0F 1A /r
		Bndmov_bnd_bndm128,											// 66 0F 1A /r

		Bndcl_bnd_rm32,												// F3 0F 1A /r
		Bndcl_bnd_rm64,												// F3 0F 1A /r

		Bndcu_bnd_rm32,												// F2 0F 1A /r
		Bndcu_bnd_rm64,												// F2 0F 1A /r

		Bndstx_mib_bnd,												// NP 0F 1B /r

		Bndmov_bndm64_bnd,											// 66 0F 1B /r
		Bndmov_bndm128_bnd,											// 66 0F 1B /r

		Bndmk_bnd_m32,												// F3 0F 1B /r
		Bndmk_bnd_m64,												// F3 0F 1B /r

		Bndcn_bnd_rm32,												// F2 0F 1B /r
		Bndcn_bnd_rm64,												// F2 0F 1B /r

		Cldemote_m8,												// NP 0F 1C /0

		Rdsspd_r32,													// F3 0F 1E /1
		Rdsspq_r64,													// F3 REX.W 0F 1E /1
		Endbr64,													// F3 0F 1E FA
		Endbr32,													// F3 0F 1E FB

		Nop_rm16,													// o16 0F 1F /0
		Nop_rm32,													// o32 0F 1F /0
		Nop_rm64,													// REX.W 0F 1F /0

		Mov_r32_cr,													// 0F 20 /r
		Mov_r64_cr,													// 0F 20 /r
		Mov_r32_dr,													// 0F 21 /r
		Mov_r64_dr,													// 0F 21 /r
		Mov_cr_r32,													// 0F 22 /r
		Mov_cr_r64,													// 0F 22 /r
		Mov_dr_r32,													// 0F 23 /r
		Mov_dr_r64,													// 0F 23 /r
		Mov_r32_tr,													// 0F 24 /r
		Mov_tr_r32,													// 0F 26 /r

		Movaps_xmm_xmmm128,											// NP 0F 28 /r
		VEX_Vmovaps_xmm_xmmm128,									// VEX.128.0F.WIG 28 /r
		VEX_Vmovaps_ymm_ymmm256,									// VEX.256.0F.WIG 28 /r
		EVEX_Vmovaps_xmm_k1z_xmmm128,								// EVEX.128.0F.W0 28 /r
		EVEX_Vmovaps_ymm_k1z_ymmm256,								// EVEX.256.0F.W0 28 /r
		EVEX_Vmovaps_zmm_k1z_zmmm512,								// EVEX.512.0F.W0 28 /r

		Movapd_xmm_xmmm128,											// 66 0F 28 /r
		VEX_Vmovapd_xmm_xmmm128,									// VEX.128.66.0F.WIG 28 /r
		VEX_Vmovapd_ymm_ymmm256,									// VEX.256.66.0F.WIG 28 /r
		EVEX_Vmovapd_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 28 /r
		EVEX_Vmovapd_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 28 /r
		EVEX_Vmovapd_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 28 /r

		Movaps_xmmm128_xmm,											// NP 0F 29 /r
		VEX_Vmovaps_xmmm128_xmm,									// VEX.128.0F.WIG 29 /r
		VEX_Vmovaps_ymmm256_ymm,									// VEX.256.0F.WIG 29 /r
		EVEX_Vmovaps_xmmm128_k1z_xmm,								// EVEX.128.0F.W0 29 /r
		EVEX_Vmovaps_ymmm256_k1z_ymm,								// EVEX.256.0F.W0 29 /r
		EVEX_Vmovaps_zmmm512_k1z_zmm,								// EVEX.512.0F.W0 29 /r

		Movapd_xmmm128_xmm,											// 66 0F 29 /r
		VEX_Vmovapd_xmmm128_xmm,									// VEX.128.66.0F.WIG 29 /r
		VEX_Vmovapd_ymmm256_ymm,									// VEX.256.66.0F.WIG 29 /r
		EVEX_Vmovapd_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 29 /r
		EVEX_Vmovapd_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 29 /r
		EVEX_Vmovapd_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 29 /r

		Cvtpi2ps_xmm_mmm64,											// NP 0F 2A /r

		Cvtpi2pd_xmm_mmm64,											// 66 0F 2A /r

		Cvtsi2ss_xmm_rm32,											// F3 0F 2A /r
		Cvtsi2ss_xmm_rm64,											// F3 REX.W 0F 2A /r
		VEX_Vcvtsi2ss_xmm_xmm_rm32,									// VEX.LIG.F3.0F.W0 2A /r
		VEX_Vcvtsi2ss_xmm_xmm_rm64,									// VEX.LIG.F3.0F.W1 2A /r
		EVEX_Vcvtsi2ss_xmm_xmm_rm32_er,								// EVEX.LIG.F3.0F.W0 2A /r
		EVEX_Vcvtsi2ss_xmm_xmm_rm64_er,								// EVEX.LIG.F3.0F.W1 2A /r

		Cvtsi2sd_xmm_rm32,											// F2 0F 2A /r
		Cvtsi2sd_xmm_rm64,											// F2 REX.W 0F 2A /r
		VEX_Vcvtsi2sd_xmm_xmm_rm32,									// VEX.LIG.F2.0F.W0 2A /r
		VEX_Vcvtsi2sd_xmm_xmm_rm64,									// VEX.LIG.F2.0F.W1 2A /r
		EVEX_Vcvtsi2sd_xmm_xmm_rm32_er,								// EVEX.LIG.F2.0F.W0 2A /r
		EVEX_Vcvtsi2sd_xmm_xmm_rm64_er,								// EVEX.LIG.F2.0F.W1 2A /r

		Movntps_m128_xmm,											// NP 0F 2B /r
		VEX_Vmovntps_m128_xmm,										// VEX.128.0F.WIG 2B /r
		VEX_Vmovntps_m256_ymm,										// VEX.256.0F.WIG 2B /r
		EVEX_Vmovntps_m128_xmm,										// EVEX.128.0F.W0 2B /r
		EVEX_Vmovntps_m256_ymm,										// EVEX.256.0F.W0 2B /r
		EVEX_Vmovntps_m512_zmm,										// EVEX.512.0F.W0 2B /r

		Movntpd_m128_xmm,											// 66 0F 2B /r
		VEX_Vmovntpd_m128_xmm,										// VEX.128.66.0F.WIG 2B /r
		VEX_Vmovntpd_m256_ymm,										// VEX.256.66.0F.WIG 2B /r
		EVEX_Vmovntpd_m128_xmm,										// EVEX.128.66.0F.W1 2B /r
		EVEX_Vmovntpd_m256_ymm,										// EVEX.256.66.0F.W1 2B /r
		EVEX_Vmovntpd_m512_zmm,										// EVEX.512.66.0F.W1 2B /r

		Movntss_m32_xmm,											// F3 0F 2B /r

		Movntsd_m64_xmm,											// F2 0F 2B /r

		Cvttps2pi_mm_xmmm64,										// NP 0F 2C /r

		Cvttpd2pi_mm_xmmm128,										// 66 0F 2C /r

		Cvttss2si_r32_xmmm32,										// F3 0F 2C /r
		Cvttss2si_r64_xmmm32,										// F3 REX.W 0F 2C /r
		VEX_Vcvttss2si_r32_xmmm32,									// VEX.LIG.F3.0F.W0 2C /r
		VEX_Vcvttss2si_r64_xmmm32,									// VEX.LIG.F3.0F.W1 2C /r
		EVEX_Vcvttss2si_r32_xmmm32_sae,								// EVEX.LIG.F3.0F.W0 2C /r
		EVEX_Vcvttss2si_r64_xmmm32_sae,								// EVEX.LIG.F3.0F.W1 2C /r

		Cvttsd2si_r32_xmmm64,										// F2 0F 2C /r
		Cvttsd2si_r64_xmmm64,										// F2 REX.W 0F 2C /r
		VEX_Vcvttsd2si_r32_xmmm64,									// VEX.LIG.F2.0F.W0 2C /r
		VEX_Vcvttsd2si_r64_xmmm64,									// VEX.LIG.F2.0F.W1 2C /r
		EVEX_Vcvttsd2si_r32_xmmm64_sae,								// EVEX.LIG.F2.0F.W0 2C /r
		EVEX_Vcvttsd2si_r64_xmmm64_sae,								// EVEX.LIG.F2.0F.W1 2C /r

		Cvtps2pi_mm_xmmm64,											// NP 0F 2D /r

		Cvtpd2pi_mm_xmmm128,										// 66 0F 2D /r

		Cvtss2si_r32_xmmm32,										// F3 0F 2D /r
		Cvtss2si_r64_xmmm32,										// F3 REX.W 0F 2D /r
		VEX_Vcvtss2si_r32_xmmm32,									// VEX.LIG.F3.0F.W0 2D /r
		VEX_Vcvtss2si_r64_xmmm32,									// VEX.LIG.F3.0F.W1 2D /r
		EVEX_Vcvtss2si_r32_xmmm32_er,								// EVEX.LIG.F3.0F.W0 2D /r
		EVEX_Vcvtss2si_r64_xmmm32_er,								// EVEX.LIG.F3.0F.W1 2D /r

		Cvtsd2si_r32_xmmm64,										// F2 0F 2D /r
		Cvtsd2si_r64_xmmm64,										// F2 REX.W 0F 2D /r
		VEX_Vcvtsd2si_r32_xmmm64,									// VEX.LIG.F2.0F.W0 2D /r
		VEX_Vcvtsd2si_r64_xmmm64,									// VEX.LIG.F2.0F.W1 2D /r
		EVEX_Vcvtsd2si_r32_xmmm64_er,								// EVEX.LIG.F2.0F.W0 2D /r
		EVEX_Vcvtsd2si_r64_xmmm64_er,								// EVEX.LIG.F2.0F.W1 2D /r

		Ucomiss_xmm_xmmm32,											// NP 0F 2E /r
		VEX_Vucomiss_xmm_xmmm32,									// VEX.LIG.0F.WIG 2E /r
		EVEX_Vucomiss_xmm_xmmm32_sae,								// EVEX.LIG.0F.W0 2E /r

		Ucomisd_xmm_xmmm64,											// 66 0F 2E /r
		VEX_Vucomisd_xmm_xmmm64,									// VEX.LIG.66.0F.WIG 2E /r
		EVEX_Vucomisd_xmm_xmmm64_sae,								// EVEX.LIG.66.0F.W1 2E /r

		Comiss_xmm_xmmm32,											// NP 0F 2F /r

		Comisd_xmm_xmmm64,											// 66 0F 2F /r
		VEX_Vcomiss_xmm_xmmm32,										// VEX.LIG.0F.WIG 2F /r
		VEX_Vcomisd_xmm_xmmm64,										// VEX.LIG.66.0F.WIG 2F /r
		EVEX_Vcomiss_xmm_xmmm32_sae,								// EVEX.LIG.0F.W0 2F /r
		EVEX_Vcomisd_xmm_xmmm64_sae,								// EVEX.LIG.66.0F.W1 2F /r

		Wrmsr,														// 0F 30
		Rdtsc,														// 0F 31
		Rdmsr,														// 0F 32
		Rdpmc,														// 0F 33
		Sysenter,													// 0F 34
		Sysexitd,													// 0F 35
		Sysexitq,													// REX.W 0F 35
		Getsec,														// NP 0F 37

		Cmovo_r16_rm16,												// o16 0F 40 /r
		Cmovo_r32_rm32,												// o32 0F 40 /r
		Cmovo_r64_rm64,												// REX.W 0F 40 /r
		Cmovno_r16_rm16,											// o16 0F 41 /r
		Cmovno_r32_rm32,											// o32 0F 41 /r
		Cmovno_r64_rm64,											// REX.W 0F 41 /r
		Cmovb_r16_rm16,												// o16 0F 42 /r
		Cmovb_r32_rm32,												// o32 0F 42 /r
		Cmovb_r64_rm64,												// REX.W 0F 42 /r
		Cmovae_r16_rm16,											// o16 0F 43 /r
		Cmovae_r32_rm32,											// o32 0F 43 /r
		Cmovae_r64_rm64,											// REX.W 0F 43 /r
		Cmove_r16_rm16,												// o16 0F 44 /r
		Cmove_r32_rm32,												// o32 0F 44 /r
		Cmove_r64_rm64,												// REX.W 0F 44 /r
		Cmovne_r16_rm16,											// o16 0F 45 /r
		Cmovne_r32_rm32,											// o32 0F 45 /r
		Cmovne_r64_rm64,											// REX.W 0F 45 /r
		Cmovbe_r16_rm16,											// o16 0F 46 /r
		Cmovbe_r32_rm32,											// o32 0F 46 /r
		Cmovbe_r64_rm64,											// REX.W 0F 46 /r
		Cmova_r16_rm16,												// o16 0F 47 /r
		Cmova_r32_rm32,												// o32 0F 47 /r
		Cmova_r64_rm64,												// REX.W 0F 47 /r

		Cmovs_r16_rm16,												// o16 0F 48 /r
		Cmovs_r32_rm32,												// o32 0F 48 /r
		Cmovs_r64_rm64,												// REX.W 0F 48 /r
		Cmovns_r16_rm16,											// o16 0F 49 /r
		Cmovns_r32_rm32,											// o32 0F 49 /r
		Cmovns_r64_rm64,											// REX.W 0F 49 /r
		Cmovp_r16_rm16,												// o16 0F 4A /r
		Cmovp_r32_rm32,												// o32 0F 4A /r
		Cmovp_r64_rm64,												// REX.W 0F 4A /r
		Cmovnp_r16_rm16,											// o16 0F 4B /r
		Cmovnp_r32_rm32,											// o32 0F 4B /r
		Cmovnp_r64_rm64,											// REX.W 0F 4B /r
		Cmovl_r16_rm16,												// o16 0F 4C /r
		Cmovl_r32_rm32,												// o32 0F 4C /r
		Cmovl_r64_rm64,												// REX.W 0F 4C /r
		Cmovge_r16_rm16,											// o16 0F 4D /r
		Cmovge_r32_rm32,											// o32 0F 4D /r
		Cmovge_r64_rm64,											// REX.W 0F 4D /r
		Cmovle_r16_rm16,											// o16 0F 4E /r
		Cmovle_r32_rm32,											// o32 0F 4E /r
		Cmovle_r64_rm64,											// REX.W 0F 4E /r
		Cmovg_r16_rm16,												// o16 0F 4F /r
		Cmovg_r32_rm32,												// o32 0F 4F /r
		Cmovg_r64_rm64,												// REX.W 0F 4F /r

		VEX_Kandw_k_k_k,											// VEX.L1.0F.W0 41 /r
		VEX_Kandq_k_k_k,											// VEX.L1.0F.W1 41 /r

		VEX_Kandb_k_k_k,											// VEX.L1.66.0F.W0 41 /r
		VEX_Kandd_k_k_k,											// VEX.L1.66.0F.W1 41 /r

		VEX_Kandnw_k_k_k,											// VEX.L1.0F.W0 42 /r
		VEX_Kandnq_k_k_k,											// VEX.L1.0F.W1 42 /r

		VEX_Kandnb_k_k_k,											// VEX.L1.66.0F.W0 42 /r
		VEX_Kandnd_k_k_k,											// VEX.L1.66.0F.W1 42 /r

		VEX_Knotw_k_k,												// VEX.L0.0F.W0 44 /r
		VEX_Knotq_k_k,												// VEX.L0.0F.W1 44 /r

		VEX_Knotb_k_k,												// VEX.L0.66.0F.W0 44 /r
		VEX_Knotd_k_k,												// VEX.L0.66.0F.W1 44 /r

		VEX_Korw_k_k_k,												// VEX.L1.0F.W0 45 /r
		VEX_Korq_k_k_k,												// VEX.L1.0F.W1 45 /r

		VEX_Korb_k_k_k,												// VEX.L1.66.0F.W0 45 /r
		VEX_Kord_k_k_k,												// VEX.L1.66.0F.W1 45 /r

		VEX_Kxnorw_k_k_k,											// VEX.L1.0F.W0 46 /r
		VEX_Kxnorq_k_k_k,											// VEX.L1.0F.W1 46 /r

		VEX_Kxnorb_k_k_k,											// VEX.L1.66.0F.W0 46 /r
		VEX_Kxnord_k_k_k,											// VEX.L1.66.0F.W1 46 /r

		VEX_Kxorw_k_k_k,											// VEX.L1.0F.W0 47 /r
		VEX_Kxorq_k_k_k,											// VEX.L1.0F.W1 47 /r

		VEX_Kxorb_k_k_k,											// VEX.L1.66.0F.W0 47 /r
		VEX_Kxord_k_k_k,											// VEX.L1.66.0F.W1 47 /r

		VEX_Kaddw_k_k_k,											// VEX.L1.0F.W0 4A /r
		VEX_Kaddq_k_k_k,											// VEX.L1.0F.W1 4A /r

		VEX_Kaddb_k_k_k,											// VEX.L1.66.0F.W0 4A /r
		VEX_Kaddd_k_k_k,											// VEX.L1.66.0F.W1 4A /r

		VEX_Kunpckwd_k_k_k,											// VEX.L1.0F.W0 4B /r
		VEX_Kunpckdq_k_k_k,											// VEX.L1.0F.W1 4B /r

		VEX_Kunpckbw_k_k_k,											// VEX.L1.66.0F.W0 4B /r

		Movmskps_r32_xmm,											// NP 0F 50 /r
		Movmskps_r64_xmm,											// NP REX.W 0F 50 /r
		VEX_Vmovmskps_r32_xmm,										// VEX.128.0F.W0 50 /r
		VEX_Vmovmskps_r64_xmm,										// VEX.128.0F.W1 50 /r
		VEX_Vmovmskps_r32_ymm,										// VEX.256.0F.W0 50 /r
		VEX_Vmovmskps_r64_ymm,										// VEX.256.0F.W1 50 /r

		Movmskpd_r32_xmm,											// 66 0F 50 /r
		Movmskpd_r64_xmm,											// 66 REX.W 0F 50 /r
		VEX_Vmovmskpd_r32_xmm,										// VEX.128.66.0F.W0 50 /r
		VEX_Vmovmskpd_r64_xmm,										// VEX.128.66.0F.W1 50 /r
		VEX_Vmovmskpd_r32_ymm,										// VEX.256.66.0F.W0 50 /r
		VEX_Vmovmskpd_r64_ymm,										// VEX.256.66.0F.W1 50 /r

		Sqrtps_xmm_xmmm128,											// NP 0F 51 /r
		VEX_Vsqrtps_xmm_xmmm128,									// VEX.128.0F.WIG 51 /r
		VEX_Vsqrtps_ymm_ymmm256,									// VEX.256.0F.WIG 51 /r
		EVEX_Vsqrtps_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 51 /r
		EVEX_Vsqrtps_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 51 /r
		EVEX_Vsqrtps_zmm_k1z_zmmm512b32_er,							// EVEX.512.0F.W0 51 /r

		Sqrtpd_xmm_xmmm128,											// 66 0F 51 /r
		VEX_Vsqrtpd_xmm_xmmm128,									// VEX.128.66.0F.WIG 51 /r
		VEX_Vsqrtpd_ymm_ymmm256,									// VEX.256.66.0F.WIG 51 /r
		EVEX_Vsqrtpd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 51 /r
		EVEX_Vsqrtpd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 51 /r
		EVEX_Vsqrtpd_zmm_k1z_zmmm512b64_er,							// EVEX.512.66.0F.W1 51 /r

		Sqrtss_xmm_xmmm32,											// F3 0F 51 /r
		VEX_Vsqrtss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 51 /r
		EVEX_Vsqrtss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 51 /r

		Sqrtsd_xmm_xmmm64,											// F2 0F 51 /r
		VEX_Vsqrtsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 51 /r
		EVEX_Vsqrtsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 51 /r

		Rsqrtps_xmm_xmmm128,										// NP 0F 52 /r
		VEX_Vrsqrtps_xmm_xmmm128,									// VEX.128.0F.WIG 52 /r
		VEX_Vrsqrtps_ymm_ymmm256,									// VEX.256.0F.WIG 52 /r

		Rsqrtss_xmm_xmmm32,											// F3 0F 52 /r
		VEX_Vrsqrtss_xmm_xmm_xmmm32,								// VEX.LIG.F3.0F.WIG 52 /r

		Rcpps_xmm_xmmm128,											// NP 0F 53 /r
		VEX_Vrcpps_xmm_xmmm128,										// VEX.128.0F.WIG 53 /r
		VEX_Vrcpps_ymm_ymmm256,										// VEX.256.0F.WIG 53 /r

		Rcpss_xmm_xmmm32,											// F3 0F 53 /r
		VEX_Vrcpss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 53 /r

		Andps_xmm_xmmm128,											// NP 0F 54 /r
		VEX_Vandps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 54 /r
		VEX_Vandps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 54 /r
		EVEX_Vandps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 54 /r
		EVEX_Vandps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 54 /r
		EVEX_Vandps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 54 /r

		Andpd_xmm_xmmm128,											// 66 0F 54 /r
		VEX_Vandpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 54 /r
		VEX_Vandpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 54 /r
		EVEX_Vandpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 54 /r
		EVEX_Vandpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 54 /r
		EVEX_Vandpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 54 /r

		Andnps_xmm_xmmm128,											// NP 0F 55 /r
		VEX_Vandnps_xmm_xmm_xmmm128,								// VEX.128.0F.WIG 55 /r
		VEX_Vandnps_ymm_ymm_ymmm256,								// VEX.256.0F.WIG 55 /r
		EVEX_Vandnps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.0F.W0 55 /r
		EVEX_Vandnps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.0F.W0 55 /r
		EVEX_Vandnps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.0F.W0 55 /r

		Andnpd_xmm_xmmm128,											// 66 0F 55 /r
		VEX_Vandnpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 55 /r
		VEX_Vandnpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 55 /r
		EVEX_Vandnpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 55 /r
		EVEX_Vandnpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 55 /r
		EVEX_Vandnpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 55 /r

		Orps_xmm_xmmm128,											// NP 0F 56 /r
		VEX_Vorps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 56 /r
		VEX_Vorps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 56 /r
		EVEX_Vorps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 56 /r
		EVEX_Vorps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 56 /r
		EVEX_Vorps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 56 /r

		Orpd_xmm_xmmm128,											// 66 0F 56 /r
		VEX_Vorpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 56 /r
		VEX_Vorpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 56 /r
		EVEX_Vorpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 56 /r
		EVEX_Vorpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 56 /r
		EVEX_Vorpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 56 /r

		Xorps_xmm_xmmm128,											// NP 0F 57 /r
		VEX_Vxorps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 57 /r
		VEX_Vxorps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 57 /r
		EVEX_Vxorps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 57 /r
		EVEX_Vxorps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 57 /r
		EVEX_Vxorps_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.0F.W0 57 /r

		Xorpd_xmm_xmmm128,											// 66 0F 57 /r
		VEX_Vxorpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 57 /r
		VEX_Vxorpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 57 /r
		EVEX_Vxorpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 57 /r
		EVEX_Vxorpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 57 /r
		EVEX_Vxorpd_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 57 /r

		Addps_xmm_xmmm128,											// NP 0F 58 /r
		VEX_Vaddps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 58 /r
		VEX_Vaddps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 58 /r
		EVEX_Vaddps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 58 /r
		EVEX_Vaddps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 58 /r
		EVEX_Vaddps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 58 /r

		Addpd_xmm_xmmm128,											// 66 0F 58 /r
		VEX_Vaddpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 58 /r
		VEX_Vaddpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 58 /r
		EVEX_Vaddpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 58 /r
		EVEX_Vaddpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 58 /r
		EVEX_Vaddpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 58 /r

		Addss_xmm_xmmm32,											// F3 0F 58 /r
		VEX_Vaddss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 58 /r
		EVEX_Vaddss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 58 /r

		Addsd_xmm_xmmm64,											// F2 0F 58 /r
		VEX_Vaddsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 58 /r
		EVEX_Vaddsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 58 /r

		Mulps_xmm_xmmm128,											// NP 0F 59 /r
		VEX_Vmulps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 59 /r
		VEX_Vmulps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 59 /r
		EVEX_Vmulps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 59 /r
		EVEX_Vmulps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 59 /r
		EVEX_Vmulps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 59 /r

		Mulpd_xmm_xmmm128,											// 66 0F 59 /r
		VEX_Vmulpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 59 /r
		VEX_Vmulpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 59 /r
		EVEX_Vmulpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 59 /r
		EVEX_Vmulpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 59 /r
		EVEX_Vmulpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 59 /r

		Mulss_xmm_xmmm32,											// F3 0F 59 /r
		VEX_Vmulss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 59 /r
		EVEX_Vmulss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 59 /r

		Mulsd_xmm_xmmm64,											// F2 0F 59 /r
		VEX_Vmulsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 59 /r
		EVEX_Vmulsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 59 /r

		Cvtps2pd_xmm_xmmm64,										// NP 0F 5A /r
		VEX_Vcvtps2pd_xmm_xmmm64,									// VEX.128.0F.WIG 5A /r
		VEX_Vcvtps2pd_ymm_xmmm128,									// VEX.256.0F.WIG 5A /r
		EVEX_Vcvtps2pd_xmm_k1z_xmmm64b32,							// EVEX.128.0F.W0 5A /r
		EVEX_Vcvtps2pd_ymm_k1z_xmmm128b32,							// EVEX.256.0F.W0 5A /r
		EVEX_Vcvtps2pd_zmm_k1z_ymmm256b32_sae,						// EVEX.512.0F.W0 5A /r

		Cvtpd2ps_xmm_xmmm128,										// 66 0F 5A /r
		VEX_Vcvtpd2ps_xmm_xmmm128,									// VEX.128.66.0F.WIG 5A /r
		VEX_Vcvtpd2ps_xmm_ymmm256,									// VEX.256.66.0F.WIG 5A /r
		EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 5A /r
		EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 5A /r
		EVEX_Vcvtpd2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 5A /r

		Cvtss2sd_xmm_xmmm32,										// F3 0F 5A /r
		VEX_Vcvtss2sd_xmm_xmm_xmmm32,								// VEX.LIG.F3.0F.WIG 5A /r
		EVEX_Vcvtss2sd_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.F3.0F.W0 5A /r

		Cvtsd2ss_xmm_xmmm64,										// F2 0F 5A /r
		VEX_Vcvtsd2ss_xmm_xmm_xmmm64,								// VEX.LIG.F2.0F.WIG 5A /r
		EVEX_Vcvtsd2ss_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.F2.0F.W1 5A /r

		Cvtdq2ps_xmm_xmmm128,										// NP 0F 5B /r
		VEX_Vcvtdq2ps_xmm_xmmm128,									// VEX.128.0F.WIG 5B /r
		VEX_Vcvtdq2ps_ymm_ymmm256,									// VEX.256.0F.WIG 5B /r
		EVEX_Vcvtdq2ps_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 5B /r
		EVEX_Vcvtdq2ps_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 5B /r
		EVEX_Vcvtdq2ps_zmm_k1z_zmmm512b32_er,						// EVEX.512.0F.W0 5B /r
		EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64,							// EVEX.128.0F.W1 5B /r
		EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64,							// EVEX.256.0F.W1 5B /r
		EVEX_Vcvtqq2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.0F.W1 5B /r

		Cvtps2dq_xmm_xmmm128,										// 66 0F 5B /r
		VEX_Vcvtps2dq_xmm_xmmm128,									// VEX.128.66.0F.WIG 5B /r
		VEX_Vcvtps2dq_ymm_ymmm256,									// VEX.256.66.0F.WIG 5B /r
		EVEX_Vcvtps2dq_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F.W0 5B /r
		EVEX_Vcvtps2dq_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F.W0 5B /r
		EVEX_Vcvtps2dq_zmm_k1z_zmmm512b32_er,						// EVEX.512.66.0F.W0 5B /r

		Cvttps2dq_xmm_xmmm128,										// F3 0F 5B /r
		VEX_Vcvttps2dq_xmm_xmmm128,									// VEX.128.F3.0F.WIG 5B /r
		VEX_Vcvttps2dq_ymm_ymmm256,									// VEX.256.F3.0F.WIG 5B /r
		EVEX_Vcvttps2dq_xmm_k1z_xmmm128b32,							// EVEX.128.F3.0F.W0 5B /r
		EVEX_Vcvttps2dq_ymm_k1z_ymmm256b32,							// EVEX.256.F3.0F.W0 5B /r
		EVEX_Vcvttps2dq_zmm_k1z_zmmm512b32_sae,						// EVEX.512.F3.0F.W0 5B /r

		Subps_xmm_xmmm128,											// NP 0F 5C /r
		VEX_Vsubps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5C /r
		VEX_Vsubps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5C /r
		EVEX_Vsubps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5C /r
		EVEX_Vsubps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5C /r
		EVEX_Vsubps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 5C /r

		Subpd_xmm_xmmm128,											// 66 0F 5C /r
		VEX_Vsubpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5C /r
		VEX_Vsubpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5C /r
		EVEX_Vsubpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5C /r
		EVEX_Vsubpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5C /r
		EVEX_Vsubpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 5C /r

		Subss_xmm_xmmm32,											// F3 0F 5C /r
		VEX_Vsubss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5C /r
		EVEX_Vsubss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 5C /r

		Subsd_xmm_xmmm64,											// F2 0F 5C /r
		VEX_Vsubsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5C /r
		EVEX_Vsubsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 5C /r

		Minps_xmm_xmmm128,											// NP 0F 5D /r
		VEX_Vminps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5D /r
		VEX_Vminps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5D /r
		EVEX_Vminps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5D /r
		EVEX_Vminps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5D /r
		EVEX_Vminps_zmm_k1z_zmm_zmmm512b32_sae,						// EVEX.512.0F.W0 5D /r

		Minpd_xmm_xmmm128,											// 66 0F 5D /r
		VEX_Vminpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5D /r
		VEX_Vminpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5D /r
		EVEX_Vminpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5D /r
		EVEX_Vminpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5D /r
		EVEX_Vminpd_zmm_k1z_zmm_zmmm512b64_sae,						// EVEX.512.66.0F.W1 5D /r

		Minss_xmm_xmmm32,											// F3 0F 5D /r
		VEX_Vminss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5D /r
		EVEX_Vminss_xmm_k1z_xmm_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 5D /r

		Minsd_xmm_xmmm64,											// F2 0F 5D /r
		VEX_Vminsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5D /r
		EVEX_Vminsd_xmm_k1z_xmm_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 5D /r

		Divps_xmm_xmmm128,											// NP 0F 5E /r
		VEX_Vdivps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5E /r
		VEX_Vdivps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5E /r
		EVEX_Vdivps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5E /r
		EVEX_Vdivps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5E /r
		EVEX_Vdivps_zmm_k1z_zmm_zmmm512b32_er,						// EVEX.512.0F.W0 5E /r

		Divpd_xmm_xmmm128,											// 66 0F 5E /r
		VEX_Vdivpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5E /r
		VEX_Vdivpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5E /r
		EVEX_Vdivpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5E /r
		EVEX_Vdivpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5E /r
		EVEX_Vdivpd_zmm_k1z_zmm_zmmm512b64_er,						// EVEX.512.66.0F.W1 5E /r

		Divss_xmm_xmmm32,											// F3 0F 5E /r
		VEX_Vdivss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5E /r
		EVEX_Vdivss_xmm_k1z_xmm_xmmm32_er,							// EVEX.LIG.F3.0F.W0 5E /r

		Divsd_xmm_xmmm64,											// F2 0F 5E /r
		VEX_Vdivsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5E /r
		EVEX_Vdivsd_xmm_k1z_xmm_xmmm64_er,							// EVEX.LIG.F2.0F.W1 5E /r

		Maxps_xmm_xmmm128,											// NP 0F 5F /r
		VEX_Vmaxps_xmm_xmm_xmmm128,									// VEX.128.0F.WIG 5F /r
		VEX_Vmaxps_ymm_ymm_ymmm256,									// VEX.256.0F.WIG 5F /r
		EVEX_Vmaxps_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.0F.W0 5F /r
		EVEX_Vmaxps_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.0F.W0 5F /r
		EVEX_Vmaxps_zmm_k1z_zmm_zmmm512b32_sae,						// EVEX.512.0F.W0 5F /r

		Maxpd_xmm_xmmm128,											// 66 0F 5F /r
		VEX_Vmaxpd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG 5F /r
		VEX_Vmaxpd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG 5F /r
		EVEX_Vmaxpd_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 5F /r
		EVEX_Vmaxpd_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 5F /r
		EVEX_Vmaxpd_zmm_k1z_zmm_zmmm512b64_sae,						// EVEX.512.66.0F.W1 5F /r

		Maxss_xmm_xmmm32,											// F3 0F 5F /r
		VEX_Vmaxss_xmm_xmm_xmmm32,									// VEX.LIG.F3.0F.WIG 5F /r
		EVEX_Vmaxss_xmm_k1z_xmm_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 5F /r

		Maxsd_xmm_xmmm64,											// F2 0F 5F /r
		VEX_Vmaxsd_xmm_xmm_xmmm64,									// VEX.LIG.F2.0F.WIG 5F /r
		EVEX_Vmaxsd_xmm_k1z_xmm_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 5F /r

		Punpcklbw_mm_mmm32,											// NP 0F 60 /r

		Punpcklbw_xmm_xmmm128,										// 66 0F 60 /r
		VEX_Vpunpcklbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 60 /r
		VEX_Vpunpcklbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 60 /r
		EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 60 /r
		EVEX_Vpunpcklbw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 60 /r
		EVEX_Vpunpcklbw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 60 /r

		Punpcklwd_mm_mmm32,											// NP 0F 61 /r

		Punpcklwd_xmm_xmmm128,										// 66 0F 61 /r
		VEX_Vpunpcklwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 61 /r
		VEX_Vpunpcklwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 61 /r
		EVEX_Vpunpcklwd_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 61 /r
		EVEX_Vpunpcklwd_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 61 /r
		EVEX_Vpunpcklwd_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 61 /r

		Punpckldq_mm_mmm32,											// NP 0F 62 /r

		Punpckldq_xmm_xmmm128,										// 66 0F 62 /r
		VEX_Vpunpckldq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 62 /r
		VEX_Vpunpckldq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 62 /r
		EVEX_Vpunpckldq_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 62 /r
		EVEX_Vpunpckldq_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 62 /r
		EVEX_Vpunpckldq_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 62 /r

		Packsswb_mm_mmm64,											// NP 0F 63 /r

		Packsswb_xmm_xmmm128,										// 66 0F 63 /r
		VEX_Vpacksswb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 63 /r
		VEX_Vpacksswb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 63 /r
		EVEX_Vpacksswb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG 63 /r
		EVEX_Vpacksswb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG 63 /r
		EVEX_Vpacksswb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG 63 /r

		Pcmpgtb_mm_mmm64,											// NP 0F 64 /r

		Pcmpgtb_xmm_xmmm128,										// 66 0F 64 /r
		VEX_Vpcmpgtb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 64 /r
		VEX_Vpcmpgtb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 64 /r
		EVEX_Vpcmpgtb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 64 /r
		EVEX_Vpcmpgtb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 64 /r
		EVEX_Vpcmpgtb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 64 /r

		Pcmpgtw_mm_mmm64,											// NP 0F 65 /r

		Pcmpgtw_xmm_xmmm128,										// 66 0F 65 /r
		VEX_Vpcmpgtw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 65 /r
		VEX_Vpcmpgtw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 65 /r
		EVEX_Vpcmpgtw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 65 /r
		EVEX_Vpcmpgtw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 65 /r
		EVEX_Vpcmpgtw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 65 /r

		Pcmpgtd_mm_mmm64,											// NP 0F 66 /r

		Pcmpgtd_xmm_xmmm128,										// 66 0F 66 /r
		VEX_Vpcmpgtd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 66 /r
		VEX_Vpcmpgtd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 66 /r
		EVEX_Vpcmpgtd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 66 /r
		EVEX_Vpcmpgtd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 66 /r
		EVEX_Vpcmpgtd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 66 /r

		Packuswb_mm_mmm64,											// NP 0F 67 /r

		Packuswb_xmm_xmmm128,										// 66 0F 67 /r
		VEX_Vpackuswb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 67 /r
		VEX_Vpackuswb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 67 /r
		EVEX_Vpackuswb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG 67 /r
		EVEX_Vpackuswb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG 67 /r
		EVEX_Vpackuswb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG 67 /r

		Punpckhbw_mm_mmm64,											// NP 0F 68 /r

		Punpckhbw_xmm_xmmm128,										// 66 0F 68 /r
		VEX_Vpunpckhbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 68 /r
		VEX_Vpunpckhbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 68 /r
		EVEX_Vpunpckhbw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 68 /r
		EVEX_Vpunpckhbw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 68 /r
		EVEX_Vpunpckhbw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 68 /r

		Punpckhwd_mm_mmm64,											// NP 0F 69 /r

		Punpckhwd_xmm_xmmm128,										// 66 0F 69 /r
		VEX_Vpunpckhwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 69 /r
		VEX_Vpunpckhwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 69 /r
		EVEX_Vpunpckhwd_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F.WIG 69 /r
		EVEX_Vpunpckhwd_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F.WIG 69 /r
		EVEX_Vpunpckhwd_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F.WIG 69 /r

		Punpckhdq_mm_mmm64,											// NP 0F 6A /r

		Punpckhdq_xmm_xmmm128,										// 66 0F 6A /r
		VEX_Vpunpckhdq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 6A /r
		VEX_Vpunpckhdq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 6A /r
		EVEX_Vpunpckhdq_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 6A /r
		EVEX_Vpunpckhdq_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 6A /r
		EVEX_Vpunpckhdq_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 6A /r

		Packssdw_mm_mmm64,											// NP 0F 6B /r

		Packssdw_xmm_xmmm128,										// 66 0F 6B /r
		VEX_Vpackssdw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 6B /r
		VEX_Vpackssdw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 6B /r
		EVEX_Vpackssdw_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 6B /r
		EVEX_Vpackssdw_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 6B /r
		EVEX_Vpackssdw_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 6B /r

		Punpcklqdq_xmm_xmmm128,										// 66 0F 6C /r
		VEX_Vpunpcklqdq_xmm_xmm_xmmm128,							// VEX.128.66.0F.WIG 6C /r
		VEX_Vpunpcklqdq_ymm_ymm_ymmm256,							// VEX.256.66.0F.WIG 6C /r
		EVEX_Vpunpcklqdq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F.W1 6C /r
		EVEX_Vpunpcklqdq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F.W1 6C /r
		EVEX_Vpunpcklqdq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F.W1 6C /r

		Punpckhqdq_xmm_xmmm128,										// 66 0F 6D /r
		VEX_Vpunpckhqdq_xmm_xmm_xmmm128,							// VEX.128.66.0F.WIG 6D /r
		VEX_Vpunpckhqdq_ymm_ymm_ymmm256,							// VEX.256.66.0F.WIG 6D /r
		EVEX_Vpunpckhqdq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F.W1 6D /r
		EVEX_Vpunpckhqdq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F.W1 6D /r
		EVEX_Vpunpckhqdq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F.W1 6D /r

		Movd_mm_rm32,												// NP 0F 6E /r
		Movq_mm_rm64,												// NP REX.W 0F 6E /r

		Movd_xmm_rm32,												// 66 0F 6E /r
		Movq_xmm_rm64,												// 66 REX.W 0F 6E /r
		VEX_Vmovd_xmm_rm32,											// VEX.128.66.0F.W0 6E /r
		VEX_Vmovq_xmm_rm64,											// VEX.128.66.0F.W1 6E /r
		EVEX_Vmovd_xmm_rm32,										// EVEX.128.66.0F.W0 6E /r
		EVEX_Vmovq_xmm_rm64,										// EVEX.128.66.0F.W1 6E /r

		Movq_mm_mmm64,												// NP 0F 6F /r

		Movdqa_xmm_xmmm128,											// 66 0F 6F /r
		VEX_Vmovdqa_xmm_xmmm128,									// VEX.128.66.0F.WIG 6F /r
		VEX_Vmovdqa_ymm_ymmm256,									// VEX.256.66.0F.WIG 6F /r
		EVEX_Vmovdqa32_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W0 6F /r
		EVEX_Vmovdqa32_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W0 6F /r
		EVEX_Vmovdqa32_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W0 6F /r
		EVEX_Vmovdqa64_xmm_k1z_xmmm128,								// EVEX.128.66.0F.W1 6F /r
		EVEX_Vmovdqa64_ymm_k1z_ymmm256,								// EVEX.256.66.0F.W1 6F /r
		EVEX_Vmovdqa64_zmm_k1z_zmmm512,								// EVEX.512.66.0F.W1 6F /r

		Movdqu_xmm_xmmm128,											// F3 0F 6F /r
		VEX_Vmovdqu_xmm_xmmm128,									// VEX.128.F3.0F.WIG 6F /r
		VEX_Vmovdqu_ymm_ymmm256,									// VEX.256.F3.0F.WIG 6F /r
		EVEX_Vmovdqu32_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W0 6F /r
		EVEX_Vmovdqu32_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W0 6F /r
		EVEX_Vmovdqu32_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W0 6F /r
		EVEX_Vmovdqu64_xmm_k1z_xmmm128,								// EVEX.128.F3.0F.W1 6F /r
		EVEX_Vmovdqu64_ymm_k1z_ymmm256,								// EVEX.256.F3.0F.W1 6F /r
		EVEX_Vmovdqu64_zmm_k1z_zmmm512,								// EVEX.512.F3.0F.W1 6F /r

		EVEX_Vmovdqu8_xmm_k1z_xmmm128,								// EVEX.128.F2.0F.W0 6F /r
		EVEX_Vmovdqu8_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W0 6F /r
		EVEX_Vmovdqu8_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W0 6F /r
		EVEX_Vmovdqu16_xmm_k1z_xmmm128,								// EVEX.128.F2.0F.W1 6F /r
		EVEX_Vmovdqu16_ymm_k1z_ymmm256,								// EVEX.256.F2.0F.W1 6F /r
		EVEX_Vmovdqu16_zmm_k1z_zmmm512,								// EVEX.512.F2.0F.W1 6F /r

		Pshufw_mm_mmm64_imm8,										// NP 0F 70 /r ib

		Pshufd_xmm_xmmm128_imm8,									// 66 0F 70 /r ib
		VEX_Vpshufd_xmm_xmmm128_imm8,								// VEX.128.66.0F.WIG 70 /r ib
		VEX_Vpshufd_ymm_ymmm256_imm8,								// VEX.256.66.0F.WIG 70 /r ib
		EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 70 /r ib
		EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 70 /r ib
		EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 70 /r ib

		Pshufhw_xmm_xmmm128_imm8,									// F3 0F 70 /r ib
		VEX_Vpshufhw_xmm_xmmm128_imm8,								// VEX.128.F3.0F.WIG 70 /r ib
		VEX_Vpshufhw_ymm_ymmm256_imm8,								// VEX.256.F3.0F.WIG 70 /r ib
		EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8,							// EVEX.128.F3.0F.WIG 70 /r ib
		EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8,							// EVEX.256.F3.0F.WIG 70 /r ib
		EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8,							// EVEX.512.F3.0F.WIG 70 /r ib

		Pshuflw_xmm_xmmm128_imm8,									// F2 0F 70 /r ib
		VEX_Vpshuflw_xmm_xmmm128_imm8,								// VEX.128.F2.0F.WIG 70 /r ib
		VEX_Vpshuflw_ymm_ymmm256_imm8,								// VEX.256.F2.0F.WIG 70 /r ib
		EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8,							// EVEX.128.F2.0F.WIG 70 /r ib
		EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8,							// EVEX.256.F2.0F.WIG 70 /r ib
		EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8,							// EVEX.512.F2.0F.WIG 70 /r ib

		Psrlw_mm_imm8,												// NP 0F 71 /2 ib

		Psrlw_xmm_imm8,												// 66 0F 71 /2 ib
		VEX_Vpsrlw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /2 ib
		VEX_Vpsrlw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /2 ib
		EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /2 ib
		EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /2 ib
		EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /2 ib

		Psraw_mm_imm8,												// NP 0F 71 /4 ib

		Psraw_xmm_imm8,												// 66 0F 71 /4 ib
		VEX_Vpsraw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /4 ib
		VEX_Vpsraw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /4 ib
		EVEX_Vpsraw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /4 ib
		EVEX_Vpsraw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /4 ib
		EVEX_Vpsraw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /4 ib

		Psllw_mm_imm8,												// NP 0F 71 /6 ib

		Psllw_xmm_imm8,												// 66 0F 71 /6 ib
		VEX_Vpsllw_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 71 /6 ib
		VEX_Vpsllw_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 71 /6 ib
		EVEX_Vpsllw_xmm_k1z_xmmm128_imm8,							// EVEX.128.66.0F.WIG 71 /6 ib
		EVEX_Vpsllw_ymm_k1z_ymmm256_imm8,							// EVEX.256.66.0F.WIG 71 /6 ib
		EVEX_Vpsllw_zmm_k1z_zmmm512_imm8,							// EVEX.512.66.0F.WIG 71 /6 ib

		EVEX_Vprord_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /0 ib
		EVEX_Vprord_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /0 ib
		EVEX_Vprord_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /0 ib
		EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /0 ib
		EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /0 ib
		EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /0 ib

		EVEX_Vprold_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /1 ib
		EVEX_Vprold_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /1 ib
		EVEX_Vprold_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /1 ib
		EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /1 ib
		EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /1 ib
		EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /1 ib

		Psrld_mm_imm8,												// NP 0F 72 /2 ib

		Psrld_xmm_imm8,												// 66 0F 72 /2 ib
		VEX_Vpsrld_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /2 ib
		VEX_Vpsrld_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /2 ib
		EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /2 ib
		EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /2 ib
		EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /2 ib

		Psrad_mm_imm8,												// NP 0F 72 /4 ib

		Psrad_xmm_imm8,												// 66 0F 72 /4 ib
		VEX_Vpsrad_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /4 ib
		VEX_Vpsrad_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /4 ib
		EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /4 ib
		EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /4 ib
		EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /4 ib
		EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 72 /4 ib
		EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 72 /4 ib
		EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 72 /4 ib

		Pslld_mm_imm8,												// NP 0F 72 /6 ib

		Pslld_xmm_imm8,												// 66 0F 72 /6 ib
		VEX_Vpslld_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 72 /6 ib
		VEX_Vpslld_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 72 /6 ib
		EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F.W0 72 /6 ib
		EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F.W0 72 /6 ib
		EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F.W0 72 /6 ib

		Psrlq_mm_imm8,												// NP 0F 73 /2 ib

		Psrlq_xmm_imm8,												// 66 0F 73 /2 ib
		VEX_Vpsrlq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /2 ib
		VEX_Vpsrlq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /2 ib
		EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 73 /2 ib
		EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 73 /2 ib
		EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 73 /2 ib

		Psrldq_xmm_imm8,											// 66 0F 73 /3 ib
		VEX_Vpsrldq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /3 ib
		VEX_Vpsrldq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /3 ib
		EVEX_Vpsrldq_xmm_xmmm128_imm8,								// EVEX.128.66.0F.WIG 73 /3 ib
		EVEX_Vpsrldq_ymm_ymmm256_imm8,								// EVEX.256.66.0F.WIG 73 /3 ib
		EVEX_Vpsrldq_zmm_zmmm512_imm8,								// EVEX.512.66.0F.WIG 73 /3 ib

		Psllq_mm_imm8,												// NP 0F 73 /6 ib

		Psllq_xmm_imm8,												// 66 0F 73 /6 ib
		VEX_Vpsllq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /6 ib
		VEX_Vpsllq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /6 ib
		EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 73 /6 ib
		EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 73 /6 ib
		EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F.W1 73 /6 ib

		Pslldq_xmm_imm8,											// 66 0F 73 /7 ib
		VEX_Vpslldq_xmm_xmm_imm8,									// VEX.128.66.0F.WIG 73 /7 ib
		VEX_Vpslldq_ymm_ymm_imm8,									// VEX.256.66.0F.WIG 73 /7 ib
		EVEX_Vpslldq_xmm_xmmm128_imm8,								// EVEX.128.66.0F.WIG 73 /7 ib
		EVEX_Vpslldq_ymm_ymmm256_imm8,								// EVEX.256.66.0F.WIG 73 /7 ib
		EVEX_Vpslldq_zmm_zmmm512_imm8,								// EVEX.512.66.0F.WIG 73 /7 ib

		Pcmpeqb_mm_mmm64,											// NP 0F 74 /r

		Pcmpeqb_xmm_xmmm128,										// 66 0F 74 /r
		VEX_Vpcmpeqb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 74 /r
		VEX_Vpcmpeqb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 74 /r
		EVEX_Vpcmpeqb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 74 /r
		EVEX_Vpcmpeqb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 74 /r
		EVEX_Vpcmpeqb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 74 /r

		Pcmpeqw_mm_mmm64,											// NP 0F 75 /r

		Pcmpeqw_xmm_xmmm128,										// 66 0F 75 /r
		VEX_Vpcmpeqw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 75 /r
		VEX_Vpcmpeqw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 75 /r
		EVEX_Vpcmpeqw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F.WIG 75 /r
		EVEX_Vpcmpeqw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F.WIG 75 /r
		EVEX_Vpcmpeqw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F.WIG 75 /r

		Pcmpeqd_mm_mmm64,											// NP 0F 76 /r

		Pcmpeqd_xmm_xmmm128,										// 66 0F 76 /r
		VEX_Vpcmpeqd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 76 /r
		VEX_Vpcmpeqd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 76 /r
		EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 76 /r
		EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 76 /r
		EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 76 /r

		Emms,														// NP 0F 77
		VEX_Vzeroupper,												// VEX.128.0F.WIG 77
		VEX_Vzeroall,												// VEX.256.0F.WIG 77

		Vmread_rm32_r32,											// NP 0F 78 /r
		Vmread_rm64_r64,											// NP 0F 78 /r
		EVEX_Vcvttps2udq_xmm_k1z_xmmm128b32,						// EVEX.128.0F.W0 78 /r
		EVEX_Vcvttps2udq_ymm_k1z_ymmm256b32,						// EVEX.256.0F.W0 78 /r
		EVEX_Vcvttps2udq_zmm_k1z_zmmm512b32_sae,					// EVEX.512.0F.W0 78 /r
		EVEX_Vcvttpd2udq_xmm_k1z_xmmm128b64,						// EVEX.128.0F.W1 78 /r
		EVEX_Vcvttpd2udq_xmm_k1z_ymmm256b64,						// EVEX.256.0F.W1 78 /r
		EVEX_Vcvttpd2udq_ymm_k1z_zmmm512b64_sae,					// EVEX.512.0F.W1 78 /r

		Extrq_xmm_imm8_imm8,										// 66 0F 78 /0 ib ib
		EVEX_Vcvttps2uqq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 78 /r
		EVEX_Vcvttps2uqq_ymm_k1z_xmmm128b32,						// EVEX.256.66.0F.W0 78 /r
		EVEX_Vcvttps2uqq_zmm_k1z_ymmm256b32_sae,					// EVEX.512.66.0F.W0 78 /r
		EVEX_Vcvttpd2uqq_xmm_k1z_xmmm128b64,						// EVEX.128.66.0F.W1 78 /r
		EVEX_Vcvttpd2uqq_ymm_k1z_ymmm256b64,						// EVEX.256.66.0F.W1 78 /r
		EVEX_Vcvttpd2uqq_zmm_k1z_zmmm512b64_sae,					// EVEX.512.66.0F.W1 78 /r

		EVEX_Vcvttss2usi_r32_xmmm32_sae,							// EVEX.LIG.F3.0F.W0 78 /r
		EVEX_Vcvttss2usi_r64_xmmm32_sae,							// EVEX.LIG.F3.0F.W1 78 /r

		Insertq_xmm_xmm_imm8_imm8,									// F2 0F 78 /r ib ib
		EVEX_Vcvttsd2usi_r32_xmmm64_sae,							// EVEX.LIG.F2.0F.W0 78 /r
		EVEX_Vcvttsd2usi_r64_xmmm64_sae,							// EVEX.LIG.F2.0F.W1 78 /r

		Vmwrite_r32_rm32,											// NP 0F 79 /r
		Vmwrite_r64_rm64,											// NP 0F 79 /r
		EVEX_Vcvtps2udq_xmm_k1z_xmmm128b32,							// EVEX.128.0F.W0 79 /r
		EVEX_Vcvtps2udq_ymm_k1z_ymmm256b32,							// EVEX.256.0F.W0 79 /r
		EVEX_Vcvtps2udq_zmm_k1z_zmmm512b32_er,						// EVEX.512.0F.W0 79 /r
		EVEX_Vcvtpd2udq_xmm_k1z_xmmm128b64,							// EVEX.128.0F.W1 79 /r
		EVEX_Vcvtpd2udq_xmm_k1z_ymmm256b64,							// EVEX.256.0F.W1 79 /r
		EVEX_Vcvtpd2udq_ymm_k1z_zmmm512b64_er,						// EVEX.512.0F.W1 79 /r

		Extrq_xmm_xmm,												// 66 0F 79 /r
		EVEX_Vcvtps2uqq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 79 /r
		EVEX_Vcvtps2uqq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 79 /r
		EVEX_Vcvtps2uqq_zmm_k1z_ymmm256b32_er,						// EVEX.512.66.0F.W0 79 /r
		EVEX_Vcvtpd2uqq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 79 /r
		EVEX_Vcvtpd2uqq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 79 /r
		EVEX_Vcvtpd2uqq_zmm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 79 /r

		EVEX_Vcvtss2usi_r32_xmmm32_er,								// EVEX.LIG.F3.0F.W0 79 /r
		EVEX_Vcvtss2usi_r64_xmmm32_er,								// EVEX.LIG.F3.0F.W1 79 /r

		Insertq_xmm_xmm,											// F2 0F 79 /r
		EVEX_Vcvtsd2usi_r32_xmmm64_er,								// EVEX.LIG.F2.0F.W0 79 /r
		EVEX_Vcvtsd2usi_r64_xmmm64_er,								// EVEX.LIG.F2.0F.W1 79 /r

		EVEX_Vcvttps2qq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 7A /r
		EVEX_Vcvttps2qq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 7A /r
		EVEX_Vcvttps2qq_zmm_k1z_ymmm256b32_sae,						// EVEX.512.66.0F.W0 7A /r
		EVEX_Vcvttpd2qq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 7A /r
		EVEX_Vcvttpd2qq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 7A /r
		EVEX_Vcvttpd2qq_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F.W1 7A /r

		EVEX_Vcvtudq2pd_xmm_k1z_xmmm64b32,							// EVEX.128.F3.0F.W0 7A /r
		EVEX_Vcvtudq2pd_ymm_k1z_xmmm128b32,							// EVEX.256.F3.0F.W0 7A /r
		EVEX_Vcvtudq2pd_zmm_k1z_ymmm256b32,							// EVEX.512.F3.0F.W0 7A /r
		EVEX_Vcvtuqq2pd_xmm_k1z_xmmm128b64,							// EVEX.128.F3.0F.W1 7A /r
		EVEX_Vcvtuqq2pd_ymm_k1z_ymmm256b64,							// EVEX.256.F3.0F.W1 7A /r
		EVEX_Vcvtuqq2pd_zmm_k1z_zmmm512b64_er,						// EVEX.512.F3.0F.W1 7A /r

		EVEX_Vcvtudq2ps_xmm_k1z_xmmm128b32,							// EVEX.128.F2.0F.W0 7A /r
		EVEX_Vcvtudq2ps_ymm_k1z_ymmm256b32,							// EVEX.256.F2.0F.W0 7A /r
		EVEX_Vcvtudq2ps_zmm_k1z_zmmm512b32_er,						// EVEX.512.F2.0F.W0 7A /r
		EVEX_Vcvtuqq2ps_xmm_k1z_xmmm128b64,							// EVEX.128.F2.0F.W1 7A /r
		EVEX_Vcvtuqq2ps_xmm_k1z_ymmm256b64,							// EVEX.256.F2.0F.W1 7A /r
		EVEX_Vcvtuqq2ps_ymm_k1z_zmmm512b64_er,						// EVEX.512.F2.0F.W1 7A /r

		EVEX_Vcvtps2qq_xmm_k1z_xmmm64b32,							// EVEX.128.66.0F.W0 7B /r
		EVEX_Vcvtps2qq_ymm_k1z_xmmm128b32,							// EVEX.256.66.0F.W0 7B /r
		EVEX_Vcvtps2qq_zmm_k1z_ymmm256b32_er,						// EVEX.512.66.0F.W0 7B /r
		EVEX_Vcvtpd2qq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 7B /r
		EVEX_Vcvtpd2qq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 7B /r
		EVEX_Vcvtpd2qq_zmm_k1z_zmmm512b64_er,						// EVEX.512.66.0F.W1 7B /r

		EVEX_Vcvtusi2ss_xmm_xmm_rm32_er,							// EVEX.LIG.F3.0F.W0 7B /r
		EVEX_Vcvtusi2ss_xmm_xmm_rm64_er,							// EVEX.LIG.F3.0F.W1 7B /r

		EVEX_Vcvtusi2sd_xmm_xmm_rm32_er,							// EVEX.LIG.F2.0F.W0 7B /r
		EVEX_Vcvtusi2sd_xmm_xmm_rm64_er,							// EVEX.LIG.F2.0F.W1 7B /r

		Haddpd_xmm_xmmm128,											// 66 0F 7C /r
		VEX_Vhaddpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 7C /r
		VEX_Vhaddpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 7C /r

		Haddps_xmm_xmmm128,											// F2 0F 7C /r
		VEX_Vhaddps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG 7C /r
		VEX_Vhaddps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG 7C /r

		Hsubpd_xmm_xmmm128,											// 66 0F 7D /r
		VEX_Vhsubpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG 7D /r
		VEX_Vhsubpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG 7D /r

		Hsubps_xmm_xmmm128,											// F2 0F 7D /r
		VEX_Vhsubps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG 7D /r
		VEX_Vhsubps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG 7D /r

		Movd_rm32_mm,												// NP 0F 7E /r
		Movq_rm64_mm,												// NP REX.W 0F 7E /r

		Movd_rm32_xmm,												// 66 0F 7E /r
		Movq_rm64_xmm,												// 66 REX.W 0F 7E /r
		VEX_Vmovd_rm32_xmm,											// VEX.128.66.0F.W0 7E /r
		VEX_Vmovq_rm64_xmm,											// VEX.128.66.0F.W1 7E /r
		EVEX_Vmovd_rm32_xmm,										// EVEX.128.66.0F.W0 7E /r
		EVEX_Vmovq_rm64_xmm,										// EVEX.128.66.0F.W1 7E /r

		Movq_xmm_xmmm64,											// F3 0F 7E /r
		VEX_Vmovq_xmm_xmmm64,										// VEX.128.F3.0F.WIG 7E /r
		EVEX_Vmovq_xmm_xmmm64,										// EVEX.128.F3.0F.W1 7E /r

		Movq_mmm64_mm,												// NP 0F 7F /r

		Movdqa_xmmm128_xmm,											// 66 0F 7F /r
		VEX_Vmovdqa_xmmm128_xmm,									// VEX.128.66.0F.WIG 7F /r
		VEX_Vmovdqa_ymmm256_ymm,									// VEX.256.66.0F.WIG 7F /r
		EVEX_Vmovdqa32_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W0 7F /r
		EVEX_Vmovdqa32_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W0 7F /r
		EVEX_Vmovdqa32_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W0 7F /r
		EVEX_Vmovdqa64_xmmm128_k1z_xmm,								// EVEX.128.66.0F.W1 7F /r
		EVEX_Vmovdqa64_ymmm256_k1z_ymm,								// EVEX.256.66.0F.W1 7F /r
		EVEX_Vmovdqa64_zmmm512_k1z_zmm,								// EVEX.512.66.0F.W1 7F /r

		Movdqu_xmmm128_xmm,											// F3 0F 7F /r
		VEX_Vmovdqu_xmmm128_xmm,									// VEX.128.F3.0F.WIG 7F /r
		VEX_Vmovdqu_ymmm256_ymm,									// VEX.256.F3.0F.WIG 7F /r
		EVEX_Vmovdqu32_xmmm128_k1z_xmm,								// EVEX.128.F3.0F.W0 7F /r
		EVEX_Vmovdqu32_ymmm256_k1z_ymm,								// EVEX.256.F3.0F.W0 7F /r
		EVEX_Vmovdqu32_zmmm512_k1z_zmm,								// EVEX.512.F3.0F.W0 7F /r
		EVEX_Vmovdqu64_xmmm128_k1z_xmm,								// EVEX.128.F3.0F.W1 7F /r
		EVEX_Vmovdqu64_ymmm256_k1z_ymm,								// EVEX.256.F3.0F.W1 7F /r
		EVEX_Vmovdqu64_zmmm512_k1z_zmm,								// EVEX.512.F3.0F.W1 7F /r

		EVEX_Vmovdqu8_xmmm128_k1z_xmm,								// EVEX.128.F2.0F.W0 7F /r
		EVEX_Vmovdqu8_ymmm256_k1z_ymm,								// EVEX.256.F2.0F.W0 7F /r
		EVEX_Vmovdqu8_zmmm512_k1z_zmm,								// EVEX.512.F2.0F.W0 7F /r
		EVEX_Vmovdqu16_xmmm128_k1z_xmm,								// EVEX.128.F2.0F.W1 7F /r
		EVEX_Vmovdqu16_ymmm256_k1z_ymm,								// EVEX.256.F2.0F.W1 7F /r
		EVEX_Vmovdqu16_zmmm512_k1z_zmm,								// EVEX.512.F2.0F.W1 7F /r

		Jo_rel16,													// o16 0F 80 cw
		Jo_rel32_32,												// o32 0F 80 cd
		Jo_rel32_64,												// 0F 80 cd
		Jno_rel16,													// o16 0F 81 cw
		Jno_rel32_32,												// o32 0F 81 cd
		Jno_rel32_64,												// 0F 81 cd
		Jb_rel16,													// o16 0F 82 cw
		Jb_rel32_32,												// o32 0F 82 cd
		Jb_rel32_64,												// 0F 82 cd
		Jae_rel16,													// o16 0F 83 cw
		Jae_rel32_32,												// o32 0F 83 cd
		Jae_rel32_64,												// 0F 83 cd
		Je_rel16,													// o16 0F 84 cw
		Je_rel32_32,												// o32 0F 84 cd
		Je_rel32_64,												// 0F 84 cd
		Jne_rel16,													// o16 0F 85 cw
		Jne_rel32_32,												// o32 0F 85 cd
		Jne_rel32_64,												// 0F 85 cd
		Jbe_rel16,													// o16 0F 86 cw
		Jbe_rel32_32,												// o32 0F 86 cd
		Jbe_rel32_64,												// 0F 86 cd
		Ja_rel16,													// o16 0F 87 cw
		Ja_rel32_32,												// o32 0F 87 cd
		Ja_rel32_64,												// 0F 87 cd

		Js_rel16,													// o16 0F 88 cw
		Js_rel32_32,												// o32 0F 88 cd
		Js_rel32_64,												// 0F 88 cd
		Jns_rel16,													// o16 0F 89 cw
		Jns_rel32_32,												// o32 0F 89 cd
		Jns_rel32_64,												// 0F 89 cd
		Jp_rel16,													// o16 0F 8A cw
		Jp_rel32_32,												// o32 0F 8A cd
		Jp_rel32_64,												// 0F 8A cd
		Jnp_rel16,													// o16 0F 8B cw
		Jnp_rel32_32,												// o32 0F 8B cd
		Jnp_rel32_64,												// 0F 8B cd
		Jl_rel16,													// o16 0F 8C cw
		Jl_rel32_32,												// o32 0F 8C cd
		Jl_rel32_64,												// 0F 8C cd
		Jge_rel16,													// o16 0F 8D cw
		Jge_rel32_32,												// o32 0F 8D cd
		Jge_rel32_64,												// 0F 8D cd
		Jle_rel16,													// o16 0F 8E cw
		Jle_rel32_32,												// o32 0F 8E cd
		Jle_rel32_64,												// 0F 8E cd
		Jg_rel16,													// o16 0F 8F cw
		Jg_rel32_32,												// o32 0F 8F cd
		Jg_rel32_64,												// 0F 8F cd

		Seto_rm8,													// 0F 90 /r
		Setno_rm8,													// 0F 91 /r
		Setb_rm8,													// 0F 92 /r
		Setae_rm8,													// 0F 93 /r
		Sete_rm8,													// 0F 94 /r
		Setne_rm8,													// 0F 95 /r
		Setbe_rm8,													// 0F 96 /r
		Seta_rm8,													// 0F 97 /r

		Sets_rm8,													// 0F 98 /r
		Setns_rm8,													// 0F 99 /r
		Setp_rm8,													// 0F 9A /r
		Setnp_rm8,													// 0F 9B /r
		Setl_rm8,													// 0F 9C /r
		Setge_rm8,													// 0F 9D /r
		Setle_rm8,													// 0F 9E /r
		Setg_rm8,													// 0F 9F /r

		VEX_Kmovw_k_km16,											// VEX.L0.0F.W0 90 /r
		VEX_Kmovq_k_km64,											// VEX.L0.0F.W1 90 /r

		VEX_Kmovb_k_km8,											// VEX.L0.66.0F.W0 90 /r
		VEX_Kmovd_k_km32,											// VEX.L0.66.0F.W1 90 /r

		VEX_Kmovw_m16_k,											// VEX.L0.0F.W0 91 /r
		VEX_Kmovq_m64_k,											// VEX.L0.0F.W1 91 /r

		VEX_Kmovb_m8_k,												// VEX.L0.66.0F.W0 91 /r
		VEX_Kmovd_m32_k,											// VEX.L0.66.0F.W1 91 /r

		VEX_Kmovw_k_r32,											// VEX.L0.0F.W0 92 /r

		VEX_Kmovb_k_r32,											// VEX.L0.66.0F.W0 92 /r

		VEX_Kmovd_k_r32,											// VEX.L0.F2.0F.W0 92 /r
		VEX_Kmovq_k_r64,											// VEX.L0.F2.0F.W1 92 /r

		VEX_Kmovw_r32_k,											// VEX.L0.0F.W0 93 /r

		VEX_Kmovb_r32_k,											// VEX.L0.66.0F.W0 93 /r

		VEX_Kmovd_r32_k,											// VEX.L0.F2.0F.W0 93 /r
		VEX_Kmovq_r64_k,											// VEX.L0.F2.0F.W1 93 /r

		VEX_Kortestw_k_k,											// VEX.L0.0F.W0 98 /r
		VEX_Kortestq_k_k,											// VEX.L0.0F.W1 98 /r

		VEX_Kortestb_k_k,											// VEX.L0.66.0F.W0 98 /r
		VEX_Kortestd_k_k,											// VEX.L0.66.0F.W1 98 /r

		VEX_Ktestw_k_k,												// VEX.L0.0F.W0 99 /r
		VEX_Ktestq_k_k,												// VEX.L0.0F.W1 99 /r

		VEX_Ktestb_k_k,												// VEX.L0.66.0F.W0 99 /r
		VEX_Ktestd_k_k,												// VEX.L0.66.0F.W1 99 /r

		Pushw_FS,													// o16 0F A0
		Pushd_FS,													// o32 0F A0
		Pushq_FS,													// 0F A0
		Popw_FS,													// o16 0F A1
		Popd_FS,													// o32 0F A1
		Popq_FS,													// 0F A1
		Cpuid,														// 0F A2
		Bt_rm16_r16,												// o16 0F A3 /r
		Bt_rm32_r32,												// o32 0F A3 /r
		Bt_rm64_r64,												// REX.W 0F A3 /r
		Shld_rm16_r16_imm8,											// o16 0F A4 /r ib
		Shld_rm32_r32_imm8,											// o32 0F A4 /r ib
		Shld_rm64_r64_imm8,											// REX.W 0F A4 /r ib
		Shld_rm16_r16_CL,											// o16 0F A5 /r
		Shld_rm32_r32_CL,											// o32 0F A5 /r
		Shld_rm64_r64_CL,											// REX.W 0F A5 /r
		Montmul_16,													// a16 0F A6 C0
		Montmul_32,													// a32 0F A6 C0
		Montmul_64,													// 0F A6 C0
		Xsha1_16,													// a16 0F A6 C8
		Xsha1_32,													// a32 0F A6 C8
		Xsha1_64,													// 0F A6 C8
		Xsha256_16,													// a16 0F A6 D0
		Xsha256_32,													// a32 0F A6 D0
		Xsha256_64,													// 0F A6 D0
		Xbts_r16_rm16,												// o16 0F A6 /r
		Xbts_r32_rm32,												// o32 0F A6 /r
		Xstore_16,													// a16 0F A7 C0
		Xstore_32,													// a32 0F A7 C0
		Xstore_64,													// 0F A7 C0
		XcryptEcb_16,												// a16 0F A7 C8
		XcryptEcb_32,												// a32 0F A7 C8
		XcryptEcb_64,												// 0F A7 C8
		XcryptCbc_16,												// a16 0F A7 D0
		XcryptCbc_32,												// a32 0F A7 D0
		XcryptCbc_64,												// 0F A7 D0
		XcryptCtr_16,												// a16 0F A7 D8
		XcryptCtr_32,												// a32 0F A7 D8
		XcryptCtr_64,												// 0F A7 D8
		XcryptCfb_16,												// a16 0F A7 E0
		XcryptCfb_32,												// a32 0F A7 E0
		XcryptCfb_64,												// 0F A7 E0
		XcryptOfb_16,												// a16 0F A7 E8
		XcryptOfb_32,												// a32 0F A7 E8
		XcryptOfb_64,												// 0F A7 E8
		Ibts_rm16_r16,												// o16 0F A7 /r
		Ibts_rm32_r32,												// o32 0F A7 /r
		Cmpxchg486_rm8_r8,											// 0F A6 /r
		Cmpxchg486_rm16_r16,										// o16 0F A7 /r
		Cmpxchg486_rm32_r32,										// o32 0F A7 /r

		Pushw_GS,													// o16 0F A8
		Pushd_GS,													// o32 0F A8
		Pushq_GS,													// 0F A8
		Popw_GS,													// o16 0F A9
		Popd_GS,													// o32 0F A9
		Popq_GS,													// 0F A9
		Rsm,														// 0F AA
		Bts_rm16_r16,												// o16 0F AB /r
		Bts_rm32_r32,												// o32 0F AB /r
		Bts_rm64_r64,												// REX.W 0F AB /r
		Shrd_rm16_r16_imm8,											// o16 0F AC /r ib
		Shrd_rm32_r32_imm8,											// o32 0F AC /r ib
		Shrd_rm64_r64_imm8,											// REX.W 0F AC /r ib
		Shrd_rm16_r16_CL,											// o16 0F AD /r
		Shrd_rm32_r32_CL,											// o32 0F AD /r
		Shrd_rm64_r64_CL,											// REX.W 0F AD /r

		Fxsave_m512byte,											// NP 0F AE /0
		Fxsave64_m512byte,											// NP REX.W 0F AE /0
		Rdfsbase_r32,												// F3 0F AE /0
		Rdfsbase_r64,												// F3 REX.W 0F AE /0
		Fxrstor_m512byte,											// NP 0F AE /1
		Fxrstor64_m512byte,											// NP REX.W 0F AE /1
		Rdgsbase_r32,												// F3 0F AE /1
		Rdgsbase_r64,												// F3 REX.W 0F AE /1
		Ldmxcsr_m32,												// NP 0F AE /2
		Wrfsbase_r32,												// F3 0F AE /2
		Wrfsbase_r64,												// F3 REX.W 0F AE /2
		VEX_Vldmxcsr_m32,											// VEX.LZ.0F.WIG AE /2
		Stmxcsr_m32,												// NP 0F AE /3
		Wrgsbase_r32,												// F3 0F AE /3
		Wrgsbase_r64,												// F3 REX.W 0F AE /3
		VEX_Vstmxcsr_m32,											// VEX.LZ.0F.WIG AE /3
		Xsave_mem,													// NP 0F AE /4
		Xsave64_mem,												// NP REX.W 0F AE /4
		Ptwrite_rm32,												// F3 0F AE /4
		Ptwrite_rm64,												// F3 REX.W 0F AE /4
		Xrstor_mem,													// NP 0F AE /5
		Xrstor64_mem,												// NP REX.W 0F AE /5
		Incsspd_r32,												// F3 0F AE /5
		Incsspq_r64,												// F3 REX.W 0F AE /5
		Xsaveopt_mem,												// NP 0F AE /6
		Xsaveopt64_mem,												// NP REX.W 0F AE /6
		Clwb_m8,													// 66 0F AE /6
		Tpause_r32,													// 66 0F AE /6
		Tpause_r64,													// 66 REX.W 0F AE /6
		Clrssbsy_m64,												// F3 0F AE /6
		Umonitor_r16,												// a16 F3 0F AE /6
		Umonitor_r32,												// a32 F3 0F AE /6
		Umonitor_r64,												// F3 0F AE /6
		Umwait_r32,													// F2 0F AE /6
		Umwait_r64,													// F2 REX.W 0F AE /6
		Clflush_m8,													// NP 0F AE /7
		Clflushopt_m8,												// 66 0F AE /7
		Lfence,														// NP 0F AE E8
		Lfence_E9,													// NP 0F AE E9
		Lfence_EA,													// NP 0F AE EA
		Lfence_EB,													// NP 0F AE EB
		Lfence_EC,													// NP 0F AE EC
		Lfence_ED,													// NP 0F AE ED
		Lfence_EE,													// NP 0F AE EE
		Lfence_EF,													// NP 0F AE EF
		Mfence,														// NP 0F AE F0
		Mfence_F1,													// NP 0F AE F1
		Mfence_F2,													// NP 0F AE F2
		Mfence_F3,													// NP 0F AE F3
		Mfence_F4,													// NP 0F AE F4
		Mfence_F5,													// NP 0F AE F5
		Mfence_F6,													// NP 0F AE F6
		Mfence_F7,													// NP 0F AE F7
		Sfence,														// NP 0F AE F8
		Sfence_F9,													// NP 0F AE F9
		Sfence_FA,													// NP 0F AE FA
		Sfence_FB,													// NP 0F AE FB
		Sfence_FC,													// NP 0F AE FC
		Sfence_FD,													// NP 0F AE FD
		Sfence_FE,													// NP 0F AE FE
		Sfence_FF,													// NP 0F AE FF
		Pcommit,													// 66 0F AE F8
		Imul_r16_rm16,												// o16 0F AF /r
		Imul_r32_rm32,												// o32 0F AF /r
		Imul_r64_rm64,												// REX.W 0F AF /r

		Cmpxchg_rm8_r8,												// 0F B0 /r
		Cmpxchg_rm16_r16,											// o16 0F B1 /r
		Cmpxchg_rm32_r32,											// o32 0F B1 /r
		Cmpxchg_rm64_r64,											// REX.W 0F B1 /r
		Lss_r16_m1616,												// o16 0F B2 /r
		Lss_r32_m1632,												// o32 0F B2 /r
		Lss_r64_m1664,												// REX.W 0F B2 /r
		Btr_rm16_r16,												// o16 0F B3 /r
		Btr_rm32_r32,												// o32 0F B3 /r
		Btr_rm64_r64,												// REX.W 0F B3 /r
		Lfs_r16_m1616,												// o16 0F B4 /r
		Lfs_r32_m1632,												// o32 0F B4 /r
		Lfs_r64_m1664,												// REX.W 0F B4 /r
		Lgs_r16_m1616,												// o16 0F B5 /r
		Lgs_r32_m1632,												// o32 0F B5 /r
		Lgs_r64_m1664,												// REX.W 0F B5 /r
		Movzx_r16_rm8,												// o16 0F B6 /r
		Movzx_r32_rm8,												// o32 0F B6 /r
		Movzx_r64_rm8,												// REX.W 0F B6 /r
		Movzx_r16_rm16,												// o16 0F B7 /r
		Movzx_r32_rm16,												// o32 0F B7 /r
		Movzx_r64_rm16,												// REX.W 0F B7 /r

		Jmpe_disp16,												// o16 0F B8 cw
		Jmpe_disp32,												// o32 0F B8 cd

		Popcnt_r16_rm16,											// o16 F3 0F B8 /r
		Popcnt_r32_rm32,											// o32 F3 0F B8 /r
		Popcnt_r64_rm64,											// F3 REX.W 0F B8 /r

		Ud1_r16_rm16,												// o16 0F B9 /r
		Ud1_r32_rm32,												// o32 0F B9 /r
		Ud1_r64_rm64,												// REX.W 0F B9 /r
		Bt_rm16_imm8,												// o16 0F BA /4 ib
		Bt_rm32_imm8,												// o32 0F BA /4 ib
		Bt_rm64_imm8,												// REX.W 0F BA /4 ib
		Bts_rm16_imm8,												// o16 0F BA /5 ib
		Bts_rm32_imm8,												// o32 0F BA /5 ib
		Bts_rm64_imm8,												// REX.W 0F BA /5 ib
		Btr_rm16_imm8,												// o16 0F BA /6 ib
		Btr_rm32_imm8,												// o32 0F BA /6 ib
		Btr_rm64_imm8,												// REX.W 0F BA /6 ib
		Btc_rm16_imm8,												// o16 0F BA /7 ib
		Btc_rm32_imm8,												// o32 0F BA /7 ib
		Btc_rm64_imm8,												// REX.W 0F BA /7 ib
		Btc_rm16_r16,												// o16 0F BB /r
		Btc_rm32_r32,												// o32 0F BB /r
		Btc_rm64_r64,												// REX.W 0F BB /r
		Bsf_r16_rm16,												// o16 0F BC /r
		Bsf_r32_rm32,												// o32 0F BC /r
		Bsf_r64_rm64,												// REX.W 0F BC /r
		Tzcnt_r16_rm16,												// o16 F3 0F BC /r
		Tzcnt_r32_rm32,												// o32 F3 0F BC /r
		Tzcnt_r64_rm64,												// F3 REX.W 0F BC /r
		Bsr_r16_rm16,												// o16 0F BD /r
		Bsr_r32_rm32,												// o32 0F BD /r
		Bsr_r64_rm64,												// REX.W 0F BD /r
		Lzcnt_r16_rm16,												// o16 F3 0F BD /r
		Lzcnt_r32_rm32,												// o32 F3 0F BD /r
		Lzcnt_r64_rm64,												// F3 REX.W 0F BD /r
		Movsx_r16_rm8,												// o16 0F BE /r
		Movsx_r32_rm8,												// o32 0F BE /r
		Movsx_r64_rm8,												// REX.W 0F BE /r
		Movsx_r16_rm16,												// o16 0F BF /r
		Movsx_r32_rm16,												// o32 0F BF /r
		Movsx_r64_rm16,												// REX.W 0F BF /r

		Xadd_rm8_r8,												// 0F C0 /r
		Xadd_rm16_r16,												// o16 0F C1 /r
		Xadd_rm32_r32,												// o32 0F C1 /r
		Xadd_rm64_r64,												// REX.W 0F C1 /r

		Cmpps_xmm_xmmm128_imm8,										// NP 0F C2 /r ib
		VEX_Vcmpps_xmm_xmm_xmmm128_imm8,							// VEX.128.0F.WIG C2 /r ib
		VEX_Vcmpps_ymm_ymm_ymmm256_imm8,							// VEX.256.0F.WIG C2 /r ib
		EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.0F.W0 C2 /r ib
		EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.0F.W0 C2 /r ib
		EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae,					// EVEX.512.0F.W0 C2 /r ib

		Cmppd_xmm_xmmm128_imm8,										// 66 0F C2 /r ib
		VEX_Vcmppd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F.WIG C2 /r ib
		VEX_Vcmppd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F.WIG C2 /r ib
		EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F.W1 C2 /r ib
		EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F.W1 C2 /r ib
		EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae,					// EVEX.512.66.0F.W1 C2 /r ib

		Cmpss_xmm_xmmm32_imm8,										// F3 0F C2 /r ib
		VEX_Vcmpss_xmm_xmm_xmmm32_imm8,								// VEX.LIG.F3.0F.WIG C2 /r ib
		EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae,						// EVEX.LIG.F3.0F.W0 C2 /r ib

		Cmpsd_xmm_xmmm64_imm8,										// F2 0F C2 /r ib
		VEX_Vcmpsd_xmm_xmm_xmmm64_imm8,								// VEX.LIG.F2.0F.WIG C2 /r ib
		EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae,						// EVEX.LIG.F2.0F.W1 C2 /r ib

		Movnti_m32_r32,												// NP 0F C3 /r
		Movnti_m64_r64,												// NP REX.W 0F C3 /r

		Pinsrw_mm_r32m16_imm8,										// NP 0F C4 /r ib
		Pinsrw_mm_r64m16_imm8,										// NP REX.W 0F C4 /r ib

		Pinsrw_xmm_r32m16_imm8,										// 66 0F C4 /r ib
		Pinsrw_xmm_r64m16_imm8,										// 66 REX.W 0F C4 /r ib
		VEX_Vpinsrw_xmm_xmm_r32m16_imm8,							// VEX.128.66.0F.W0 C4 /r ib
		VEX_Vpinsrw_xmm_xmm_r64m16_imm8,							// VEX.128.66.0F.W1 C4 /r ib
		EVEX_Vpinsrw_xmm_xmm_r32m16_imm8,							// EVEX.128.66.0F.W0 C4 /r ib
		EVEX_Vpinsrw_xmm_xmm_r64m16_imm8,							// EVEX.128.66.0F.W1 C4 /r ib

		Pextrw_r32_mm_imm8,											// NP 0F C5 /r ib
		Pextrw_r64_mm_imm8,											// NP REX.W 0F C5 /r ib

		Pextrw_r32_xmm_imm8,										// 66 0F C5 /r ib
		Pextrw_r64_xmm_imm8,										// 66 REX.W 0F C5 /r ib
		VEX_Vpextrw_r32_xmm_imm8,									// VEX.128.66.0F.W0 C5 /r ib
		VEX_Vpextrw_r64_xmm_imm8,									// VEX.128.66.0F.W1 C5 /r ib
		EVEX_Vpextrw_r32_xmm_imm8,									// EVEX.128.66.0F.W0 C5 /r ib
		EVEX_Vpextrw_r64_xmm_imm8,									// EVEX.128.66.0F.W1 C5 /r ib

		Shufps_xmm_xmmm128_imm8,									// NP 0F C6 /r ib
		VEX_Vshufps_xmm_xmm_xmmm128_imm8,							// VEX.128.0F.WIG C6 /r ib
		VEX_Vshufps_ymm_ymm_ymmm256_imm8,							// VEX.256.0F.WIG C6 /r ib
		EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.0F.W0 C6 /r ib
		EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.0F.W0 C6 /r ib
		EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.0F.W0 C6 /r ib

		Shufpd_xmm_xmmm128_imm8,									// 66 0F C6 /r ib
		VEX_Vshufpd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F.WIG C6 /r ib
		VEX_Vshufpd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F.WIG C6 /r ib
		EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F.W1 C6 /r ib
		EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F.W1 C6 /r ib
		EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F.W1 C6 /r ib

		Cmpxchg8b_m64,												// 0F C7 /1
		Cmpxchg16b_m128,											// REX.W 0F C7 /1
		Xrstors_mem,												// NP 0F C7 /3
		Xrstors64_mem,												// NP REX.W 0F C7 /3
		Xsavec_mem,													// NP 0F C7 /4
		Xsavec64_mem,												// NP REX.W 0F C7 /4
		Xsaves_mem,													// NP 0F C7 /5
		Xsaves64_mem,												// NP REX.W 0F C7 /5
		Vmptrld_m64,												// NP 0F C7 /6
		Vmclear_m64,												// 66 0F C7 /6
		Vmxon_m64,													// F3 0F C7 /6
		Rdrand_r16,													// o16 0F C7 /6
		Rdrand_r32,													// o32 0F C7 /6
		Rdrand_r64,													// REX.W 0F C7 /6
		Vmptrst_m64,												// NP 0F C7 /7
		Rdseed_r16,													// o16 0F C7 /7
		Rdseed_r32,													// o32 0F C7 /7
		Rdseed_r64,													// REX.W 0F C7 /7
		Rdpid_r32,													// F3 0F C7 /7
		Rdpid_r64,													// F3 0F C7 /7

		Bswap_r16,													// o16 0F C8+rw
		Bswap_r32,													// o32 0F C8+rd
		Bswap_r64,													// REX.W 0F C8+ro

		Addsubpd_xmm_xmmm128,										// 66 0F D0 /r
		VEX_Vaddsubpd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D0 /r
		VEX_Vaddsubpd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D0 /r

		Addsubps_xmm_xmmm128,										// F2 0F D0 /r
		VEX_Vaddsubps_xmm_xmm_xmmm128,								// VEX.128.F2.0F.WIG D0 /r
		VEX_Vaddsubps_ymm_ymm_ymmm256,								// VEX.256.F2.0F.WIG D0 /r

		Psrlw_mm_mmm64,												// NP 0F D1 /r

		Psrlw_xmm_xmmm128,											// 66 0F D1 /r
		VEX_Vpsrlw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D1 /r
		VEX_Vpsrlw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D1 /r
		EVEX_Vpsrlw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D1 /r
		EVEX_Vpsrlw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG D1 /r
		EVEX_Vpsrlw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG D1 /r

		Psrld_mm_mmm64,												// NP 0F D2 /r

		Psrld_xmm_xmmm128,											// 66 0F D2 /r
		VEX_Vpsrld_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D2 /r
		VEX_Vpsrld_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D2 /r
		EVEX_Vpsrld_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 D2 /r
		EVEX_Vpsrld_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 D2 /r
		EVEX_Vpsrld_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 D2 /r

		Psrlq_mm_mmm64,												// NP 0F D3 /r

		Psrlq_xmm_xmmm128,											// 66 0F D3 /r
		VEX_Vpsrlq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D3 /r
		VEX_Vpsrlq_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG D3 /r
		EVEX_Vpsrlq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 D3 /r
		EVEX_Vpsrlq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 D3 /r
		EVEX_Vpsrlq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 D3 /r

		Paddq_mm_mmm64,												// NP 0F D4 /r

		Paddq_xmm_xmmm128,											// 66 0F D4 /r
		VEX_Vpaddq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG D4 /r
		VEX_Vpaddq_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG D4 /r
		EVEX_Vpaddq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 D4 /r
		EVEX_Vpaddq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 D4 /r
		EVEX_Vpaddq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 D4 /r

		Pmullw_mm_mmm64,											// NP 0F D5 /r

		Pmullw_xmm_xmmm128,											// 66 0F D5 /r
		VEX_Vpmullw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D5 /r
		VEX_Vpmullw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D5 /r
		EVEX_Vpmullw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D5 /r
		EVEX_Vpmullw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D5 /r
		EVEX_Vpmullw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D5 /r

		Movq_xmmm64_xmm,											// 66 0F D6 /r
		VEX_Vmovq_xmmm64_xmm,										// VEX.128.66.0F.WIG D6 /r
		EVEX_Vmovq_xmmm64_xmm,										// EVEX.128.66.0F.W1 D6 /r

		Movq2dq_xmm_mm,												// F3 0F D6 /r

		Movdq2q_mm_xmm,												// F2 0F D6 /r

		Pmovmskb_r32_mm,											// NP 0F D7 /r
		Pmovmskb_r64_mm,											// NP REX.W 0F D7 /r

		Pmovmskb_r32_xmm,											// 66 0F D7 /r
		Pmovmskb_r64_xmm,											// 66 REX.W 0F D7 /r
		VEX_Vpmovmskb_r32_xmm,										// VEX.128.66.0F.W0 D7 /r
		VEX_Vpmovmskb_r64_xmm,										// VEX.128.66.0F.W1 D7 /r
		VEX_Vpmovmskb_r32_ymm,										// VEX.256.66.0F.W0 D7 /r
		VEX_Vpmovmskb_r64_ymm,										// VEX.256.66.0F.W1 D7 /r

		Psubusb_mm_mmm64,											// NP 0F D8 /r

		Psubusb_xmm_xmmm128,										// 66 0F D8 /r
		VEX_Vpsubusb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D8 /r
		VEX_Vpsubusb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D8 /r
		EVEX_Vpsubusb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D8 /r
		EVEX_Vpsubusb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D8 /r
		EVEX_Vpsubusb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D8 /r

		Psubusw_mm_mmm64,											// NP 0F D9 /r

		Psubusw_xmm_xmmm128,										// 66 0F D9 /r
		VEX_Vpsubusw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG D9 /r
		VEX_Vpsubusw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG D9 /r
		EVEX_Vpsubusw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG D9 /r
		EVEX_Vpsubusw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG D9 /r
		EVEX_Vpsubusw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG D9 /r

		Pminub_mm_mmm64,											// NP 0F DA /r

		Pminub_xmm_xmmm128,											// 66 0F DA /r
		VEX_Vpminub_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DA /r
		VEX_Vpminub_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DA /r
		EVEX_Vpminub_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DA /r
		EVEX_Vpminub_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DA /r
		EVEX_Vpminub_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DA /r

		Pand_mm_mmm64,												// NP 0F DB /r

		Pand_xmm_xmmm128,											// 66 0F DB /r
		VEX_Vpand_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG DB /r
		VEX_Vpand_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG DB /r
		EVEX_Vpandd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 DB /r
		EVEX_Vpandd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 DB /r
		EVEX_Vpandd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 DB /r
		EVEX_Vpandq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 DB /r
		EVEX_Vpandq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 DB /r
		EVEX_Vpandq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 DB /r

		Paddusb_mm_mmm64,											// NP 0F DC /r

		Paddusb_xmm_xmmm128,										// 66 0F DC /r
		VEX_Vpaddusb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DC /r
		VEX_Vpaddusb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DC /r
		EVEX_Vpaddusb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DC /r
		EVEX_Vpaddusb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DC /r
		EVEX_Vpaddusb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DC /r

		Paddusw_mm_mmm64,											// NP 0F DD /r

		Paddusw_xmm_xmmm128,										// 66 0F DD /r
		VEX_Vpaddusw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DD /r
		VEX_Vpaddusw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DD /r
		EVEX_Vpaddusw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DD /r
		EVEX_Vpaddusw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DD /r
		EVEX_Vpaddusw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DD /r

		Pmaxub_mm_mmm64,											// NP 0F DE /r

		Pmaxub_xmm_xmmm128,											// 66 0F DE /r
		VEX_Vpmaxub_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG DE /r
		VEX_Vpmaxub_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG DE /r
		EVEX_Vpmaxub_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG DE /r
		EVEX_Vpmaxub_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG DE /r
		EVEX_Vpmaxub_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG DE /r

		Pandn_mm_mmm64,												// NP 0F DF /r

		Pandn_xmm_xmmm128,											// 66 0F DF /r
		VEX_Vpandn_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG DF /r
		VEX_Vpandn_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG DF /r
		EVEX_Vpandnd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F.W0 DF /r
		EVEX_Vpandnd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F.W0 DF /r
		EVEX_Vpandnd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F.W0 DF /r
		EVEX_Vpandnq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 DF /r
		EVEX_Vpandnq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 DF /r
		EVEX_Vpandnq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 DF /r

		Pavgb_mm_mmm64,												// NP 0F E0 /r

		Pavgb_xmm_xmmm128,											// 66 0F E0 /r
		VEX_Vpavgb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E0 /r
		VEX_Vpavgb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG E0 /r
		EVEX_Vpavgb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E0 /r
		EVEX_Vpavgb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E0 /r
		EVEX_Vpavgb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E0 /r

		Psraw_mm_mmm64,												// NP 0F E1 /r

		Psraw_xmm_xmmm128,											// 66 0F E1 /r
		VEX_Vpsraw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E1 /r
		VEX_Vpsraw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG E1 /r
		EVEX_Vpsraw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E1 /r
		EVEX_Vpsraw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG E1 /r
		EVEX_Vpsraw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG E1 /r

		Psrad_mm_mmm64,												// NP 0F E2 /r

		Psrad_xmm_xmmm128,											// 66 0F E2 /r
		VEX_Vpsrad_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E2 /r
		VEX_Vpsrad_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG E2 /r
		EVEX_Vpsrad_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 E2 /r
		EVEX_Vpsrad_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 E2 /r
		EVEX_Vpsrad_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 E2 /r
		EVEX_Vpsraq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 E2 /r
		EVEX_Vpsraq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 E2 /r
		EVEX_Vpsraq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 E2 /r

		Pavgw_mm_mmm64,												// NP 0F E3 /r

		Pavgw_xmm_xmmm128,											// 66 0F E3 /r
		VEX_Vpavgw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG E3 /r
		VEX_Vpavgw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG E3 /r
		EVEX_Vpavgw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E3 /r
		EVEX_Vpavgw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E3 /r
		EVEX_Vpavgw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E3 /r

		Pmulhuw_mm_mmm64,											// NP 0F E4 /r

		Pmulhuw_xmm_xmmm128,										// 66 0F E4 /r
		VEX_Vpmulhuw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E4 /r
		VEX_Vpmulhuw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E4 /r
		EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E4 /r
		EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E4 /r
		EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E4 /r

		Pmulhw_mm_mmm64,											// NP 0F E5 /r

		Pmulhw_xmm_xmmm128,											// 66 0F E5 /r
		VEX_Vpmulhw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E5 /r
		VEX_Vpmulhw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E5 /r
		EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E5 /r
		EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E5 /r
		EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E5 /r

		Cvttpd2dq_xmm_xmmm128,										// 66 0F E6 /r
		VEX_Vcvttpd2dq_xmm_xmmm128,									// VEX.128.66.0F.WIG E6 /r
		VEX_Vcvttpd2dq_xmm_ymmm256,									// VEX.256.66.0F.WIG E6 /r
		EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F.W1 E6 /r
		EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64,							// EVEX.256.66.0F.W1 E6 /r
		EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F.W1 E6 /r

		Cvtdq2pd_xmm_xmmm64,										// F3 0F E6 /r
		VEX_Vcvtdq2pd_xmm_xmmm64,									// VEX.128.F3.0F.WIG E6 /r
		VEX_Vcvtdq2pd_ymm_xmmm128,									// VEX.256.F3.0F.WIG E6 /r
		EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32,							// EVEX.128.F3.0F.W0 E6 /r
		EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32,							// EVEX.256.F3.0F.W0 E6 /r
		EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32,							// EVEX.512.F3.0F.W0 E6 /r
		EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64,							// EVEX.128.F3.0F.W1 E6 /r
		EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64,							// EVEX.256.F3.0F.W1 E6 /r
		EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er,						// EVEX.512.F3.0F.W1 E6 /r

		Cvtpd2dq_xmm_xmmm128,										// F2 0F E6 /r
		VEX_Vcvtpd2dq_xmm_xmmm128,									// VEX.128.F2.0F.WIG E6 /r
		VEX_Vcvtpd2dq_xmm_ymmm256,									// VEX.256.F2.0F.WIG E6 /r
		EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64,							// EVEX.128.F2.0F.W1 E6 /r
		EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64,							// EVEX.256.F2.0F.W1 E6 /r
		EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er,						// EVEX.512.F2.0F.W1 E6 /r

		Movntq_m64_mm,												// NP 0F E7 /r

		Movntdq_m128_xmm,											// 66 0F E7 /r
		VEX_Vmovntdq_m128_xmm,										// VEX.128.66.0F.WIG E7 /r
		VEX_Vmovntdq_m256_ymm,										// VEX.256.66.0F.WIG E7 /r
		EVEX_Vmovntdq_m128_xmm,										// EVEX.128.66.0F.W0 E7 /r
		EVEX_Vmovntdq_m256_ymm,										// EVEX.256.66.0F.W0 E7 /r
		EVEX_Vmovntdq_m512_zmm,										// EVEX.512.66.0F.W0 E7 /r

		Psubsb_mm_mmm64,											// NP 0F E8 /r

		Psubsb_xmm_xmmm128,											// 66 0F E8 /r
		VEX_Vpsubsb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E8 /r
		VEX_Vpsubsb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E8 /r
		EVEX_Vpsubsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E8 /r
		EVEX_Vpsubsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E8 /r
		EVEX_Vpsubsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E8 /r

		Psubsw_mm_mmm64,											// NP 0F E9 /r

		Psubsw_xmm_xmmm128,											// 66 0F E9 /r
		VEX_Vpsubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG E9 /r
		VEX_Vpsubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG E9 /r
		EVEX_Vpsubsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG E9 /r
		EVEX_Vpsubsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG E9 /r
		EVEX_Vpsubsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG E9 /r

		Pminsw_mm_mmm64,											// NP 0F EA /r

		Pminsw_xmm_xmmm128,											// 66 0F EA /r
		VEX_Vpminsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EA /r
		VEX_Vpminsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EA /r
		EVEX_Vpminsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EA /r
		EVEX_Vpminsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EA /r
		EVEX_Vpminsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EA /r

		Por_mm_mmm64,												// NP 0F EB /r

		Por_xmm_xmmm128,											// 66 0F EB /r
		VEX_Vpor_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG EB /r
		VEX_Vpor_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG EB /r
		EVEX_Vpord_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 EB /r
		EVEX_Vpord_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 EB /r
		EVEX_Vpord_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 EB /r
		EVEX_Vporq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 EB /r
		EVEX_Vporq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 EB /r
		EVEX_Vporq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 EB /r

		Paddsb_mm_mmm64,											// NP 0F EC /r

		Paddsb_xmm_xmmm128,											// 66 0F EC /r
		VEX_Vpaddsb_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EC /r
		VEX_Vpaddsb_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EC /r
		EVEX_Vpaddsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EC /r
		EVEX_Vpaddsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EC /r
		EVEX_Vpaddsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EC /r

		Paddsw_mm_mmm64,											// NP 0F ED /r

		Paddsw_xmm_xmmm128,											// 66 0F ED /r
		VEX_Vpaddsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG ED /r
		VEX_Vpaddsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG ED /r
		EVEX_Vpaddsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG ED /r
		EVEX_Vpaddsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG ED /r
		EVEX_Vpaddsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG ED /r

		Pmaxsw_mm_mmm64,											// NP 0F EE /r

		Pmaxsw_xmm_xmmm128,											// 66 0F EE /r
		VEX_Vpmaxsw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG EE /r
		VEX_Vpmaxsw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG EE /r
		EVEX_Vpmaxsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG EE /r
		EVEX_Vpmaxsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG EE /r
		EVEX_Vpmaxsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG EE /r

		Pxor_mm_mmm64,												// NP 0F EF /r

		Pxor_xmm_xmmm128,											// 66 0F EF /r
		VEX_Vpxor_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG EF /r
		VEX_Vpxor_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG EF /r
		EVEX_Vpxord_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 EF /r
		EVEX_Vpxord_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 EF /r
		EVEX_Vpxord_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 EF /r
		EVEX_Vpxorq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 EF /r
		EVEX_Vpxorq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 EF /r
		EVEX_Vpxorq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 EF /r

		Lddqu_xmm_m128,												// F2 0F F0 /r
		VEX_Vlddqu_xmm_m128,										// VEX.128.F2.0F.WIG F0 /r
		VEX_Vlddqu_ymm_m256,										// VEX.256.F2.0F.WIG F0 /r

		Psllw_mm_mmm64,												// NP 0F F1 /r

		Psllw_xmm_xmmm128,											// 66 0F F1 /r
		VEX_Vpsllw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F1 /r
		VEX_Vpsllw_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F1 /r
		EVEX_Vpsllw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F1 /r
		EVEX_Vpsllw_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.WIG F1 /r
		EVEX_Vpsllw_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.WIG F1 /r

		Pslld_mm_mmm64,												// NP 0F F2 /r

		Pslld_xmm_xmmm128,											// 66 0F F2 /r
		VEX_Vpslld_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F2 /r
		VEX_Vpslld_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F2 /r
		EVEX_Vpslld_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W0 F2 /r
		EVEX_Vpslld_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W0 F2 /r
		EVEX_Vpslld_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W0 F2 /r

		Psllq_mm_mmm64,												// NP 0F F3 /r

		Psllq_xmm_xmmm128,											// 66 0F F3 /r
		VEX_Vpsllq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F3 /r
		VEX_Vpsllq_ymm_ymm_xmmm128,									// VEX.256.66.0F.WIG F3 /r
		EVEX_Vpsllq_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.W1 F3 /r
		EVEX_Vpsllq_ymm_k1z_ymm_xmmm128,							// EVEX.256.66.0F.W1 F3 /r
		EVEX_Vpsllq_zmm_k1z_zmm_xmmm128,							// EVEX.512.66.0F.W1 F3 /r

		Pmuludq_mm_mmm64,											// NP 0F F4 /r

		Pmuludq_xmm_xmmm128,										// 66 0F F4 /r
		VEX_Vpmuludq_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F4 /r
		VEX_Vpmuludq_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F4 /r
		EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F.W1 F4 /r
		EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F.W1 F4 /r
		EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F.W1 F4 /r

		Pmaddwd_mm_mmm64,											// NP 0F F5 /r

		Pmaddwd_xmm_xmmm128,										// 66 0F F5 /r
		VEX_Vpmaddwd_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F5 /r
		VEX_Vpmaddwd_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F5 /r
		EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F5 /r
		EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F5 /r
		EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F5 /r

		Psadbw_mm_mmm64,											// NP 0F F6 /r

		Psadbw_xmm_xmmm128,											// 66 0F F6 /r
		VEX_Vpsadbw_xmm_xmm_xmmm128,								// VEX.128.66.0F.WIG F6 /r
		VEX_Vpsadbw_ymm_ymm_ymmm256,								// VEX.256.66.0F.WIG F6 /r
		EVEX_Vpsadbw_xmm_xmm_xmmm128,								// EVEX.128.66.0F.WIG F6 /r
		EVEX_Vpsadbw_ymm_ymm_ymmm256,								// EVEX.256.66.0F.WIG F6 /r
		EVEX_Vpsadbw_zmm_zmm_zmmm512,								// EVEX.512.66.0F.WIG F6 /r

		Maskmovq_rDI_mm_mm,											// NP 0F F7 /r

		Maskmovdqu_rDI_xmm_xmm,										// 66 0F F7 /r
		VEX_Vmaskmovdqu_rDI_xmm_xmm,								// VEX.128.66.0F.WIG F7 /r

		Psubb_mm_mmm64,												// NP 0F F8 /r

		Psubb_xmm_xmmm128,											// 66 0F F8 /r
		VEX_Vpsubb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F8 /r
		VEX_Vpsubb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG F8 /r
		EVEX_Vpsubb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F8 /r
		EVEX_Vpsubb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F8 /r
		EVEX_Vpsubb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F8 /r

		Psubw_mm_mmm64,												// NP 0F F9 /r

		Psubw_xmm_xmmm128,											// 66 0F F9 /r
		VEX_Vpsubw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG F9 /r
		VEX_Vpsubw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG F9 /r
		EVEX_Vpsubw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG F9 /r
		EVEX_Vpsubw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG F9 /r
		EVEX_Vpsubw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG F9 /r

		Psubd_mm_mmm64,												// NP 0F FA /r

		Psubd_xmm_xmmm128,											// 66 0F FA /r
		VEX_Vpsubd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FA /r
		VEX_Vpsubd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FA /r
		EVEX_Vpsubd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 FA /r
		EVEX_Vpsubd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 FA /r
		EVEX_Vpsubd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 FA /r

		Psubq_mm_mmm64,												// NP 0F FB /r

		Psubq_xmm_xmmm128,											// 66 0F FB /r
		VEX_Vpsubq_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FB /r
		VEX_Vpsubq_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FB /r
		EVEX_Vpsubq_xmm_k1z_xmm_xmmm128b64,							// EVEX.128.66.0F.W1 FB /r
		EVEX_Vpsubq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F.W1 FB /r
		EVEX_Vpsubq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F.W1 FB /r

		Paddb_mm_mmm64,												// NP 0F FC /r

		Paddb_xmm_xmmm128,											// 66 0F FC /r
		VEX_Vpaddb_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FC /r
		VEX_Vpaddb_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FC /r
		EVEX_Vpaddb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG FC /r
		EVEX_Vpaddb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG FC /r
		EVEX_Vpaddb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG FC /r

		Paddw_mm_mmm64,												// NP 0F FD /r

		Paddw_xmm_xmmm128,											// 66 0F FD /r
		VEX_Vpaddw_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FD /r
		VEX_Vpaddw_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FD /r
		EVEX_Vpaddw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F.WIG FD /r
		EVEX_Vpaddw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F.WIG FD /r
		EVEX_Vpaddw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F.WIG FD /r

		Paddd_mm_mmm64,												// NP 0F FE /r

		Paddd_xmm_xmmm128,											// 66 0F FE /r
		VEX_Vpaddd_xmm_xmm_xmmm128,									// VEX.128.66.0F.WIG FE /r
		VEX_Vpaddd_ymm_ymm_ymmm256,									// VEX.256.66.0F.WIG FE /r
		EVEX_Vpaddd_xmm_k1z_xmm_xmmm128b32,							// EVEX.128.66.0F.W0 FE /r
		EVEX_Vpaddd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F.W0 FE /r
		EVEX_Vpaddd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F.W0 FE /r

		Ud0_r16_rm16,												// o16 0F FF /r
		Ud0_r32_rm32,												// o32 0F FF /r
		Ud0_r64_rm64,												// REX.W 0F FF /r

		// 0F 38 xx opcodes

		Pshufb_mm_mmm64,											// NP 0F 38 00 /r

		Pshufb_xmm_xmmm128,											// 66 0F 38 00 /r
		VEX_Vpshufb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 00 /r
		VEX_Vpshufb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 00 /r
		EVEX_Vpshufb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 00 /r
		EVEX_Vpshufb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 00 /r
		EVEX_Vpshufb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 00 /r

		Phaddw_mm_mmm64,											// NP 0F 38 01 /r

		Phaddw_xmm_xmmm128,											// 66 0F 38 01 /r
		VEX_Vphaddw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 01 /r
		VEX_Vphaddw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 01 /r

		Phaddd_mm_mmm64,											// NP 0F 38 02 /r

		Phaddd_xmm_xmmm128,											// 66 0F 38 02 /r
		VEX_Vphaddd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 02 /r
		VEX_Vphaddd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 02 /r

		Phaddsw_mm_mmm64,											// NP 0F 38 03 /r

		Phaddsw_xmm_xmmm128,										// 66 0F 38 03 /r
		VEX_Vphaddsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 03 /r
		VEX_Vphaddsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 03 /r

		Pmaddubsw_mm_mmm64,											// NP 0F 38 04 /r

		Pmaddubsw_xmm_xmmm128,										// 66 0F 38 04 /r
		VEX_Vpmaddubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 04 /r
		VEX_Vpmaddubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 04 /r
		EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F38.WIG 04 /r
		EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F38.WIG 04 /r
		EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F38.WIG 04 /r

		Phsubw_mm_mmm64,											// NP 0F 38 05 /r

		Phsubw_xmm_xmmm128,											// 66 0F 38 05 /r
		VEX_Vphsubw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 05 /r
		VEX_Vphsubw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 05 /r

		Phsubd_mm_mmm64,											// NP 0F 38 06 /r

		Phsubd_xmm_xmmm128,											// 66 0F 38 06 /r
		VEX_Vphsubd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 06 /r
		VEX_Vphsubd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 06 /r

		Phsubsw_mm_mmm64,											// NP 0F 38 07 /r

		Phsubsw_xmm_xmmm128,										// 66 0F 38 07 /r
		VEX_Vphsubsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 07 /r
		VEX_Vphsubsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 07 /r

		Psignb_mm_mmm64,											// NP 0F 38 08 /r

		Psignb_xmm_xmmm128,											// 66 0F 38 08 /r
		VEX_Vpsignb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 08 /r
		VEX_Vpsignb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 08 /r

		Psignw_mm_mmm64,											// NP 0F 38 09 /r

		Psignw_xmm_xmmm128,											// 66 0F 38 09 /r
		VEX_Vpsignw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 09 /r
		VEX_Vpsignw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 09 /r

		Psignd_mm_mmm64,											// NP 0F 38 0A /r

		Psignd_xmm_xmmm128,											// 66 0F 38 0A /r
		VEX_Vpsignd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 0A /r
		VEX_Vpsignd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 0A /r

		Pmulhrsw_mm_mmm64,											// NP 0F 38 0B /r

		Pmulhrsw_xmm_xmmm128,										// 66 0F 38 0B /r
		VEX_Vpmulhrsw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 0B /r
		VEX_Vpmulhrsw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 0B /r
		EVEX_Vpmulhrsw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 0B /r
		EVEX_Vpmulhrsw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 0B /r
		EVEX_Vpmulhrsw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 0B /r

		VEX_Vpermilps_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 0C /r
		VEX_Vpermilps_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 0C /r
		EVEX_Vpermilps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 0C /r
		EVEX_Vpermilps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 0C /r
		EVEX_Vpermilps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 0C /r

		VEX_Vpermilpd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 0D /r
		VEX_Vpermilpd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 0D /r
		EVEX_Vpermilpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 0D /r
		EVEX_Vpermilpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 0D /r
		EVEX_Vpermilpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 0D /r

		VEX_Vtestps_xmm_xmmm128,									// VEX.128.66.0F38.W0 0E /r
		VEX_Vtestps_ymm_ymmm256,									// VEX.256.66.0F38.W0 0E /r

		VEX_Vtestpd_xmm_xmmm128,									// VEX.128.66.0F38.W0 0F /r
		VEX_Vtestpd_ymm_ymmm256,									// VEX.256.66.0F38.W0 0F /r

		Pblendvb_xmm_xmmm128,										// 66 0F 38 10 /r

		EVEX_Vpsrlvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 10 /r
		EVEX_Vpsrlvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 10 /r
		EVEX_Vpsrlvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 10 /r

		EVEX_Vpmovuswb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 10 /r
		EVEX_Vpmovuswb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 10 /r
		EVEX_Vpmovuswb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 10 /r

		EVEX_Vpsravw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 11 /r
		EVEX_Vpsravw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 11 /r
		EVEX_Vpsravw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 11 /r

		EVEX_Vpmovusdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 11 /r
		EVEX_Vpmovusdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 11 /r
		EVEX_Vpmovusdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 11 /r

		EVEX_Vpsllvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 12 /r
		EVEX_Vpsllvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 12 /r
		EVEX_Vpsllvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 12 /r

		EVEX_Vpmovusqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 12 /r
		EVEX_Vpmovusqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 12 /r
		EVEX_Vpmovusqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 12 /r

		VEX_Vcvtph2ps_xmm_xmmm64,									// VEX.128.66.0F38.W0 13 /r
		VEX_Vcvtph2ps_ymm_xmmm128,									// VEX.256.66.0F38.W0 13 /r
		EVEX_Vcvtph2ps_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 13 /r
		EVEX_Vcvtph2ps_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 13 /r
		EVEX_Vcvtph2ps_zmm_k1z_ymmm256_sae,							// EVEX.512.66.0F38.W0 13 /r

		EVEX_Vpmovusdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 13 /r
		EVEX_Vpmovusdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 13 /r
		EVEX_Vpmovusdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 13 /r

		Blendvps_xmm_xmmm128,										// 66 0F 38 14 /r
		EVEX_Vprorvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 14 /r
		EVEX_Vprorvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 14 /r
		EVEX_Vprorvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 14 /r
		EVEX_Vprorvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 14 /r
		EVEX_Vprorvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 14 /r
		EVEX_Vprorvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 14 /r

		EVEX_Vpmovusqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 14 /r
		EVEX_Vpmovusqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 14 /r
		EVEX_Vpmovusqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 14 /r

		Blendvpd_xmm_xmmm128,										// 66 0F 38 15 /r
		EVEX_Vprolvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 15 /r
		EVEX_Vprolvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 15 /r
		EVEX_Vprolvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 15 /r
		EVEX_Vprolvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 15 /r
		EVEX_Vprolvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 15 /r
		EVEX_Vprolvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 15 /r

		EVEX_Vpmovusqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 15 /r
		EVEX_Vpmovusqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 15 /r
		EVEX_Vpmovusqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 15 /r

		VEX_Vpermps_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 16 /r
		EVEX_Vpermps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 16 /r
		EVEX_Vpermps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 16 /r
		EVEX_Vpermpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 16 /r
		EVEX_Vpermpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 16 /r

		Ptest_xmm_xmmm128,											// 66 0F 38 17 /r
		VEX_Vptest_xmm_xmmm128,										// VEX.128.66.0F38.WIG 17 /r
		VEX_Vptest_ymm_ymmm256,										// VEX.256.66.0F38.WIG 17 /r

		VEX_Vbroadcastss_xmm_xmmm32,								// VEX.128.66.0F38.W0 18 /r
		VEX_Vbroadcastss_ymm_xmmm32,								// VEX.256.66.0F38.W0 18 /r
		EVEX_Vbroadcastss_xmm_k1z_xmmm32,							// EVEX.128.66.0F38.W0 18 /r
		EVEX_Vbroadcastss_ymm_k1z_xmmm32,							// EVEX.256.66.0F38.W0 18 /r
		EVEX_Vbroadcastss_zmm_k1z_xmmm32,							// EVEX.512.66.0F38.W0 18 /r

		VEX_Vbroadcastsd_ymm_xmmm64,								// VEX.256.66.0F38.W0 19 /r
		EVEX_Vbroadcastf32x2_ymm_k1z_xmmm64,						// EVEX.256.66.0F38.W0 19 /r
		EVEX_Vbroadcastf32x2_zmm_k1z_xmmm64,						// EVEX.512.66.0F38.W0 19 /r
		EVEX_Vbroadcastsd_ymm_k1z_xmmm64,							// EVEX.256.66.0F38.W1 19 /r
		EVEX_Vbroadcastsd_zmm_k1z_xmmm64,							// EVEX.512.66.0F38.W1 19 /r

		VEX_Vbroadcastf128_ymm_m128,								// VEX.256.66.0F38.W0 1A /r
		EVEX_Vbroadcastf32x4_ymm_k1z_m128,							// EVEX.256.66.0F38.W0 1A /r
		EVEX_Vbroadcastf32x4_zmm_k1z_m128,							// EVEX.512.66.0F38.W0 1A /r
		EVEX_Vbroadcastf64x2_ymm_k1z_m128,							// EVEX.256.66.0F38.W1 1A /r
		EVEX_Vbroadcastf64x2_zmm_k1z_m128,							// EVEX.512.66.0F38.W1 1A /r

		EVEX_Vbroadcastf32x8_zmm_k1z_m256,							// EVEX.512.66.0F38.W0 1B /r
		EVEX_Vbroadcastf64x4_zmm_k1z_m256,							// EVEX.512.66.0F38.W1 1B /r

		Pabsb_mm_mmm64,												// NP 0F 38 1C /r

		Pabsb_xmm_xmmm128,											// 66 0F 38 1C /r
		VEX_Vpabsb_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1C /r
		VEX_Vpabsb_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1C /r
		EVEX_Vpabsb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.WIG 1C /r
		EVEX_Vpabsb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.WIG 1C /r
		EVEX_Vpabsb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.WIG 1C /r

		Pabsw_mm_mmm64,												// NP 0F 38 1D /r

		Pabsw_xmm_xmmm128,											// 66 0F 38 1D /r
		VEX_Vpabsw_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1D /r
		VEX_Vpabsw_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1D /r
		EVEX_Vpabsw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.WIG 1D /r
		EVEX_Vpabsw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.WIG 1D /r
		EVEX_Vpabsw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.WIG 1D /r

		Pabsd_mm_mmm64,												// NP 0F 38 1E /r

		Pabsd_xmm_xmmm128,											// 66 0F 38 1E /r
		VEX_Vpabsd_xmm_xmmm128,										// VEX.128.66.0F38.WIG 1E /r
		VEX_Vpabsd_ymm_ymmm256,										// VEX.256.66.0F38.WIG 1E /r
		EVEX_Vpabsd_xmm_k1z_xmmm128b32,								// EVEX.128.66.0F38.W0 1E /r
		EVEX_Vpabsd_ymm_k1z_ymmm256b32,								// EVEX.256.66.0F38.W0 1E /r
		EVEX_Vpabsd_zmm_k1z_zmmm512b32,								// EVEX.512.66.0F38.W0 1E /r

		EVEX_Vpabsq_xmm_k1z_xmmm128b64,								// EVEX.128.66.0F38.W1 1F /r
		EVEX_Vpabsq_ymm_k1z_ymmm256b64,								// EVEX.256.66.0F38.W1 1F /r
		EVEX_Vpabsq_zmm_k1z_zmmm512b64,								// EVEX.512.66.0F38.W1 1F /r

		Pmovsxbw_xmm_xmmm64,										// 66 0F 38 20 /r
		VEX_Vpmovsxbw_xmm_xmmm64,									// VEX.128.66.0F38.WIG 20 /r
		VEX_Vpmovsxbw_ymm_xmmm128,									// VEX.256.66.0F38.WIG 20 /r
		EVEX_Vpmovsxbw_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 20 /r
		EVEX_Vpmovsxbw_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 20 /r
		EVEX_Vpmovsxbw_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 20 /r

		EVEX_Vpmovswb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 20 /r
		EVEX_Vpmovswb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 20 /r
		EVEX_Vpmovswb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 20 /r

		Pmovsxbd_xmm_xmmm32,										// 66 0F 38 21 /r
		VEX_Vpmovsxbd_xmm_xmmm32,									// VEX.128.66.0F38.WIG 21 /r
		VEX_Vpmovsxbd_ymm_xmmm64,									// VEX.256.66.0F38.WIG 21 /r
		EVEX_Vpmovsxbd_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 21 /r
		EVEX_Vpmovsxbd_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 21 /r
		EVEX_Vpmovsxbd_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 21 /r

		EVEX_Vpmovsdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 21 /r
		EVEX_Vpmovsdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 21 /r
		EVEX_Vpmovsdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 21 /r

		Pmovsxbq_xmm_xmmm16,										// 66 0F 38 22 /r
		VEX_Vpmovsxbq_xmm_xmmm16,									// VEX.128.66.0F38.WIG 22 /r
		VEX_Vpmovsxbq_ymm_xmmm32,									// VEX.256.66.0F38.WIG 22 /r
		EVEX_Vpmovsxbq_xmm_k1z_xmmm16,								// EVEX.128.66.0F38.WIG 22 /r
		EVEX_Vpmovsxbq_ymm_k1z_xmmm32,								// EVEX.256.66.0F38.WIG 22 /r
		EVEX_Vpmovsxbq_zmm_k1z_xmmm64,								// EVEX.512.66.0F38.WIG 22 /r

		EVEX_Vpmovsqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 22 /r
		EVEX_Vpmovsqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 22 /r
		EVEX_Vpmovsqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 22 /r

		Pmovsxwd_xmm_xmmm64,										// 66 0F 38 23 /r
		VEX_Vpmovsxwd_xmm_xmmm64,									// VEX.128.66.0F38.WIG 23 /r
		VEX_Vpmovsxwd_ymm_xmmm128,									// VEX.256.66.0F38.WIG 23 /r
		EVEX_Vpmovsxwd_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 23 /r
		EVEX_Vpmovsxwd_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 23 /r
		EVEX_Vpmovsxwd_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 23 /r

		EVEX_Vpmovsdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 23 /r
		EVEX_Vpmovsdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 23 /r
		EVEX_Vpmovsdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 23 /r

		Pmovsxwq_xmm_xmmm32,										// 66 0F 38 24 /r
		VEX_Vpmovsxwq_xmm_xmmm32,									// VEX.128.66.0F38.WIG 24 /r
		VEX_Vpmovsxwq_ymm_xmmm64,									// VEX.256.66.0F38.WIG 24 /r
		EVEX_Vpmovsxwq_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 24 /r
		EVEX_Vpmovsxwq_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 24 /r
		EVEX_Vpmovsxwq_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 24 /r

		EVEX_Vpmovsqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 24 /r
		EVEX_Vpmovsqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 24 /r
		EVEX_Vpmovsqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 24 /r

		Pmovsxdq_xmm_xmmm64,										// 66 0F 38 25 /r
		VEX_Vpmovsxdq_xmm_xmmm64,									// VEX.128.66.0F38.WIG 25 /r
		VEX_Vpmovsxdq_ymm_xmmm128,									// VEX.256.66.0F38.WIG 25 /r
		EVEX_Vpmovsxdq_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 25 /r
		EVEX_Vpmovsxdq_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 25 /r
		EVEX_Vpmovsxdq_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.W0 25 /r

		EVEX_Vpmovsqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 25 /r
		EVEX_Vpmovsqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 25 /r
		EVEX_Vpmovsqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 25 /r

		EVEX_Vptestmb_k_k1_xmm_xmmm128,								// EVEX.128.66.0F38.W0 26 /r
		EVEX_Vptestmb_k_k1_ymm_ymmm256,								// EVEX.256.66.0F38.W0 26 /r
		EVEX_Vptestmb_k_k1_zmm_zmmm512,								// EVEX.512.66.0F38.W0 26 /r
		EVEX_Vptestmw_k_k1_xmm_xmmm128,								// EVEX.128.66.0F38.W1 26 /r
		EVEX_Vptestmw_k_k1_ymm_ymmm256,								// EVEX.256.66.0F38.W1 26 /r
		EVEX_Vptestmw_k_k1_zmm_zmmm512,								// EVEX.512.66.0F38.W1 26 /r

		EVEX_Vptestnmb_k_k1_xmm_xmmm128,							// EVEX.128.F3.0F38.W0 26 /r
		EVEX_Vptestnmb_k_k1_ymm_ymmm256,							// EVEX.256.F3.0F38.W0 26 /r
		EVEX_Vptestnmb_k_k1_zmm_zmmm512,							// EVEX.512.F3.0F38.W0 26 /r
		EVEX_Vptestnmw_k_k1_xmm_xmmm128,							// EVEX.128.F3.0F38.W1 26 /r
		EVEX_Vptestnmw_k_k1_ymm_ymmm256,							// EVEX.256.F3.0F38.W1 26 /r
		EVEX_Vptestnmw_k_k1_zmm_zmmm512,							// EVEX.512.F3.0F38.W1 26 /r

		EVEX_Vptestmd_k_k1_xmm_xmmm128b32,							// EVEX.128.66.0F38.W0 27 /r
		EVEX_Vptestmd_k_k1_ymm_ymmm256b32,							// EVEX.256.66.0F38.W0 27 /r
		EVEX_Vptestmd_k_k1_zmm_zmmm512b32,							// EVEX.512.66.0F38.W0 27 /r
		EVEX_Vptestmq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 27 /r
		EVEX_Vptestmq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 27 /r
		EVEX_Vptestmq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 27 /r

		EVEX_Vptestnmd_k_k1_xmm_xmmm128b32,							// EVEX.128.F3.0F38.W0 27 /r
		EVEX_Vptestnmd_k_k1_ymm_ymmm256b32,							// EVEX.256.F3.0F38.W0 27 /r
		EVEX_Vptestnmd_k_k1_zmm_zmmm512b32,							// EVEX.512.F3.0F38.W0 27 /r
		EVEX_Vptestnmq_k_k1_xmm_xmmm128b64,							// EVEX.128.F3.0F38.W1 27 /r
		EVEX_Vptestnmq_k_k1_ymm_ymmm256b64,							// EVEX.256.F3.0F38.W1 27 /r
		EVEX_Vptestnmq_k_k1_zmm_zmmm512b64,							// EVEX.512.F3.0F38.W1 27 /r

		Pmuldq_xmm_xmmm128,											// 66 0F 38 28 /r
		VEX_Vpmuldq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 28 /r
		VEX_Vpmuldq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 28 /r
		EVEX_Vpmuldq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 28 /r
		EVEX_Vpmuldq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 28 /r
		EVEX_Vpmuldq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 28 /r

		EVEX_Vpmovm2b_xmm_k,										// EVEX.128.F3.0F38.W0 28 /r
		EVEX_Vpmovm2b_ymm_k,										// EVEX.256.F3.0F38.W0 28 /r
		EVEX_Vpmovm2b_zmm_k,										// EVEX.512.F3.0F38.W0 28 /r
		EVEX_Vpmovm2w_xmm_k,										// EVEX.128.F3.0F38.W1 28 /r
		EVEX_Vpmovm2w_ymm_k,										// EVEX.256.F3.0F38.W1 28 /r
		EVEX_Vpmovm2w_zmm_k,										// EVEX.512.F3.0F38.W1 28 /r

		Pcmpeqq_xmm_xmmm128,										// 66 0F 38 29 /r
		VEX_Vpcmpeqq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 29 /r
		VEX_Vpcmpeqq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 29 /r
		EVEX_Vpcmpeqq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 29 /r
		EVEX_Vpcmpeqq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 29 /r
		EVEX_Vpcmpeqq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 29 /r

		EVEX_Vpmovb2m_k_xmm,										// EVEX.128.F3.0F38.W0 29 /r
		EVEX_Vpmovb2m_k_ymm,										// EVEX.256.F3.0F38.W0 29 /r
		EVEX_Vpmovb2m_k_zmm,										// EVEX.512.F3.0F38.W0 29 /r
		EVEX_Vpmovw2m_k_xmm,										// EVEX.128.F3.0F38.W1 29 /r
		EVEX_Vpmovw2m_k_ymm,										// EVEX.256.F3.0F38.W1 29 /r
		EVEX_Vpmovw2m_k_zmm,										// EVEX.512.F3.0F38.W1 29 /r

		Movntdqa_xmm_m128,											// 66 0F 38 2A /r
		VEX_Vmovntdqa_xmm_m128,										// VEX.128.66.0F38.WIG 2A /r
		VEX_Vmovntdqa_ymm_m256,										// VEX.256.66.0F38.WIG 2A /r
		EVEX_Vmovntdqa_xmm_m128,									// EVEX.128.66.0F38.W0 2A /r
		EVEX_Vmovntdqa_ymm_m256,									// EVEX.256.66.0F38.W0 2A /r
		EVEX_Vmovntdqa_zmm_m512,									// EVEX.512.66.0F38.W0 2A /r

		EVEX_Vpbroadcastmb2q_xmm_k,									// EVEX.128.F3.0F38.W1 2A /r
		EVEX_Vpbroadcastmb2q_ymm_k,									// EVEX.256.F3.0F38.W1 2A /r
		EVEX_Vpbroadcastmb2q_zmm_k,									// EVEX.512.F3.0F38.W1 2A /r

		Packusdw_xmm_xmmm128,										// 66 0F 38 2B /r
		VEX_Vpackusdw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 2B /r
		VEX_Vpackusdw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 2B /r
		EVEX_Vpackusdw_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 2B /r
		EVEX_Vpackusdw_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 2B /r
		EVEX_Vpackusdw_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 2B /r

		VEX_Vmaskmovps_xmm_xmm_m128,								// VEX.128.66.0F38.W0 2C /r
		VEX_Vmaskmovps_ymm_ymm_m256,								// VEX.256.66.0F38.W0 2C /r
		EVEX_Vscalefps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 2C /r
		EVEX_Vscalefps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 2C /r
		EVEX_Vscalefps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 2C /r
		EVEX_Vscalefpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 2C /r
		EVEX_Vscalefpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 2C /r
		EVEX_Vscalefpd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 2C /r

		VEX_Vmaskmovpd_xmm_xmm_m128,								// VEX.128.66.0F38.W0 2D /r
		VEX_Vmaskmovpd_ymm_ymm_m256,								// VEX.256.66.0F38.W0 2D /r
		EVEX_Vscalefss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 2D /r
		EVEX_Vscalefsd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 2D /r

		VEX_Vmaskmovps_m128_xmm_xmm,								// VEX.128.66.0F38.W0 2E /r
		VEX_Vmaskmovps_m256_ymm_ymm,								// VEX.256.66.0F38.W0 2E /r

		VEX_Vmaskmovpd_m128_xmm_xmm,								// VEX.128.66.0F38.W0 2F /r
		VEX_Vmaskmovpd_m256_ymm_ymm,								// VEX.256.66.0F38.W0 2F /r

		Pmovzxbw_xmm_xmmm64,										// 66 0F 38 30 /r
		VEX_Vpmovzxbw_xmm_xmmm64,									// VEX.128.66.0F38.WIG 30 /r
		VEX_Vpmovzxbw_ymm_xmmm128,									// VEX.256.66.0F38.WIG 30 /r
		EVEX_Vpmovzxbw_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 30 /r
		EVEX_Vpmovzxbw_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 30 /r
		EVEX_Vpmovzxbw_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 30 /r

		EVEX_Vpmovwb_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 30 /r
		EVEX_Vpmovwb_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 30 /r
		EVEX_Vpmovwb_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 30 /r

		Pmovzxbd_xmm_xmmm32,										// 66 0F 38 31 /r
		VEX_Vpmovzxbd_xmm_xmmm32,									// VEX.128.66.0F38.WIG 31 /r
		VEX_Vpmovzxbd_ymm_xmmm64,									// VEX.256.66.0F38.WIG 31 /r
		EVEX_Vpmovzxbd_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 31 /r
		EVEX_Vpmovzxbd_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 31 /r
		EVEX_Vpmovzxbd_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 31 /r

		EVEX_Vpmovdb_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 31 /r
		EVEX_Vpmovdb_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 31 /r
		EVEX_Vpmovdb_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 31 /r

		Pmovzxbq_xmm_xmmm16,										// 66 0F 38 32 /r
		VEX_Vpmovzxbq_xmm_xmmm16,									// VEX.128.66.0F38.WIG 32 /r
		VEX_Vpmovzxbq_ymm_xmmm32,									// VEX.256.66.0F38.WIG 32 /r
		EVEX_Vpmovzxbq_xmm_k1z_xmmm16,								// EVEX.128.66.0F38.WIG 32 /r
		EVEX_Vpmovzxbq_ymm_k1z_xmmm32,								// EVEX.256.66.0F38.WIG 32 /r
		EVEX_Vpmovzxbq_zmm_k1z_xmmm64,								// EVEX.512.66.0F38.WIG 32 /r

		EVEX_Vpmovqb_xmmm16_k1z_xmm,								// EVEX.128.F3.0F38.W0 32 /r
		EVEX_Vpmovqb_xmmm32_k1z_ymm,								// EVEX.256.F3.0F38.W0 32 /r
		EVEX_Vpmovqb_xmmm64_k1z_zmm,								// EVEX.512.F3.0F38.W0 32 /r

		Pmovzxwd_xmm_xmmm64,										// 66 0F 38 33 /r
		VEX_Vpmovzxwd_xmm_xmmm64,									// VEX.128.66.0F38.WIG 33 /r
		VEX_Vpmovzxwd_ymm_xmmm128,									// VEX.256.66.0F38.WIG 33 /r
		EVEX_Vpmovzxwd_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.WIG 33 /r
		EVEX_Vpmovzxwd_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.WIG 33 /r
		EVEX_Vpmovzxwd_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.WIG 33 /r

		EVEX_Vpmovdw_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 33 /r
		EVEX_Vpmovdw_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 33 /r
		EVEX_Vpmovdw_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 33 /r

		Pmovzxwq_xmm_xmmm32,										// 66 0F 38 34 /r
		VEX_Vpmovzxwq_xmm_xmmm32,									// VEX.128.66.0F38.WIG 34 /r
		VEX_Vpmovzxwq_ymm_xmmm64,									// VEX.256.66.0F38.WIG 34 /r
		EVEX_Vpmovzxwq_xmm_k1z_xmmm32,								// EVEX.128.66.0F38.WIG 34 /r
		EVEX_Vpmovzxwq_ymm_k1z_xmmm64,								// EVEX.256.66.0F38.WIG 34 /r
		EVEX_Vpmovzxwq_zmm_k1z_xmmm128,								// EVEX.512.66.0F38.WIG 34 /r

		EVEX_Vpmovqw_xmmm32_k1z_xmm,								// EVEX.128.F3.0F38.W0 34 /r
		EVEX_Vpmovqw_xmmm64_k1z_ymm,								// EVEX.256.F3.0F38.W0 34 /r
		EVEX_Vpmovqw_xmmm128_k1z_zmm,								// EVEX.512.F3.0F38.W0 34 /r

		Pmovzxdq_xmm_xmmm64,										// 66 0F 38 35 /r
		VEX_Vpmovzxdq_xmm_xmmm64,									// VEX.128.66.0F38.WIG 35 /r
		VEX_Vpmovzxdq_ymm_xmmm128,									// VEX.256.66.0F38.WIG 35 /r
		EVEX_Vpmovzxdq_xmm_k1z_xmmm64,								// EVEX.128.66.0F38.W0 35 /r
		EVEX_Vpmovzxdq_ymm_k1z_xmmm128,								// EVEX.256.66.0F38.W0 35 /r
		EVEX_Vpmovzxdq_zmm_k1z_ymmm256,								// EVEX.512.66.0F38.W0 35 /r

		EVEX_Vpmovqd_xmmm64_k1z_xmm,								// EVEX.128.F3.0F38.W0 35 /r
		EVEX_Vpmovqd_xmmm128_k1z_ymm,								// EVEX.256.F3.0F38.W0 35 /r
		EVEX_Vpmovqd_ymmm256_k1z_zmm,								// EVEX.512.F3.0F38.W0 35 /r

		VEX_Vpermd_ymm_ymm_ymmm256,									// VEX.256.66.0F38.W0 36 /r
		EVEX_Vpermd_ymm_k1z_ymm_ymmm256b32,							// EVEX.256.66.0F38.W0 36 /r
		EVEX_Vpermd_zmm_k1z_zmm_zmmm512b32,							// EVEX.512.66.0F38.W0 36 /r
		EVEX_Vpermq_ymm_k1z_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 36 /r
		EVEX_Vpermq_zmm_k1z_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 36 /r

		Pcmpgtq_xmm_xmmm128,										// 66 0F 38 37 /r
		VEX_Vpcmpgtq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 37 /r
		VEX_Vpcmpgtq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 37 /r
		EVEX_Vpcmpgtq_k_k1_xmm_xmmm128b64,							// EVEX.128.66.0F38.W1 37 /r
		EVEX_Vpcmpgtq_k_k1_ymm_ymmm256b64,							// EVEX.256.66.0F38.W1 37 /r
		EVEX_Vpcmpgtq_k_k1_zmm_zmmm512b64,							// EVEX.512.66.0F38.W1 37 /r

		Pminsb_xmm_xmmm128,											// 66 0F 38 38 /r
		VEX_Vpminsb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 38 /r
		VEX_Vpminsb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 38 /r
		EVEX_Vpminsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 38 /r
		EVEX_Vpminsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 38 /r
		EVEX_Vpminsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 38 /r

		EVEX_Vpmovm2d_xmm_k,										// EVEX.128.F3.0F38.W0 38 /r
		EVEX_Vpmovm2d_ymm_k,										// EVEX.256.F3.0F38.W0 38 /r
		EVEX_Vpmovm2d_zmm_k,										// EVEX.512.F3.0F38.W0 38 /r
		EVEX_Vpmovm2q_xmm_k,										// EVEX.128.F3.0F38.W1 38 /r
		EVEX_Vpmovm2q_ymm_k,										// EVEX.256.F3.0F38.W1 38 /r
		EVEX_Vpmovm2q_zmm_k,										// EVEX.512.F3.0F38.W1 38 /r

		Pminsd_xmm_xmmm128,											// 66 0F 38 39 /r
		VEX_Vpminsd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 39 /r
		VEX_Vpminsd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 39 /r
		EVEX_Vpminsd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 39 /r
		EVEX_Vpminsd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 39 /r
		EVEX_Vpminsd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 39 /r
		EVEX_Vpminsq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 39 /r
		EVEX_Vpminsq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 39 /r
		EVEX_Vpminsq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 39 /r

		EVEX_Vpmovd2m_k_xmm,										// EVEX.128.F3.0F38.W0 39 /r
		EVEX_Vpmovd2m_k_ymm,										// EVEX.256.F3.0F38.W0 39 /r
		EVEX_Vpmovd2m_k_zmm,										// EVEX.512.F3.0F38.W0 39 /r
		EVEX_Vpmovq2m_k_xmm,										// EVEX.128.F3.0F38.W1 39 /r
		EVEX_Vpmovq2m_k_ymm,										// EVEX.256.F3.0F38.W1 39 /r
		EVEX_Vpmovq2m_k_zmm,										// EVEX.512.F3.0F38.W1 39 /r

		Pminuw_xmm_xmmm128,											// 66 0F 38 3A /r
		VEX_Vpminuw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3A /r
		VEX_Vpminuw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3A /r
		EVEX_Vpminuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3A /r
		EVEX_Vpminuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3A /r
		EVEX_Vpminuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3A /r

		EVEX_Vpbroadcastmw2d_xmm_k,									// EVEX.128.F3.0F38.W0 3A /r
		EVEX_Vpbroadcastmw2d_ymm_k,									// EVEX.256.F3.0F38.W0 3A /r
		EVEX_Vpbroadcastmw2d_zmm_k,									// EVEX.512.F3.0F38.W0 3A /r

		Pminud_xmm_xmmm128,											// 66 0F 38 3B /r
		VEX_Vpminud_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3B /r
		VEX_Vpminud_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3B /r
		EVEX_Vpminud_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3B /r
		EVEX_Vpminud_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3B /r
		EVEX_Vpminud_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3B /r
		EVEX_Vpminuq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3B /r
		EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3B /r
		EVEX_Vpminuq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3B /r

		Pmaxsb_xmm_xmmm128,											// 66 0F 38 3C /r
		VEX_Vpmaxsb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3C /r
		VEX_Vpmaxsb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3C /r
		EVEX_Vpmaxsb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3C /r
		EVEX_Vpmaxsb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3C /r
		EVEX_Vpmaxsb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3C /r

		Pmaxsd_xmm_xmmm128,											// 66 0F 38 3D /r
		VEX_Vpmaxsd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3D /r
		VEX_Vpmaxsd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3D /r
		EVEX_Vpmaxsd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3D /r
		EVEX_Vpmaxsd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3D /r
		EVEX_Vpmaxsd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3D /r
		EVEX_Vpmaxsq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3D /r
		EVEX_Vpmaxsq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3D /r
		EVEX_Vpmaxsq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3D /r

		Pmaxuw_xmm_xmmm128,											// 66 0F 38 3E /r
		VEX_Vpmaxuw_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3E /r
		VEX_Vpmaxuw_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3E /r
		EVEX_Vpmaxuw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.WIG 3E /r
		EVEX_Vpmaxuw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.WIG 3E /r
		EVEX_Vpmaxuw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.WIG 3E /r

		Pmaxud_xmm_xmmm128,											// 66 0F 38 3F /r
		VEX_Vpmaxud_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 3F /r
		VEX_Vpmaxud_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 3F /r
		EVEX_Vpmaxud_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 3F /r
		EVEX_Vpmaxud_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 3F /r
		EVEX_Vpmaxud_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 3F /r
		EVEX_Vpmaxuq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 3F /r
		EVEX_Vpmaxuq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 3F /r
		EVEX_Vpmaxuq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 3F /r

		Pmulld_xmm_xmmm128,											// 66 0F 38 40 /r
		VEX_Vpmulld_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG 40 /r
		VEX_Vpmulld_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG 40 /r
		EVEX_Vpmulld_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 40 /r
		EVEX_Vpmulld_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 40 /r
		EVEX_Vpmulld_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 40 /r
		EVEX_Vpmullq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 40 /r
		EVEX_Vpmullq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 40 /r
		EVEX_Vpmullq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 40 /r

		Phminposuw_xmm_xmmm128,										// 66 0F 38 41 /r
		VEX_Vphminposuw_xmm_xmmm128,								// VEX.128.66.0F38.WIG 41 /r

		EVEX_Vgetexpps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 42 /r
		EVEX_Vgetexpps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 42 /r
		EVEX_Vgetexpps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 42 /r
		EVEX_Vgetexppd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 42 /r
		EVEX_Vgetexppd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 42 /r
		EVEX_Vgetexppd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 42 /r

		EVEX_Vgetexpss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 43 /r
		EVEX_Vgetexpsd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 43 /r

		EVEX_Vplzcntd_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 44 /r
		EVEX_Vplzcntd_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 44 /r
		EVEX_Vplzcntd_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 44 /r
		EVEX_Vplzcntq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 44 /r
		EVEX_Vplzcntq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 44 /r
		EVEX_Vplzcntq_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 44 /r

		VEX_Vpsrlvd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 45 /r
		VEX_Vpsrlvd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 45 /r
		VEX_Vpsrlvq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W1 45 /r
		VEX_Vpsrlvq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W1 45 /r
		EVEX_Vpsrlvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 45 /r
		EVEX_Vpsrlvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 45 /r
		EVEX_Vpsrlvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 45 /r
		EVEX_Vpsrlvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 45 /r
		EVEX_Vpsrlvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 45 /r
		EVEX_Vpsrlvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 45 /r

		VEX_Vpsravd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 46 /r
		VEX_Vpsravd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 46 /r
		EVEX_Vpsravd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 46 /r
		EVEX_Vpsravd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 46 /r
		EVEX_Vpsravd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 46 /r
		EVEX_Vpsravq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 46 /r
		EVEX_Vpsravq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 46 /r
		EVEX_Vpsravq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 46 /r

		VEX_Vpsllvd_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 47 /r
		VEX_Vpsllvd_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 47 /r
		VEX_Vpsllvq_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W1 47 /r
		VEX_Vpsllvq_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W1 47 /r
		EVEX_Vpsllvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 47 /r
		EVEX_Vpsllvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 47 /r
		EVEX_Vpsllvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 47 /r
		EVEX_Vpsllvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 47 /r
		EVEX_Vpsllvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 47 /r
		EVEX_Vpsllvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 47 /r

		EVEX_Vrcp14ps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 4C /r
		EVEX_Vrcp14ps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 4C /r
		EVEX_Vrcp14ps_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 4C /r
		EVEX_Vrcp14pd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 4C /r
		EVEX_Vrcp14pd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 4C /r
		EVEX_Vrcp14pd_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 4C /r

		EVEX_Vrcp14ss_xmm_k1z_xmm_xmmm32,							// EVEX.LIG.66.0F38.W0 4D /r
		EVEX_Vrcp14sd_xmm_k1z_xmm_xmmm64,							// EVEX.LIG.66.0F38.W1 4D /r

		EVEX_Vrsqrt14ps_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 4E /r
		EVEX_Vrsqrt14ps_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 4E /r
		EVEX_Vrsqrt14ps_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 4E /r
		EVEX_Vrsqrt14pd_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 4E /r
		EVEX_Vrsqrt14pd_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 4E /r
		EVEX_Vrsqrt14pd_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 4E /r

		EVEX_Vrsqrt14ss_xmm_k1z_xmm_xmmm32,							// EVEX.LIG.66.0F38.W0 4F /r
		EVEX_Vrsqrt14sd_xmm_k1z_xmm_xmmm64,							// EVEX.LIG.66.0F38.W1 4F /r

		EVEX_Vpdpbusd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 50 /r
		EVEX_Vpdpbusd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 50 /r
		EVEX_Vpdpbusd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 50 /r

		EVEX_Vpdpbusds_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 51 /r
		EVEX_Vpdpbusds_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 51 /r
		EVEX_Vpdpbusds_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 51 /r

		EVEX_Vpdpwssd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 52 /r
		EVEX_Vpdpwssd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 52 /r
		EVEX_Vpdpwssd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 52 /r

		EVEX_Vdpbf16ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.F3.0F38.W0 52 /r
		EVEX_Vdpbf16ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.F3.0F38.W0 52 /r
		EVEX_Vdpbf16ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.F3.0F38.W0 52 /r

		EVEX_Vp4dpwssd_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 52 /r

		EVEX_Vpdpwssds_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 53 /r
		EVEX_Vpdpwssds_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 53 /r
		EVEX_Vpdpwssds_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 53 /r

		EVEX_Vp4dpwssds_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 53 /r

		EVEX_Vpopcntb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 54 /r
		EVEX_Vpopcntb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 54 /r
		EVEX_Vpopcntb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 54 /r
		EVEX_Vpopcntw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 54 /r
		EVEX_Vpopcntw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 54 /r
		EVEX_Vpopcntw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 54 /r

		EVEX_Vpopcntd_xmm_k1z_xmmm128b32,							// EVEX.128.66.0F38.W0 55 /r
		EVEX_Vpopcntd_ymm_k1z_ymmm256b32,							// EVEX.256.66.0F38.W0 55 /r
		EVEX_Vpopcntd_zmm_k1z_zmmm512b32,							// EVEX.512.66.0F38.W0 55 /r
		EVEX_Vpopcntq_xmm_k1z_xmmm128b64,							// EVEX.128.66.0F38.W1 55 /r
		EVEX_Vpopcntq_ymm_k1z_ymmm256b64,							// EVEX.256.66.0F38.W1 55 /r
		EVEX_Vpopcntq_zmm_k1z_zmmm512b64,							// EVEX.512.66.0F38.W1 55 /r

		VEX_Vpbroadcastd_xmm_xmmm32,								// VEX.128.66.0F38.W0 58 /r
		VEX_Vpbroadcastd_ymm_xmmm32,								// VEX.256.66.0F38.W0 58 /r
		EVEX_Vpbroadcastd_xmm_k1z_xmmm32,							// EVEX.128.66.0F38.W0 58 /r
		EVEX_Vpbroadcastd_ymm_k1z_xmmm32,							// EVEX.256.66.0F38.W0 58 /r
		EVEX_Vpbroadcastd_zmm_k1z_xmmm32,							// EVEX.512.66.0F38.W0 58 /r

		VEX_Vpbroadcastq_xmm_xmmm64,								// VEX.128.66.0F38.W0 59 /r
		VEX_Vpbroadcastq_ymm_xmmm64,								// VEX.256.66.0F38.W0 59 /r
		EVEX_Vbroadcasti32x2_xmm_k1z_xmmm64,						// EVEX.128.66.0F38.W0 59 /r
		EVEX_Vbroadcasti32x2_ymm_k1z_xmmm64,						// EVEX.256.66.0F38.W0 59 /r
		EVEX_Vbroadcasti32x2_zmm_k1z_xmmm64,						// EVEX.512.66.0F38.W0 59 /r
		EVEX_Vpbroadcastq_xmm_k1z_xmmm64,							// EVEX.128.66.0F38.W1 59 /r
		EVEX_Vpbroadcastq_ymm_k1z_xmmm64,							// EVEX.256.66.0F38.W1 59 /r
		EVEX_Vpbroadcastq_zmm_k1z_xmmm64,							// EVEX.512.66.0F38.W1 59 /r

		VEX_Vbroadcasti128_ymm_m128,								// VEX.256.66.0F38.W0 5A /r
		EVEX_Vbroadcasti32x4_ymm_k1z_m128,							// EVEX.256.66.0F38.W0 5A /r
		EVEX_Vbroadcasti32x4_zmm_k1z_m128,							// EVEX.512.66.0F38.W0 5A /r
		EVEX_Vbroadcasti64x2_ymm_k1z_m128,							// EVEX.256.66.0F38.W1 5A /r
		EVEX_Vbroadcasti64x2_zmm_k1z_m128,							// EVEX.512.66.0F38.W1 5A /r

		EVEX_Vbroadcasti32x8_zmm_k1z_m256,							// EVEX.512.66.0F38.W0 5B /r
		EVEX_Vbroadcasti64x4_zmm_k1z_m256,							// EVEX.512.66.0F38.W1 5B /r

		EVEX_Vpexpandb_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 62 /r
		EVEX_Vpexpandb_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 62 /r
		EVEX_Vpexpandb_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 62 /r
		EVEX_Vpexpandw_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 62 /r
		EVEX_Vpexpandw_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 62 /r
		EVEX_Vpexpandw_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 62 /r

		EVEX_Vpcompressb_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 63 /r
		EVEX_Vpcompressb_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 63 /r
		EVEX_Vpcompressb_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 63 /r
		EVEX_Vpcompressw_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 63 /r
		EVEX_Vpcompressw_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 63 /r
		EVEX_Vpcompressw_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 63 /r

		EVEX_Vpblendmd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 64 /r
		EVEX_Vpblendmd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 64 /r
		EVEX_Vpblendmd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 64 /r
		EVEX_Vpblendmq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 64 /r
		EVEX_Vpblendmq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 64 /r
		EVEX_Vpblendmq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 64 /r

		EVEX_Vblendmps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 65 /r
		EVEX_Vblendmps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 65 /r
		EVEX_Vblendmps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 65 /r
		EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 65 /r
		EVEX_Vblendmpd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 65 /r
		EVEX_Vblendmpd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 65 /r

		EVEX_Vpblendmb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 66 /r
		EVEX_Vpblendmb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 66 /r
		EVEX_Vpblendmb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 66 /r
		EVEX_Vpblendmw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 66 /r
		EVEX_Vpblendmw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 66 /r
		EVEX_Vpblendmw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 66 /r

		EVEX_Vp2intersectd_kp1_xmm_xmmm128b32,						// EVEX.128.F2.0F38.W0 68 /r
		EVEX_Vp2intersectd_kp1_ymm_ymmm256b32,						// EVEX.256.F2.0F38.W0 68 /r
		EVEX_Vp2intersectd_kp1_zmm_zmmm512b32,						// EVEX.512.F2.0F38.W0 68 /r
		EVEX_Vp2intersectq_kp1_xmm_xmmm128b64,						// EVEX.128.F2.0F38.W1 68 /r
		EVEX_Vp2intersectq_kp1_ymm_ymmm256b64,						// EVEX.256.F2.0F38.W1 68 /r
		EVEX_Vp2intersectq_kp1_zmm_zmmm512b64,						// EVEX.512.F2.0F38.W1 68 /r

		EVEX_Vpshldvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 70 /r
		EVEX_Vpshldvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 70 /r
		EVEX_Vpshldvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 70 /r

		EVEX_Vpshldvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 71 /r
		EVEX_Vpshldvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 71 /r
		EVEX_Vpshldvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 71 /r
		EVEX_Vpshldvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 71 /r
		EVEX_Vpshldvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 71 /r
		EVEX_Vpshldvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 71 /r

		EVEX_Vpshrdvw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 72 /r
		EVEX_Vpshrdvw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 72 /r
		EVEX_Vpshrdvw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 72 /r

		EVEX_Vcvtneps2bf16_xmm_k1z_xmmm128b32,						// EVEX.128.F3.0F38.W0 72 /r
		EVEX_Vcvtneps2bf16_xmm_k1z_ymmm256b32,						// EVEX.256.F3.0F38.W0 72 /r
		EVEX_Vcvtneps2bf16_ymm_k1z_zmmm512b32,						// EVEX.512.F3.0F38.W0 72 /r

		EVEX_Vcvtne2ps2bf16_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.F2.0F38.W0 72 /r
		EVEX_Vcvtne2ps2bf16_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.F2.0F38.W0 72 /r
		EVEX_Vcvtne2ps2bf16_zmm_k1z_zmm_zmmm512b32,					// EVEX.512.F2.0F38.W0 72 /r

		EVEX_Vpshrdvd_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 73 /r
		EVEX_Vpshrdvd_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 73 /r
		EVEX_Vpshrdvd_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 73 /r
		EVEX_Vpshrdvq_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 73 /r
		EVEX_Vpshrdvq_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 73 /r
		EVEX_Vpshrdvq_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 73 /r

		EVEX_Vpermi2b_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 75 /r
		EVEX_Vpermi2b_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 75 /r
		EVEX_Vpermi2b_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 75 /r
		EVEX_Vpermi2w_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 75 /r
		EVEX_Vpermi2w_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 75 /r
		EVEX_Vpermi2w_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 75 /r

		EVEX_Vpermi2d_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 76 /r
		EVEX_Vpermi2d_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 76 /r
		EVEX_Vpermi2d_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 76 /r
		EVEX_Vpermi2q_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 76 /r
		EVEX_Vpermi2q_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 76 /r
		EVEX_Vpermi2q_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 76 /r

		EVEX_Vpermi2ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 77 /r
		EVEX_Vpermi2ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 77 /r
		EVEX_Vpermi2ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 77 /r
		EVEX_Vpermi2pd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 77 /r
		EVEX_Vpermi2pd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 77 /r
		EVEX_Vpermi2pd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 77 /r

		VEX_Vpbroadcastb_xmm_xmmm8,									// VEX.128.66.0F38.W0 78 /r
		VEX_Vpbroadcastb_ymm_xmmm8,									// VEX.256.66.0F38.W0 78 /r
		EVEX_Vpbroadcastb_xmm_k1z_xmmm8,							// EVEX.128.66.0F38.W0 78 /r
		EVEX_Vpbroadcastb_ymm_k1z_xmmm8,							// EVEX.256.66.0F38.W0 78 /r
		EVEX_Vpbroadcastb_zmm_k1z_xmmm8,							// EVEX.512.66.0F38.W0 78 /r

		VEX_Vpbroadcastw_xmm_xmmm16,								// VEX.128.66.0F38.W0 79 /r
		VEX_Vpbroadcastw_ymm_xmmm16,								// VEX.256.66.0F38.W0 79 /r
		EVEX_Vpbroadcastw_xmm_k1z_xmmm16,							// EVEX.128.66.0F38.W0 79 /r
		EVEX_Vpbroadcastw_ymm_k1z_xmmm16,							// EVEX.256.66.0F38.W0 79 /r
		EVEX_Vpbroadcastw_zmm_k1z_xmmm16,							// EVEX.512.66.0F38.W0 79 /r

		EVEX_Vpbroadcastb_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7A /r
		EVEX_Vpbroadcastb_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7A /r
		EVEX_Vpbroadcastb_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7A /r

		EVEX_Vpbroadcastw_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7B /r
		EVEX_Vpbroadcastw_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7B /r
		EVEX_Vpbroadcastw_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7B /r

		EVEX_Vpbroadcastd_xmm_k1z_r32,								// EVEX.128.66.0F38.W0 7C /r
		EVEX_Vpbroadcastd_ymm_k1z_r32,								// EVEX.256.66.0F38.W0 7C /r
		EVEX_Vpbroadcastd_zmm_k1z_r32,								// EVEX.512.66.0F38.W0 7C /r
		EVEX_Vpbroadcastq_xmm_k1z_r64,								// EVEX.128.66.0F38.W1 7C /r
		EVEX_Vpbroadcastq_ymm_k1z_r64,								// EVEX.256.66.0F38.W1 7C /r
		EVEX_Vpbroadcastq_zmm_k1z_r64,								// EVEX.512.66.0F38.W1 7C /r

		EVEX_Vpermt2b_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 7D /r
		EVEX_Vpermt2b_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 7D /r
		EVEX_Vpermt2b_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 7D /r
		EVEX_Vpermt2w_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 7D /r
		EVEX_Vpermt2w_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 7D /r
		EVEX_Vpermt2w_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 7D /r

		EVEX_Vpermt2d_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 7E /r
		EVEX_Vpermt2d_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 7E /r
		EVEX_Vpermt2d_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 7E /r
		EVEX_Vpermt2q_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 7E /r
		EVEX_Vpermt2q_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 7E /r
		EVEX_Vpermt2q_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 7E /r

		EVEX_Vpermt2ps_xmm_k1z_xmm_xmmm128b32,						// EVEX.128.66.0F38.W0 7F /r
		EVEX_Vpermt2ps_ymm_k1z_ymm_ymmm256b32,						// EVEX.256.66.0F38.W0 7F /r
		EVEX_Vpermt2ps_zmm_k1z_zmm_zmmm512b32,						// EVEX.512.66.0F38.W0 7F /r
		EVEX_Vpermt2pd_xmm_k1z_xmm_xmmm128b64,						// EVEX.128.66.0F38.W1 7F /r
		EVEX_Vpermt2pd_ymm_k1z_ymm_ymmm256b64,						// EVEX.256.66.0F38.W1 7F /r
		EVEX_Vpermt2pd_zmm_k1z_zmm_zmmm512b64,						// EVEX.512.66.0F38.W1 7F /r

		Invept_r32_m128,											// 66 0F 38 80 /r
		Invept_r64_m128,											// 66 0F 38 80 /r

		Invvpid_r32_m128,											// 66 0F 38 81 /r
		Invvpid_r64_m128,											// 66 0F 38 81 /r

		Invpcid_r32_m128,											// 66 0F 38 82 /r
		Invpcid_r64_m128,											// 66 0F 38 82 /r

		EVEX_Vpmultishiftqb_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 83 /r
		EVEX_Vpmultishiftqb_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 83 /r
		EVEX_Vpmultishiftqb_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 83 /r

		EVEX_Vexpandps_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 88 /r
		EVEX_Vexpandps_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 88 /r
		EVEX_Vexpandps_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 88 /r
		EVEX_Vexpandpd_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 88 /r
		EVEX_Vexpandpd_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 88 /r
		EVEX_Vexpandpd_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 88 /r

		EVEX_Vpexpandd_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W0 89 /r
		EVEX_Vpexpandd_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W0 89 /r
		EVEX_Vpexpandd_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W0 89 /r
		EVEX_Vpexpandq_xmm_k1z_xmmm128,								// EVEX.128.66.0F38.W1 89 /r
		EVEX_Vpexpandq_ymm_k1z_ymmm256,								// EVEX.256.66.0F38.W1 89 /r
		EVEX_Vpexpandq_zmm_k1z_zmmm512,								// EVEX.512.66.0F38.W1 89 /r

		EVEX_Vcompressps_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 8A /r
		EVEX_Vcompressps_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 8A /r
		EVEX_Vcompressps_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 8A /r
		EVEX_Vcompresspd_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 8A /r
		EVEX_Vcompresspd_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 8A /r
		EVEX_Vcompresspd_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 8A /r

		EVEX_Vpcompressd_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W0 8B /r
		EVEX_Vpcompressd_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W0 8B /r
		EVEX_Vpcompressd_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W0 8B /r
		EVEX_Vpcompressq_xmmm128_k1z_xmm,							// EVEX.128.66.0F38.W1 8B /r
		EVEX_Vpcompressq_ymmm256_k1z_ymm,							// EVEX.256.66.0F38.W1 8B /r
		EVEX_Vpcompressq_zmmm512_k1z_zmm,							// EVEX.512.66.0F38.W1 8B /r

		VEX_Vpmaskmovd_xmm_xmm_m128,								// VEX.128.66.0F38.W0 8C /r
		VEX_Vpmaskmovd_ymm_ymm_m256,								// VEX.256.66.0F38.W0 8C /r
		VEX_Vpmaskmovq_xmm_xmm_m128,								// VEX.128.66.0F38.W1 8C /r
		VEX_Vpmaskmovq_ymm_ymm_m256,								// VEX.256.66.0F38.W1 8C /r

		EVEX_Vpermb_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W0 8D /r
		EVEX_Vpermb_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W0 8D /r
		EVEX_Vpermb_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W0 8D /r
		EVEX_Vpermw_xmm_k1z_xmm_xmmm128,							// EVEX.128.66.0F38.W1 8D /r
		EVEX_Vpermw_ymm_k1z_ymm_ymmm256,							// EVEX.256.66.0F38.W1 8D /r
		EVEX_Vpermw_zmm_k1z_zmm_zmmm512,							// EVEX.512.66.0F38.W1 8D /r

		VEX_Vpmaskmovd_m128_xmm_xmm,								// VEX.128.66.0F38.W0 8E /r
		VEX_Vpmaskmovd_m256_ymm_ymm,								// VEX.256.66.0F38.W0 8E /r
		VEX_Vpmaskmovq_m128_xmm_xmm,								// VEX.128.66.0F38.W1 8E /r
		VEX_Vpmaskmovq_m256_ymm_ymm,								// VEX.256.66.0F38.W1 8E /r

		EVEX_Vpshufbitqmb_k_k1_xmm_xmmm128,							// EVEX.128.66.0F38.W0 8F /r
		EVEX_Vpshufbitqmb_k_k1_ymm_ymmm256,							// EVEX.256.66.0F38.W0 8F /r
		EVEX_Vpshufbitqmb_k_k1_zmm_zmmm512,							// EVEX.512.66.0F38.W0 8F /r

		VEX_Vpgatherdd_xmm_vm32x_xmm,								// VEX.128.66.0F38.W0 90 /r
		VEX_Vpgatherdd_ymm_vm32y_ymm,								// VEX.256.66.0F38.W0 90 /r
		VEX_Vpgatherdq_xmm_vm32x_xmm,								// VEX.128.66.0F38.W1 90 /r
		VEX_Vpgatherdq_ymm_vm32x_ymm,								// VEX.256.66.0F38.W1 90 /r
		EVEX_Vpgatherdd_xmm_k1_vm32x,								// EVEX.128.66.0F38.W0 90 /vsib
		EVEX_Vpgatherdd_ymm_k1_vm32y,								// EVEX.256.66.0F38.W0 90 /vsib
		EVEX_Vpgatherdd_zmm_k1_vm32z,								// EVEX.512.66.0F38.W0 90 /vsib
		EVEX_Vpgatherdq_xmm_k1_vm32x,								// EVEX.128.66.0F38.W1 90 /vsib
		EVEX_Vpgatherdq_ymm_k1_vm32x,								// EVEX.256.66.0F38.W1 90 /vsib
		EVEX_Vpgatherdq_zmm_k1_vm32y,								// EVEX.512.66.0F38.W1 90 /vsib

		VEX_Vpgatherqd_xmm_vm64x_xmm,								// VEX.128.66.0F38.W0 91 /r
		VEX_Vpgatherqd_xmm_vm64y_xmm,								// VEX.256.66.0F38.W0 91 /r
		VEX_Vpgatherqq_xmm_vm64x_xmm,								// VEX.128.66.0F38.W1 91 /r
		VEX_Vpgatherqq_ymm_vm64y_ymm,								// VEX.256.66.0F38.W1 91 /r
		EVEX_Vpgatherqd_xmm_k1_vm64x,								// EVEX.128.66.0F38.W0 91 /vsib
		EVEX_Vpgatherqd_xmm_k1_vm64y,								// EVEX.256.66.0F38.W0 91 /vsib
		EVEX_Vpgatherqd_ymm_k1_vm64z,								// EVEX.512.66.0F38.W0 91 /vsib
		EVEX_Vpgatherqq_xmm_k1_vm64x,								// EVEX.128.66.0F38.W1 91 /vsib
		EVEX_Vpgatherqq_ymm_k1_vm64y,								// EVEX.256.66.0F38.W1 91 /vsib
		EVEX_Vpgatherqq_zmm_k1_vm64z,								// EVEX.512.66.0F38.W1 91 /vsib

		VEX_Vgatherdps_xmm_vm32x_xmm,								// VEX.128.66.0F38.W0 92 /r
		VEX_Vgatherdps_ymm_vm32y_ymm,								// VEX.256.66.0F38.W0 92 /r
		VEX_Vgatherdpd_xmm_vm32x_xmm,								// VEX.128.66.0F38.W1 92 /r
		VEX_Vgatherdpd_ymm_vm32x_ymm,								// VEX.256.66.0F38.W1 92 /r
		EVEX_Vgatherdps_xmm_k1_vm32x,								// EVEX.128.66.0F38.W0 92 /vsib
		EVEX_Vgatherdps_ymm_k1_vm32y,								// EVEX.256.66.0F38.W0 92 /vsib
		EVEX_Vgatherdps_zmm_k1_vm32z,								// EVEX.512.66.0F38.W0 92 /vsib
		EVEX_Vgatherdpd_xmm_k1_vm32x,								// EVEX.128.66.0F38.W1 92 /vsib
		EVEX_Vgatherdpd_ymm_k1_vm32x,								// EVEX.256.66.0F38.W1 92 /vsib
		EVEX_Vgatherdpd_zmm_k1_vm32y,								// EVEX.512.66.0F38.W1 92 /vsib

		VEX_Vgatherqps_xmm_vm64x_xmm,								// VEX.128.66.0F38.W0 93 /r
		VEX_Vgatherqps_xmm_vm64y_xmm,								// VEX.256.66.0F38.W0 93 /r
		VEX_Vgatherqpd_xmm_vm64x_xmm,								// VEX.128.66.0F38.W1 93 /r
		VEX_Vgatherqpd_ymm_vm64y_ymm,								// VEX.256.66.0F38.W1 93 /r
		EVEX_Vgatherqps_xmm_k1_vm64x,								// EVEX.128.66.0F38.W0 93 /vsib
		EVEX_Vgatherqps_xmm_k1_vm64y,								// EVEX.256.66.0F38.W0 93 /vsib
		EVEX_Vgatherqps_ymm_k1_vm64z,								// EVEX.512.66.0F38.W0 93 /vsib
		EVEX_Vgatherqpd_xmm_k1_vm64x,								// EVEX.128.66.0F38.W1 93 /vsib
		EVEX_Vgatherqpd_ymm_k1_vm64y,								// EVEX.256.66.0F38.W1 93 /vsib
		EVEX_Vgatherqpd_zmm_k1_vm64z,								// EVEX.512.66.0F38.W1 93 /vsib

		VEX_Vfmaddsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 96 /r
		VEX_Vfmaddsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 96 /r
		VEX_Vfmaddsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 96 /r
		VEX_Vfmaddsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 96 /r
		EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 96 /r
		EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 96 /r
		EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 96 /r
		EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 96 /r
		EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 96 /r
		EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 96 /r

		VEX_Vfmsubadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 97 /r
		VEX_Vfmsubadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 97 /r
		VEX_Vfmsubadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 97 /r
		VEX_Vfmsubadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 97 /r
		EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 97 /r
		EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 97 /r
		EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 97 /r
		EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 97 /r
		EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 97 /r
		EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 97 /r

		VEX_Vfmadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 98 /r
		VEX_Vfmadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 98 /r
		VEX_Vfmadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 98 /r
		VEX_Vfmadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 98 /r
		EVEX_Vfmadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 98 /r
		EVEX_Vfmadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 98 /r
		EVEX_Vfmadd132ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 98 /r
		EVEX_Vfmadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 98 /r
		EVEX_Vfmadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 98 /r
		EVEX_Vfmadd132pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 98 /r

		VEX_Vfmadd132ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 99 /r
		VEX_Vfmadd132sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 99 /r
		EVEX_Vfmadd132ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 99 /r
		EVEX_Vfmadd132sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 99 /r

		VEX_Vfmsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9A /r
		VEX_Vfmsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9A /r
		VEX_Vfmsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9A /r
		VEX_Vfmsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9A /r
		EVEX_Vfmsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9A /r
		EVEX_Vfmsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9A /r
		EVEX_Vfmsub132ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 9A /r
		EVEX_Vfmsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9A /r
		EVEX_Vfmsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9A /r
		EVEX_Vfmsub132pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 9A /r

		EVEX_V4fmaddps_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 9A /r

		VEX_Vfmsub132ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 9B /r
		VEX_Vfmsub132sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 9B /r
		EVEX_Vfmsub132ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 9B /r
		EVEX_Vfmsub132sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 9B /r

		EVEX_V4fmaddss_xmm_k1z_xmmp3_m128,							// EVEX.LIG.F2.0F38.W0 9B /r

		VEX_Vfnmadd132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9C /r
		VEX_Vfnmadd132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9C /r
		VEX_Vfnmadd132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9C /r
		VEX_Vfnmadd132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9C /r
		EVEX_Vfnmadd132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9C /r
		EVEX_Vfnmadd132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9C /r
		EVEX_Vfnmadd132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 9C /r
		EVEX_Vfnmadd132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9C /r
		EVEX_Vfnmadd132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9C /r
		EVEX_Vfnmadd132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 9C /r

		VEX_Vfnmadd132ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 9D /r
		VEX_Vfnmadd132sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 9D /r
		EVEX_Vfnmadd132ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 9D /r
		EVEX_Vfnmadd132sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 9D /r

		VEX_Vfnmsub132ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 9E /r
		VEX_Vfnmsub132ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 9E /r
		VEX_Vfnmsub132pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 9E /r
		VEX_Vfnmsub132pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 9E /r
		EVEX_Vfnmsub132ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 9E /r
		EVEX_Vfnmsub132ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 9E /r
		EVEX_Vfnmsub132ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 9E /r
		EVEX_Vfnmsub132pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 9E /r
		EVEX_Vfnmsub132pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 9E /r
		EVEX_Vfnmsub132pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 9E /r

		VEX_Vfnmsub132ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 9F /r
		VEX_Vfnmsub132sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 9F /r
		EVEX_Vfnmsub132ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 9F /r
		EVEX_Vfnmsub132sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 9F /r

		EVEX_Vpscatterdd_vm32x_k1_xmm,								// EVEX.128.66.0F38.W0 A0 /vsib
		EVEX_Vpscatterdd_vm32y_k1_ymm,								// EVEX.256.66.0F38.W0 A0 /vsib
		EVEX_Vpscatterdd_vm32z_k1_zmm,								// EVEX.512.66.0F38.W0 A0 /vsib
		EVEX_Vpscatterdq_vm32x_k1_xmm,								// EVEX.128.66.0F38.W1 A0 /vsib
		EVEX_Vpscatterdq_vm32x_k1_ymm,								// EVEX.256.66.0F38.W1 A0 /vsib
		EVEX_Vpscatterdq_vm32y_k1_zmm,								// EVEX.512.66.0F38.W1 A0 /vsib

		EVEX_Vpscatterqd_vm64x_k1_xmm,								// EVEX.128.66.0F38.W0 A1 /vsib
		EVEX_Vpscatterqd_vm64y_k1_xmm,								// EVEX.256.66.0F38.W0 A1 /vsib
		EVEX_Vpscatterqd_vm64z_k1_ymm,								// EVEX.512.66.0F38.W0 A1 /vsib
		EVEX_Vpscatterqq_vm64x_k1_xmm,								// EVEX.128.66.0F38.W1 A1 /vsib
		EVEX_Vpscatterqq_vm64y_k1_ymm,								// EVEX.256.66.0F38.W1 A1 /vsib
		EVEX_Vpscatterqq_vm64z_k1_zmm,								// EVEX.512.66.0F38.W1 A1 /vsib

		EVEX_Vscatterdps_vm32x_k1_xmm,								// EVEX.128.66.0F38.W0 A2 /vsib
		EVEX_Vscatterdps_vm32y_k1_ymm,								// EVEX.256.66.0F38.W0 A2 /vsib
		EVEX_Vscatterdps_vm32z_k1_zmm,								// EVEX.512.66.0F38.W0 A2 /vsib
		EVEX_Vscatterdpd_vm32x_k1_xmm,								// EVEX.128.66.0F38.W1 A2 /vsib
		EVEX_Vscatterdpd_vm32x_k1_ymm,								// EVEX.256.66.0F38.W1 A2 /vsib
		EVEX_Vscatterdpd_vm32y_k1_zmm,								// EVEX.512.66.0F38.W1 A2 /vsib

		EVEX_Vscatterqps_vm64x_k1_xmm,								// EVEX.128.66.0F38.W0 A3 /vsib
		EVEX_Vscatterqps_vm64y_k1_xmm,								// EVEX.256.66.0F38.W0 A3 /vsib
		EVEX_Vscatterqps_vm64z_k1_ymm,								// EVEX.512.66.0F38.W0 A3 /vsib
		EVEX_Vscatterqpd_vm64x_k1_xmm,								// EVEX.128.66.0F38.W1 A3 /vsib
		EVEX_Vscatterqpd_vm64y_k1_ymm,								// EVEX.256.66.0F38.W1 A3 /vsib
		EVEX_Vscatterqpd_vm64z_k1_zmm,								// EVEX.512.66.0F38.W1 A3 /vsib

		VEX_Vfmaddsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A6 /r
		VEX_Vfmaddsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A6 /r
		VEX_Vfmaddsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A6 /r
		VEX_Vfmaddsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A6 /r
		EVEX_Vfmaddsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A6 /r
		EVEX_Vfmaddsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A6 /r
		EVEX_Vfmaddsub213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 A6 /r
		EVEX_Vfmaddsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A6 /r
		EVEX_Vfmaddsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A6 /r
		EVEX_Vfmaddsub213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 A6 /r

		VEX_Vfmsubadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A7 /r
		VEX_Vfmsubadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A7 /r
		VEX_Vfmsubadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A7 /r
		VEX_Vfmsubadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A7 /r
		EVEX_Vfmsubadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A7 /r
		EVEX_Vfmsubadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A7 /r
		EVEX_Vfmsubadd213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 A7 /r
		EVEX_Vfmsubadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A7 /r
		EVEX_Vfmsubadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A7 /r
		EVEX_Vfmsubadd213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 A7 /r

		VEX_Vfmadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 A8 /r
		VEX_Vfmadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 A8 /r
		VEX_Vfmadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 A8 /r
		VEX_Vfmadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 A8 /r
		EVEX_Vfmadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 A8 /r
		EVEX_Vfmadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 A8 /r
		EVEX_Vfmadd213ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 A8 /r
		EVEX_Vfmadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 A8 /r
		EVEX_Vfmadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 A8 /r
		EVEX_Vfmadd213pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 A8 /r

		VEX_Vfmadd213ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 A9 /r
		VEX_Vfmadd213sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 A9 /r
		EVEX_Vfmadd213ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 A9 /r
		EVEX_Vfmadd213sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 A9 /r

		VEX_Vfmsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AA /r
		VEX_Vfmsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AA /r
		VEX_Vfmsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AA /r
		VEX_Vfmsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AA /r
		EVEX_Vfmsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AA /r
		EVEX_Vfmsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AA /r
		EVEX_Vfmsub213ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 AA /r
		EVEX_Vfmsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AA /r
		EVEX_Vfmsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AA /r
		EVEX_Vfmsub213pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 AA /r

		EVEX_V4fnmaddps_zmm_k1z_zmmp3_m128,							// EVEX.512.F2.0F38.W0 AA /r

		VEX_Vfmsub213ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 AB /r
		VEX_Vfmsub213sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 AB /r
		EVEX_Vfmsub213ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 AB /r
		EVEX_Vfmsub213sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 AB /r

		EVEX_V4fnmaddss_xmm_k1z_xmmp3_m128,							// EVEX.LIG.F2.0F38.W0 AB /r

		VEX_Vfnmadd213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AC /r
		VEX_Vfnmadd213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AC /r
		VEX_Vfnmadd213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AC /r
		VEX_Vfnmadd213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AC /r
		EVEX_Vfnmadd213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AC /r
		EVEX_Vfnmadd213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AC /r
		EVEX_Vfnmadd213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 AC /r
		EVEX_Vfnmadd213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AC /r
		EVEX_Vfnmadd213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AC /r
		EVEX_Vfnmadd213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 AC /r

		VEX_Vfnmadd213ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 AD /r
		VEX_Vfnmadd213sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 AD /r
		EVEX_Vfnmadd213ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 AD /r
		EVEX_Vfnmadd213sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 AD /r

		VEX_Vfnmsub213ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 AE /r
		VEX_Vfnmsub213ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 AE /r
		VEX_Vfnmsub213pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 AE /r
		VEX_Vfnmsub213pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 AE /r
		EVEX_Vfnmsub213ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 AE /r
		EVEX_Vfnmsub213ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 AE /r
		EVEX_Vfnmsub213ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 AE /r
		EVEX_Vfnmsub213pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 AE /r
		EVEX_Vfnmsub213pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 AE /r
		EVEX_Vfnmsub213pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 AE /r

		VEX_Vfnmsub213ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 AF /r
		VEX_Vfnmsub213sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 AF /r
		EVEX_Vfnmsub213ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 AF /r
		EVEX_Vfnmsub213sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 AF /r

		EVEX_Vpmadd52luq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B4 /r
		EVEX_Vpmadd52luq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B4 /r
		EVEX_Vpmadd52luq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 B4 /r

		EVEX_Vpmadd52huq_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B5 /r
		EVEX_Vpmadd52huq_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B5 /r
		EVEX_Vpmadd52huq_zmm_k1z_zmm_zmmm512b64,					// EVEX.512.66.0F38.W1 B5 /r

		VEX_Vfmaddsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B6 /r
		VEX_Vfmaddsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B6 /r
		VEX_Vfmaddsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B6 /r
		VEX_Vfmaddsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B6 /r
		EVEX_Vfmaddsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B6 /r
		EVEX_Vfmaddsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B6 /r
		EVEX_Vfmaddsub231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 B6 /r
		EVEX_Vfmaddsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B6 /r
		EVEX_Vfmaddsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B6 /r
		EVEX_Vfmaddsub231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 B6 /r

		VEX_Vfmsubadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B7 /r
		VEX_Vfmsubadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B7 /r
		VEX_Vfmsubadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B7 /r
		VEX_Vfmsubadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B7 /r
		EVEX_Vfmsubadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B7 /r
		EVEX_Vfmsubadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B7 /r
		EVEX_Vfmsubadd231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 B7 /r
		EVEX_Vfmsubadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B7 /r
		EVEX_Vfmsubadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B7 /r
		EVEX_Vfmsubadd231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 B7 /r

		VEX_Vfmadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 B8 /r
		VEX_Vfmadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 B8 /r
		VEX_Vfmadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 B8 /r
		VEX_Vfmadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 B8 /r
		EVEX_Vfmadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 B8 /r
		EVEX_Vfmadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 B8 /r
		EVEX_Vfmadd231ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 B8 /r
		EVEX_Vfmadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 B8 /r
		EVEX_Vfmadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 B8 /r
		EVEX_Vfmadd231pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 B8 /r

		VEX_Vfmadd231ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 B9 /r
		VEX_Vfmadd231sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 B9 /r
		EVEX_Vfmadd231ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 B9 /r
		EVEX_Vfmadd231sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 B9 /r

		VEX_Vfmsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BA /r
		VEX_Vfmsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BA /r
		VEX_Vfmsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BA /r
		VEX_Vfmsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BA /r
		EVEX_Vfmsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BA /r
		EVEX_Vfmsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BA /r
		EVEX_Vfmsub231ps_zmm_k1z_zmm_zmmm512b32_er,					// EVEX.512.66.0F38.W0 BA /r
		EVEX_Vfmsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BA /r
		EVEX_Vfmsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BA /r
		EVEX_Vfmsub231pd_zmm_k1z_zmm_zmmm512b64_er,					// EVEX.512.66.0F38.W1 BA /r

		VEX_Vfmsub231ss_xmm_xmm_xmmm32,								// VEX.LIG.66.0F38.W0 BB /r
		VEX_Vfmsub231sd_xmm_xmm_xmmm64,								// VEX.LIG.66.0F38.W1 BB /r
		EVEX_Vfmsub231ss_xmm_k1z_xmm_xmmm32_er,						// EVEX.LIG.66.0F38.W0 BB /r
		EVEX_Vfmsub231sd_xmm_k1z_xmm_xmmm64_er,						// EVEX.LIG.66.0F38.W1 BB /r

		VEX_Vfnmadd231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BC /r
		VEX_Vfnmadd231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BC /r
		VEX_Vfnmadd231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BC /r
		VEX_Vfnmadd231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BC /r
		EVEX_Vfnmadd231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BC /r
		EVEX_Vfnmadd231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BC /r
		EVEX_Vfnmadd231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 BC /r
		EVEX_Vfnmadd231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BC /r
		EVEX_Vfnmadd231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BC /r
		EVEX_Vfnmadd231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 BC /r

		VEX_Vfnmadd231ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 BD /r
		VEX_Vfnmadd231sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 BD /r
		EVEX_Vfnmadd231ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 BD /r
		EVEX_Vfnmadd231sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 BD /r

		VEX_Vfnmsub231ps_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W0 BE /r
		VEX_Vfnmsub231ps_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W0 BE /r
		VEX_Vfnmsub231pd_xmm_xmm_xmmm128,							// VEX.128.66.0F38.W1 BE /r
		VEX_Vfnmsub231pd_ymm_ymm_ymmm256,							// VEX.256.66.0F38.W1 BE /r
		EVEX_Vfnmsub231ps_xmm_k1z_xmm_xmmm128b32,					// EVEX.128.66.0F38.W0 BE /r
		EVEX_Vfnmsub231ps_ymm_k1z_ymm_ymmm256b32,					// EVEX.256.66.0F38.W0 BE /r
		EVEX_Vfnmsub231ps_zmm_k1z_zmm_zmmm512b32_er,				// EVEX.512.66.0F38.W0 BE /r
		EVEX_Vfnmsub231pd_xmm_k1z_xmm_xmmm128b64,					// EVEX.128.66.0F38.W1 BE /r
		EVEX_Vfnmsub231pd_ymm_k1z_ymm_ymmm256b64,					// EVEX.256.66.0F38.W1 BE /r
		EVEX_Vfnmsub231pd_zmm_k1z_zmm_zmmm512b64_er,				// EVEX.512.66.0F38.W1 BE /r

		VEX_Vfnmsub231ss_xmm_xmm_xmmm32,							// VEX.LIG.66.0F38.W0 BF /r
		VEX_Vfnmsub231sd_xmm_xmm_xmmm64,							// VEX.LIG.66.0F38.W1 BF /r
		EVEX_Vfnmsub231ss_xmm_k1z_xmm_xmmm32_er,					// EVEX.LIG.66.0F38.W0 BF /r
		EVEX_Vfnmsub231sd_xmm_k1z_xmm_xmmm64_er,					// EVEX.LIG.66.0F38.W1 BF /r

		EVEX_Vpconflictd_xmm_k1z_xmmm128b32,						// EVEX.128.66.0F38.W0 C4 /r
		EVEX_Vpconflictd_ymm_k1z_ymmm256b32,						// EVEX.256.66.0F38.W0 C4 /r
		EVEX_Vpconflictd_zmm_k1z_zmmm512b32,						// EVEX.512.66.0F38.W0 C4 /r
		EVEX_Vpconflictq_xmm_k1z_xmmm128b64,						// EVEX.128.66.0F38.W1 C4 /r
		EVEX_Vpconflictq_ymm_k1z_ymmm256b64,						// EVEX.256.66.0F38.W1 C4 /r
		EVEX_Vpconflictq_zmm_k1z_zmmm512b64,						// EVEX.512.66.0F38.W1 C4 /r

		EVEX_Vgatherpf0dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /1 /vsib
		EVEX_Vgatherpf0dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /1 /vsib
		EVEX_Vgatherpf1dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /2 /vsib
		EVEX_Vgatherpf1dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /2 /vsib
		EVEX_Vscatterpf0dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /5 /vsib
		EVEX_Vscatterpf0dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /5 /vsib
		EVEX_Vscatterpf1dps_vm32z_k1,								// EVEX.512.66.0F38.W0 C6 /6 /vsib
		EVEX_Vscatterpf1dpd_vm32y_k1,								// EVEX.512.66.0F38.W1 C6 /6 /vsib

		EVEX_Vgatherpf0qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /1 /vsib
		EVEX_Vgatherpf0qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /1 /vsib
		EVEX_Vgatherpf1qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /2 /vsib
		EVEX_Vgatherpf1qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /2 /vsib
		EVEX_Vscatterpf0qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /5 /vsib
		EVEX_Vscatterpf0qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /5 /vsib
		EVEX_Vscatterpf1qps_vm64z_k1,								// EVEX.512.66.0F38.W0 C7 /6 /vsib
		EVEX_Vscatterpf1qpd_vm64z_k1,								// EVEX.512.66.0F38.W1 C7 /6 /vsib

		Sha1nexte_xmm_xmmm128,										// NP 0F 38 C8 /r

		EVEX_Vexp2ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 C8 /r
		EVEX_Vexp2pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 C8 /r

		Sha1msg1_xmm_xmmm128,										// NP 0F 38 C9 /r

		Sha1msg2_xmm_xmmm128,										// NP 0F 38 CA /r

		EVEX_Vrcp28ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 CA /r
		EVEX_Vrcp28pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 CA /r

		Sha256rnds2_xmm_xmmm128,									// NP 0F 38 CB /r

		EVEX_Vrcp28ss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 CB /r
		EVEX_Vrcp28sd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 CB /r

		Sha256msg1_xmm_xmmm128,										// NP 0F 38 CC /r

		EVEX_Vrsqrt28ps_zmm_k1z_zmmm512b32_sae,						// EVEX.512.66.0F38.W0 CC /r
		EVEX_Vrsqrt28pd_zmm_k1z_zmmm512b64_sae,						// EVEX.512.66.0F38.W1 CC /r

		Sha256msg2_xmm_xmmm128,										// NP 0F 38 CD /r

		EVEX_Vrsqrt28ss_xmm_k1z_xmm_xmmm32_sae,						// EVEX.LIG.66.0F38.W0 CD /r
		EVEX_Vrsqrt28sd_xmm_k1z_xmm_xmmm64_sae,						// EVEX.LIG.66.0F38.W1 CD /r

		Gf2p8mulb_xmm_xmmm128,										// 66 0F 38 CF /r
		VEX_Vgf2p8mulb_xmm_xmm_xmmm128,								// VEX.128.66.0F38.W0 CF /r
		VEX_Vgf2p8mulb_ymm_ymm_ymmm256,								// VEX.256.66.0F38.W0 CF /r
		EVEX_Vgf2p8mulb_xmm_k1z_xmm_xmmm128,						// EVEX.128.66.0F38.W0 CF /r
		EVEX_Vgf2p8mulb_ymm_k1z_ymm_ymmm256,						// EVEX.256.66.0F38.W0 CF /r
		EVEX_Vgf2p8mulb_zmm_k1z_zmm_zmmm512,						// EVEX.512.66.0F38.W0 CF /r

		Aesimc_xmm_xmmm128,											// 66 0F 38 DB /r
		VEX_Vaesimc_xmm_xmmm128,									// VEX.128.66.0F38.WIG DB /r

		Aesenc_xmm_xmmm128,											// 66 0F 38 DC /r
		VEX_Vaesenc_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG DC /r
		VEX_Vaesenc_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG DC /r
		EVEX_Vaesenc_xmm_xmm_xmmm128,								// EVEX.128.66.0F38.WIG DC /r
		EVEX_Vaesenc_ymm_ymm_ymmm256,								// EVEX.256.66.0F38.WIG DC /r
		EVEX_Vaesenc_zmm_zmm_zmmm512,								// EVEX.512.66.0F38.WIG DC /r

		Aesenclast_xmm_xmmm128,										// 66 0F 38 DD /r
		VEX_Vaesenclast_xmm_xmm_xmmm128,							// VEX.128.66.0F38.WIG DD /r
		VEX_Vaesenclast_ymm_ymm_ymmm256,							// VEX.256.66.0F38.WIG DD /r
		EVEX_Vaesenclast_xmm_xmm_xmmm128,							// EVEX.128.66.0F38.WIG DD /r
		EVEX_Vaesenclast_ymm_ymm_ymmm256,							// EVEX.256.66.0F38.WIG DD /r
		EVEX_Vaesenclast_zmm_zmm_zmmm512,							// EVEX.512.66.0F38.WIG DD /r

		Aesdec_xmm_xmmm128,											// 66 0F 38 DE /r
		VEX_Vaesdec_xmm_xmm_xmmm128,								// VEX.128.66.0F38.WIG DE /r
		VEX_Vaesdec_ymm_ymm_ymmm256,								// VEX.256.66.0F38.WIG DE /r
		EVEX_Vaesdec_xmm_xmm_xmmm128,								// EVEX.128.66.0F38.WIG DE /r
		EVEX_Vaesdec_ymm_ymm_ymmm256,								// EVEX.256.66.0F38.WIG DE /r
		EVEX_Vaesdec_zmm_zmm_zmmm512,								// EVEX.512.66.0F38.WIG DE /r

		Aesdeclast_xmm_xmmm128,										// 66 0F 38 DF /r
		VEX_Vaesdeclast_xmm_xmm_xmmm128,							// VEX.128.66.0F38.WIG DF /r
		VEX_Vaesdeclast_ymm_ymm_ymmm256,							// VEX.256.66.0F38.WIG DF /r
		EVEX_Vaesdeclast_xmm_xmm_xmmm128,							// EVEX.128.66.0F38.WIG DF /r
		EVEX_Vaesdeclast_ymm_ymm_ymmm256,							// EVEX.256.66.0F38.WIG DF /r
		EVEX_Vaesdeclast_zmm_zmm_zmmm512,							// EVEX.512.66.0F38.WIG DF /r

		Movbe_r16_m16,												// o16 0F 38 F0 /r
		Movbe_r32_m32,												// o32 0F 38 F0 /r
		Movbe_r64_m64,												// REX.W 0F 38 F0 /r

		Crc32_r32_rm8,												// F2 0F 38 F0 /r
		Crc32_r64_rm8,												// F2 REX.W 0F 38 F0 /r

		Movbe_m16_r16,												// o16 0F 38 F1 /r
		Movbe_m32_r32,												// o32 0F 38 F1 /r
		Movbe_m64_r64,												// REX.W 0F 38 F1 /r

		Crc32_r32_rm16,												// o16 F2 0F 38 F1 /r
		Crc32_r32_rm32,												// o32 F2 0F 38 F1 /r
		Crc32_r64_rm64,												// F2 REX.W 0F 38 F1 /r

		VEX_Andn_r32_r32_rm32,										// VEX.LZ.0F38.W0 F2 /r
		VEX_Andn_r64_r64_rm64,										// VEX.LZ.0F38.W1 F2 /r

		VEX_Blsr_r32_rm32,											// VEX.LZ.0F38.W0 F3 /1
		VEX_Blsr_r64_rm64,											// VEX.LZ.0F38.W1 F3 /1
		VEX_Blsmsk_r32_rm32,										// VEX.LZ.0F38.W0 F3 /2
		VEX_Blsmsk_r64_rm64,										// VEX.LZ.0F38.W1 F3 /2
		VEX_Blsi_r32_rm32,											// VEX.LZ.0F38.W0 F3 /3
		VEX_Blsi_r64_rm64,											// VEX.LZ.0F38.W1 F3 /3

		VEX_Bzhi_r32_rm32_r32,										// VEX.LZ.0F38.W0 F5 /r
		VEX_Bzhi_r64_rm64_r64,										// VEX.LZ.0F38.W1 F5 /r

		Wrussd_m32_r32,												// 66 0F 38 F5 /r
		Wrussq_m64_r64,												// 66 REX.W 0F 38 F5 /r

		VEX_Pext_r32_r32_rm32,										// VEX.LZ.F3.0F38.W0 F5 /r
		VEX_Pext_r64_r64_rm64,										// VEX.LZ.F3.0F38.W1 F5 /r

		VEX_Pdep_r32_r32_rm32,										// VEX.LZ.F2.0F38.W0 F5 /r
		VEX_Pdep_r64_r64_rm64,										// VEX.LZ.F2.0F38.W1 F5 /r

		Wrssd_m32_r32,												// NP 0F 38 F6 /r
		Wrssq_m64_r64,												// NP REX.W 0F 38 F6 /r

		Adcx_r32_rm32,												// 66 0F 38 F6 /r
		Adcx_r64_rm64,												// 66 REX.W 0F 38 F6 /r

		Adox_r32_rm32,												// F3 0F 38 F6 /r
		Adox_r64_rm64,												// F3 REX.W 0F 38 F6 /r

		VEX_Mulx_r32_r32_rm32,										// VEX.LZ.F2.0F38.W0 F6 /r
		VEX_Mulx_r64_r64_rm64,										// VEX.LZ.F2.0F38.W1 F6 /r

		VEX_Bextr_r32_rm32_r32,										// VEX.LZ.0F38.W0 F7 /r
		VEX_Bextr_r64_rm64_r64,										// VEX.LZ.0F38.W1 F7 /r

		VEX_Shlx_r32_rm32_r32,										// VEX.LZ.66.0F38.W0 F7 /r
		VEX_Shlx_r64_rm64_r64,										// VEX.LZ.66.0F38.W1 F7 /r

		VEX_Sarx_r32_rm32_r32,										// VEX.LZ.F3.0F38.W0 F7 /r
		VEX_Sarx_r64_rm64_r64,										// VEX.LZ.F3.0F38.W1 F7 /r

		VEX_Shrx_r32_rm32_r32,										// VEX.LZ.F2.0F38.W0 F7 /r
		VEX_Shrx_r64_rm64_r64,										// VEX.LZ.F2.0F38.W1 F7 /r

		Movdir64b_r16_m512,											// a16 66 0F 38 F8 /r
		Movdir64b_r32_m512,											// a32 66 0F 38 F8 /r
		Movdir64b_r64_m512,											// 66 0F 38 F8 /r

		Enqcmds_r16_m512,											// a16 F3 0F 38 F8 /r
		Enqcmds_r32_m512,											// a32 F3 0F 38 F8 /r
		Enqcmds_r64_m512,											// F3 0F 38 F8 /r

		Enqcmd_r16_m512,											// a16 F2 0F 38 F8 /r
		Enqcmd_r32_m512,											// a32 F2 0F 38 F8 /r
		Enqcmd_r64_m512,											// F2 0F 38 F8 /r

		Movdiri_m32_r32,											// NP 0F 38 F9 /r
		Movdiri_m64_r64,											// NP REX.W 0F 38 F9 /r

		// 0F 3A xx opcodes

		VEX_Vpermq_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W1 00 /r ib
		EVEX_Vpermq_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 00 /r ib
		EVEX_Vpermq_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 00 /r ib

		VEX_Vpermpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W1 01 /r ib
		EVEX_Vpermpd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 01 /r ib
		EVEX_Vpermpd_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 01 /r ib

		VEX_Vpblendd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 02 /r ib
		VEX_Vpblendd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.W0 02 /r ib

		EVEX_Valignd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 03 /r ib
		EVEX_Valignd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 03 /r ib
		EVEX_Valignd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 03 /r ib
		EVEX_Valignq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 03 /r ib
		EVEX_Valignq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 03 /r ib
		EVEX_Valignq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 03 /r ib

		VEX_Vpermilps_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.W0 04 /r ib
		VEX_Vpermilps_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W0 04 /r ib
		EVEX_Vpermilps_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 04 /r ib
		EVEX_Vpermilps_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 04 /r ib
		EVEX_Vpermilps_zmm_k1z_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 04 /r ib

		VEX_Vpermilpd_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.W0 05 /r ib
		VEX_Vpermilpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.W0 05 /r ib
		EVEX_Vpermilpd_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 05 /r ib
		EVEX_Vpermilpd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 05 /r ib
		EVEX_Vpermilpd_zmm_k1z_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 05 /r ib

		VEX_Vperm2f128_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.W0 06 /r ib

		Roundps_xmm_xmmm128_imm8,									// 66 0F 3A 08 /r ib
		VEX_Vroundps_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 08 /r ib
		VEX_Vroundps_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 08 /r ib
		EVEX_Vrndscaleps_xmm_k1z_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 08 /r ib
		EVEX_Vrndscaleps_ymm_k1z_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 08 /r ib
		EVEX_Vrndscaleps_zmm_k1z_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 08 /r ib

		Roundpd_xmm_xmmm128_imm8,									// 66 0F 3A 09 /r ib
		VEX_Vroundpd_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 09 /r ib
		VEX_Vroundpd_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 09 /r ib
		EVEX_Vrndscalepd_xmm_k1z_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 09 /r ib
		EVEX_Vrndscalepd_ymm_k1z_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 09 /r ib
		EVEX_Vrndscalepd_zmm_k1z_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 09 /r ib

		Roundss_xmm_xmmm32_imm8,									// 66 0F 3A 0A /r ib
		VEX_Vroundss_xmm_xmm_xmmm32_imm8,							// VEX.LIG.66.0F3A.WIG 0A /r ib
		EVEX_Vrndscaless_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 0A /r ib

		Roundsd_xmm_xmmm64_imm8,									// 66 0F 3A 0B /r ib
		VEX_Vroundsd_xmm_xmm_xmmm64_imm8,							// VEX.LIG.66.0F3A.WIG 0B /r ib
		EVEX_Vrndscalesd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 0B /r ib

		Blendps_xmm_xmmm128_imm8,									// 66 0F 3A 0C /r ib
		VEX_Vblendps_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0C /r ib
		VEX_Vblendps_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0C /r ib

		Blendpd_xmm_xmmm128_imm8,									// 66 0F 3A 0D /r ib
		VEX_Vblendpd_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0D /r ib
		VEX_Vblendpd_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0D /r ib

		Pblendw_xmm_xmmm128_imm8,									// 66 0F 3A 0E /r ib
		VEX_Vpblendw_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0E /r ib
		VEX_Vpblendw_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0E /r ib

		Palignr_mm_mmm64_imm8,										// NP 0F 3A 0F /r ib

		Palignr_xmm_xmmm128_imm8,									// 66 0F 3A 0F /r ib
		VEX_Vpalignr_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 0F /r ib
		VEX_Vpalignr_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 0F /r ib
		EVEX_Vpalignr_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.WIG 0F /r ib
		EVEX_Vpalignr_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.WIG 0F /r ib
		EVEX_Vpalignr_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.WIG 0F /r ib

		Pextrb_r32m8_xmm_imm8,										// 66 0F 3A 14 /r ib
		Pextrb_r64m8_xmm_imm8,										// 66 REX.W 0F 3A 14 /r ib
		VEX_Vpextrb_r32m8_xmm_imm8,									// VEX.128.66.0F3A.W0 14 /r ib
		VEX_Vpextrb_r64m8_xmm_imm8,									// VEX.128.66.0F3A.W1 14 /r ib
		EVEX_Vpextrb_r32m8_xmm_imm8,								// EVEX.128.66.0F3A.W0 14 /r ib
		EVEX_Vpextrb_r64m8_xmm_imm8,								// EVEX.128.66.0F3A.W1 14 /r ib

		Pextrw_r32m16_xmm_imm8,										// 66 0F 3A 15 /r ib
		Pextrw_r64m16_xmm_imm8,										// 66 REX.W 0F 3A 15 /r ib
		VEX_Vpextrw_r32m16_xmm_imm8,								// VEX.128.66.0F3A.W0 15 /r ib
		VEX_Vpextrw_r64m16_xmm_imm8,								// VEX.128.66.0F3A.W1 15 /r ib
		EVEX_Vpextrw_r32m16_xmm_imm8,								// EVEX.128.66.0F3A.W0 15 /r ib
		EVEX_Vpextrw_r64m16_xmm_imm8,								// EVEX.128.66.0F3A.W1 15 /r ib

		Pextrd_rm32_xmm_imm8,										// 66 0F 3A 16 /r ib
		Pextrq_rm64_xmm_imm8,										// 66 REX.W 0F 3A 16 /r ib
		VEX_Vpextrd_rm32_xmm_imm8,									// VEX.128.66.0F3A.W0 16 /r ib
		VEX_Vpextrq_rm64_xmm_imm8,									// VEX.128.66.0F3A.W1 16 /r ib
		EVEX_Vpextrd_rm32_xmm_imm8,									// EVEX.128.66.0F3A.W0 16 /r ib
		EVEX_Vpextrq_rm64_xmm_imm8,									// EVEX.128.66.0F3A.W1 16 /r ib

		Extractps_rm32_xmm_imm8,									// 66 0F 3A 17 /r ib
		Extractps_r64m32_xmm_imm8,									// 66 REX.W 0F 3A 17 /r ib
		VEX_Vextractps_rm32_xmm_imm8,								// VEX.128.66.0F3A.W0 17 /r ib
		VEX_Vextractps_r64m32_xmm_imm8,								// VEX.128.66.0F3A.W1 17 /r ib
		EVEX_Vextractps_rm32_xmm_imm8,								// EVEX.128.66.0F3A.W0 17 /r ib
		EVEX_Vextractps_r64m32_xmm_imm8,							// EVEX.128.66.0F3A.W1 17 /r ib

		VEX_Vinsertf128_ymm_ymm_xmmm128_imm8,						// VEX.256.66.0F3A.W0 18 /r ib
		EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W0 18 /r ib
		EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W0 18 /r ib
		EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W1 18 /r ib
		EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W1 18 /r ib

		VEX_Vextractf128_xmmm128_ymm_imm8,							// VEX.256.66.0F3A.W0 19 /r ib
		EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W0 19 /r ib
		EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 19 /r ib
		EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W1 19 /r ib
		EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 19 /r ib

		EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W0 1A /r ib
		EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W1 1A /r ib

		EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 1B /r ib
		EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 1B /r ib

		VEX_Vcvtps2ph_xmmm64_xmm_imm8,								// VEX.128.66.0F3A.W0 1D /r ib
		VEX_Vcvtps2ph_xmmm128_ymm_imm8,								// VEX.256.66.0F3A.W0 1D /r ib
		EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8,							// EVEX.128.66.0F3A.W0 1D /r ib
		EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8,						// EVEX.256.66.0F3A.W0 1D /r ib
		EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae,					// EVEX.512.66.0F3A.W0 1D /r ib

		EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 1E /r ib
		EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 1E /r ib
		EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 1E /r ib
		EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 1E /r ib
		EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 1E /r ib
		EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 1E /r ib

		EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 1F /r ib
		EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 1F /r ib
		EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 1F /r ib
		EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 1F /r ib
		EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 1F /r ib
		EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 1F /r ib

		Pinsrb_xmm_r32m8_imm8,										// 66 0F 3A 20 /r ib
		Pinsrb_xmm_r64m8_imm8,										// 66 REX.W 0F 3A 20 /r ib
		VEX_Vpinsrb_xmm_xmm_r32m8_imm8,								// VEX.128.66.0F3A.W0 20 /r ib
		VEX_Vpinsrb_xmm_xmm_r64m8_imm8,								// VEX.128.66.0F3A.W1 20 /r ib
		EVEX_Vpinsrb_xmm_xmm_r32m8_imm8,							// EVEX.128.66.0F3A.W0 20 /r ib
		EVEX_Vpinsrb_xmm_xmm_r64m8_imm8,							// EVEX.128.66.0F3A.W1 20 /r ib

		Insertps_xmm_xmmm32_imm8,									// 66 0F 3A 21 /r ib
		VEX_Vinsertps_xmm_xmm_xmmm32_imm8,							// VEX.128.66.0F3A.WIG 21 /r ib
		EVEX_Vinsertps_xmm_xmm_xmmm32_imm8,							// EVEX.128.66.0F3A.W0 21 /r ib

		Pinsrd_xmm_rm32_imm8,										// 66 0F 3A 22 /r ib
		Pinsrq_xmm_rm64_imm8,										// 66 REX.W 0F 3A 22 /r ib
		VEX_Vpinsrd_xmm_xmm_rm32_imm8,								// VEX.128.66.0F3A.W0 22 /r ib
		VEX_Vpinsrq_xmm_xmm_rm64_imm8,								// VEX.128.66.0F3A.W1 22 /r ib
		EVEX_Vpinsrd_xmm_xmm_rm32_imm8,								// EVEX.128.66.0F3A.W0 22 /r ib
		EVEX_Vpinsrq_xmm_xmm_rm64_imm8,								// EVEX.128.66.0F3A.W1 22 /r ib

		EVEX_Vshuff32x4_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 23 /r ib
		EVEX_Vshuff32x4_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 23 /r ib
		EVEX_Vshuff64x2_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 23 /r ib
		EVEX_Vshuff64x2_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 23 /r ib

		EVEX_Vpternlogd_xmm_k1z_xmm_xmmm128b32_imm8,				// EVEX.128.66.0F3A.W0 25 /r ib
		EVEX_Vpternlogd_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 25 /r ib
		EVEX_Vpternlogd_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 25 /r ib
		EVEX_Vpternlogq_xmm_k1z_xmm_xmmm128b64_imm8,				// EVEX.128.66.0F3A.W1 25 /r ib
		EVEX_Vpternlogq_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 25 /r ib
		EVEX_Vpternlogq_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 25 /r ib

		EVEX_Vgetmantps_xmm_k1z_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 26 /r ib
		EVEX_Vgetmantps_ymm_k1z_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 26 /r ib
		EVEX_Vgetmantps_zmm_k1z_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 26 /r ib
		EVEX_Vgetmantpd_xmm_k1z_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 26 /r ib
		EVEX_Vgetmantpd_ymm_k1z_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 26 /r ib
		EVEX_Vgetmantpd_zmm_k1z_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 26 /r ib

		EVEX_Vgetmantss_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 27 /r ib
		EVEX_Vgetmantsd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 27 /r ib

		VEX_Kshiftrb_k_k_imm8,										// VEX.L0.66.0F3A.W0 30 /r ib
		VEX_Kshiftrw_k_k_imm8,										// VEX.L0.66.0F3A.W1 30 /r ib
		VEX_Kshiftrd_k_k_imm8,										// VEX.L0.66.0F3A.W0 31 /r ib
		VEX_Kshiftrq_k_k_imm8,										// VEX.L0.66.0F3A.W1 31 /r ib

		VEX_Kshiftlb_k_k_imm8,										// VEX.L0.66.0F3A.W0 32 /r ib
		VEX_Kshiftlw_k_k_imm8,										// VEX.L0.66.0F3A.W1 32 /r ib
		VEX_Kshiftld_k_k_imm8,										// VEX.L0.66.0F3A.W0 33 /r ib
		VEX_Kshiftlq_k_k_imm8,										// VEX.L0.66.0F3A.W1 33 /r ib

		VEX_Vinserti128_ymm_ymm_xmmm128_imm8,						// VEX.256.66.0F3A.W0 38 /r ib
		EVEX_Vinserti32x4_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W0 38 /r ib
		EVEX_Vinserti32x4_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W0 38 /r ib
		EVEX_Vinserti64x2_ymm_k1z_ymm_xmmm128_imm8,					// EVEX.256.66.0F3A.W1 38 /r ib
		EVEX_Vinserti64x2_zmm_k1z_zmm_xmmm128_imm8,					// EVEX.512.66.0F3A.W1 38 /r ib

		VEX_Vextracti128_xmmm128_ymm_imm8,							// VEX.256.66.0F3A.W0 39 /r ib
		EVEX_Vextracti32x4_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W0 39 /r ib
		EVEX_Vextracti32x4_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 39 /r ib
		EVEX_Vextracti64x2_xmmm128_k1z_ymm_imm8,					// EVEX.256.66.0F3A.W1 39 /r ib
		EVEX_Vextracti64x2_xmmm128_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 39 /r ib

		EVEX_Vinserti32x8_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W0 3A /r ib
		EVEX_Vinserti64x4_zmm_k1z_zmm_ymmm256_imm8,					// EVEX.512.66.0F3A.W1 3A /r ib

		EVEX_Vextracti32x8_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W0 3B /r ib
		EVEX_Vextracti64x4_ymmm256_k1z_zmm_imm8,					// EVEX.512.66.0F3A.W1 3B /r ib

		EVEX_Vpcmpub_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W0 3E /r ib
		EVEX_Vpcmpub_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W0 3E /r ib
		EVEX_Vpcmpub_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W0 3E /r ib
		EVEX_Vpcmpuw_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W1 3E /r ib
		EVEX_Vpcmpuw_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W1 3E /r ib
		EVEX_Vpcmpuw_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W1 3E /r ib

		EVEX_Vpcmpb_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W0 3F /r ib
		EVEX_Vpcmpb_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W0 3F /r ib
		EVEX_Vpcmpb_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W0 3F /r ib
		EVEX_Vpcmpw_k_k1_xmm_xmmm128_imm8,							// EVEX.128.66.0F3A.W1 3F /r ib
		EVEX_Vpcmpw_k_k1_ymm_ymmm256_imm8,							// EVEX.256.66.0F3A.W1 3F /r ib
		EVEX_Vpcmpw_k_k1_zmm_zmmm512_imm8,							// EVEX.512.66.0F3A.W1 3F /r ib

		Dpps_xmm_xmmm128_imm8,										// 66 0F 3A 40 /r ib
		VEX_Vdpps_xmm_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 40 /r ib
		VEX_Vdpps_ymm_ymm_ymmm256_imm8,								// VEX.256.66.0F3A.WIG 40 /r ib

		Dppd_xmm_xmmm128_imm8,										// 66 0F 3A 41 /r ib
		VEX_Vdppd_xmm_xmm_xmmm128_imm8,								// VEX.128.66.0F3A.WIG 41 /r ib

		Mpsadbw_xmm_xmmm128_imm8,									// 66 0F 3A 42 /r ib
		VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 42 /r ib
		VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8,							// VEX.256.66.0F3A.WIG 42 /r ib
		EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8,					// EVEX.128.66.0F3A.W0 42 /r ib
		EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8,					// EVEX.256.66.0F3A.W0 42 /r ib
		EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8,					// EVEX.512.66.0F3A.W0 42 /r ib

		EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 43 /r ib
		EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8,				// EVEX.512.66.0F3A.W0 43 /r ib
		EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 43 /r ib
		EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8,				// EVEX.512.66.0F3A.W1 43 /r ib

		Pclmulqdq_xmm_xmmm128_imm8,									// 66 0F 3A 44 /r ib
		VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8,						// VEX.128.66.0F3A.WIG 44 /r ib
		VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.WIG 44 /r ib
		EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.WIG 44 /r ib
		EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.WIG 44 /r ib
		EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.WIG 44 /r ib

		VEX_Vperm2i128_ymm_ymm_ymmm256_imm8,						// VEX.256.66.0F3A.W0 46 /r ib

		VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm2,					// VEX.128.66.0F3A.W0 48 /r /is5
		VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm2,					// VEX.256.66.0F3A.W0 48 /r /is5
		VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm2,					// VEX.128.66.0F3A.W1 48 /r /is5
		VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm2,					// VEX.256.66.0F3A.W1 48 /r /is5

		VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm2,					// VEX.128.66.0F3A.W0 49 /r /is5
		VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm2,					// VEX.256.66.0F3A.W0 49 /r /is5
		VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm2,					// VEX.128.66.0F3A.W1 49 /r /is5
		VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm2,					// VEX.256.66.0F3A.W1 49 /r /is5

		VEX_Vblendvps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4A /r /is4
		VEX_Vblendvps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4A /r /is4

		VEX_Vblendvpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4B /r /is4
		VEX_Vblendvpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4B /r /is4

		VEX_Vpblendvb_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 4C /r /is4
		VEX_Vpblendvb_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 4C /r /is4

		EVEX_Vrangeps_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 50 /r ib
		EVEX_Vrangeps_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 50 /r ib
		EVEX_Vrangeps_zmm_k1z_zmm_zmmm512b32_imm8_sae,				// EVEX.512.66.0F3A.W0 50 /r ib
		EVEX_Vrangepd_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 50 /r ib
		EVEX_Vrangepd_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 50 /r ib
		EVEX_Vrangepd_zmm_k1z_zmm_zmmm512b64_imm8_sae,				// EVEX.512.66.0F3A.W1 50 /r ib

		EVEX_Vrangess_xmm_k1z_xmm_xmmm32_imm8_sae,					// EVEX.LIG.66.0F3A.W0 51 /r ib
		EVEX_Vrangesd_xmm_k1z_xmm_xmmm64_imm8_sae,					// EVEX.LIG.66.0F3A.W1 51 /r ib

		EVEX_Vfixupimmps_xmm_k1z_xmm_xmmm128b32_imm8,				// EVEX.128.66.0F3A.W0 54 /r ib
		EVEX_Vfixupimmps_ymm_k1z_ymm_ymmm256b32_imm8,				// EVEX.256.66.0F3A.W0 54 /r ib
		EVEX_Vfixupimmps_zmm_k1z_zmm_zmmm512b32_imm8_sae,			// EVEX.512.66.0F3A.W0 54 /r ib
		EVEX_Vfixupimmpd_xmm_k1z_xmm_xmmm128b64_imm8,				// EVEX.128.66.0F3A.W1 54 /r ib
		EVEX_Vfixupimmpd_ymm_k1z_ymm_ymmm256b64_imm8,				// EVEX.256.66.0F3A.W1 54 /r ib
		EVEX_Vfixupimmpd_zmm_k1z_zmm_zmmm512b64_imm8_sae,			// EVEX.512.66.0F3A.W1 54 /r ib

		EVEX_Vfixupimmss_xmm_k1z_xmm_xmmm32_imm8_sae,				// EVEX.LIG.66.0F3A.W0 55 /r ib
		EVEX_Vfixupimmsd_xmm_k1z_xmm_xmmm64_imm8_sae,				// EVEX.LIG.66.0F3A.W1 55 /r ib

		EVEX_Vreduceps_xmm_k1z_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 56 /r ib
		EVEX_Vreduceps_ymm_k1z_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 56 /r ib
		EVEX_Vreduceps_zmm_k1z_zmmm512b32_imm8_sae,					// EVEX.512.66.0F3A.W0 56 /r ib
		EVEX_Vreducepd_xmm_k1z_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 56 /r ib
		EVEX_Vreducepd_ymm_k1z_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 56 /r ib
		EVEX_Vreducepd_zmm_k1z_zmmm512b64_imm8_sae,					// EVEX.512.66.0F3A.W1 56 /r ib

		EVEX_Vreducess_xmm_k1z_xmm_xmmm32_imm8_sae,					// EVEX.LIG.66.0F3A.W0 57 /r ib
		EVEX_Vreducesd_xmm_k1z_xmm_xmmm64_imm8_sae,					// EVEX.LIG.66.0F3A.W1 57 /r ib

		VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5C /r /is4
		VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5C /r /is4
		VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5C /r /is4
		VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5C /r /is4

		VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5D /r /is4
		VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5D /r /is4
		VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5D /r /is4
		VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5D /r /is4

		VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5E /r /is4
		VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5E /r /is4
		VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5E /r /is4
		VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5E /r /is4

		VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm,						// VEX.128.66.0F3A.W0 5F /r /is4
		VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm,						// VEX.256.66.0F3A.W0 5F /r /is4
		VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128,						// VEX.128.66.0F3A.W1 5F /r /is4
		VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256,						// VEX.256.66.0F3A.W1 5F /r /is4

		Pcmpestrm_xmm_xmmm128_imm8,									// 66 0F 3A 60 /r ib
		Pcmpestrm64_xmm_xmmm128_imm8,								// 66 REX.W 0F 3A 60 /r ib
		VEX_Vpcmpestrm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 60 /r ib
		VEX_Vpcmpestrm64_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W1 60 /r ib

		Pcmpestri_xmm_xmmm128_imm8,									// 66 0F 3A 61 /r ib
		Pcmpestri64_xmm_xmmm128_imm8,								// 66 REX.W 0F 3A 61 /r ib
		VEX_Vpcmpestri_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W0 61 /r ib
		VEX_Vpcmpestri64_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.W1 61 /r ib

		Pcmpistrm_xmm_xmmm128_imm8,									// 66 0F 3A 62 /r ib
		VEX_Vpcmpistrm_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 62 /r ib

		Pcmpistri_xmm_xmmm128_imm8,									// 66 0F 3A 63 /r ib
		VEX_Vpcmpistri_xmm_xmmm128_imm8,							// VEX.128.66.0F3A.WIG 63 /r ib

		EVEX_Vfpclassps_k_k1_xmmm128b32_imm8,						// EVEX.128.66.0F3A.W0 66 /r ib
		EVEX_Vfpclassps_k_k1_ymmm256b32_imm8,						// EVEX.256.66.0F3A.W0 66 /r ib
		EVEX_Vfpclassps_k_k1_zmmm512b32_imm8,						// EVEX.512.66.0F3A.W0 66 /r ib
		EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8,						// EVEX.128.66.0F3A.W1 66 /r ib
		EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8,						// EVEX.256.66.0F3A.W1 66 /r ib
		EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8,						// EVEX.512.66.0F3A.W1 66 /r ib

		EVEX_Vfpclassss_k_k1_xmmm32_imm8,							// EVEX.LIG.66.0F3A.W0 67 /r ib
		EVEX_Vfpclasssd_k_k1_xmmm64_imm8,							// EVEX.LIG.66.0F3A.W1 67 /r ib

		VEX_Vfmaddps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 68 /r /is4
		VEX_Vfmaddps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 68 /r /is4
		VEX_Vfmaddps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 68 /r /is4
		VEX_Vfmaddps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 68 /r /is4

		VEX_Vfmaddpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 69 /r /is4
		VEX_Vfmaddpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 69 /r /is4
		VEX_Vfmaddpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 69 /r /is4
		VEX_Vfmaddpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 69 /r /is4

		VEX_Vfmaddss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 6A /r /is4
		VEX_Vfmaddss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 6A /r /is4

		VEX_Vfmaddsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 6B /r /is4
		VEX_Vfmaddsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 6B /r /is4

		VEX_Vfmsubps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 6C /r /is4
		VEX_Vfmsubps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 6C /r /is4
		VEX_Vfmsubps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 6C /r /is4
		VEX_Vfmsubps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 6C /r /is4

		VEX_Vfmsubpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 6D /r /is4
		VEX_Vfmsubpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 6D /r /is4
		VEX_Vfmsubpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 6D /r /is4
		VEX_Vfmsubpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 6D /r /is4

		VEX_Vfmsubss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 6E /r /is4
		VEX_Vfmsubss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 6E /r /is4

		VEX_Vfmsubsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 6F /r /is4
		VEX_Vfmsubsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 6F /r /is4

		EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.W1 70 /r ib
		EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.W1 70 /r ib
		EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.W1 70 /r ib

		EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 71 /r ib
		EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 71 /r ib
		EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 71 /r ib
		EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 71 /r ib
		EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 71 /r ib
		EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 71 /r ib

		EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8,						// EVEX.128.66.0F3A.W1 72 /r ib
		EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8,						// EVEX.256.66.0F3A.W1 72 /r ib
		EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8,						// EVEX.512.66.0F3A.W1 72 /r ib

		EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8,					// EVEX.128.66.0F3A.W0 73 /r ib
		EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8,					// EVEX.256.66.0F3A.W0 73 /r ib
		EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8,					// EVEX.512.66.0F3A.W0 73 /r ib
		EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8,					// EVEX.128.66.0F3A.W1 73 /r ib
		EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8,					// EVEX.256.66.0F3A.W1 73 /r ib
		EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8,					// EVEX.512.66.0F3A.W1 73 /r ib

		VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 78 /r /is4
		VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 78 /r /is4
		VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 78 /r /is4
		VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 78 /r /is4

		VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 79 /r /is4
		VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 79 /r /is4
		VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 79 /r /is4
		VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 79 /r /is4

		VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 7A /r /is4
		VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 7A /r /is4

		VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 7B /r /is4
		VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 7B /r /is4

		VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 7C /r /is4
		VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 7C /r /is4
		VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 7C /r /is4
		VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 7C /r /is4

		VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm,							// VEX.128.66.0F3A.W0 7D /r /is4
		VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm,							// VEX.256.66.0F3A.W0 7D /r /is4
		VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128,							// VEX.128.66.0F3A.W1 7D /r /is4
		VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256,							// VEX.256.66.0F3A.W1 7D /r /is4

		VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm,							// VEX.LIG.66.0F3A.W0 7E /r /is4
		VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32,							// VEX.LIG.66.0F3A.W1 7E /r /is4

		VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm,							// VEX.LIG.66.0F3A.W0 7F /r /is4
		VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64,							// VEX.LIG.66.0F3A.W1 7F /r /is4

		Sha1rnds4_xmm_xmmm128_imm8,									// NP 0F 3A CC /r ib

		Gf2p8affineqb_xmm_xmmm128_imm8,								// 66 0F 3A CE /r ib
		VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 CE /r ib
		VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 CE /r ib
		EVEX_Vgf2p8affineqb_xmm_k1z_xmm_xmmm128b64_imm8,			// EVEX.128.66.0F3A.W1 CE /r ib
		EVEX_Vgf2p8affineqb_ymm_k1z_ymm_ymmm256b64_imm8,			// EVEX.256.66.0F3A.W1 CE /r ib
		EVEX_Vgf2p8affineqb_zmm_k1z_zmm_zmmm512b64_imm8,			// EVEX.512.66.0F3A.W1 CE /r ib

		Gf2p8affineinvqb_xmm_xmmm128_imm8,							// 66 0F 3A CF /r ib
		VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8,					// VEX.128.66.0F3A.W1 CF /r ib
		VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8,					// VEX.256.66.0F3A.W1 CF /r ib
		EVEX_Vgf2p8affineinvqb_xmm_k1z_xmm_xmmm128b64_imm8,			// EVEX.128.66.0F3A.W1 CF /r ib
		EVEX_Vgf2p8affineinvqb_ymm_k1z_ymm_ymmm256b64_imm8,			// EVEX.256.66.0F3A.W1 CF /r ib
		EVEX_Vgf2p8affineinvqb_zmm_k1z_zmm_zmmm512b64_imm8,			// EVEX.512.66.0F3A.W1 CF /r ib

		Aeskeygenassist_xmm_xmmm128_imm8,							// 66 0F 3A DF /r ib
		VEX_Vaeskeygenassist_xmm_xmmm128_imm8,						// VEX.128.66.0F3A.WIG DF /r ib

		VEX_Rorx_r32_rm32_imm8,										// VEX.LZ.F2.0F3A.W0 F0 /r ib
		VEX_Rorx_r64_rm64_imm8,										// VEX.LZ.F2.0F3A.W1 F0 /r ib

		// XOP8 opcodes

		XOP_Vpmacssww_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 85 /r /is4

		XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 86 /r /is4

		XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 87 /r /is4

		XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 8E /r /is4

		XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 8F /r /is4

		XOP_Vpmacsww_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 95 /r /is4

		XOP_Vpmacswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 96 /r /is4

		XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 97 /r /is4

		XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 9E /r /is4

		XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 9F /r /is4

		XOP_Vpcmov_xmm_xmm_xmmm128_xmm,								// XOP.128.X8.W0 A2 /r /is4
		XOP_Vpcmov_ymm_ymm_ymmm256_ymm,								// XOP.256.X8.W0 A2 /r /is4
		XOP_Vpcmov_xmm_xmm_xmm_xmmm128,								// XOP.128.X8.W1 A2 /r /is4
		XOP_Vpcmov_ymm_ymm_ymm_ymmm256,								// XOP.256.X8.W1 A2 /r /is4

		XOP_Vpperm_xmm_xmm_xmmm128_xmm,								// XOP.128.X8.W0 A3 /r /is4
		XOP_Vpperm_xmm_xmm_xmm_xmmm128,								// XOP.128.X8.W1 A3 /r /is4

		XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 A6 /r /is4

		XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm,							// XOP.128.X8.W0 B6 /r /is4

		XOP_Vprotb_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C0 /r ib

		XOP_Vprotw_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C1 /r ib

		XOP_Vprotd_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C2 /r ib

		XOP_Vprotq_xmm_xmmm128_imm8,								// XOP.128.X8.W0 C3 /r ib

		XOP_Vpcomb_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CC /r ib

		XOP_Vpcomw_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CD /r ib

		XOP_Vpcomd_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CE /r ib

		XOP_Vpcomq_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 CF /r ib

		XOP_Vpcomub_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EC /r ib

		XOP_Vpcomuw_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 ED /r ib

		XOP_Vpcomud_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EE /r ib

		XOP_Vpcomuq_xmm_xmm_xmmm128_imm8,							// XOP.128.X8.W0 EF /r ib

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

		XOP_Vfrczps_xmm_xmmm128,									// XOP.128.X9.W0 80 /r
		XOP_Vfrczps_ymm_ymmm256,									// XOP.256.X9.W0 80 /r

		XOP_Vfrczpd_xmm_xmmm128,									// XOP.128.X9.W0 81 /r
		XOP_Vfrczpd_ymm_ymmm256,									// XOP.256.X9.W0 81 /r

		XOP_Vfrczss_xmm_xmmm32,										// XOP.128.X9.W0 82 /r

		XOP_Vfrczsd_xmm_xmmm64,										// XOP.128.X9.W0 83 /r

		XOP_Vprotb_xmm_xmmm128_xmm,									// XOP.128.X9.W0 90 /r
		XOP_Vprotb_xmm_xmm_xmmm128,									// XOP.128.X9.W1 90 /r

		XOP_Vprotw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 91 /r
		XOP_Vprotw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 91 /r

		XOP_Vprotd_xmm_xmmm128_xmm,									// XOP.128.X9.W0 92 /r
		XOP_Vprotd_xmm_xmm_xmmm128,									// XOP.128.X9.W1 92 /r

		XOP_Vprotq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 93 /r
		XOP_Vprotq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 93 /r

		XOP_Vpshlb_xmm_xmmm128_xmm,									// XOP.128.X9.W0 94 /r
		XOP_Vpshlb_xmm_xmm_xmmm128,									// XOP.128.X9.W1 94 /r

		XOP_Vpshlw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 95 /r
		XOP_Vpshlw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 95 /r

		XOP_Vpshld_xmm_xmmm128_xmm,									// XOP.128.X9.W0 96 /r
		XOP_Vpshld_xmm_xmm_xmmm128,									// XOP.128.X9.W1 96 /r

		XOP_Vpshlq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 97 /r
		XOP_Vpshlq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 97 /r

		XOP_Vpshab_xmm_xmmm128_xmm,									// XOP.128.X9.W0 98 /r
		XOP_Vpshab_xmm_xmm_xmmm128,									// XOP.128.X9.W1 98 /r

		XOP_Vpshaw_xmm_xmmm128_xmm,									// XOP.128.X9.W0 99 /r
		XOP_Vpshaw_xmm_xmm_xmmm128,									// XOP.128.X9.W1 99 /r

		XOP_Vpshad_xmm_xmmm128_xmm,									// XOP.128.X9.W0 9A /r
		XOP_Vpshad_xmm_xmm_xmmm128,									// XOP.128.X9.W1 9A /r

		XOP_Vpshaq_xmm_xmmm128_xmm,									// XOP.128.X9.W0 9B /r
		XOP_Vpshaq_xmm_xmm_xmmm128,									// XOP.128.X9.W1 9B /r

		XOP_Vphaddbw_xmm_xmmm128,									// XOP.128.X9.W0 C1 /r

		XOP_Vphaddbd_xmm_xmmm128,									// XOP.128.X9.W0 C2 /r

		XOP_Vphaddbq_xmm_xmmm128,									// XOP.128.X9.W0 C3 /r

		XOP_Vphaddwd_xmm_xmmm128,									// XOP.128.X9.W0 C6 /r

		XOP_Vphaddwq_xmm_xmmm128,									// XOP.128.X9.W0 C7 /r

		XOP_Vphadddq_xmm_xmmm128,									// XOP.128.X9.W0 CB /r

		XOP_Vphaddubw_xmm_xmmm128,									// XOP.128.X9.W0 D1 /r

		XOP_Vphaddubd_xmm_xmmm128,									// XOP.128.X9.W0 D2 /r

		XOP_Vphaddubq_xmm_xmmm128,									// XOP.128.X9.W0 D3 /r

		XOP_Vphadduwd_xmm_xmmm128,									// XOP.128.X9.W0 D6 /r

		XOP_Vphadduwq_xmm_xmmm128,									// XOP.128.X9.W0 D7 /r

		XOP_Vphaddudq_xmm_xmmm128,									// XOP.128.X9.W0 DB /r

		XOP_Vphsubbw_xmm_xmmm128,									// XOP.128.X9.W0 E1 /r

		XOP_Vphsubwd_xmm_xmmm128,									// XOP.128.X9.W0 E2 /r

		XOP_Vphsubdq_xmm_xmmm128,									// XOP.128.X9.W0 E3 /r

		// XOPA opcodes

		XOP_Bextr_r32_rm32_imm32,									// XOP.L0.XA.W0 10 /r id
		XOP_Bextr_r64_rm64_imm32,									// XOP.L0.XA.W1 10 /r id

		XOP_Lwpins_r32_rm32_imm32,									// XOP.L0.XA.W0 12 /0 id
		XOP_Lwpins_r64_rm32_imm32,									// XOP.L0.XA.W1 12 /0 id
		XOP_Lwpval_r32_rm32_imm32,									// XOP.L0.XA.W0 12 /1 id
		XOP_Lwpval_r64_rm32_imm32,									// XOP.L0.XA.W1 12 /1 id

		// 3DNow! opcodes

		D3NOW_Pi2fw_mm_mmm64,										// 0F 0F /r 0C
		D3NOW_Pi2fd_mm_mmm64,										// 0F 0F /r 0D

		D3NOW_Pf2iw_mm_mmm64,										// 0F 0F /r 1C
		D3NOW_Pf2id_mm_mmm64,										// 0F 0F /r 1D

		D3NOW_Pfrcpv_mm_mmm64,										// 0F 0F /r 86
		D3NOW_Pfrsqrtv_mm_mmm64,									// 0F 0F /r 87

		D3NOW_Pfnacc_mm_mmm64,										// 0F 0F /r 8A
		D3NOW_Pfpnacc_mm_mmm64,										// 0F 0F /r 8E

		D3NOW_Pfcmpge_mm_mmm64,										// 0F 0F /r 90
		D3NOW_Pfmin_mm_mmm64,										// 0F 0F /r 94
		D3NOW_Pfrcp_mm_mmm64,										// 0F 0F /r 96
		D3NOW_Pfrsqrt_mm_mmm64,										// 0F 0F /r 97

		D3NOW_Pfsub_mm_mmm64,										// 0F 0F /r 9A
		D3NOW_Pfadd_mm_mmm64,										// 0F 0F /r 9E

		D3NOW_Pfcmpgt_mm_mmm64,										// 0F 0F /r A0
		D3NOW_Pfmax_mm_mmm64,										// 0F 0F /r A4
		D3NOW_Pfrcpit1_mm_mmm64,									// 0F 0F /r A6
		D3NOW_Pfrsqit1_mm_mmm64,									// 0F 0F /r A7

		D3NOW_Pfsubr_mm_mmm64,										// 0F 0F /r AA
		D3NOW_Pfacc_mm_mmm64,										// 0F 0F /r AE

		D3NOW_Pfcmpeq_mm_mmm64,										// 0F 0F /r B0
		D3NOW_Pfmul_mm_mmm64,										// 0F 0F /r B4
		D3NOW_Pfrcpit2_mm_mmm64,									// 0F 0F /r B6
		D3NOW_Pmulhrw_mm_mmm64,										// 0F 0F /r B7

		D3NOW_Pswapd_mm_mmm64,										// 0F 0F /r BB
		D3NOW_Pavgusb_mm_mmm64,										// 0F 0F /r BF

		// Misc

		/// <summary>
		/// A 'db' asm directive that can store 1-16 bytes
		/// </summary>
		DeclareByte,												// <db>
		/// <summary>
		/// A 'dw' asm directive that can store 1-8 words
		/// </summary>
		DeclareWord,												// <dw>
		/// <summary>
		/// A 'dd' asm directive that can store 1-4 dwords
		/// </summary>
		DeclareDword,												// <dd>
		/// <summary>
		/// A 'dq' asm directive that can store 1-2 qwords
		/// </summary>
		DeclareQword,												// <dq>
#pragma warning restore 1591 // Missing XML comment for publicly visible type or member
	}
}
