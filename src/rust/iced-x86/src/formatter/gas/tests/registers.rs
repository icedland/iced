// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::tests::registers::register_tests;
use super::fmt_factory;

#[test]
fn test_regs1() {
	register_tests("Gas", "RegisterTests_1", || fmt_factory::create_registers(false));
}

#[test]
fn test_regs2() {
	register_tests("Gas", "RegisterTests_2", || fmt_factory::create_registers(true));
}
