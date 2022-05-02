-- SPDX-License-Identifier: MIT
-- Copyright (C) 2018-present iced project and contributors

local iced_dec = require("iced_x86_priv.dec")
local Instruction = require("iced-x86.Instruction")

---Decodes 16/32/64-bit x86 instructions
---@class Decoder
local Decoder = {}

---Creates a new decoder
---
---@param bitness integer #16, 32 or 64
---@param data string #Data to decode
---@param options? integer #(optional, default = `None`) Decoder options, eg. `DecoderOptions.NoInvalidCheck + DecoderOptions.AMD`
---@param ip? integer #(optional, default = `0`) Address of first decoded instruction
---@return Decoder
---
---# Examples
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local DecoderOptions = require("iced-x86.DecoderOptions")
---local Code = require("iced-x86.Code")
---local Mnemonic = require("iced-x86.Mnemonic")
---
----- xchg ah,[rdx+rsi+16h]
----- xacquire lock add dword ptr [rax],5Ah
----- vmovdqu64 zmm18{k3}{z},zmm11
---local bytes = "\x86\x64\x32\x16\xF0\xF2\x83\x00\x5A\x62\xC1\xFE\xCB\x6F\xD3"
---local decoder = Decoder:new(64, bytes, DecoderOptions.None, 0x12345678)
---
---local instr1 = decoder:decode()
---assert(instr1.code() == Code.Xchg_rm8_r8)
---assert(instr1.mnemonic() == Mnemonic.Xchg)
---assert(instr1.len() == 4)
---
---local instr2 = decoder:decode()
---assert(instr2.code() == Code.Add_rm32_imm8)
---assert(instr2.mnemonic() == Mnemonic.Add)
---assert(instr2.len() == 5)
---
---local instr3 = decoder:decode()
---assert(instr3.code() == Code.EVEX_Vmovdqu64_zmm_k1z_zmmm512)
---assert(instr3.mnemonic() == Mnemonic.Vmovdqu64)
---assert(instr3.len() == 6)
---```
---
---It's sometimes useful to decode some invalid instructions, eg. `lock add esi,ecx`.
---Pass in `DecoderOptions.NO_INVALID_CHECK` to the constructor and the decoder
---will decode some invalid encodings.
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local DecoderOptions = require("iced-x86.DecoderOptions")
---local Code = require("iced-x86.Code")
---
----- lock add esi,ecx    lock not allowed
---local bytes = "\xF0\x01\xCE"
---local decoder = Decoder:new(64, bytes, DecoderOptions.None, 0x12345678)
---local instr = decoder:decode()
---assert(instr.code() == Code.INVALID)
---
----- We want to decode some instructions with invalid encodings
---local decoder = Decoder:new(64, bytes, DecoderOptions.NoInvalidCheck, 0x12345678)
---local instr = decoder:decode()
---assert(instr.code() == Code.Add_rm32_r32)
---assert(instr.has_lock_prefix())
---```
function Decoder:new(bitness, data, options, ip)
	local raw_dec = iced_dec.decoder_new(bitness, data, options ~= nil and options or 0, ip ~= nil and ip or 0)
	local dec = { _dec = raw_dec }
	return setmetatable(dec, { __index = self })
end

---The current `IP`/`EIP`/`RIP` value, see also `Decoder:position()`
---@return integer
function Decoder:ip()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_ip(self._dec)
end

---The current `IP`/`EIP`/`RIP` value, see also `Decoder:position()`
---@param value integer #New value
function Decoder:set_ip(value)
	---@diagnostic disable-next-line: undefined-field
	iced_dec.decoder_set_ip(self._dec, value)
end

---Gets the bitness (16, 32 or 64)
---@return integer
function Decoder:bitness()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_bitness(self._dec)
end

---Gets the max value that can be written to `Decoder:position()`
---
---This is the size of the data that gets decoded to instructions and it's the length of the data that was passed to the constructor.
---@return integer
function Decoder:max_position()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_max_position(self._dec)
end

---The current data position, which is the index into the data passed to the constructor.
---
---This value is always <= `Decoder:max_position()`. When `Decoder:position()` == `Decoder:max_position()`, it's not possible to decode more
---instructions and `Decoder:can_decode()` returns `false`.
---@return integer
---
---# Examples
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local Code = require("iced-x86.Code")
---
----- nop and pause
---local data = "\x90\xF3\x90"
---local decoder = Decoder:new(64, data)
---
---assert(decoder.position() == 0)
---assert(decoder.max_position() == 3)
---local instr = decoder.decode()
---assert(decoder.position() == 1)
---assert(instr.code() == Code.Nopd)
---
---instr = decoder.decode()
---assert(decoder.position() == 3)
---assert(instr.code() == Code.Pause)
---
----- Start all over again
---decoder.set_position(0)
---decoder.set_ip(0)
---assert(decoder.position() == 0)
---assert(decoder.decode().code() == Code.Nopd)
---assert(decoder.decode().code() == Code.Pause)
---assert(decoder.position() == 3)
---```
function Decoder:position()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_position(self._dec)
end

---The current data position, which is the index into the data passed to the constructor.
---@param value integer #New position
function Decoder:set_position(value)
	---@diagnostic disable-next-line: undefined-field
	iced_dec.decoder_set_position(self._dec, value)
end

---Returns `true` if there's at least one more byte to decode.
---
---It doesn't verify that the next instruction is valid, it only checks if there's
---at least one more byte to read. See also `Decoder:position()` and `Decoder:max_position()`.
---
---It's not required to call this method. If this method returns `false`, then `Decoder:decode_out()`
---and `Decoder:decode()` will return an instruction whose `Instruction:code()` == `Code.INVALID`.
---@return boolean
---
---# Examples
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local Code = require("iced-x86.Code")
---local DecoderError = require("iced-x86.DecoderError")
---
----- nop and an incomplete instruction
---local data = "\x90\xF3\x0F"
---local decoder = Decoder:new(64, data)
---
----- 3 bytes left to read
---assert(decoder.can_decode())
---local instr = decoder.decode()
---assert(instr.code() == Code.Nopd)
---
----- 2 bytes left to read
---assert(decoder.can_decode())
---instr = decoder.decode()
----- Not enough bytes left to decode a full instruction
---assert(decoder.last_error() == DecoderError.NoMoreBytes)
---assert(instr.code() == Code.INVALID)
---assert(instr.is_invalid())
---
----- 0 bytes left to read
---assert(not decoder.can_decode())
---```
function Decoder:can_decode()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_can_decode(self._dec)
end

---Gets the last decoder error (a `DecoderError` enum value).
---
---Unless you need to know the reason it failed, it's better to check `Instruction:is_invalid()`.
---@return integer #`DecoderError` enum value
function Decoder:last_error()
	---@diagnostic disable-next-line: undefined-field
	return iced_dec.decoder_last_error(self._dec)
end

---Decodes and returns the next instruction.
---
---See also `Decoder:decode_out()` which avoids copying the decoded instruction to the caller's return variable.
---See also `Decoder:last_error()`.
---
---@return Instruction #The next instruction
---
---# Examples
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local Code = require("iced-x86.Code")
---local Mnemonic = require("iced-x86.Mnemonic")
---local OpKind = require("iced-x86.OpKind")
---local Register = require("iced-x86.Register")
---local MemorySize = require("iced-x86.MemorySize")
---
----- xrelease lock add [rax],ebx
---local data = "\xF0\xF3\x01\x18"
---local decoder = Decoder:new(64, data)
---local instr = decoder.decode()
---
---assert(instr.code() == Code.Add_rm32_r32)
---assert(instr.mnemonic() == Mnemonic.Add)
---assert(instr.len() == 4)
---assert(instr.op_count() == 2)
---
---assert(instr.op0_kind() == OpKind.Memory)
---assert(instr.memory_base() == Register.RAX)
---assert(instr.memory_index() == Register.None)
---assert(instr.memory_index_scale() == 1)
---assert(instr.memory_displacement() == 0)
---assert(instr.memory_segment() == Register.DS)
---assert(instr.segment_prefix() == Register.None)
---assert(instr.memory_size() == MemorySize.UInt32)
---
---assert(instr.op1_kind() == OpKind.Register)
---assert(instr.op1_register() == Register.EBX)
---
---assert(instr.has_lock_prefix())
---assert(instr.has_xrelease_prefix())
---```
function Decoder:decode()
	---@diagnostic disable-next-line: undefined-field
	local instr = iced_dec.decoder_decode(self._dec)
	return Instruction:_from_raw(instr)
end

---Decodes the next instruction
---
---The difference between this method and `Decoder:decode` is that this method doesn't need to
---allocate a new instruction since it overwrites the input instruction.
---
---@param instr Instruction #Updated with the decoded instruction
---
---# Examples
---
---```lua
---local Decoder = require("iced-x86.Decoder")
---local Instruction = require("iced-x86.Instruction")
---local Code = require("iced-x86.Code")
---local Mnemonic = require("iced-x86.Mnemonic")
---local OpKind = require("iced-x86.OpKind")
---local Register = require("iced-x86.Register")
---local MemorySize = require("iced-x86.MemorySize")
---
----- xrelease lock add [rax],ebx
---local data = "\xF0\xF3\x01\x18"
---local decoder = Decoder(64, data)
---local instr = Instruction:new()
---decoder.decode_out(instr)
---
---assert(instr.code() == Code.Add_rm32_r32)
---assert(instr.mnemonic() == Mnemonic.Add)
---assert(instr.len() == 4)
---assert(instr.op_count() == 2)
---
---assert(instr.op0_kind() == OpKind.Memory)
---assert(instr.memory_base() == Register.RAX)
---assert(instr.memory_index() == Register.None)
---assert(instr.memory_index_scale() == 1)
---assert(instr.memory_displacement() == 0)
---assert(instr.memory_segment() == Register.DS)
---assert(instr.segment_prefix() == Register.None)
---assert(instr.memory_size() == MemorySize.UInt32)
---
---assert(instr.op1_kind() == OpKind.Register)
---assert(instr.op1_register() == Register.EBX)
---
---assert(instr.has_lock_prefix())
---assert(instr.has_xrelease_prefix())
---```
function Decoder:decode_out(instr)
	---@diagnostic disable-next-line: undefined-field
	iced_dec.decoder_decode_out(self._dec, instr._instr)
end

--TODO: get_constant_offsets()
--TODO: iterator

return Decoder
