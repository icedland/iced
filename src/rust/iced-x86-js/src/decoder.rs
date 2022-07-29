// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(feature = "instr_info")]
use crate::constant_offsets::ConstantOffsets;
use crate::decoder_error::{iced_to_decoder_error, DecoderError};
use crate::decoder_options::DecoderOptions;
use crate::ex_utils::to_js_error;
use crate::instruction::Instruction;
use std::slice;
use wasm_bindgen::prelude::*;

/// Decodes 16/32/64-bit x86 instructions
#[wasm_bindgen]
pub struct Decoder {
	// The decoder has a reference to this vector and the 'static lifetime is really the lifetime of
	// this vector. We can't use another lifetime. This vector and the field are read-only.
	#[allow(dead_code)]
	__data_do_not_use: Vec<u8>,
	decoder: iced_x86_rust::Decoder<'static>,
}

#[wasm_bindgen]
impl Decoder {
	/// Creates a decoder
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32 or 64
	/// * `data`: Data to decode
	/// * `options`: Decoder options (a [`DecoderOptions`] flags value), `0` or eg. `DecoderOptions.NoInvalidCheck | DecoderOptions.AMD`
	///
	/// [`DecoderOptions`]: enum.DecoderOptions.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions, Mnemonic } = require("iced-x86");
	///
	/// // xchg ah,[rdx+rsi+16h]
	/// // xacquire lock add dword ptr [rax],5Ah
	/// // vmovdqu64 zmm18{k3}{z},zmm11
	/// const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16, 0xF0, 0xF2, 0x83, 0x00, 0x5A, 0x62, 0xC1, 0xFE, 0xCB, 0x6F, 0xD3]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	///
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Xchg_rm8_r8);
	/// assert.equal(instr.mnemonic, Mnemonic.Xchg);
	/// assert.equal(instr.length, 4);
	///
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.code, Code.Add_rm32_imm8);
	/// assert.equal(instr.mnemonic, Mnemonic.Add);
	/// assert.equal(instr.length, 5);
	///
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.code, Code.EVEX_Vmovdqu64_zmm_k1z_zmmm512);
	/// assert.equal(instr.mnemonic, Mnemonic.Vmovdqu64);
	/// assert.equal(instr.length, 6);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	///
	/// It's sometimes useful to decode some invalid instructions, eg. `lock add esi,ecx`.
	/// Pass in [`DecoderOptions.NoInvalidCheck`] to the constructor and the decoder
	/// will decode some invalid encodings.
	///
	/// [`DecoderOptions.NoInvalidCheck`]: enum.DecoderOptions.html#variant.NoInvalidCheck
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
	///
	/// // lock add esi,ecx   ; lock not allowed
	/// const bytes = new Uint8Array([0xF0, 0x01, 0xCE]);
	/// const decoder1 = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder1.ip = 0x12345678n;
	/// const instr1 = decoder1.decode();
	/// assert.equal(instr1.code, Code.INVALID);
	///
	/// // We want to decode some instructions with invalid encodings
	/// const decoder2 = new Decoder(64, bytes, DecoderOptions.NoInvalidCheck);
	/// decoder2.ip = 0x12345678n;
	/// const instr2 = decoder2.decode();
	/// assert.equal(instr2.code, Code.Add_rm32_r32);
	/// assert.ok(instr2.hasLockPrefix);
	///
	/// // Free wasm memory
	/// decoder1.free();
	/// decoder2.free();
	/// instr1.free();
	/// instr2.free();
	/// ```
	#[wasm_bindgen(constructor)]
	pub fn new(bitness: u32, data: Vec<u8>, options: u32 /*flags: DecoderOptions*/) -> Result<Decoder, JsValue> {
		// It's not part of the method sig so make sure it's still compiled by referencing it here
		const _: () = assert!(DecoderOptions::None as u32 == 0);
		// SAFETY: We only read it, we own the data, and store it in the returned value.
		// The decoder also doesn't impl Drop (it can't ref possibly freed data in drop()).
		let decoder_data = unsafe { slice::from_raw_parts(data.as_ptr(), data.len()) };
		let decoder = iced_x86_rust::Decoder::try_new(bitness, decoder_data, options).map_err(to_js_error)?;
		Ok(Decoder { __data_do_not_use: data, decoder })
	}

	/// Gets the current `IP`/`EIP`/`RIP` value, see also [`position`]
	///
	/// [`position`]: #method.position
	#[wasm_bindgen(getter)]
	pub fn ip(&self) -> u64 {
		self.decoder.ip()
	}

	/// Sets the current `IP`/`EIP`/`RIP` value, see also [`position`]
	///
	/// Writing to this property only updates the IP value, it does not change the data position, use [`position`] to change the position.
	///
	/// [`position`]: #method.set_position
	///
	/// # Arguments
	///
	/// * `new_value`: New IP
	#[wasm_bindgen(setter)]
	pub fn set_ip(&mut self, new_value: u64) {
		self.decoder.set_ip(new_value)
	}

	/// Gets the bitness (16, 32 or 64)
	#[wasm_bindgen(getter)]
	pub fn bitness(&self) -> u32 {
		self.decoder.bitness()
	}

	/// Gets the max value that can be written to [`position`]. This is the size of the data that gets
	/// decoded to instructions and it's the length of the array that was passed to the constructor.
	///
	/// [`position`]: #method.set_position
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "maxPosition")]
	pub fn max_position(&self) -> usize {
		self.decoder.max_position()
	}

	/// Gets the current data position. This value is always <= [`maxPosition`].
	/// When [`position`] == [`maxPosition`], it's not possible to decode more
	/// instructions and [`canDecode`] returns `false`.
	///
	/// [`maxPosition`]: #method.max_position
	/// [`position`]: #method.position
	/// [`canDecode`]: #method.can_decode
	#[wasm_bindgen(getter)]
	pub fn position(&self) -> usize {
		self.decoder.position()
	}

	/// Sets the current data position, which is the index into the data passed to the constructor.
	/// This value is always <= [`maxPosition`]
	///
	/// [`maxPosition`]: #method.max_position
	///
	/// # Throws
	///
	/// Throws if the new position is invalid.
	///
	/// # Arguments
	///
	/// * `new_pos`: New position and must be <= [`maxPosition`]
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
	///
	/// // nop and pause
	/// const bytes = new Uint8Array([0x90, 0xF3, 0x90]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	///
	/// assert.equal(decoder.position, 0);
	/// assert.equal(decoder.maxPosition, 3);
	/// const instr = decoder.decode();
	/// assert.equal(decoder.position, 1);
	/// assert.equal(instr.code, Code.Nopd);
	///
	/// decoder.decodeOut(instr);
	/// assert.equal(decoder.position, 3);
	/// assert.equal(instr.code, Code.Pause);
	///
	/// // Start all over again
	/// decoder.position = 0;
	/// assert.equal(decoder.position, 0);
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.code, Code.Nopd);
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.code, Code.Pause);
	/// assert.equal(decoder.position, 3);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(setter)]
	pub fn set_position(&mut self, new_pos: usize) -> Result<(), JsValue> {
		self.decoder.set_position(new_pos).map_err(to_js_error)
	}

	/// Returns `true` if there's at least one more byte to decode. It doesn't verify that the
	/// next instruction is valid, it only checks if there's at least one more byte to read.
	/// See also [`position`] and [`maxPosition`]
	///
	/// It's not required to call this method. If this method returns `false`, then [`decodeOut()`]
	/// and [`decode()`] will return an instruction whose [`code`] == [`Code.INVALID`].
	///
	/// [`position`]: #method.position
	/// [`maxPosition`]: #method.max_position
	/// [`decodeOut()`]: #method.decode_out
	/// [`decode()`]: #method.decode
	/// [`code`]: struct.Instruction.html#method.code
	/// [`Code.INVALID`]: enum.Code.html#variant.INVALID
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
	///
	/// // nop and an incomplete instruction
	/// const bytes = new Uint8Array([0x90, 0xF3, 0x0F]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	///
	/// // 3 bytes left to read
	/// assert.ok(decoder.canDecode);
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Nopd);
	///
	/// // 2 bytes left to read
	/// assert.ok(decoder.canDecode);
	/// decoder.decodeOut(instr);
	/// // Not enough bytes left to decode a full instruction
	/// assert.equal(instr.code, Code.INVALID);
	///
	/// // 0 bytes left to read
	/// assert.ok(!decoder.canDecode);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "canDecode")]
	pub fn can_decode(&self) -> bool {
		self.decoder.can_decode()
	}

	/// Decodes all instructions and returns an array of [`Instruction`]s
	///
	/// [`Instruction`]: struct.Instruction.html
	#[wasm_bindgen(js_name = "decodeAll")]
	pub fn decode_all(&mut self) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.decoder.iter().map(|i| JsValue::from(Instruction(i))).collect()
	}

	/// Decodes at most `count` instructions and returns an array of [`Instruction`]s.
	/// It returns less than `count` instructions if there's not enough data left to decode.
	///
	/// [`Instruction`]: struct.Instruction.html
	///
	/// # Arguments
	///
	/// - `count`: Max number of instructions to decode
	#[wasm_bindgen(js_name = "decodeInstructions")]
	pub fn decode_instructions(&mut self, count: usize) -> js_sys::Array {
		//TODO: https://github.com/rustwasm/wasm-bindgen/issues/111
		self.decoder.iter().take(count).map(|i| JsValue::from(Instruction(i))).collect()
	}

	/// Gets the last decoder error. Unless you need to know the reason it failed,
	/// it's better to check [`instruction.isInvalid()`].
	///
	/// It returns a [`DecoderError`] enum value.
	///
	/// [`instruction.isInvalid()`]: struct.Instruction.html#method.is_invalid
	/// [`DecoderError`]: enum.DecoderError.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "lastError")]
	pub fn last_error(&self) -> DecoderError {
		iced_to_decoder_error(self.decoder.last_error())
	}

	/// Decodes and returns the next instruction, see also [`decodeOut()`]
	/// which avoids allocating a new instruction.
	/// See also [`lastError`].
	///
	/// [`decodeOut()`]: #method.decode_out
	/// [`lastError`]: #method.last_error
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions, MemorySize, Mnemonic, OpKind, Register } = require("iced-x86");
	///
	/// // xrelease lock add [rax],ebx
	/// const bytes = new Uint8Array([0xF0, 0xF3, 0x01, 0x18]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	/// const instr = decoder.decode();
	///
	/// assert.equal(instr.code, Code.Add_rm32_r32);
	/// assert.equal(instr.mnemonic, Mnemonic.Add);
	/// assert.equal(instr.length, 4);
	/// assert.equal(instr.opCount, 2);
	///
	/// assert.equal(instr.op0Kind, OpKind.Memory);
	/// assert.equal(instr.memoryBase, Register.RAX);
	/// assert.equal(instr.memoryIndex, Register.None);
	/// assert.equal(instr.memoryIndexScale, 1);
	/// assert.equal(instr.memoryDisplacement, 0);
	/// assert.equal(instr.memorySegment, Register.DS);
	/// assert.equal(instr.segmentPrefix, Register.None);
	/// assert.equal(instr.memorySize, MemorySize.UInt32);
	///
	/// assert.equal(instr.op1Kind, OpKind.Register);
	/// assert.equal(instr.op1Register, Register.EBX);
	///
	/// assert.ok(instr.hasLockPrefix);
	/// assert.ok(instr.hasXreleasePrefix);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	pub fn decode(&mut self) -> Instruction {
		Instruction(self.decoder.decode())
	}

	/// Decodes the next instruction. The difference between this method and [`decode()`] is that this
	/// method doesn't need to allocate a new instruction.
	/// See also [`lastError`].
	///
	/// [`decode()`]: #method.decode
	/// [`lastError`]: #method.last_error
	///
	/// # Arguments
	///
	/// * `instruction`: Updated with the decoded instruction. All fields are initialized (it's an `out` argument)
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions, Instruction, MemorySize, Mnemonic, OpKind, Register } = require("iced-x86");
	///
	/// // xrelease lock add [rax],ebx
	/// const bytes = new Uint8Array([0xF0, 0xF3, 0x01, 0x18]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	/// const instr = new Instruction();
	/// decoder.decodeOut(instr);
	///
	/// assert.equal(instr.code, Code.Add_rm32_r32);
	/// assert.equal(instr.mnemonic, Mnemonic.Add);
	/// assert.equal(instr.length, 4);
	/// assert.equal(instr.opCount, 2);
	///
	/// assert.equal(instr.op0Kind, OpKind.Memory);
	/// assert.equal(instr.memoryBase, Register.RAX);
	/// assert.equal(instr.memoryIndex, Register.None);
	/// assert.equal(instr.memoryIndexScale, 1);
	/// assert.equal(instr.memoryDisplacement, 0);
	/// assert.equal(instr.memorySegment, Register.DS);
	/// assert.equal(instr.segmentPrefix, Register.None);
	/// assert.equal(instr.memorySize, MemorySize.UInt32);
	///
	/// assert.equal(instr.op1Kind, OpKind.Register);
	/// assert.equal(instr.op1Register, Register.EBX);
	///
	/// assert.ok(instr.hasLockPrefix);
	/// assert.ok(instr.hasXreleasePrefix);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "decodeOut")]
	pub fn decode_out(&mut self, instruction: &mut Instruction) {
		self.decoder.decode_out(&mut instruction.0);
	}
}

#[wasm_bindgen]
#[cfg(feature = "instr_info")]
impl Decoder {
	/// Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
	/// The caller can check if there are any relocations at those addresses.
	///
	/// # Arguments
	///
	/// * `instruction`: The latest instruction that was decoded by this decoder
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
	///
	/// // nop
	/// // xor dword ptr [rax-5AA5EDCCh],5Ah
	/// const bytes = new Uint8Array(
	///     //     00    01    02    03    04    05    06
	///     //     opc   modrm displacement__________  imm
	///     [0x90, 0x83, 0xB3, 0x34, 0x12, 0x5A, 0xA5, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// decoder.ip = 0x12345678n;
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Nopd);
	/// decoder.decodeOut(instr);
	/// const co = decoder.getConstantOffsets(instr);
	///
	/// assert.ok(co.hasDisplacement);
	/// assert.equal(co.displacementOffset, 2);
	/// assert.equal(co.displacementSize, 4);
	/// assert.ok(co.hasImmediate);
	/// assert.equal(co.immediateOffset, 6);
	/// assert.equal(co.immediateSize, 1);
	/// // It's not an instruction with two immediates (e.g. enter)
	/// assert.ok(!co.hasImmediate2);
	/// assert.equal(co.immediateOffset2, 0);
	/// assert.equal(co.immediateSize2, 0);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// co.free();
	/// ```
	#[wasm_bindgen(js_name = "getConstantOffsets")]
	pub fn get_constant_offsets(&self, instruction: &Instruction) -> ConstantOffsets {
		ConstantOffsets(self.decoder.get_constant_offsets(&instruction.0))
	}
}
