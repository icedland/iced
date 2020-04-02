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

// No #[wasm_bindgen] should be added to iced-x86.
// 1. We shouldn't have to update it to support JavaScript.
// 2. It results in smaller wasm/js/ts files since we have better control of what gets included.
// 3. We can add better docs and rename methods to camelCase.

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
#![allow(clippy::verbose_bit_mask)]
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
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unused_self)]
#![warn(clippy::used_underscore_binding)]

#[cfg(feature = "instr_info")]
#[macro_use]
extern crate static_assertions;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder;
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder_options;
#[cfg(any(feature = "instruction_api", all(feature = "instruction_api", feature = "encoder", feature = "op_code_info")))]
mod code;
#[cfg(feature = "instruction_api")]
mod code_size;
#[cfg(feature = "instr_info")]
#[cfg(feature = "instruction_api")]
mod condition_code;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "decoder")]
mod decoder_options;
#[cfg(feature = "encoder")]
mod encoder;
#[cfg(any(feature = "instr_info", feature = "instruction_api", all(feature = "instruction_api", feature = "encoder", feature = "op_code_info")))]
mod encoding_kind;
#[cfg(any(feature = "instr_info", all(feature = "instr_info", feature = "instruction_api")))]
mod flow_control;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod formatter;
#[cfg(feature = "instr_info")]
mod info;
mod instruction;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
mod mandatory_prefix;
#[cfg(any(feature = "instruction_api", feature = "instr_info"))]
mod memory_size;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod memory_size_options;
#[cfg(feature = "instruction_api")]
mod mnemonic;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod number_base;
#[cfg(feature = "instr_info")]
mod op_access;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
mod op_code_info;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
mod op_code_operand_kind;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
mod op_code_table_kind;
#[cfg(feature = "instruction_api")]
mod op_kind;
#[cfg(any(feature = "instruction_api", feature = "instr_info"))]
mod register;
#[cfg(feature = "instruction_api")]
mod rounding_control;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
mod tuple_type;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use block_encoder::*;
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use block_encoder_options::*;
#[cfg(any(feature = "instruction_api", all(feature = "instruction_api", feature = "encoder", feature = "op_code_info")))]
pub use code::*;
#[cfg(feature = "instruction_api")]
pub use code_size::*;
#[cfg(feature = "instr_info")]
#[cfg(feature = "instruction_api")]
pub use condition_code::*;
#[cfg(feature = "decoder")]
pub use decoder::*;
#[cfg(feature = "decoder")]
pub use decoder_options::*;
#[cfg(feature = "encoder")]
pub use encoder::*;
#[cfg(any(
	feature = "instr_info",
	feature = "instruction_api",
	all(feature = "instruction_api", feature = "encoder", feature = "op_code_info")
))]
pub use encoding_kind::*;
#[cfg(any(feature = "instr_info", all(feature = "instr_info", feature = "instruction_api")))]
pub use flow_control::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use formatter::*;
#[cfg(feature = "instr_info")]
pub use info::*;
pub use instruction::*;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
pub use mandatory_prefix::*;
#[cfg(any(feature = "instruction_api", feature = "instr_info"))]
pub use memory_size::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use memory_size_options::*;
#[cfg(feature = "instruction_api")]
pub use mnemonic::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use number_base::*;
#[cfg(feature = "instr_info")]
pub use op_access::*;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
pub use op_code_info::*;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
pub use op_code_operand_kind::*;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
pub use op_code_table_kind::*;
#[cfg(feature = "instruction_api")]
pub use op_kind::*;
#[cfg(any(feature = "instruction_api", feature = "instr_info"))]
pub use register::*;
#[cfg(feature = "instruction_api")]
pub use rounding_control::*;
#[cfg(all(feature = "instruction_api", feature = "encoder", feature = "op_code_info"))]
pub use tuple_type::*;
