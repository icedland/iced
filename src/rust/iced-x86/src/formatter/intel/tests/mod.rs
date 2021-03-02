// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod fmt_factory;
mod misc;
mod number;
mod options;
mod registers;
mod symres;

use crate::formatter::intel::tests::fmt_factory::*;
use crate::formatter::tests::formatter_test;
#[cfg(feature = "encoder")]
use crate::formatter::tests::formatter_test_nondec;

#[test]
fn fmt_memalways_16() {
	formatter_test(16, "Intel", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_16() {
	formatter_test(16, "Intel", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_16() {
	formatter_test(16, "Intel", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_16() {
	formatter_test(16, "Intel", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_16() {
	formatter_test_nondec(16, "Intel", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_16() {
	formatter_test_nondec(16, "Intel", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_16() {
	formatter_test_nondec(16, "Intel", "NonDec_MemMinimum", || create_memminimum());
}

#[test]
fn fmt_memalways_32() {
	formatter_test(32, "Intel", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_32() {
	formatter_test(32, "Intel", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_32() {
	formatter_test(32, "Intel", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_32() {
	formatter_test(32, "Intel", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_32() {
	formatter_test_nondec(32, "Intel", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_32() {
	formatter_test_nondec(32, "Intel", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_32() {
	formatter_test_nondec(32, "Intel", "NonDec_MemMinimum", || create_memminimum());
}

#[test]
fn fmt_memalways_64() {
	formatter_test(64, "Intel", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_64() {
	formatter_test(64, "Intel", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_64() {
	formatter_test(64, "Intel", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_64() {
	formatter_test(64, "Intel", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_64() {
	formatter_test_nondec(64, "Intel", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_64() {
	formatter_test_nondec(64, "Intel", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_64() {
	formatter_test_nondec(64, "Intel", "NonDec_MemMinimum", || create_memminimum());
}
