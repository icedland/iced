// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;

#[derive(Default)]
pub(super) struct RegisterInfoTestCase {
	#[allow(dead_code)]
	pub(super) line_number: u32,
	pub(super) register: Register,
	pub(super) number: usize,
	pub(super) base: Register,
	pub(super) full_register: Register,
	pub(super) full_register32: Register,
	pub(super) size: usize,
	pub(super) flags: u32, // RegisterFlags
}
