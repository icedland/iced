// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod fmt_factory;
mod misc;
mod number;
mod options;
mod registers;
mod symres;

use crate::formatter::gas::tests::fmt_factory::*;
use crate::formatter::tests::formatter_test;
#[cfg(feature = "encoder")]
use crate::formatter::tests::formatter_test_nondec;

#[test]
fn fmt_forcesuffix_16() {
	formatter_test(16, "Gas", "ForceSuffix", false, || create_forcesuffix());
}

#[test]
fn fmt_nosuffix_16() {
	formatter_test(16, "Gas", "NoSuffix", false, || create_nosuffix());
}

#[test]
fn fmt_misc_16() {
	formatter_test(16, "Gas", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_forcesuffix_16() {
	formatter_test_nondec(16, "Gas", "NonDec_ForceSuffix", || create_forcesuffix());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_nosuffix_16() {
	formatter_test_nondec(16, "Gas", "NonDec_NoSuffix", || create_nosuffix());
}

#[test]
fn fmt_forcesuffix_32() {
	formatter_test(32, "Gas", "ForceSuffix", false, || create_forcesuffix());
}

#[test]
fn fmt_nosuffix_32() {
	formatter_test(32, "Gas", "NoSuffix", false, || create_nosuffix());
}

#[test]
fn fmt_misc_32() {
	formatter_test(32, "Gas", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_forcesuffix_32() {
	formatter_test_nondec(32, "Gas", "NonDec_ForceSuffix", || create_forcesuffix());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_nosuffix_32() {
	formatter_test_nondec(32, "Gas", "NonDec_NoSuffix", || create_nosuffix());
}

#[test]
fn fmt_forcesuffix_64() {
	formatter_test(64, "Gas", "ForceSuffix", false, || create_forcesuffix());
}

#[test]
fn fmt_nosuffix_64() {
	formatter_test(64, "Gas", "NoSuffix", false, || create_nosuffix());
}

#[test]
fn fmt_misc_64() {
	formatter_test(64, "Gas", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_forcesuffix_64() {
	formatter_test_nondec(64, "Gas", "NonDec_ForceSuffix", || create_forcesuffix());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_nosuffix_64() {
	formatter_test_nondec(64, "Gas", "NonDec_NoSuffix", || create_nosuffix());
}
