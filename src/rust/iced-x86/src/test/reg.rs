// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::iced_constants::IcedConstants;
use crate::Register;
use std::panic;

#[test]
#[allow(clippy::identity_op)]
fn test_reg_add_ops() {
	assert_eq!(0i32 + Register::AX, Register::AX);
	assert_eq!(0u32 + Register::AX, Register::AX);
	assert_eq!(Register::AX + 0i32, Register::AX);
	assert_eq!(Register::EAX + 0u32, Register::EAX);

	assert_eq!(3i32 + Register::AX, Register::BX);
	assert_eq!(3u32 + Register::EAX, Register::EBX);
	assert_eq!(Register::AX + 3i32, Register::BX);
	assert_eq!(Register::EAX + 3u32, Register::EBX);

	let mut reg = Register::AX;
	reg += 3i32;
	assert_eq!(reg, Register::BX);

	let mut reg = Register::EAX;
	reg += 3u32;
	assert_eq!(reg, Register::EBX);
}

#[test]
#[allow(clippy::identity_op)]
fn test_reg_sub_ops() {
	assert_eq!(Register::SP - 0i32, Register::SP);
	assert_eq!(Register::ESP - 0u32, Register::ESP);

	assert_eq!(Register::SP - 2i32, Register::DX);
	assert_eq!(Register::ESP - 2u32, Register::EDX);

	let mut reg = Register::SP;
	reg -= 2i32;
	assert_eq!(reg, Register::DX);

	let mut reg = Register::ESP;
	reg -= 2u32;
	assert_eq!(reg, Register::EDX);
}

#[test]
fn test_reg_add_ops_panics() {
	assert!(panic::catch_unwind(|| Register::None + IcedConstants::REGISTER_ENUM_COUNT as i32).is_err());
	assert!(panic::catch_unwind(|| Register::None + IcedConstants::REGISTER_ENUM_COUNT as u32).is_err());
	assert!(panic::catch_unwind(|| IcedConstants::REGISTER_ENUM_COUNT as i32 + Register::None).is_err());
	assert!(panic::catch_unwind(|| IcedConstants::REGISTER_ENUM_COUNT as u32 + Register::None).is_err());
	assert!(panic::catch_unwind(|| {
		let mut reg = Register::None;
		reg += IcedConstants::REGISTER_ENUM_COUNT as i32;
	})
	.is_err());
	assert!(panic::catch_unwind(|| {
		let mut reg = Register::None;
		reg += IcedConstants::REGISTER_ENUM_COUNT as u32;
	})
	.is_err());
}

#[test]
fn test_reg_sub_ops_panics() {
	assert!(panic::catch_unwind(|| Register::None - 1i32).is_err());
	assert!(panic::catch_unwind(|| Register::None - 1u32).is_err());
	assert!(panic::catch_unwind(|| {
		let mut reg = Register::None;
		reg -= 1i32;
	})
	.is_err());
	assert!(panic::catch_unwind(|| {
		let mut reg = Register::None;
		reg -= 1u32;
	})
	.is_err());
}
