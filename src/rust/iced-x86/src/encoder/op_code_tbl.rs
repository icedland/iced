// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::iced_constants::IcedConstants;
use super::super::{Code, OpCodeInfo};
use super::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use super::op_code_data::{OPC_FLAGS1, OPC_FLAGS2};
use alloc::string::String;
use alloc::vec::Vec;
use core::mem;
use lazy_static::lazy_static;

lazy_static! {
	pub(crate) static ref OP_CODE_INFO_TBL: Vec<OpCodeInfo> = {
		let mut result = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
		let mut sb = String::new();
		for i in 0..IcedConstants::CODE_ENUM_COUNT {
			// SAFETY: All values 0-max are valid Code enum values
			let code: Code = unsafe { mem::transmute(i as u16) };
			let enc_flags1 = ENC_FLAGS1[i];
			let enc_flags2 = ENC_FLAGS2[i];
			let enc_flags3 = ENC_FLAGS3[i];
			let opc_flags1 = OPC_FLAGS1[i];
			let opc_flags2 = OPC_FLAGS2[i];
			result.push(OpCodeInfo::new(code, enc_flags1, enc_flags2, enc_flags3, opc_flags1, opc_flags2, &mut sb));
		}
		result
	};
}
