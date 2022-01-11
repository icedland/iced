// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::{DefaultFastFormatterTraitOptions, FastFormatterOptions, SpecializedFormatterTraitOptions};

// Since SpecializedFormatterTraitOptions::__IS_FAST_FORMATTER exists, we have code
// paths that aren't tested completely. We create a new FastFormatter with that const
// set to false so these new paths are tested.

pub(super) struct NotFastFormatterTraitOptions;
impl SpecializedFormatterTraitOptions for NotFastFormatterTraitOptions {
	const __IS_FAST_FORMATTER: bool = false;
	const ENABLE_SYMBOL_RESOLVER: bool = DefaultFastFormatterTraitOptions::ENABLE_SYMBOL_RESOLVER;
	const ENABLE_DB_DW_DD_DQ: bool = DefaultFastFormatterTraitOptions::ENABLE_DB_DW_DD_DQ;
	unsafe fn verify_output_has_enough_bytes_left() -> bool {
		unsafe { DefaultFastFormatterTraitOptions::verify_output_has_enough_bytes_left() }
	}
	fn space_after_operand_separator(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::space_after_operand_separator(options)
	}
	fn rip_relative_addresses(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::rip_relative_addresses(options)
	}
	fn use_pseudo_ops(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::use_pseudo_ops(options)
	}
	fn show_symbol_address(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::show_symbol_address(options)
	}
	fn always_show_segment_register(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::always_show_segment_register(options)
	}
	fn always_show_memory_size(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::always_show_memory_size(options)
	}
	fn uppercase_hex(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::uppercase_hex(options)
	}
	fn use_hex_prefix(options: &FastFormatterOptions) -> bool {
		DefaultFastFormatterTraitOptions::use_hex_prefix(options)
	}
}
