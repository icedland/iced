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

/// Gets the available features
#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
pub struct IcedFeatures;

impl IcedFeatures {
	/// `true` if the gas (AT&amp;T) formatter is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_gas_formatter() -> bool {
		cfg!(feature = "gas_formatter")
	}

	/// `true` if the Intel (xed) formatter is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_intel_formatter() -> bool {
		cfg!(feature = "intel_formatter")
	}

	/// `true` if the masm formatter is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_masm_formatter() -> bool {
		cfg!(feature = "masm_formatter")
	}

	/// `true` if the nasm formatter is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_nasm_formatter() -> bool {
		cfg!(feature = "nasm_formatter")
	}

	/// `true` if the decoder is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_decoder() -> bool {
		cfg!(feature = "decoder")
	}

	/// `true` if the encoder is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_encoder() -> bool {
		cfg!(feature = "encoder")
	}

	/// `true` if the instruction info code is available
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	pub fn has_instruction_info() -> bool {
		cfg!(feature = "instr_info")
	}
}
