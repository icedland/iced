// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::test_utils::get_formatter_unit_tests_dir;
use crate::formatter::tests::filter_removed_code_tests;
use crate::formatter::tests::sym_res_test_case::*;
use crate::formatter::tests::sym_res_test_parser::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::Formatter;
use crate::Instruction;
use crate::{FormatterTextKind, SymResTextInfo, SymResTextPart, SymbolResolver, SymbolResult};
#[cfg(feature = "fast_fmt")]
use crate::{SpecializedFormatter, SpecializedFormatterTraitOptions};
use alloc::boxed::Box;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use std::collections::HashSet;

lazy_static! {
	static ref ALL_INFOS: (Vec<SymbolResolverTestCase>, HashSet<u32>) = {
		let mut filename = get_formatter_unit_tests_dir();
		filename.push("SymbolResolverTests.txt");
		let mut ignored: HashSet<u32> = HashSet::new();
		let v = SymbolResolverTestParser::new(filename.as_path(), &mut ignored).into_iter().collect();
		(v, ignored)
	};
}

struct SymbolResolverImpl<'a> {
	info: &'a SymbolResolverTestCase,
	vec: Vec<SymResTextPart<'a>>,
}

impl SymbolResolver for SymbolResolverImpl<'_> {
	fn symbol(
		&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>, address: u64, address_size: u32,
	) -> Option<SymbolResult<'_>> {
		for tc in &self.info.symbol_results {
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

fn get_infos_and_lines(dir: &str, filename: &str) -> (&'static [SymbolResolverTestCase], Vec<String>) {
	let mut path = get_formatter_unit_tests_dir();
	path.push(dir);
	path.push(format!("{}.txt", filename));
	let (infos, ignored) = &*ALL_INFOS;
	let formatted_lines = filter_removed_code_tests(super::get_lines_ignore_comments(path.as_path()), ignored);
	if infos.len() != formatted_lines.len() {
		panic!("infos.len() ({}) != formatted_lines.len() ({})", infos.len(), formatted_lines.len());
	}
	(infos, formatted_lines)
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(in super::super) fn symbol_resolver_test(
	dir: &str, filename: &str, fmt_factory: fn(symbol_resolver: Box<dyn SymbolResolver>) -> Box<dyn Formatter>,
) {
	let (infos, formatted_lines) = get_infos_and_lines(dir, filename);
	for (info, formatted_line) in infos.iter().zip(formatted_lines.into_iter()) {
		let symbol_resolver = Box::new(SymbolResolverImpl { info, vec: Vec::new() });
		let mut formatter = fmt_factory(symbol_resolver);
		for props in &info.options {
			props.1.initialize_options(formatter.options_mut(), props.0);
		}
		super::simple_format_test(
			info.bitness,
			&info.hex_bytes,
			info.ip,
			info.code,
			info.decoder_options,
			info.line_number,
			formatted_line.as_str(),
			formatter.as_mut(),
			|decoder| {
				for props in &info.options {
					props.1.initialize_decoder(decoder, props.0);
				}
			},
		);
	}
}

#[cfg(feature = "fast_fmt")]
pub(in super::super) fn symbol_resolver_test_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	dir: &str, filename: &str, fmt_factory: fn(symbol_resolver: Box<dyn SymbolResolver>) -> Box<SpecializedFormatter<TraitOptions>>,
) {
	let (infos, formatted_lines) = get_infos_and_lines(dir, filename);
	for (info, formatted_line) in infos.iter().zip(formatted_lines.into_iter()) {
		let symbol_resolver = Box::new(SymbolResolverImpl { info, vec: Vec::new() });
		let mut formatter = fmt_factory(symbol_resolver);
		for props in &info.options {
			props.1.initialize_options_fast(formatter.options_mut(), props.0);
		}
		super::simple_format_test_fast(
			info.bitness,
			&info.hex_bytes,
			info.ip,
			info.code,
			info.decoder_options,
			info.line_number,
			formatted_line.as_str(),
			formatter.as_mut(),
			|decoder| {
				for props in &info.options {
					props.1.initialize_decoder(decoder, props.0);
				}
			},
		);
	}
}
