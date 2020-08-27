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
mod misc;
mod options;
mod symres;

use self::fmt_factory::*;
use super::super::tests::formatter_test_fast;
#[cfg(feature = "encoder")]
use super::super::tests::formatter_test_nondec_fast;

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
