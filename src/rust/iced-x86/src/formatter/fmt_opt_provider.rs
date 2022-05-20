// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;
use core::mem;

/// Can override options used by a [`Formatter`]
///
/// [`Formatter`]: trait.Formatter.html
pub trait FormatterOptionsProvider {
	/// Called by the formatter. The method can override any options before the formatter uses them.
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `options`: Options. Only those options that will be used by the formatter are initialized.
	/// - `number_options`: Number formatting options
	fn operand_options(
		&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, options: &mut FormatterOperandOptions,
		number_options: &mut NumberFormattingOptions<'_>,
	);
}

pub(super) struct FormatterOperandOptionsFlags;
impl FormatterOperandOptionsFlags {
	#[cfg(any(feature = "intel", feature = "masm", feature = "nasm"))]
	pub(super) const NONE: u32 = 0x0000_0000;
	pub(super) const NO_BRANCH_SIZE: u32 = 0x0000_0001;
	const RIP_RELATIVE_ADDRESSES: u32 = 0x0000_0002;
	const MEMORY_SIZE_SHIFT: u32 = 30;
	const MEMORY_SIZE_MASK: u32 = 3 << FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT;
}

/// Operand options
#[derive(Debug, Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct FormatterOperandOptions {
	flags: u32, // FormatterOperandOptionsFlags
}

impl FormatterOperandOptions {
	#[cfg(any(feature = "intel", feature = "masm", feature = "nasm"))]
	#[must_use]
	#[inline]
	pub(super) const fn new(flags: u32) -> Self {
		Self { flags }
	}

	#[must_use]
	#[inline]
	pub(super) const fn with_memory_size_options(options: MemorySizeOptions) -> Self {
		Self { flags: (options as u32) << FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT }
	}

	/// Show branch size (eg. `SHORT`, `NEAR PTR`)
	#[must_use]
	#[inline]
	pub const fn branch_size(&self) -> bool {
		(self.flags & FormatterOperandOptionsFlags::NO_BRANCH_SIZE) == 0
	}

	/// Show branch size (eg. `SHORT`, `NEAR PTR`)
	///
	/// # Arguments
	///
	/// - `value`: New value
	#[inline]
	pub fn set_branch_size(&mut self, value: bool) {
		if value {
			self.flags &= !FormatterOperandOptionsFlags::NO_BRANCH_SIZE;
		} else {
			self.flags |= FormatterOperandOptionsFlags::NO_BRANCH_SIZE;
		}
	}

	/// If `true`, show `RIP` relative addresses as `[rip+12345678h]`, else show the linear address eg. `[1029384756AFBECDh]`
	#[must_use]
	#[inline]
	pub const fn rip_relative_addresses(&self) -> bool {
		(self.flags & FormatterOperandOptionsFlags::RIP_RELATIVE_ADDRESSES) != 0
	}

	/// If `true`, show `RIP` relative addresses as `[rip+12345678h]`, else show the linear address eg. `[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// - `value`: New value
	#[inline]
	pub fn set_rip_relative_addresses(&mut self, value: bool) {
		if value {
			self.flags |= FormatterOperandOptionsFlags::RIP_RELATIVE_ADDRESSES;
		} else {
			self.flags &= !FormatterOperandOptionsFlags::RIP_RELATIVE_ADDRESSES;
		}
	}

	/// Memory size options
	#[must_use]
	#[inline]
	pub fn memory_size_options(&self) -> MemorySizeOptions {
		// SAFETY: the bits can only be a valid enum value
		unsafe { mem::transmute((self.flags >> FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT) as MemorySizeOptionsUnderlyingType) }
	}

	/// Memory size options
	///
	/// # Arguments
	///
	/// - `value`: New value
	#[inline]
	pub fn set_memory_size_options(&mut self, value: MemorySizeOptions) {
		self.flags =
			(self.flags & !FormatterOperandOptionsFlags::MEMORY_SIZE_MASK) | ((value as u32) << FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT)
	}
}
