// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::super::tests::registers::register_tests;
use super::fmt_factory;

#[test]
fn test_regs() {
	register_tests("Masm", "RegisterTests", || fmt_factory::create_registers());
}
