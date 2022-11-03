// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::tests::decoder_mem_test_case::*;
use crate::decoder::tests::decoder_test_case::*;
use crate::decoder::tests::mem_test_parser::*;
use crate::decoder::tests::test_parser::*;
use crate::test_utils::*;
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;

fn read_decoder_test_cases_core(bitness: u32, filename: String) -> Vec<DecoderTestCase> {
	let mut path = get_decoder_unit_tests_dir();
	path.push(filename);
	DecoderTestParser::new(bitness, path.as_path()).into_iter().collect()
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
	DecoderMemoryTestParser::new(bitness, filename.as_path()).into_iter().collect()
}

pub(crate) fn get_test_cases(bitness: u32) -> &'static Vec<DecoderTestCase> {
	match bitness {
		16 => &TEST_CASES_16,
		32 => &TEST_CASES_32,
		64 => &TEST_CASES_64,
		_ => unreachable!(),
	}
}

pub(crate) fn get_misc_test_cases(bitness: u32) -> &'static Vec<DecoderTestCase> {
	match bitness {
		16 => &MISC_TEST_CASES_16,
		32 => &MISC_TEST_CASES_32,
		64 => &MISC_TEST_CASES_64,
		_ => unreachable!(),
	}
}

pub(crate) fn get_mem_test_cases(bitness: u32) -> &'static Vec<DecoderMemoryTestCase> {
	match bitness {
		16 => &TEST_CASES_MEM_16,
		32 => &TEST_CASES_MEM_32,
		64 => &TEST_CASES_MEM_64,
		_ => unreachable!(),
	}
}

lazy_static! {
	static ref TEST_CASES_16: Vec<DecoderTestCase> = read_decoder_test_cases(16);
}
lazy_static! {
	static ref TEST_CASES_32: Vec<DecoderTestCase> = read_decoder_test_cases(32);
}
lazy_static! {
	static ref TEST_CASES_64: Vec<DecoderTestCase> = read_decoder_test_cases(64);
}

lazy_static! {
	static ref MISC_TEST_CASES_16: Vec<DecoderTestCase> = read_decoder_misc_test_cases(16);
}
lazy_static! {
	static ref MISC_TEST_CASES_32: Vec<DecoderTestCase> = read_decoder_misc_test_cases(32);
}
lazy_static! {
	static ref MISC_TEST_CASES_64: Vec<DecoderTestCase> = read_decoder_misc_test_cases(64);
}

lazy_static! {
	static ref TEST_CASES_MEM_16: Vec<DecoderMemoryTestCase> = read_decoder_mem_test_cases(16);
}
lazy_static! {
	static ref TEST_CASES_MEM_32: Vec<DecoderMemoryTestCase> = read_decoder_mem_test_cases(32);
}
lazy_static! {
	static ref TEST_CASES_MEM_64: Vec<DecoderMemoryTestCase> = read_decoder_mem_test_cases(64);
}
