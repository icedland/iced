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

use super::super::iced_constants::IcedConstants;
use super::super::*;
use super::mnemonic_str_tbl::TO_MNEMONIC_STR;
use super::op_code::OpCodeInfo;
use std::fmt::Write;
use std::{char, mem};

pub(crate) struct InstructionFormatter<'a, 'b> {
	op_code: &'a OpCodeInfo,
	sb: &'b mut String,
	r32_count: u32,
	r64_count: u32,
	bnd_count: u32,
	start_op_index: u32,
	r32_index: u32,
	r64_index: u32,
	bnd_index: u32,
	k_index: u32,
	vec_index: u32,
	op_count: u32,
	// true: k2 {k1}, false: k1 {k2}
	op_mask_is_k1: bool,
	no_vec_index: bool,
	swap_vec_index_12: bool,
}

impl<'a, 'b> InstructionFormatter<'a, 'b> {
	pub(crate) fn new(op_code: &'a OpCodeInfo, sb: &'b mut String) -> InstructionFormatter<'a, 'b> {
		let mut no_vec_index = false;
		let mut swap_vec_index_12 = false;
		let mut start_op_index = 0;
		let mut bnd_count = 0;
		let mut r32_count = 0;
		let mut r64_count = 0;
		let r32_index = 0;
		let r64_index = 0;
		let k_index = 0;
		let mut vec_index = 0;
		let bnd_index = 0;
		let mut op_count = op_code.op_count();
		let mut op_mask_is_k1 = false;
		if (op_code.op0_kind() == OpCodeOperandKind::k_reg || op_code.op0_kind() == OpCodeOperandKind::kp1_reg) && op_code.op_count() > 2 {
			vec_index += 1;
		}
		match op_code.code() {
			Code::EVEX_Vfpclassps_k_k1_xmmm128b32_imm8
			| Code::EVEX_Vfpclassps_k_k1_ymmm256b32_imm8
			| Code::EVEX_Vfpclassps_k_k1_zmmm512b32_imm8
			| Code::EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8
			| Code::EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8
			| Code::EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8
			| Code::EVEX_Vfpclassss_k_k1_xmmm32_imm8
			| Code::EVEX_Vfpclasssd_k_k1_xmmm64_imm8
			| Code::EVEX_Vptestmb_k_k1_xmm_xmmm128
			| Code::EVEX_Vptestmb_k_k1_ymm_ymmm256
			| Code::EVEX_Vptestmb_k_k1_zmm_zmmm512
			| Code::EVEX_Vptestmw_k_k1_xmm_xmmm128
			| Code::EVEX_Vptestmw_k_k1_ymm_ymmm256
			| Code::EVEX_Vptestmw_k_k1_zmm_zmmm512
			| Code::EVEX_Vptestnmb_k_k1_xmm_xmmm128
			| Code::EVEX_Vptestnmb_k_k1_ymm_ymmm256
			| Code::EVEX_Vptestnmb_k_k1_zmm_zmmm512
			| Code::EVEX_Vptestnmw_k_k1_xmm_xmmm128
			| Code::EVEX_Vptestnmw_k_k1_ymm_ymmm256
			| Code::EVEX_Vptestnmw_k_k1_zmm_zmmm512
			| Code::EVEX_Vptestmd_k_k1_xmm_xmmm128b32
			| Code::EVEX_Vptestmd_k_k1_ymm_ymmm256b32
			| Code::EVEX_Vptestmd_k_k1_zmm_zmmm512b32
			| Code::EVEX_Vptestmq_k_k1_xmm_xmmm128b64
			| Code::EVEX_Vptestmq_k_k1_ymm_ymmm256b64
			| Code::EVEX_Vptestmq_k_k1_zmm_zmmm512b64
			| Code::EVEX_Vptestnmd_k_k1_xmm_xmmm128b32
			| Code::EVEX_Vptestnmd_k_k1_ymm_ymmm256b32
			| Code::EVEX_Vptestnmd_k_k1_zmm_zmmm512b32
			| Code::EVEX_Vptestnmq_k_k1_xmm_xmmm128b64
			| Code::EVEX_Vptestnmq_k_k1_ymm_ymmm256b64
			| Code::EVEX_Vptestnmq_k_k1_zmm_zmmm512b64 => op_mask_is_k1 = true,

			Code::VEX_Vpextrw_r32m16_xmm_imm8
			| Code::VEX_Vpextrw_r64m16_xmm_imm8
			| Code::EVEX_Vpextrw_r32m16_xmm_imm8
			| Code::EVEX_Vpextrw_r64m16_xmm_imm8
			| Code::VEX_Vmovmskpd_r32_xmm
			| Code::VEX_Vmovmskpd_r64_xmm
			| Code::VEX_Vmovmskpd_r32_ymm
			| Code::VEX_Vmovmskpd_r64_ymm
			| Code::VEX_Vmovmskps_r32_xmm
			| Code::VEX_Vmovmskps_r64_xmm
			| Code::VEX_Vmovmskps_r32_ymm
			| Code::VEX_Vmovmskps_r64_ymm
			| Code::Pextrb_r32m8_xmm_imm8
			| Code::Pextrb_r64m8_xmm_imm8
			| Code::Pextrd_rm32_xmm_imm8
			| Code::Pextrq_rm64_xmm_imm8
			| Code::VEX_Vpextrb_r32m8_xmm_imm8
			| Code::VEX_Vpextrb_r64m8_xmm_imm8
			| Code::VEX_Vpextrd_rm32_xmm_imm8
			| Code::VEX_Vpextrq_rm64_xmm_imm8
			| Code::EVEX_Vpextrb_r32m8_xmm_imm8
			| Code::EVEX_Vpextrb_r64m8_xmm_imm8
			| Code::EVEX_Vpextrd_rm32_xmm_imm8
			| Code::EVEX_Vpextrq_rm64_xmm_imm8 => vec_index += 1,

			Code::Pxor_mm_mmm64
			| Code::Punpckldq_mm_mmm32
			| Code::Punpcklwd_mm_mmm32
			| Code::Punpcklbw_mm_mmm32
			| Code::Punpckhdq_mm_mmm64
			| Code::Punpckhwd_mm_mmm64
			| Code::Punpckhbw_mm_mmm64
			| Code::Psubusb_mm_mmm64
			| Code::Psubusw_mm_mmm64
			| Code::Psubsw_mm_mmm64
			| Code::Psubsb_mm_mmm64
			| Code::Psubd_mm_mmm64
			| Code::Psubw_mm_mmm64
			| Code::Psubb_mm_mmm64
			| Code::Psrlq_mm_imm8
			| Code::Psrlq_mm_mmm64
			| Code::Psrld_mm_imm8
			| Code::Psrld_mm_mmm64
			| Code::Psrlw_mm_imm8
			| Code::Psrlw_mm_mmm64
			| Code::Psrad_mm_imm8
			| Code::Psrad_mm_mmm64
			| Code::Psraw_mm_imm8
			| Code::Psraw_mm_mmm64
			| Code::Psllq_mm_imm8
			| Code::Psllq_mm_mmm64
			| Code::Pslld_mm_imm8
			| Code::Pslld_mm_mmm64
			| Code::Psllw_mm_mmm64
			| Code::Por_mm_mmm64
			| Code::Pmullw_mm_mmm64
			| Code::Pmulhw_mm_mmm64
			| Code::Pmovmskb_r32_mm
			| Code::Pmovmskb_r64_mm
			| Code::Pmovmskb_r32_xmm
			| Code::Pmovmskb_r64_xmm
			| Code::Pmaddwd_mm_mmm64
			| Code::Pinsrw_mm_r32m16_imm8
			| Code::Pinsrw_mm_r64m16_imm8
			| Code::Pinsrw_xmm_r32m16_imm8
			| Code::Pinsrw_xmm_r64m16_imm8
			| Code::Pextrw_r32_xmm_imm8
			| Code::Pextrw_r64_xmm_imm8
			| Code::Pextrw_r32m16_xmm_imm8
			| Code::Pextrw_r64m16_xmm_imm8
			| Code::Pextrw_r32_mm_imm8
			| Code::Pextrw_r64_mm_imm8
			| Code::Cvtpd2pi_mm_xmmm128
			| Code::Cvtpi2pd_xmm_mmm64
			| Code::Cvtpi2ps_xmm_mmm64
			| Code::Cvtps2pi_mm_xmmm64
			| Code::Cvttpd2pi_mm_xmmm128
			| Code::Cvttps2pi_mm_xmmm64
			| Code::Movd_mm_rm32
			| Code::Movq_mm_rm64
			| Code::Movd_rm32_mm
			| Code::Movq_rm64_mm
			| Code::Movd_xmm_rm32
			| Code::Movq_xmm_rm64
			| Code::Movd_rm32_xmm
			| Code::Movq_rm64_xmm
			| Code::Movdq2q_mm_xmm
			| Code::Movmskpd_r32_xmm
			| Code::Movmskpd_r64_xmm
			| Code::Movmskps_r32_xmm
			| Code::Movmskps_r64_xmm
			| Code::Movntq_m64_mm
			| Code::Movq_mm_mmm64
			| Code::Movq_mmm64_mm
			| Code::Movq2dq_xmm_mm
			| Code::Packuswb_mm_mmm64
			| Code::Paddb_mm_mmm64
			| Code::Paddw_mm_mmm64
			| Code::Paddd_mm_mmm64
			| Code::Paddq_mm_mmm64
			| Code::Paddsb_mm_mmm64
			| Code::Paddsw_mm_mmm64
			| Code::Paddusb_mm_mmm64
			| Code::Paddusw_mm_mmm64
			| Code::Pand_mm_mmm64
			| Code::Pandn_mm_mmm64
			| Code::Pcmpeqb_mm_mmm64
			| Code::Pcmpeqw_mm_mmm64
			| Code::Pcmpeqd_mm_mmm64
			| Code::Pcmpgtb_mm_mmm64
			| Code::Pcmpgtw_mm_mmm64
			| Code::Pcmpgtd_mm_mmm64 => no_vec_index = true,

			Code::Movapd_xmmm128_xmm
			| Code::VEX_Vmovapd_xmmm128_xmm
			| Code::VEX_Vmovapd_ymmm256_ymm
			| Code::EVEX_Vmovapd_xmmm128_k1z_xmm
			| Code::EVEX_Vmovapd_ymmm256_k1z_ymm
			| Code::EVEX_Vmovapd_zmmm512_k1z_zmm
			| Code::Movaps_xmmm128_xmm
			| Code::VEX_Vmovaps_xmmm128_xmm
			| Code::VEX_Vmovaps_ymmm256_ymm
			| Code::EVEX_Vmovaps_xmmm128_k1z_xmm
			| Code::EVEX_Vmovaps_ymmm256_k1z_ymm
			| Code::EVEX_Vmovaps_zmmm512_k1z_zmm
			| Code::Movdqa_xmmm128_xmm
			| Code::VEX_Vmovdqa_xmmm128_xmm
			| Code::VEX_Vmovdqa_ymmm256_ymm
			| Code::EVEX_Vmovdqa32_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqa32_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqa32_zmmm512_k1z_zmm
			| Code::EVEX_Vmovdqa64_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqa64_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqa64_zmmm512_k1z_zmm
			| Code::Movdqu_xmmm128_xmm
			| Code::VEX_Vmovdqu_xmmm128_xmm
			| Code::VEX_Vmovdqu_ymmm256_ymm
			| Code::EVEX_Vmovdqu8_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqu8_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqu8_zmmm512_k1z_zmm
			| Code::EVEX_Vmovdqu16_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqu16_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqu16_zmmm512_k1z_zmm
			| Code::EVEX_Vmovdqu32_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqu32_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqu32_zmmm512_k1z_zmm
			| Code::EVEX_Vmovdqu64_xmmm128_k1z_xmm
			| Code::EVEX_Vmovdqu64_ymmm256_k1z_ymm
			| Code::EVEX_Vmovdqu64_zmmm512_k1z_zmm
			| Code::VEX_Vmovhpd_xmm_xmm_m64
			| Code::EVEX_Vmovhpd_xmm_xmm_m64
			| Code::VEX_Vmovhps_xmm_xmm_m64
			| Code::EVEX_Vmovhps_xmm_xmm_m64
			| Code::VEX_Vmovlpd_xmm_xmm_m64
			| Code::EVEX_Vmovlpd_xmm_xmm_m64
			| Code::VEX_Vmovlps_xmm_xmm_m64
			| Code::EVEX_Vmovlps_xmm_xmm_m64
			| Code::Movq_xmmm64_xmm
			| Code::Movss_xmmm32_xmm
			| Code::Movupd_xmmm128_xmm
			| Code::VEX_Vmovupd_xmmm128_xmm
			| Code::VEX_Vmovupd_ymmm256_ymm
			| Code::EVEX_Vmovupd_xmmm128_k1z_xmm
			| Code::EVEX_Vmovupd_ymmm256_k1z_ymm
			| Code::EVEX_Vmovupd_zmmm512_k1z_zmm
			| Code::Movups_xmmm128_xmm
			| Code::VEX_Vmovups_xmmm128_xmm
			| Code::VEX_Vmovups_ymmm256_ymm
			| Code::EVEX_Vmovups_xmmm128_k1z_xmm
			| Code::EVEX_Vmovups_ymmm256_k1z_ymm
			| Code::EVEX_Vmovups_zmmm512_k1z_zmm => swap_vec_index_12 = true,

			_ => {}
		}
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::r32_reg | OpCodeOperandKind::r32_rm | OpCodeOperandKind::r32_opcode | OpCodeOperandKind::r32_vvvv => {
					r32_count += 1
				}

				OpCodeOperandKind::r64_reg | OpCodeOperandKind::r64_rm | OpCodeOperandKind::r64_opcode | OpCodeOperandKind::r64_vvvv => {
					r64_count += 1
				}

				OpCodeOperandKind::bnd_or_mem_mpx | OpCodeOperandKind::bnd_reg => bnd_count += 1,

				OpCodeOperandKind::st0 => {
					if i == 0 {
						match op_code.code() {
							Code::Fcom_st0_sti
							| Code::Fcom_st0_sti_DCD0
							| Code::Fcomp_st0_sti
							| Code::Fcomp_st0_sti_DCD8
							| Code::Fcomp_st0_sti_DED0
							| Code::Fld_st0_sti
							| Code::Fucom_st0_sti
							| Code::Fucomp_st0_sti
							| Code::Fxch_st0_sti
							| Code::Fxch_st0_sti_DDC8
							| Code::Fxch_st0_sti_DFC8 => start_op_index = 1,
							_ => {}
						}
					}
				}

				OpCodeOperandKind::None
				| OpCodeOperandKind::farbr2_2
				| OpCodeOperandKind::farbr4_2
				| OpCodeOperandKind::mem_offs
				| OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64z
				| OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::r8_reg
				| OpCodeOperandKind::r8_opcode
				| OpCodeOperandKind::r16_reg
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r16_opcode
				| OpCodeOperandKind::seg_reg
				| OpCodeOperandKind::k_reg
				| OpCodeOperandKind::kp1_reg
				| OpCodeOperandKind::k_rm
				| OpCodeOperandKind::k_vvvv
				| OpCodeOperandKind::mm_reg
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::xmm_reg
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::xmm_vvvv
				| OpCodeOperandKind::xmmp3_vvvv
				| OpCodeOperandKind::xmm_is4
				| OpCodeOperandKind::xmm_is5
				| OpCodeOperandKind::ymm_reg
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::ymm_vvvv
				| OpCodeOperandKind::ymm_is4
				| OpCodeOperandKind::ymm_is5
				| OpCodeOperandKind::zmm_reg
				| OpCodeOperandKind::zmm_rm
				| OpCodeOperandKind::zmm_vvvv
				| OpCodeOperandKind::zmmp3_vvvv
				| OpCodeOperandKind::cr_reg
				| OpCodeOperandKind::dr_reg
				| OpCodeOperandKind::tr_reg
				| OpCodeOperandKind::es
				| OpCodeOperandKind::cs
				| OpCodeOperandKind::ss
				| OpCodeOperandKind::ds
				| OpCodeOperandKind::fs
				| OpCodeOperandKind::gs
				| OpCodeOperandKind::al
				| OpCodeOperandKind::cl
				| OpCodeOperandKind::ax
				| OpCodeOperandKind::dx
				| OpCodeOperandKind::eax
				| OpCodeOperandKind::rax
				| OpCodeOperandKind::sti_opcode
				| OpCodeOperandKind::imm2_m2z
				| OpCodeOperandKind::imm8
				| OpCodeOperandKind::imm8_const_1
				| OpCodeOperandKind::imm8sex16
				| OpCodeOperandKind::imm8sex32
				| OpCodeOperandKind::imm8sex64
				| OpCodeOperandKind::imm16
				| OpCodeOperandKind::imm32
				| OpCodeOperandKind::imm32sex64
				| OpCodeOperandKind::imm64
				| OpCodeOperandKind::seg_rDI
				| OpCodeOperandKind::br16_1
				| OpCodeOperandKind::br32_1
				| OpCodeOperandKind::br64_1
				| OpCodeOperandKind::br16_2
				| OpCodeOperandKind::br32_4
				| OpCodeOperandKind::br64_4
				| OpCodeOperandKind::xbegin_2
				| OpCodeOperandKind::xbegin_4
				| OpCodeOperandKind::brdisp_2
				| OpCodeOperandKind::brdisp_4 => {}

				OpCodeOperandKind::seg_rSI | OpCodeOperandKind::es_rDI | OpCodeOperandKind::seg_rBX_al => {
					// string instructions, xlat
					op_count = 0;
				}
			}
		}

		Self {
			op_code,
			sb,
			r32_count,
			r64_count,
			bnd_count,
			start_op_index,
			r32_index,
			r64_index,
			bnd_index,
			k_index,
			vec_index,
			op_count,
			op_mask_is_k1,
			no_vec_index,
			swap_vec_index_12,
		}
	}

	fn get_k_index(&mut self) -> u32 {
		self.k_index += 1;
		if self.op_mask_is_k1 {
			if self.k_index == 1 {
				return 2;
			}
			if self.k_index == 2 {
				return 1;
			}
		}
		self.k_index
	}

	fn get_bnd_index(&mut self) -> u32 {
		if self.bnd_count <= 1 {
			0
		} else {
			self.bnd_index += 1;
			self.bnd_index
		}
	}

	fn get_vec_index(&mut self) -> u32 {
		if self.no_vec_index {
			return 0;
		}
		self.vec_index += 1;
		if self.swap_vec_index_12 {
			if self.vec_index == 1 {
				return 2;
			}
			if self.vec_index == 2 {
				return 1;
			}
		}
		self.vec_index
	}

	fn get_memory_size(&self, is_broadcast: bool) -> MemorySize {
		let mut index = self.op_code.code() as usize;
		if is_broadcast {
			index += IcedConstants::NUMBER_OF_CODE_VALUES as usize;
		}
		unsafe { mem::transmute(*instruction_memory_sizes::SIZES.get_unchecked(index)) }
	}

	pub(crate) fn format(&mut self) -> String {
		if !self.op_code.is_instruction() {
			match self.op_code.code() {
				Code::INVALID => return "<invalid>".to_owned(),
				Code::DeclareByte => return "<db>".to_owned(),
				Code::DeclareWord => return "<dw>".to_owned(),
				Code::DeclareDword => return "<dd>".to_owned(),
				Code::DeclareQword => return "<dq>".to_owned(),
				_ => panic!(),
			}
		}

		self.sb.clear();

		// Temp needed if rustc < 1.36.0 (2015 edition)
		let tmp_mnemonic = self.get_mnemonic();
		self.write(tmp_mnemonic, true);
		if self.start_op_index < self.op_count {
			self.sb.push(' ');
			let mut sae_er_index = self.op_count - 1;
			if self.op_code.encoding() != EncodingKind::Legacy && self.op_code.op_kind(sae_er_index) == OpCodeOperandKind::imm8 {
				sae_er_index -= 1;
			}
			let mut add_comma = false;
			for i in self.start_op_index..self.op_count {
				let mut tmp;
				let tmp2;
				if add_comma {
					self.write_op_separator();
				}
				add_comma = true;

				let op_kind = self.op_code.op_kind(i);
				match op_kind {
					OpCodeOperandKind::farbr2_2 => self.sb.push_str("ptr16:16"),
					OpCodeOperandKind::farbr4_2 => self.sb.push_str("ptr16:32"),

					OpCodeOperandKind::mem_offs => {
						self.sb.push_str("moffs");
						// Temp needed if rustc < 1.36.0 (2015 edition)
						let tmp_mem_size = self.get_memory_size(false);
						self.write_memory_size(tmp_mem_size);
					}

					OpCodeOperandKind::mem | OpCodeOperandKind::mem_mpx => self.write_memory(),
					OpCodeOperandKind::mem_mib => self.sb.push_str("mib"),

					OpCodeOperandKind::mem_vsib32x => self.sb.push_str("vm32x"),
					OpCodeOperandKind::mem_vsib64x => self.sb.push_str("vm64x"),
					OpCodeOperandKind::mem_vsib32y => self.sb.push_str("vm32y"),
					OpCodeOperandKind::mem_vsib64y => self.sb.push_str("vm64y"),
					OpCodeOperandKind::mem_vsib32z => self.sb.push_str("vm32z"),
					OpCodeOperandKind::mem_vsib64z => self.sb.push_str("vm64z"),
					OpCodeOperandKind::r8_or_mem => self.write_gpr_mem(8),
					OpCodeOperandKind::r16_or_mem => self.write_gpr_mem(16),
					OpCodeOperandKind::r32_or_mem | OpCodeOperandKind::r32_or_mem_mpx => self.write_gpr_mem(32),
					OpCodeOperandKind::r64_or_mem | OpCodeOperandKind::r64_or_mem_mpx => self.write_gpr_mem(64),

					OpCodeOperandKind::mm_or_mem => {
						tmp = self.get_vec_index();
						self.write_reg_mem("mm", tmp);
					}

					OpCodeOperandKind::xmm_or_mem => {
						tmp = self.get_vec_index();
						self.write_reg_mem("xmm", tmp);
					}

					OpCodeOperandKind::ymm_or_mem => {
						tmp = self.get_vec_index();
						self.write_reg_mem("ymm", tmp);
					}

					OpCodeOperandKind::zmm_or_mem => {
						tmp = self.get_vec_index();
						self.write_reg_mem("zmm", tmp);
					}

					OpCodeOperandKind::bnd_or_mem_mpx => {
						tmp = self.get_bnd_index();
						self.write_reg_op2("bnd", tmp);
						self.sb.push('/');
						self.write_memory();
					}

					OpCodeOperandKind::k_or_mem => {
						tmp = self.get_k_index();
						self.write_reg_mem("k", tmp);
					}

					OpCodeOperandKind::r8_reg | OpCodeOperandKind::r8_opcode => self.write_reg_op1("r8"),
					OpCodeOperandKind::r16_reg | OpCodeOperandKind::r16_rm | OpCodeOperandKind::r16_opcode => self.write_reg_op1("r16"),

					OpCodeOperandKind::r32_reg | OpCodeOperandKind::r32_rm | OpCodeOperandKind::r32_opcode | OpCodeOperandKind::r32_vvvv => {
						self.write_reg_op1("r32");
						tmp2 = self.r32_count;
						tmp = self.r32_index;
						self.append_gpr_suffix(tmp2, &mut tmp);
						self.r32_index = tmp;
					}

					OpCodeOperandKind::r64_reg | OpCodeOperandKind::r64_rm | OpCodeOperandKind::r64_opcode | OpCodeOperandKind::r64_vvvv => {
						self.write_reg_op1("r64");
						tmp2 = self.r64_count;
						tmp = self.r64_index;
						self.append_gpr_suffix(tmp2, &mut tmp);
						self.r64_index = tmp;
					}

					OpCodeOperandKind::seg_reg => self.sb.push_str("Sreg"),
					OpCodeOperandKind::k_reg | OpCodeOperandKind::k_rm | OpCodeOperandKind::k_vvvv => {
						tmp = self.get_k_index();
						self.write_reg_op2("k", tmp);
					}

					OpCodeOperandKind::kp1_reg => {
						tmp = self.get_k_index();
						self.write_reg_op2("k", tmp);
						self.sb.push_str("+1");
					}

					OpCodeOperandKind::mm_reg | OpCodeOperandKind::mm_rm => {
						tmp = self.get_vec_index();
						self.write_reg_op2("mm", tmp);
					}

					OpCodeOperandKind::xmm_reg
					| OpCodeOperandKind::xmm_rm
					| OpCodeOperandKind::xmm_vvvv
					| OpCodeOperandKind::xmm_is4
					| OpCodeOperandKind::xmm_is5 => {
						tmp = self.get_vec_index();
						self.write_reg_op2("xmm", tmp);
					}

					OpCodeOperandKind::xmmp3_vvvv => {
						tmp = self.get_vec_index();
						self.write_reg_op2("xmm", tmp);
						self.sb.push_str("+3");
					}

					OpCodeOperandKind::ymm_reg
					| OpCodeOperandKind::ymm_rm
					| OpCodeOperandKind::ymm_vvvv
					| OpCodeOperandKind::ymm_is4
					| OpCodeOperandKind::ymm_is5 => {
						tmp = self.get_vec_index();
						self.write_reg_op2("ymm", tmp);
					}

					OpCodeOperandKind::zmm_reg | OpCodeOperandKind::zmm_rm | OpCodeOperandKind::zmm_vvvv => {
						tmp = self.get_vec_index();
						self.write_reg_op2("zmm", tmp);
					}

					OpCodeOperandKind::zmmp3_vvvv => {
						tmp = self.get_vec_index();
						self.write_reg_op2("zmm", tmp);
						self.sb.push_str("+3");
					}

					OpCodeOperandKind::bnd_reg => {
						tmp = self.get_bnd_index();
						self.write_reg_op2("bnd", tmp);
					}

					OpCodeOperandKind::cr_reg => self.write_reg_op1("cr"),
					OpCodeOperandKind::dr_reg => self.write_reg_op1("dr"),
					OpCodeOperandKind::tr_reg => self.write_reg_op1("tr"),
					OpCodeOperandKind::es => self.write_register("es"),
					OpCodeOperandKind::cs => self.write_register("cs"),
					OpCodeOperandKind::ss => self.write_register("ss"),
					OpCodeOperandKind::ds => self.write_register("ds"),
					OpCodeOperandKind::fs => self.write_register("fs"),
					OpCodeOperandKind::gs => self.write_register("gs"),
					OpCodeOperandKind::al => self.write_register("al"),
					OpCodeOperandKind::cl => self.write_register("cl"),
					OpCodeOperandKind::ax => self.write_register("ax"),
					OpCodeOperandKind::dx => self.write_register("dx"),
					OpCodeOperandKind::eax => self.write_register("eax"),
					OpCodeOperandKind::rax => self.write_register("rax"),

					OpCodeOperandKind::st0 | OpCodeOperandKind::sti_opcode => {
						self.write_register("ST");
						if i == 0
							&& (self.op_code.code() == Code::Fcomi_st0_sti
								|| self.op_code.code() == Code::Fcomip_st0_sti
								|| self.op_code.code() == Code::Fucomi_st0_sti
								|| self.op_code.code() == Code::Fucomip_st0_sti)
						{
							// nothing, it should be ST and not ST(0)
						} else if op_kind == OpCodeOperandKind::st0 {
							self.sb.push_str("(0)");
						} else {
							debug_assert_eq!(OpCodeOperandKind::sti_opcode, op_kind);
							self.sb.push_str("(i)");
						}
					}

					OpCodeOperandKind::imm2_m2z => self.sb.push_str("imm2"),

					OpCodeOperandKind::imm8 | OpCodeOperandKind::imm8sex16 | OpCodeOperandKind::imm8sex32 | OpCodeOperandKind::imm8sex64 => {
						self.sb.push_str("imm8")
					}
					OpCodeOperandKind::imm8_const_1 => self.sb.push_str("1"),
					OpCodeOperandKind::imm16 => self.sb.push_str("imm16"),
					OpCodeOperandKind::imm32 | OpCodeOperandKind::imm32sex64 => self.sb.push_str("imm32"),
					OpCodeOperandKind::imm64 => self.sb.push_str("imm64"),

					OpCodeOperandKind::seg_rSI | OpCodeOperandKind::es_rDI | OpCodeOperandKind::seg_rDI | OpCodeOperandKind::seg_rBX_al => {
						add_comma = false
					}

					OpCodeOperandKind::br16_1 | OpCodeOperandKind::br32_1 | OpCodeOperandKind::br64_1 => self.sb.push_str("rel8"),
					OpCodeOperandKind::br16_2 | OpCodeOperandKind::xbegin_2 => self.sb.push_str("rel16"),
					OpCodeOperandKind::br32_4 | OpCodeOperandKind::br64_4 | OpCodeOperandKind::xbegin_4 => self.sb.push_str("rel32"),
					OpCodeOperandKind::brdisp_2 => self.sb.push_str("disp16"),
					OpCodeOperandKind::brdisp_4 => self.sb.push_str("disp32"),
					OpCodeOperandKind::None => panic!(),
				}

				if i == 0 {
					if self.op_code.can_use_op_mask_register() {
						self.sb.push(' ');
						tmp = self.get_k_index();
						self.write_reg_decorator("k", tmp);
						if self.op_code.can_use_zeroing_masking() {
							self.write_decorator("z");
						}
					}
				}
				if i == sae_er_index {
					if self.op_code.can_suppress_all_exceptions() {
						self.write_decorator("sae");
					}
					if self.op_code.can_use_rounding_control() {
						if self.op_code.code() != Code::EVEX_Vcvtusi2sd_xmm_xmm_rm32_er && self.op_code.code() != Code::EVEX_Vcvtsi2sd_xmm_xmm_rm32_er
						{
							self.write_decorator("er");
						}
					}
				}
			}
		}

		match self.op_code.code() {
			Code::Blendvpd_xmm_xmmm128 | Code::Blendvps_xmm_xmmm128 | Code::Pblendvb_xmm_xmmm128 | Code::Sha256rnds2_xmm_xmmm128 => {
				self.write_op_separator();
				self.write("<XMM0>", true);
			}

			Code::Tpause_r32 | Code::Tpause_r64 | Code::Umwait_r32 | Code::Umwait_r64 => {
				self.write_op_separator();
				self.write("<edx>", false);
				self.write_op_separator();
				self.write("<eax>", false);
			}

			_ => {}
		}

		self.sb.to_owned()
	}

	fn write_memory_size(&mut self, memory_size: MemorySize) {
		match self.op_code.code() {
			Code::Fldcw_m2byte | Code::Fnstcw_m2byte | Code::Fstcw_m2byte | Code::Fnstsw_m2byte | Code::Fstsw_m2byte => {
				self.sb.push_str("2byte");
				return;
			}
			_ => {}
		}

		match memory_size {
			MemorySize::Bound16_WordWord => self.sb.push_str("16&16"),
			MemorySize::Bound32_DwordDword => self.sb.push_str("32&32"),
			MemorySize::FpuEnv14 => self.sb.push_str("14byte"),
			MemorySize::FpuEnv28 => self.sb.push_str("28byte"),
			MemorySize::FpuState94 => self.sb.push_str("94byte"),
			MemorySize::FpuState108 => self.sb.push_str("108byte"),
			MemorySize::Fxsave_512Byte | MemorySize::Fxsave64_512Byte => self.sb.push_str("512byte"),
			MemorySize::Xsave | MemorySize::Xsave64 => self.sb.push_str("em"), // 'm' has already been appended
			MemorySize::SegPtr16 => self.sb.push_str("16:16"),
			MemorySize::SegPtr32 => self.sb.push_str("16:32"),
			MemorySize::SegPtr64 => self.sb.push_str("16:64"),

			MemorySize::Fword6 => {
				if !self.is_sgdt_or_sidt() {
					self.sb.push_str("16&32");
				}
			}

			MemorySize::Fword10 => {
				if !self.is_sgdt_or_sidt() {
					self.sb.push_str("16&64");
				}
			}

			_ => {
				let mem_size = memory_size.size();
				if mem_size != 0 {
					write!(self.sb, "{}", mem_size * 8).unwrap();
				}
			}
		}

		if Self::is_fpu_instruction(self.op_code.code()) {
			match memory_size {
				MemorySize::Int16 | MemorySize::Int32 | MemorySize::Int64 => self.sb.push_str("int"),
				MemorySize::Float32 | MemorySize::Float64 | MemorySize::Float80 => self.sb.push_str("fp"),
				MemorySize::Bcd => self.sb.push_str("bcd"),
				_ => {}
			}
		}
	}

	fn is_sgdt_or_sidt(&self) -> bool {
		match self.op_code.code() {
			Code::Sgdt_m1632_16 | Code::Sgdt_m1632 | Code::Sgdt_m1664 | Code::Sidt_m1632_16 | Code::Sidt_m1632 | Code::Sidt_m1664 => true,
			_ => false,
		}
	}

	fn write_register(&mut self, register: &str) {
		self.write(register, true);
	}

	fn write_reg_op1(&mut self, register: &str) {
		self.write(register, false);
	}

	fn write_reg_op2(&mut self, register: &str, index: u32) {
		self.write_reg_op1(register);
		if index > 0 {
			write!(self.sb, "{}", index).unwrap();
		}
	}

	fn write_decorator(&mut self, decorator: &str) {
		self.sb.push('{');
		self.write(decorator, false);
		self.sb.push('}');
	}

	fn write_reg_decorator(&mut self, register: &str, index: u32) {
		self.sb.push('{');
		self.write(register, false);
		write!(self.sb, "{}", index).unwrap();
		self.sb.push('}');
	}

	fn append_gpr_suffix(&mut self, count: u32, index: &mut u32) {
		if count <= 1 {
			return;
		}
		self.sb.push(char::from_u32('a' as u32 + *index).unwrap());
		*index += 1;
	}

	fn write_op_separator(&mut self) {
		self.sb.push_str(", ");
	}

	fn write(&mut self, s: &str, upper: bool) {
		if upper {
			for c in s.chars() {
				for uc in c.to_uppercase() {
					self.sb.push(uc);
				}
			}
		} else {
			for c in s.chars() {
				for uc in c.to_lowercase() {
					self.sb.push(uc);
				}
			}
		}
	}

	fn write_gpr_mem(&mut self, reg_size: u32) {
		debug_assert!(!self.op_code.can_broadcast());
		self.sb.push('r');
		let mem_size = self.get_memory_size(false).size() * 8;
		if mem_size != reg_size {
			write!(self.sb, "{}", reg_size).unwrap();
		}
		self.sb.push('/');
		self.write_memory();
	}

	fn write_reg_mem(&mut self, register: &str, index: u32) {
		self.write_reg_op2(register, index);
		self.sb.push('/');
		self.write_memory();
	}

	fn write_memory(&mut self) {
		self.write_memory1(false);
		if self.op_code.can_broadcast() {
			self.sb.push('/');
			self.write_memory1(true);
		}
	}

	fn write_memory1(&mut self, is_broadcast: bool) {
		let memory_size = self.get_memory_size(is_broadcast);
		self.sb.push('m');
		self.write_memory_size(memory_size);
		if is_broadcast {
			self.sb.push_str("bcst");
		}
	}

	fn get_mnemonic(&self) -> &'static str {
		let code = self.op_code.code();
		match code {
			Code::Retfw | Code::Retfw_imm16 | Code::Retfd | Code::Retfd_imm16 | Code::Retfq | Code::Retfq_imm16 => "ret",
			Code::Iretd => "iretd",
			Code::Iretq => "iretq",
			Code::Pushad => "pushad",
			Code::Popad => "popad",
			Code::Pushfd => "pushfd",
			Code::Pushfq => "pushfq",
			Code::Popfd => "popfd",
			Code::Popfq => "popfq",
			Code::Int3 => "int3",
			_ => TO_MNEMONIC_STR[code.mnemonic() as usize],
		}
	}

	fn is_fpu_instruction(code: Code) -> bool {
		(code as u32).wrapping_sub(Code::Fadd_m32fp as u32) <= (Code::Fcomip_st0_sti as u32 - Code::Fadd_m32fp as u32)
	}
}
