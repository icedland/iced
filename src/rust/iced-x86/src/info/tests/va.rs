// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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

		#[allow(deprecated)]
		{
			let value1 = used_mem.virtual_address(tc.element_index, |register, element_index, element_size| {
				for reg_value in &tc.register_values {
					if (reg_value.register, reg_value.element_index, reg_value.element_size) == (register, element_index, element_size) {
						return reg_value.value;
					}
				}
				unreachable!();
			});
			assert_eq!(tc.expected_value, value1);
		}

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
