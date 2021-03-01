// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::FastFormatter;

#[test]
fn verify_default_formatter_options() {
	let fmt = FastFormatter::new();
	let options = fmt.options();
	assert!(!options.space_after_operand_separator());
	assert!(!options.always_show_segment_register());
	assert!(options.uppercase_hex());
	assert!(!options.use_hex_prefix());
	assert!(!options.always_show_memory_size());
	assert!(!options.rip_relative_addresses());
	assert!(options.use_pseudo_ops());
	assert!(!options.show_symbol_address());
}
