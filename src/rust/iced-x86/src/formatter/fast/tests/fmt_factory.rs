// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::{SpecializedFormatter, SpecializedFormatterTraitOptions, SymbolResolver};
use alloc::boxed::Box;

pub(super) fn create_default<TraitOptions: SpecializedFormatterTraitOptions>() -> Box<SpecializedFormatter<TraitOptions>> {
	Box::new(SpecializedFormatter::<TraitOptions>::new())
}

pub(super) fn create_inverted<TraitOptions: SpecializedFormatterTraitOptions>() -> Box<SpecializedFormatter<TraitOptions>> {
	let mut fmt = SpecializedFormatter::<TraitOptions>::new();

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

pub(super) fn create_options<TraitOptions: SpecializedFormatterTraitOptions>() -> Box<SpecializedFormatter<TraitOptions>> {
	let mut fmt = SpecializedFormatter::<TraitOptions>::new();
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}

pub(super) fn create_resolver<TraitOptions: SpecializedFormatterTraitOptions>(
	symbol_resolver: Box<dyn SymbolResolver>,
) -> Box<SpecializedFormatter<TraitOptions>> {
	let mut fmt = SpecializedFormatter::<TraitOptions>::try_with_options(Some(symbol_resolver)).unwrap();
	fmt.options_mut().set_rip_relative_addresses(true);
	Box::new(fmt)
}
