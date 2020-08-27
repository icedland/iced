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

#[cfg(not(feature = "std"))]
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
use alloc::string::String;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
use core::str;

pub(crate) struct DataReader<'a> {
	data: &'a [u8],
	index: usize,
}

impl<'a> DataReader<'a> {
	pub(crate) fn new(data: &'a [u8]) -> Self {
		Self { data, index: 0 }
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) fn index(&self) -> usize {
		self.index
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) fn set_index(&mut self, index: usize) {
		self.index = index
	}

	pub(crate) fn can_read(&self) -> bool {
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
			if shift >= 32 {
				panic!();
			}

			let b = self.read_u8() as u32;
			if (b & 0x80) == 0 {
				return result | (b << shift);
			}
			result |= (b & 0x7F) << shift;

			shift += 7;
		}
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
	pub(crate) fn read_ascii_string(&mut self) -> String {
		let len = self.read_u8();
		let s = str::from_utf8(&self.data[self.index..self.index + len]).unwrap();
		self.index += len;
		String::from(s)
	}
}
