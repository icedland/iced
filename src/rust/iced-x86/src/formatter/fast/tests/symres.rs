// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::MAX_FMT_INSTR_LEN;
use crate::iced_constants::IcedConstants;
use crate::{Decoder, DecoderOptions, FastFormatter, Instruction, SymbolResolver, SymbolResult};

macro_rules! mk_tests {
	($mod_name:ident, $create_options:path) => {
		mod $mod_name {
			use crate::formatter::tests::sym_res::symbol_resolver_test_fast;

			#[test]
			fn symres() {
				symbol_resolver_test_fast("Fast", "SymbolResolverTests", |symbol_resolver| $create_options(symbol_resolver));
			}
		}
	};
}
mk_tests! {test_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_resolver::<crate::DefaultFastFormatterTraitOptions>}
mk_tests! {test_not_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_resolver::<crate::formatter::fast::tests::not_fast_fmt::NotFastFormatterTraitOptions>}

struct LongSymbolResolver {
	symbol_result: String,
	instruction_operand: u32,
}
impl SymbolResolver for LongSymbolResolver {
	fn symbol(
		&mut self, _instruction: &Instruction, _operand: u32, instruction_operand: Option<u32>, address: u64, _address_size: u32,
	) -> Option<SymbolResult<'_>> {
		if instruction_operand == Some(self.instruction_operand) {
			Some(SymbolResult::with_str(address, &self.symbol_result))
		} else {
			None
		}
	}
}

#[test]
fn test_long_symbols() {
	const MAX_SYMBOL_LEN: usize = 1024;
	const _: () = assert!(MAX_SYMBOL_LEN > MAX_FMT_INSTR_LEN);

	#[rustfmt::skip]
	let test_cases: [(u32, &'static [u8]); IcedConstants::MAX_OP_COUNT] = [
		// mov [rax+12345678h],rcx
		(0, b"\x48\x89\x88\x78\x56\x34\x12"),
		// op1: mov rcx,[rax+12345678h]
		(1, b"\x48\x8B\x88\x78\x56\x34\x12"),
		// op2: imul ecx,[rsi+12345678h],9ABCDEF1h
		(2, b"\x69\x8E\x78\x56\x34\x12\xF1\xDE\xBC\x9A"),
		// op3: vpermil2ps xmm2,xmm6,xmm4,[rax+12345678h],1
		(3, b"\xC4\xE3\xC9\x48\x90\x78\x56\x34\x12\x41"),
		// op4: vpermil2ps xmm2,xmm6,[rax],xmm4,1
		(4, b"\xC4\xE3\x49\x48\x10\x41"),
	];
	for &(op, bytes) in &test_cases {
		let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
		let instr = decoder.decode();
		for symbol_len in 0..=MAX_SYMBOL_LEN {
			// Don't re-use it, always create a new one
			let mut output = String::new();
			let resolver = LongSymbolResolver { symbol_result: "a".repeat(symbol_len), instruction_operand: op };
			let mut formatter = FastFormatter::try_with_options(Some(Box::new(resolver))).unwrap();
			formatter.format(&instr, &mut output);
			assert!(output.len() >= symbol_len);
		}
	}
}
