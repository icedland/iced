// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::data_reader::DataReader;
use crate::formatter::masm::enums::*;
use crate::formatter::masm::fmt_data::FORMATTER_TBL_DATA;
use crate::formatter::masm::info::*;
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

			CtorKind::AX => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_AX::new(s, s2))
			}

			CtorKind::AY => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_AY::new(s, s2))
			}

			CtorKind::bnd => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_bnd::new(s, v))
			}

			CtorKind::DeclareData => Box::new(SimpleInstrInfo_DeclareData::new(unsafe { mem::transmute(i as CodeUnderlyingType) }, s)),

			CtorKind::DX => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_DX::new(s, s2))
			}

			CtorKind::fword => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_u8() as u32;
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_fword::new(unsafe { mem::transmute(v as CodeSizeUnderlyingType) }, v2, s, s2))
			}

			CtorKind::Int3 => Box::new(SimpleInstrInfo_Int3::new(s)),
			CtorKind::imul => Box::new(SimpleInstrInfo_imul::new(s)),

			CtorKind::invlpga => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_invlpga::new(v, s))
			}

			CtorKind::CCa_1 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s]))
			}

			CtorKind::CCa_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2]))
			}

			CtorKind::CCa_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2, s3]))
			}

			CtorKind::CCb_1 => {
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s], v))
			}

			CtorKind::CCb_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s, s2], v))
			}

			CtorKind::CCb_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s, s2, s3], v))
			}

			CtorKind::jcc_1 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s]))
			}

			CtorKind::jcc_2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s, s2]))
			}

			CtorKind::jcc_3 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s, s2, s3]))
			}

			CtorKind::Loopcc1 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2]))
			}

			CtorKind::Loopcc2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				c = reader.read_u8() as u8 as char;
				v2 = reader.read_compressed_u32();
				let s3 = add_suffix(&s, c);
				let s4 = add_suffix(&s2, c);
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_OpSize_cc::new(unsafe { mem::transmute(v as CodeSizeUnderlyingType) }, v2, vec![s, s2], vec![s3, s4]))
			}

			CtorKind::maskmovq => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_maskmovq::new(s, v))
			}

			CtorKind::memsize => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_memsize::new(v, s))
			}

			CtorKind::monitor => {
				v = reader.read_u8() as u32;
				v2 = reader.read_u8() as u32;
				v3 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_monitor::new(
					s,
					unsafe { mem::transmute(v as RegisterUnderlyingType) },
					unsafe { mem::transmute(v2 as RegisterUnderlyingType) },
					unsafe { mem::transmute(v3 as RegisterUnderlyingType) },
				))
			}

			CtorKind::mwait => Box::new(SimpleInstrInfo_mwait::new(s)),
			CtorKind::mwaitx => Box::new(SimpleInstrInfo_mwaitx::new(s)),

			CtorKind::nop => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_nop::new(v, s, unsafe { mem::transmute(v2 as RegisterUnderlyingType) }))
			}

			CtorKind::OpSize_1 => {
				v = reader.read_u8() as u32;
				let s2 = add_suffix(&s, 'w');
				let s3 = add_suffix(&s, 'd');
				let s4 = add_suffix(&s, 'q');
				Box::new(SimpleInstrInfo_OpSize::new(unsafe { mem::transmute(v as CodeSizeUnderlyingType) }, s, s2, s3, s4))
			}

			CtorKind::OpSize_2 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_OpSize::new(unsafe { mem::transmute(v as CodeSizeUnderlyingType) }, s, s2.clone(), s2.clone(), s2))
			}

			CtorKind::OpSize2 => {
				let s2 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s3 = String::from(strings[reader.read_compressed_u32() as usize]);
				let s4 = String::from(strings[reader.read_compressed_u32() as usize]);
				v = reader.read_u8() as u32;
				debug_assert!(v <= 1);
				Box::new(SimpleInstrInfo_OpSize2::new(s, s2, s3, s4, v != 0))
			}

			CtorKind::pblendvb => Box::new(SimpleInstrInfo_pblendvb::new(s)),

			CtorKind::pclmulqdq => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pclmulqdq::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) })))
			}

			CtorKind::pops_2 => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_pops::with_mnemonic(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) })))
			}

			CtorKind::pops_3 => {
				v = reader.read_u8() as u32;
				v2 = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_pops::new(s, get_pseudo_ops(unsafe { mem::transmute(v as u8) }), v2))
			}

			CtorKind::reg => {
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_reg::new(s, unsafe { mem::transmute(v as RegisterUnderlyingType) }))
			}

			CtorKind::Reg16 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_Reg16::new(s, v))
			}

			CtorKind::Reg32 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_Reg32::new(s, v))
			}

			CtorKind::reverse => Box::new(SimpleInstrInfo_reverse::new(s)),
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

			CtorKind::XLAT => {
				let s2 = add_suffix(&s, 'b');
				Box::new(SimpleInstrInfo_XLAT::new(s, s2))
			}

			CtorKind::XY => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_XY::new(s, s2))
			}

			CtorKind::YA => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_YA::new(s, s2))
			}

			CtorKind::YD => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_YD::new(s, s2))
			}

			CtorKind::YX => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				Box::new(SimpleInstrInfo_YX::new(s, s2))
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
