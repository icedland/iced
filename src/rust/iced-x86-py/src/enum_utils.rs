// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;
use std::convert::TryFrom;

macro_rules! mk_to_enum {
	($name:ident, $enum_ty:ty, $name_str:literal) => {
		pub(super) fn $name(value: u32) -> PyResult<$enum_ty> {
			<$enum_ty as TryFrom<usize>>::try_from(value as usize).map_err(|_| PyValueError::new_err(concat!("Invalid ", $name_str, " value")))
		}
	};
}

mk_to_enum! {to_register, iced_x86::Register, "Register"}
mk_to_enum! {to_rounding_control, iced_x86::RoundingControl, "RoundingControl"}
mk_to_enum! {to_code_size, iced_x86::CodeSize, "CodeSize"}
mk_to_enum! {to_code, iced_x86::Code, "Code"}
mk_to_enum! {to_op_kind, iced_x86::OpKind, "OpKind"}
mk_to_enum! {to_memory_size_options, iced_x86::MemorySizeOptions, "MemorySizeOptions"}
mk_to_enum! {to_cc_b, iced_x86::CC_b, "CC_b"}
mk_to_enum! {to_cc_ae, iced_x86::CC_ae, "CC_ae"}
mk_to_enum! {to_cc_e, iced_x86::CC_e, "CC_e"}
mk_to_enum! {to_cc_ne, iced_x86::CC_ne, "CC_ne"}
mk_to_enum! {to_cc_be, iced_x86::CC_be, "CC_be"}
mk_to_enum! {to_cc_a, iced_x86::CC_a, "CC_a"}
mk_to_enum! {to_cc_p, iced_x86::CC_p, "CC_p"}
mk_to_enum! {to_cc_np, iced_x86::CC_np, "CC_np"}
mk_to_enum! {to_cc_l, iced_x86::CC_l, "CC_l"}
mk_to_enum! {to_cc_ge, iced_x86::CC_ge, "CC_ge"}
mk_to_enum! {to_cc_le, iced_x86::CC_le, "CC_le"}
mk_to_enum! {to_cc_g, iced_x86::CC_g, "CC_g"}
mk_to_enum! {to_memory_size, iced_x86::MemorySize, "MemorySize"}
mk_to_enum! {to_rep_prefix_kind, iced_x86::RepPrefixKind, "RepPrefixKind"}
mk_to_enum! {to_mvex_reg_mem_conv, iced_x86::MvexRegMemConv, "MvexRegMemConvRegister"}
