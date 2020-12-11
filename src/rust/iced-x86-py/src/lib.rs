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

//! iced-x86 Python bindings (native module written in Rust). Don't reference this module directly.

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
#![allow(clippy::collapsible_if)]
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::clone_on_ref_ptr)]
#![warn(clippy::dbg_macro)]
#![warn(clippy::debug_assert_with_mut_call)]
#![warn(clippy::default_trait_access)]
#![warn(clippy::doc_markdown)]
#![warn(clippy::empty_line_after_outer_attr)]
#![warn(clippy::explicit_into_iter_loop)]
#![warn(clippy::explicit_iter_loop)]
#![warn(clippy::fallible_impl_from)]
#![warn(clippy::implicit_saturating_sub)]
#![warn(clippy::large_digit_groups)]
#![warn(clippy::let_unit_value)]
#![warn(clippy::match_bool)]
#![warn(clippy::missing_errors_doc)]
#![warn(clippy::needless_borrow)]
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
#![warn(clippy::used_underscore_binding)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]

#[macro_use]
extern crate static_assertions;

mod block_encoder;
mod constant_offsets;
mod decoder;
mod encoder;
mod enum_utils;
mod fast_formatter;
mod formatter;
mod iced_constants;
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
fn _iced_x86_py(_py: Python, m: &PyModule) -> PyResult<()> {
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
