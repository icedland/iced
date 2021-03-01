// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::iced_constants::IcedConstants;

pub(super) struct Registers;
impl Registers {
	pub(super) const REGISTER_ST: u32 = IcedConstants::REGISTER_ENUM_COUNT as u32;
	#[allow(dead_code)]
	pub(super) const EXTRA_REGISTERS: u32 = 1;
}
