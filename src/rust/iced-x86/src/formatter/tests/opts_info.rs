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

use super::super::super::*;
use super::enums::OptionsProps;
use super::opt_value::OptionValue;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(super) struct OptionsInstructionInfo {
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) code: Code,
	pub(super) vec: Vec<(OptionsProps, OptionValue)>,
}

impl OptionsInstructionInfo {
	pub(super) fn initialize_options(&self, options: &mut FormatterOptions) {
		for info in &self.vec {
			info.1.initialize_options(options, info.0);
		}
	}

	pub(super) fn initialize_decoder(&self, decoder: &mut Decoder) {
		for info in &self.vec {
			info.1.initialize_decoder(decoder, info.0);
		}
	}
}
