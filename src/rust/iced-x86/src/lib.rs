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

//! iced-x86 is an x86/x64 disassembler, assembler and instruction decoder written in Rust
//!
//! TODO:
//!
//! ## Minimum supported `rustc` version
//!
//! iced-x86 supports `rustc` `1.20.0` or later.
//!
//! ## Crate feature flags
//!
//! You can enable/disable these in your `Cargo.toml` file.
//!
//! - `decoder`: (Enabled by default) Enables the decoder
//! - `encoder`: (Enabled by default) Enables the encoder
//! - `instr_info`: (Enabled by default) Enables the instruction info code
//! - `all_formatters`: (Enabled by default) Enables all formatters
//! - `gas_formatter`: (Enabled by default) Enables the gas (AT&T) formatter
//! - `intel_formatter`: (Enabled by default) Enables the Intel (XED) formatter
//! - `masm_formatter`: (Enabled by default) Enables the masm formatter
//! - `nasm_formatter`: (Enabled by default) Enables the nasm formatter
//! - `std`: (Enabled by default) Enables the `std` crate. `std` or `no_std` must be defined, but not both.
//! - `no_std`: Enables `#![no_std]`. `std` or `no_std` must be defined, but not both. This feature uses the `alloc` crate (rustc `1.36.0+`).
//! - `exhaustive_enums`: Enables exhaustive enums, i.e., no enum has the `#[non_exhaustive]` attribute

#![doc(html_logo_url = "https://raw.githubusercontent.com/0xd4d/iced/master/logo.png")]
#![doc(html_root_url = "https://docs.rs/iced-x86/0.0.0")]
#![allow(unknown_lints)]
#![allow(bare_trait_objects)] // dyn syntax supported by rustc >= 1.27.0
#![allow(dead_code)] //TODO: REMOVE
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
#![cfg_attr(feature = "cargo-clippy", allow(clippy::cast_lossless))]
#![cfg_attr(feature = "cargo-clippy", allow(clippy::cognitive_complexity))]
#![cfg_attr(feature = "cargo-clippy", allow(clippy::collapsible_if))]
#![cfg_attr(feature = "cargo-clippy", allow(clippy::match_ref_pats))] // Not supported if < 1.26.0
#![cfg_attr(feature = "cargo-clippy", allow(clippy::needless_lifetimes))] // '_ requires rustc >= 1.31.0
#![cfg_attr(feature = "cargo-clippy", allow(clippy::range_plus_one))] // Requires rustc >= 1.26.0
#![cfg_attr(feature = "cargo-clippy", allow(clippy::verbose_bit_mask))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::dbg_macro))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::default_trait_access))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::doc_markdown))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::fallible_impl_from))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::large_digit_groups))]
//TODO: enable this when rustc 1.41.0 has been released
//#![cfg_attr(feature = "cargo-clippy", warn(clippy::missing_inline_in_public_items))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::must_use_candidate))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::unimplemented))]
#![cfg_attr(feature = "cargo-clippy", warn(clippy::used_underscore_binding))]
#![cfg_attr(not(feature = "std"), no_std)]

// This should be the only place in the source code that uses no_std
#[cfg(all(feature = "std", feature = "no_std"))]
compile_error!("`std` and `no_std` features can't be used at the same time");
#[cfg(all(not(feature = "std"), not(feature = "no_std")))]
compile_error!("`std` or `no_std` feature must be defined");

#[cfg(has_alloc)]
#[macro_use]
extern crate alloc;
#[cfg(feature = "std")]
extern crate core;
#[macro_use]
extern crate lazy_static;
#[macro_use]
extern crate static_assertions;
#[cfg(not(feature = "std"))]
extern crate hashbrown;

#[cfg(feature = "encoder")]
mod block_enc;
mod code;
#[cfg(any(feature = "decoder", feature = "encoder"))]
mod constant_offsets;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "encoder")]
mod encoder;
mod enums;
#[cfg(any(
	feature = "gas_formatter",
	feature = "intel_formatter",
	feature = "masm_formatter",
	feature = "nasm_formatter",
	feature = "all_formatters",
))]
mod formatter;
pub(crate) mod iced_constants;
mod iced_features;
#[cfg(feature = "instr_info")]
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
mod test;
#[cfg(test)]
pub(crate) mod test_utils;

#[cfg(feature = "encoder")]
pub use self::block_enc::*;
pub use self::code::*;
#[cfg(any(feature = "decoder", feature = "encoder"))]
pub use self::constant_offsets::*;
#[cfg(feature = "decoder")]
pub use self::decoder::*;
#[cfg(feature = "encoder")]
pub use self::encoder::*;
pub use self::enums::*;
#[cfg(any(
	feature = "gas_formatter",
	feature = "intel_formatter",
	feature = "masm_formatter",
	feature = "nasm_formatter",
	feature = "all_formatters",
))]
pub use self::formatter::*;
pub use self::iced_features::*;
#[cfg(feature = "instr_info")]
pub use self::info::*;
pub use self::instruction::*;
pub use self::memory_size::*;
pub use self::mnemonic::*;
pub use self::register::*;
