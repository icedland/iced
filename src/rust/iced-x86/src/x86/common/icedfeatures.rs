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
pub struct IcedFeatures;

impl IcedFeatures {
	/// true if the gas (AT&amp;T) formatter is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_gas_formatter() -> bool {
		#[cfg(any(feature = "GAS_FORMATTER", feature = "ALL_FORMATTERS"))]
		{ true }
		#[cfg(not(any(feature = "GAS_FORMATTER", feature = "ALL_FORMATTERS")))]
		{ false }
	}

	/// true if the Intel (xed) formatter is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_intel_formatter() -> bool {
		#[cfg(any(feature = "INTEL_FORMATTER", feature = "ALL_FORMATTERS"))]
		{ true }
		#[cfg(not(any(feature = "INTEL_FORMATTER", feature = "ALL_FORMATTERS")))]
		{ false }
	}

	/// true if the masm formatter is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_masm_formatter() -> bool {
		#[cfg(any(feature = "MASM_FORMATTER", feature = "ALL_FORMATTERS"))]
		{ true }
		#[cfg(not(any(feature = "MASM_FORMATTER", feature = "ALL_FORMATTERS")))]
		{ false }
	}

	/// true if the nasm formatter is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_nasm_formatter() -> bool {
		#[cfg(any(feature = "NASM_FORMATTER", feature = "ALL_FORMATTERS"))]
		{ true }
		#[cfg(not(any(feature = "NASM_FORMATTER", feature = "ALL_FORMATTERS")))]
		{ false }
	}

	/// true if the decoder is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_decoder() -> bool {
		#[cfg(feature = "DECODER")]
		{ true }
		#[cfg(not(feature = "DECODER"))]
		{ false }
	}

	/// true if the encoder is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_encoder() -> bool {
		#[cfg(feature = "ENCODER")]
		{ true }
		#[cfg(not(feature = "ENCODER"))]
		{ false }
	}

	/// true if the instruction info code is available
	#[inline]
	#[rustfmt::skip]
	pub fn has_instruction_info() -> bool {
		#[cfg(feature = "INSTR_INFO")]
		{ true }
		#[cfg(not(feature = "INSTR_INFO"))]
		{ false }
	}
}
