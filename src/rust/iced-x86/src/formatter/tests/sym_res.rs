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

use super::super::super::Instruction;
use super::super::test_utils::get_formatter_unit_tests_dir;
use super::super::*;
use super::sym_res_test_case::*;
use super::sym_res_test_parser::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

lazy_static! {
	static ref ALL_INFOS: Vec<SymbolResolverTestCase> = {
		let mut filename = get_formatter_unit_tests_dir();
		filename.push("SymbolResolverTests.txt");
		SymbolResolverTestParser::new(filename.as_path()).into_iter().collect()
	};
}

struct SymbolResolverImpl<'a> {
	info: &'a SymbolResolverTestCase,
	vec: Vec<SymResTextPart<'a>>,
}

impl<'a> SymbolResolver for SymbolResolverImpl<'a> {
	fn symbol(
		&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>, address: u64, address_size: u32,
	) -> Option<SymbolResult> {
		for tc in self.info.symbol_results.iter() {
			if tc.address != address || tc.address_size != address_size {
				continue;
			}
			self.vec.clear();
			self.vec.extend(tc.symbol_parts.iter().map(|a| SymResTextPart::new(a, FormatterTextKind::Text)));
			let text = SymResTextInfo::with_vec(&self.vec);
			if let Some(memory_size) = tc.memory_size {
				return Some(SymbolResult::with_text_flags_size(tc.symbol_address, text, tc.flags, memory_size));
			} else {
				return Some(SymbolResult::with_text_flags(tc.symbol_address, text, tc.flags));
			}
		}
		None
	}
}

pub(in super::super) fn symbol_resolver_test(dir: &str, filename: &str, fmt_factory: fn(symbol_resolver: Box<SymbolResolver>) -> Box<Formatter>) {
	let mut path = get_formatter_unit_tests_dir();
	path.push(dir);
	path.push(format!("{}.txt", filename));
	let formatted_lines = super::get_lines_ignore_comments(path.as_path());
	let infos = &*ALL_INFOS;
	if infos.len() != formatted_lines.len() {
		panic!("infos.len() ({}) != formatted_lines.len() ({})", infos.len(), formatted_lines.len());
	}

	for (info, formatted_line) in infos.iter().zip(formatted_lines.into_iter()) {
		let symbol_resolver = Box::new(SymbolResolverImpl { info: &info, vec: Vec::new() });
		let mut formatter = fmt_factory(symbol_resolver);
		for props in info.options.iter() {
			props.1.initialize_options(formatter.options_mut(), props.0);
		}
		super::simple_format_test(info.bitness, &info.hex_bytes, info.code, 0, formatted_line.as_str(), formatter.as_mut(), |decoder| {
			for props in info.options.iter() {
				props.1.initialize_decoder(decoder, props.0);
			}
		});
	}
}
