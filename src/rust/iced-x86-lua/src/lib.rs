// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(elided_lifetimes_in_paths)]
#![warn(explicit_outlives_requirements)]
#![warn(invalid_html_tags)]
#![warn(keyword_idents)]
#![warn(macro_use_extern_crate)]
#![warn(meta_variable_misuse)]
#![warn(missing_copy_implementations)]
#![warn(missing_debug_implementations)]
#![warn(non_ascii_idents)]
#![warn(trivial_casts)]
#![warn(trivial_numeric_casts)]
#![warn(unsafe_op_in_unsafe_fn)]
#![warn(unused_extern_crates)]
#![warn(unused_import_braces)]
#![warn(unused_lifetimes)]
#![warn(unused_must_use)]
#![warn(unused_results)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::collapsible_else_if)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::field_reassign_with_default)]
#![allow(clippy::manual_range_contains)]
#![allow(clippy::match_ref_pats)]
#![allow(clippy::ptr_eq)]
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
#![warn(clippy::redundant_closure)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::unwrap_in_result)]
#![warn(clippy::unwrap_used)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]
#![warn(clippy::zero_sized_map_values)]

#[macro_use]
mod lua_macros;
mod blk_enc;
mod coffs;
mod dec;
mod enc;
mod enum_utils;
mod fmt;
mod fpui;
mod grp_idx;
mod info;
mod instr;
mod mem_op;
mod memi;
mod memx;
mod opci;
mod regi;
mod regx;
mod ud;
