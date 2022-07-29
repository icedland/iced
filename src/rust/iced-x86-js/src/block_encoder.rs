// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_encoder_options::BlockEncoderOptions;
use crate::instruction::Instruction;
use iced_x86_rust::InstructionBlock;
use wasm_bindgen::prelude::*;

/// Encodes instructions. It can be used to move instructions from one location to another location.
#[wasm_bindgen]
pub struct BlockEncoder {
	instructions: Vec<iced_x86_rust::Instruction>,
	bitness: u32,
	options: u32,
}

#[wasm_bindgen]
impl BlockEncoder {
	/// Constructor
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `options`: Encoder options ([`BlockEncoderOptions`])
	///
	/// [`BlockEncoderOptions`]: enum.BlockEncoderOptions.html
	#[wasm_bindgen(constructor)]
	pub fn new(bitness: u32, options: u32 /*flags: BlockEncoderOptions*/) -> Result<BlockEncoder, JsValue> {
		// It's not part of the method sig so make sure it's still compiled by referencing it here
		const _: () = assert!(BlockEncoderOptions::None as u32 == 0);
		if bitness != 16 && bitness != 32 && bitness != 64 {
			Err(js_sys::Error::new("Invalid bitness").into())
		} else {
			Ok(BlockEncoder { instructions: Vec::new(), bitness, options })
		}
	}

	/// Adds an instruction that will be encoded when [`encode()`] is called.
	/// The input `instruction` can be a decoded instruction or an instruction
	/// created by the user, eg. `Instruction.with*()` constructor methods.
	///
	/// [`encode()`]: #method.encode
	///
	/// # Arguments
	///
	/// * `instruction`: Next instruction to encode
	pub fn add(&mut self, instruction: &Instruction) {
		self.instructions.push(instruction.0);
	}

	/// Encodes all instructions added by [`add()`] and returns the encoded bytes
	///
	/// [`add()`]: #method.add
	///
	/// # Arguments
	///
	/// * `rip`: Base IP of all encoded instructions
	pub fn encode(&mut self, rip: u64) -> Result<Vec<u8>, JsValue> {
		self.encode_core(rip)
	}

	fn encode_core(&mut self, rip: u64) -> Result<Vec<u8>, JsValue> {
		let block = InstructionBlock::new(&self.instructions, rip);
		iced_x86_rust::BlockEncoder::encode(self.bitness, block, self.options)
			.map_or_else(|error| Err(js_sys::Error::new(&format!("{}", error)).into()), |result| Ok(result.code_buffer))
	}
}
