// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// This requires Rust 1.54.0 or later. The double cfg_attr is required so older versions can compile it.
#![cfg_attr(doc, cfg_attr(doc, doc = include_str!("../README.md")))]
#![cfg_attr(not(doc), doc = "Run cargo doc to see the docs (needs Rust 1.54.0 or later)")]
#![doc(html_logo_url = "https://raw.githubusercontent.com/icedland/iced/master/logo.png")]
#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(elided_lifetimes_in_paths)]
#![warn(explicit_outlives_requirements)]
#![warn(invalid_html_tags)]
#![warn(keyword_idents)]
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
#![allow(clippy::bool_assert_comparison)]
#![allow(clippy::branches_sharing_code)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::collapsible_else_if)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::field_reassign_with_default)]
#![allow(clippy::manual_range_contains)]
#![allow(clippy::match_ref_pats)]
#![allow(clippy::ptr_eq)]
#![allow(clippy::redundant_closure)] // Clippy is buggy
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::upper_case_acronyms)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::cloned_instead_of_copied)]
#![warn(clippy::dbg_macro)]
#![warn(clippy::debug_assert_with_mut_call)]
#![warn(clippy::default_trait_access)]
#![warn(clippy::doc_markdown)]
#![warn(clippy::empty_line_after_outer_attr)]
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
#![warn(clippy::missing_inline_in_public_items)]
#![warn(clippy::must_use_candidate)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stderr)]
#![warn(clippy::print_stdout)]
#![warn(clippy::rc_buffer)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::unwrap_in_result)]
#![warn(clippy::used_underscore_binding)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]
#![warn(clippy::zero_sized_map_values)]
#![cfg_attr(not(test), warn(clippy::expect_used))]
#![cfg_attr(not(test), warn(clippy::unwrap_used))]
#![cfg_attr(not(feature = "std"), no_std)]
#![doc(test(attr(deny(warnings))))]

// This should be the only place in the source code that uses no_std
#[cfg(all(feature = "std", feature = "no_std"))]
compile_error!("`std` and `no_std` features can't be used at the same time");
#[cfg(all(not(feature = "std"), not(feature = "no_std")))]
compile_error!("`std` or `no_std` feature must be defined");

#[cfg_attr(
	any(
		feature = "encoder",
		feature = "block_encoder",
		feature = "op_code_info",
		feature = "gas",
		feature = "intel",
		feature = "masm",
		feature = "nasm",
		feature = "fast_fmt"
	),
	macro_use
)]
extern crate alloc;
#[cfg(feature = "std")]
extern crate core;

#[macro_use]
mod iced_assert {
	macro_rules! iced_assert {
		($($expr:tt)*) => {{
			// If it's a debug build, include the expression string
			#[cfg(debug_assertions)]
			{
				assert!($($expr)*);
			}

			// If it's not a debug build, don't include the expression string
			#[cfg(not(debug_assertions))]
			{
				if !($($expr)*) {
					panic!();
				}
			}
		}};
	}
}

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_enc;
mod code;
#[cfg(feature = "code_asm")]
pub mod code_asm;
#[cfg(any(feature = "decoder", feature = "encoder"))]
mod constant_offsets;
#[cfg(any(feature = "decoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod data_reader;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "encoder")]
mod encoder;
mod enums;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod formatter;
pub(crate) mod iced_constants;
mod iced_error;
mod iced_features;
#[cfg(feature = "instr_info")]
mod info;
mod instruction;
#[cfg(feature = "encoder")]
mod instruction_create;
mod instruction_internal;
mod instruction_memory_sizes;
mod instruction_op_counts;
mod memory_size;
mod mnemonic;
mod mnemonics;
#[cfg(feature = "mvex")]
mod mvex;
mod register;
#[cfg(test)]
pub(crate) mod test;
#[cfg(test)]
pub(crate) mod test_utils;
#[cfg(any(feature = "decoder", feature = "encoder"))]
mod tuple_type_tbl;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use crate::block_enc::*;
pub use crate::code::*;
#[cfg(any(feature = "decoder", feature = "encoder"))]
pub use crate::constant_offsets::*;
#[cfg(feature = "decoder")]
pub use crate::decoder::*;
#[cfg(feature = "encoder")]
pub use crate::encoder::*;
pub use crate::enums::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub use crate::formatter::*;
pub use crate::iced_error::*;
pub use crate::iced_features::*;
#[cfg(feature = "instr_info")]
pub use crate::info::*;
pub use crate::instruction::*;
pub use crate::memory_size::*;
pub use crate::mnemonic::*;
pub use crate::register::*;
