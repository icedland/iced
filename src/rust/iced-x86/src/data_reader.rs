// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use core::str;

pub(crate) struct DataReader<'a> {
	data: &'a [u8],
	index: usize,
}

impl<'a> DataReader<'a> {
	pub(crate) const fn new(data: &'a [u8]) -> Self {
		Self { data, index: 0 }
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) const fn index(&self) -> usize {
		self.index
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) fn set_index(&mut self, index: usize) {
		self.index = index
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) const fn len_left(&self) -> usize {
		self.data.len() - self.index
	}

	pub(crate) const fn can_read(&self) -> bool {
		self.index < self.data.len()
	}

	pub(crate) fn read_u8(&mut self) -> usize {
		let b = self.data[self.index] as usize;
		self.index += 1;
		b
	}

	pub(crate) fn read_compressed_u32(&mut self) -> u32 {
		let mut result = 0;
		let mut shift = 0;
		loop {
			debug_assert!(shift < 32);

			let b = self.read_u8() as u32;
			if (b & 0x80) == 0 {
				return result | (b << shift);
			}
			result |= (b & 0x7F) << shift;

			shift += 7;
		}
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
	#[allow(clippy::unwrap_used)]
	pub(crate) fn read_ascii_str(&mut self) -> &'a str {
		let len = self.read_u8();
		let s = str::from_utf8(&self.data[self.index..self.index + len]).unwrap();
		self.index += len;
		s
	}

	// Returns the whole slice starting at the current index,
	// including the length byte, and advances the index by the current length + 1
	#[cfg(feature = "fast_fmt")]
	pub(crate) fn read_len_data(&mut self) -> &'a [u8] {
		let len = usize::from(self.data[self.index]);
		debug_assert!(self.index + 1 + len <= self.data.len());
		let len_data = &self.data[self.index..];
		self.index += 1 + len;
		len_data
	}
}
