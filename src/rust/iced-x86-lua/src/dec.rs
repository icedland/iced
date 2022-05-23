// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::coffs::ConstantOffsets;
use crate::instr::Instruction;
use iced_x86::{DecoderOptions, IcedError};
use loona::lua_api::lua_CFunction;
use loona::prelude::*;
use std::{ptr, slice};

lua_struct_module! { luaopen_iced_x86_Decoder : Decoder }
lua_impl_userdata! { Decoder }

/// Decodes 16/32/64-bit x86 instructions
/// @class Decoder
struct Decoder {
	// The decoder holds a reference to this data
	#[allow(dead_code)]
	data: Vec<u8>,
	inner: iced_x86::Decoder<'static>,
}

impl Decoder {
	fn new(bitness: u32, data: &[u8], options: u32, ip: u64) -> Result<Self, IcedError> {
		let data = data.to_vec();
		let decoder_data: &'static [u8] = unsafe { slice::from_raw_parts(data.as_ptr(), data.len()) };

		let decoder = iced_x86::Decoder::try_with_ip(bitness, decoder_data, ip, options)?;
		Ok(Decoder { data, inner: decoder })
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in DECODER_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__gc", decoder_dtor),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static DECODER_EXPORTS =>
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
	unsafe fn new(lua, _ignore: LuaIgnore, bitness: u32, data: &[u8], options: LuaDefaultU32<{DecoderOptions::NONE}>, ip: LuaDefaultU64<0>) -> 1 {
		unsafe {
			let decoder = match Decoder::new(bitness, data, options, ip) {
				Ok(decoder) => decoder,
				Err(e) => lua.throw_error(e),
			};
			let _ = lua.push_user_data(decoder);

			lua_get_or_init_metatable!(Decoder : lua);
			let _ = lua.set_metatable(-2);
		}
	}

	/// The current `IP`/`EIP`/`RIP` value, see also `Decoder:position()`
	/// @return integer
	unsafe fn ip(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.ip())
		}
	}

	unsafe fn set_ip(lua, decoder: &mut Decoder, ip: u64) -> 0 {
		decoder.inner.set_ip(ip);
	}

	/// Gets the bitness (16, 32 or 64)
	/// @return integer
	unsafe fn bitness(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.bitness());
		}
	}

	/// Gets the max value that can be written to `Decoder:position()`
	///
	/// This is the size of the data that gets decoded to instructions and it's the length of the data that was passed to the constructor.
	/// @return integer
	unsafe fn max_position(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.max_position());
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
	unsafe fn position(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.position());
		}
	}

	unsafe fn set_position(lua, decoder: &mut Decoder, pos: usize) -> 0 {
		unsafe {
			if let Err(e) = decoder.inner.set_position(pos) {
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
	unsafe fn can_decode(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.can_decode());
		}
	}

	/// Gets the last decoder error (a `DecoderError` enum value).
	///
	/// Unless you need to know the reason it failed, it's better to check `Instruction:is_invalid()`.
	/// @return integer #`DecoderError` enum value
	unsafe fn last_error(lua, decoder: &Decoder) -> 1 {
		unsafe {
			lua.push(decoder.inner.last_error() as u32);
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
	unsafe fn decode(lua, decoder: &mut Decoder) -> 1 {
		unsafe {
			let instr = Instruction::push_new(lua);
			decoder.inner.decode_out(&mut instr.inner);
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
	unsafe fn decode_out(lua, decoder: &mut Decoder, instr: &mut Instruction) -> 1 {
		unsafe {
			lua.push(decoder.inner.can_decode());
		}
		decoder.inner.decode_out(&mut instr.inner);
	}

	/// An iterator that returns the remaining instructions.
	///
	/// The iterator returns the passed in instruction, which gets overwritten with the next
	/// decoded instruction. If no instruction is passed in, a new one is created and that one
	/// is returned every iteration.
	///
	/// @param instr? Instruction #(optional) Overwrite this instruction with the new decoded instruction
	///
	/// ```lua
	/// --TODO:
	/// ```
	unsafe fn iter_out(lua, _decoder: &Decoder, instr: Option<&Instruction>) -> 3 {
		unsafe {
			lua.push_c_function(iter_out_worker);
			lua.push_value(1);
			if instr.is_some() {
				lua.push_value(2);
			} else {
				let _instr = Instruction::push_new(lua);
			}
		}
	}

	/// An iterator that decodes and returns the remaining instructions.
	///
	/// This iterator is slower than `iter_out()` because it allocates and returns a new
	/// instruction. `iter_out()` overwrites the passed in instruction and never allocates.
	///
	/// ```lua
	/// --TODO:
	/// ```
	unsafe fn iter_slow_copy(lua, _decoder: &Decoder) -> 3 {
		unsafe {
			lua.push_c_function(iter_slow_copy_worker);
			lua.push_value(1);
			lua.push(Nil);
		}
	}

	/// Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
	///
	/// The caller can check if there are any relocations at those addresses.
	///
	/// @param instr Instruction #The latest instruction that was decoded by this decoder
	/// @return ConstantOffsets #Offsets and sizes of immediates
	///
	/// # Examples
	///
	/// ```lua
	/// from iced_x86 import *
	///
	/// # nop
	/// # xor dword ptr [rax-5AA5EDCCh],5Ah
	/// #              00  01  02  03  04  05  06
	/// #            \opc\mrm\displacement___\imm
	/// data = b"\x90\x83\xB3\x34\x12\x5A\xA5\x5A"
	/// decoder = Decoder(64, data, ip=0x1234_5678)
	/// assert decoder.decode().code == Code.NOPD
	/// instr = decoder.decode()
	/// co = decoder.get_constant_offsets(instr)
	///
	/// assert co.has_displacement
	/// assert co.displacement_offset == 2
	/// assert co.displacement_size == 4
	/// assert co.has_immediate
	/// assert co.immediate_offset == 6
	/// assert co.immediate_size == 1
	/// # It's not an instruction with two immediates (e.g. enter)
	/// assert not co.has_immediate2
	/// assert co.immediate_offset2 == 0
	/// assert co.immediate_size2 == 0
	/// ```
	unsafe fn get_constant_offsets(lua, decoder: &Decoder, instr: &Instruction) -> 1 {
		let co = ConstantOffsets { inner: decoder.inner.get_constant_offsets(&instr.inner) };
		let _ = unsafe { ConstantOffsets::init_and_push(lua, &co) };
	}
}

lua_methods! {
	unsafe fn iter_out_worker(lua, decoder: &mut Decoder, instr: &mut Instruction) -> 1 {
		unsafe {
			if decoder.inner.can_decode() {
				decoder.inner.decode_out(&mut instr.inner);
				lua.push_value(2);
			} else {
				lua.push(Nil);
			}
		}
	}

	unsafe fn iter_slow_copy_worker(lua, decoder: &mut Decoder) -> 1 {
		unsafe {
			if decoder.inner.can_decode() {
				let instr = Instruction::push_new(lua);
				decoder.inner.decode_out(&mut instr.inner);
			} else {
				lua.push(Nil);
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
