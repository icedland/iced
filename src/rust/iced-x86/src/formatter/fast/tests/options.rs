// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::{
	Decoder, DecoderOptions, DefaultFastFormatterTraitOptions, DefaultSpecializedFormatterTraitOptions, FastFormatterOptions, SpecializedFormatter,
	SpecializedFormatterTraitOptions,
};

macro_rules! mk_tests {
	($mod_name:ident, $create_options:path) => {
		mod $mod_name {
			use crate::formatter::tests::options::{test_format_file_common_fast, test_format_file_fast};

			#[test]
			fn test_options_common() {
				test_format_file_common_fast("Fast", "OptionsResult.Common", $create_options);
			}

			#[test]
			fn test_options2() {
				test_format_file_fast("Fast", "OptionsResult2", "Options2", $create_options);
			}
		}
	};
}
mk_tests! {test_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_options::<crate::DefaultFastFormatterTraitOptions>}
mk_tests! {test_not_fmt_factory, crate::formatter::fast::tests::fmt_factory::create_options::<crate::formatter::fast::tests::not_fast_fmt::NotFastFormatterTraitOptions>}

fn invert_options(options: &mut FastFormatterOptions) {
	options.set_space_after_operand_separator(options.space_after_operand_separator() ^ true);
	options.set_rip_relative_addresses(options.rip_relative_addresses() ^ true);
	options.set_use_pseudo_ops(options.use_pseudo_ops() ^ true);
	options.set_show_symbol_address(options.show_symbol_address() ^ true);
	options.set_always_show_segment_register(options.always_show_segment_register() ^ true);
	options.set_always_show_memory_size(options.always_show_memory_size() ^ true);
	options.set_uppercase_hex(options.uppercase_hex() ^ true);
	options.set_use_hex_prefix(options.use_hex_prefix() ^ true);
}

#[test]
fn test_specialized_formatter_trait_options() {
	struct MyTraitOptions;
	impl SpecializedFormatterTraitOptions for MyTraitOptions {}
	type MyFormatter = SpecializedFormatter<MyTraitOptions>;

	let mut formatter = MyFormatter::new();
	let options = formatter.options_mut();

	for _ in 0..2 {
		assert_eq!(MyTraitOptions::__IS_FAST_FORMATTER, false);
		assert_eq!(MyTraitOptions::ENABLE_SYMBOL_RESOLVER, false);
		assert_eq!(MyTraitOptions::ENABLE_DB_DW_DD_DQ, false);
		assert_eq!(unsafe { MyTraitOptions::verify_output_has_enough_bytes_left() }, true);
		assert_eq!(MyTraitOptions::space_after_operand_separator(options), false);
		assert_eq!(MyTraitOptions::rip_relative_addresses(options), true);
		assert_eq!(MyTraitOptions::use_pseudo_ops(options), false);
		assert_eq!(MyTraitOptions::show_symbol_address(options), false);
		assert_eq!(MyTraitOptions::always_show_segment_register(options), false);
		assert_eq!(MyTraitOptions::always_show_memory_size(options), false);
		assert_eq!(MyTraitOptions::uppercase_hex(options), true);
		assert_eq!(MyTraitOptions::use_hex_prefix(options), true);

		invert_options(options);
	}
}

#[test]
fn test_default_specialized_formatter_trait_options() {
	type MyFormatter = SpecializedFormatter<DefaultSpecializedFormatterTraitOptions>;

	let mut formatter = MyFormatter::new();
	let options = formatter.options_mut();

	for _ in 0..2 {
		assert_eq!(DefaultSpecializedFormatterTraitOptions::__IS_FAST_FORMATTER, false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::ENABLE_SYMBOL_RESOLVER, false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::ENABLE_DB_DW_DD_DQ, false);
		assert_eq!(unsafe { DefaultSpecializedFormatterTraitOptions::verify_output_has_enough_bytes_left() }, true);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::space_after_operand_separator(options), false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::rip_relative_addresses(options), true);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::use_pseudo_ops(options), false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::show_symbol_address(options), false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::always_show_segment_register(options), false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::always_show_memory_size(options), false);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::uppercase_hex(options), true);
		assert_eq!(DefaultSpecializedFormatterTraitOptions::use_hex_prefix(options), true);

		invert_options(options);
	}
}

#[test]
fn test_default_fast_formatter_trait_options() {
	type MyFormatter = SpecializedFormatter<DefaultFastFormatterTraitOptions>;

	let mut formatter = MyFormatter::new();
	let options = formatter.options_mut();

	assert_eq!(DefaultFastFormatterTraitOptions::__IS_FAST_FORMATTER, true);
	assert_eq!(DefaultFastFormatterTraitOptions::ENABLE_SYMBOL_RESOLVER, true);
	assert_eq!(DefaultFastFormatterTraitOptions::ENABLE_DB_DW_DD_DQ, true);
	assert_eq!(unsafe { DefaultFastFormatterTraitOptions::verify_output_has_enough_bytes_left() }, true);
	assert_eq!(DefaultFastFormatterTraitOptions::space_after_operand_separator(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::rip_relative_addresses(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::use_pseudo_ops(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::show_symbol_address(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::always_show_segment_register(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::always_show_memory_size(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::uppercase_hex(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::use_hex_prefix(options), false);

	invert_options(options);

	assert_eq!(DefaultFastFormatterTraitOptions::ENABLE_SYMBOL_RESOLVER, true);
	assert_eq!(DefaultFastFormatterTraitOptions::ENABLE_DB_DW_DD_DQ, true);
	assert_eq!(unsafe { DefaultFastFormatterTraitOptions::verify_output_has_enough_bytes_left() }, true);
	assert_eq!(DefaultFastFormatterTraitOptions::space_after_operand_separator(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::rip_relative_addresses(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::use_pseudo_ops(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::show_symbol_address(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::always_show_segment_register(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::always_show_memory_size(options), true);
	assert_eq!(DefaultFastFormatterTraitOptions::uppercase_hex(options), false);
	assert_eq!(DefaultFastFormatterTraitOptions::use_hex_prefix(options), true);
}

#[test]
fn test_specialized_fmt_does_not_call_options_methods() {
	macro_rules! test_option {
		($space_after_operand_separator:literal, $rip_relative_addresses:literal, $use_pseudo_ops:literal, $show_symbol_address:literal,
		$always_show_segment_register:literal, $always_show_memory_size:literal, $uppercase_hex:literal, $use_hex_prefix:literal,
		$bytes:literal, $disasm:literal) => {{
			struct MyTraitOptions;
			impl SpecializedFormatterTraitOptions for MyTraitOptions {
				fn space_after_operand_separator(_options: &FastFormatterOptions) -> bool {
					$space_after_operand_separator
				}
				fn rip_relative_addresses(_options: &FastFormatterOptions) -> bool {
					$rip_relative_addresses
				}
				fn use_pseudo_ops(_options: &FastFormatterOptions) -> bool {
					$use_pseudo_ops
				}
				fn show_symbol_address(_options: &FastFormatterOptions) -> bool {
					$show_symbol_address
				}
				fn always_show_segment_register(_options: &FastFormatterOptions) -> bool {
					$always_show_segment_register
				}
				fn always_show_memory_size(_options: &FastFormatterOptions) -> bool {
					$always_show_memory_size
				}
				fn uppercase_hex(_options: &FastFormatterOptions) -> bool {
					$uppercase_hex
				}
				fn use_hex_prefix(_options: &FastFormatterOptions) -> bool {
					$use_hex_prefix
				}
			}
			type MyFormatter = SpecializedFormatter<MyTraitOptions>;

			let instr = Decoder::with_ip(64, $bytes, 0x1234_5678_9ABC_DEF1, DecoderOptions::NONE).decode();

			let mut formatter = MyFormatter::new();
			let mut output = String::new();

			for _ in 0..2 {
				output.clear();
				formatter.format(&instr, &mut output);
				assert_eq!(output, $disasm);
				invert_options(formatter.options_mut());
			}
		}};
	}

	// opt=false, other=false
	// opt=false, other=true
	// opt=true , other=false
	// opt=true , other=true

	// space_after_operand_separator
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0ffffffff82345aa5h");
	test_option!(false, true, true, true, true, true, true, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0xFFFFFFFF82345AA5");
	test_option!(true, false, false, false, false, false, false, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0ffffffff82345aa5h");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0xFFFFFFFF82345AA5");
	// uppercase_hex
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0ffffffff82345aa5h");
	test_option!(true, true, true, true, true, true, false, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0xffffffff82345aa5");
	test_option!(false, false, false, false, false, false, true, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0FFFFFFFF82345AA5h");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0xFFFFFFFF82345AA5");
	// use_hex_prefix
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0ffffffff82345aa5h");
	test_option!(true, true, true, true, true, true, true, false, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0FFFFFFFF82345AA5h");
	test_option!(false, false, false, false, false, false, false, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax,0xffffffff82345aa5");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x05\xA5\x5A\x34\x82", "add rax, 0xFFFFFFFF82345AA5");
	// rip_relative_addresses (64-bit)
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[12345678acf13570h]");
	test_option!(true, false, true, true, true, true, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[0x12345678ACF13570]");
	test_option!(false, true, false, false, false, false, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[rip+12345678h]");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[rip+0x12345678]");
	// rip_relative_addresses (32-bit)
	test_option!(false, false, false, false, false, false, false, false, b"\x67\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[0acf13571h]");
	test_option!(true, false, true, true, true, true, true, true, b"\x67\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[0xACF13571]");
	test_option!(false, true, false, false, false, false, false, false, b"\x67\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[eip+12345678h]");
	test_option!(true, true, true, true, true, true, true, true, b"\x67\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[eip+0x12345678]");
	// always_show_memory_size
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[12345678acf13570h]");
	test_option!(true, true, true, true, true, false, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, ds:[rip+0x12345678]");
	test_option!(false, false, false, false, false, true, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,qword ptr [12345678acf13570h]");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[rip+0x12345678]");
	// always_show_segment_register
	test_option!(false, false, false, false, false, false, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,[12345678acf13570h]");
	test_option!(true, true, true, true, false, true, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr [rip+0x12345678]");
	test_option!(false, false, false, false, true, false, false, false, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx,ds:[12345678acf13570h]");
	test_option!(true, true, true, true, true, true, true, true, b"\x48\x8B\x0D\x78\x56\x34\x12", "mov rcx, qword ptr ds:[rip+0x12345678]");
	// use_pseudo_ops
	test_option!(false, false, false, false, false, false, false, false, b"\x0F\xC2\xCD\x01", "cmpps xmm1,xmm5,1h");
	test_option!(true, true, false, true, true, true, true, true, b"\x0F\xC2\xCD\x01", "cmpps xmm1, xmm5, 0x1");
	test_option!(false, false, true, false, false, false, false, false, b"\x0F\xC2\xCD\x01", "cmpltps xmm1,xmm5");
	test_option!(true, true, true, true, true, true, true, true, b"\x0F\xC2\xCD\x01", "cmpltps xmm1, xmm5");
	// Not tested: show_symbol_address
}
