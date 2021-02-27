// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! iced-x86 Python bindings (native module written in Rust). Don't reference this module directly.

#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(deprecated_in_future)]
#![warn(elided_lifetimes_in_paths)]
#![warn(explicit_outlives_requirements)]
#![warn(invalid_html_tags)]
#![warn(keyword_idents)]
#![warn(macro_use_extern_crate)]
#![warn(meta_variable_misuse)]
#![warn(missing_copy_implementations)]
#![warn(missing_debug_implementations)]
#![warn(missing_docs)]
#![warn(non_ascii_idents)]
#![warn(trivial_casts)]
#![warn(trivial_numeric_casts)]
#![warn(unused_extern_crates)]
#![warn(unused_import_braces)]
#![warn(unused_lifetimes)]
#![warn(unused_must_use)]
#![warn(unused_qualifications)]
#![warn(unused_results)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::field_reassign_with_default)]
#![allow(clippy::manual_range_contains)]
#![allow(clippy::manual_strip)]
#![allow(clippy::match_like_matches_macro)]
#![allow(clippy::match_ref_pats)]
#![allow(clippy::ptr_eq)]
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::unknown_clippy_lints)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::clone_on_ref_ptr)]
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
#![warn(clippy::missing_errors_doc)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stderr)]
#![warn(clippy::print_stdout)]
#![warn(clippy::rc_buffer)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::redundant_closure)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unnested_or_patterns)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::unwrap_in_result)]
#![warn(clippy::unwrap_used)]
#![warn(clippy::used_underscore_binding)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]
#![warn(clippy::zero_sized_map_values)]

mod block_encoder;
mod constant_offsets;
mod decoder;
mod encoder;
mod enum_utils;
mod fast_formatter;
mod formatter;
mod info;
mod instruction;
mod memory_operand;
mod memory_size_ext;
mod memory_size_info;
mod op_code_info;
mod register_ext;
mod register_info;
mod utils;

use block_encoder::BlockEncoder;
use constant_offsets::ConstantOffsets;
use decoder::Decoder;
use encoder::Encoder;
use fast_formatter::FastFormatter;
use formatter::Formatter;
use info::{InstructionInfo, InstructionInfoFactory, UsedMemory, UsedRegister};
use instruction::{FpuStackIncrementInfo, Instruction};
use memory_operand::MemoryOperand;
use memory_size_ext::MemorySizeExt;
use memory_size_info::MemorySizeInfo;
use op_code_info::OpCodeInfo;
use pyo3::prelude::*;
use register_ext::RegisterExt;
use register_info::RegisterInfo;

#[pymodule]
fn _iced_x86_py(_py: Python<'_>, m: &PyModule) -> PyResult<()> {
	// If you add a new struct, also add it to
	//	- src/iced_x86/__init__.py's `__init__` array and `from` statement
	//	- docs/index.rst
	//	- docs/src/<ClassName>.rst
	m.add_class::<BlockEncoder>()?;
	m.add_class::<ConstantOffsets>()?;
	m.add_class::<Decoder>()?;
	m.add_class::<Encoder>()?;
	m.add_class::<FastFormatter>()?;
	m.add_class::<Formatter>()?;
	m.add_class::<FpuStackIncrementInfo>()?;
	m.add_class::<Instruction>()?;
	m.add_class::<InstructionInfo>()?;
	m.add_class::<InstructionInfoFactory>()?;
	m.add_class::<MemoryOperand>()?;
	m.add_class::<MemorySizeExt>()?;
	m.add_class::<MemorySizeInfo>()?;
	m.add_class::<OpCodeInfo>()?;
	m.add_class::<RegisterExt>()?;
	m.add_class::<RegisterInfo>()?;
	m.add_class::<UsedMemory>()?;
	m.add_class::<UsedRegister>()?;

	Ok(())
}
