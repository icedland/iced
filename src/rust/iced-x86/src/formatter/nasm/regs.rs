// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::Register;
use super::super::regs_tbl::{MAX_STRING_LENGTH, REGS_TBL};
use super::super::FormatterString;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::fmt::Write;

#[allow(dead_code)]
pub(super) struct Registers;
impl Registers {
	#[allow(dead_code)]
	pub(super) const EXTRA_REGISTERS: u32 = 0;
}

lazy_static! {
	pub(super) static ref ALL_REGISTERS: Vec<FormatterString> = {
		let mut v: Vec<_> = (&*REGS_TBL).to_vec();
		let mut s = String::with_capacity(MAX_STRING_LENGTH);
		for i in 0..8usize {
			write!(s, "st{}", i).unwrap();
			v[Register::ST0 as usize + i] = FormatterString::new(s.clone());
			s.clear();
		}
		v
	};
}
