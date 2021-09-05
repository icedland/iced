// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::test::va_test_cases::VA_TEST_CASES;
use crate::test_utils::from_str_conv::to_vec_u8;
use crate::test_utils::{create_decoder, get_default_ip};

#[test]
fn va_tests() {
	for tc in &*VA_TEST_CASES {
		if tc.operand < 0 {
			continue;
		}
		let operand = tc.operand as u32;
		let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &bytes, get_default_ip(tc.bitness), tc.decoder_options).0;
		let instruction = decoder.decode();

		let value1 = instruction.virtual_address(operand, tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return Some(reg_value.value);
				}
			}
			None
		});
		assert_eq!(value1, Some(tc.expected_value));

		let value2 = instruction.try_virtual_address(operand, tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return Some(reg_value.value);
				}
			}
			None
		});
		assert_eq!(value2, Some(tc.expected_value));

		let value3 = instruction.virtual_address(operand, tc.element_index, |_register, _element_index, _element_size| None);
		assert_eq!(value3, None);

		let value4 = instruction.try_virtual_address(operand, tc.element_index, |_register, _element_index, _element_size| None);
		assert_eq!(value4, None);
	}
}
