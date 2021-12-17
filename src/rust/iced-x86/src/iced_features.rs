// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

/// Gets the available features
#[doc(hidden)]
#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
pub struct IcedFeatures;

impl IcedFeatures {
	/// `true` if the gas (AT&amp;T) formatter is available
	#[must_use]
	#[inline]
	pub const fn has_gas() -> bool {
		cfg!(feature = "gas")
	}

	/// `true` if the Intel (xed) formatter is available
	#[must_use]
	#[inline]
	pub const fn has_intel() -> bool {
		cfg!(feature = "intel")
	}

	/// `true` if the masm formatter is available
	#[must_use]
	#[inline]
	pub const fn has_masm() -> bool {
		cfg!(feature = "masm")
	}

	/// `true` if the nasm formatter is available
	#[must_use]
	#[inline]
	pub const fn has_nasm() -> bool {
		cfg!(feature = "nasm")
	}

	/// `true` if the fast formatter is available
	#[must_use]
	#[inline]
	pub const fn has_fast_fmt() -> bool {
		cfg!(feature = "fast_fmt")
	}

	/// `true` if the decoder is available
	#[must_use]
	#[inline]
	pub const fn has_decoder() -> bool {
		cfg!(feature = "decoder")
	}

	/// `true` if the encoder is available
	#[must_use]
	#[inline]
	pub const fn has_encoder() -> bool {
		cfg!(feature = "encoder")
	}

	/// `true` if the block encoder is available
	#[must_use]
	#[inline]
	pub const fn has_block_encoder() -> bool {
		cfg!(all(feature = "encoder", feature = "block_encoder"))
	}

	/// `true` if the opcode info is available
	#[must_use]
	#[inline]
	pub const fn has_op_code_info() -> bool {
		cfg!(all(feature = "encoder", feature = "op_code_info"))
	}

	/// `true` if the instruction info code is available
	#[must_use]
	#[inline]
	pub const fn has_instruction_info() -> bool {
		cfg!(feature = "instr_info")
	}
}
