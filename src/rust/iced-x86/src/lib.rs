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

//! iced is an x86/x64 disassembler, decoder, encoder written in Rust

#![doc(html_logo_url = "https://raw.githubusercontent.com/0xd4d/iced/master/logo.png")]
#![allow(unknown_lints)]
#![cfg_attr(feature = "cargo-clippy", allow(clippy::cast_lossless))]
// ptr.add(x) is available in 1.26.0+ so we must use ptr.offset(x as isize)
#![cfg_attr(feature = "cargo-clippy", allow(clippy::ptr_offset_with_cast))]
#![cfg_attr(feature = "cargo-clippy", allow(clippy::collapsible_if))]
#![deny(absolute_paths_not_starting_with_crate)]
#![deny(deprecated_in_future)]
#![deny(keyword_idents)]
#![deny(missing_copy_implementations)]
#![deny(missing_docs)]
#![deny(trivial_casts)]
#![deny(trivial_numeric_casts)]
#![deny(unused_import_braces)]
#![deny(unused_lifetimes)]
#![deny(unused_must_use)]
#![deny(unused_qualifications)]
#![deny(unused_results)]

#[macro_use]
extern crate lazy_static;
#[macro_use]
extern crate static_assertions;

mod code;
#[cfg(any(feature = "DECODER", feature = "ENCODER"))]
mod constant_offsets;
#[cfg(feature = "DECODER")]
mod decoder;
#[cfg(feature = "ENCODER")]
mod encoder;
mod enums;
#[cfg(any(
	feature = "GAS_FORMATTER",
	feature = "INTEL_FORMATTER",
	feature = "MASM_FORMATTER",
	feature = "NASM_FORMATTER",
	feature = "ALL_FORMATTERS",
))]
mod formatter;
pub(crate) mod iced_constants;
mod iced_features;
#[cfg(feature = "INSTR_INFO")]
mod info;
mod instruction;
mod instruction_internal;
mod instruction_memory_sizes;
mod instruction_op_counts;
mod memory_size;
mod mnemonic;
mod mnemonics;
mod register;
#[cfg(test)]
pub(crate) mod test_utils;

pub use self::code::*;
#[cfg(any(feature = "DECODER", feature = "ENCODER"))]
pub use self::constant_offsets::*;
#[cfg(feature = "DECODER")]
pub use self::decoder::*;
#[cfg(feature = "ENCODER")]
pub use self::encoder::*;
pub use self::enums::*;
#[cfg(any(
	feature = "GAS_FORMATTER",
	feature = "INTEL_FORMATTER",
	feature = "MASM_FORMATTER",
	feature = "NASM_FORMATTER",
	feature = "ALL_FORMATTERS",
))]
pub use self::formatter::*;
pub use self::iced_features::*;
#[cfg(feature = "INSTR_INFO")]
pub use self::info::*;
pub use self::instruction::*;
pub use self::memory_size::*;
pub use self::mnemonic::*;
pub use self::register::*;
