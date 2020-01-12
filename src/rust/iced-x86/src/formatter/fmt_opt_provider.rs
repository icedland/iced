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

use super::super::*;
use std::mem;

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
		number_options: &mut NumberFormattingOptions,
	);
}

pub(super) struct FormatterOperandOptionsFlags;
impl FormatterOperandOptionsFlags {
	pub(super) const NONE: u32 = 0;
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

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
impl FormatterOperandOptions {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(super) fn new(flags: u32) -> Self {
		Self { flags }
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(super) fn with_memory_size_options(options: MemorySizeOptions) -> Self {
		Self { flags: (options as u32) << FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT }
	}

	/// Show branch size (eg. `SHORT`, `NEAR PTR`)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn branch_size(&self) -> bool {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rip_relative_addresses(&self) -> bool {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn memory_size_options(&self) -> MemorySizeOptions {
		unsafe { mem::transmute((self.flags >> FormatterOperandOptionsFlags::MEMORY_SIZE_SHIFT) as u8) }
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
