// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::iced_constants::IcedConstants;
use crate::*;
use alloc::string::String;

#[cfg(feature = "mvex")]
#[derive(Default)]
pub(crate) struct MvexDecoderInfo {
	pub(crate) eviction_hint: bool,
	pub(crate) reg_mem_conv: MvexRegMemConv,
}

#[derive(Default)]
pub(crate) struct DecoderTestCase {
	#[allow(dead_code)]
	pub(crate) line_number: u32,
	pub(crate) test_options: u32,
	pub(crate) decoder_error: DecoderError,
	pub(crate) decoder_options: u32,
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) ip: u64,
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
	#[cfg(feature = "mvex")]
	pub(crate) mvex: MvexDecoderInfo,
}
