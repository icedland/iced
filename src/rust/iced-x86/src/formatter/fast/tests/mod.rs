// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod fmt_factory;
mod misc;
mod options;
mod symres;

use self::fmt_factory::*;
use crate::formatter::tests::formatter_test_fast;
#[cfg(feature = "encoder")]
use crate::formatter::tests::formatter_test_nondec_fast;

#[test]
fn fmt_default_16() {
	formatter_test_fast(16, "Fast", "Default", false, create_default);
}

#[test]
fn fmt_inverted_16() {
	formatter_test_fast(16, "Fast", "Inverted", false, create_inverted);
}

#[test]
fn fmt_misc_16() {
	formatter_test_fast(16, "Fast", "Misc", true, create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_default_16() {
	formatter_test_nondec_fast(16, "Fast", "NonDec_Default", create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_inverted_16() {
	formatter_test_nondec_fast(16, "Fast", "NonDec_Inverted", create_inverted);
}

#[test]
fn fmt_default_32() {
	formatter_test_fast(32, "Fast", "Default", false, create_default);
}

#[test]
fn fmt_inverted_32() {
	formatter_test_fast(32, "Fast", "Inverted", false, create_inverted);
}

#[test]
fn fmt_misc_32() {
	formatter_test_fast(32, "Fast", "Misc", true, create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_default_32() {
	formatter_test_nondec_fast(32, "Fast", "NonDec_Default", create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_inverted_32() {
	formatter_test_nondec_fast(32, "Fast", "NonDec_Inverted", create_inverted);
}

#[test]
fn fmt_default_64() {
	formatter_test_fast(64, "Fast", "Default", false, create_default);
}

#[test]
fn fmt_inverted_64() {
	formatter_test_fast(64, "Fast", "Inverted", false, create_inverted);
}

#[test]
fn fmt_misc_64() {
	formatter_test_fast(64, "Fast", "Misc", true, create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_default_64() {
	formatter_test_nondec_fast(64, "Fast", "NonDec_Default", create_default);
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_inverted_64() {
	formatter_test_nondec_fast(64, "Fast", "NonDec_Inverted", create_inverted);
}
