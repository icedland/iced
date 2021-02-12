// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

use super::super::super::tests::registers::register_tests;
use super::fmt_factory;

#[test]
fn test_regs() {
	register_tests("Masm", "RegisterTests", || fmt_factory::create_registers());
}
