// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod fmt_factory;
mod misc;
mod number;
mod options;
mod registers;
mod sym_opts;
mod sym_opts_parser;
mod symres;

use crate::formatter::masm::tests::fmt_factory::*;
use crate::formatter::tests::formatter_test;
#[cfg(feature = "encoder")]
use crate::formatter::tests::formatter_test_nondec;

#[test]
fn fmt_memalways_16() {
	formatter_test(16, "Masm", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_16() {
	formatter_test(16, "Masm", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_16() {
	formatter_test(16, "Masm", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_16() {
	formatter_test(16, "Masm", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_16() {
	formatter_test_nondec(16, "Masm", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_16() {
	formatter_test_nondec(16, "Masm", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_16() {
	formatter_test_nondec(16, "Masm", "NonDec_MemMinimum", || create_memminimum());
}

#[test]
fn fmt_memalways_32() {
	formatter_test(32, "Masm", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_32() {
	formatter_test(32, "Masm", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_32() {
	formatter_test(32, "Masm", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_32() {
	formatter_test(32, "Masm", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_32() {
	formatter_test_nondec(32, "Masm", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_32() {
	formatter_test_nondec(32, "Masm", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_32() {
	formatter_test_nondec(32, "Masm", "NonDec_MemMinimum", || create_memminimum());
}

#[test]
fn fmt_memalways_64() {
	formatter_test(64, "Masm", "MemAlways", false, || create_memalways());
}

#[test]
fn fmt_memdefault_64() {
	formatter_test(64, "Masm", "MemDefault", false, || create_memdefault());
}

#[test]
fn fmt_memminimum_64() {
	formatter_test(64, "Masm", "MemMinimum", false, || create_memminimum());
}

#[test]
fn fmt_misc_64() {
	formatter_test(64, "Masm", "Misc", true, || create());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memalways_64() {
	formatter_test_nondec(64, "Masm", "NonDec_MemAlways", || create_memalways());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memdefault_64() {
	formatter_test_nondec(64, "Masm", "NonDec_MemDefault", || create_memdefault());
}

#[test]
#[cfg(feature = "encoder")]
fn fmt_nondec_memminimum_64() {
	formatter_test_nondec(64, "Masm", "NonDec_MemMinimum", || create_memminimum());
}
