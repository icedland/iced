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
#[cfg(not(feature = "std"))]
use alloc::string::String;

#[allow(dead_code)]
pub(crate) struct DecoderMemoryTestCase {
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) code: Code,
	pub(crate) register: Register,
	pub(crate) prefix_segment: Register,
	pub(crate) segment: Register,
	pub(crate) base_register: Register,
	pub(crate) index_register: Register,
	pub(crate) scale: u32,
	pub(crate) displacement: u64,
	pub(crate) displ_size: u32,
	pub(crate) constant_offsets: ConstantOffsets,
	pub(crate) encoded_hex_bytes: String,
	pub(crate) decoder_options: u32,
	pub(crate) line_number: u32,
	pub(crate) test_options: u32,
}
