// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::{MvexConvFn, MvexEHBit, MvexInfoFlags1, MvexInfoFlags2, MvexTupleTypeLutKind};
use core::mem;

#[allow(dead_code)]
pub(crate) struct MvexInfo {
	pub(crate) tuple_type_lut_kind: MvexTupleTypeLutKind,
	pub(crate) eh_bit: MvexEHBit,
	pub(crate) conv_fn: MvexConvFn,
	pub(crate) invalid_conv_fns: u8,
	pub(crate) invalid_swizzle_fns: u8,
	pub(crate) flags1: u8,
	pub(crate) flags2: u8,
	pub(crate) pad: u8,
}

const _: () = assert!(mem::size_of::<MvexInfo>() == 8);

impl MvexInfo {
	#[must_use]
	#[inline]
	#[cfg(feature = "op_code_info")]
	pub(crate) const fn is_ndd(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::NDD as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "op_code_info")]
	pub(crate) const fn is_nds(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::NDS as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(any(feature = "decoder", feature = "op_code_info"))]
	pub(crate) const fn can_use_eviction_hint(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::EVICTION_HINT as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "op_code_info")]
	pub(crate) const fn can_use_imm_rounding_control(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::IMM_ROUNDING_CONTROL as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "decoder")]
	pub(crate) const fn can_use_rounding_control(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::ROUNDING_CONTROL as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "decoder")]
	pub(crate) const fn can_use_suppress_all_exceptions(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::SUPPRESS_ALL_EXCEPTIONS as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "decoder")]
	pub(crate) const fn ignores_op_mask_register(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::IGNORES_OP_MASK_REGISTER as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "decoder")]
	pub(crate) const fn require_op_mask_register(&self) -> bool {
		(self.flags1 & (MvexInfoFlags1::REQUIRE_OP_MASK_REGISTER as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(any(feature = "decoder", feature = "op_code_info"))]
	pub(crate) const fn no_sae_rc(&self) -> bool {
		(self.flags2 & (MvexInfoFlags2::NO_SAE_ROUNDING_CONTROL as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) const fn is_conv_fn_32(&self) -> bool {
		(self.flags2 & (MvexInfoFlags2::CONV_FN32 as u8)) != 0
	}

	#[must_use]
	#[inline]
	#[cfg(feature = "decoder")]
	pub(crate) const fn ignores_eviction_hint(&self) -> bool {
		(self.flags2 & (MvexInfoFlags2::IGNORES_EVICTION_HINT as u8)) != 0
	}
}

impl MvexInfo {
	#[inline]
	#[must_use]
	pub(super) const fn new(
		tuple_type_lut_kind: MvexTupleTypeLutKind, eh_bit: MvexEHBit, conv_fn: MvexConvFn, invalid_conv_fns: u8, invalid_swizzle_fns: u8, flags1: u8,
		flags2: u8,
	) -> Self {
		Self { tuple_type_lut_kind, eh_bit, conv_fn, invalid_conv_fns, invalid_swizzle_fns, flags1, flags2, pad: 0 }
	}
}
