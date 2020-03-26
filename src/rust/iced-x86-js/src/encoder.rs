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

use iced_x86::{ConstantOffsets, Encoder, Instruction};
use wasm_bindgen::prelude::*;

/// Encodes instructions decoded by the decoder or instructions created by other code.
/// See also [`BlockEncoder`] which can encode any number of instructions.
///
/// [`BlockEncoder`]: struct.BlockEncoder.html
///
/// ```
/// use iced_x86::*;
///
/// // xchg [rdx+rsi+16h],ah
/// let bytes = b"\x86\x64\x32\x16";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// decoder.set_ip(0x1234_5678);
/// let instr = decoder.decode();
///
/// let mut encoder = Encoder::new(64);
/// match encoder.encode(&instr, 0x5555_5555) {
///     Ok(len) => assert_eq!(4, len),
///     Err(err) => panic!("{}", err),
/// }
/// // We're done, take ownership of the buffer
/// let buffer = encoder.take_buffer();
/// assert_eq!(vec![0x86, 0x64, 0x32, 0x16], buffer);
/// ```
#[wasm_bindgen]
#[allow(missing_debug_implementations)]
pub struct EncoderX86 {
	encoder: Encoder,
}

#[wasm_bindgen]
impl EncoderX86 {
	/// Creates an encoder
	///
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	#[must_use]
	#[wasm_bindgen(constructor)]
	pub fn new(bitness: u32) -> Self {
		Self::with_capacity(bitness, 0)
	}

	/// Creates an encoder with an initial buffer capacity
	///
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	/// * `capacity`: Initial capacity of the `u8` buffer
	#[must_use]
	pub fn with_capacity(bitness: u32, capacity: usize) -> Self {
		let encoder = Encoder::with_capacity(bitness, capacity);
		Self { encoder }
	}

	/// Encodes an instruction and returns the size of the encoded instruction
	///
	/// # Errors
	///
	/// Returns an error message on failure.
	///
	/// # Arguments
	///
	/// * `instruction`: Instruction to encode
	/// * `rip`: `RIP` of the encoded instruction
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // je short $+4
	/// let bytes = b"\x75\x02";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// decoder.set_ip(0x1234_5678);
	/// let instr = decoder.decode();
	///
	/// let mut encoder = Encoder::new(64);
	/// // Use a different IP (orig rip + 0x10)
	/// match encoder.encode(&instr, 0x1234_5688) {
	///     Ok(len) => assert_eq!(2, len),
	///     Err(err) => panic!("{}", err),
	/// }
	/// // We're done, take ownership of the buffer
	/// let buffer = encoder.take_buffer();
	/// assert_eq!(vec![0x75, 0xF2], buffer);
	/// ```
	pub fn encode(&mut self, instruction: &Instruction, rip: u64) -> Result<u32, JsValue> {
		match self.encoder.encode(instruction, rip) {
			Ok(size) => Ok(size as u32),
			#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
			Err(error) => Err(js_sys::Error::new(&format!("{} ({})", error, instruction)).into()),
			#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
			Err(error) => Err(js_sys::Error::new(&error).into()),
		}
	}

	/// Writes a byte to the output buffer
	///
	/// # Arguments
	///
	/// `value`: Value to write
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// let mut encoder = Encoder::new(64);
	/// let instr = Instruction::with_reg_reg(Code::Add_r64_rm64, Register::R8, Register::RBP);
	/// encoder.write_u8(0x90);
	/// match encoder.encode(&instr, 0x5555_5555) {
	///     Ok(len) => assert_eq!(3, len),
	///     Err(err) => panic!("{}", err),
	/// }
	/// encoder.write_u8(0xCC);
	/// // We're done, take ownership of the buffer
	/// let buffer = encoder.take_buffer();
	/// assert_eq!(vec![0x90, 0x4C, 0x03, 0xC5, 0xCC], buffer);
	/// ```
	pub fn write_u8(&mut self, value: u8) {
		self.encoder.write_u8(value)
	}

	/// Returns the buffer and initializes the internal buffer to an empty vector. Should be called when
	/// you've encoded all instructions and need the raw instruction bytes. See also [`set_buffer()`].
	///
	/// [`set_buffer()`]: #method.set_buffer
	#[must_use]
	pub fn take_buffer(&mut self) -> Vec<u8> {
		self.encoder.take_buffer()
	}

	/// Overwrites the buffer with a new vector. The old buffer is dropped. See also [`take_buffer()`].
	///
	/// [`take_buffer()`]: #method.take_buffer
	pub fn set_buffer(&mut self, buffer: Vec<u8>) {
		self.encoder.set_buffer(buffer)
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the encoded instruction.
	/// The caller can use this information to add relocations if needed.
	#[must_use]
	pub fn get_constant_offsets(&self) -> ConstantOffsets {
		self.encoder.get_constant_offsets()
	}

	/// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn prevent_vex2(&self) -> bool {
		self.encoder.prevent_vex2()
	}

	/// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	///
	/// # Arguments
	///
	/// * `new_value`: new value
	#[wasm_bindgen(setter)]
	pub fn set_prevent_vex2(&mut self, new_value: bool) {
		self.encoder.set_prevent_vex2(new_value)
	}

	/// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn vex_wig(&self) -> u32 {
		self.encoder.vex_wig()
	}

	/// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	pub fn set_vex_wig(&mut self, new_value: u32) {
		self.encoder.set_vex_wig(new_value)
	}

	/// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn vex_lig(&self) -> u32 {
		self.encoder.vex_lig()
	}

	/// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	pub fn set_vex_lig(&mut self, new_value: u32) {
		self.encoder.set_vex_lig(new_value)
	}

	/// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn evex_wig(&self) -> u32 {
		self.encoder.evex_wig()
	}

	/// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	pub fn set_evex_wig(&mut self, new_value: u32) {
		self.encoder.set_evex_wig(new_value)
	}

	/// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn evex_lig(&self) -> u32 {
		self.encoder.evex_lig()
	}

	/// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 3)
	#[wasm_bindgen(setter)]
	pub fn set_evex_lig(&mut self, new_value: u32) {
		self.encoder.set_evex_lig(new_value)
	}

	/// Gets the bitness (16, 32 or 64)
	#[wasm_bindgen(getter)]
	#[must_use]
	pub fn bitness(&self) -> u32 {
		self.encoder.bitness()
	}
}
