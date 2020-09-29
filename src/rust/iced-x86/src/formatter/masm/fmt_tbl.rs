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

use super::super::super::data_reader::DataReader;
use super::super::super::iced_constants::IcedConstants;
use super::super::pseudo_ops::get_pseudo_ops;
use super::super::strings_tbl::get_strings_table;
use super::enums::*;
use super::fmt_data::FORMATTER_TBL_DATA;
use super::info::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::{mem, u32};

lazy_static! {
	pub(super) static ref ALL_INFOS: Vec<Box<InstrInfo + Sync + Send>> = read();
}

fn add_suffix(s: &str, c: char) -> String {
	let mut res = String::with_capacity(s.len() + 1);
	res.push_str(s);
	res.push(c);
	res
}

fn read() -> Vec<Box<InstrInfo + Sync + Send>> {
	let mut infos: Vec<Box<InstrInfo + Sync + Send>> = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
	let mut reader = DataReader::new(FORMATTER_TBL_DATA);
	let strings = get_strings_table();
	let mut prev_index = -1isize;
	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
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
			let s = &strings[reader.read_compressed_u32() as usize];
			let mut res = String::with_capacity(s.len() + 1);
			res.push('v');
			res.push_str(s);
			res
		} else {
			strings[reader.read_compressed_u32() as usize].clone()
		};

		let c;
		let v;
		let v2;
		let v3;
		let info: Box<InstrInfo + Sync + Send> = match ctor_kind {
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

			CtorKind::DeclareData => Box::new(SimpleInstrInfo_DeclareData::new(unsafe { mem::transmute(i as u16) }, s)),

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
				Box::new(SimpleInstrInfo_fword::new(unsafe { mem::transmute(v as u8) }, v2, s, s2))
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
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2]))
			}

			CtorKind::CCa_3 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				let s3 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2, s3]))
			}

			CtorKind::CCb_1 => {
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s], v))
			}

			CtorKind::CCb_2 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s, s2], v))
			}

			CtorKind::CCb_3 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				let s3 = strings[reader.read_compressed_u32() as usize].clone();
				v2 = reader.read_compressed_u32();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::with_flags(v2, vec![s, s2, s3], v))
			}

			CtorKind::jcc_1 => {
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s]))
			}

			CtorKind::jcc_2 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s, s2]))
			}

			CtorKind::jcc_3 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				let s3 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_jcc::new(v, vec![s, s2, s3]))
			}

			CtorKind::Loopcc1 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_compressed_u32();
				Box::new(SimpleInstrInfo_cc::new(v, vec![s, s2]))
			}

			CtorKind::Loopcc2 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				c = reader.read_u8() as u8 as char;
				v2 = reader.read_compressed_u32();
				let s3 = add_suffix(&s, c);
				let s4 = add_suffix(&s2, c);
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_OpSize_cc::new(unsafe { mem::transmute(v as u8) }, v2, vec![s, s2], vec![s3, s4]))
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
				Box::new(SimpleInstrInfo_monitor::new(s, unsafe { mem::transmute(v as u8) }, unsafe { mem::transmute(v2 as u8) }, unsafe {
					mem::transmute(v3 as u8)
				}))
			}

			CtorKind::mwait => Box::new(SimpleInstrInfo_mwait::new(s)),
			CtorKind::mwaitx => Box::new(SimpleInstrInfo_mwaitx::new(s)),

			CtorKind::nop => {
				v = reader.read_compressed_u32();
				v2 = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_nop::new(v, s, unsafe { mem::transmute(v2 as u8) }))
			}

			CtorKind::OpSize_1 => {
				v = reader.read_u8() as u32;
				let s2 = add_suffix(&s, 'w');
				let s3 = add_suffix(&s, 'd');
				let s4 = add_suffix(&s, 'q');
				Box::new(SimpleInstrInfo_OpSize::new(unsafe { mem::transmute(v as u8) }, s, s2, s3, s4))
			}

			CtorKind::OpSize_2 => {
				c = reader.read_u8() as u8 as char;
				let s2 = add_suffix(&s, c);
				v = reader.read_u8() as u32;
				Box::new(SimpleInstrInfo_OpSize::new(unsafe { mem::transmute(v as u8) }, s, s2.clone(), s2.clone(), s2))
			}

			CtorKind::OpSize2 => {
				let s2 = strings[reader.read_compressed_u32() as usize].clone();
				let s3 = strings[reader.read_compressed_u32() as usize].clone();
				let s4 = strings[reader.read_compressed_u32() as usize].clone();
				v = reader.read_u8() as u32;
				if v > 1 {
					panic!();
				}
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
				Box::new(SimpleInstrInfo_reg::new(s, unsafe { mem::transmute(v as u8) }))
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
				if v > 1 {
					panic!();
				}
				Box::new(SimpleInstrInfo_STi_ST::new(s, v != 0))
			}

			CtorKind::STIG1 => {
				v = reader.read_u8() as u32;
				if v > 1 {
					panic!();
				}
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
	if reader.can_read() {
		panic!();
	}
	infos
}
