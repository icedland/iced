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

use self::fmt_factory::*;
use super::super::tests::formatter_test;
#[cfg(feature = "encoder")]
use super::super::tests::formatter_test_nondec;

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
