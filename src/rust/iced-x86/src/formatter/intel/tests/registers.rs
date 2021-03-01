// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::intel::tests::fmt_factory;
use crate::formatter::tests::registers::register_tests;

#[test]
fn test_regs() {
	register_tests("Intel", "RegisterTests", || fmt_factory::create_registers());
}
