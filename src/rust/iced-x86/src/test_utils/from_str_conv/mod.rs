// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod cc_table;
mod code_table;
#[cfg(feature = "instr_info")]
mod condition_code_table;
#[cfg(feature = "instr_info")]
mod cpuid_feature_table;
mod decoder_error_table;
#[cfg(any(feature = "decoder", feature = "instr_info", feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod decoder_options_table;
#[cfg(feature = "instr_info")]
mod encoding_kind_table;
#[cfg(feature = "instr_info")]
mod flow_control_table;
mod ignored_code_table;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod memory_size_options_table;
mod memory_size_table;
mod mnemonic_table;
#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
mod mvex_conv_fn_table;
#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
mod mvex_tt_lut_kind_table;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod number_base_table;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod op_code_operand_kind_table;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod options_props_table;
mod register_table;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod tuple_type_table;

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
use crate::formatter::tests::enums::OptionsProps;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::test_utils::from_str_conv::cc_table::*;
use crate::test_utils::from_str_conv::code_table::*;
#[cfg(feature = "instr_info")]
use crate::test_utils::from_str_conv::condition_code_table::*;
#[cfg(feature = "instr_info")]
use crate::test_utils::from_str_conv::cpuid_feature_table::*;
use crate::test_utils::from_str_conv::decoder_error_table::*;
#[cfg(any(feature = "decoder", feature = "instr_info", feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::test_utils::from_str_conv::decoder_options_table::*;
#[cfg(feature = "instr_info")]
use crate::test_utils::from_str_conv::encoding_kind_table::*;
#[cfg(feature = "instr_info")]
use crate::test_utils::from_str_conv::flow_control_table::*;
use crate::test_utils::from_str_conv::ignored_code_table::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
use crate::test_utils::from_str_conv::memory_size_options_table::*;
use crate::test_utils::from_str_conv::memory_size_table::*;
use crate::test_utils::from_str_conv::mnemonic_table::*;
#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
use crate::test_utils::from_str_conv::mvex_conv_fn_table::*;
#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
use crate::test_utils::from_str_conv::mvex_tt_lut_kind_table::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::test_utils::from_str_conv::number_base_table::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
use crate::test_utils::from_str_conv::op_code_operand_kind_table::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
use crate::test_utils::from_str_conv::options_props_table::*;
use crate::test_utils::from_str_conv::register_table::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
use crate::test_utils::from_str_conv::tuple_type_table::*;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
#[cfg(feature = "instr_info")]
use std::collections::HashMap;

pub(crate) fn to_vec_u8(hex_data: &str) -> Result<Vec<u8>, String> {
	let mut bytes = Vec::with_capacity(hex_data.len() / 2);
	let mut iter = hex_data.chars().filter(|c| !c.is_whitespace());
	loop {
		let hi = try_parse_hex_char(match iter.next() {
			Some(c) => c,
			None => break,
		});
		let lo = try_parse_hex_char(match iter.next() {
			Some(c) => c,
			None => return Err(format!("Missing hex digit in string: '{}'", hex_data)),
		});
		if hi < 0 || lo < 0 {
			return Err(format!("Invalid hex string: '{}'", hex_data));
		}
		bytes.push(((hi << 4) | lo) as u8);
	}

	fn try_parse_hex_char(c: char) -> i32 {
		if '0' <= c && c <= '9' {
			return c as i32 - '0' as i32;
		}
		if 'A' <= c && c <= 'F' {
			return c as i32 - 'A' as i32 + 10;
		}
		if 'a' <= c && c <= 'f' {
			return c as i32 - 'a' as i32 + 10;
		}
		-1
	}

	Ok(bytes)
}

pub(crate) fn to_u64(value: &str) -> Result<u64, String> {
	let value = value.trim();
	let result = if let Some(value) = value.strip_prefix("0x") { u64::from_str_radix(value, 16) } else { value.trim().parse() };
	match result {
		Ok(value) => Ok(value),
		Err(_) => Err(format!("Invalid number: {}", value)),
	}
}

pub(crate) fn to_i64(value: &str) -> Result<i64, String> {
	let mut unsigned_value = value.trim();
	let mult = if unsigned_value.starts_with('-') {
		unsigned_value = &unsigned_value[1..];
		-1
	} else {
		1
	};
	let result = if let Some(value) = unsigned_value.strip_prefix("0x") { u64::from_str_radix(value, 16) } else { unsigned_value.trim().parse() };
	match result {
		Ok(value) => Ok((value as i64).wrapping_mul(mult)),
		Err(_) => Err(format!("Invalid number: {}", value)),
	}
}

pub(crate) fn to_u32(value: &str) -> Result<u32, String> {
	let value = value.trim();
	if let Ok(v64) = to_u64(value) {
		if v64 <= u32::MAX as u64 {
			return Ok(v64 as u32);
		}
	}
	Err(format!("Invalid number: {}", value))
}

pub(crate) fn to_i32(value: &str) -> Result<i32, String> {
	let value = value.trim();
	if let Ok(v64) = to_i64(value) {
		if i32::MIN as i64 <= v64 && v64 <= i32::MAX as i64 {
			return Ok(v64 as i32);
		}
	}
	Err(format!("Invalid number: {}", value))
}

pub(crate) fn to_u16(value: &str) -> Result<u16, String> {
	let value = value.trim();
	if let Ok(v64) = to_u64(value) {
		if v64 <= u16::MAX as u64 {
			return Ok(v64 as u16);
		}
	}
	Err(format!("Invalid number: {}", value))
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_i16(value: &str) -> Result<i16, String> {
	let value = value.trim();
	if let Ok(v64) = to_i64(value) {
		if i16::MIN as i64 <= v64 && v64 <= i16::MAX as i64 {
			return Ok(v64 as i16);
		}
	}
	Err(format!("Invalid number: {}", value))
}

pub(crate) fn to_u8(value: &str) -> Result<u8, String> {
	let value = value.trim();
	if let Ok(v64) = to_u64(value) {
		if v64 <= u8::MAX as u64 {
			return Ok(v64 as u8);
		}
	}
	Err(format!("Invalid number: {}", value))
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_i8(value: &str) -> Result<i8, String> {
	let value = value.trim();
	if let Ok(v64) = to_i64(value) {
		if i8::MIN as i64 <= v64 && v64 <= i8::MAX as i64 {
			return Ok(v64 as i8);
		}
	}
	Err(format!("Invalid number: {}", value))
}

pub(crate) fn to_code(value: &str) -> Result<Code, String> {
	let value = value.trim();
	match TO_CODE_HASH.get(value) {
		Some(code) => Ok(*code),
		None => Err(format!("Invalid Code value: {}", value)),
	}
}

pub(crate) fn is_ignored_code(value: &str) -> bool {
	let value = value.trim();
	if cfg!(feature = "no_vex") && value.starts_with("VEX_") {
		return true;
	}
	if cfg!(feature = "no_evex") && value.starts_with("EVEX_") {
		return true;
	}
	if cfg!(feature = "no_xop") && value.starts_with("XOP_") {
		return true;
	}
	if cfg!(feature = "no_d3now") && value.starts_with("D3NOW_") {
		return true;
	}
	if cfg!(not(feature = "mvex")) && (value.starts_with("MVEX_") || value.starts_with("VEX_KNC_")) {
		return true;
	}
	IGNORED_CODE_HASH.contains(value)
}

#[cfg(any(feature = "decoder", feature = "encoder", feature = "instr_info", feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn code_names() -> Vec<&'static str> {
	let mut v: Vec<_> = (*TO_CODE_HASH).iter().collect();
	v.sort_unstable_by_key(|kv| *kv.1 as u32);
	v.into_iter().map(|kv| *kv.0).collect()
}

pub(crate) fn to_mnemonic(value: &str) -> Result<Mnemonic, String> {
	let value = value.trim();
	match TO_MNEMONIC_HASH.get(value) {
		Some(mnemonic) => Ok(*mnemonic),
		None => Err(format!("Invalid Mnemonic value: {}", value)),
	}
}

pub(crate) fn to_register(value: &str) -> Result<Register, String> {
	let value = value.trim();
	if value.is_empty() {
		return Ok(Register::None);
	}
	match TO_REGISTER_HASH.get(value) {
		Some(register) => Ok(*register),
		None => Err(format!("Invalid Register value: {}", value)),
	}
}

#[cfg(feature = "instr_info")]
pub(crate) fn clone_register_hashmap() -> HashMap<String, Register> {
	TO_REGISTER_HASH.iter().map(|kv| ((*kv.0).to_string(), *kv.1)).collect()
}

pub(crate) fn to_memory_size(value: &str) -> Result<MemorySize, String> {
	let value = value.trim();
	match TO_MEMORY_SIZE_HASH.get(value) {
		Some(memory_size) => Ok(*memory_size),
		None => Err(format!("Invalid MemorySize value: {}", value)),
	}
}

pub(crate) fn to_decoder_error(value: &str) -> Result<DecoderError, String> {
	let value = value.trim();
	match TO_DECODER_ERROR_HASH.get(value) {
		Some(decoder_error) => Ok(*decoder_error),
		None => Err(format!("Invalid DecoderError value: {}", value)),
	}
}

#[cfg(any(feature = "decoder", feature = "instr_info", feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_decoder_options(value: &str) -> Result<u32, String> {
	let value = value.trim();
	match TO_DECODER_OPTIONS_HASH.get(value) {
		Some(decoder_options) => Ok(*decoder_options),
		None => Err(format!("Invalid DecoderOptions value: {}", value)),
	}
}

#[cfg(feature = "instr_info")]
pub(crate) fn to_encoding_kind(value: &str) -> Result<EncodingKind, String> {
	let value = value.trim();
	match TO_ENCODING_KIND_HASH.get(value) {
		Some(encoding_kind) => Ok(*encoding_kind),
		None => Err(format!("Invalid EncodingKind value: {}", value)),
	}
}

#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub(crate) fn to_tuple_type(value: &str) -> Result<TupleType, String> {
	let value = value.trim();
	match TO_TUPLE_TYPE_HASH.get(value) {
		Some(tuple_type) => Ok(*tuple_type),
		None => Err(format!("Invalid TupleType value: {}", value)),
	}
}

#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
pub(crate) fn to_mvex_conv_fn(value: &str) -> Result<MvexConvFn, String> {
	let value = value.trim();
	match TO_MVEX_CONV_FN_HASH.get(value) {
		Some(mvex_conv_fn) => Ok(*mvex_conv_fn),
		None => Err(format!("Invalid MvexConvFn value: {}", value)),
	}
}

#[cfg(all(feature = "encoder", feature = "op_code_info", feature = "mvex"))]
pub(crate) fn to_mvex_tuple_type_lut_kind(value: &str) -> Result<MvexTupleTypeLutKind, String> {
	let value = value.trim();
	match TO_MVEX_TT_LUT_KIND_HASH.get(value) {
		Some(tt_lut_kind) => Ok(*tt_lut_kind),
		None => Err(format!("Invalid MvexTupleTypeLutKind value: {}", value)),
	}
}

#[cfg(feature = "instr_info")]
pub(crate) fn to_cpuid_features(value: &str) -> Result<CpuidFeature, String> {
	let value = value.trim();
	match TO_CPUID_FEATURE_HASH.get(value) {
		Some(cpuid_features) => Ok(*cpuid_features),
		None => Err(format!("Invalid CpuidFeature value: {}", value)),
	}
}

#[cfg(feature = "instr_info")]
pub(crate) fn to_flow_control(value: &str) -> Result<FlowControl, String> {
	let value = value.trim();
	match TO_FLOW_CONTROL_HASH.get(value) {
		Some(flow_control) => Ok(*flow_control),
		None => Err(format!("Invalid FlowControl value: {}", value)),
	}
}

#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub(crate) fn to_op_code_operand_kind(value: &str) -> Result<OpCodeOperandKind, String> {
	let value = value.trim();
	match TO_OP_CODE_OPERAND_KIND_HASH.get(value) {
		Some(op_code_operand_kind) => Ok(*op_code_operand_kind),
		None => Err(format!("Invalid OpCodeOperandKind value: {}", value)),
	}
}

#[cfg(feature = "instr_info")]
pub(crate) fn to_condition_code(value: &str) -> Result<ConditionCode, String> {
	let value = value.trim();
	match TO_CONDITION_CODE_HASH.get(value) {
		Some(condition_code) => Ok(*condition_code),
		None => Err(format!("Invalid ConditionCode value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub(crate) fn to_options_props(value: &str) -> Result<OptionsProps, String> {
	let value = value.trim();
	match TO_OPTIONS_PROPS_HASH.get(value) {
		Some(options_props) => Ok(*options_props),
		None => Err(format!("Invalid OptionsProps value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub(crate) fn to_memory_size_options(value: &str) -> Result<MemorySizeOptions, String> {
	let value = value.trim();
	match TO_MEMORY_SIZE_OPTIONS_HASH.get(value) {
		Some(memory_size_options) => Ok(*memory_size_options),
		None => Err(format!("Invalid MemorySizeOptions value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_number_base(value: &str) -> Result<NumberBase, String> {
	let value = value.trim();
	match TO_NUMBER_BASE_HASH.get(value) {
		Some(number_base) => Ok(*number_base),
		None => Err(format!("Invalid NumberBase value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn number_base_len() -> usize {
	TO_NUMBER_BASE_HASH.len()
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub(crate) fn to_boolean(value: &str) -> Result<bool, String> {
	let value = value.trim();
	match value {
		"false" => Ok(false),
		"true" => Ok(true),
		_ => Err(format!("Invalid boolean value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_b(value: &str) -> Result<CC_b, String> {
	let value = value.trim();
	match TO_CC_B_HASH.get(value) {
		Some(cc_b) => Ok(*cc_b),
		None => Err(format!("Invalid CC_b value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_ae(value: &str) -> Result<CC_ae, String> {
	let value = value.trim();
	match TO_CC_AE_HASH.get(value) {
		Some(cc_ae) => Ok(*cc_ae),
		None => Err(format!("Invalid CC_ae value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_e(value: &str) -> Result<CC_e, String> {
	let value = value.trim();
	match TO_CC_E_HASH.get(value) {
		Some(cc_e) => Ok(*cc_e),
		None => Err(format!("Invalid CC_e value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_ne(value: &str) -> Result<CC_ne, String> {
	let value = value.trim();
	match TO_CC_NE_HASH.get(value) {
		Some(cc_ne) => Ok(*cc_ne),
		None => Err(format!("Invalid CC_ne value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_be(value: &str) -> Result<CC_be, String> {
	let value = value.trim();
	match TO_CC_BE_HASH.get(value) {
		Some(cc_be) => Ok(*cc_be),
		None => Err(format!("Invalid CC_be value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_a(value: &str) -> Result<CC_a, String> {
	let value = value.trim();
	match TO_CC_A_HASH.get(value) {
		Some(cc_a) => Ok(*cc_a),
		None => Err(format!("Invalid CC_a value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_p(value: &str) -> Result<CC_p, String> {
	let value = value.trim();
	match TO_CC_P_HASH.get(value) {
		Some(cc_p) => Ok(*cc_p),
		None => Err(format!("Invalid CC_p value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_np(value: &str) -> Result<CC_np, String> {
	let value = value.trim();
	match TO_CC_NP_HASH.get(value) {
		Some(cc_np) => Ok(*cc_np),
		None => Err(format!("Invalid CC_np value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_l(value: &str) -> Result<CC_l, String> {
	let value = value.trim();
	match TO_CC_L_HASH.get(value) {
		Some(cc_l) => Ok(*cc_l),
		None => Err(format!("Invalid CC_l value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_ge(value: &str) -> Result<CC_ge, String> {
	let value = value.trim();
	match TO_CC_GE_HASH.get(value) {
		Some(cc_ge) => Ok(*cc_ge),
		None => Err(format!("Invalid CC_ge value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_le(value: &str) -> Result<CC_le, String> {
	let value = value.trim();
	match TO_CC_LE_HASH.get(value) {
		Some(cc_le) => Ok(*cc_le),
		None => Err(format!("Invalid CC_le value: {}", value)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn to_cc_g(value: &str) -> Result<CC_g, String> {
	let value = value.trim();
	match TO_CC_G_HASH.get(value) {
		Some(cc_g) => Ok(*cc_g),
		None => Err(format!("Invalid CC_g value: {}", value)),
	}
}
