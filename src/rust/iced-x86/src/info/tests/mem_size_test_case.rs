// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;

#[derive(Default)]
pub(super) struct MemorySizeInfoTestCase {
	#[allow(dead_code)]
	pub(super) line_number: u32,
	pub(super) memory_size: MemorySize,
	pub(super) size: usize,
	pub(super) element_size: usize,
	pub(super) element_type: MemorySize,
	pub(super) element_count: usize,
	pub(super) flags: u32, // MemorySizeFlags
}
