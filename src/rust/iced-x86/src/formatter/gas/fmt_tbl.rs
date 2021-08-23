// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::data_reader::DataReader;
use crate::formatter::gas::enums::*;
use crate::formatter::gas::fmt_data::FORMATTER_TBL_DATA;
use crate::formatter::gas::info::*;
use crate::formatter::pseudo_ops::get_pseudo_ops;
use crate::formatter::strings_tbl::get_strings_table_ref;
use crate::iced_constants::IcedConstants;
use crate::{CodeSizeUnderlyingType, CodeUnderlyingType, RegisterUnderlyingType};
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
	if c == '\0' {
		String::from(s)
	} else {
		let mut res = String::with_capacity(s.len() + 1);
		res.push_str(s);
		res.push(c);
		res
	}
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
		let mut s = if (f & 0x80) != 0 {
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

			CtorKind::Normal_2a => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo::with_mnemonic_suffix(s, s2))
			}

			CtorKind::Normal_2b => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo::with_mnemonic_flags(s, v))
			}

			CtorKind::Normal_2c => {
				c = reader.read_u8() as u8 as char;
				s = add_suffix(&s, c);
				Box::new(SimpleInstrInfo::with_mnemonic(s))
			}

			CtorKind::Normal_3 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo::new(s, s2, v))
			}

			CtorKind::AamAad => Box::new(SimpleInstrInfo_AamAad::new(s)),

			CtorKind::asz => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_as::new(v, s))
			}

			CtorKind::bnd => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_bnd::new(s, s2, v))
			}

			CtorKind::DeclareData => Box::new(SimpleInstrInfo_DeclareData::new(unsafe { mem::transmute(i as CodeUnderlyingType) }, s)),

			CtorKind::er_2 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_er::with_mnemonic(v, s))
			}

			CtorKind::er_4 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_er::new(v, s, s2, v2))
			}

			CtorKind::far => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_far::new(v, s, s2))
			}

			CtorKind::imul => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_imul::new(s, s2))
			}

			CtorKind::maskmovq => Box::new(SimpleInstrInfo_maskmovq::new(s)),

			CtorKind::movabs => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s4 = add_suffix(&s3, c);
				Box::new(SimpleInstrInfo_movabs::new(s, s2, s3, s4))
			}

			CtorKind::nop => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_nop::new(v, s, unsafe { mem::transmute(v2 as RegisterUnderlyingType) }))
			}

			CtorKind::OpSize => {
				v = reader.read_u8() as u32;
				let s2 = add_suffix(&s, 'w');
				let s3 = add_suffix(&s, 'l');
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

			CtorKind::os => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				debug_assert!(v2 <= 1);
				v3 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os::new(v, s, v2 != 0, v3))
			}

			CtorKind::CC_1 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s], vec![s2]))
			}

			CtorKind::CC_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				c = reader.read_u8() as u8 as char;
				let s3 = add_suffix(&s, c);
				let s4 = add_suffix(&s2, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2], vec![s3, s4]))
			}

			CtorKind::CC_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				c = reader.read_u8() as u8 as char;
				let s4 = add_suffix(&s, c);
				let s5 = add_suffix(&s2, c);
				let s6 = add_suffix(&s3, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2, s3], vec![s4, s5, s6]))
			}

			CtorKind::os_jcc_1 => {
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v2, vec![s]))
			}

			CtorKind::os_jcc_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v2, vec![s, s2]))
			}

			CtorKind::os_jcc_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_jcc::new(v, v2, vec![s, s2, s3]))
			}

			CtorKind::os_loopcc => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				c = reader.read_u8() as u8 as char;
				let s3 = add_suffix(&s, c);
				let s4 = add_suffix(&s2, c);
				v3 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_loop::new(v, v2, v3, vec![s, s2], vec![s3, s4]))
			}

			CtorKind::os_loop => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_loop::new(v, v2, u32::MAX, vec![s], vec![s2]))
			}

			CtorKind::os_mem => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_mem::new(v, s, s2))
			}

			CtorKind::Reg16 => {
				let s2 = add_suffix(&s, 'w');
				Box::new(SimpleInstrInfo_Reg16::new(s, s2))
			}

			CtorKind::os_mem2 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os_mem2::new(v, s, s2))
			}

			CtorKind::os2_3 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				debug_assert!(v2 <= 1);
				Box::new(SimpleInstrInfo_os2::new(v, s, s2, v2 != 0, 0))
			}

			CtorKind::os2_4 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				debug_assert!(v2 <= 1);
				v3 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_os2::new(v, s, s2, v2 != 0, v3))
			}

			CtorKind::pblendvb => Box::new(SimpleInstrInfo_pblendvb::new(s)),

			CtorKind::pclmulqdq => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pclmulqdq::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) })))
			}

			CtorKind::pops => {
				v = reader.read_u8() as u32;
				v2 = reader.read_u8() as u32;
				debug_assert!(v2 <= 1);
				Box::new(SimpleInstrInfo_pops::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) }), v2 != 0))
			}

			CtorKind::mem16 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				let s3 = add_suffix(&s, 'w');
				Box::new(SimpleInstrInfo_mem16::new(s, s2, s3))
			}

			CtorKind::Reg32 => Box::new(SimpleInstrInfo_Reg32::new(s)),

			CtorKind::sae => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_sae::new(v, s))
			}

			CtorKind::ST_STi => Box::new(SimpleInstrInfo_ST_STi::new(s)),
			CtorKind::STi_ST => {
				v = reader.read_u8() as u32;
				debug_assert!(v <= 1);
				Box::new(SimpleInstrInfo_STi_ST::new(s, v != 0))
			}

			CtorKind::STIG1 => {
				v = reader.read_u8() as u32;
				debug_assert!(v <= 1);
				Box::new(SimpleInstrInfo_STIG1::new(s, v != 0))
			}
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
