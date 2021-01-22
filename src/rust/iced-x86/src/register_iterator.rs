use core::intrinsics::transmute;
use core::iter::Iterator;

use super::iced_constants::IcedConstants;
use super::Register;

/// Iterator for `Register` enum values
#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]
pub struct RegisterIterator {
	curr_val: u8,
}

impl RegisterIterator {
	/// Creates a new iterator
	#[must_use]
	#[inline]
	pub fn new() -> Self {
		RegisterIterator { curr_val: 0 }
	}
}

impl Default for RegisterIterator {
	#[inline]
	fn default() -> Self {
		Self::new()
	}
}

impl Iterator for RegisterIterator {
	type Item = Register;

	#[inline]
	fn next(&mut self) -> Option<Self::Item> {
		if usize::from(self.curr_val) < IcedConstants::REGISTER_ENUM_COUNT {
			let reg: Register = unsafe { transmute(self.curr_val) };
			self.curr_val += 1;

			return Some(reg);
		}

		None
	}
}
