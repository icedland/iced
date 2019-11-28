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

use super::super::super::test_utils::*;
use super::decoder_mem_test_case::*;
use super::decoder_test_case::*;
use super::mem_test_parser::*;
use super::test_parser::*;

fn read_decoder_test_cases_core(bitness: u32, filename: String) -> Vec<DecoderTestCase> {
	let mut path = get_decoder_unit_tests_dir();
	path.push(filename);
	let parser = DecoderTestParser::new(bitness, path.as_path());
	let mut v = Vec::new();
	v.extend(parser);
	v
}

fn read_decoder_test_cases(bitness: u32) -> Vec<DecoderTestCase> {
	read_decoder_test_cases_core(bitness, format!("DecoderTest{}.txt", bitness))
}

fn read_decoder_misc_test_cases(bitness: u32) -> Vec<DecoderTestCase> {
	read_decoder_test_cases_core(bitness, format!("DecoderTestMisc{}.txt", bitness))
}

fn read_decoder_mem_test_cases(bitness: u32) -> Vec<DecoderMemoryTestCase> {
	let mut filename = get_decoder_unit_tests_dir();
	filename.push(format!("MemoryTest{}.txt", bitness));
	let parser = DecoderMemoryTestParser::new(bitness, filename.as_path());
	let mut v = Vec::new();
	v.extend(parser);
	v
}

pub(crate) fn get_test_cases(bitness: u32) -> &'static Vec<DecoderTestCase> {
	match bitness {
		16 => &*TEST_CASES_16,
		32 => &*TEST_CASES_32,
		64 => &*TEST_CASES_64,
		_ => panic!(),
	}
}

pub(crate) fn get_misc_test_cases(bitness: u32) -> &'static Vec<DecoderTestCase> {
	match bitness {
		16 => &*MISC_TEST_CASES_16,
		32 => &*MISC_TEST_CASES_32,
		64 => &*MISC_TEST_CASES_64,
		_ => panic!(),
	}
}

pub(crate) fn get_mem_test_cases(bitness: u32) -> &'static Vec<DecoderMemoryTestCase> {
	match bitness {
		16 => &*TEST_CASES_MEM_16,
		32 => &*TEST_CASES_MEM_32,
		64 => &*TEST_CASES_MEM_64,
		_ => panic!(),
	}
}

lazy_static! {
	static ref TEST_CASES_16: Vec<DecoderTestCase> = { read_decoder_test_cases(16) };
}
lazy_static! {
	static ref TEST_CASES_32: Vec<DecoderTestCase> = { read_decoder_test_cases(32) };
}
lazy_static! {
	static ref TEST_CASES_64: Vec<DecoderTestCase> = { read_decoder_test_cases(64) };
}

lazy_static! {
	static ref MISC_TEST_CASES_16: Vec<DecoderTestCase> = { read_decoder_misc_test_cases(16) };
}
lazy_static! {
	static ref MISC_TEST_CASES_32: Vec<DecoderTestCase> = { read_decoder_misc_test_cases(32) };
}
lazy_static! {
	static ref MISC_TEST_CASES_64: Vec<DecoderTestCase> = { read_decoder_misc_test_cases(64) };
}

lazy_static! {
	static ref TEST_CASES_MEM_16: Vec<DecoderMemoryTestCase> = { read_decoder_mem_test_cases(16) };
}
lazy_static! {
	static ref TEST_CASES_MEM_32: Vec<DecoderMemoryTestCase> = { read_decoder_mem_test_cases(32) };
}
lazy_static! {
	static ref TEST_CASES_MEM_64: Vec<DecoderMemoryTestCase> = { read_decoder_mem_test_cases(64) };
}
