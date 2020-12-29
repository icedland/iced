/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::super::iced_constants::IcedConstants;
use super::super::Register;
use std::panic;

#[test]
#[allow(clippy::identity_op)]
fn test_reg_add_ops() {
	assert_eq!(Register::AX, 0i32 + Register::AX);
	assert_eq!(Register::AX, 0u32 + Register::AX);
	assert_eq!(Register::AX, Register::AX + 0i32);
	assert_eq!(Register::EAX, Register::EAX + 0u32);

	assert_eq!(Register::BX, 3i32 + Register::AX);
	assert_eq!(Register::EBX, 3u32 + Register::EAX);
	assert_eq!(Register::BX, Register::AX + 3i32);
	assert_eq!(Register::EBX, Register::EAX + 3u32);

	let mut reg = Register::AX;
	reg += 3i32;
	assert_eq!(Register::BX, reg);

	let mut reg = Register::EAX;
	reg += 3u32;
	assert_eq!(Register::EBX, reg);
}

#[test]
#[allow(clippy::identity_op)]
fn test_reg_sub_ops() {
	assert_eq!(Register::SP, Register::SP - 0i32);
	assert_eq!(Register::ESP, Register::ESP - 0u32);

	assert_eq!(Register::DX, Register::SP - 2i32);
	assert_eq!(Register::EDX, Register::ESP - 2u32);

	let mut reg = Register::SP;
	reg -= 2i32;
	assert_eq!(Register::DX, reg);

	let mut reg = Register::ESP;
	reg -= 2u32;
	assert_eq!(Register::EDX, reg);
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
