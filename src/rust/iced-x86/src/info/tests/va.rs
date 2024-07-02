// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::info::tests::test::va_test_cases::VA_TEST_CASES;
use crate::info::tests::InstructionInfoFactory;
use crate::test_utils::from_str_conv::to_vec_u8;
use crate::test_utils::{create_decoder, get_default_ip};

#[test]
fn va_tests() {
	for tc in &*VA_TEST_CASES {
		if tc.used_mem_index < 0 {
			continue;
		}
		let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &bytes, get_default_ip(tc.bitness), tc.decoder_options).0;
		let instruction = decoder.decode();

		let mut factory = InstructionInfoFactory::new();
		let info = factory.info(&instruction);
		let used_mem = info.used_memory().get(tc.used_mem_index as usize).unwrap();

		let value1 = used_mem.virtual_address(tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return Some(reg_value.value);
				}
			}
			None
		});
		assert_eq!(value1, Some(tc.expected_value));

		let value2 = used_mem.try_virtual_address(tc.element_index, |register, element_index, element_size| {
			for reg_value in &tc.register_values {
				if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
					return Some(reg_value.value);
				}
			}
			None
		});
		assert_eq!(value2, Some(tc.expected_value));

		let value3 = used_mem.virtual_address(tc.element_index, |_register, _element_index, _element_size| None);
		assert_eq!(value3, None);

		let value4 = used_mem.try_virtual_address(tc.element_index, |_register, _element_index, _element_size| None);
		assert_eq!(value4, None);
	}
}
