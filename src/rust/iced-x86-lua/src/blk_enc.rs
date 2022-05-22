// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::instr::Instruction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_BlockEncoder : BlockEncoder }
lua_impl_userdata! { BlockEncoder }

/// Encodes instructions
///
/// `Encoder` can only encode one instruction at a time. This class can encode any number of
/// instructions and can also fix short branches if the target is too far away.
///
/// It will fail if there's an instruction with an RIP-relative operand (`[rip+123h]`) and the target is too far away.
/// A workaround is to use a new base RIP of the encoded instructions that is close (+/-2GB) to the original location.
///
/// # Examples
///
/// ```lua
/// from iced_x86 import *
///
/// data = b"\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3"
/// decoder = Decoder(64, data, ip=0x1234_5678)
///
/// instrs = [instr for instr in decoder]
///
/// encoder = BlockEncoder(64)
/// # Add an instruction
/// encoder.add(instrs[0])
/// # Add more instructions
/// encoder.add_many(instrs[1:])
/// try:
///     # Encode all added instructions and get the raw bytes
///     raw_data = encoder.encode(0x3456_789A)
/// except ValueError as ex:
///     print("Could not encode all instructions")
///     raise
///
/// # It has no IP-relative instructions (eg. branches or [rip+xxx] ops)
/// # so the result should be identical to the original code.
/// assert data == raw_data
/// ```
/// @class BlockEncoder
#[allow(clippy::doc_markdown)]
struct BlockEncoder;

impl BlockEncoder {
	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in BLOCK_ENCODER_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);
		}
	}
}

lua_pub_methods! { static BLOCK_ENCODER_EXPORTS =>
	/// Encodes all instructions and returns the raw bytes
	///
	/// Error if one or more instructions couldn't be encoded.
	///
	/// @param bitness integer # 16, 32 or 64
	/// @param instructions Instruction[] # Instructions to encode
	/// @param rip integer # Base IP of all encoded instructions
	/// @param options integer # (optional, default = `BlockEncoderOptions.None`) Options
	/// @return table
	unsafe fn encode(lua, bitness: u32, instructions: LuaAny, rip: u64, options: Option<u32>) -> 1 {
		let options = options.unwrap_or(iced_x86::BlockEncoderOptions::NONE);
		unsafe {
			let instrs = lua.read_array(instructions.idx, |lua| {
				if let Some(instr) = lua.try_get_user_data::<Instruction>(-1) {
					Ok(instr.inner)
				} else {
					Err(LuaError::MessageStr("Expected an array of instructions"))
				}
			});
			let block = iced_x86::InstructionBlock::new(&instrs, rip);
			match iced_x86::BlockEncoder::encode(bitness, block, options) {
				Ok(result) => {
					lua.create_table(0, 1);
					lua.push("code_buffer");
					lua.push(&result.code_buffer);
					lua.raw_set(-3);
				}
				Err(e) => lua.throw_error(e),
			}
		}
	}
}
