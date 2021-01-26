// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::*;

#[derive(Default)]
pub(super) struct MemorySizeInfoTestCase {
	pub(super) line_number: u32,
	pub(super) memory_size: MemorySize,
	pub(super) size: usize,
	pub(super) element_size: usize,
	pub(super) element_type: MemorySize,
	pub(super) element_count: usize,
	pub(super) flags: u32, // MemorySizeFlags
}
