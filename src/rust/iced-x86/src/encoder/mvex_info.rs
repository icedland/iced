// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::{MvexConvFn, MvexEHBit, MvexInfoFlags};
use core::mem;
use static_assertions::const_assert_eq;

pub(super) struct MvexInfo {
	pub(super) tuple_type_size: u8,
	pub(super) mem_size: u8,
	pub(super) elem_size: u8,
	pub(super) eh_bit: MvexEHBit,
	pub(super) conv_fn: MvexConvFn,
	pub(super) valid_conv_fn: u8,
	pub(super) valid_swizzle_fn: u8,
	pub(super) flags: u8,
}

const_assert_eq!(mem::size_of::<MvexInfo>(), 8);

impl MvexInfo {
	#[must_use]
	#[inline]
	pub(super) fn is_ndd(&self) -> bool {
		(self.flags & (MvexInfoFlags::NDD as u8)) != 0
	}

	#[must_use]
	#[inline]
	pub(super) fn is_nds(&self) -> bool {
		(self.flags & (MvexInfoFlags::NDS as u8)) != 0
	}

	#[must_use]
	#[inline]
	pub(super) fn can_use_eviction_hint(&self) -> bool {
		(self.flags & (MvexInfoFlags::EVICTION_HINT as u8)) != 0
	}

	#[must_use]
	#[inline]
	pub(super) fn can_use_imm_rounding_control(&self) -> bool {
		(self.flags & (MvexInfoFlags::IMM_ROUNDING_CONTROL as u8)) != 0
	}
}

impl MvexInfo {
	#[inline]
	pub(super) const fn new(
		tuple_type_size: u8, mem_size: u8, elem_size: u8, eh_bit: MvexEHBit, conv_fn: MvexConvFn, valid_conv_fn: u8, valid_swizzle_fn: u8, flags: u8,
	) -> Self {
		Self { tuple_type_size, mem_size, elem_size, eh_bit, conv_fn, valid_conv_fn, valid_swizzle_fn, flags }
	}
}
