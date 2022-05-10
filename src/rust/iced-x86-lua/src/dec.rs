// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::instr::Instruction;
use crate::ud::UserDataIds;
use iced_x86::{DecoderOptions, IcedError};
use libc::c_int;
use loona::lua::{Lua, LuaUserData};
use loona::lua_api::{lua_CFunction, lua_Integer};
use loona::lua_methods;
use std::{ptr, slice};

lua_struct_module! { luaopen_iced_x86_Decoder : Decoder }
lua_impl_userdata! { Decoder }

/// Decodes 16/32/64-bit x86 instructions
/// @class Decoder
struct Decoder {
	// The decoder holds a reference to this data
	#[allow(dead_code)]
	data: Vec<u8>,
	decoder: iced_x86::Decoder<'static>,
}

impl Decoder {
	fn new(bitness: u32, data: &[u8], options: u32, ip: u64) -> Result<Self, IcedError> {
		let data = data.to_vec();
		let decoder_data: &'static [u8] = unsafe { slice::from_raw_parts(data.as_ptr(), data.len()) };

		let decoder = iced_x86::Decoder::try_with_ip(bitness, decoder_data, ip, options)?;
		Ok(Decoder { data, decoder })
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push_literal("__index");
			lua.new_table();

			let methods: &[(&str, lua_CFunction)] = &[
				("new", decoder_new),
				("ip", decoder_ip),
				("set_ip", decoder_set_ip),
				("bitness", decoder_bitness),
				("max_position", decoder_max_position),
				("position", decoder_position),
				("set_position", decoder_set_position),
				("can_decode", decoder_can_decode),
				("last_error", decoder_last_error),
				("decode", decoder_decode),
				("decode_out", decoder_decode_out),
				("iter_out", decoder_iter_out),
				("iter_slow_copy", decoder_iter_slow_copy),
				//TODO: get_constant_offsets
			];
			for &(name, method) in methods {
				lua.push_literal(name);
				lua.push_c_function(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			lua.push_literal("__gc");
			lua.push_c_function(decoder_dtor);
			lua.raw_set(-3);
		}
	}
}

lua_methods! {
	/// Creates a new decoder
	///
	/// @param bitness integer #16, 32 or 64
	/// @param data string #Data to decode
	/// @param options? integer #(optional, default = `None`) Decoder options, eg. `DecoderOptions.NoInvalidCheck + DecoderOptions.AMD`
	/// @param ip? integer #(optional, default = `0`) Address of first decoded instruction
	/// @return Decoder
	///
	/// # Examples
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local DecoderOptions = require("iced_x86.DecoderOptions")
	/// local Code = require("iced_x86.Code")
	/// local Mnemonic = require("iced_x86.Mnemonic")
	///
	/// -- xchg ah,[rdx+rsi+16h]
	/// -- xacquire lock add dword ptr [rax],5Ah
	/// -- vmovdqu64 zmm18{k3}{z},zmm11
	/// local bytes = "\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3"
	/// local decoder = Decoder:new(64, bytes, DecoderOptions.None, 0x12345678)
	///
	/// local instr1 = decoder:decode()
	/// assert(instr1:code() == Code.Xchg_rm8_r8)
	/// assert(instr1:mnemonic() == Mnemonic.Xchg)
	/// assert(instr1:len() == 4)
	///
	/// local instr2 = decoder:decode()
	/// assert(instr2:code() == Code.Add_rm32_imm8)
	/// assert(instr2:mnemonic() == Mnemonic.Add)
	/// assert(instr2:len() == 5)
	///
	/// local instr3 = decoder:decode()
	/// assert(instr3:code() == Code.EVEX_Vmovdqu64_zmm_k1z_zmmm512)
	/// assert(instr3:mnemonic() == Mnemonic.Vmovdqu64)
	/// assert(instr3:len() == 6)
	/// ```
	///
	/// It's sometimes useful to decode some invalid instructions, eg. `lock add esi,ecx`.
	/// Pass in `DecoderOptions.NO_INVALID_CHECK` to the constructor and the decoder
	/// will decode some invalid encodings.
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local DecoderOptions = require("iced_x86.DecoderOptions")
	/// local Code = require("iced_x86.Code")
	///
	/// -- lock add esi,ecx    lock not allowed
	/// local bytes = "\xF0\x01\xCE"
	/// local decoder = Decoder:new(64, bytes, DecoderOptions.None, 0x12345678)
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.INVALID)
	///
	/// -- We want to decode some instructions with invalid encodings
	/// local decoder = Decoder:new(64, bytes, DecoderOptions.NoInvalidCheck, 0x12345678)
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.Add_rm32_r32)
	/// assert(instr:has_lock_prefix())
	/// ```
	unsafe fn decoder_new(lua) -> 1 {
		unsafe {
			// 1 = decoder.metatable.__index
			let bitness = lua.get_u32(2);
			let data = lua.get_string(3);
			let options = lua.get_u32_default(4, DecoderOptions::NONE);
			let ip = lua.get_u64_default(5, 0);

			let decoder = match Decoder::new(bitness, data, options, ip) {
				Ok(decoder) => decoder,
				Err(e) => lua.throw_error(e),
			};
			let _ = lua.push_user_data(decoder);

			lua.get_registry_metatable(Decoder::METATABLE_KEY);
			let _ = lua.set_metatable(-2);
		}
	}

	/// The current `IP`/`EIP`/`RIP` value, see also `Decoder:position()`
	/// @return integer
	unsafe fn decoder_ip(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_integer(decoder.decoder.ip() as lua_Integer)
		}
	}

	/// The current `IP`/`EIP`/`RIP` value, see also `Decoder:position()`
	/// @param value integer #New value
	unsafe fn decoder_set_ip(lua) -> 0 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			let ip = lua.get_u64(2);
			decoder.decoder.set_ip(ip);
		}
	}

	/// Gets the bitness (16, 32 or 64)
	/// @return integer
	unsafe fn decoder_bitness(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_integer(decoder.decoder.bitness() as lua_Integer);
		}
	}

	/// Gets the max value that can be written to `Decoder:position()`
	///
	/// This is the size of the data that gets decoded to instructions and it's the length of the data that was passed to the constructor.
	/// @return integer
	unsafe fn decoder_max_position(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_integer(decoder.decoder.max_position() as lua_Integer);
		}
	}

	/// The current data position, which is the index into the data passed to the constructor.
	///
	/// This value is always <= `Decoder:max_position()`. When `Decoder:position()` == `Decoder:max_position()`, it's not possible to decode more
	/// instructions and `Decoder:can_decode()` returns `false`.
	/// @return integer
	///
	/// # Examples
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Code = require("iced_x86.Code")
	///
	/// -- nop and pause
	/// local data = "\x90\xF3\x90"
	/// local decoder = Decoder:new(64, data)
	///
	/// assert(decoder:position() == 0)
	/// assert(decoder:max_position() == 3)
	/// local instr = decoder:decode()
	/// assert(decoder:position() == 1)
	/// assert(instr:code() == Code.Nopd)
	///
	/// instr = decoder:decode()
	/// assert(decoder:position() == 3)
	/// assert(instr:code() == Code.Pause)
	///
	/// -- Start all over again
	/// decoder:set_position(0)
	/// decoder:set_ip(0)
	/// assert(decoder:position() == 0)
	/// assert(decoder:decode().code() == Code.Nopd)
	/// assert(decoder:decode().code() == Code.Pause)
	/// assert(decoder:position() == 3)
	/// ```
	unsafe fn decoder_position(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_integer(decoder.decoder.position() as lua_Integer);
		}
	}

	/// The current data position, which is the index into the data passed to the constructor.
	/// @param value integer #New position
	unsafe fn decoder_set_position(lua) -> 0 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			let pos = lua.get_usize(2);
			if let Err(e) = decoder.decoder.set_position(pos) {
				lua.throw_error(e);
			}
		}
	}

	/// Returns `true` if there's at least one more byte to decode.
	///
	/// It doesn't verify that the next instruction is valid, it only checks if there's
	/// at least one more byte to read. See also `Decoder:position()` and `Decoder:max_position()`.
	///
	/// It's not required to call this method. If this method returns `false`, then `Decoder:decode_out()`
	/// and `Decoder:decode()` will return an instruction whose `Instruction:code()` == `Code.INVALID`.
	/// @return boolean
	///
	/// # Examples
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Code = require("iced_x86.Code")
	/// local DecoderError = require("iced_x86.DecoderError")
	///
	/// -- nop and an incomplete instruction
	/// local data = "\x90\xF3\x0F"
	/// local decoder = Decoder:new(64, data)
	///
	/// -- 3 bytes left to read
	/// assert(decoder:can_decode())
	/// local instr = decoder:decode()
	/// assert(instr:code() == Code.Nopd)
	///
	/// -- 2 bytes left to read
	/// assert(decoder:can_decode())
	/// instr = decoder:decode()
	/// -- Not enough bytes left to decode a full instruction
	/// assert(decoder:last_error() == DecoderError.NoMoreBytes)
	/// assert(instr:code() == Code.INVALID)
	/// assert(instr:is_invalid())
	///
	/// -- 0 bytes left to read
	/// assert(not decoder:can_decode())
	/// ```
	unsafe fn decoder_can_decode(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_boolean(decoder.decoder.can_decode() as c_int);
		}
	}

	/// Gets the last decoder error (a `DecoderError` enum value).
	///
	/// Unless you need to know the reason it failed, it's better to check `Instruction:is_invalid()`.
	/// @return integer #`DecoderError` enum value
	unsafe fn decoder_last_error(lua) -> 1 {
		unsafe {
			let decoder: &Decoder = lua.get_user_data(1);
			lua.push_integer(decoder.decoder.last_error() as lua_Integer);
		}
	}

	/// Decodes and returns the next instruction.
	///
	/// See also `Decoder:decode_out()` which avoids copying the decoded instruction to the caller's return variable.
	/// See also `Decoder:last_error()`.
	///
	/// @return Instruction #The next instruction
	///
	/// # Examples
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Code = require("iced_x86.Code")
	/// local Mnemonic = require("iced_x86.Mnemonic")
	/// local OpKind = require("iced_x86.OpKind")
	/// local Register = require("iced_x86.Register")
	/// local MemorySize = require("iced_x86.MemorySize")
	///
	/// -- xrelease lock add [rax],ebx
	/// local data = "\xF0\xF3\x01\x18"
	/// local decoder = Decoder:new(64, data)
	/// local instr = decoder:decode()
	///
	/// assert(instr:code() == Code.Add_rm32_r32)
	/// assert(instr:mnemonic() == Mnemonic.Add)
	/// assert(instr:len() == 4)
	/// assert(instr:op_count() == 2)
	///
	/// assert(instr:op0_kind() == OpKind.Memory)
	/// assert(instr:memory_base() == Register.RAX)
	/// assert(instr:memory_index() == Register.None)
	/// assert(instr:memory_index_scale() == 1)
	/// assert(instr:memory_displacement() == 0)
	/// assert(instr:memory_segment() == Register.DS)
	/// assert(instr:segment_prefix() == Register.None)
	/// assert(instr:memory_size() == MemorySize.UInt32)
	///
	/// assert(instr:op1_kind() == OpKind.Register)
	/// assert(instr:op1_register() == Register.EBX)
	///
	/// assert(instr:has_lock_prefix())
	/// assert(instr:has_xrelease_prefix())
	/// ```
	unsafe fn decoder_decode(lua) -> 1 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			let instr = Instruction::new(&lua);
			decoder.decoder.decode_out(&mut instr.instr);
		}
	}

	/// Decodes the next instruction. Returns a boolean indicating whether there was at least one
	/// byte available to read when this method was called.
	///
	/// The difference between this method and `Decoder:decode` is that this method doesn't need to
	/// allocate a new instruction since it overwrites the input instruction.
	///
	/// @param instr Instruction #Updated with the decoded instruction
	///
	/// # Examples
	///
	/// ```lua
	/// local Decoder = require("iced_x86.Decoder")
	/// local Instruction = require("iced_x86.Instruction")
	/// local Code = require("iced_x86.Code")
	/// local Mnemonic = require("iced_x86.Mnemonic")
	/// local OpKind = require("iced_x86.OpKind")
	/// local Register = require("iced_x86.Register")
	/// local MemorySize = require("iced_x86.MemorySize")
	///
	/// -- xrelease lock add [rax],ebx
	/// local data = "\xF0\xF3\x01\x18"
	/// local decoder = Decoder(64, data)
	/// local instr = Instruction:new()
	/// decoder:decode_out(instr)
	///
	/// assert(instr:code() == Code.Add_rm32_r32)
	/// assert(instr:mnemonic() == Mnemonic.Add)
	/// assert(instr:len() == 4)
	/// assert(instr:op_count() == 2)
	///
	/// assert(instr:op0_kind() == OpKind.Memory)
	/// assert(instr:memory_base() == Register.RAX)
	/// assert(instr:memory_index() == Register.None)
	/// assert(instr:memory_index_scale() == 1)
	/// assert(instr:memory_displacement() == 0)
	/// assert(instr:memory_segment() == Register.DS)
	/// assert(instr:segment_prefix() == Register.None)
	/// assert(instr:memory_size() == MemorySize.UInt32)
	///
	/// assert(instr:op1_kind() == OpKind.Register)
	/// assert(instr:op1_register() == Register.EBX)
	///
	/// assert(instr:has_lock_prefix())
	/// assert(instr:has_xrelease_prefix())
	/// ```
	unsafe fn decoder_decode_out(lua) -> 1 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			let instr: &mut Instruction = lua.get_user_data_mut(2);
			lua.push_boolean(if decoder.decoder.can_decode() { 1 } else { 0 });
			decoder.decoder.decode_out(&mut instr.instr);
		}
	}

	//TODO: doc comments here
	unsafe fn decoder_iter_out(lua) -> 3 {
		unsafe {
			let _decoder: &Decoder = lua.get_user_data(1);
			let has_instr = lua.get_top() >= 2;
			if has_instr {
				let _instr: &Instruction = lua.get_user_data(2);
			}
			lua.push_c_function(decoder_iter_out_worker);
			lua.push_value(1);
			if has_instr {
				lua.push_value(2);
			} else {
				let _instr = Instruction::new(&lua);
			}
		}
	}

	unsafe fn decoder_iter_out_worker(lua) -> 1 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			let instr: &mut Instruction = lua.get_user_data_mut(2);
			if decoder.decoder.can_decode() {
				decoder.decoder.decode_out(&mut instr.instr);
				lua.push_value(2);
			} else {
				lua.push_nil();
			}
		}
	}

	//TODO: doc comments here
	unsafe fn decoder_iter_slow_copy(lua) -> 3 {
		unsafe {
			let _decoder: &Decoder = lua.get_user_data(1);
			lua.push_c_function(decoder_iter_slow_copy_worker);
			lua.push_value(1);
			lua.push_nil();
		}
	}

	unsafe fn decoder_iter_slow_copy_worker(lua) -> 1 {
		unsafe {
			let decoder: &mut Decoder = lua.get_user_data_mut(1);
			if decoder.decoder.can_decode() {
				let instr = Instruction::new(&lua);
				decoder.decoder.decode_out(&mut instr.instr);
			} else {
				lua.push_nil();
			}
		}
	}

	unsafe fn decoder_dtor(lua) -> 0 {
		unsafe {
			let decoder: *mut Decoder = lua.get_user_data_mut(1);
			ptr::drop_in_place(decoder);
		}
	}
}
