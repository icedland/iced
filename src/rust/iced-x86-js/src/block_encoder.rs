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

use iced_x86::{Instruction, InstructionBlock};
use wasm_bindgen::prelude::*;

/// Encodes instructions. It can be used to move instructions from one location to another location.
#[wasm_bindgen]
#[derive(Debug)]
pub struct BlockEncoder {
	instructions: Vec<Instruction>,
	bitness: u32,
	options: u32,
}

#[wasm_bindgen]
impl BlockEncoder {
	/// Constructor
	///
	/// # Panics
	///
	/// Panics if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `options`: Encoder options (`BlockEncoderOptions`)
	#[cfg_attr(has_must_use, must_use)]
	#[wasm_bindgen(constructor)]
	pub fn new(bitness: u32, options: u32) -> Self {
		if bitness != 16 && bitness != 32 && bitness != 64 {
			panic!();
		}
		BlockEncoder { instructions: Vec::new(), bitness, options }
	}

	/// Adds an instruction that will be encoded when `encode()` is called.
	/// The input `instruction` can be a decoded instruction or an instruction
	/// created by the user, eg. `Instruction::with_*()` constructor methods.
	///
	/// # Arguments
	///
	/// * `instruction`: Next instruction to encode
	pub fn add(&mut self, instruction: &Instruction) {
		self.instructions.push(*instruction);
	}

	/// Encodes all instructions added by `add()`
	///
	/// # Arguments
	///
	/// * `rip`: Base IP of all encoded instructions
	pub fn encode(&mut self, rip: u64) -> Result<Vec<u8>, JsValue> {
		let block = InstructionBlock::new(&self.instructions, rip);
		match iced_x86::BlockEncoder::encode(self.bitness, block, self.options) {
			Ok(result) => Ok(result.code_buffer),
			Err(error) => Err(js_sys::Error::new(&error).into()),
		}
	}
}
