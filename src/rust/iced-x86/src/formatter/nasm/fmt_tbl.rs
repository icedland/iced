// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::data_reader::DataReader;
use crate::formatter::nasm::enums::*;
use crate::formatter::nasm::fmt_data::FORMATTER_TBL_DATA;
use crate::formatter::nasm::info::*;
use crate::formatter::pseudo_ops::get_pseudo_ops;
use crate::formatter::strings_tbl::get_strings_table_ref;
use crate::iced_constants::IcedConstants;
use crate::{CodeSizeUnderlyingType, CodeUnderlyingType, MemorySizeUnderlyingType, RegisterUnderlyingType};
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use core::convert::TryInto;
use core::mem;
use lazy_static::lazy_static;

lazy_static! {
	pub(super) static ref ALL_INFOS: Box<[Box<dyn InstrInfo + Send + Sync>; IcedConstants::CODE_ENUM_COUNT]> = read();
}

fn add_suffix(s: &str, c: char) -> String {
	let mut res = String::with_capacity(s.len() + 1);
	res.push_str(s);
	res.push(c);
	res
}

fn read() -> Box<[Box<dyn InstrInfo + Send + Sync>; IcedConstants::CODE_ENUM_COUNT]> {
	let mut infos: Vec<Box<dyn InstrInfo + Send + Sync>> = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
	let mut reader = DataReader::new(FORMATTER_TBL_DATA);
	let strings = get_strings_table_ref();
	let mut prev_index = -1isize;
	for i in 0..IcedConstants::CODE_ENUM_COUNT {
		// SAFETY: generated (and immutable) data is valid
		let f = reader.read_u8();
		let mut ctor_kind: CtorKind = unsafe { mem::transmute((f & 0x7F) as u8) };
		let current_index;
		if ctor_kind == CtorKind::Previous {
			current_index = reader.index() as isize;
			reader.set_index(prev_index as usize);
			ctor_kind = unsafe { mem::transmute((reader.read_u8() & 0x7F) as u8) };
		} else {
			current_index = -1;
			prev_index = reader.index() as isize - 1;
		}
		let s = if (f & 0x80) != 0 {
			let s = strings[reader.read_compressed_u32() as usize];
			let mut res = String::with_capacity(s.len() + 1);
			res.push('v');
			res.push_str(s);
			res
		} else {
			String::from(strings[reader.read_compressed_u32() as usize])
		};

		let c;
		let v;
		let v2;
		let v3;
		let info: Box<dyn InstrInfo + Send + Sync> = match ctor_kind {
			CtorKind::Previous => unreachable!(),
			CtorKind::Normal_1 => Box::new(SimpleInstrInfo::with_mnemonic(s)),

			CtorKind::Normal_2 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo::new(s, v))
			}

			CtorKind::AamAad => Box::new(SimpleInstrInfo_AamAad::new(s)),

			CtorKind::asz => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_as::new(v, s))
			}

			CtorKind::String => Box::new(SimpleInstrInfo_String::new(s)),

			CtorKind::bcst => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_bcst::new(s, v))
			}

			CtorKind::bnd => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_bnd::new(s, v))
			}

			CtorKind::DeclareData => Box::new(SimpleInstrInfo_DeclareData::new(unsafe { mem::transmute(i as CodeUnderlyingType) }, s)),

			CtorKind::er_2 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_er::with_mnemonic(v, s))
			}

			CtorKind::er_3 => {
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_er::new(v, s, v2))
			}

			CtorKind::far => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_far::new(v, s))
			}

			CtorKind::far_mem => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_far_mem::new(v, s))
			}

			CtorKind::invlpga => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_invlpga::new(v, s))
			}

			CtorKind::maskmovq => Box::new(SimpleInstrInfo_maskmovq::new(s)),
			CtorKind::movabs => Box::new(SimpleInstrInfo_movabs::new(s)),

			CtorKind::nop => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_nop::new(v, s, unsafe { mem::transmute(v2 as RegisterUnderlyingType) }))
			}

			CtorKind::OpSize => {
				v = reader.read_u8() as u32;
				let s2 = add_suffix(&s, 'w');
				let s3 = add_suffix(&s, 'd');
				let s4 = add_suffix(&s, 'q');
				Box::new(SimpleInstrInfo_OpSize::new(unsafe { mem::transmute(v as CodeSizeUnderlyingType) }, s, s2, s3, s4))
			}

			CtorKind::OpSize2_bnd => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s4 = String::from(strings[reader.read_compressed_u32() as usize]);
				Box::new(SimpleInstrInfo_OpSize2_bnd::new(s, s2, s3, s4))
			}

			CtorKind::OpSize3 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_OpSize3::new(v, s, s2))
			}

			CtorKind::os_2 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os::with_mnemonic(v, s))
			}

			CtorKind::os_3 => {
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os::new(v, s, v2))
			}

			CtorKind::os_call => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				debug_assert!(v2 <= 1);
				Box::new(SimpleInstrInfo_os_call::new(v, s, v2 != 0))
			}

			CtorKind::CC_1 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s]))
			}

			CtorKind::CC_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2]))
			}

			CtorKind::CC_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2, s3]))
			}

			CtorKind::os_jcc_a_1 => {
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::with_mnemonic(v, v2, vec![s]))
			}

			CtorKind::os_jcc_a_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::with_mnemonic(v, v2, vec![s, s2]))
			}

			CtorKind::os_jcc_a_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::with_mnemonic(v, v2, vec![s, s2, s3]))
			}

			CtorKind::os_jcc_b_1 => {
				v3 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v3, vec![s], v2))
			}

			CtorKind::os_jcc_b_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v3 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v3, vec![s, s2], v2))
			}

			CtorKind::os_jcc_b_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v3 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v3, vec![s, s2, s3], v2))
			}

			CtorKind::os_loopcc => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v3 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_os_loop::new(v, v3, unsafe { mem::transmute(v2 as RegisterUnderlyingType) }, vec![s, s2]))
			}

			CtorKind::os_loop => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_os_loop::new(v, u32::MAX, unsafe { mem::transmute(v2 as RegisterUnderlyingType) }, vec![s]))
			}

			CtorKind::os_mem => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_mem::new(v, s))
			}

			CtorKind::os_mem_reg16 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_mem_reg16::new(v, s))
			}

			CtorKind::os_mem2 => {
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_mem2::new(v, s, v2))
			}

			CtorKind::pblendvb => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pblendvb::new(s, unsafe { mem::transmute(v as MemorySizeUnderlyingType) }))
			}

			CtorKind::pclmulqdq => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pclmulqdq::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) })))
			}

			CtorKind::pops => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pops::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) })))
			}

			CtorKind::Reg16 => Box::new(SimpleInstrInfo_Reg16::new(s)),
			CtorKind::Reg32 => Box::new(SimpleInstrInfo_Reg32::new(s)),
			CtorKind::reverse => Box::new(SimpleInstrInfo_reverse::new(s)),

			CtorKind::sae => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_sae::new(v, s))
			}

			CtorKind::push_imm8 => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_push_imm8::new(v, unsafe { mem::transmute(v2 as u8) }, s))
			}

			CtorKind::push_imm => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_push_imm::new(v, unsafe { mem::transmute(v2 as u8) }, s))
			}

			CtorKind::SignExt_3 => {
				v = reader.read_u8() as u32;
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_SignExt::new3(unsafe { mem::transmute(v as u8) }, s, v2))
			}

			CtorKind::SignExt_4 => {
				v = reader.read_u8() as u32;
				v2 = reader.read_u8() as u32;
				v3 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_SignExt::new(unsafe { mem::transmute(v as u8) }, unsafe { mem::transmute(v2 as u8) }, s, v3))
			}

			CtorKind::imul => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_imul::new(unsafe { mem::transmute(v as u8) }, s))
			}

			CtorKind::STIG1 => {
				v = reader.read_u8() as u32;
				debug_assert!(v <= 1);
				Box::new(SimpleInstrInfo_STIG1::new(s, v != 0))
			}

			CtorKind::STIG2_2a => {
				v = reader.read_u8() as u32;
				debug_assert!(v <= 1);
				Box::new(SimpleInstrInfo_STIG2::with_pseudo_op(s, v != 0))
			}

			CtorKind::STIG2_2b => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_STIG2::with_flags(s, v))
			}

			CtorKind::XLAT => Box::new(SimpleInstrInfo_XLAT::new(s)),
		};

		infos.push(info);
		if current_index >= 0 {
			reader.set_index(current_index as usize);
		}
	}
	debug_assert!(!reader.can_read());

	#[allow(clippy::unwrap_used)]
	infos.into_boxed_slice().try_into().ok().unwrap()
}
