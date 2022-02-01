// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::constant_offsets::ConstantOffsets;
use crate::ex_utils::to_js_error;
use crate::instruction::Instruction;
use wasm_bindgen::prelude::*;

/// Encodes instructions decoded by the decoder or instructions created by other code.
/// See also [`BlockEncoder`] which can encode any number of instructions.
///
/// [`BlockEncoder`]: struct.BlockEncoder.html
///
/// ```js
/// const assert = require("assert").strict;
/// const { Decoder, DecoderOptions, Encoder } = require("iced-x86");
///
/// // xchg ah,[rdx+rsi+16h]
/// const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16]);
/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
/// decoder.ip = 0x12345678n;
/// const instr = decoder.decode();
///
/// const encoder = new Encoder(64);
/// const len = encoder.encode(instr, 0x55555555n);
/// assert.equal(len, 4);
/// // We're done, take ownership of the buffer
/// const buffer = encoder.takeBuffer();
/// assert.equal(buffer.length, 4);
/// assert.equal(buffer[0], 0x86);
/// assert.equal(buffer[1], 0x64);
/// assert.equal(buffer[2], 0x32);
/// assert.equal(buffer[3], 0x16);
///
/// // Free wasm memory
/// decoder.free();
/// instr.free();
/// encoder.free();
/// ```
#[wasm_bindgen]
pub struct Encoder(iced_x86_rust::Encoder);

#[wasm_bindgen]
impl Encoder {
	/// Creates an encoder
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	#[wasm_bindgen(constructor)]
	pub fn new(bitness: u32) -> Result<Encoder, JsValue> {
		Self::with_capacity(bitness, 0)
	}

	/// Creates an encoder with an initial buffer capacity
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	/// * `capacity`: Initial capacity of the `u8` buffer
	#[wasm_bindgen(js_name = "withCapacity")]
	pub fn with_capacity(bitness: u32, capacity: usize) -> Result<Encoder, JsValue> {
		Ok(Self(iced_x86_rust::Encoder::try_with_capacity(bitness, capacity).map_err(to_js_error)?))
	}

	/// Encodes an instruction and returns the size of the encoded instruction
	///
	/// # Throws
	///
	/// Throws an error on failure.
	///
	/// # Arguments
	///
	/// * `instruction`: Instruction to encode
	/// * `rip`: `RIP` of the encoded instruction
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, Encoder } = require("iced-x86");
	///
	/// // je short $+4
	/// const bytes = new Uint8Array([0x75, 0x02]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	/// const instr = decoder.decode();
	///
	/// const encoder = new Encoder(64);
	/// // Use a different IP (orig rip + 0x10)
	/// const len = encoder.encode(instr, 0x12345688n);
	/// assert.equal(len, 2);
	/// // We're done, take ownership of the buffer
	/// const buffer = encoder.takeBuffer();
	/// assert.equal(buffer.length, 2);
	/// assert.equal(buffer[0], 0x75);
	/// assert.equal(buffer[1], 0xF2);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// encoder.free();
	/// instr.free();
	/// ```
	pub fn encode(&mut self, instruction: &Instruction, rip: u64) -> Result<u32, JsValue> {
		self.encode_core(instruction, rip)
	}

	fn encode_core(&mut self, instruction: &Instruction, rip: u64) -> Result<u32, JsValue> {
		self.0.encode(&instruction.0, rip).map_or_else(
			|error| {
				#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
				{
					Err(js_sys::Error::new(&format!("{} ({})", error, instruction.0)).into())
				}
				#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
				{
					Err(js_sys::Error::new(&format!("{}", error)).into())
				}
			},
			|size| Ok(size as u32),
		)
	}

	/// Writes a byte to the output buffer
	///
	/// # Arguments
	///
	/// `value`: Value to write
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Encoder, Instruction, Register } = require("iced-x86");
	///
	/// const encoder = new Encoder(64);
	/// const instr = Instruction.createRegReg(Code.Add_r64_rm64, Register.R8, Register.RBP);
	/// encoder.writeU8(0x90);
	/// const len = encoder.encode(instr, 0x55555555n);
	/// assert.equal(len, 3);
	/// encoder.writeU8(0xCC);
	/// // We're done, take ownership of the buffer
	/// const buffer = encoder.takeBuffer();
	/// assert.equal(buffer.length, 5);
	/// assert.equal(buffer[0], 0x90);
	/// assert.equal(buffer[1], 0x4C);
	/// assert.equal(buffer[2], 0x03);
	/// assert.equal(buffer[3], 0xC5);
	/// assert.equal(buffer[4], 0xCC);
	///
	/// // Free wasm memory
	/// encoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "writeU8")]
	pub fn write_u8(&mut self, value: u8) {
		self.0.write_u8(value)
	}

	/// Returns the buffer and initializes the internal buffer to an empty vector. Should be called when
	/// you've encoded all instructions and need the raw instruction bytes. See also [`setBuffer()`].
	///
	/// [`setBuffer()`]: #method.set_buffer
	#[wasm_bindgen(js_name = "takeBuffer")]
	pub fn take_buffer(&mut self) -> Vec<u8> {
		self.0.take_buffer()
	}

	/// Overwrites the buffer with a new vector. The old buffer is dropped. See also [`takeBuffer()`].
	///
	/// [`takeBuffer()`]: #method.take_buffer
	#[wasm_bindgen(js_name = "setBuffer")]
	pub fn set_buffer(&mut self, buffer: Vec<u8>) {
		self.0.set_buffer(buffer)
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the encoded instruction.
	/// The caller can use this information to add relocations if needed.
	#[wasm_bindgen(js_name = "getConstantOffsets")]
	pub fn get_constant_offsets(&self) -> ConstantOffsets {
		ConstantOffsets(self.0.get_constant_offsets())
	}

	/// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "preventVEX2")]
	pub fn prevent_vex2(&self) -> bool {
		self.0.prevent_vex2()
	}

	/// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	///
	/// # Arguments
	///
	/// * `new_value`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "preventVEX2")]
	pub fn set_prevent_vex2(&mut self, new_value: bool) {
		self.0.set_prevent_vex2(new_value)
	}

	/// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "VEX_WIG")]
	pub fn vex_wig(&self) -> u32 {
		self.0.vex_wig()
	}

	/// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "VEX_WIG")]
	pub fn set_vex_wig(&mut self, new_value: u32) {
		self.0.set_vex_wig(new_value)
	}

	/// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "VEX_LIG")]
	pub fn vex_lig(&self) -> u32 {
		self.0.vex_lig()
	}

	/// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "VEX_LIG")]
	pub fn set_vex_lig(&mut self, new_value: u32) {
		self.0.set_vex_lig(new_value)
	}

	/// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "EVEX_WIG")]
	pub fn evex_wig(&self) -> u32 {
		self.0.evex_wig()
	}

	/// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "EVEX_WIG")]
	pub fn set_evex_wig(&mut self, new_value: u32) {
		self.0.set_evex_wig(new_value)
	}

	/// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "EVEX_LIG")]
	pub fn evex_lig(&self) -> u32 {
		self.0.evex_lig()
	}

	/// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 3)
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "EVEX_LIG")]
	pub fn set_evex_lig(&mut self, new_value: u32) {
		self.0.set_evex_lig(new_value)
	}

	/// Value of the `MVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "MVEX_WIG")]
	#[cfg(feature = "mvex")]
	pub fn mvex_wig(&self) -> u32 {
		self.0.mvex_wig()
	}

	/// Value of the `MVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	///
	/// # Arguments
	///
	/// * `new_value`: new value (0 or 1)
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "MVEX_WIG")]
	#[cfg(feature = "mvex")]
	pub fn set_mvex_wig(&mut self, new_value: u32) {
		self.0.set_mvex_wig(new_value)
	}

	/// Gets the bitness (16, 32 or 64)
	#[wasm_bindgen(getter)]
	pub fn bitness(&self) -> u32 {
		self.0.bitness()
	}
}
