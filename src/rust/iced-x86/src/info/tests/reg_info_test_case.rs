// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

use super::super::super::*;

#[derive(Default)]
pub(super) struct RegisterInfoTestCase {
	pub(super) line_number: u32,
	pub(super) register: Register,
	pub(super) number: usize,
	pub(super) base: Register,
	pub(super) full_register: Register,
	pub(super) full_register32: Register,
	pub(super) size: usize,
	pub(super) flags: u32, // RegisterFlags
}
