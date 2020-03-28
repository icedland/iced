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
#![warn(missing_copy_implementations)]
#![warn(missing_debug_implementations)]
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
#![warn(clippy::must_use_candidate)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stdout)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unused_self)]
#![warn(clippy::used_underscore_binding)]

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_encoder;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "encoder")]
mod encoder;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod formatter;
#[cfg(feature = "instr_info")]
mod info;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use block_encoder::*;
#[cfg(feature = "decoder")]
pub use decoder::*;
#[cfg(feature = "encoder")]
pub use encoder::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use formatter::*;
pub use iced_x86::*;
#[cfg(feature = "instr_info")]
pub use info::*;
