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
/// local BlockEncoder = require("iced_x86.BlockEncoder")
/// local Decoder = require("iced_x86.Decoder")
///
/// local data = "\134\100\050\022\240\242\131\000\090\098\193\254\203\111\211"
/// local decoder = Decoder.new(64, data, nil, 0x12345678)
///
/// local instrs = {}
/// for instr in decoder:iter_out() do
///     instrs[#instrs + 1] = instr:copy()
/// end
///
/// -- Encode all added instructions and get the raw bytes
/// local result = BlockEncoder.encode(64, instrs, 0x3456789A)
/// local raw_data = result.code_buffer
///
/// -- It has no IP-relative instructions (eg. branches or [rip+xxx] ops)
/// -- so the result should be identical to the original code.
/// assert(#data, #raw_data)
/// for i = 1, #data do
///     assert(data[i] == raw_data[i])
/// end
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
	/// @param options? integer # (optional, default = `BlockEncoderOptions.None`) Options
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
					lua.push(result.code_buffer);
					lua.raw_set(-3);
				}
				Err(e) => lua.throw_error(e),
			}
		}
	}
}
