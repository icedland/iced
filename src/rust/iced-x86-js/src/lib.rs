/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

//! iced-x86 JavaScript bindings

#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(deprecated_in_future)]
#![warn(keyword_idents)]
#![warn(meta_variable_misuse)]
#![warn(missing_docs)]
#![warn(non_ascii_idents)]
#![warn(trivial_casts)]
#![warn(trivial_numeric_casts)]
#![warn(unused_extern_crates)]
#![warn(unused_import_braces)]
#![warn(unused_labels)]
#![warn(unused_lifetimes)]
#![warn(unused_must_use)]
#![warn(unused_qualifications)]
#![warn(unused_results)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::cognitive_complexity)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::verbose_bit_mask)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::cargo_common_metadata)]
#![warn(clippy::clone_on_ref_ptr)]
#![warn(clippy::dbg_macro)]
#![warn(clippy::debug_assert_with_mut_call)]
#![warn(clippy::default_trait_access)]
#![warn(clippy::doc_markdown)]
#![warn(clippy::empty_line_after_outer_attr)]
#![warn(clippy::explicit_into_iter_loop)]
#![warn(clippy::explicit_iter_loop)]
#![warn(clippy::fallible_impl_from)]
#![warn(clippy::large_digit_groups)]
#![warn(clippy::missing_errors_doc)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stdout)]
#![warn(clippy::redundant_closure)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::used_underscore_binding)]

#[cfg(any(
	feature = "instr_info",
	feature = "decoder",
	feature = "gas",
	feature = "intel",
	feature = "masm",
	feature = "nasm",
	all(feature = "encoder", feature = "block_encoder")
))]
#[macro_use]
extern crate static_assertions;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder;
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder_options;
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
mod decoder_options;
#[cfg(feature = "encoder")]
mod encoder;
#[cfg(any(feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
mod encoding_kind;
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
#[cfg(any(feature = "instr_api", feature = "instr_info"))]
mod memory_size;
#[cfg(feature = "instr_info")]
mod memory_size_ext;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod memory_size_options;
#[cfg(feature = "instr_api")]
mod mnemonic;
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
pub use decoder_options::*;
#[cfg(feature = "encoder")]
pub use encoder::*;
#[cfg(any(feature = "instr_info", all(feature = "encoder", feature = "op_code_info")))]
pub use encoding_kind::*;
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
#[cfg(any(feature = "instr_api", feature = "instr_info"))]
pub use memory_size::*;
#[cfg(feature = "instr_info")]
pub use memory_size_ext::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use memory_size_options::*;
#[cfg(feature = "instr_api")]
pub use mnemonic::*;
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
