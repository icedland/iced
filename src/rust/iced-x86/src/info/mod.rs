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

pub(crate) mod cpuid_table;
pub(crate) mod enums;
pub(crate) mod info_table;
pub(crate) mod rflags_table;
#[cfg(test)]
mod tests;

use super::*;
use std::fmt;

/// A register used by an instruction
#[derive(Copy, Clone, Eq, PartialEq, Default, Hash)]
pub struct UsedRegister {
	register: Register,
	access: OpAccess,
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
impl UsedRegister {
	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `register`: Register
	/// * `access`: Register access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(register: Register, access: OpAccess) -> Self {
		Self { register, access }
	}

	/// Gets the register
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn register(&self) -> Register {
		self.register
	}

	/// Gets the register access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn access(&self) -> OpAccess {
		self.access
	}
}

impl fmt::Debug for UsedRegister {
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {
		write!(f, "{:?}:{:?}", self.register(), self.access())?;
		Ok(())
	}
}

/// A memory location used by an instruction
#[derive(Copy, Clone, Eq, PartialEq, Default, Hash)]
pub struct UsedMemory {
	displacement: u64,
	segment: Register,
	base: Register,
	index: Register,
	scale: u8,
	memory_size: MemorySize,
	access: OpAccess,
}

impl UsedMemory {
	/// Creates a new instance
	///
	/// # Arguments
	///
	/// * `segment`: Effective segment register
	/// * `base`: Base register
	/// * `index`: Index register
	/// * `scale`: Scale: 1, 2, 4 or 8
	/// * `displacement`: Displacement
	/// * `memory_size`: Memory size
	/// * `access`: Access
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn new(segment: Register, base: Register, index: Register, scale: u32, displacement: u64, memory_size: MemorySize, access: OpAccess) -> Self {
		Self {
			segment,
			base,
			index,
			scale: scale as u8,
			displacement,
			memory_size,
			access,
		}
	}

	/// Effective segment register
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn segment(&self) -> Register {
		self.segment
	}

	/// Base register or `Register::None` if none
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn base(&self) -> Register {
		self.base
	}

	/// Index register or `Register::None` if none
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn index(&self) -> Register {
		self.index
	}

	/// Index scale (1, 2, 4 or 8)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn scale(&self) -> u32 {
		self.scale as u32
	}

	/// Displacement
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn displacement(&self) -> u64 {
		self.displacement
	}

	/// Size of location
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn memory_size(&self) -> MemorySize {
		self.memory_size
	}

	/// Memory access
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn access(&self) -> OpAccess {
		self.access
	}
}

impl fmt::Debug for UsedMemory {
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {
		write!(f, "[{:?}:", self.segment())?;
		let mut need_plus = false;
		if self.base() != Register::None {
			write!(f, "{:?}", self.base())?;
			need_plus = true;
		}
		if self.index() != Register::None {
			if need_plus {
				write!(f, "+")?;
			}
			need_plus = true;
			write!(f, "{:?}", self.index())?;
			if self.scale() != 1 {
				write!(f, "*{}", self.scale())?;
			}
			if self.displacement() != 0 || !need_plus {
				if need_plus {
					write!(f, "+")?;
				}
				if self.displacement() <= 9 {
					write!(f, "{}", self.displacement())?;
				} else {
					write!(f, "0x{:X}", self.displacement())?;
				}
			}
		}
		write!(f, ";{:?};{:?}]", self.memory_size(), self.access())?;
		Ok(())
	}
}
