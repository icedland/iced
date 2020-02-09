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

use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::test_utils::{create_decoder, get_instruction_unit_tests_dir};
use super::va_test_parser::*;

#[test]
fn va_tests() {
	let mut path = get_instruction_unit_tests_dir();
	path.push("VirtualAddressTests.txt");
	for tc in VirtualAddressTestParser::new(&path) {
		let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &bytes, 0).0;
		let instruction = decoder.decode();
		let value1 = instruction.virtual_address(tc.operand, tc.element_index, |register, element_index, element_size| {
			for reg_value in tc.register_values.iter() {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return reg_value.value;
				}
			}
			panic!();
		});
		assert_eq!(tc.expected_value, value1);
	}
}
