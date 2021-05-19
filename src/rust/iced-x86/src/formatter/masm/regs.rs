// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::Register;

pub(super) struct Registers;
impl Registers {
	// Should be 1 past the last real register (not including DontUseF9-DontUseFF)
	pub(super) const REGISTER_ST: u32 = Register::DontUseF9 as u32;
	#[allow(dead_code)]
	pub(super) const EXTRA_REGISTERS: u32 = 0;
}
