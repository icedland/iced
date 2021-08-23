// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use crate::encoder::op_code_data::{OPC_FLAGS1, OPC_FLAGS2};
use crate::iced_constants::IcedConstants;
use crate::{Code, OpCodeInfo};
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use core::convert::TryInto;
use lazy_static::lazy_static;

lazy_static! {
	pub(crate) static ref OP_CODE_INFO_TBL: Box<[OpCodeInfo; IcedConstants::CODE_ENUM_COUNT]> = {
		let mut result = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
		let mut sb = String::new();
		for code in Code::values() {
			let enc_flags1 = ENC_FLAGS1[code as usize];
			let enc_flags2 = ENC_FLAGS2[code as usize];
			let enc_flags3 = ENC_FLAGS3[code as usize];
			let opc_flags1 = OPC_FLAGS1[code as usize];
			let opc_flags2 = OPC_FLAGS2[code as usize];
			result.push(OpCodeInfo::new(code, enc_flags1, enc_flags2, enc_flags3, opc_flags1, opc_flags2, &mut sb));
		}

		#[allow(clippy::unwrap_used)]
		result.into_boxed_slice().try_into().ok().unwrap()
	};
}
