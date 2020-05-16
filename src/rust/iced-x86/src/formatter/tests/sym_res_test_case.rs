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

use super::super::super::*;
use super::enums::OptionsProps;
use super::opt_value::OptionValue;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(super) struct SymbolResolverTestCase {
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) decoder_options: u32,
	pub(super) code: Code,
	pub(super) options: Vec<(OptionsProps, OptionValue)>,
	pub(super) symbol_results: Vec<SymbolResultTestCase>,
}

pub(super) struct SymbolResultTestCase {
	pub(super) address: u64,
	pub(super) symbol_address: u64,
	pub(super) address_size: u32,
	pub(super) flags: u32, // SymbolFlags
	pub(super) memory_size: Option<MemorySize>,
	pub(super) symbol_parts: Vec<String>,
}
