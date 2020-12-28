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

use super::super::super::iced_constants::IcedConstants;
use super::super::super::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;

#[derive(Default)]
pub(crate) struct DecoderTestCase {
	pub(crate) line_number: u32,
	pub(crate) test_options: u32,
	pub(crate) decoder_error: DecoderError,
	pub(crate) decoder_options: u32,
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) encoded_hex_bytes: String,
	pub(crate) code: Code,
	pub(crate) mnemonic: Mnemonic,
	pub(crate) op_count: u32,
	pub(crate) zeroing_masking: bool,
	pub(crate) suppress_all_exceptions: bool,
	pub(crate) is_broadcast: bool,
	pub(crate) has_xacquire_prefix: bool,
	pub(crate) has_xrelease_prefix: bool,
	pub(crate) has_repe_prefix: bool,
	pub(crate) has_repne_prefix: bool,
	pub(crate) has_lock_prefix: bool,
	pub(crate) vsib_bitness: u32,
	pub(crate) op_mask: Register,
	pub(crate) rounding_control: RoundingControl,
	pub(crate) op_kinds: [OpKind; IcedConstants::MAX_OP_COUNT],
	pub(crate) segment_prefix: Register,
	pub(crate) memory_segment: Register,
	pub(crate) memory_base: Register,
	pub(crate) memory_index: Register,
	pub(crate) memory_displ_size: u32,
	pub(crate) memory_size: MemorySize,
	pub(crate) memory_index_scale: u32,
	pub(crate) memory_displacement: u64,
	pub(crate) immediate: u64,
	pub(crate) immediate_2nd: u8,
	pub(crate) near_branch: u64,
	pub(crate) far_branch: u32,
	pub(crate) far_branch_selector: u16,
	pub(crate) op_registers: [Register; IcedConstants::MAX_OP_COUNT],
	pub(crate) constant_offsets: ConstantOffsets,
}
