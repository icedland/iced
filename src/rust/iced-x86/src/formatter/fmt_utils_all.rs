// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::{Code, CodeSize, Instruction, Register};

#[must_use]
pub(super) const fn is_rep_repe_repne_instruction(code: Code) -> bool {
	matches!(
		code,
		Code::Insb_m8_DX
			| Code::Insw_m16_DX
			| Code::Insd_m32_DX
			| Code::Outsb_DX_m8
			| Code::Outsw_DX_m16
			| Code::Outsd_DX_m32
			| Code::Movsb_m8_m8
			| Code::Movsw_m16_m16
			| Code::Movsd_m32_m32
			| Code::Movsq_m64_m64
			| Code::Cmpsb_m8_m8
			| Code::Cmpsw_m16_m16
			| Code::Cmpsd_m32_m32
			| Code::Cmpsq_m64_m64
			| Code::Stosb_m8_AL
			| Code::Stosw_m16_AX
			| Code::Stosd_m32_EAX
			| Code::Stosq_m64_RAX
			| Code::Lodsb_AL_m8
			| Code::Lodsw_AX_m16
			| Code::Lodsd_EAX_m32
			| Code::Lodsq_RAX_m64
			| Code::Scasb_AL_m8
			| Code::Scasw_AX_m16
			| Code::Scasd_EAX_m32
			| Code::Scasq_RAX_m64
			| Code::Montmul_16
			| Code::Montmul_32
			| Code::Montmul_64
			| Code::Xsha1_16
			| Code::Xsha1_32
			| Code::Xsha1_64
			| Code::Xsha256_16
			| Code::Xsha256_32
			| Code::Xsha256_64
			| Code::Xstore_16
			| Code::Xstore_32
			| Code::Xstore_64
			| Code::Xcryptecb_16
			| Code::Xcryptecb_32
			| Code::Xcryptecb_64
			| Code::Xcryptcbc_16
			| Code::Xcryptcbc_32
			| Code::Xcryptcbc_64
			| Code::Xcryptctr_16
			| Code::Xcryptctr_32
			| Code::Xcryptctr_64
			| Code::Xcryptcfb_16
			| Code::Xcryptcfb_32
			| Code::Xcryptcfb_64
			| Code::Xcryptofb_16
			| Code::Xcryptofb_32
			| Code::Xcryptofb_64
			| Code::Ccs_hash_16
			| Code::Ccs_hash_32
			| Code::Ccs_hash_64
			| Code::Ccs_encrypt_16
			| Code::Ccs_encrypt_32
			| Code::Ccs_encrypt_64
			| Code::Via_undoc_F30FA6F0_16
			| Code::Via_undoc_F30FA6F0_32
			| Code::Via_undoc_F30FA6F0_64
			| Code::Via_undoc_F30FA6F8_16
			| Code::Via_undoc_F30FA6F8_32
			| Code::Via_undoc_F30FA6F8_64
			| Code::Xsha512_16
			| Code::Xsha512_32
			| Code::Xsha512_64
			| Code::Xstore_alt_16
			| Code::Xstore_alt_32
			| Code::Xstore_alt_64
			| Code::Xsha512_alt_16
			| Code::Xsha512_alt_32
			| Code::Xsha512_alt_64
	)
}

#[must_use]
pub(super) const fn show_rep_or_repe_prefix_bool(code: Code, show_useless_prefixes: bool) -> bool {
	if show_useless_prefixes || is_rep_repe_repne_instruction(code) {
		true
	} else {
		// We allow 'rep ret' too since some old code use it to work around an old AMD bug
		match code {
			Code::Retnw | Code::Retnd | Code::Retnq => true,
			_ => show_useless_prefixes,
		}
	}
}

#[must_use]
pub(super) const fn show_repne_prefix_bool(code: Code, show_useless_prefixes: bool) -> bool {
	// If it's a 'rep/repne' instruction, always show the prefix
	if show_useless_prefixes || is_rep_repe_repne_instruction(code) {
		true
	} else {
		show_useless_prefixes
	}
}

#[must_use]
pub(super) fn is_code64(code_size: CodeSize) -> bool {
	code_size == CodeSize::Code64 || code_size == CodeSize::Unknown
}

#[must_use]
pub(super) fn get_default_segment_register(instruction: &Instruction) -> Register {
	let base_reg = instruction.memory_base();
	if base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP || base_reg == Register::RBP || base_reg == Register::RSP {
		Register::SS
	} else {
		Register::DS
	}
}

#[must_use]
pub(super) fn show_segment_prefix_bool(mut default_seg_reg: Register, instruction: &Instruction, show_useless_prefixes: bool) -> bool {
	if instruction.code().ignores_segment() {
		return show_useless_prefixes;
	}
	let prefix_seg = instruction.segment_prefix();
	debug_assert_ne!(prefix_seg, Register::None);
	if is_code64(instruction.code_size()) {
		// ES,CS,SS,DS are ignored
		if prefix_seg == Register::FS || prefix_seg == Register::GS {
			true
		} else {
			show_useless_prefixes
		}
	} else {
		if default_seg_reg == Register::None {
			default_seg_reg = get_default_segment_register(instruction);
		}
		if prefix_seg != default_seg_reg {
			true
		} else {
			show_useless_prefixes
		}
	}
}

#[must_use]
pub(super) const fn is_repe_or_repne_instruction(code: Code) -> bool {
	matches!(
		code,
		Code::Cmpsb_m8_m8
			| Code::Cmpsw_m16_m16
			| Code::Cmpsd_m32_m32
			| Code::Cmpsq_m64_m64
			| Code::Scasb_AL_m8
			| Code::Scasw_AX_m16
			| Code::Scasd_EAX_m32
			| Code::Scasq_RAX_m64
	)
}

#[must_use]
#[inline]
pub(super) const fn is_notrack_prefix_branch(code: Code) -> bool {
	const _: () = assert!(Code::Jmp_rm16 as u32 + 1 == Code::Jmp_rm32 as u32);
	const _: () = assert!(Code::Jmp_rm16 as u32 + 2 == Code::Jmp_rm64 as u32);
	const _: () = assert!(Code::Call_rm16 as u32 + 1 == Code::Call_rm32 as u32);
	const _: () = assert!(Code::Call_rm16 as u32 + 2 == Code::Call_rm64 as u32);
	(code as u32).wrapping_sub(Code::Jmp_rm16 as u32) <= 2 || (code as u32).wrapping_sub(Code::Call_rm16 as u32) <= 2
}
