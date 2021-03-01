// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::gas::tests::fmt_factory;
use crate::formatter::tests::registers::register_tests;

#[test]
fn test_regs1() {
	register_tests("Gas", "RegisterTests_1", || fmt_factory::create_registers(false));
}

#[test]
fn test_regs2() {
	register_tests("Gas", "RegisterTests_2", || fmt_factory::create_registers(true));
}
