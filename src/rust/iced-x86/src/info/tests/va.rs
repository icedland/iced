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

use super::super::super::test_utils::create_decoder;
use super::super::super::test_utils::from_str_conv::to_vec_u8;
use super::test::va_test_cases::VA_TEST_CASES;
use super::InstructionInfoFactory;

#[test]
fn va_tests() {
	for tc in &*VA_TEST_CASES {
		if tc.used_mem_index < 0 {
			continue;
		}
		let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &bytes, tc.decoder_options).0;
		let instruction = decoder.decode();

		let mut factory = InstructionInfoFactory::new();
		let info = factory.info(&instruction);
		let used_mem = info.used_memory().iter().nth(tc.used_mem_index as usize).unwrap();

		let value1 = used_mem.virtual_address(tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return reg_value.value;
				}
			}
			unreachable!();
		});
		assert_eq!(tc.expected_value, value1);

		let value2 = used_mem.try_virtual_address(tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return Some(reg_value.value);
				}
			}
			None
		});
		assert_eq!(Some(tc.expected_value), value2);

		let value3 = used_mem.try_virtual_address(tc.element_index, |_register, _element_index, _element_size| None);
		assert_eq!(None, value3);
	}
}
