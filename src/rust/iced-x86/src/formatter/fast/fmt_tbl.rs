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
use super::super::strings_tbl::get_strings_table;
use super::enums::*;
use super::fmt_data::FORMATTER_TBL_DATA;
use super::FmtTableData;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

lazy_static! {
	pub(super) static ref FMT_DATA: FmtTableData = read();
}

fn read() -> FmtTableData {
	let mut mnemonics: Vec<&'static str> = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
	let mut flags: Vec<u8> = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
	let mut reader = DataReader::new(FORMATTER_TBL_DATA);
	let strings = get_strings_table();
	let mut prev_index = -1isize;
	for _ in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let f = reader.read_u8();
		let current_index;
		if (f & (FastFmtFlags::SAME_AS_PREV as usize)) != 0 {
			current_index = reader.index() as isize;
			reader.set_index(prev_index as usize);
		} else {
			current_index = -1;
			prev_index = reader.index() as isize;
		}
		let mnemonic = if (f & (FastFmtFlags::HAS_VPREFIX as usize)) != 0 {
			let s = &strings[reader.read_compressed_u32() as usize];
			let mut res = String::with_capacity(s.len() + 1);
			res.push('v');
			res.push_str(s);
			res
		} else {
			strings[reader.read_compressed_u32() as usize].clone()
		};

		let mnemonic = Box::into_raw(Box::new(mnemonic));
		let mnemonic: &'static str = unsafe { (*mnemonic).as_str() };
		flags.push(f as u8);
		mnemonics.push(mnemonic);

		if current_index >= 0 {
			reader.set_index(current_index as usize);
		}
	}
	if reader.can_read() {
		panic!();
	}

	FmtTableData { mnemonics, flags }
}
