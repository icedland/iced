// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use crate::encoder::op_code_data::{OPC_FLAGS1, OPC_FLAGS2};
use crate::iced_constants::IcedConstants;
use crate::{Code, OpCodeInfo};
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;

lazy_static! {
	pub(crate) static ref OP_CODE_INFO_TBL: Vec<OpCodeInfo> = {
		let mut result = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
		let mut sb = String::new();
		for code in Code::values() {
			let i = code as usize;
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
