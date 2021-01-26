// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::tests::registers::register_tests;
use super::fmt_factory;

#[test]
fn test_regs() {
	register_tests("Nasm", "RegisterTests", || fmt_factory::create_registers());
}
