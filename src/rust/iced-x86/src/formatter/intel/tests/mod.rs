/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

mod fmt_factory;
mod number;
mod options;
mod registers;

use self::fmt_factory::*;
use super::super::tests::formatter_test;
#[cfg(feature = "encoder")]
use super::super::tests::formatter_test_nondec;

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
