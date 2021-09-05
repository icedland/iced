// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod fmt_factory;
mod misc;
mod not_fast_fmt;
mod options;
mod symres;

use crate::{
	Code, Decoder, DecoderOptions, FastFormatter, Instruction, SpecializedFormatter, SpecializedFormatterTraitOptions, SymbolResolver, SymbolResult,
};

macro_rules! mk_tests {
	($mod_name:ident, $create_default:path, $create_inverted:path) => {
		mod $mod_name {
			use crate::formatter::tests::formatter_test_fast;
			#[cfg(feature = "encoder")]
			use crate::formatter::tests::formatter_test_nondec_fast;

			#[test]
			fn fmt_default_16() {
				formatter_test_fast(16, "Fast", "Default", false, $create_default);
			}

			#[test]
			fn fmt_inverted_16() {
				formatter_test_fast(16, "Fast", "Inverted", false, $create_inverted);
			}

			#[test]
			fn fmt_misc_16() {
				formatter_test_fast(16, "Fast", "Misc", true, $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_default_16() {
				formatter_test_nondec_fast(16, "Fast", "NonDec_Default", $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_inverted_16() {
				formatter_test_nondec_fast(16, "Fast", "NonDec_Inverted", $create_inverted);
			}

			#[test]
			fn fmt_default_32() {
				formatter_test_fast(32, "Fast", "Default", false, $create_default);
			}

			#[test]
			fn fmt_inverted_32() {
				formatter_test_fast(32, "Fast", "Inverted", false, $create_inverted);
			}

			#[test]
			fn fmt_misc_32() {
				formatter_test_fast(32, "Fast", "Misc", true, $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_default_32() {
				formatter_test_nondec_fast(32, "Fast", "NonDec_Default", $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_inverted_32() {
				formatter_test_nondec_fast(32, "Fast", "NonDec_Inverted", $create_inverted);
			}

			#[test]
			fn fmt_default_64() {
				formatter_test_fast(64, "Fast", "Default", false, $create_default);
			}

			#[test]
			fn fmt_inverted_64() {
				formatter_test_fast(64, "Fast", "Inverted", false, $create_inverted);
			}

			#[test]
			fn fmt_misc_64() {
				formatter_test_fast(64, "Fast", "Misc", true, $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_default_64() {
				formatter_test_nondec_fast(64, "Fast", "NonDec_Default", $create_default);
			}

			#[test]
			#[cfg(feature = "encoder")]
			fn fmt_nondec_inverted_64() {
				formatter_test_nondec_fast(64, "Fast", "NonDec_Inverted", $create_inverted);
			}
		}
	};
}

mk_tests! {test_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_default::<crate::DefaultFastFormatterTraitOptions>, crate::formatter::fast::tests::fmt_factory::create_inverted::<crate::DefaultFastFormatterTraitOptions>}
mk_tests! {test_not_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_default::<crate::formatter::fast::tests::not_fast_fmt::NotFastFormatterTraitOptions>, crate::formatter::fast::tests::fmt_factory::create_inverted::<crate::formatter::fast::tests::not_fast_fmt::NotFastFormatterTraitOptions>}

#[test]
fn format_hex2() {
	// mov rax,0000_0000_0000_0000h
	let mut instr = Decoder::new(64, b"\x48\xB8\x00\x00\x00\x00\x00\x00\x00\x00", DecoderOptions::NONE).decode();
	assert_eq!(instr.code(), Code::Mov_r64_imm64);

	// Test formatting every value 00-FF in every nibble position + upper/lower + hex prefix/suffix.
	// This test uses '0' for every 'x' or a random value.
	//			00xxxxxxxxxxxxxx-FFxxxxxxxxxxxxxx
	//			x00xxxxxxxxxxxxx-xFFxxxxxxxxxxxxx
	//			xx00xxxxxxxxxxxx-xxFFxxxxxxxxxxxx
	//			...
	//			xxxxxxxxxxxxxx00-xxxxxxxxxxxxxxFF
	let mut actual_instr = String::new();
	for &or_value in &[0, 0x1234_5678_9ABC_DEF1, 0xFEDC_BA98_7654_321F] {
		for &uppercase in &[false, true] {
			for &hex_prefix in &[false, true] {
				for hex_shift in 0..15 {
					let hex_shift = hex_shift * 4;
					for hex2_value in 0..0x100u64 {
						let mut formatter = FastFormatter::new();
						formatter.options_mut().set_uppercase_hex(uppercase);
						formatter.options_mut().set_use_hex_prefix(hex_prefix);

						let mask = 0xFFu64 << hex_shift;
						let imm: u64 = (hex2_value << hex_shift) | (or_value & !mask);
						assert_eq!((imm >> hex_shift) & 0xFF, hex2_value);
						instr.set_immediate64(imm);

						let expected_imm = format!("{:x}", imm);
						let leading_zero = if !hex_prefix && expected_imm.as_bytes()[0] >= b'a' { "0" } else { "" };
						let expected_imm = if uppercase { expected_imm.to_uppercase() } else { expected_imm };
						let (prefix, suffix) = if hex_prefix { ("0x", "") } else { ("", "h") };

						let expected_instr = format!("mov rax,{}{}{}{}", prefix, leading_zero, expected_imm, suffix);

						actual_instr.clear();
						formatter.format(&instr, &mut actual_instr);
						assert_eq!(actual_instr, expected_instr);
					}
				}
			}
		}
	}
}

struct MySymbolResolver;
impl SymbolResolver for MySymbolResolver {
	fn symbol(
		&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>, _address: u64, _address_size: u32,
	) -> Option<SymbolResult<'_>> {
		panic!()
	}
}

#[test]
fn test_no_symresolver_try_with_options_ok1() {
	struct MyTraitOptions;
	impl SpecializedFormatterTraitOptions for MyTraitOptions {
		const ENABLE_SYMBOL_RESOLVER: bool = false;
	}
	type MyFormatter = SpecializedFormatter<MyTraitOptions>;
	assert!(MyFormatter::try_with_options(None).is_ok());
}

#[test]
fn test_no_symresolver_try_with_options_ok2() {
	struct MyTraitOptions;
	impl SpecializedFormatterTraitOptions for MyTraitOptions {
		const ENABLE_SYMBOL_RESOLVER: bool = true;
	}
	type MyFormatter = SpecializedFormatter<MyTraitOptions>;
	assert!(MyFormatter::try_with_options(None).is_ok());
}

#[test]
fn test_symresolver_try_with_options_err() {
	struct MyTraitOptions;
	impl SpecializedFormatterTraitOptions for MyTraitOptions {
		const ENABLE_SYMBOL_RESOLVER: bool = false;
	}
	type MyFormatter = SpecializedFormatter<MyTraitOptions>;
	assert!(MyFormatter::try_with_options(Some(Box::new(MySymbolResolver {}))).is_err());
}

#[test]
fn test_symresolver_try_with_options_ok() {
	struct MyTraitOptions;
	impl SpecializedFormatterTraitOptions for MyTraitOptions {
		const ENABLE_SYMBOL_RESOLVER: bool = true;
	}
	type MyFormatter = SpecializedFormatter<MyTraitOptions>;
	assert!(MyFormatter::try_with_options(Some(Box::new(MySymbolResolver {}))).is_ok());
}
