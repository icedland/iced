// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

use super::super::super::FastFormatter;
use super::super::super::SymbolResolver;
use alloc::boxed::Box;

pub(super) fn create_default() -> Box<FastFormatter> {
	Box::new(FastFormatter::new())
}

pub(super) fn create_inverted() -> Box<FastFormatter> {
	let mut fmt = FastFormatter::new();

	let opt = fmt.options().space_after_operand_separator() ^ true;
	fmt.options_mut().set_space_after_operand_separator(opt);

	let opt = fmt.options().rip_relative_addresses() ^ true;
	fmt.options_mut().set_rip_relative_addresses(opt);

	let opt = fmt.options().use_pseudo_ops() ^ true;
	fmt.options_mut().set_use_pseudo_ops(opt);

	let opt = fmt.options().show_symbol_address() ^ true;
	fmt.options_mut().set_show_symbol_address(opt);

	let opt = fmt.options().always_show_segment_register() ^ true;
	fmt.options_mut().set_always_show_segment_register(opt);

	let opt = fmt.options().always_show_memory_size() ^ true;
	fmt.options_mut().set_always_show_memory_size(opt);

	let opt = fmt.options().uppercase_hex() ^ true;
	fmt.options_mut().set_uppercase_hex(opt);

	let opt = fmt.options().use_hex_prefix() ^ true;
	fmt.options_mut().set_use_hex_prefix(opt);

	Box::new(fmt)
}

pub(super) fn create_options() -> Box<FastFormatter> {
	let mut fmt = FastFormatter::new();
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}

pub(super) fn create_resolver(symbol_resolver: Box<dyn SymbolResolver>) -> Box<FastFormatter> {
	let mut fmt = FastFormatter::with_options(Some(symbol_resolver));
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}
