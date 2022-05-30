// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::coffs::ConstantOffsets;
use crate::instr::Instruction;
use loona::lua_api::lua_CFunction;
use loona::prelude::*;
use std::ptr;

lua_struct_module! { luaopen_iced_x86_Encoder : Encoder }
lua_impl_userdata! { Encoder }

/// Encodes instructions decoded by the decoder or instructions created by other code.
/// See also `BlockEncoder` which can encode any number of instructions.
/// @class Encoder
struct Encoder {
	inner: iced_x86::Encoder,
}

impl Encoder {
	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in ENCODER_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__gc", encoder_dtor),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static ENCODER_EXPORTS =>
	/// Encodes instructions decoded by the decoder or instructions created by other code.
	///
	/// See also `BlockEncoder` which can encode any number of instructions.
	///
	/// @param bitness integer # 16, 32 or 64
	/// @param capacity? integer # (default = 0) Initial capacity of the byte buffer
	/// @return Encoder
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Encoder = require("iced_x86.Encoder")
	///
	/// -- xchg ah,[rdx+rsi+16h]
	/// local data = "\134\100\050\022"
	/// local decoder = Decoder.new(64, data, nil, 0x12345678)
	/// local instr = decoder:decode()
	///
	/// local encoder = Encoder.new(64)
	/// local instr_len = encoder:encode(instr, 0x55555555)
	/// assert(instr_len == 4)
	///
	/// -- We're done, take ownership of the buffer
	/// local buffer = encoder:take_buffer()
	/// assert(buffer == "\134\100\050\022")
	/// ```
	unsafe fn new(lua, bitness: u32, capacity: Option<usize>) -> 1 {
		unsafe {
			let capacity = capacity.unwrap_or(0);
			let encoder = match iced_x86::Encoder::try_with_capacity(bitness, capacity) {
				Ok(encoder) => Encoder { inner: encoder },
				Err(e) => lua.throw_error(e),
			};
			let _ = lua.push_user_data(encoder);
			lua_get_or_init_metatable!(Encoder : lua);
			let _ = lua.set_metatable(-2);
		}
	}

	/// Encodes an instruction and returns the size of the encoded instruction
	///
	/// Error if it failed to encode the instruction (eg. a target branch / RIP-rel operand is too far away)
	///
	/// @param instruction Instruction # Instruction to encode
	/// @param rip integer # (`u64`) `RIP` of the encoded instruction
	/// @return integer # Size of the encoded instruction
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Encoder = require("iced_x86.Encoder")
	///
	/// -- je short $+4
	/// local data = "\117\002"
	/// local decoder = Decoder.new(64, data, nil, 0x12345678)
	/// local instr = decoder:decode()
	///
	/// local encoder = Encoder.new(64)
	/// -- Use a different IP (orig rip + 0x10)
	/// local instr_len = encoder:encode(instr, 0x12345688)
	/// assert(instr_len == 2)
	///
	/// -- We're done, take ownership of the buffer
	/// local buffer = encoder:take_buffer()
	/// assert(buffer == "\117\242")
	/// ```
	unsafe fn encode(lua, this: &mut Encoder, instruction: &Instruction, rip: u64) -> 1 {
		match this.inner.encode(&instruction.inner, rip) {
			Ok(size) => unsafe { lua.push(size) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Writes a byte to the output buffer
	///
	/// @param value integer # (`u8`) Value to write
	///
	/// # Examples
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Encoder = require("iced_x86.Encoder")
	///
	/// -- je short $+4
	/// local data = "\117\002"
	/// local decoder = Decoder.new(64, data, nil, 0x12345678)
	/// local instr = decoder:decode()
	///
	/// local encoder = Encoder.new(64)
	/// -- Add a random byte
	/// encoder:write_u8(0x90)
	///
	/// -- Use a different IP (orig rip + 0x10)
	/// local instr_len = encoder:encode(instr, 0x12345688)
	/// assert(instr_len == 2)
	///
	/// -- Add a random byte
	/// encoder:write_u8(0x90)
	///
	/// -- We're done, take ownership of the buffer
	/// local buffer = encoder:take_buffer()
	/// assert(buffer == "\144\117\242\144")
	/// ```
	unsafe fn write_u8(lua, this: &mut Encoder, value: u8) -> 0 {
		this.inner.write_u8(value)
	}

	/// Returns the buffer and initializes the internal buffer to an empty array.
	///
	/// Should be called when you've encoded all instructions and need the raw instruction bytes.
	///
	/// @return string # The encoded instructions
	unsafe fn take_buffer(lua, this: &mut Encoder) -> 1 {
		let mut buffer = this.inner.take_buffer();
		unsafe { lua.push(&buffer); }
		buffer.clear();
		this.inner.set_buffer(buffer);
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the encoded instruction.
	///
	/// The caller can use this information to add relocations if needed.
	///
	/// @return ConstantOffsets # Offsets and sizes of immediates
	unsafe fn get_constant_offsets(lua, this: &Encoder) -> 1 {
		let co = ConstantOffsets { inner: this.inner.get_constant_offsets() };
		let _ = unsafe { ConstantOffsets::init_and_push(lua, &co) };
	}

	/// Disables 2-byte VEX encoding and encodes all VEX instructions with the 3-byte VEX encoding
	/// @return boolean
	unsafe fn prevent_vex2(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.prevent_vex2()); }
	}

	unsafe fn set_prevent_vex2(lua, this: &mut Encoder, new_value: bool) -> 0 {
		this.inner.set_prevent_vex2(new_value)
	}

	/// Value of the `VEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	/// @return integer
	unsafe fn vex_wig(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.vex_wig()); }
	}

	unsafe fn set_vex_wig(lua, this: &mut Encoder, new_value: u32) -> 0 {
		this.inner.set_vex_wig(new_value)
	}

	/// Value of the `VEX.L` bit to use if it's an instruction that ignores the bit. Default is 0.
	/// @return integer
	unsafe fn vex_lig(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.vex_lig()); }
	}

	unsafe fn set_vex_lig(lua, this: &mut Encoder, new_value: u32) -> 0 {
		this.inner.set_vex_lig(new_value)
	}

	/// Value of the `EVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	/// @return integer
	unsafe fn evex_wig(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.evex_wig()); }
	}

	unsafe fn set_evex_wig(lua, this: &mut Encoder, new_value: u32) -> 0 {
		this.inner.set_evex_wig(new_value)
	}

	/// Value of the `EVEX.L'L` bits to use if it's an instruction that ignores the bits. Default is 0.
	/// @return integer
	unsafe fn evex_lig(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.evex_lig()); }
	}

	unsafe fn set_evex_lig(lua, this: &mut Encoder, new_value: u32) -> 0 {
		this.inner.set_evex_lig(new_value)
	}

	/// Value of the `MVEX.W` bit to use if it's an instruction that ignores the bit. Default is 0.
	/// @return integer
	unsafe fn mvex_wig(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.mvex_wig()); }
	}

	unsafe fn set_mvex_wig(lua, this: &mut Encoder, new_value: u32) -> 0 {
		this.inner.set_mvex_wig(new_value)
	}

	/// Gets the bitness (16, 32 or 64)
	/// @return integer
	unsafe fn bitness(lua, this: &Encoder) -> 1 {
		unsafe { lua.push(this.inner.bitness()); }
	}
}

lua_methods! {
	unsafe fn encoder_dtor(lua) -> 0 {
		unsafe {
			let encoder: *mut Encoder = lua.get_user_data_mut(1);
			ptr::drop_in_place(encoder);
		}
	}
}
