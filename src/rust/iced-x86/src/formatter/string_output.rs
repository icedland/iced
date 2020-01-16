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

use super::enums::FormatterOutputTextKind;
use super::FormatterOutput;
#[cfg(not(feature = "std"))]
use alloc::string::String;

/// Implements [`FormatterOutput`] and writes all output to a string
///
/// [`FormatterOutput`]: trait.FormatterOutput.html
#[derive(Debug, Default, Clone, Eq, PartialEq, Hash)]
pub struct StringOutput {
	sb: String,
}

impl StringOutput {
	/// Constructor
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new() -> Self {
		Self { sb: String::new() }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// `capacity`: Initial capacity
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_capacity(capacity: usize) -> Self {
		Self { sb: String::with_capacity(capacity) }
	}

	/// Clears the internal string so this instance can be re-used for the next instruction
	#[inline]
	pub fn clear(&mut self) {
		self.sb.clear()
	}

	/// Gets the current string
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn get(&self) -> &str {
		&self.sb
	}
}

impl FormatterOutput for StringOutput {
	#[inline]
	fn write(&mut self, text: &str, _kind: FormatterOutputTextKind) {
		self.sb.push_str(text);
	}
}
