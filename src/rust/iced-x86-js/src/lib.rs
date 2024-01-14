// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! iced-x86 JavaScript bindings

#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(elided_lifetimes_in_paths)]
#![warn(explicit_outlives_requirements)]
#![warn(invalid_html_tags)]
#![warn(keyword_idents)]
#![warn(macro_use_extern_crate)]
#![warn(meta_variable_misuse)]
#![warn(missing_docs)]
#![warn(non_ascii_idents)]
#![warn(trivial_casts)]
#![warn(trivial_numeric_casts)]
#![warn(unused_extern_crates)]
#![warn(unused_import_braces)]
#![warn(unused_lifetimes)]
#![warn(unused_must_use)]
#![warn(unused_results)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::collapsible_else_if)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::drop_non_drop)] // wasm-bindgen warning
#![allow(clippy::field_reassign_with_default)]
#![allow(clippy::manual_range_contains)]
#![allow(clippy::match_ref_pats)]
#![allow(clippy::ptr_eq)]
#![allow(clippy::return_self_not_must_use)]
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::unused_unit)] // wasm-bindgen 0.2.79 https://github.com/rustwasm/wasm-bindgen/issues/2774
#![allow(clippy::upper_case_acronyms)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::cloned_instead_of_copied)]
#![warn(clippy::dbg_macro)]
#![warn(clippy::debug_assert_with_mut_call)]
#![warn(clippy::default_trait_access)]
#![warn(clippy::doc_markdown)]
#![warn(clippy::empty_line_after_outer_attr)]
#![warn(clippy::expect_used)]
#![warn(clippy::explicit_into_iter_loop)]
#![warn(clippy::explicit_iter_loop)]
#![warn(clippy::fallible_impl_from)]
#![warn(clippy::get_unwrap)]
#![warn(clippy::implicit_saturating_sub)]
#![warn(clippy::large_digit_groups)]
#![warn(clippy::let_unit_value)]
#![warn(clippy::match_bool)]
#![warn(clippy::match_on_vec_items)]
#![warn(clippy::match_wild_err_arm)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stderr)]
#![warn(clippy::print_stdout)]
#![warn(clippy::rc_buffer)]
#![warn(clippy::redundant_closure)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::unwrap_in_result)]
#![warn(clippy::unwrap_used)]
#![warn(clippy::used_underscore_binding)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]
#![warn(clippy::zero_sized_map_values)]

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder;
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder_options;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod cc;
#[cfg(any(feature = "instr_api", all(feature = "encoder", feature = "op_code_info"), feature = "instr_create"))]
mod code;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod code_ext;
#[cfg(feature = "instr_api")]
mod code_size;
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
mod condition_code;
#[cfg(any(feature = "encoder", all(feature = "decoder", feature = "instr_info")))]
mod constant_offsets;
#[cfg(feature = "instr_info")]
mod cpuid_feature;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "decoder")]
mod decoder_error;
#[cfg(feature = "decoder")]
mod decoder_options;
#[cfg(feature = "encoder")]
mod encoder;
#[cfg(any(feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
mod encoding_kind;
#[cfg(any(feature = "decoder", feature = "instr_info", feature = "encoder", feature = "instr_api", feature = "instr_create"))]
mod ex_utils;
#[cfg(feature = "fast_fmt")]
mod fast_fmt;
#[cfg(any(feature = "instr_info", all(feature = "instr_info", feature = "instr_api")))]
mod flow_control;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod format_mnemonic_options;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod formatter;
#[cfg(feature = "instr_info")]
mod info;
mod instruction;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod mandatory_prefix;
#[cfg(feature = "instr_create")]
mod memory_operand;
#[cfg(any(feature = "instr_api", feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
mod memory_size;
#[cfg(feature = "instr_info")]
mod memory_size_ext;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod memory_size_options;
#[cfg(any(feature = "instr_api", all(feature = "encoder", feature = "op_code_info")))]
mod mnemonic;
#[cfg(feature = "mvex")]
mod mvex_cvt_fn;
#[cfg(feature = "mvex")]
mod mvex_eh_bit;
#[cfg(feature = "mvex")]
mod mvex_rm_conv;
#[cfg(feature = "mvex")]
mod mvex_tt_lut;
#[cfg(feature = "instr_info")]
mod op_access;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod op_code_info;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod op_code_operand_kind;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod op_code_table_kind;
#[cfg(feature = "instr_api")]
mod op_kind;
#[cfg(any(feature = "instr_api", feature = "instr_info", feature = "instr_create"))]
mod register;
#[cfg(feature = "instr_info")]
mod register_ext;
#[cfg(feature = "instr_create")]
mod rep_prefix_kind;
#[cfg(feature = "instr_info")]
mod rflags_bits;
#[cfg(feature = "instr_api")]
mod rounding_control;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
mod tuple_type;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use block_encoder::*;
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use block_encoder_options::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use cc::*;
#[cfg(any(feature = "instr_api", all(feature = "encoder", feature = "op_code_info"), feature = "instr_create"))]
pub use code::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use code_ext::*;
#[cfg(feature = "instr_api")]
pub use code_size::*;
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
pub use condition_code::*;
#[cfg(any(feature = "encoder", all(feature = "decoder", feature = "instr_info")))]
pub use constant_offsets::*;
#[cfg(feature = "instr_info")]
pub use cpuid_feature::*;
#[cfg(feature = "decoder")]
pub use decoder::*;
#[cfg(feature = "decoder")]
pub use decoder_error::*;
#[cfg(feature = "decoder")]
pub use decoder_options::*;
#[cfg(feature = "encoder")]
pub use encoder::*;
#[cfg(any(feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
pub use encoding_kind::*;
#[cfg(feature = "fast_fmt")]
pub use fast_fmt::*;
#[cfg(any(feature = "instr_info", all(feature = "instr_info", feature = "instr_api")))]
pub use flow_control::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use format_mnemonic_options::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use formatter::*;
#[cfg(feature = "instr_info")]
pub use info::*;
pub use instruction::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use mandatory_prefix::*;
#[cfg(feature = "instr_create")]
pub use memory_operand::*;
#[cfg(any(feature = "instr_api", feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
pub use memory_size::*;
#[cfg(feature = "instr_info")]
pub use memory_size_ext::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use memory_size_options::*;
#[cfg(any(feature = "instr_api", all(feature = "encoder", feature = "op_code_info")))]
pub use mnemonic::*;
#[cfg(feature = "mvex")]
pub use mvex_cvt_fn::*;
#[cfg(feature = "mvex")]
pub use mvex_eh_bit::*;
#[cfg(feature = "mvex")]
pub use mvex_rm_conv::*;
#[cfg(feature = "mvex")]
pub use mvex_tt_lut::*;
#[cfg(feature = "instr_info")]
pub use op_access::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use op_code_info::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use op_code_operand_kind::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use op_code_table_kind::*;
#[cfg(feature = "instr_api")]
pub use op_kind::*;
#[cfg(any(feature = "instr_api", feature = "instr_info", feature = "instr_create"))]
pub use register::*;
#[cfg(feature = "instr_info")]
pub use register_ext::*;
#[cfg(feature = "instr_create")]
pub use rep_prefix_kind::*;
#[cfg(feature = "instr_info")]
pub use rflags_bits::*;
#[cfg(feature = "instr_api")]
pub use rounding_control::*;
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub use tuple_type::*;
use wasm_bindgen::prelude::*;

/// Gets feature flags.
///
/// Flag | Value
/// -----|-------
/// 0x01 | `VEX`
/// 0x02 | `EVEX`
/// 0x04 | `XOP`
/// 0x08 | `3DNow!`
/// 0x10 | `MVEX`
#[wasm_bindgen(js_name = "getIcedFeatures")]
pub fn get_iced_features() -> u32 {
	#[allow(unused_mut)]
	let mut flags = 0;
	#[cfg(not(feature = "no_vex"))]
	{
		flags |= 1;
	}
	#[cfg(not(feature = "no_evex"))]
	{
		flags |= 2;
	}
	#[cfg(not(feature = "no_xop"))]
	{
		flags |= 4;
	}
	#[cfg(not(feature = "no_d3now"))]
	{
		flags |= 8;
	}
	#[cfg(feature = "mvex")]
	{
		flags |= 0x10;
	}
	flags
}
